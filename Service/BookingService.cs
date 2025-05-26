using Models.Dtos;
using Models;
using Interfaces;
using Infrastructure;
namespace Service;

public class BookingService:IBookingService
{
  private IAdminDbRepo _adminDbRepo;
  private IBookingDbRepo  _BookingDbRepo;
  private IJourneyDbRepo _journeyDbRepo;

  private IApartmentDbRepo _apartmentDbRepo;
  public BookingService(IAdminDbRepo adminDbRepo,IBookingDbRepo BookingDbRepo,IJourneyDbRepo journeyDbRepo,IApartmentDbRepo apartmentDbRepo)
  {
    _adminDbRepo=adminDbRepo;
    _BookingDbRepo=BookingDbRepo;
    _journeyDbRepo=journeyDbRepo;
    _apartmentDbRepo=apartmentDbRepo;
  }

  public async Task<ResponseModel> SelectAllSeatsForJourney(int journeyId)
  {
    return new ModdelMapper().ResponseToFormalResponse<IEnumerable<SeatModel>>(await _BookingDbRepo.SelectAllSeatsForJourney(journeyId));
  }

  public async Task<ResponseModel> SelectBookedSeatsForJourney(int fromjourneyId,int tojourneyId,int apartmentId)
  {
    return new ModdelMapper().ResponseToFormalResponse<IEnumerable<SeatModel>>(await _BookingDbRepo.SelectBookedSeatsForApartment(fromjourneyId,tojourneyId,apartmentId));
  }

  public async Task<ResponseModel> SelectAllJourneysForSchedule(string scheduleId)
  {
    return new ModdelMapper().ResponseToFormalResponse<IEnumerable<ReturnJourneyStationDto>>(await _BookingDbRepo.SelectAllJourneysForSchedule(scheduleId));
  }

  public async Task<ResponseModel> BookForSchedule(AddBookingDto addBookingDto)
  {
    //First check the authentication 
    ResponseModelTyped<AuthenticateTokenModel> res=await _adminDbRepo.AuthenticateUser(addBookingDto.tokenId);

    if(res.Success ==true && res.Data.is_token_valid)
    {
      if(res.Data.is_user_admin==false && res.Data.is_user_active)
      {
        //check all journeys align with schedule Id
        ResponseModelTyped<IEnumerable<JourneyTrainModel>> journeyTrainModel=await _journeyDbRepo.selectScheduleDetails(addBookingDto.scheduleId);

        if(journeyTrainModel.Success==false)
        {
          return new ModdelMapper().ResponseToFormalResponse<IEnumerable<JourneyTrainModel>>(journeyTrainModel);
        }

        if(journeyTrainModel.Success)
        {
          if(journeyTrainModel.Data.Count() != 0)
          {
            //contains 
            JourneyTrainModel[] journeyTrainModelArray=journeyTrainModel.Data.ToArray();

            bool journeyValidation=await CheckBookingJourney(addBookingDto,journeyTrainModelArray);

            if(journeyValidation)
            {
              ResponseModelTyped<IEnumerable<ReturnApartmentDto>> apartrmentsForTrain=
              await _apartmentDbRepo.selectAllApartmentsForTrain(journeyTrainModelArray[0].trainNo,journeyTrainModelArray[0].trainSeqNo);

              if(apartrmentsForTrain.Success==false)
              {
                return new ModdelMapper().ResponseToFormalResponse<IEnumerable<ReturnApartmentDto>>(apartrmentsForTrain);
              }

              if(apartrmentsForTrain.Data.Count() != 0)
              {
                ReturnApartmentDto[] apartrmentsForTrainArray=apartrmentsForTrain.Data.ToArray();

                bool apartmentValidation=await CheckBookingApartments(addBookingDto,apartrmentsForTrainArray);
                
                if(apartmentValidation)
                {
                   //Get booked seats for a train
                   ResponseModelTyped<IEnumerable<SeatModel>> bookedseats=
                    await _BookingDbRepo.SelectBookedSeatsForTrain(
                      addBookingDto.fromJourneyId,addBookingDto.ToJourneyId,journeyTrainModelArray[0].trainNo,journeyTrainModelArray[0].trainSeqNo);

                    if(bookedseats.Success==false)
                    {
                      return new ModdelMapper().ResponseToFormalResponse<IEnumerable<SeatModel>>(bookedseats);
                    }
                    
                    SeatModel[] bookedSeatArray=bookedseats.Data.ToArray();
                    
                    bool seatValidation=await CheckBookedSeats(addBookingDto,bookedSeatArray);

                    if(seatValidation)
                    {
                      //book the seats
                      return new ModdelMapper().ResponseToFormalResponse<string>
                      (
                          await _BookingDbRepo.BookForSchedule
                          (
                            addBookingDto,res.Data.username,
                            await getNetPriceB(await getPriceBookedSeats(addBookingDto) ),
                            await getPriceBookedSeats(addBookingDto)
                          )
                      );
                    }
                    else
                    {
                      return new ResponseModel
                      {
                          Success=false,
                          ErrCode=403,
                          Data="One or more seats have already booked!"
                      };
                    }

                }
                else
                {
                  return new ResponseModel
                  {
                      Success=false,
                      ErrCode=400,
                      Data="Apartment ids are invalid!"
                  };
                }

              }
              else
              {
                return new ResponseModel
                {
                    Success=false,
                    ErrCode=400,
                    Data="No apartments for train!"
                };
              }
            }
            else
            {
                return new ResponseModel
                {
                    Success=false,
                    ErrCode=400,
                    Data="Journey ids are invalid!"
                };
            }

          }
          else
          {
            return new ResponseModel
            {
                Success=false,
                ErrCode=400,
                Data="Schedule id is invalid!"
            };
          }
        }
        else
        {
          return new ModdelMapper().ResponseToFormalResponse<IEnumerable<JourneyTrainModel>>(journeyTrainModel);
        }
        //check all apartments are align with train
      }
      else
      {
        return new ResponseModel
        {
            Success=false,
            ErrCode=403,
            Data=res.Data
        };
      }
    }
    else if(res.Success)
    {
        return new ResponseModel
        {
            Success=false,
            ErrCode=401,
            Data=res.Data
        };
    }  
    else
    {
        return new ModdelMapper().ResponseToFormalResponse<AuthenticateTokenModel>(res);
    }
  }

  //check schedule ids against journey ids
  private static async Task<bool> CheckBookingJourney(AddBookingDto addBookingDto,JourneyTrainModel[] journeyTrainModel)
  {
    bool fromJourneyId=false;
    bool ToJourneyId=false;

    ////////////////////
    foreach(var i in journeyTrainModel)
    {
      if(i.JourneyId==addBookingDto.fromJourneyId)
        fromJourneyId=true;
      else if(i.JourneyId==addBookingDto.ToJourneyId)
        ToJourneyId=true;

      if(fromJourneyId && ToJourneyId)
      {
        return fromJourneyId && ToJourneyId;
      }
    }

    return fromJourneyId && ToJourneyId;
  }

  private static async Task<bool> CheckBookingApartments(AddBookingDto addBookingDto,ReturnApartmentDto[] returnApartmentDto)
  {
    foreach(var i in addBookingDto.seatModel)
    {
      bool found=false;
      foreach(var j in returnApartmentDto)
      {
        if(i.apartmentId==j.Apartment_id)
        {
          found=true;
          break;
        }
      }

      if(!found)
      {
        return false;
      }
    }

    return true;
  }

  private static async Task<bool> CheckBookedSeats(AddBookingDto addBookingDto,SeatModel[] seatModels)
  {
    foreach(var i in seatModels)
    {
      foreach(var j in addBookingDto.seatModel)
      {
        if(i.isLeft==j.isLeft && i.rowNo==j.rowNo && i.seqNo==j.seqNo && i.apartmentId==j.apartmentId)
        {
          return false;
          break;
        }
      }
    }

    return true;
  }
  
  //calculate price for booked seats
  private static async Task<float[]> getPriceBookedSeats(AddBookingDto addBookingDto)
  {
    float[] prices=new float[addBookingDto.seatModel.Length];

    //dummy code
    for(int i=0;i<prices.Length;i++)
    {
      prices[i]=50.00f;
    }

    return prices;
  }

  //calculate Net Price for booked seats
  private static async Task<float> getNetPriceB(float[] prices)
  {
    //dummy code
    float total=0;
    
    for(int i=0;i<prices.Length;i++)
    {
      total+=prices[i];
    }

    return total;
  }
}
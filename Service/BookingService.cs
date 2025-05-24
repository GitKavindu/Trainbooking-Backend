using Models.Dtos;
using Models;
using Interfaces;
namespace Service;

public class BookingService:IBookingService
{
  private IAdminDbRepo _adminDbRepo;
  private IBookingDbRepo  _BookingDbRepo;
  public BookingService(IAdminDbRepo adminDbRepo,IBookingDbRepo BookingDbRepo)
  {
    _adminDbRepo=adminDbRepo;
    _BookingDbRepo=BookingDbRepo;
  }

  public async Task<ResponseModel> SelectAllSeatsForJourney(int journeyId)
  {
    return new ModdelMapper().ResponseToFormalResponse<IEnumerable<SeatModel>>(await _BookingDbRepo.SelectAllSeatsForJourney(journeyId));
  }

  public async Task<ResponseModel> SelectBookedSeatsForJourney(int journeyId,int apartmentId)
  {
    return new ModdelMapper().ResponseToFormalResponse<IEnumerable<SeatModel>>(await _BookingDbRepo.SelectBookedSeatsForJourney(journeyId,apartmentId));
  }

  public async Task<ResponseModel> SelectAllJourneysForSchedule(string scheduleId)
  {
    return new ModdelMapper().ResponseToFormalResponse<IEnumerable<ReturnJourneyStationDto>>(await _BookingDbRepo.SelectAllJourneysForSchedule(scheduleId));
  }

  // public async Task<ResponseModel> BookForSchedule(AddBookingDto addBookingDto)
  // {
  //   //First check the authentication 
  //   ResponseModelTyped<AuthenticateTokenModel> res=await _adminDbRepo.AuthenticateUser(addBookingDto.tokenId);

  //   if(res.Success ==true && res.Data.is_token_valid)
  //   {
  //     if(res.Data.is_user_admin==false && res.Data.is_user_active)
  //     {
  //      //check all journeys align with schedule Id
       
  //      //check all apartments are align with train
  //     }
  //     else
  //     {
  //       return new ResponseModel
  //       {
  //           Success=false,
  //           ErrCode=403,
  //           Data=res.Data
  //       };
  //     }
  //   }
  //   else if(res.Success)
  //   {
  //       return new ResponseModel
  //       {
  //           Success=false,
  //           ErrCode=401,
  //           Data=res.Data
  //       };
  //   }  
  //   else
  //   {
  //       return new ModdelMapper().ResponseToFormalResponse<AuthenticateTokenModel>(res);
  //   }
  // }
}


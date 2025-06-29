using Interfaces;
using Models;
using Models.Dtos;
using Dapper;
using Npgsql;
using System.Data;
using NpgsqlTypes;

namespace Infrastructure;

public class BookingDbRepo:IBookingDbRepo
{
  private IDbConnectRepo _dbConnectRepo;
    
  public BookingDbRepo(IDbConnectRepo dbConnectRepo)
  {
    _dbConnectRepo=dbConnectRepo;
    
  }

  public async Task<ResponseModelTyped<IEnumerable<SeatModel>>> SelectAllSeatsForJourney(int journeyId) 
  {
    using (var con = new NpgsqlConnection(_dbConnectRepo.GetDatabaseConnection()))
    {
        con.Open();
        try
        {
          DynamicParameters para=new DynamicParameters();
           para.Add("journey_id",journeyId);
          
          // Call the function with the parameters and retrieve the results
          IEnumerable<SeatModel> allSeats=await con.QueryAsync<SeatModel>(
            @$"SELECT j.journey_id,j.train_no,j.train_seq_no,j.schedule_id,s.seat_id,s.is_left AS isLeft,s.row_no AS rowNo,s.seq_no AS seqNo,s.apartment_id AS apartmentId
                FROM journey j 
                INNER JOIN apartments a ON j.train_no=a.train_id AND j.train_seq_no=a.train_seq_no
                INNER JOIN seat s ON s.apartment_id = a.apartment_id
                WHERE j.journey_id=@journey_id
                ORDER BY s.row_no,s.is_left,s.seq_no"
            ,para, commandType: CommandType.Text);

          
          return new ResponseModelTyped<IEnumerable<SeatModel>>()
          {
              Success = true,
              ErrCode = 200,
              Data = allSeats
          };

        }
        catch (NpgsqlException ex)
        {
            Console.WriteLine(ex);
            return new ResponseModelTyped<IEnumerable<SeatModel>>()
            {
                Success = false,
                ErrCode = 500
            };
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex);  
          return new ResponseModelTyped<IEnumerable<SeatModel>>()
            {
                Success = false,
                ErrCode = 500
            };
        }
    }
  } 

  public async Task<ResponseModelTyped<IEnumerable<SeatModel>>> SelectBookedSeatsForApartment(int fromJourneyId,int ToJourneyId,int apartmentId) 
  {
    using (var con = new NpgsqlConnection(_dbConnectRepo.GetDatabaseConnection()))
    {
        con.Open();
        try
        {
          DynamicParameters para=new DynamicParameters();
          para.Add("from_journey_id",fromJourneyId);
          para.Add("to_journey_id",ToJourneyId);
          para.Add("apartment_id",apartmentId);
          
          // Call the function with the parameters and retrieve the results
          IEnumerable<SeatModel> allSeats=await con.QueryAsync<SeatModel>(
            @$"SELECT is_left AS isLeft,row_no AS rowNo,seq_no AS seqNo,s.apartment_id AS apartmentId 
                FROM booking_journey b
                INNER JOIN seat s ON b.seat_id=s.seat_id
                WHERE b.journey_id>=@from_journey_id AND b.journey_id<=@to_journey_id AND s.apartment_id=@apartment_id"
            ,para, commandType: CommandType.Text);

          
          return new ResponseModelTyped<IEnumerable<SeatModel>>()
          {
              Success = true,
              ErrCode = 200,
              Data = allSeats
          };

        }
        catch (NpgsqlException ex)
        {
            Console.WriteLine(ex);
            return new ResponseModelTyped<IEnumerable<SeatModel>>()
            {
                Success = false,
                ErrCode = 500
            };
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex);  
          return new ResponseModelTyped<IEnumerable<SeatModel>>()
            {
                Success = false,
                ErrCode = 500
            };
        }
    }
  } 
   public async Task<ResponseModelTyped<IEnumerable<SeatModel>>> SelectBookedSeatsForTrain(int fromJourneyId,int ToJourneyId,int trainId,int trainSeqNo)
   {
    using (var con = new NpgsqlConnection(_dbConnectRepo.GetDatabaseConnection()))
    {
        con.Open();
        try
        {
          DynamicParameters para=new DynamicParameters();
          para.Add("from_journey_id",fromJourneyId);
          para.Add("to_journey_id",ToJourneyId);
          para.Add("train_id",trainId);
          para.Add("train_seq_no",trainSeqNo);
          
          // Call the function with the parameters and retrieve the results
          IEnumerable<SeatModel> allSeats=await con.QueryAsync<SeatModel>(
            @$"SELECT is_left AS isLeft,row_no AS rowNo,seq_no AS seqNo,s.apartment_id AS apartmentId 
                FROM booking_journey bj
				        INNER JOIN booking b ON bj.booking_id=b.booking_id AND b.is_canceled=false
                INNER JOIN seat s ON bj.seat_id=s.seat_id
                INNER JOIN apartments a ON s.apartment_id=a.apartment_id
                WHERE bj.journey_id>=@from_journey_id AND bj.journey_id<=@to_journey_id AND a.train_id=@train_id AND a.train_seq_no=@train_seq_no"
            ,para, commandType: CommandType.Text);

          
          return new ResponseModelTyped<IEnumerable<SeatModel>>()
          {
              Success = true,
              ErrCode = 200,
              Data = allSeats
          };

        }
        catch (NpgsqlException ex)
        {
            Console.WriteLine(ex);
            return new ResponseModelTyped<IEnumerable<SeatModel>>()
            {
                Success = false,
                ErrCode = 500
            };
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex);  
          return new ResponseModelTyped<IEnumerable<SeatModel>>()
            {
                Success = false,
                ErrCode = 500
            };
        }
    }
  }
  public async Task<ResponseModelTyped<IEnumerable<ReturnJourneyStationDto>>> SelectAllJourneysForSchedule(string scheduleId) 
  {
    using (var con = new NpgsqlConnection(_dbConnectRepo.GetDatabaseConnection()))
    {
        con.Open();
        try
        {
          DynamicParameters para=new DynamicParameters();
          para.Add("scheduleId",scheduleId);
          
          // Call the function with the parameters and retrieve the results
          IEnumerable<ReturnJourneyStationDto> allSeats=await con.QueryAsync<ReturnJourneyStationDto>(
            @$"SELECT j.scheduled_start_time AS startTime,t.scheduled_start_time AS endTime,j.journey_id AS startJourneyId,t.journey_id AS endJourneyId,
                        s.station_name AS StartStation,n.station_name AS EndStation
                FROM journey j
                INNER join journey t on j.journey_id + 1 = t.journey_id AND j.schedule_id=t.schedule_id
                inner join station s on j.seq_no=s.seq_no AND j.station_no=s.station_id
                INNER join station n on t.seq_no=n.seq_no AND t.station_no=n.station_id
                WHERE j.is_active=true AND j.schedule_id=@scheduleId
                ORDER BY j.journey_id"
            ,para, commandType: CommandType.Text);

          
          return new ResponseModelTyped<IEnumerable<ReturnJourneyStationDto>>()
          {
              Success = true,
              ErrCode = 200,
              Data = allSeats
          };

        }
        catch (NpgsqlException ex)
        {
            Console.WriteLine(ex);
            return new ResponseModelTyped<IEnumerable<ReturnJourneyStationDto>>()
            {
                Success = false,
                ErrCode = 500
            };
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex);  
          return new ResponseModelTyped<IEnumerable<ReturnJourneyStationDto>>()
            {
                Success = false,
                ErrCode = 500
            };
        }
    }
  }

  public async Task<ResponseModelTyped<string>> BookForSchedule
  (
    AddBookingDto addBookingDto,string bookedUser,float netPrice,float[] prices
  )
  {
    using(var con=new NpgsqlConnection(_dbConnectRepo.GetDatabaseConnection()))
    {
        con.Open();
        // Start a transaction
        using (var transaction = con.BeginTransaction())
        {
            try
            {
                DynamicParameters para=new DynamicParameters();

                para.Add("schedule_id",addBookingDto.scheduleId);
                para.Add("booked_by",bookedUser);
                para.Add("is_canceled",false);
                para.Add("netprice",netPrice);
  
                int booking_id=await con.ExecuteScalarAsync<int>
                (   @"INSERT INTO booking(schedule_id,booked_by,is_canceled,netprice) 
                      VALUES (@schedule_id,@booked_by,@is_canceled,@netprice)
                      RETURNING booking_id",
                    para, commandType: CommandType.Text
                );             

                await insertBookings(booking_id,addBookingDto.scheduleId,addBookingDto.fromJourneyId,addBookingDto.seatModel,prices,con);
                
                transaction.Commit();
                
                return new ResponseModelTyped<string>()
                {
                    Success=true,
                    ErrCode=200,
                    Data="Operation successful"
                };
            }
            catch(NpgsqlException ex)
            {
                transaction.Rollback();
                Console.WriteLine(ex);
                return new ResponseModelTyped<string>()
                {
                    Success=false,
                    ErrCode=500,
                    Data="Database error!"
                };
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                transaction.Rollback();
                return new ResponseModelTyped<string>()
                {
                    Success=false,
                    ErrCode=500,
                    Data="Something went wrong!"
                };
            }
        }

    }
  }

  private async Task<ResponseModelTyped<int>> insertBookings(
    int bookingId,string scheduleId,int fromJourneyId,SeatModel[] seatModel,float[] prices,NpgsqlConnection con)
  {
     
    DynamicParameters para=new DynamicParameters();
    string sql=$"INSERT INTO booking_journey(booking_id,journey_id,price,seat_id,schedule_id) VALUES ";

    for(int i=0;i<seatModel.Length;i++)
    {
        sql=sql+@$"(@booking_id{i},@journey_id{i},@price{i},
                    (select seat_id from seat where is_left=@is_left{i} AND row_no=@row_no{i} AND seq_no=@seq_no{i} AND apartment_id=@apartment_id{i}),
                    @schedule_id{i})";

        if(i==seatModel.Length-1)
        {
            sql=sql+";";
        }
        else
        {
            sql=sql+",";
        }

        para.Add($"booking_id{i}",bookingId); 
        para.Add($"journey_id{i}",fromJourneyId);
        para.Add($"price{i}",prices[i]);
        para.Add($"is_left{i}",seatModel[i].isLeft);
        para.Add($"row_no{i}",seatModel[i].rowNo);
        para.Add($"seq_no{i}",seatModel[i].seqNo);
        para.Add($"apartment_id{i}",seatModel[i].apartmentId);
        para.Add($"schedule_id{i}",scheduleId);

        //increment from journeyId by 1
        fromJourneyId++;
        
    }
    Console.WriteLine("sql "+sql);
    int results=await con.ExecuteAsync(sql,para, commandType: CommandType.Text);

    return new ResponseModelTyped<int>()
    {
        Success=true,
        ErrCode=200,
        Data=results
    };    
  }
  
  public async Task<ResponseModelTyped<ReturnBookingDetailsDto>> GetBookingDetails(int bookingId) 
  {
    using (var con = new NpgsqlConnection(_dbConnectRepo.GetDatabaseConnection()))
    {
        con.Open();
        try
        {
          DynamicParameters para=new DynamicParameters();
          para.Add("booking_id",bookingId);
          
          // Call the function with the parameters and retrieve the results
          ReturnBookingDetailsDto bookingDetails=await con.QueryFirstAsync<ReturnBookingDetailsDto>(
            @$"SELECT booking_id AS bookingId, booked_by AS bookedBy,netPrice AS price,is_canceled AS isCanceled 
                FROM booking b
                INNER JOIN journey j ON b.schedule_id=j.schedule_id AND j.is_active=true
                WHERE booking_id=@booking_id
                GROUP BY booking_id,booked_by,netPrice,is_canceled,j.schedule_id"
            ,para, commandType: CommandType.Text);

          IEnumerable<SeatModel> seatsBooked=await con.QueryAsync<SeatModel>(
            @$"SELECT is_left AS isLeft,row_no AS rowNo,seq_no AS seqNo,s.apartment_id AS apartmentId 
                FROM booking_journey bj
				        INNER JOIN booking b ON bj.booking_id=b.booking_id AND b.is_canceled=false
                INNER JOIN seat s ON bj.seat_id=s.seat_id
                INNER JOIN apartments a ON s.apartment_id=a.apartment_id
                WHERE b.booking_id=@booking_id"
            ,para, commandType: CommandType.Text);

          bookingDetails.bookedSeats=seatsBooked.ToArray();
          
          return new ResponseModelTyped<ReturnBookingDetailsDto>()
          {
              Success = true,
              ErrCode = 200,
              Data = bookingDetails
          };

        }
        catch (NpgsqlException ex)
        {
            Console.WriteLine(ex);
            return new ResponseModelTyped<ReturnBookingDetailsDto>()
            {
                Success = false,
                ErrCode = 500
            };
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex);  
          return new ResponseModelTyped<ReturnBookingDetailsDto>()
            {
                Success = false,
                ErrCode = 500
            };
        }
    }
  }

  public async Task<ResponseModelTyped<string>> CancelBooking(int bookingId,float refundPrice) 
  {
    using (var con = new NpgsqlConnection(_dbConnectRepo.GetDatabaseConnection()))
    {
        con.Open();
        try
        {
          DynamicParameters para=new DynamicParameters();
          para.Add("booking_id",bookingId);
          para.Add("refund_price",refundPrice);
          
          // Call the function with the parameters and retrieve the results
          await con.ExecuteAsync(
            @$"UPDATE booking SET is_canceled=true,refund_price=@refund_price WHERE booking_id=@booking_id"
            ,para, commandType: CommandType.Text);
          
          return new ResponseModelTyped<string>()
          {
              Success = true,
              ErrCode = 200,
              Data = "Booking canceled sucessfully!!"
          };

        }
        catch (NpgsqlException ex)
        {
            Console.WriteLine(ex);
            return new ResponseModelTyped<string>()
            {
                Success = false,
                ErrCode = 500
            };
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex);  
          return new ResponseModelTyped<string>()
            {
                Success = false,
                ErrCode = 500
            };
        }
    }
  }  
  
}

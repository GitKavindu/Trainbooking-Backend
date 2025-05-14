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
            @$"SELECT j.journey_id,j.train_no,j.train_seq_no,j.schedule_id,s.seat_id,s.is_left AS isLeft,s.row_no AS rowNo,s.seq_no AS seqNo
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

  public async Task<ResponseModelTyped<IEnumerable<SeatModel>>> SelectBookedSeatsForJourney(int journeyId,int apartmentId) 
  {
    using (var con = new NpgsqlConnection(_dbConnectRepo.GetDatabaseConnection()))
    {
        con.Open();
        try
        {
          DynamicParameters para=new DynamicParameters();
          para.Add("journey_id",journeyId);
          para.Add("apartment_id",apartmentId);
          
          // Call the function with the parameters and retrieve the results
          IEnumerable<SeatModel> allSeats=await con.QueryAsync<SeatModel>(
            @$"SELECT * FROM booking_journey b
                INNER JOIN seat s ON b.seat_id=s.seat_id
                WHERE b.journey_id=@journey_id AND s.apartment_id=@apartment_id"
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
            @$"SELECT j.scheduled_start_time AS startTime,t.scheduled_start_time AS endTime,s.station_name AS StartStation,n.station_name AS EndStation
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

  
}

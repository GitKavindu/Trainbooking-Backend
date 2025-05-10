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
                WHERE j.is_active=false AND j.journey_id=@journey_id
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

  
}

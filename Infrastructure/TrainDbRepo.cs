using Interfaces;
using Models;
using Npgsql;
using System.Data;
using Dapper;
using Models.Dtos;
namespace Infrastructure;


public class TrainDbRepo:ITrainDbRepo
{
  private IDbConnectRepo _dbConnectRepo;
    
  public TrainDbRepo(IDbConnectRepo dbConnectRepo)
  {
    _dbConnectRepo=dbConnectRepo;
    
  }

  public async Task<ResponseModelTyped<string>> UpsertTrain(bool isUpdate,int trainId,string trainName,bool isActive,string username)
  {
    using (var con = new NpgsqlConnection(_dbConnectRepo.GetDatabaseConnection()))
    {
        con.Open();
        try
        {
          DynamicParameters para = new DynamicParameters();
          para.Add("_is_update",isUpdate); 
          para.Add("_train_id",trainId); 
          para.Add("_train_name",trainName); 
          para.Add("_is_active",isActive);
          para.Add("_added_by",username); 

          // Call the function with the parameters and retrieve the results
          await con.ExecuteAsync($"CALL public.upsert_train(@_is_update,@_train_id,@_train_name,@_is_active,@_added_by)",para, commandType: CommandType.Text);

          // Return the result
          return new ResponseModelTyped<string>()
          {
              Success = true,
              ErrCode = 200,
              Data = trainId.ToString()
          };

        }
        catch (NpgsqlException ex)
        {
            Console.WriteLine(ex);
            if (ex.SqlState == "45000") // Station not exist
            {
                return new ResponseModelTyped<string>()
                {
                    Success = false,
                    ErrCode = 400,
                    Data = ex.Message
                };
            }

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
  
  public async Task<ResponseModelTyped<IEnumerable<ReturnTrainnDto>>> selectAllTrains()
  {
    using (var con = new NpgsqlConnection(_dbConnectRepo.GetDatabaseConnection()))
    {
        con.Open();
        try
        {
          // Call the function with the parameters and retrieve the results
          IEnumerable<ReturnTrainnDto> results=await con.QueryAsync<ReturnTrainnDto>(
            $"SELECT train_no,name AS train_name,added_by,TO_CHAR(added_date, 'YYYY-MM-DD') AS created_date,TO_CHAR(modified_date, 'YYYY-MM-DD') AS lastUpdated_date FROM train WHERE is_active=true"
            , commandType: CommandType.Text);

          // Return the result
          return new ResponseModelTyped<IEnumerable<ReturnTrainnDto>>()
          {
              Success = true,
              ErrCode = 200,
              Data = results
          };

        }
        catch (NpgsqlException ex)
        {
            return new ResponseModelTyped<IEnumerable<ReturnTrainnDto>>()
            {
                Success = false,
                ErrCode = 500
            };
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex);  
          return new ResponseModelTyped<IEnumerable<ReturnTrainnDto>>()
            {
                Success = false,
                ErrCode = 500
            };
        }
    }
  }
}

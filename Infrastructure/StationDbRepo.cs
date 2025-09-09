using Interfaces;
using Models;
using Npgsql;
using System.Data;
using Dapper;
using Models.Dtos;
namespace Infrastructure;


public class StationDbRepo:IStationDbRepo
{
  private IDbConnectRepo _dbConnectRepo;
    
  public StationDbRepo(IDbConnectRepo dbConnectRepo)
  {
    _dbConnectRepo=dbConnectRepo;
    
  }

  public async Task<ResponseModelTyped<string>> UpsertStation(bool isUpdate,int stationId,string stationName,bool isActive,string username)
  {
    using (var con = new NpgsqlConnection(_dbConnectRepo.GetDatabaseConnection()))
    {
        con.Open();
        try
        {
          DynamicParameters para = new DynamicParameters();
          para.Add("_is_update",isUpdate); 
          para.Add("_station_id",stationId); 
          para.Add("_station_name",stationName); 
          para.Add("_is_active",isActive);
          para.Add("_added_by",username); 

          // Call the function with the parameters and retrieve the results
          await con.ExecuteAsync($"CALL public.upsert_station(@_is_update,@_station_id,@_station_name,@_is_active,@_added_by)",para, commandType: CommandType.Text);

          // Return the result
          return new ResponseModelTyped<string>()
          {
              Success = true,
              ErrCode = 200,
              Data = stationId.ToString()
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
                    Data =  new CommonService().CleanMessage(ex)
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
  
  public async Task<ResponseModelTyped<IEnumerable<ReturnStationDto>>> selectAllStations()
  {
    using (var con = new NpgsqlConnection(_dbConnectRepo.GetDatabaseConnection()))
    {
        con.Open();
        try
        {
          // Call the function with the parameters and retrieve the results
          IEnumerable<ReturnStationDto> results=await con.QueryAsync<ReturnStationDto>(
            $"SELECT station_id,seq_no AS stationSeqNo,station_name,added_by,TO_CHAR(added_date, 'YYYY-MM-DD') AS created_date,TO_CHAR(modified_date, 'YYYY-MM-DD') AS lastUpdated_date FROM station WHERE is_active=true"
            , commandType: CommandType.Text);

          // Return the result
          return new ResponseModelTyped<IEnumerable<ReturnStationDto>>()
          {
              Success = true,
              ErrCode = 200,
              Data = results
          };

        }
        catch (NpgsqlException ex)
        {
            return new ResponseModelTyped<IEnumerable<ReturnStationDto>>()
            {
                Success = false,
                ErrCode = 500
            };
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex);  
          return new ResponseModelTyped<IEnumerable<ReturnStationDto>>()
            {
                Success = false,
                ErrCode = 500
            };
        }
    }
  }
}

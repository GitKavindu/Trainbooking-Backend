using Interfaces;
using Models;
using Npgsql;
using System.Data;
using Dapper;
using Models.Dtos;
namespace Infrastructure;


public class JourneyDbRepo:IJourneyDbRepo
{
  private IDbConnectRepo _dbConnectRepo;
    
  public JourneyDbRepo(IDbConnectRepo dbConnectRepo)
  {
    _dbConnectRepo=dbConnectRepo;
    
  }

  public async Task<ResponseModelTyped<IEnumerable<ReturnJourneyDto>>> selectAJourney(string schedule_id)
  {
    using (var con = new NpgsqlConnection(_dbConnectRepo.GetDatabaseConnection()))
    {
        con.Open();
        try
        {
          DynamicParameters para=new DynamicParameters();
          para.Add("schedule_id",schedule_id);

          // Call the function with the parameters and retrieve the results
          IEnumerable<ReturnJourneyDto> results=await con.QueryAsync<ReturnJourneyDto>(
            @$"SELECT t.name AS train,s.station_name AS station,j.scheduled_start_time as scheduledTime 
            FROM journey j 
            INNER JOIN station s ON s.station_id=j.station_no AND s.seq_no = j.seq_no 
            INNER JOIN train t ON t.train_no = j.train_no AND t.seq_no = j.train_seq_no 
            WHERE j.schedule_id=@schedule_id AND j.is_active=true 
            ORDER BY j.journey_id"
            ,para, commandType: CommandType.Text);

          // Return the result
          return new ResponseModelTyped<IEnumerable<ReturnJourneyDto>>()
          {
              Success = true,
              ErrCode = 200,
              Data = results
          };

        }
        catch (NpgsqlException ex)
        {
            Console.WriteLine(ex); 
            return new ResponseModelTyped<IEnumerable<ReturnJourneyDto>>()
            {
                Success = false,
                ErrCode = 500
            };
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex);  
          return new ResponseModelTyped<IEnumerable<ReturnJourneyDto>>()
            {
                Success = false,
                ErrCode = 500
            };
        }
    }
  }

  public async Task<ResponseModelTyped<string>> AddJourney(AddJourneyDto addJourneyDto,string username)
  {
    using (var con = new NpgsqlConnection(_dbConnectRepo.GetDatabaseConnection()))
    {
        con.Open();
        try
        {    
          ResponseModelTyped<int> results=await insertJourney(addJourneyDto.addJourneyStationDto,addJourneyDto.scheduleId,username,addJourneyDto.trainId,addJourneyDto.trainSeqNo,con);

          // Return the result
          return new ResponseModelTyped<string>()
          {
              Success = true,
              ErrCode = 200,
              Data = results.Data.ToString()
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
  
  public async Task<ResponseModelTyped<string>> UpdateJourney(AddJourneyDto addJourneyDto,string username)
  {
    using (var con = new NpgsqlConnection(_dbConnectRepo.GetDatabaseConnection()))
    {
        con.Open();
        using (var transaction = con.BeginTransaction())
        {
          try
          {
            bool isExists=(await ValidateScheduleId(addJourneyDto.scheduleId)).Data;
            if(!isExists)
            {
              return new ResponseModelTyped<string>()
              {
                  Success = false,
                  ErrCode = 404,
                  Data = "Schedule Id not exists!"
              };
            }
            
            DynamicParameters para=new DynamicParameters();
            para.Add("schedule_id",addJourneyDto.scheduleId);
            
            string sql=$"UPDATE journey SET is_active=false WHERE schedule_id=@schedule_id";
            await con.ExecuteAsync(sql,para, commandType: CommandType.Text);

            ResponseModelTyped<int> results=await insertJourney(addJourneyDto.addJourneyStationDto,addJourneyDto.scheduleId,username,addJourneyDto.trainId,addJourneyDto.trainSeqNo,con);

            transaction.Commit();

            // Return the result
            return new ResponseModelTyped<string>()
            {
                Success = true,
                ErrCode = 200,
                Data = results.Data.ToString()
            };
            
          }
          catch (NpgsqlException ex)
          {
              Console.WriteLine(ex);
              transaction.Rollback();
              return new ResponseModelTyped<string>()
              {
                  Success = false,
                  ErrCode = 500
              };
          }
          catch (Exception ex)
          {
            Console.WriteLine(ex); 
            transaction.Rollback(); 
            return new ResponseModelTyped<string>()
              {
                  Success = false,
                  ErrCode = 500
              };
          }
        }  
    }
  }

  private async Task<ResponseModelTyped<int>> insertJourney(
    AddJourneyStationDto []addJourneyStationDto,string scheduleId,string username,int trainId,int JourneyeqNo,NpgsqlConnection con)
  {
     
    DynamicParameters para=new DynamicParameters();
    string sql=$"INSERT INTO journey(schedule_id,scheduled_by,scheduled_start_time,is_active,train_no,station_no,seq_no,train_seq_no) VALUES ";

    for(int i=0;i<addJourneyStationDto.Length;i++)
    {
        sql=sql+$"(@schedule_id{i},@scheduled_by{i},@scheduled_start_time{i},@is_active{i},@train_no{i},@station_no{i},@seq_no{i},@train_seq_no{i})";

        if(i==addJourneyStationDto.Length-1)
        {
            sql=sql+";";
        }
        else
        {
            sql=sql+",";
        }

        para.Add($"schedule_id{i}",scheduleId); 
        para.Add($"scheduled_by{i}",username);
        para.Add($"scheduled_start_time{i}",addJourneyStationDto[i].ScheduledStartTime);
        para.Add($"is_active{i}",true);
        para.Add($"train_no{i}",trainId);
        para.Add($"station_no{i}",addJourneyStationDto[i].StationId);
        para.Add($"seq_no{i}",addJourneyStationDto[i].StationSeqNo);
        para.Add($"train_seq_no{i}",JourneyeqNo);
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

  public async Task<ResponseModelTyped<string>> DeleteJourney(string scheduleId)
  {
    using (var con = new NpgsqlConnection(_dbConnectRepo.GetDatabaseConnection()))
    {
        con.Open();
        
          try
          {
            bool isExists=(await ValidateScheduleId(scheduleId)).Data;
            if(!isExists)
            {
              return new ResponseModelTyped<string>()
              {
                  Success = false,
                  ErrCode = 404,
                  Data = "Schedule Id not exists!"
              };
            }
            
            DynamicParameters para=new DynamicParameters();
            para.Add("schedule_id",scheduleId);
            
            string sql=$"UPDATE journey SET is_active=false WHERE schedule_id=@schedule_id AND is_active=true";
            int results=await con.ExecuteAsync(sql,para, commandType: CommandType.Text);

            // Return the result
            return new ResponseModelTyped<string>()
            {
                Success = true,
                ErrCode = 200,
                Data = results.ToString()
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
  
  private async Task<ResponseModelTyped<bool>> ValidateScheduleId(string schedule_id)
  {
    using (var con = new NpgsqlConnection(_dbConnectRepo.GetDatabaseConnection()))
    {
        
      con.Open();
      DynamicParameters para=new DynamicParameters();
      para.Add("schedule_id",schedule_id);
              
      string sql=$"SELECT EXISTS (SELECT 1 FROM journey WHERE schedule_id=@schedule_id and is_active=true) AS is_Exists;";
      bool isExists=await con.QueryFirstAsync<bool>(sql,para, commandType: CommandType.Text);

      return new ResponseModelTyped<bool>()
      {
          Success = true,
          ErrCode = 200,
          Data = isExists
      };
    }  

  }
   
}

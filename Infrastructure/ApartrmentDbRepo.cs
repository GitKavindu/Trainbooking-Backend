using Interfaces;
using Models;
using Npgsql;
using System.Data;
using Dapper;
using Models.Dtos;
using System.Linq;
namespace Infrastructure;


public class ApartmentDbRepo:IApartmentDbRepo
{
  private IDbConnectRepo _dbConnectRepo;
    
  public ApartmentDbRepo(IDbConnectRepo dbConnectRepo)
  {
    _dbConnectRepo=dbConnectRepo;
    
  }

  public async Task<ResponseModelTyped<int>> selectMaxSeqNoForTrain(int trainId)
  {
    using (var con = new NpgsqlConnection(_dbConnectRepo.GetDatabaseConnection()))
    {
        con.Open();
        try
        {
          
          DynamicParameters para=new DynamicParameters();
          para.Add("train_no",trainId);

          int seqNo=await con.QueryFirstAsync<int>(
              @$"SELECT MAX(seq_no) FROM train WHERE train_no=@train_no AND is_active=true"
              ,para, commandType: CommandType.Text);

          return new ResponseModelTyped<int>()
          {
              Success = true,
              ErrCode = 200,
              Data = seqNo
          };

        }
        catch (NpgsqlException ex)
        {
            return new ResponseModelTyped<int>()
            {
                Success = false,
                ErrCode = 500
            };
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex);  
          return new ResponseModelTyped<int>()
            {
                Success = false,
                ErrCode = 500
            };
        }
    }
  }

  public async Task<ResponseModelTyped<IEnumerable<ReturnApartmentDto>>> selectAllApartmentsForTrain(int trainId,int seqNo)
  {
    using (var con = new NpgsqlConnection(_dbConnectRepo.GetDatabaseConnection()))
    {
        con.Open();
        try
        {
          // Call the function with the parameters and retrieve the results
          IEnumerable<ReturnApartmentDto> apartment;
          DynamicParameters para=new DynamicParameters();
          if(trainId==0 | seqNo==0)
          {
             apartment=await con.QueryAsync<ReturnApartmentDto>(
              @$"SELECT Apartment_id,class AS ApartmrntClass,added_by,
                    TO_CHAR(added_date, 'YYYY-MM-DD') AS created_date,TO_CHAR(modified_date, 'YYYY-MM-DD') AS lastUpdated_date 
                  FROM apartments 
                  WHERE is_active=true and train_id is null AND train_seq_no is null"
              , commandType: CommandType.Text);
          }
          else
          {
            para.Add("train_id",trainId);
            para.Add("train_seq_no",seqNo);

            apartment=await con.QueryAsync<ReturnApartmentDto>(
              @$"SELECT Apartment_id,class AS ApartmrntClass,added_by,
                    TO_CHAR(added_date, 'YYYY-MM-DD') AS created_date,TO_CHAR(modified_date, 'YYYY-MM-DD') AS lastUpdated_date 
                  FROM apartments 
                  WHERE is_active=true and train_id=@train_id AND train_seq_no=@train_seq_no"
              ,para, commandType: CommandType.Text);
          }

          foreach(var i in apartment)
          {
            IEnumerable<SeatModel> seats=await con.QueryAsync<SeatModel>(
            @$"SELECT is_left AS isLeft,row_no AS rowNo,seq_no AS seqNo,apartment_id AS apartmentId 
                FROM seat 
                WHERE apartment_id={i.Apartment_id}
                ORDER BY row_no,is_left DESC,seq_no"
            , commandType: CommandType.Text);

            i.seatModel=seats.ToArray();
          }
          //apartment.
          // Return the result
          return new ResponseModelTyped<IEnumerable<ReturnApartmentDto>>()
          {
              Success = true,
              ErrCode = 200,
              Data = apartment
          };

        }
        catch (NpgsqlException ex)
        {
            return new ResponseModelTyped<IEnumerable<ReturnApartmentDto>>()
            {
                Success = false,
                ErrCode = 500
            };
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex);  
          return new ResponseModelTyped<IEnumerable<ReturnApartmentDto>>()
            {
                Success = false,
                ErrCode = 500
            };
        }
    }
  }
  
  public async Task<ResponseModelTyped<string>> UpsertApartment
  (
    bool isUpdate, int apartmentId, string apartmentClass, int trainId, int trainSeqNo, bool isActive, string username, SeatModel[] seatModel,NpgsqlConnection con
  )
  {
      DynamicParameters para=new DynamicParameters();

      para.Add("_is_update",isUpdate);
      para.Add("_apartment_id",apartmentId);
      para.Add("_class",apartmentClass);
      para.Add("_train_id",trainId);
      para.Add("_train_seq_no",trainSeqNo);
      para.Add("_is_active",isActive);
      para.Add("_added_by",username);
      // OUT parameter
      para.Add("_result", dbType: DbType.Int32, direction: ParameterDirection.Output);

      var results=await con.ExecuteAsync
      (   "CALL public.upsert_apartment(@_is_update,@_apartment_id,@_class,@_train_id,@_train_seq_no,@_is_active,@_added_by,NULL)",
          para, commandType: CommandType.Text
      );

      // Get OUT parameter value for Added_apartmentId
      int Added_apartmentId= para.Get<int>("_result");
      

      if(seatModel is not null)
      {
          results=results+( await insertSeats(seatModel,Added_apartmentId,con) ).Data;
      }
      
      return new ResponseModelTyped<string>()
      {
          Success=true,
          ErrCode=200,
          Data="Operation successful"
      };
  }

  public async Task<ResponseModelTyped<string>> UpsertApartment
  (
    bool isUpdate, int apartmentId, string apartmentClass, int trainId, int trainSeqNo, bool isActive, string username, SeatModel[] seatModel
  )
  {
    using(var con=new NpgsqlConnection(_dbConnectRepo.GetDatabaseConnection()))
    {
        con.Open();
       
        using (var transaction = con.BeginTransaction())
        {
            try
            {
                
                await UpsertApartment(isUpdate,apartmentId,apartmentClass,trainId,trainSeqNo,isActive,username,seatModel,con);
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
                if (ex.SqlState == "45000") // Apartment not exist
                {
                    return new ResponseModelTyped<string>()
                    {
                        Success = false,
                        ErrCode = 400,
                        Data = new CommonService().CleanMessage(ex)
                    };
                }
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

  private async Task<ResponseModelTyped<int>> insertSeats(SeatModel []seatModels,int apartmentId,NpgsqlConnection con)
  {
     
    DynamicParameters para=new DynamicParameters();
    string sql=$"INSERT INTO seat(is_Left,row_no,seq_no,apartment_id) VALUES ";

    for(int i=0;i<seatModels.Length;i++)
    {
        sql=sql+$"(@is_Left{i},@row_no{i},@seq_no{i},@apartment_id{i})";

        if(i==seatModels.Length-1)
        {
            sql=sql+";";
        }
        else
        {
            sql=sql+",";
        }

        para.Add($"is_Left{i}",seatModels[i].isLeft);
        para.Add($"row_no{i}",seatModels[i].rowNo);
        para.Add($"seq_no{i}",seatModels[i].seqNo);
        para.Add($"apartment_id{i}",apartmentId);
    }
    int results=await con.ExecuteAsync(sql,para, commandType: CommandType.Text);

    return new ResponseModelTyped<int>()
    {
        Success=true,
        ErrCode=200,
        Data=results
    };    
  }
}

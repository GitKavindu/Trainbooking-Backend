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
  private IApartmentDbRepo _ApartmentDbRepo;
    
  public TrainDbRepo(IDbConnectRepo dbConnectRepo,IApartmentDbRepo ApartmentDbRepo)
  {
    _dbConnectRepo=dbConnectRepo;
    _ApartmentDbRepo=ApartmentDbRepo;
  }

  public async Task<ResponseModelTyped<string>> UpsertTrain(bool isUpdate,int trainId,string trainName,bool isActive,string username)
  {
      using (var con = new NpgsqlConnection(_dbConnectRepo.GetDatabaseConnection()))
      {
          con.Open();
          using (var transaction = con.BeginTransaction())
          {
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

            if(isUpdate==true && isActive==true)
            {

              ResponseModelTyped<int> train_seq_no=await _ApartmentDbRepo.selectMaxSeqNoForTrain(trainId);

              if(train_seq_no.Success==false)
              {
                return new ResponseModelTyped<string>()
                {
                    Success = false,
                    ErrCode = 400,
                    Data = "Could not grab train seq no"
                };
              }

              int trainSeqNo=train_seq_no.Data;

              ResponseModelTyped<IEnumerable<ReturnApartmentDto>> apartmentModel=await _ApartmentDbRepo.selectAllApartmentsForTrain(trainId,trainSeqNo++);
              
              if(apartmentModel.Success==false)
              {
                return new ResponseModelTyped<string>()
                {
                    Success = false,
                    ErrCode = 400,
                    Data ="Could not grab apartment list"
                };
              }
              
              ReturnApartmentDto[] apartments=apartmentModel.Data.ToArray();
              Console.WriteLine(trainSeqNo+" train no "+trainId);
              Console.WriteLine(apartments.Length);
              foreach(var i in apartments)
              {
                await _ApartmentDbRepo.UpsertApartment(isUpdate,(int)i.Apartment_id,i.ApartmrntClass,trainId,trainSeqNo,isActive,username,i.seatModel,con);
              }
              
            }

            transaction.Commit();
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
              transaction.Rollback();
              if (ex.SqlState == "45000")
              {
                  return new ResponseModelTyped<string>()
                  {
                      Success = false,
                      ErrCode = 400,
                      Data = new CommonService().CleanMessage(ex)
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
  
  public async Task<ResponseModelTyped<IEnumerable<ReturnTrainnDto>>> selectAllTrains()
  {
    using (var con = new NpgsqlConnection(_dbConnectRepo.GetDatabaseConnection()))
    {
        con.Open();
        try
        {
          // Call the function with the parameters and retrieve the results
          IEnumerable<ReturnTrainnDto> results=await con.QueryAsync<ReturnTrainnDto>(
            $"SELECT train_no,seq_no AS train_seq_no,name AS train_name,added_by,TO_CHAR(added_date, 'YYYY-MM-DD') AS created_date,TO_CHAR(modified_date, 'YYYY-MM-DD') AS lastUpdated_date FROM train WHERE is_active=true"
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

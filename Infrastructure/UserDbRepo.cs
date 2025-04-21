using Interfaces;
using Models;
using Models.Dtos;
using Dapper;
using Npgsql;
using System.Data;
using NpgsqlTypes;

namespace Infrastructure;

public class UserDbRepo:IUserDbRepo
{
  private IDbConnectRepo _dbConnectRepo;
    
  public UserDbRepo(IDbConnectRepo dbConnectRepo)
  {
    _dbConnectRepo=dbConnectRepo;
    
  }

  public async Task<ResponseModel> ResgisterUser(UserDbModel userDbModel,string[] userNames) 
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

        para.Add("_username",userDbModel.username);
        para.Add("_mobile_no",userDbModel.mobile_no);
        
        para.Add("_email",userDbModel.email);
        para.Add("_national_id",userDbModel.national_id);
        para.Add("_password",userDbModel.password);
        para.Add("_is_admin",userDbModel.is_admin);
        para.Add("_prefered_name",userDbModel.prefered_name);
        
        var results = await con.ExecuteAsync
          ("CALL public.insert_user(@_username,@_mobile_no,@_email,@_national_id,@_password,@_is_admin,@_prefered_name)",para, commandType: CommandType.Text);
        
        for(int i=0;i<userNames.Length;i++)
        {
          para=new DynamicParameters();
        
          para.Add("_username",userDbModel.username);
          para.Add("_name",userNames[i]);
          para.Add("_seq_no",i+1);
          
          results += await con.ExecuteAsync
          ("CALL public.insert_user_names(@_seq_no,@_username,@_name)",para, commandType: CommandType.Text);

        }
        
        transaction.Commit();

        string messege="Failed";
        if( results==(userNames.Length+1)*-1 )
        {
          messege="Added Sucessfully";
        }

        return new ResponseModel()
        {
          Success=true,
          ErrCode=200,
          Data=messege
        };
    }
    catch(NpgsqlException ex)
    {
      transaction.Rollback();
      Console.WriteLine(ex);
      return new ResponseModel()
      {
        Success=false,
        ErrCode=500,
        Data="Database error!"
      };
    }
    catch(Exception)
    {
       transaction.Rollback();
       return new ResponseModel()
       {
         Success=false,
         ErrCode=500,
         Data="Something went wrong!"
       };
    }
   }
   }
  } 

  public async Task<ResponseModelTyped<bool>> VerifyPassword(UserSubmitModel userSubmitModel)
  {
   using(var con=new NpgsqlConnection(_dbConnectRepo.GetDatabaseConnection()))
   {
    con.Open();
      try
      {
         DynamicParameters para=new DynamicParameters();

        para.Add("user_name",userSubmitModel.UserName);
        para.Add("user_password",userSubmitModel.UserPassword);

        bool results=await con.QueryFirstAsync<bool>("select verify_password(@user_name,@user_password)",para, commandType: CommandType.Text);
        return new ResponseModelTyped<bool>()
        {
          Success=true,
          ErrCode=200,
            Data=results
        };
       
      }
      catch(NpgsqlException ex)
      {
        Console.WriteLine(ex);
        return new ResponseModelTyped<bool>()
        {
          Success=false,
          ErrCode=500,

        };
      }
      catch(Exception)
      {
        return new ResponseModelTyped<bool>()
        {
          Success=false,
          ErrCode=500,

        };

      }
     }

  }


  public async Task<ResponseModelTyped<string>> GetToken(TokenModel tokenModel)
  {
    using (var con = new NpgsqlConnection(_dbConnectRepo.GetDatabaseConnection()))
    {
        con.Open();
        try
        {
                      // Set up the parameters to pass to the function
          DynamicParameters para = new DynamicParameters();
          para.Add("token_id", tokenModel.tokenId); 
          para.Add("start_time", tokenModel.recievedDate); 
          para.Add("end_time", tokenModel.endDate);
          para.Add("username", tokenModel.username);

          // Call the function with the parameters and retrieve the result
          string ?newTokenId = await con.ExecuteScalarAsync<string>(
              "SELECT public.get_usertoken(@token_id, @start_time, @end_time, @username)", 
              para, 
              commandType: CommandType.Text
          );

          // Return the result
          return new ResponseModelTyped<string>()
          {
              Success = true,
              ErrCode = 200,
              Data = newTokenId // Return the new_token_id as the data
          };

        }
        catch (NpgsqlException ex)
        {
            Console.WriteLine(ex);
            return new ResponseModelTyped<string>()
            {
                Success = false,
                ErrCode = 500,
                Data = "Error during procedure call"
            };
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex);  
          return new ResponseModelTyped<string>()
            {
                Success = false,
                ErrCode = 500,
                Data = "Unexpected error"
            };
        }
    }
  }

  public async Task<ResponseModelTyped<string>> DisableToken(disableTokenModel disableTokenModel)
  {
    using (var con = new NpgsqlConnection(_dbConnectRepo.GetDatabaseConnection()))
    {
        con.Open();
        try
        {
          DynamicParameters para = new DynamicParameters();
          para.Add("token_id", disableTokenModel.tokenId); 

          // Call the function with the parameters and retrieve the results
          await con.ExecuteAsync($"update token set is_active=false where token_id=@token_id",para, commandType: CommandType.Text);

          // Return the result
          return new ResponseModelTyped<string>()
          {
              Success = true,
              ErrCode = 200,
              Data =$"Sucessfully disabled token {disableTokenModel.tokenId}" 
          };

        }
        catch (NpgsqlException ex)
        {
            Console.WriteLine(ex);
            return new ResponseModelTyped<string>()
            {
                Success = false,
                ErrCode = 500,
                Data = "Error during procedure call"
            };
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex);  
          return new ResponseModelTyped<string>()
            {
                Success = false,
                ErrCode = 500,
                Data = "Unexpected error"
            };
        }
    }
  }
  public async Task<ResponseModelTyped<UserDbModel>> CheckUserStatus(string username)
  {
    using (var con = new NpgsqlConnection(_dbConnectRepo.GetDatabaseConnection()))
    {
        con.Open();
        try
        {
          DynamicParameters para = new DynamicParameters();
          para.Add("@username",username); 

          // Call the function with the parameters and retrieve the results
          UserDbModel res=
            await con.QueryFirstAsync<UserDbModel>($"SELECT * from users where username=@username;",para, commandType: CommandType.Text);

          // Return the result
          if(res is null)
          {
              return new ResponseModelTyped<UserDbModel>()
              {
                Success = false,
                ErrCode = 404
              }; 
          }
          return new ResponseModelTyped<UserDbModel>()
          {
              Success = true,
              ErrCode = 200,
              Data = res
          };

        }
        catch (NpgsqlException ex)
        {
            Console.WriteLine(ex);
            return new ResponseModelTyped<UserDbModel>()
            {
                Success = false,
                ErrCode = 500
            };
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex);  
          return new ResponseModelTyped<UserDbModel>()
            {
                Success = false,
                ErrCode = 500
            };
        }
    } 
  }

}

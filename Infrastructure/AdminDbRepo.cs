using Interfaces;
using Models;
using Models.Dtos;
using Npgsql;
using System.Data;
using Dapper;
namespace Infrastructure;


public class AdminDbRepo:IAdminDbRepo
{
  private IDbConnectRepo _dbConnectRepo;
    
  public AdminDbRepo(IDbConnectRepo dbConnectRepo)
  {
    _dbConnectRepo=dbConnectRepo;
    
  }

  public async Task<ResponseModelTyped<AuthenticateTokenModel>> AuthenticateUser(string tokenId)
  {
      using (var con = new NpgsqlConnection(_dbConnectRepo.GetDatabaseConnection()))
    {
        con.Open();
        try
        {
          DynamicParameters para = new DynamicParameters();
          para.Add("_token_id",tokenId); 

          // Call the function with the parameters and retrieve the results
          AuthenticateTokenModel res=await con.QueryFirstAsync<AuthenticateTokenModel>($"SELECT * from authenticate_user(@_token_id);",para, commandType: CommandType.Text);

          // Return the result
          if(res.is_token_valid==false)
          {
            return new ResponseModelTyped<AuthenticateTokenModel>()
            {
              Success=false,
                ErrCode=400,
                Data=res
            };
          }
          return new ResponseModelTyped<AuthenticateTokenModel>()
          {
              Success = true,
              ErrCode = 200,
              Data = res
          };

        }
        catch (NpgsqlException ex)
        {
            Console.WriteLine(ex);
            return new ResponseModelTyped<AuthenticateTokenModel>()
            {
                Success = false,
                ErrCode = 500
            };
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex);  
          return new ResponseModelTyped<AuthenticateTokenModel>()
            {
                Success = false,
                ErrCode = 500
            };
        }
    }
  }
  
  public async Task<ResponseModelTyped<string>> AppointAdmin(AppointAdmin appointAdmin)
  {
    using (var con = new NpgsqlConnection(_dbConnectRepo.GetDatabaseConnection()))
    {
        con.Open();
        try
        {
          DynamicParameters para = new DynamicParameters();
          para.Add("_appointed_by",appointAdmin.getAppointedBy()); 
          para.Add("_username",appointAdmin.UserName); 
          para.Add("_position",appointAdmin.Position); 
          para.Add("_do_appoint",true); 

          // Call the function with the parameters and retrieve the results
          await con.ExecuteAsync($"SELECT appoint_admin(@_appointed_by,@_username,@_position,@_do_appoint)",para, commandType: CommandType.Text);

          // Return the result
          return new ResponseModelTyped<string>()
          {
              Success = true,
              ErrCode = 200,
              Data = "Admin added sucessfully"
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

  public async Task<ResponseModelTyped<string>> DisableAdmin(string username)
  {
    using (var con = new NpgsqlConnection(_dbConnectRepo.GetDatabaseConnection()))
    {
        con.Open();
        try
        {
          DynamicParameters para = new DynamicParameters();
          para.Add("_appointed_by"," "); 
          para.Add("_username",username); 
          para.Add("_position"," "); 
          para.Add("_do_appoint",false); 

          // Call the function with the parameters and retrieve the results
          await con.ExecuteAsync($"SELECT appoint_admin(@_appointed_by,@_username,@_position,@_do_appoint)",para, commandType: CommandType.Text);

          // Return the result
          return new ResponseModelTyped<string>()
          {
              Success = true,
              ErrCode = 200,
              Data = "Admin disabled sucessfully"
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

  public async Task<ResponseModelTyped<string>> EnableDisableUser(string username,bool isEnable)
  {
    using (var con = new NpgsqlConnection(_dbConnectRepo.GetDatabaseConnection()))
    {
        con.Open();
        try
        {
          DynamicParameters para = new DynamicParameters();
          para.Add("_username",username); 
          para.Add("_is_active",isEnable);

          // Call the function with the parameters and retrieve the results
          await con.ExecuteAsync($"UPDATE users SET is_active=@_is_active WHERE username=@_username",para, commandType: CommandType.Text);
          
          string messege;
          if(isEnable)
            messege="User enabled sucessfully";
          else
            messege="User disabled succesfully";

          // Return the result
          return new ResponseModelTyped<string>()
          {
              Success = true,
              ErrCode = 200,
              Data =messege 
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

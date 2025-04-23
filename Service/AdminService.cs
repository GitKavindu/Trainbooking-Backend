using Models.Dtos;
using Models;
using Interfaces;
namespace Service;

public class AdminService:IAdminService
{
  private IAdminDbRepo _adminDbRepo;
  private IUserDbRepo _userDbRepo;
  public AdminService(IAdminDbRepo adminDbRepo ,IUserDbRepo userDbRepo)
  {
    _adminDbRepo=adminDbRepo; 
    _userDbRepo=userDbRepo;
  }

  public async Task<ResponseModel> AppointAdmin(AppointAdmin appointAdmin)
  {
    //First check the authentication 
    ResponseModelTyped<AuthenticateTokenModel> res=await _adminDbRepo.AuthenticateUser(appointAdmin.TokenId);
    
    //Then check about appointed user
    ResponseModelTyped<UserDbModel> user=await _userDbRepo.CheckUserStatus(appointAdmin.UserName);

    if(res.Success ==true && user.Success==true)
    {
      if(res.Data.is_user_admin==true && res.Data.is_token_valid && res.Data.is_user_active)
      {

        if(user.Data.is_admin==false && user.Data.is_active==true)
        {
          //Then appoint admin
          appointAdmin.setAppointedBy(res.Data.username);
          return new ModdelMapper().ResponseToFormalResponse<string>(await _adminDbRepo.AppointAdmin(appointAdmin));
        }
        else
        {
          return new ResponseModel
          {
            Success=false,
              ErrCode=400,
              Data="User is already admin or User is inactive"
          };
        }
     
      }
      else
      {
        return new ResponseModel
        {
          Success=false,
            ErrCode=403,
            Data=res.Data
        };
      }
    }
    else
    {
      if(!res.Success)
        return new ModdelMapper().ResponseToFormalResponse<AuthenticateTokenModel>(res);
      else
        return new ModdelMapper().ResponseToFormalResponse<UserDbModel>(user);
    }
    
  }

  public async Task<ResponseModel> DisableAdmin(AppointAdmin appointAdmin)
  {
    //First check the authentication 
    ResponseModelTyped<AuthenticateTokenModel> res=await _adminDbRepo.AuthenticateUser(appointAdmin.TokenId);
    
    //Then check about appointed user
    ResponseModelTyped<UserDbModel> user=await _userDbRepo.CheckUserStatus(appointAdmin.UserName);

    if(res.Success ==true && user.Success==true)
    {
      if(res.Data.is_user_admin==true && res.Data.is_token_valid && res.Data.is_user_active)
      {

        if(user.Data.is_admin==true && user.Data.is_active==true)
        {
          //Then appoint admin
          return new ModdelMapper().ResponseToFormalResponse<string>(await _adminDbRepo.DisableAdmin(appointAdmin.UserName));
        }
        else
        {
          return new ResponseModel
          {
            Success=false,
              ErrCode=400,
              Data="User is not a admin or User is inactive"
          };
        }
     
      }
      else
      {
        return new ResponseModel
        {
          Success=false,
            ErrCode=403,
            Data=res.Data
        };
      }
    }
    else
    {
      if(!res.Success)
        return new ModdelMapper().ResponseToFormalResponse<AuthenticateTokenModel>(res);
      else
        return new ModdelMapper().ResponseToFormalResponse<UserDbModel>(user);
    }
    
  }
  
  public async Task<ResponseModel> DisableUser(AppointAdmin appointAdmin)
  {
    //First check the authentication 
    ResponseModelTyped<AuthenticateTokenModel> res=await _adminDbRepo.AuthenticateUser(appointAdmin.TokenId);
    
    //Then check about appointed user
    ResponseModelTyped<UserDbModel> user=await _userDbRepo.CheckUserStatus(appointAdmin.UserName);

    if(res.Success ==true && user.Success==true)
    {
      if(res.Data.is_user_admin==true && res.Data.is_token_valid && res.Data.is_user_active)
      {

        if(user.Data.is_admin==false && user.Data.is_active==true)
        {
          //Then appoint admin
          return new ModdelMapper().ResponseToFormalResponse<string>(await _adminDbRepo.EnableDisableUser(appointAdmin.UserName,false));
        }
        else
        {
          return new ResponseModel
          {
            Success=false,
              ErrCode=400,
              Data="User is a admin or User is already inactive"
          };
        }
     
      }
      else
      {
        return new ResponseModel
        {
          Success=false,
            ErrCode=403,
            Data=res.Data
        };
      }
    }
    else
    {
      if(!res.Success)
        return new ModdelMapper().ResponseToFormalResponse<AuthenticateTokenModel>(res);
      else
        return new ModdelMapper().ResponseToFormalResponse<UserDbModel>(user);
    }
    
  }

  public async Task<ResponseModel> EnableUser(AppointAdmin appointAdmin)
  {
    //First check the authentication 
    ResponseModelTyped<AuthenticateTokenModel> res=await _adminDbRepo.AuthenticateUser(appointAdmin.TokenId);
    
    //Then check about appointed user
    ResponseModelTyped<UserDbModel> user=await _userDbRepo.CheckUserStatus(appointAdmin.UserName);

    if(res.Success ==true && user.Success==true)
    {
      if(res.Data.is_user_admin==true && res.Data.is_token_valid && res.Data.is_user_active)
      {

        if(user.Data.is_active==false)
        {
          //Then appoint admin
          return new ModdelMapper().ResponseToFormalResponse<string>(await _adminDbRepo.EnableDisableUser(appointAdmin.UserName,true));
        }
        else
        {
          return new ResponseModel
          {
            Success=false,
              ErrCode=400,
              Data="User is already active"
          };
        }
     
      }
      else
      {
        return new ResponseModel
        {
          Success=false,
            ErrCode=403,
            Data=res.Data
        };
      }
    }
    else
    {
      if(!res.Success)
        return new ModdelMapper().ResponseToFormalResponse<AuthenticateTokenModel>(res);
      else
        return new ModdelMapper().ResponseToFormalResponse<UserDbModel>(user);
    }
    
  }

  public async Task<ResponseModel> GetUserStatus(string username)
  {
    ResponseModelTyped<bool> res=await _adminDbRepo.GetUserStatus(username);

    return new ModdelMapper().ResponseToFormalResponse<bool>(res);
  }
}


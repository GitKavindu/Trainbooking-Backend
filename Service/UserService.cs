using Interfaces;
using Models.Dtos;
using Models;

namespace Service;
public class UserService:IUserService
{
  private IUserDbRepo _userDbRepo;
  public UserService(IUserDbRepo userDbRepo)
  {
    _userDbRepo=userDbRepo; 
  }

  public async Task<ResponseModel> RegisterUser(RegisterUserDto registerUserDto)
  {
    ResponseModel validationModel=registerUserDto.ValidateDto();
    if(!validationModel.Success)
    {
      return validationModel; 
    }

    registerUserDto.Password=new CommonService().GenerateSha256Hash(registerUserDto.Password);
    
    UserDbModel userDbModel=new ModdelMapper().RegisterUserDtoToUsers(registerUserDto);
    userDbModel.is_admin=false;

    return await _userDbRepo.ResgisterUser(userDbModel,registerUserDto.Name); 
  }

  public async Task<ResponseModel> GetUserToken(UserSubmitModel userSubmitModel)
  {
      ResponseModel responseModel=userSubmitModel.ValidateDto();
      if(!responseModel.Success)
          return responseModel;

      userSubmitModel.UserPassword=new CommonService().GenerateSha256Hash(userSubmitModel.UserPassword);

      ResponseModelTyped<bool> res =await _userDbRepo.VerifyPassword(userSubmitModel);
      if(res.Data==true)
      {
        TokenModel token =new TokenModel();
        token.recievedDate=DateTime.Now;
        token.endDate=token.recievedDate.AddHours(20);
        token.username=userSubmitModel.UserName;
        token.tokenId=new CommonService().GenerateSha256Hash(token.username+token.recievedDate+token.endDate);

        ResponseModelTyped<IResult> response= await _userDbRepo.GetToken(token);
        return new ModdelMapper().ResponseToFormalResponse<IResult>(response);
      }
      else if(res.ErrCode!=500)
      {
        return new ResponseModel()
        {
          Success=false,
            ErrCode=403,
            Data=new ReturnErrDto("authentication failed")
        };
      }
      else
      {
        return new ModdelMapper().ResponseToFormalResponse<bool>(res);
      }
  }

  public async Task<ResponseModel> DisableToken(disableTokenModel disableTokenModel)
  {
    return new ModdelMapper().ResponseToFormalResponse<string>(await _userDbRepo.DisableToken(disableTokenModel));
  }

  
}

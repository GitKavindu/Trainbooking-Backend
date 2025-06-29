using Models;
using Models.Dtos;

namespace Interfaces;
public interface IUserDbRepo
{
  Task<ResponseModel> ResgisterUser(UserDbModel userDbModel,string[] userNames); 
  Task<ResponseModelTyped<bool>> VerifyPassword(UserSubmitModel userSubmitModel);
  Task<ResponseModelTyped<IResult>> GetToken(TokenModel tokenModel);
  Task<ResponseModelTyped<string>> DisableToken(disableTokenModel disableTokenModel);
  Task<ResponseModelTyped<UserDbModel>> CheckUserStatus(string username);

}

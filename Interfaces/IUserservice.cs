using Models.Dtos;
using Models;
namespace Interfaces;
public interface IUserService
{
  Task<ResponseModel> RegisterUser(RegisterUserDto registerUserDto);
  Task<ResponseModel> GetUserToken(UserSubmitModel userSubmitModel);
  Task<ResponseModel> DisableToken(disableTokenModel disableTokenModel);
}


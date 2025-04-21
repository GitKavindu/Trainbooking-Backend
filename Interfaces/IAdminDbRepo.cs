using Models;
using Models.Dtos;

namespace Interfaces;

public interface IAdminDbRepo
{
  Task<ResponseModelTyped<AuthenticateTokenModel>> AuthenticateUser(string tokenId);
  Task<ResponseModelTyped<string>> AppointAdmin(AppointAdmin appointAdmin);
  Task<ResponseModelTyped<string>> DisableAdmin(string username);
  Task<ResponseModelTyped<string>> EnableDisableUser(string username,bool Isenable);



}

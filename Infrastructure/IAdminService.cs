using Models.Dtos;
using Models;
namespace Interfaces;

public interface IAdminService
{
  Task<ResponseModel> AppointAdmin(AppointAdmin appointAdmin); 
  Task<ResponseModel> DisableAdmin(AppointAdmin appointAdmin);
  Task<ResponseModel> DisableUser(AppointAdmin appointAdmin);
  Task<ResponseModel> EnableUser(AppointAdmin appointAdmin);



}

using Microsoft.AspNetCore.Mvc;
using Interfaces;
using Models;
using Models.Dtos;

using Microsoft.AspNetCore.Http;

namespace TrainApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AdminController: ControllerBase
{
    

    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
      _adminService=adminService;
    }

    [HttpGet("testing")] 
    public string Testing()
    {
      return "Testing 1";
    }

    [HttpPost("appointAdmin")] 
    public async Task<ResponseModel> AppointAdmin([FromBody] AppointAdmin appointAdmin)
    {
      ResponseModel responseModel=await _adminService.AppointAdmin(appointAdmin);  

      HttpContext.Response.StatusCode = responseModel.ErrCode;

      return responseModel;
    }

   [HttpDelete("disableAdmin")] 
    public async Task<ResponseModel> disableAdmin([FromBody] AppointAdmin appointAdmin)
    {
      ResponseModel responseModel=await _adminService.DisableAdmin(appointAdmin);  

      HttpContext.Response.StatusCode = responseModel.ErrCode;

      return responseModel;
    }

    [HttpPost("enableUser")] 
    public async Task<ResponseModel> enableUser([FromBody] AppointAdmin appointAdmin)
    {
      ResponseModel responseModel=await _adminService.EnableUser(appointAdmin);  

      HttpContext.Response.StatusCode = responseModel.ErrCode;

      return responseModel;
    } 

    [HttpDelete("disableUser")] 
    public async Task<ResponseModel> disableUser([FromBody] AppointAdmin appointAdmin)
    {
      ResponseModel responseModel=await _adminService.DisableUser(appointAdmin);  

      HttpContext.Response.StatusCode = responseModel.ErrCode;

      return responseModel;
    }

    
}

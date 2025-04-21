using Microsoft.AspNetCore.Mvc;
using Interfaces;
using Models;
using Models.Dtos;

using Microsoft.AspNetCore.Http;

namespace TrainApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    

    private readonly IUserService _userService1;

    public UserController(IUserService userService)
    {
      _userService1=userService;
    }


    [HttpPost("registerUser")] 
    public async Task<ResponseModel> RegisterUser([FromBody] RegisterUserDto registerUserDto)
    {
      ResponseModel responseModel=await _userService1.RegisterUser(registerUserDto);  

      HttpContext.Response.StatusCode = responseModel.ErrCode;

      return responseModel;
    }

    [HttpPost("getuserToken")] 
    public async Task<ResponseModel> GetUserToken([FromBody] UserSubmitModel userSubmitModel) 
    {
      ResponseModel responseModel=await _userService1.GetUserToken(userSubmitModel);
      HttpContext.Response.StatusCode = responseModel.ErrCode;

      return responseModel;
    }
    
    [HttpDelete("deleteToken")] 
    public async Task<ResponseModel> DisableToken([FromBody] disableTokenModel disableTokenModel)
    {
      ResponseModel responseModel=await _userService1.DisableToken(disableTokenModel);
      HttpContext.Response.StatusCode = responseModel.ErrCode;

      return responseModel;
    }
}

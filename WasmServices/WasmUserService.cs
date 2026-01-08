using Interfaces;
using Models.Dtos;
using Models;
using System.Net.Http.Json;

namespace WasmServices;
public class WasmUserService:IUserService
{
  private HttpClient _httpClient;
  public WasmUserService(HttpClient httpClient)
  {
    _httpClient = httpClient;
  }

  public async Task<ResponseModel> RegisterUser(RegisterUserDto registerUserDto)
  {
   return new ResponseModel();
  }

  public async Task<ResponseModel> GetUserToken(UserSubmitModel userSubmitModel)
  {
    HttpResponseMessage response = await _httpClient.PostAsJsonAsync(
        "User/getuserToken",
        userSubmitModel
    );

    if (!response.IsSuccessStatusCode)
    {
        ResponseModelTyped<ReturnErrDto> err = await response.Content.ReadFromJsonAsync<ResponseModelTyped<ReturnErrDto>>();

        return new ResponseModel
        {
            Success = false,
            ErrCode = err.ErrCode,
            Data=err.Data.messege
        };
    }

    ResponseModelTyped<ReturnTokenDto> user = await response.Content.ReadFromJsonAsync<ResponseModelTyped<ReturnTokenDto>>();

    return new ResponseModel
    {
        Success = true,
        ErrCode= user.ErrCode,
        Data = user.Data
    };

  }

  public async Task<ResponseModel> DisableToken(disableTokenModel disableTokenModel)
  {
    return new ResponseModel();
  }

  public async Task<ResponseModel> GetTokenDetails(string tokenId)
  {
    return new ResponseModel();
  }
  
  public async Task<ResponseModel> GetUserDetails(disableTokenModel disableTokenModel)
  {
    return new ResponseModel();
  }
}

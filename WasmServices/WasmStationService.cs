using Interfaces;
using Models.Dtos;
using Models;
using System.Net.Http.Json;

namespace WasmServices;
public class WasmStationService:IStationService
{
  private HttpClient _httpClient;
  public WasmStationService(HttpClient httpClient)
  {
    _httpClient = httpClient;
  }

  public async  Task<ResponseModel> AddStation(AddStationDto stationDto)
  {
    return new ResponseModel();
  }

  public async  Task<ResponseModel> UpdateStation(AddStationDto stationDto)
  {
    return new ResponseModel();
  }
  public async  Task<ResponseModel> DeleteStation(AddStationDto stationDto)
  {
    return new ResponseModel();
  }

  public async  Task<ResponseModel> GetStations()
  {
    HttpResponseMessage response = await _httpClient.GetAsync(
        "/Station/getAllStations"
    );

    if (!response.IsSuccessStatusCode)
    {
        ResponseModel err = await response.Content.ReadFromJsonAsync<ResponseModel>();

        return new ResponseModel
        {
            Success = false,
            ErrCode = err.ErrCode
        };
    }

    ResponseModelTyped<IEnumerable<ReturnStationDto>> stations = 
      await response.Content.ReadFromJsonAsync<ResponseModelTyped<IEnumerable<ReturnStationDto>>>();

    return new ResponseModel
    {
        Success = true,
        ErrCode= stations.ErrCode,
        Data = stations.Data
    };
  }
}

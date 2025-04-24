using Microsoft.AspNetCore.Mvc;
using Interfaces;
using Models;
using Models.Dtos;

using Microsoft.AspNetCore.Http;

namespace TrainApi.Controllers;

[ApiController]
[Route("[controller]")]
public class StationController: ControllerBase
{
    

    private readonly IStationService _stationService;

    public StationController(IStationService stationService)
    {
       _stationService=stationService;
    }

    [HttpGet("getAllStations")] 
    public async Task<ResponseModel> GetAllStations()
    {
      ResponseModel responseModel=await _stationService.GetStations();  

      HttpContext.Response.StatusCode = responseModel.ErrCode;

      return responseModel;
    } 

    [HttpDelete("deleteStation")] 
    public async Task<ResponseModel> DeleteStation(AddStationDto stationDto)
    {
      ResponseModel responseModel=await _stationService.DeleteStation(stationDto);  

      HttpContext.Response.StatusCode = responseModel.ErrCode;

      return responseModel;
    }

    [HttpPost("addStation")] 
    public async Task<ResponseModel> AddStation(AddStationDto stationDto)
    {
      ResponseModel responseModel=await _stationService.AddStation(stationDto);

      HttpContext.Response.StatusCode = responseModel.ErrCode;

      return responseModel;
    } 

    [HttpPut("updateStation")] 
    public async Task<ResponseModel> UpdateStation(AddStationDto stationDto)
    {
      ResponseModel responseModel=await _stationService.UpdateStation(stationDto);

      HttpContext.Response.StatusCode = responseModel.ErrCode;

      return responseModel;
    }

    
}

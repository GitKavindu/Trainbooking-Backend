using Microsoft.AspNetCore.Mvc;
using Interfaces;
using Models;
using Models.Dtos;

using Microsoft.AspNetCore.Http;

namespace TrainApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TrainController: ControllerBase
{
    

    private readonly ITrainService _TrainService;

    public TrainController(ITrainService TrainService)
    {
       _TrainService=TrainService;
    }

    [HttpGet("getAllTrains")] 
    public async Task<ResponseModel> GetAllTrains()
    {
      ResponseModel responseModel=await _TrainService.GetTrains();  

      HttpContext.Response.StatusCode = responseModel.ErrCode;

      return responseModel;
    } 

    [HttpDelete("deleteTrain")] 
    public async Task<ResponseModel> DeleteTrain(AddTrainDto TrainDto)
    {
      ResponseModel responseModel=await _TrainService.DeleteTrain(TrainDto);  

      HttpContext.Response.StatusCode = responseModel.ErrCode;

      return responseModel;
    }

    [HttpPost("addTrain")] 
    public async Task<ResponseModel> AddTrain(AddTrainDto TrainDto)
    {
      ResponseModel responseModel=await _TrainService.AddTrain(TrainDto);

      HttpContext.Response.StatusCode = responseModel.ErrCode;

      return responseModel;
    } 

    [HttpPut("updateTrain")] 
    public async Task<ResponseModel> UpdateTrain(AddTrainDto TrainDto)
    {
      ResponseModel responseModel=await _TrainService.UpdateTrain(TrainDto);

      HttpContext.Response.StatusCode = responseModel.ErrCode;

      return responseModel;
    }

    
}

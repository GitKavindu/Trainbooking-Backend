using Microsoft.AspNetCore.Mvc;
using Interfaces;
using Models;
using Models.Dtos;

using Microsoft.AspNetCore.Http;

namespace TrainApi.Controllers;

[ApiController]
[Route("[controller]")]
public class JourneyController: ControllerBase
{
    

    private readonly IJourneyService _JourneyService;

    public JourneyController(IJourneyService JourneyService)
    {
       _JourneyService=JourneyService;
    }

    [HttpGet("getAllSchedules")] 
    public async Task<ResponseModel> GetAllJourneys()
    {
      ResponseModel responseModel=await _JourneyService.selectAllJourneys(); 

      HttpContext.Response.StatusCode = responseModel.ErrCode;

      return responseModel;
    } 

    [HttpPost("addJourney")] 
    public async Task<ResponseModel> AddJourney(AddJourneyDto JourneyDto)
    {
      ResponseModel responseModel=await _JourneyService.AddJourney(JourneyDto);

      HttpContext.Response.StatusCode = responseModel.ErrCode;

      return responseModel;
    } 

    [HttpPut("updateJourney")] 
    public async Task<ResponseModel> UpdateJourney([FromBody] AddJourneyDto JourneyDto) 
    {
      ResponseModel responseModel=await _JourneyService.UpdateJourney(JourneyDto);
      HttpContext.Response.StatusCode = responseModel.ErrCode;

      return responseModel;
    }
    
    [HttpDelete("deleteJourney/{scheduleId}")] 
    public async Task<ResponseModel> DeleteJourney(string scheduleId,[FromBody] disableTokenModel token)
    {
      ResponseModel responseModel=await _JourneyService.DeleteJourney(scheduleId,token.tokenId);
      HttpContext.Response.StatusCode = responseModel.ErrCode;

      return responseModel;
    }

}

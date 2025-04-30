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

    [HttpGet("getAJourney")] 
    public async Task<ResponseModel> GetAJourney(int scheduleId)
    {
      ResponseModel responseModel=await _JourneyService.selectAJourney(scheduleId); 

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

}

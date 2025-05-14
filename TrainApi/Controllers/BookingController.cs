using Microsoft.AspNetCore.Mvc;
using Interfaces;
using Models;
using Models.Dtos;

using Microsoft.AspNetCore.Http;

namespace TrainApi.Controllers;

[ApiController]
[Route("[controller]")]
public class BookingController: ControllerBase
{
    

    private readonly IBookingService _BookingService;

    public BookingController(IBookingService BookingService)
    {
       _BookingService=BookingService;
    }

    [HttpGet("selectAllSeatsForJourney")] 
    public async Task<ResponseModel> SelectAllSeatsForJourney(int journeyId)
    {
      ResponseModel responseModel=await _BookingService.SelectAllSeatsForJourney(journeyId); 

      HttpContext.Response.StatusCode = responseModel.ErrCode;

      return responseModel;
    } 

    [HttpGet("selectBookedSeatsForJourney")] 
    public async Task<ResponseModel> SelectBookedSeatsForJourney(int journeyId,int apartmentId)
    {
      ResponseModel responseModel=await _BookingService.SelectBookedSeatsForJourney(journeyId,apartmentId); 

      HttpContext.Response.StatusCode = responseModel.ErrCode;

      return responseModel;
    }

    [HttpGet("selectAllJourneysForSchedule")] 
    public async Task<ResponseModel> SelectAllJourneysForSchedule(string scheduleId)
    {
      ResponseModel responseModel=await _BookingService.SelectAllJourneysForSchedule(scheduleId); 

      HttpContext.Response.StatusCode = responseModel.ErrCode;

      return responseModel;
    } 

    

}

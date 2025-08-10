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
    public async Task<ResponseModel> SelectAllSeatsForJourney(string scheduleId,int apartmentId)
    {
      ResponseModel responseModel=await _BookingService.SelectAllSeatsForJourney(scheduleId,apartmentId); 

      HttpContext.Response.StatusCode = responseModel.ErrCode;

      return responseModel;
    } 

    [HttpGet("selectBookedSeatsForJourney")]
    public async Task<ResponseModel> SelectBookedSeatsForApartment(int fromJourneyId,int ToJourneyId,int apartmentId)
    {
      ResponseModel responseModel=await _BookingService.SelectBookedSeatsForApartment(fromJourneyId,ToJourneyId,apartmentId); 

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

    [HttpPost("BookForSchedule")] 
    public async Task<ResponseModel> BookForSchedule(AddBookingDto addBookingDto)
    {
      ResponseModel responseModel=await _BookingService.BookForSchedule(addBookingDto); 

      HttpContext.Response.StatusCode = responseModel.ErrCode;

      return responseModel;
    }

    [HttpGet("getBookingDetails")] 
    public async Task<ResponseModel> GetBookingDetails(int bookingId)
    {
      ResponseModel responseModel=await _BookingService.GetBookingDetails(bookingId); 

      HttpContext.Response.StatusCode = responseModel.ErrCode;

      return responseModel;
    }

    [HttpPost("cancelBooking")] 
    public async Task<ResponseModel> CancelBooking(CancelBookingDto cancelBookingDto)
    {
      ResponseModel responseModel=await _BookingService.CancelBooking(cancelBookingDto); 

      HttpContext.Response.StatusCode = responseModel.ErrCode;

      return responseModel;
    }

    [HttpPost("selectSortedSchedules")] 
    public async Task<ResponseModel> SelectSortedSchedules(GetSortedSchedulesDto getSortedSchedulesDto)
    {
      ResponseModel responseModel=await _BookingService.SelectSortedSchedules(getSortedSchedulesDto);

      HttpContext.Response.StatusCode = responseModel.ErrCode;

      return responseModel;
    }

    [HttpPost("selectBookingsForUser")] 
    public async Task<ResponseModel> SelectBookingsForUser(disableTokenModel disableTokenModel)
    {
      ResponseModel responseModel=await _BookingService.SelectBookingsForUser(disableTokenModel.tokenId); 

      HttpContext.Response.StatusCode = responseModel.ErrCode;

      return responseModel;
    }
}

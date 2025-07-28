using Models;
using Models.Dtos;

namespace Interfaces;
public interface IBookingService
{
  Task<ResponseModel> SelectAllSeatsForJourney(string scheduleId,int apartmentId);
  Task<ResponseModel> SelectSortedSchedules(GetSortedSchedulesDto getSortedSchedulesDto);
  Task<ResponseModel> SelectBookedSeatsForJourney(string scheduleId,int apartmentId);
  Task<ResponseModel> SelectAllJourneysForSchedule(string scheduleId);

  Task<ResponseModel> BookForSchedule(AddBookingDto addBookingDto);
  Task<ResponseModel> GetBookingDetails(int bookingId);
  Task<ResponseModel> CancelBooking(CancelBookingDto cancelBookingDto);

}

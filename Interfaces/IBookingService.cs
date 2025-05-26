using Models;
using Models.Dtos;

namespace Interfaces;
public interface IBookingService
{
  Task<ResponseModel> SelectAllSeatsForJourney(int journeyId);
  Task<ResponseModel> SelectBookedSeatsForJourney(int fromJourneyId,int toJourneyId,int apartmentId);
  Task<ResponseModel> SelectAllJourneysForSchedule(string scheduleId);

  Task<ResponseModel> BookForSchedule(AddBookingDto addBookingDto);
  Task<ResponseModel> GetBookingDetails(int bookingId);
  Task<ResponseModel> CancelBooking(CancelBookingDto cancelBookingDto);

}

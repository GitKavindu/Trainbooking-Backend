using Models;
using Models.Dtos;

namespace Interfaces;
public interface IBookingDbRepo
{
  Task<ResponseModelTyped<IEnumerable<SeatModel>>> SelectAllSeatsForJourney(string scheduleId,int apartmentId);
  Task<ResponseModelTyped<IEnumerable<ReturnSortedSchedulesDto>>> getSortedSchedules(GetSortedSchedulesDto getSortedSchedulesDto,bool onlyStart);
  Task<ResponseModelTyped<IEnumerable<SeatModel>>> SelectBookedSeatsForApartment(int fromJourneyId,int ToJourneyId,int apartmentId);
  Task<ResponseModelTyped<IEnumerable<SeatModel>>> SelectBookedSeatsForTrain(int fromJourneyId,int ToJourneyId,int trainId,int trainSeqNo);
  Task<ResponseModelTyped<IEnumerable<ReturnJourneyStationDto>>> SelectAllJourneysForSchedule(string scheduleId);
  Task<ResponseModelTyped<IEnumerable<ReturnBookingDetailsDto>>> SelectBookingsForUser(string username);

  Task<ResponseModelTyped<string>> BookForSchedule
  (
    AddBookingDto addBookingDto,string bookedUser,float netPrice,float[] prices
  );

  Task<ResponseModelTyped<ReturnBookingDetailsDto>> GetBookingDetails(int bookingId);
  Task<ResponseModelTyped<string>> CancelBooking(int bookingId,decimal refundPrice);

}

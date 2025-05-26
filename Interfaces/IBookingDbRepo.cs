using Models;
using Models.Dtos;

namespace Interfaces;
public interface IBookingDbRepo
{
  Task<ResponseModelTyped<IEnumerable<SeatModel>>> SelectAllSeatsForJourney(int journeyId);
  Task<ResponseModelTyped<IEnumerable<SeatModel>>> SelectBookedSeatsForApartment(int fromJourneyId,int ToJourneyId,int apartmentId);
  Task<ResponseModelTyped<IEnumerable<SeatModel>>> SelectBookedSeatsForTrain(int fromJourneyId,int ToJourneyId,int trainId,int trainSeqNo);
  Task<ResponseModelTyped<IEnumerable<ReturnJourneyStationDto>>> SelectAllJourneysForSchedule(string scheduleId);

  Task<ResponseModelTyped<string>> BookForSchedule
  (
    AddBookingDto addBookingDto,string bookedUser,float netPrice,float[] prices
  );

  Task<ResponseModelTyped<ReturnBookingDetailsDto>> GetBookingDetails(int bookingId);
  Task<ResponseModelTyped<string>> CancelBooking(int bookingId,float refundPrice);

}

using Models;
using Models.Dtos;

namespace Interfaces;
public interface IBookingDbRepo
{
  Task<ResponseModelTyped<IEnumerable<SeatModel>>> SelectAllSeatsForJourney(int journeyId);
  Task<ResponseModelTyped<IEnumerable<SeatModel>>> SelectBookedSeatsForJourney(int journeyId,int apartmentId);

}

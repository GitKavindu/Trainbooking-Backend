using Models;
using Models.Dtos;

namespace Interfaces;
public interface IBookingService
{
  Task<ResponseModel> SelectAllSeatsForJourney(int journeyId);

}

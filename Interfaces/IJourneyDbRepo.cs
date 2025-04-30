using Models;
using Models.Dtos;

namespace Interfaces;

public interface IJourneyDbRepo
{
  Task<ResponseModelTyped<string>> AddJourney(AddJourneyDto addJourneyDto,string username);
  Task<ResponseModelTyped<IEnumerable<ReturnJourneyDto>>> selectAJourney(int schedule_id);
}

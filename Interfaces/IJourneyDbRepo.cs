using Models;
using Models.Dtos;

namespace Interfaces;

public interface IJourneyDbRepo
{
  Task<ResponseModelTyped<string>> AddJourney(AddJourneyDto addJourneyDto,string username);
  Task<ResponseModelTyped<IEnumerable<ReturnJourneyDto>>> selectAJourney(string schedule_id);
  Task<ResponseModelTyped<string>> UpdateJourney(AddJourneyDto addJourneyDto,string username);
  Task<ResponseModelTyped<string>> DeleteJourney(string scheduleId);
}

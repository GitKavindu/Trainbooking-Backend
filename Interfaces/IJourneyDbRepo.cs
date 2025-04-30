using Models;
using Models.Dtos;

namespace Interfaces;

public interface IJourneyDbRepo
{
  Task<ResponseModelTyped<string>> AddJourney(AddJourneyDto addJourneyDto,string username);
  Task<ResponseModelTyped<IEnumerable<ReturnJourneyDto>>> selectAJourney(int schedule_id);
  Task<ResponseModelTyped<string>> UpdateJourney(int scheduleId,AddJourneyDto addJourneyDto,string username);
  Task<ResponseModelTyped<string>> DeleteJourney(int scheduleId);
}

using Models;
using Models.Dtos;

namespace Interfaces;

public interface IJourneyService
{
    Task<ResponseModel> AddJourney(AddJourneyDto JourneyDto);
    Task<ResponseModel> selectAJourney(int schedule_id);
    Task<ResponseModel> UpdateJourney(int scheduleId,AddJourneyDto JourneyDto);
    Task<ResponseModel> DeleteJourney(int scheduleId,string tokenId);
}

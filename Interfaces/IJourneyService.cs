using Models;
using Models.Dtos;

namespace Interfaces;

public interface IJourneyService
{
    Task<ResponseModel> AddJourney(AddJourneyDto JourneyDto);
    Task<ResponseModel> selectAJourney(string schedule_id);
    Task<ResponseModel> UpdateJourney(AddJourneyDto JourneyDto);
    Task<ResponseModel> DeleteJourney(string scheduleId,string tokenId);
}

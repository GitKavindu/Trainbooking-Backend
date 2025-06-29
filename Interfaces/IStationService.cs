using Models;
using Models.Dtos;

namespace Interfaces;

public interface IStationService
{
  Task<ResponseModel> AddStation(AddStationDto stationDto);
  Task<ResponseModel> UpdateStation(AddStationDto stationDto);
  Task<ResponseModel> DeleteStation(AddStationDto stationDto);
  Task<ResponseModel> GetStations();
}

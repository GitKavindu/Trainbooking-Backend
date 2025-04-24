using Models;
using Models.Dtos;

namespace Interfaces;

public interface IStationDbRepo
{
  Task<ResponseModelTyped<string>> UpsertStation(bool isUpdate,int stationId,string stationName,bool isActive,string username);
  Task<ResponseModelTyped<IEnumerable<ReturnStationDto>>> selectAllStations();
}

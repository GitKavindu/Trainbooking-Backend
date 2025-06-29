using Models;
using Models.Dtos;

namespace Interfaces;

public interface ITrainDbRepo
{
  Task<ResponseModelTyped<string>> UpsertTrain(bool isUpdate,int trainId,string trainName,bool isActive,string username);
  Task<ResponseModelTyped<IEnumerable<ReturnTrainnDto>>> selectAllTrains();
}

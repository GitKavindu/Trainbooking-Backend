using Models;
using Models.Dtos;

namespace Interfaces;

public interface ITrainService
{
  Task<ResponseModel> AddTrain(AddTrainDto addTrainDto);
  Task<ResponseModel> UpdateTrain(AddTrainDto TrainDto);
  Task<ResponseModel> DeleteTrain(AddTrainDto TrainDto);
  Task<ResponseModel> GetTrains();
}

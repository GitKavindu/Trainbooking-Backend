using Models.Dtos;
using Models;
namespace Interfaces;

public interface IApartmentDbRepo
{
  Task<ResponseModelTyped<string>> UpsertApartment(bool isUpdate,int apartmentId,string apartmentClass,int trainId,int trainSeqNo,bool isActive,string username,SeatModel []seatModel);
}

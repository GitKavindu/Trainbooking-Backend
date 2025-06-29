using Models.Dtos;
using Models;
using Npgsql;
namespace Interfaces;

public interface IApartmentDbRepo
{
  Task<ResponseModelTyped<string>> UpsertApartment(bool isUpdate,int apartmentId,string apartmentClass,int trainId,int trainSeqNo,bool isActive,string username,SeatModel []seatModel);
  Task<ResponseModelTyped<string>> UpsertApartment
  (
    bool isUpdate, int apartmentId, string apartmentClass, int trainId, int trainSeqNo, bool isActive, string username, SeatModel[] seatModel,NpgsqlConnection con
  );
  Task<ResponseModelTyped<IEnumerable<ReturnApartmentDto>>> selectAllApartmentsForTrain(int trainId,int seqNo);
  Task<ResponseModelTyped<int>> selectMaxSeqNoForTrain(int trainId);
}

using Models;
using Models.Dtos;

namespace Interfaces;

public interface IApartmentService
{
  Task<ResponseModel> AddApartment(AddApartmentDto ApartmentDto);
  Task<ResponseModel> UpdateApartment(AddApartmentDto ApartmentDto);
  Task<ResponseModel> DeleteApartment(AddApartmentDto ApartmentDto);
  //Task<ResponseModel> GetApartments();
}

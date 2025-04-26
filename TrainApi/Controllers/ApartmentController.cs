using Microsoft.AspNetCore.Mvc;
using Interfaces;
using Models;
using Models.Dtos;

using Microsoft.AspNetCore.Http;

namespace TrainApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ApartmentController: ControllerBase
{
    

    private readonly IApartmentService _ApartmentService;

    public ApartmentController(IApartmentService ApartmentService)
    {
       _ApartmentService=ApartmentService;
    }

    [HttpGet("getAllApartmentsForTrain")] 
    public async Task<ResponseModel> getAllApartmentsForTrain(int trainId,int seqNo)
    {
      ResponseModel responseModel=await _ApartmentService.GetApartmentsForTrain(trainId,seqNo);  

      HttpContext.Response.StatusCode = responseModel.ErrCode;

      return responseModel;
    } 

    [HttpDelete("deleteApartment")] 
    public async Task<ResponseModel> DeleteApartment(AddApartmentDto ApartmentDto)
    {
      ResponseModel responseModel=await _ApartmentService.DeleteApartment(ApartmentDto);  

      HttpContext.Response.StatusCode = responseModel.ErrCode;

      return responseModel;
    }

    [HttpPost("addApartment")] 
    public async Task<ResponseModel> AddApartment(AddApartmentDto ApartmentDto)
    {
      ResponseModel responseModel=await _ApartmentService.AddApartment(ApartmentDto);

      HttpContext.Response.StatusCode = responseModel.ErrCode;

      return responseModel;
    } 

    [HttpPut("updateApartment")] 
    public async Task<ResponseModel> UpdateApartment(AddApartmentDto ApartmentDto)
    {
      ResponseModel responseModel=await _ApartmentService.UpdateApartment(ApartmentDto);

      HttpContext.Response.StatusCode = responseModel.ErrCode;

      return responseModel;
    }

    
}

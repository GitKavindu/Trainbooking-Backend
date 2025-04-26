using Models.Dtos;
using Models;
using Interfaces;
namespace Service;

public class ApartmentService:IApartmentService
{
  private IAdminDbRepo _adminDbRepo;
  private IApartmentDbRepo  _ApartmentDbRepo;
  public ApartmentService(IAdminDbRepo adminDbRepo,IApartmentDbRepo ApartmentDbRepo)
  {
    _adminDbRepo=adminDbRepo;
    _ApartmentDbRepo=ApartmentDbRepo;
  }
 
  public async Task<ResponseModel> GetApartmentsForTrain(int trainId,int seqNo)
  {
    return new ModdelMapper().ResponseToFormalResponse<IEnumerable<ReturnApartmentDto>>(await _ApartmentDbRepo.selectAllApartmentsForTrain(trainId,seqNo));
  }
//bool isUpdate, int apartmentId, string apartmentClass, int trainId, int trainSeqNo, bool isActive, string username, SeatModel[] seatModel
  public async Task<ResponseModel> AddApartment(AddApartmentDto ApartmentDto)
  {
    //First check the authentication 
    ResponseModelTyped<AuthenticateTokenModel> res=await _adminDbRepo.AuthenticateUser(ApartmentDto.tokenId);

    if(res.Success ==true && res.Data.is_token_valid)
    {
      if(res.Data.is_user_admin==true && res.Data.is_token_valid && res.Data.is_user_active)
      {
        ResponseModel returnModel= new ModdelMapper().ResponseToFormalResponse<string>
        (
            await _ApartmentDbRepo.UpsertApartment
            (false,ApartmentDto.apartment_id,ApartmentDto._class,ApartmentDto.train_id,ApartmentDto.train_seq_no,true,res.Data.username,ApartmentDto.seatModel)
        );

        if(returnModel.Success==true)
            returnModel.ErrCode=201;

        return returnModel;
      }
      else
      {
        return new ResponseModel
        {
            Success=false,
            ErrCode=403,
            Data=res.Data
        };
      }
    }
    else if(res.Success)
    {
        return new ResponseModel
        {
            Success=false,
            ErrCode=401,
            Data=res.Data
        };
    }  
    else
    {
        return new ModdelMapper().ResponseToFormalResponse<AuthenticateTokenModel>(res);
    }
  
  }

  public async Task<ResponseModel> UpdateApartment(AddApartmentDto ApartmentDto)
  {
    //First check the authentication 
    ResponseModelTyped<AuthenticateTokenModel> res=await _adminDbRepo.AuthenticateUser(ApartmentDto.tokenId);

    if(res.Success ==true && res.Data.is_token_valid)
    {
      if(res.Data.is_user_admin==true && res.Data.is_token_valid && res.Data.is_user_active)
      {
        ResponseModel returnModel= new ModdelMapper().ResponseToFormalResponse<string>
        (
            await _ApartmentDbRepo.UpsertApartment
            (true,ApartmentDto.apartment_id,ApartmentDto._class,ApartmentDto.train_id,ApartmentDto.train_seq_no,true,res.Data.username,ApartmentDto.seatModel)
        );

        if(returnModel.Success==true)
            returnModel.ErrCode=201;

        return returnModel;
      }
      else
      {
        return new ResponseModel
        {
            Success=false,
            ErrCode=403,
            Data=res.Data
        };
      }
    }
    else if(res.Success)
    {
        return new ResponseModel
        {
            Success=false,
            ErrCode=401,
            Data=res.Data
        };
    }  
    else
    {
        return new ModdelMapper().ResponseToFormalResponse<AuthenticateTokenModel>(res);
    }
  
  }

  public async Task<ResponseModel> DeleteApartment(AddApartmentDto ApartmentDto)
  {
    //First check the authentication 
    ResponseModelTyped<AuthenticateTokenModel> res=await _adminDbRepo.AuthenticateUser(ApartmentDto.tokenId);

    if(res.Success ==true && res.Data.is_token_valid)
    {
      if(res.Data.is_user_admin==true && res.Data.is_token_valid && res.Data.is_user_active)
      {
        return new ModdelMapper().ResponseToFormalResponse<string>
        (
            await _ApartmentDbRepo.UpsertApartment
            (true,ApartmentDto.apartment_id,ApartmentDto._class,ApartmentDto.train_id,ApartmentDto.train_seq_no,false,res.Data.username,null)
        );

      }
      else
      {
        return new ResponseModel
        {
            Success=false,
            ErrCode=403,
            Data=res.Data
        };
      }
    }
    else if(res.Success)
    {
        return new ResponseModel
        {
            Success=false,
            ErrCode=401,
            Data=res.Data
        };
    }  
    else
    {
        return new ModdelMapper().ResponseToFormalResponse<AuthenticateTokenModel>(res);
    }
  
  }
}


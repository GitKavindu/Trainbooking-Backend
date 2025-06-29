using Models.Dtos;
using Models;
using Interfaces;
namespace Service;

public class TrainService:ITrainService
{
  private IAdminDbRepo _adminDbRepo;
  private ITrainDbRepo  _TrainDbRepo;
  public TrainService(IAdminDbRepo adminDbRepo,ITrainDbRepo TrainDbRepo)
  {
    _adminDbRepo=adminDbRepo;
    _TrainDbRepo=TrainDbRepo;
  }
 
  public async Task<ResponseModel> GetTrains()
  {
    return new ModdelMapper().ResponseToFormalResponse<IEnumerable<ReturnTrainnDto>>(await _TrainDbRepo.selectAllTrains());
  }

  public async Task<ResponseModel> AddTrain(AddTrainDto TrainDto)
  {
    //First check the authentication 
    ResponseModelTyped<AuthenticateTokenModel> res=await _adminDbRepo.AuthenticateUser(TrainDto.token_id);

    if(res.Success ==true && res.Data.is_token_valid)
    {
      if(res.Data.is_user_admin==true && res.Data.is_token_valid && res.Data.is_user_active)
      {
        ResponseModel returnModel= new ModdelMapper().ResponseToFormalResponse<string>
        (
            await _TrainDbRepo.UpsertTrain(false,int.Parse(TrainDto.train_id),TrainDto.train_name,true,res.Data.username)
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

  public async Task<ResponseModel> UpdateTrain(AddTrainDto TrainDto)
  {
    //First check the authentication 
    ResponseModelTyped<AuthenticateTokenModel> res=await _adminDbRepo.AuthenticateUser(TrainDto.token_id);

    if(res.Success ==true && res.Data.is_token_valid)
    {
      if(res.Data.is_user_admin==true && res.Data.is_token_valid && res.Data.is_user_active)
      {
        ResponseModel returnModel= new ModdelMapper().ResponseToFormalResponse<string>
        (
            await _TrainDbRepo.UpsertTrain(true,int.Parse(TrainDto.train_id),TrainDto.train_name,true,res.Data.username)
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

  public async Task<ResponseModel> DeleteTrain(AddTrainDto TrainDto)
  {
    //First check the authentication 
    ResponseModelTyped<AuthenticateTokenModel> res=await _adminDbRepo.AuthenticateUser(TrainDto.token_id);

    if(res.Success ==true && res.Data.is_token_valid)
    {
      if(res.Data.is_user_admin==true && res.Data.is_token_valid && res.Data.is_user_active)
      {
        return new ModdelMapper().ResponseToFormalResponse<string>
        (
            await _TrainDbRepo.UpsertTrain(true,int.Parse(TrainDto.train_id),TrainDto.train_name,false,res.Data.username)
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


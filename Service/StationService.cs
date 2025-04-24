using Models.Dtos;
using Models;
using Interfaces;
namespace Service;

public class StationService:IStationService
{
  private IAdminDbRepo _adminDbRepo;
  private IStationDbRepo  _stationDbRepo;
  public StationService(IAdminDbRepo adminDbRepo,IStationDbRepo stationDbRepo)
  {
    _adminDbRepo=adminDbRepo;
    _stationDbRepo=stationDbRepo;
  }
 
  public async Task<ResponseModel> GetStations()
  {
    return new ModdelMapper().ResponseToFormalResponse<IEnumerable<ReturnStationDto>>(await _stationDbRepo.selectAllStations());
  }

  public async Task<ResponseModel> AddStation(AddStationDto stationDto)
  {
    //First check the authentication 
    ResponseModelTyped<AuthenticateTokenModel> res=await _adminDbRepo.AuthenticateUser(stationDto.token_id);

    if(res.Success ==true && res.Data.is_token_valid)
    {
      if(res.Data.is_user_admin==true && res.Data.is_token_valid && res.Data.is_user_active)
      {
        ResponseModel returnModel= new ModdelMapper().ResponseToFormalResponse<string>
        (
            await _stationDbRepo.UpsertStation(false,int.Parse(stationDto.station_id),stationDto.station_name,true,res.Data.username)
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

  public async Task<ResponseModel> UpdateStation(AddStationDto stationDto)
  {
    //First check the authentication 
    ResponseModelTyped<AuthenticateTokenModel> res=await _adminDbRepo.AuthenticateUser(stationDto.token_id);

    if(res.Success ==true && res.Data.is_token_valid)
    {
      if(res.Data.is_user_admin==true && res.Data.is_token_valid && res.Data.is_user_active)
      {
        ResponseModel returnModel= new ModdelMapper().ResponseToFormalResponse<string>
        (
            await _stationDbRepo.UpsertStation(true,int.Parse(stationDto.station_id),stationDto.station_name,true,res.Data.username)
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

  public async Task<ResponseModel> DeleteStation(AddStationDto stationDto)
  {
    //First check the authentication 
    ResponseModelTyped<AuthenticateTokenModel> res=await _adminDbRepo.AuthenticateUser(stationDto.token_id);

    if(res.Success ==true && res.Data.is_token_valid)
    {
      if(res.Data.is_user_admin==true && res.Data.is_token_valid && res.Data.is_user_active)
      {
        return new ModdelMapper().ResponseToFormalResponse<string>
        (
            await _stationDbRepo.UpsertStation(true,int.Parse(stationDto.station_id),stationDto.station_name,false,res.Data.username)
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


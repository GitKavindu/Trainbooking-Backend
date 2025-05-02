using Models.Dtos;
using Models;
using Interfaces;
namespace Service;

public class JourneyService:IJourneyService
{
  private IAdminDbRepo _adminDbRepo;
  private IJourneyDbRepo  _JourneyDbRepo;
  public JourneyService(IAdminDbRepo adminDbRepo,IJourneyDbRepo JourneyDbRepo)
  {
    _adminDbRepo=adminDbRepo;
    _JourneyDbRepo=JourneyDbRepo;
  }
 
//   public async Task<ResponseModel> GetJourneys()
//   {
//     return new ModdelMapper().ResponseToFormalResponse<IEnumerable<ReturnJourneyDto>>(await _JourneyDbRepo.selectAllJourneys());
//   }
  public async Task<ResponseModel> selectAJourney(string schedule_id)
  {
    return new ModdelMapper().ResponseToFormalResponse<IEnumerable<ReturnJourneyDto>>( await _JourneyDbRepo.selectAJourney(schedule_id) );
  }
  public async Task<ResponseModel> AddJourney(AddJourneyDto JourneyDto)
  {
    //First check the authentication 
    ResponseModelTyped<AuthenticateTokenModel> res=await _adminDbRepo.AuthenticateUser(JourneyDto.tokenId);

    if(res.Success ==true && res.Data.is_token_valid)
    {
      if(res.Data.is_user_admin==true && res.Data.is_token_valid && res.Data.is_user_active)
      {
        JourneyDto.scheduleId=new CommonService().GenerateSha256Hash(DateTime.Now.ToString()+JourneyDto.trainId+JourneyDto.trainSeqNo);
        ResponseModel returnModel= new ModdelMapper().ResponseToFormalResponse<string>
        (
            await _JourneyDbRepo.AddJourney(JourneyDto,res.Data.username)
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

  public async Task<ResponseModel> UpdateJourney(AddJourneyDto JourneyDto)
  {
    //First check the authentication 
    ResponseModelTyped<AuthenticateTokenModel> res=await _adminDbRepo.AuthenticateUser(JourneyDto.tokenId);

    if(res.Success ==true && res.Data.is_token_valid)
    {
      if(res.Data.is_user_admin==true && res.Data.is_token_valid && res.Data.is_user_active)
      {
        ResponseModel returnModel= new ModdelMapper().ResponseToFormalResponse<string>
        (
            await _JourneyDbRepo.UpdateJourney(JourneyDto,res.Data.username)
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

   public async Task<ResponseModel> DeleteJourney(string scheduleId,string tokenId)
  {
    //First check the authentication 
    ResponseModelTyped<AuthenticateTokenModel> res=await _adminDbRepo.AuthenticateUser(tokenId);

    if(res.Success ==true && res.Data.is_token_valid)
    {
      if(res.Data.is_user_admin==true && res.Data.is_token_valid && res.Data.is_user_active)
      {
        ResponseModel returnModel= new ModdelMapper().ResponseToFormalResponse<string>
        (
            await _JourneyDbRepo.DeleteJourney(scheduleId)
        );

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
}


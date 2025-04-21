using Models.Dtos;

namespace Models;

public class ModdelMapper
{
  public UserDbModel RegisterUserDtoToUsers(RegisterUserDto registerUserDto)
  {
    return new UserDbModel
    {
      username=registerUserDto.UserName,
        password=registerUserDto.Password,
        email=registerUserDto.Email,
        national_id=registerUserDto.NationalId,
        mobile_no=registerUserDto.MobileNo,
        prefered_name=registerUserDto.PreferedName
    };
  }

  public ResponseModel ResponseToFormalResponse<T>(ResponseModelTyped<T> responseModel)
  {
      return new ResponseModel()
      {
        Success=responseModel.Success,
          ErrCode=responseModel.ErrCode,
          Data=responseModel.Data
      };
  }
}

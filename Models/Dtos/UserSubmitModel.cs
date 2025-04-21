namespace Models.Dtos;
public class UserSubmitModel
{
  public string ?UserName{get;set;}
  public string ?UserPassword{get;set;}
  

  private ValidationModel validationModel=new ValidationModel();

    private void ValidateNullFields()
    {
  
      if (string.IsNullOrWhiteSpace(UserName))
            validationModel.errors.Add("Username is required."); 

         if (string.IsNullOrWhiteSpace(UserPassword))
            validationModel.errors.Add("UserPassword is required.");
    }

    public ResponseModel ValidateDto()
    {
      ValidateNullFields();
      if(validationModel.errors.Count()==0)
      {
        validationModel.IsValidated=true;
      }
      else
      {
        validationModel.IsValidated=false;
      }
      return new ResponseModel()
        {
          Success=validationModel.IsValidated,
            ErrCode=validationModel.IsValidated ? 200:400,
            Data=validationModel.errors
        };
    }

}

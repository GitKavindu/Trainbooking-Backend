using System;
namespace Models.Dtos
{
    public class RegisterUserDto
    {
      public string ?UserName{get;set;}
      public string ?MobileNo{get;set;}
      public string ?Email{get;set;}
      public string ?NationalId{get;set;}
      public string ?Password{get;set;}
      public string ?PreferedName{get;set;}
      public string [] Name{get;set;}
      
      private ValidationModel validationModel=new ValidationModel();
      public ResponseModel ValidateDto()
      {
        ValidateNullFields();
        if(validationModel.errors.Count!=0)
        {
          validationModel.IsValidated=false;
          return new ResponseModel()
          {
            Success=validationModel.IsValidated,
            ErrCode=validationModel.IsValidated? 200:400,
            Data=validationModel.errors
          }; 
        }

        ValidateIdNo();
        ValidateMobileNo();
        ValidatePreferedName();
        
        if(validationModel.errors.Count!=0)
        {
          validationModel.IsValidated=false;
        }
        else
        {
          validationModel.IsValidated=true;
        }

        return new ResponseModel(){
          Success=validationModel.IsValidated,
          ErrCode=validationModel.IsValidated? 200:400,
          Data=validationModel.errors
        };

      }

      private void ValidateNullFields()
      {
         if (string.IsNullOrWhiteSpace(UserName))
            validationModel.errors.Add("Username is required."); 

         if (string.IsNullOrWhiteSpace(MobileNo))
            validationModel.errors.Add("MobileNo is required.");

         if (string.IsNullOrWhiteSpace(Email))
            validationModel.errors.Add("Emal is required.");

         if (string.IsNullOrWhiteSpace(NationalId))
            validationModel.errors.Add("NationalId is required."); 

         if (string.IsNullOrWhiteSpace(Password))
            validationModel.errors.Add("Password is required.");

         if (string.IsNullOrWhiteSpace(PreferedName))
            validationModel.errors.Add("PreferedName is required.");

         if (Name.Any(c=>string.IsNullOrWhiteSpace(c)))
            validationModel.errors.Add("Name filelds cannot be empty.");

      }

      public void ValidateMobileNo()
      {

        for(int i=0;i<MobileNo.Length;i++)
        {
          if(!Char.IsDigit(MobileNo[i]))
          {
            validationModel.errors.Add("Mobile number should contain only digits");
            return;
          }
        }

        if(MobileNo.Length>10)
        {
          validationModel.errors.Add("Moblie number must contain only 10 digits");
        }
      }

      public void ValidateIdNo()
      {
        //If a old id 10 charactors ending v 
        if(NationalId[NationalId.Length-1]=='V')
        {
          if(NationalId.Length!=10)
            validationModel.errors.Add("Old ID No should contain 10 charactors");

          for(int i=0;i<NationalId.Length-1;i++)
          {
            if(!Char.IsDigit(NationalId[i]))
            {
              validationModel.errors.Add("Old id no must contain 9 digits");
              break;
            }
          }
        }
        //If a new id 11 digits
        else
        {
          if(NationalId.Length!=11)
            validationModel.errors.Add("New ID No should contain 11 charactors");

          for(int i=0;i<NationalId.Length-1;i++)
          {
            if(Char.IsDigit(NationalId[i]))
            {
              validationModel.errors.Add("New id no must contain 11 digits");
              break;
            }
          }
        }
      }

      public void ValidatePreferedName()
      {
        bool isPresent=false;
        for(int i=0;i<Name.Length;i++)
        {
          if(Name[i]==PreferedName)
          {
            isPresent=true;
            break;
          }
        }

        if(!isPresent)
            validationModel.errors.Add("Prefered name is not present in full name");
      }


  }
}

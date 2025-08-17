namespace Models.Dtos
{
    public class ReturnUUserStatusDto:IResult
    {
      public string ?UserName{get;set;}
      public string ?PreferedName{get;set;}

      public string ?MobileNo{get;set;}
      public string ?Email{get;set;}

      public string ?NationalId{get;set;}
      
      public bool IsAdmin{get;set;}
      public bool ?IsActive{get;set;}
    }

}

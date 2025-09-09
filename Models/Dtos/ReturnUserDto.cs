namespace Models.Dtos
{
    public class ReturnUserDto
    {
      public string ?UserName{get;set;}
      public string ?PreferedName{get;set;}
      
      public bool IsAdmin{get;set;}
      public bool ?IsActive{get;set;}
    }
}

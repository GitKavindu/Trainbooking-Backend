namespace Models.Dtos;
    public class GetUserStatusDto
    {
      public string ?tokenId{get;set;}
      public string ?value{get;set;}
      public int columnId{get;set;}
      public int IsAdmin{get;set;}
      public int ?IsActive{get;set;}
    }
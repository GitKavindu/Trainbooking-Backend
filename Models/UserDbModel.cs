namespace Models;

public class UserDbModel
{
  public string username{get;set;}
  public string mobile_no{get;set;}
  public string email{get;set;}
  public string national_id{get;set;}
  public string password{get;set;}
  public bool is_admin{get;set;}
  public string prefered_name{get;set;}
  public bool is_active{get;set;}=true;

}

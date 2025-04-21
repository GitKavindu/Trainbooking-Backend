namespace Models;

public class AuthenticateTokenModel
{
  public bool is_token_valid{get;set;}
	public string ?username {get;set;}
  public bool is_user_active{get;set;}
  public bool is_user_admin{get;set;}
}

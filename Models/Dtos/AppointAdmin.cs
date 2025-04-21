namespace Models.Dtos;
public class AppointAdmin
{
  public string ?TokenId{get;set;}
  public string ?UserName{get;set;}
  public string ?Position{get;set;}
  private string _appointed_by;

  public void setAppointedBy(string username)
  {
    _appointed_by=username;
  }
  
  public string getAppointedBy()
  {
    return _appointed_by;
  }
}

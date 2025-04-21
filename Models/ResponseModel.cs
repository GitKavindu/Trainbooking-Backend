namespace Models;
public class ResponseModel
{
  public bool Success {get;set;}
  public int ErrCode{get;set;}
  public object? Data {get;set;}
}

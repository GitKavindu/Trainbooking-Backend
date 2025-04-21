namespace Models;
public class ResponseModelTyped<T>
{
  public bool Success {get;set;}
  public int ErrCode{get;set;}
  public T? Data {get;set;}
}

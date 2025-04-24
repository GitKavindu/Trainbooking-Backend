namespace Models.Dtos;
public class ReturnTrainnDto
{
  public int ?train_no{get;set;}
  public string ?train_name{get;set;}
  public string ?added_by{get;set;}

  private DateTime? _createdDate;
  private DateTime? _lastUpdatedDate;

  public string created_date
  {
      get => _createdDate?.ToString("yyyy-MM-dd");
      set => _createdDate = string.IsNullOrWhiteSpace(value) 
          ? (DateTime?)null 
          : DateTime.Parse(value);
  }

  public string lastUpdated_date
  {
      get => _lastUpdatedDate?.ToString("yyyy-MM-dd");
      set => _lastUpdatedDate = string.IsNullOrWhiteSpace(value) 
          ? (DateTime?)null 
          : DateTime.Parse(value);
  }

}
namespace Models.Dtos;
public class ReturnApartmentDto
{
  public int ?Apartment_id{get;set;}
  public int ?train_seq_no{get;set;}
  public string ?ApartmrntClass{get;set;}
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

  public SeatModel[] seatModel{get;set;}

}
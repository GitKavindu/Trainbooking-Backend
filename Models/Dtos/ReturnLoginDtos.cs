namespace Models.Dtos;
public interface IResult { }

public class ReturnTokenDto : IResult
{
    public  string tokenId { get; set; }
    public string username  { get; set; }
    public DateTime endTime { get; set; }
    public bool isActive { get; set; }
}

public class ReturnErrDto : IResult
{
    public string messege { get; set; }

    public ReturnErrDto(string messege)
    {
        this.messege = messege;
    }
}


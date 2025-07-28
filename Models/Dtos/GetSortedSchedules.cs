namespace Models.Dtos;
public class GetSortedSchedulesDto
{
    public int ?startStationId{get;set;}
    public int ?startStationSeqNo{get;set;}
    private string _startDate;
    public string ?startDate
    {
        set
        {
            _startDate=value;
        }
    }

    private string _startTime;
    public string ?startTime
    {
        set
        {
            _startTime=value;
        }
    }

    public DateTime scheduledStartTime 
    {
        get
        {
            return new CommonService().CombineDateAndTime(_startDate,_startTime);
        }
    }

    //-----------

     public int ?endStationId{get;set;}
    public int ?endStationSeqNo{get;set;}
    private string _endDate;
    public string ?endDate
    {
        set
        {
            _endDate=value;
        }
    }

    private string _endTime;
    public string ?endTime
    {
        set
        {
            _endTime=value;
        }
    }

    public DateTime scheduledEndTime 
    {
        get
        {
            return new CommonService().CombineDateAndTime(_endDate,_endTime);
        }
    }

}
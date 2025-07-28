namespace Models.Dtos;
public class ReturnSortedSchedulesDto
{
    public string ?scheduleId{get;set;}
    public int ?trainId{get;set;}
    public int ?trainSeqNo{get;set;}
    public string ?trainName{get;set;}

    public int ?startJourneyId{get;set;}
    public int ?startStationId{get;set;}
    public int ?startSeqNo{get;set;}
    public string ?startStationName{get;set;}
    
    public string ?startDate
    {
        get
        {
           return _scheduledStartTime.ToString("yyyy-MM-dd");
        }
    }

    public string ?startTime
    {
        get
        {
            return _scheduledStartTime.ToString("hh.mm tt");
        }
    }
    
    private DateTime _scheduledStartTime ;
    public DateTime scheduledStartTime 
    {
        set
        {
            _scheduledStartTime=value;
        }
    }

    //------------------------------

     public int ?endJourneyId{get;set;}
    public int ?endStationId{get;set;}
    public int ?endSeqNo{get;set;}
    public string ?endStationName{get;set;}
    
    public string ?endDate
    {
        get
        {
           return _scheduledendTime.ToString("yyyy-MM-dd");
        }
    }

    public string ?endTime
    {
        get
        {
            return _scheduledendTime.ToString("hh.mm tt");
        }
    }
    
    private DateTime _scheduledendTime ;
    public DateTime scheduledEndTime 
    {
        set
        {
            _scheduledendTime=value;
        }
    }

}
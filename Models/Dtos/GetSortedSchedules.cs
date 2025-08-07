using System.Globalization;

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
            if(validateStartDate()==true)
                return new CommonService().CombineDateAndTime(_startDate,_startTime);
            else
                return new DateTime();
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
            if(validateEndDate()==true)
                return new CommonService().CombineDateAndTime(_endDate,_endTime);
            else
                return new DateTime();
        }
    }

    private bool validateDate(string input)
    {
        return DateTime.TryParseExact(input,"yyyy-MM-dd",CultureInfo.InvariantCulture,DateTimeStyles.None,out _);
    }

    private bool validateTime(string input)
    {
        return DateTime.TryParseExact(input,"hh:mm tt",CultureInfo.InvariantCulture,DateTimeStyles.None,out _);
    }

    private bool validateStartDate()
    {
        return validateDate(_startDate) && validateTime(_startTime);
    }
    
    private bool validateEndDate()
    {
        return validateDate(_endDate) && validateTime(_endTime);
    }
}   

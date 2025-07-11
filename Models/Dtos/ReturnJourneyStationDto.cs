namespace Models.Dtos
{
    public class ReturnJourneyStationDto
    {
        public string? scheduleId{get;set;}
        public string? startstation{get;set;}
        public string? endstation{get;set;}
        public DateTime startTime
        {   
            set
            {
                this.startingDate=value.ToString("yyyy-MM-dd");
                this.startingTime=value.ToString("hh:mm tt");
            }
        } 
        public DateTime endTime
        {
            set
            {
                this.endingDate=value.ToString("yyyy-MM-dd");
                this.endingTime=value.ToString("hh:mm tt");
            }
        }

        public int startJourneyId{get;set;} 
        public int endJourneyId{get;set;}
        
        public string startingDate{get;set;}
        public string startingTime{get;set;}

        public string endingDate{get;set;}
        public string endingTime{get;set;}

        public string train{get;set;}
    }
}
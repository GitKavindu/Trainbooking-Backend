namespace Models.Dtos
{
    public class AddJourneyStationDto
    {
        public DateTime ScheduledStartTime 
        {
            get
            {
                return new CommonService().CombineDateAndTime(StartDate,StartTime);
            }
        }

        public int StationId{get;set;}

        public int StationSeqNo{get;set;}

        public string StartDate{get;set;}
        public string StartTime{get;set;}
    }
}
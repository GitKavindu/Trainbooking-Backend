namespace Models.Dtos
{
    public class AddJourneyDto
    {
        public int scheduleId{get;set;}
        public int trainId{get;set;}
        public int trainSeqNo{get;set;}
        
        public string? tokenId{get;set;}

        public AddJourneyStationDto []? addJourneyStationDto{get;set;}
    }
}
namespace Models.Dtos
{
    public class AddBookingDto
    {        
        public string? tokenId{get;set;}

        public int scheduleId{get;set;}

        public int fromJourneyId{get;set;}
        public int ToJourneyId{get;set;}
        public SeatModel[] ?seatModel{get;set;}
        
    }

}
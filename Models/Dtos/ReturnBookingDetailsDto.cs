namespace Models.Dtos
{
    public class ReturnBookingDetailsDto
    {        
        public int bookingId{get;set;}

        public string ?bookedBy{get;set;}

        public float price{get;set;}

        public bool isCanceled{get;set;}
        
        public SeatModel[] ?bookedSeats{get;set;}
    }
}
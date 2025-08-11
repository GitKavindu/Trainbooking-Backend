namespace Models.Dtos
{
    public class ReturnBookingDetailsDto
    {        
        public int bookingId{get;set;}

        public string ?bookedBy{get;set;}

        public string ?trainName{get;set;}

        public float price{get;set;}

        public bool isCanceled{get;set;}
        
        public SeatModel[] ?bookedSeats{get;set;}

        private DateTime _bookingdate;
        public DateTime bookingDateTime
        {
            set
            {
                _bookingdate=value;
            }
        }

        public string bookingDate
        {
            get
            {
                return _bookingdate.ToString("yyyy-MM-dd");
            }
        }

        public string bookingTime
        {
            get
            {
                return _bookingdate.ToString("hh:mm tt");
            }
        }
    }
}
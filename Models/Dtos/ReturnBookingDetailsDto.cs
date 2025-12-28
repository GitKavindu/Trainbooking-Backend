namespace Models.Dtos
{
    public class ReturnBookingDetailsDto
    {        
        public int bookingId{get;set;}

        public string ?bookedBy{get;set;}

        public string ?scheduleId{get;set;}

        public string ?trainName{get;set;}

        public int trainNo{get;set;}
        public int trainSeqNo{get;set;}

        public int fromStationNo{get;set;}
        public int fromStationSeqNo{get;set;}
        public string ?fromStation{get;set;}
        
        public int toStationNo{get;set;}
        public int toStationSeqNo{get;set;}
        public string ?toStation{get;set;}

        private decimal _price;
        public decimal price{
            get
            {
                return _price;
            }
            set
            {
                _price=Math.Round(value, 2);
            }
        }

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
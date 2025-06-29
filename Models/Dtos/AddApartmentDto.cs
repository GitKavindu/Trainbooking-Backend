namespace Models.Dtos
{
    public class AddApartmentDto
    {
        public int apartment_id{get;set;}
        public string ?_class{get;set;}
        public int train_id{get;set;}
        public int train_seq_no{get;set;}
        
        public string? tokenId{get;set;}

        public SeatModel []?seatModel{get;set;}
    }
}
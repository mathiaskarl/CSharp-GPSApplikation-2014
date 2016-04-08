namespace TuristApp5akaTheFinalCut.Model
{
    public class Event : Location
    {
        public string OpeningHours { get; set; }
        public string Phone { get; set; }
        public string Program { get; set; }
        public string TicketPrice { get; set; }
        public string Timeframe { get; set; }

        public Event() { }

        public Event(string openingHours, string phone, string program, string ticketPrice, string timeframe)
        {
            OpeningHours = openingHours;
            Phone = phone;
            Program = program;
            TicketPrice = ticketPrice;
            Timeframe = timeframe;
        }
    }
}

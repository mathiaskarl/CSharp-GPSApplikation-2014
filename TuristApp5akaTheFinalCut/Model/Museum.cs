using Windows.Foundation;

namespace TuristApp5akaTheFinalCut.Model
{
    public class Museum : Location
    {
        public string OpeningHours { get; set; }
        public string Phone { get; set; }
        public string TicketPrice { get; set; }
        public Museum() {}
        public Museum(string adress, string audio, string description, Point thisLocation, string name, string picture, string icon, string infobox, string openingHours, string phone, string ticketPrice)
            : base(adress, audio, description, thisLocation, name, picture, icon, infobox)
        {
            OpeningHours = openingHours;
            Phone = phone;
            TicketPrice = ticketPrice;
        }
        
    }
}

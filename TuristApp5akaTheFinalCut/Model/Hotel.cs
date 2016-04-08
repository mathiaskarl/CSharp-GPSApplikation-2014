using Windows.Foundation;

namespace TuristApp5akaTheFinalCut.Model
{
    public class Hotel : Location
    {
        public string OpeningHours { get; set; }
        public string Phone { get; set; }
        public string PriceRange { get; set; }
        public Hotel() {}
        public Hotel(string adress, string audio, string description, Point thisLocation, string name, string picture, string icon, string infobox, string openingHours, string phone, string priceRange) : base(adress, audio, description, thisLocation, name, picture, icon, infobox)
        {
            OpeningHours = openingHours;
            Phone = phone;
            PriceRange = priceRange;
        }
        
    }
}

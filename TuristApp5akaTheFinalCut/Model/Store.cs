using Windows.Foundation;

namespace TuristApp5akaTheFinalCut.Model
{
    public class Store : Location
    {
        public string OpeningHours { get; set; }
        public string Phone { get; set; }
        public Store() {}
        public Store(string adress, string audio, string description, Point thisLocation, string name, string picture, string icon, string infobox, string openingHours, string phone)
            : base(adress, audio, description, thisLocation, name, picture, icon, infobox)
        {
            OpeningHours = openingHours;
            Phone = phone;
        }
        
    }
}

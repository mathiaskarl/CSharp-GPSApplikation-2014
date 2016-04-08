using Windows.Foundation;

namespace TuristApp5akaTheFinalCut.Model
{
    public class Location
    {
        public virtual string Adress { get; set; }
        public virtual string Audio { get; set; }
        public virtual string Description { get; set; }
        public virtual Point ThisLocation { get; set; }
        public virtual string Name { get; set; }
        public virtual string Picture { get; set; }
        public virtual string Icon { get; set; }
        public virtual string Infobox { get; set; }
        public Location() {}

        public Location(string adress, string audio, string description, Point thisLocation, string name, string picture, string icon, string infobox)
        {
            Adress = adress;
            Audio = audio;
            Description = description;
            ThisLocation = thisLocation;
            Name = name;
            Picture = picture;
            Icon = icon;
            Infobox = infobox;
        }
    }
}

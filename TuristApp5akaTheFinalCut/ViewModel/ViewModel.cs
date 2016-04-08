using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using TuristApp5akaTheFinalCut.Common;
using TuristApp5akaTheFinalCut.Model;
using TuristApp5akaTheFinalCut.Model.Handlers;

namespace TuristApp5akaTheFinalCut.ViewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
        //PRIVATE VARIABLER
        private object _currentLocation;
        private Mp3Player _mp3Player;

        //Variabler for forsiden
        private ObservableCollection<Event> _events = new ObservableCollection<Event>();
        private ObservableCollection<Hotel> _hotels = new ObservableCollection<Hotel>();
        private ObservableCollection<Location> _locations = new ObservableCollection<Location>();
        private ObservableCollection<Museum> _museums = new ObservableCollection<Museum>();
        private ObservableCollection<Store> _stores = new ObservableCollection<Store>();
        private ObservableCollection<object> _favourites = new ObservableCollection<object>();
        private ObservableCollection<TextBlock> _categories = new ObservableCollection<TextBlock>();
        private ObservableCollection<Object> _locationsInMenu = new ObservableCollection<Object>();
        private ObservableCollection<object> _allLocations = new ObservableCollection<object>();
        private TextBlock _selectedCategory = null;

        //Variabler for alle locations
        private string _adress;
        private string _audio;
        private string _description;
        private Point _thisLocation;
        private string _name;
        private string _picture;
        private string _icon;
        private string _infobox;

        //Specifikke variabler
        private string _openingHours;
        private string _phone;
        private string _ticketPrice;
        private string _program;
        private string _timeFrame;
        private string _priceRange;

        //Handlers
        private FavouriteHandler _favouriteHandler = new FavouriteHandler();

        //PUBLIC VARIABLER
        public object CurrentLocation { get { return _currentLocation; } set { _currentLocation = value; OnPropertyChanged("CurrentLocation"); } }
        public TextBlock SelectedCategory { get { return _selectedCategory; } set { _selectedCategory = value; OnPropertyChanged("SelectedCategory"); LocationList(_selectedCategory); } }
        public ObservableCollection<object> AllLocations { get { return _allLocations; } set { _allLocations = value; OnPropertyChanged("AllLocations"); } }
        public ObservableCollection<object> LocationsInMenu { get { return _locationsInMenu; } set { _locationsInMenu = value; OnPropertyChanged("LocationsInMenu"); } }
        public ObservableCollection<TextBlock> Categories { get { return _categories; } set { _categories = value; OnPropertyChanged("Categories"); } }
        public ObservableCollection<object> Favourites { get { return _favourites; } set { _favourites = value; OnPropertyChanged("Favourites"); } }
        public ObservableCollection<Event> Events { get { return _events; } set { _events = value; OnPropertyChanged("Events"); } }
        public ObservableCollection<Hotel> Hotels { get { return _hotels; } set { _hotels = value; OnPropertyChanged("Hotels"); } }
        public ObservableCollection<Location> Locations { get { return _locations; } set { _locations = value; OnPropertyChanged("Locations"); } }
        public ObservableCollection<Museum> Museums { get { return _museums; } set { _museums = value; OnPropertyChanged("Museums"); } }
        public ObservableCollection<Store> Stores { get { return _stores; } set { _stores = value; OnPropertyChanged("Stores"); } }
        public string Adress { get { return _adress; } set { _adress = value; OnPropertyChanged("Adress"); } }
        public string Audio { get { return _audio; } set { _audio = value; OnPropertyChanged("Audio"); } }
        public string Description { get { return _description; } set { _description = value; OnPropertyChanged("Description"); } }
        public Point ThisLocation { get { return _thisLocation; } set { _thisLocation = value; OnPropertyChanged("ThisLocation"); } }
        public string Name { get { return _name; } set { _name = value; OnPropertyChanged("Name"); } }
        public string Picture { get { return _picture; } set { _picture = value; OnPropertyChanged("Picture"); } }
        public string Icon { get { return _icon; } set { _icon = value; OnPropertyChanged("Icon"); } }
        public string Infobox { get { return _infobox; } set { _infobox = value; OnPropertyChanged("Infobox"); } }
        public string OpeningHours { get { return _openingHours; } set { _openingHours = value; OnPropertyChanged("OpeningHours"); } }
        public string Phone { get { return _phone; } set { _phone = value; OnPropertyChanged("Phone"); } }
        public string TicketPrice { get { return _ticketPrice; } set { _ticketPrice = value; OnPropertyChanged("TicketPrice"); } }
        public string Program { get { return _program; } set { _program = value; OnPropertyChanged("Program"); } }
        public string TimeFrame { get { return _timeFrame; } set { _timeFrame = value; OnPropertyChanged("TimeFrame"); } }
        public string PriceRange { get { return _priceRange; } set { _priceRange = value; OnPropertyChanged("PriceRange"); } }

        //Constructor
        public ViewModel()
        {
            InitLists();
        }



        //ICommands
        public ICommand FavouriteCommand { get { RelayCommand _favrelay = new RelayCommand(AddOrRemoveFavourite); return _favrelay; } }
        public ICommand AudioCommand { get { RelayCommand _audrelay = new RelayCommand(PlayAudio); return _audrelay; } }



        //Metoder
        private void InitLists()
        {
            InitCategories();
            InitAttractions();
            InitHotels();
            InitStores();
        }

        private void LocationList(TextBlock category)
        {
            string key = category.Text;
            switch (key)
            {
                case "Events":
                    var item2 = new TextBlock();
                    item2.Text = "Events";
                    LocationsInMenu = new ObservableCollection<object>(Events);
                    break;
                case "Hotels":
                    LocationsInMenu = new ObservableCollection<object>(Hotels);
                    break;
                case "Attractions":
                    LocationsInMenu = new ObservableCollection<object>(Locations);
                    break;
                case "Museums":
                    LocationsInMenu = new ObservableCollection<object>(Museums);
                    break;
                case "Stores":
                    LocationsInMenu = new ObservableCollection<object>(Stores);
                    break;
                default:
                    break;
            }
        }

        private void AddOrRemoveFavourite()
        {
            if (_favouriteHandler.FavLocations.Contains(_currentLocation))
            {
                _favouriteHandler.AddFavourite(_currentLocation);
            }
            else
            {
                _favouriteHandler.RemoveFavourite(_currentLocation);
            }
            Favourites = new ObservableCollection<object>(_favouriteHandler.FavLocations);
        }
        private void PlayAudio()
        {
            _mp3Player = new Mp3Player(_audio);
            _mp3Player.Play();
        }

        #region Data
        private void InitAttractions()
        {
            _locations = new ObservableCollection<Location>()
            {
                new Location("Maglekildevej 5", "Audiopath",
                "Maglekilde is the most powerful spring in Roskilde. It produces 15.000 liters of water an hour. ",
                new Point(785, 478), "Maglekilde", "Picturepath", "maglekilde1.png", "InfoBox/kilde_info.png"),
                new Location("Sankt Peders Stræde 8 ", "Audiopath", "The monastery is a cultural gem right in the heart of Roskilde , just 100 meters behind the bustling main street and five minutes from the DomKirke . The unique complex of buildings and the large garden is a time warp where the middle of the city's hectic everyday life is a unique tranquility and historical atmosphere.", new Point(2433,941), "Roskilde Kloster", "InfoBox/Picturepath","kloster4.png","kloster_info.png"),
                new Location("Domkirkepladsen 3 ", "Audiopath", "Roskilde Cathedral wasn’t based on stone at first. It is said that one of Denmark’s most treasured heroes of all time Harald Blåtand back in the 980’s built the church based on wood. In 1020 Roskilde was announced as episcopate, which led to Roskilde Church became a Cathedral as we know this day today.", new Point(1370,645), "Roskilde Domkirke Museum", "Picturepath","Domkirke.png","InfoBox/kunst_info.png"),
                new Location("Fruegade 2 ", "Audiopath", "”Church of Our Lady” is a one of a kinde building, one of the only preserved travertine-Basilica churches left in denmark. A Basilica is a church with many nave's, where the middle nave is higher than the aisle's, and where theres a open connection between the nave's.", new Point(1645,1765), "Vor frue Kirke", "Picturepath","vor_frue_kirke.png","InfoBox/vor_frue_kirke_info.png"),

            };
            foreach (var obj in _locations)
                _allLocations.Add(obj);
        }


        private void InitHotels()
        {
            _hotels = new ObservableCollection<Hotel>()
            {
                new Hotel("Algade 13", "Audiopath", "Hotel prince in the royal city of Roskilde has more than 300 years of tradition of welcoming guests . Attention and care is a hallmark of our many employees - from the first contact with the guest until the guest leaves the house again.", new Point(2060,976), "Hotel Prindsen", "Picturepath","prindsen.png","InfoBox/hotel.png","6AM to 11PM", "+45 12345678", "1000 - 10000 DKK"),
                new Hotel("Lille Højbrøndsstræde 14", "Audiopath", "Centrally located houseing in quiet neighborhood less than 2 min  from the pedestrian street and the Roskilde Cathedral . Apartment with own kitchen and balcony, rooms with access to kitchen and garden . Has Wireless Internet access.", new Point(702,664), "B&B Roskilde City", "Picturepath","bedAndBreakfast.png","InfoBox/hotel.png","5AM to 11PM", "+45 83749283", "500 - 5000 DKK"),  
            };
            foreach (var obj in _hotels)
                _allLocations.Add(obj);
        }

        private void InitMuseums()
        {
            _museums = new ObservableCollection<Museum>()
            {

            };
            foreach (var obj in _museums)
                _allLocations.Add(obj);
        }

        private void InitStores()
        {
            _stores = new ObservableCollection<Store>()
            {
                           new Store("Algade 43 D", "Audiopath", "designer clothing, brand names,...", new Point(2546,1130), "Bestseller", "Picturepath","Bestseller.png","InfoBox/bank.png","8AM to 5PM","+45 23484734"),
                           new Store("Algade 24", "Audiopath", "Innovation is an ingrained part of our corporate culture. That is why it reads under our logo: Make a difference. This means that we try to make a positive difference every time our customers are in contact with us , in our consulting , design , behavior , communication, etc.", new Point(2300,1162), "Jyske Bank", "Picturepath","jyske_bank.png","InfoBox/bank.png","8 AM to 4 PM", "+45 38479385"),
                           new Store("Algade 16", "Audiopath", "Nordea's family tree includes approximately 300 banks established from the 1820s onwards. Through mergers number decreased to 80 banks in the 1970s and 30 banks in the 1980s. In the 1990s there were four major banks back to form the new banking group : These banks merged , and all activities have since December 2001 been operated under the name Nordea.", new Point(2030,1212), "Nordea", "Picturepath","Nodea.png","InfoBox/bank.png","8 AM to 4 PM", "+45 03030303"),
                           new Store("Algade 38", "Audiopath", "Zizzi - Scandinavia's largest and fastest growing fashion chain for curvy women.", new Point(2431,1259), "Zizzi", "Picturepath","zizzi.png","InfoBox/shopping.png","9AM to 5PM", "+45 84758375"),
                           new Store("Skomagergade 14, 4000 Roskilde", "Audiopath", "Classic , delicious , raw , timeless and yet different! Embark on an epic journey through the exciting collection material from the store brands and retrieve input and ideas for your style.", new Point(1354,1261), "Aage Müller", "Picturepath","aager_muller.png","InfoBox/shopping.png", "9AM to 5PM", "+45 28374858"),
                           new Store("Skomagergade 22", "Audiopath", "Vero Moda is a member of Roskilde stjernebutiker (star shops), with more than 60 shops, working together to give you the best shopping experience possible when you visit roskilde. Be it the well qualified service or entertaining window shpping.", new Point(1201,1132), "Vero Moda", "Picturepath","vero_moda.png","InfoBox/shopping.png","9AM to 5PM", "+45 93857684"),
                           new Store("Hestetorvet 1 C", "Audiopath", "Sultan Castle is a place for those who wish to be relieved from their stress. The whole concept of Sultan Castle is to experience a middle-eastern atmosphere while smoking an authentic water pipe.  While you’re there you’d get to choose between many different flavors which ensures you that you’d find a taste that suits you to the fullest.", new Point(3008,1384), "Sultan Castle", "Picturepath","sultan_castle.png","InfoBox/cafe.png","8AM to 6PM", "+45 8476594"),
                           new Store("Hestetorvet 7", "Audiopath", "Need to relax for a bit after touring Roskilde? Visit Kaffekilden to get Roskilde’s best coffee while you enjoy a piece of cake. Of course they serve cold drinks too for those of you who’s not in the mood for hot beverages. They can ensure you a joyful time with coffee of great quality from Ethiopia.", new Point(2885,1866), "Kaffekilden", "Picturepath","kaffekilden.png","InfoBox/cafe.png","10AM to 10PM","+45 39275837"),
                           new Store("Borchsgade 6", "Audiopath", "Enjoy a lovely beverage in the center of Roskilde while you sit back and enjoy live music’", new Point(1518,1397), "Gulland", "Picturepath","Gulland.png","InfoBox/cafe.png","10AM to 10PM", "+45 29485748"),
                           new Store("Stændertorvet 8", "Audiopath", "Café Vivaldi is a good excuse for a break no matter if you’re shopping or sightseeing. Vivaldi lies just in front of the Cathedral which ensures a great view in the summer time when you’re sitting out front. Vivaldi gives you the opportunity to consume great food and for a reasonable price or just a cup of coffee to refresh your spirit before you journey on.", new Point(1046,1248), "Vivaldi", "Picturepath","vivaldi.png","InfoBox/cafe.png","10AM to 8PM","+45 30294873"),
                           new Store("Ringstedgade 30", "Audiopath", "Interested in beer or wine? Come take a look at Bjergtroldens great collection of different beer and wine. Many of the beverages are homebrewed which ensures the guest to receive an experience which can’t be found anywhere else. To spice up the place they get visits from musicians who show their worth in the dining hall while you can sit in the bar and get a little tipsy.", new Point(750,1916), "Bjergtrolden", "Picturepath", "bjergtrolden.png", "InfoBox/cafe.png","10AM to 9PM", "+45 83927534"),
                           new Store("Fondens Bro 3", "Audiopath", "The main focus of this restaurant is great food delivered in a rustic environment. Though it might cost a bit more from time it’d be money well spent because of great service, food and stress releasing. Raadhuskælderen offer special offers depending on what time and day you come to visit. When you come by please ask the waitress what’s on their Menu of the Month or perhaps fish of the day to experience a journey of flavors interpreted in a Danish style.", new Point(1379,855), "Raadhuskælderen", "Picturepath", "Rådhuskælderen.png", "InfoBox/restaurant.png","9AM to 10PM","+45 97573853"),
                           new Store("Karen Olsdatters str 9", "Audiopath", "Mumm is a small exclusive restaurant restricted only to house 35 guests at a time or parties with 20 people. They started up the business in 2000 and has since then focused mainly on a quiet environment with local ingredients. The dishes varies in nationality between Northern, French and Spanish cousins or you could choose the 6 dish cousins where you let the cook experiment with the ingredients to make something new.", new Point(975,944), "Restuarant Mumm", "Picturepath", "mumm.png", "InfoBox/restaurant.png","12AM to 10PM","+45 302958474"),
                           new Store("Skomagergade 38", "Audiopath", "Jensens bøfhus, lies in the older part of the “gågade” in the heart of Roskilde. The location is superb, right smack in the middle of town, and well suited as a means of closure after a day well spent (window) shopping, visiting Domkirken or the vikingmuseem. Why not end the day, with a delicious, relaxing time dinning at Jensens bøfhus? We’ll welcome you with a wide variety of steak and other scrumptious temptations from our menu.", new Point(839,1175), "Jensens bøfhus", "Picturepath", "Jensens_bøfhus.png", "InfoBox/restaurant.png","11AM to 21PM", "+45 02958475"),
                           new Store("Hersegade 9", "Audiopath", "Resturant Håndværker lies at the heart of Roskilde, close by the train-station. Originally the building served as priesthouseing, but has since been redecorated to serve as a restaurant. The restaurant prides itself on its ability to serve gatherings of varying sizes, namely anywhere between 10 and upto 150 persons.", new Point(2187,1520), "Håndværkeren","Picturepath", "håndværkeren.png","Infobox/restaurant.png", "10AM to 10PM", "+45 03957385"),
                           new Store("Algade 55", "Audiopath", "Get a taste of old America. Resturant Bone’s in Roskilde boasts itself on haveing the worlds best spareribs, Denmarks largest salatbars and the towns best service. Its very famile friendly, and everyone is welcome here, even the type that prefer to eat spareribs with a knife and fork.", new Point(3034,1281), "Bone's", "Picturepath", "Bones.png", "Infobox/restaurant.png", "10AM to 10PM", "+45 94857465"),
                           new Store("Algade 65a", "Audiopath", "If you want to treat yourself to a healthy alternative to fastfood, then you should try the sushi from Yoshi Sushi. Yoshi Sushi is a sushi take-away restaurant located in Roskilde, where you can order fresh sushi of some of the highest quality in the world. It’s a treat for your tastebuds, if your in the mood for some fresh fish, rice and traditionelle sushi dishes.", new Point(3261,1235), "Sushi Yoshi", "Picturepath", "Sushi_Yoshi.png", "Infobox/restaurant.png","10AM to 10PM", "+45 83958675"),
            };
            foreach(var obj in _stores)
                _allLocations.Add(obj);
        }

        private void InitCategories()
        {
            var item1 = new TextBlock();
            item1.Text = "Attractions";
            var item2 = new TextBlock();
            item2.Text = "Events";
            var item3 = new TextBlock();
            item3.Text = "Hotels";
            var item4 = new TextBlock();
            item4.Text = "Museums";
            var item5 = new TextBlock();
            item5.Text = "Stores";
            _categories.Add(item1);
            _categories.Add(item2);
            _categories.Add(item3);
            _categories.Add(item4);
            _categories.Add(item5);
        }
        #endregion



        //INotify-junk
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

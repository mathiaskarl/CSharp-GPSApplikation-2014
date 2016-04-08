using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
using Tracker.Model;
using TuristApp5akaTheFinalCut.Model.Route;

namespace TuristApp5akaTheFinalCut.Model.Handlers
{
    class MapHandler
    {
        //Variabler
        public ObservableCollection<object> Locations = new ObservableCollection<object>();
        public Dictionary<object, Image> LocationButtons = new Dictionary<object, Image>();
        public List<Node> NodeList = new List<Node>();
        public Point CurrentPoint;
        public Canvas MyCanvas = new Canvas();
        
        //Canvas højde og bredde til brug ved navigation (Assignes i code-behind, når Canvas er loadet)
        public RouteHandler _routeHandler;
        private Image _mapImage = new Image();
        private Image _latestImage = new Image();
        private double _stackWidth;
        private double _stackHeight;
        private double _buttonWidth = 200;
        private double _buttonHeight = 100;
        private Point _newPoint;
        private DispatcherTimer _animTimer = new DispatcherTimer();
        private bool _secondPress = false;
        
        //Constructor
        public MapHandler(Canvas canvas, IEnumerable<object> locations, Image map, List<Node> NodeLists = null, List<Edge> EdgeLists = null)
        { 
            CurrentPoint = new Point(0,0);
            _mapImage = map;
            Locations = new ObservableCollection<object>(locations);
            MyCanvas = canvas;
            _routeHandler = new RouteHandler(NodeLists, EdgeLists);
            NodeList = NodeLists;
            _animTimer.Interval = new TimeSpan(0, 0, 0, 0, 2);
            _animTimer.Tick += _animTimer_Tick;
            
        }

        //Skab et kort baseret på et billede og en liste af locations
        public void CreateMap()
        {
            MyCanvas.Children.Add(_mapImage);
            foreach (object location in Locations)
            {
                
                Image tempImage = new Image();
                string imagePath = location.GetType().GetRuntimeProperty("Icon").GetValue(location).ToString();
                tempImage.Source = new BitmapImage(new Uri("ms-appx://TuristApp5akaTheFinalCut/Assets/Lokationer/" + imagePath));
                tempImage.PointerPressed += SetVisibility;
                tempImage.Width = _buttonWidth;
                tempImage.Height = _buttonHeight;
                tempImage.DataContext = location;
                LocationButtons.Add(location, tempImage);
                Canvas.SetLeft(LocationButtons[location], ((Point)location.GetType().GetRuntimeProperty("ThisLocation").GetValue(location)).X);
                Canvas.SetTop(LocationButtons[location], ((Point)location.GetType().GetRuntimeProperty("ThisLocation").GetValue(location)).Y);
                MyCanvas.Children.Add(LocationButtons[location]);
            }

            _stackHeight = MyCanvas.ActualHeight;
            _stackWidth = MyCanvas.ActualWidth;
        }

        public void CreateRoute(Point point1, Point point2)
        {
            _routeHandler.EdgePoint(point1, point2);
            _routeHandler.trackPoint = null;
            foreach (var obj in MyCanvas.Children.OfType<Ellipse>().ToList())
                MyCanvas.Children.Remove(obj);
            foreach (var obj in MyCanvas.Children.OfType<Polyline>().ToList())
                MyCanvas.Children.Remove(obj);

            foreach (Polyline obj in _routeHandler.AddRoute(NodeList.Count+1, NodeList.Count+2))
            {
                MyCanvas.Children.Add(obj);
            }
            
        }

        public Point CheckMouseCoordinates(Point mousePoint, double zoomFactor, Canvas canvas, StackPanel stackpanel)
        {
            Point tempPoint = new Point();
            if (mousePoint.X > 0)
            {
                tempPoint.X = 0;
            }
            else if (mousePoint.X < -_mapImage.ActualWidth + (stackpanel.ActualWidth / zoomFactor))
            {
                tempPoint.X = -_mapImage.ActualWidth + (stackpanel.ActualWidth / zoomFactor);
            }
            else
            {
                tempPoint.X = mousePoint.X;
            }

            if (mousePoint.Y > 0)
            {
                tempPoint.Y = 0;
            }
            else if (mousePoint.Y < -_mapImage.ActualHeight + (stackpanel.ActualHeight / zoomFactor))
            {
                tempPoint.Y = -_mapImage.ActualHeight + (stackpanel.ActualHeight / zoomFactor);
            }
            else
            {
                tempPoint.Y = mousePoint.Y;
            }
            CurrentPoint = tempPoint;
            return tempPoint;
        }

        private void SetVisibility(object sender, RoutedEventArgs e)
        {
            _secondPress = false;
            Image image = sender as Image;
            if (image == null) return;
            //if (image == _latestImage)
            //{
            //    _secondPress = true;
            //}
            //else
            //{
                foreach (Location key in LocationButtons.Keys)
                {
                    if (LocationButtons[key] == image)
                    {
                        image.Source = new BitmapImage(
                    new Uri("ms-appx://TuristApp5akaTheFinalCut/Assets/Lokationer/"+key.Infobox));
                    }
                    else
                    {
                        LocationButtons[key].Source = new BitmapImage(
                    new Uri("ms-appx://TuristApp5akaTheFinalCut/Assets/Lokationer/" + key.Icon));
                    }
                }
                _secondPress = false;
                _latestImage = image;
            //}
        }

        public void SetVisibility(object location)
        {
            if (location == null)
            {
                return;
            }

            Image image = new Image();
            foreach (Location key in LocationButtons.Keys.Where(key => key == location))
            {
                image = LocationButtons[key];
            }

            foreach (Location key in LocationButtons.Keys)
            {
                if (LocationButtons[key] == image)
                {
                    image.Source = new BitmapImage(
                new Uri("ms-appx://TuristApp5akaTheFinalCut/Assets/Lokationer/" + key.Infobox));
                }
                else
                {
                    LocationButtons[key].Source = new BitmapImage(
                new Uri("ms-appx://TuristApp5akaTheFinalCut/Assets/Lokationer/" + key.Icon));
                }
            }
            _latestImage = image;
        }

        public bool DisplayLocation()
        {
            return _secondPress;
        }

        //Navigerer til et udvalgt punkt på kortet, ud fra en Location
        public void NavigateMap(object location, double zoomFactor)
        {
            Point Location = (Point) location.GetType().GetRuntimeProperty("ThisLocation").GetValue(location);
            double xPos = CalculateCoordinate((Location.X * zoomFactor) - (_stackWidth / 2 - _buttonWidth / 2), (_mapImage.ActualWidth) - _stackWidth);
            double yPos = CalculateCoordinate((Location.Y * zoomFactor) - ((App.Framee.ActualHeight-180) / 2 - _buttonHeight / 2), (_mapImage.ActualHeight) - (App.Framee.ActualHeight - 180));
            _newPoint = new Point(xPos, yPos);
            _animTimer.Start();
        }

        void _animTimer_Tick(object sender, object e)
        {
            if (CurrentPoint.X > _newPoint.X + 20 || CurrentPoint.X < _newPoint.X - 20 || CurrentPoint.Y > _newPoint.Y + 20 || CurrentPoint.Y < _newPoint.Y - 20)
            {
                AnimateElement(_newPoint);
            }
            else
            {
                _animTimer.Stop();
            }
        }

        //Tjek om et koordinat er inden for de acceptable grænser
        private double CalculateCoordinate(double input, double valueToCompareWith)
        {
            double result;
            if (valueToCompareWith < input)
            {
                result = valueToCompareWith;
            }
            else if (input <= 0)
            {
                result = 0;
            }
            else
            {
                result = input;
            }
            return -result;
        }

        //Animation af kortet samt de knapper der repræsenterer locations
        private void AnimateElement(Point to)
        {
            if (CurrentPoint.X < to.X)
            {
                CurrentPoint.X += 10;
            }
            else if (CurrentPoint.X > to.X)
            {
                CurrentPoint.X -= 10;
            }

            if (CurrentPoint.Y < to.Y)
            {
                CurrentPoint.Y += 10;
            }
            else if (CurrentPoint.Y > to.Y)
            {
                CurrentPoint.Y -= 10;
            }
            MyCanvas.Margin = new Thickness(CurrentPoint.X, CurrentPoint.Y, 0, 0);
        }
    }
}

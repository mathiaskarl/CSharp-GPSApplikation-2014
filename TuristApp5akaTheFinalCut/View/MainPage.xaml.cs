// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Windows.Foundation;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
using Tracker.Model;
using TuristApp5akaTheFinalCut.Common;
using TuristApp5akaTheFinalCut.Model;
using TuristApp5akaTheFinalCut.Model.Handlers;
using TuristApp5akaTheFinalCut.Model.Route;

namespace TuristApp5akaTheFinalCut.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private object _selectedItem = null;
        private bool _menuOpen = false;
        private bool _subMenuOpen = false;
        private bool _favouriteOpen = false;
        private MapHandler _mapHandler;
        private Image _mapImage = new Image();
        private ViewModel.ViewModel _viewModel = new ViewModel.ViewModel();

        public List<Node> NodeList = new List<Node>(); 
        public List<Edge> EdgeList = new List<Edge>(); 
        private PointerPoint _pointerPoint;
        private Point _firstPoint = new Point();

        private Point _routeStartPoint = new Point(0, 0);
        private Point _routeSecondPoint = new Point(0, 0);
        private bool _routeStartPointGenerated = false;
        private bool _routeButtonClicked = false;
        
        public MainPage()
        {
            this.InitializeComponent();
            Initiate();

        }

        public async void Initiate()
        {
            Canvas.SetZIndex(RouteText, 10);
            Canvas.SetZIndex(RouteGuide, 10);
            MyGrid.DataContext = _viewModel;
            XMLReader reader = new XMLReader();
            NodeList = new List<Node>((await reader.XMLToList(new Node())).OfType<Node>().ToList());
            EdgeList = new List<Edge>((await reader.XMLToList(new Edge())).OfType<Edge>().ToList());
            _mapImage.Source = new BitmapImage(new Uri("ms-appx://TuristApp5akaTheFinalCut/Assets/Kort/RoskildeKort.jpg"));
            _mapHandler = new MapHandler(MyCanvas, _viewModel.AllLocations, _mapImage, NodeList, EdgeList);
            
            InitMouseControls();
            _mapHandler.CreateMap();
            ZoomFunction.ZoomToFactor(1f);
        }

        //Initialiserer mus-controls
        private void InitMouseControls()
        {
            //Tag fat i billedet når der bliver trykket på museknappen
            _mapImage.PointerPressed += (ss, ee) =>
            {
                _firstPoint = ee.GetCurrentPoint(null).Position;
                _mapImage.CapturePointer(ee.Pointer);

                

                if (_routeButtonClicked)
                {
                    if (!_routeStartPointGenerated)
                    {
                        if (_routeStartPoint.X < 1 && _routeStartPoint.Y < 1)
                        {
                            _routeStartPointGenerated = false;
                            _routeStartPoint = ee.GetCurrentPoint(null).Position;
                        }
                        else
                        {
                            if (Math.Abs(_routeStartPoint.X - ee.GetCurrentPoint(null).Position.X) < 10 &&
                                Math.Abs(_routeStartPoint.Y - ee.GetCurrentPoint(null).Position.Y) < 10)
                            {
                                _routeStartPointGenerated = true;
                                _routeStartPoint = AdjustPointToScreen(ee.GetCurrentPoint(null).Position);
                                RouteFirstPoint.Visibility = Visibility.Collapsed;
                                RouteSecondPoint.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                _routeStartPointGenerated = false;
                                _routeStartPoint = ee.GetCurrentPoint(null).Position;
                            }
                        }

                    }
                    if (_routeStartPointGenerated)
                    {
                        if (_routeSecondPoint.X < 1 && _routeSecondPoint.Y < 1)
                        {
                            _routeSecondPoint = ee.GetCurrentPoint(null).Position;
                        }
                        else
                        {
                            if (Math.Abs(_routeSecondPoint.X - ee.GetCurrentPoint(null).Position.X) < 10 &&
                                Math.Abs(_routeSecondPoint.Y - ee.GetCurrentPoint(null).Position.Y) < 10)
                            {
                                RouteText.Visibility = Visibility.Collapsed;
                                _mapHandler.CreateRoute(_routeStartPoint,
                                    AdjustPointToScreen(_routeSecondPoint));
                                RouteGuide.Visibility = Visibility.Visible;
                                GuideDistance.Text = (_mapHandler._routeHandler.TotalDistance*8) + "m";
                                GuideArrival.Text = Math.Round((_mapHandler._routeHandler.TotalDistance*8)/80) + "min";
                                _routeButtonClicked = false;
                            }
                            else
                            {
                                _routeSecondPoint = ee.GetCurrentPoint(null).Position;
                            }
                        }
                    }
                }

            };


            //Giv slip på billedet når museknappen bliver sluppet
            _mapImage.PointerReleased += (ss, ee) =>
            {
                _mapImage.ReleasePointerCapture(ee.Pointer);
            };

            //Håndtering af billedets bevægelser udfra musens bevægelser
            _mapImage.PointerMoved += (ss, ee) =>
            {
                _pointerPoint = ee.GetCurrentPoint(this);
                if (_pointerPoint.Properties.IsLeftButtonPressed)
                {
                    Point temp = ee.GetCurrentPoint(null).Position;
                    Point res = new Point(_firstPoint.X - temp.X, _firstPoint.Y - temp.Y);
                    var currentMargin = MyCanvas.Margin;
                    Point canvasPoint =
                        _mapHandler.CheckMouseCoordinates(
                            new Point(currentMargin.Left - res.X, currentMargin.Top - res.Y),
                            ZoomFunction.ZoomFactor, MyCanvas, CanvasContainer);
                    MyCanvas.Margin = new Thickness(canvasPoint.X, canvasPoint.Y, 0, 0);
                    _firstPoint = temp;
                }
            };
        }

        private Point AdjustPointToScreen(Point point)
        {
            point.Y = (Math.Abs(MyCanvas.Margin.Top) + (point.Y -= 90)) / ZoomFunction.ZoomFactor;
            point.X = (Math.Abs(MyCanvas.Margin.Left) + point.X) / ZoomFunction.ZoomFactor;
            return point;
        }

        //Find lokation-button
        private void Button_FindLocation_Click(object sender, RoutedEventArgs e)
        {
            ToggleMenu();
        }

        //Ændring i hovedmenuen
        private void LocationMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectionChanged();
        }

        //Find ud af om brugeren prøver at åbne eller lukke menuerne, og ager derefter
        private void ToggleMenu()
        {
            if (_favouriteOpen)
            {
                MinimizeFavouriteAnimation.Begin();
                _favouriteOpen = false;
            }

            if (_subMenuOpen)
            {
                MinimizeHorizontalAnimation.Completed -= SelectionChangedAnimation;
                MinimizeHorizontalAnimation.Completed -= MinimizeFirstMenu;
                MinimizeHorizontalAnimation.Completed += MinimizeFirstMenu;
                MinimizeHorizontalAnimation.Begin();
                _subMenuOpen = false;
            }
            else if (_menuOpen)
            {
                MinimizeVerticalAnimation.Begin();
                _menuOpen = false;
            }
            else
            {
                ExpandVerticalAnimation.Begin();
                _menuOpen = true;
            }
        }


        //Registrerer ændringer i valgte menu-items og agerer derefter
        private void SelectionChanged()
        {
            _selectedItem = LocationMenu.SelectedItem;
            if (_subMenuOpen)
            {
                if (_selectedItem == null)
                {
                    MinimizeHorizontalAnimation.Completed -= SelectionChangedAnimation;
                    MinimizeHorizontalAnimation.Completed -= MinimizeFirstMenu;
                    _subMenuOpen = false;
                }
                else
                {
                    MinimizeHorizontalAnimation.Completed -= SelectionChangedAnimation;
                    MinimizeHorizontalAnimation.Completed -= MinimizeFirstMenu;
                    MinimizeHorizontalAnimation.Completed += SelectionChangedAnimation;
                    _subMenuOpen = true;
                }
                MinimizeHorizontalAnimation.Begin();

            }
            else
            {
                if (_selectedItem != null)
                {
                    MinimizeHorizontalAnimation.Completed -= SelectionChangedAnimation;
                    MinimizeHorizontalAnimation.Completed -= MinimizeFirstMenu;
                    ExpandHorizontalAnimation.Begin();
                    _subMenuOpen = true;
                }
                else
                {
                    MinimizeHorizontalAnimation.Completed -= SelectionChangedAnimation;
                    MinimizeHorizontalAnimation.Completed -= MinimizeFirstMenu;
                    _subMenuOpen = false;
                }
            }
        }


        //Animation.Completed-metode
        void MinimizeFirstMenu(object sender, object e)
        {
            MinimizeHorizontalAnimation.Completed -= SelectionChangedAnimation;
            MinimizeHorizontalAnimation.Completed -= MinimizeFirstMenu;
            MinimizeVerticalAnimation.Begin();
            _menuOpen = false;
        }


        //Animation.Completed-metode
        private void SelectionChangedAnimation(object sender, object e)
        {
            ExpandHorizontalAnimation.Begin();
        }

        //Luk Lokations-menuerne, når en lokation er valgt
        private void LocationSubMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LocationSubMenu.SelectedItem != null)
            {
                MinimizeHorizontalAnimation.Completed -= SelectionChangedAnimation;
                MinimizeHorizontalAnimation.Completed -= MinimizeFirstMenu;
                MinimizeHorizontalAnimation.Completed += MinimizeFirstMenu;
                MinimizeHorizontalAnimation.Begin();
                _subMenuOpen = false;
                //FEJL
                object location = _selectedItem.ToString();



                _mapHandler.NavigateMap(LocationSubMenu.SelectedItem, ZoomFunction.ZoomFactor);
                _mapHandler.SetVisibility(LocationSubMenu.SelectedItem);
            }
        }

        //Luk appen ned
        private void Button_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private void Button_Favourites_Click(object sender, RoutedEventArgs e)
        {
            if (_subMenuOpen)
            {
                MinimizeHorizontalAnimation.Completed -= SelectionChangedAnimation;
                MinimizeHorizontalAnimation.Completed -= MinimizeFirstMenu;
                MinimizeHorizontalAnimation.Completed += MinimizeFirstMenu;
                MinimizeHorizontalAnimation.Begin();
                _subMenuOpen = false;
            }
            else if (_menuOpen)
            {
                MinimizeVerticalAnimation.Begin();
                _menuOpen = false;
            }

            if (_favouriteOpen)
            {
                MinimizeFavouriteAnimation.Begin();
                _favouriteOpen = false;
            }
            else
            {
                ExpandFavouriteAnimation.Begin();
                _favouriteOpen = true;
            }
        }

        private void FavouriteMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedItem = FavouriteMenu.SelectedItem;
            if (_favouriteOpen && FavouriteMenu.SelectedItem != null)
            {

                MinimizeFavouriteAnimation.Begin();
                _favouriteOpen = false;
                Location location = (Location)_selectedItem;
                _mapHandler.NavigateMap(location, ZoomFunction.ZoomFactor);
                _mapHandler.SetVisibility(_selectedItem);

            }
        }

        private void Button_Reset_OnClick(object sender, RoutedEventArgs e)
        {
            MyCanvas.Margin = new Thickness(0, 0, 0, 0);
            foreach (var obj in MyCanvas.Children.OfType<Polyline>().ToList())
                MyCanvas.Children.Remove(obj);
            RouteGuide.Visibility = Visibility.Collapsed;
        }

        private void Button_FindRoute_OnClick(object sender, RoutedEventArgs e)
        {
            _routeButtonClicked = _routeButtonClicked ? false : true;
            RouteText.Visibility = _routeButtonClicked ? Visibility.Visible : Visibility.Collapsed;
            RouteFirstPoint.Visibility = Visibility.Visible;
            RouteSecondPoint.Visibility = Visibility.Collapsed;
            _routeStartPoint = new Point(0, 0);
            _routeSecondPoint = new Point(0, 0);
            _routeStartPointGenerated = false;
            RouteGuide.Visibility = Visibility.Collapsed;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using TuristApp5akaTheFinalCut.Model;
using TuristApp5akaTheFinalCut.ViewModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TuristApp5akaTheFinalCut.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LocationPage : Page
    {
        private ViewModel.ViewModel _viewModel = new ViewModel.ViewModel();
        private object _location = new object();
        public LocationPage()
        {
            this.InitializeComponent();
            _location = _viewModel.CurrentLocation;
            SetStackPanels(_location);

        }

        private void ButtonBack_OnClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void SetStackPanels(object location)
        {

            StackPanelEvent.Visibility = Visibility.Collapsed;
            StackPanelHotel.Visibility = Visibility.Collapsed;
            StackPanelLocation.Visibility = Visibility.Collapsed;
            StackPanelMuseum.Visibility = Visibility.Collapsed;
            StackPanelStore.Visibility = Visibility.Collapsed;
            if (location == null) return;
            if (location.GetType() == typeof(Event))
            {
                StackPanelEvent.Visibility = Visibility.Visible;
            }
            else if (location.GetType() == typeof(Hotel))
            {
                StackPanelHotel.Visibility = Visibility.Visible;
            }
            else if (location.GetType() == typeof(Location))
            {
                StackPanelLocation.Visibility = Visibility.Visible;
            }
            else if (location.GetType() == typeof(Museum))
            {
                StackPanelMuseum.Visibility = Visibility.Visible;
            }
            else if (location.GetType() == typeof(Store))
            {
                StackPanelStore.Visibility = Visibility.Visible;
            }

            
        }


    }
}

﻿

#pragma checksum "C:\Users\mathi_000\Desktop\TuristApp5akaTheFinalCut\TuristApp5akaTheFinalCut\View\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "8033A1BCB1F3FA8E703DE28DC7A04EA3"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TuristApp5akaTheFinalCut.View
{
    partial class MainPage : global::Windows.UI.Xaml.Controls.Page
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Media.Animation.Storyboard ExpandVerticalAnimation; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Media.Animation.Storyboard MinimizeVerticalAnimation; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Media.Animation.Storyboard ExpandHorizontalAnimation; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Media.Animation.Storyboard MinimizeHorizontalAnimation; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Media.Animation.Storyboard ExpandFavouriteAnimation; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Media.Animation.Storyboard MinimizeFavouriteAnimation; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Grid MyGrid; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.StackPanel RouteText; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.StackPanel RouteGuide; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.ScrollViewer ZoomFunction; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.ListView LocationMenu; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.ListView LocationSubMenu; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.ListView FavouriteMenu; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.AppBarButton Button_FindLocation; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.AppBarButton Button_FindRoute; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.AppBarButton Button_Favourites; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.AppBarButton Button_Reset; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.AppBarButton Button_Exit; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.StackPanel CanvasContainer; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Canvas MyCanvas; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Documents.Run GuideDistance; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Documents.Run GuideArrival; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.TextBlock RouteFirstPoint; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.TextBlock RouteSecondPoint; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private bool _contentLoaded;

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent()
        {
            if (_contentLoaded)
                return;

            _contentLoaded = true;
            global::Windows.UI.Xaml.Application.LoadComponent(this, new global::System.Uri("ms-appx:///View/MainPage.xaml"), global::Windows.UI.Xaml.Controls.Primitives.ComponentResourceLocation.Application);
 
            ExpandVerticalAnimation = (global::Windows.UI.Xaml.Media.Animation.Storyboard)this.FindName("ExpandVerticalAnimation");
            MinimizeVerticalAnimation = (global::Windows.UI.Xaml.Media.Animation.Storyboard)this.FindName("MinimizeVerticalAnimation");
            ExpandHorizontalAnimation = (global::Windows.UI.Xaml.Media.Animation.Storyboard)this.FindName("ExpandHorizontalAnimation");
            MinimizeHorizontalAnimation = (global::Windows.UI.Xaml.Media.Animation.Storyboard)this.FindName("MinimizeHorizontalAnimation");
            ExpandFavouriteAnimation = (global::Windows.UI.Xaml.Media.Animation.Storyboard)this.FindName("ExpandFavouriteAnimation");
            MinimizeFavouriteAnimation = (global::Windows.UI.Xaml.Media.Animation.Storyboard)this.FindName("MinimizeFavouriteAnimation");
            MyGrid = (global::Windows.UI.Xaml.Controls.Grid)this.FindName("MyGrid");
            RouteText = (global::Windows.UI.Xaml.Controls.StackPanel)this.FindName("RouteText");
            RouteGuide = (global::Windows.UI.Xaml.Controls.StackPanel)this.FindName("RouteGuide");
            ZoomFunction = (global::Windows.UI.Xaml.Controls.ScrollViewer)this.FindName("ZoomFunction");
            LocationMenu = (global::Windows.UI.Xaml.Controls.ListView)this.FindName("LocationMenu");
            LocationSubMenu = (global::Windows.UI.Xaml.Controls.ListView)this.FindName("LocationSubMenu");
            FavouriteMenu = (global::Windows.UI.Xaml.Controls.ListView)this.FindName("FavouriteMenu");
            Button_FindLocation = (global::Windows.UI.Xaml.Controls.AppBarButton)this.FindName("Button_FindLocation");
            Button_FindRoute = (global::Windows.UI.Xaml.Controls.AppBarButton)this.FindName("Button_FindRoute");
            Button_Favourites = (global::Windows.UI.Xaml.Controls.AppBarButton)this.FindName("Button_Favourites");
            Button_Reset = (global::Windows.UI.Xaml.Controls.AppBarButton)this.FindName("Button_Reset");
            Button_Exit = (global::Windows.UI.Xaml.Controls.AppBarButton)this.FindName("Button_Exit");
            CanvasContainer = (global::Windows.UI.Xaml.Controls.StackPanel)this.FindName("CanvasContainer");
            MyCanvas = (global::Windows.UI.Xaml.Controls.Canvas)this.FindName("MyCanvas");
            GuideDistance = (global::Windows.UI.Xaml.Documents.Run)this.FindName("GuideDistance");
            GuideArrival = (global::Windows.UI.Xaml.Documents.Run)this.FindName("GuideArrival");
            RouteFirstPoint = (global::Windows.UI.Xaml.Controls.TextBlock)this.FindName("RouteFirstPoint");
            RouteSecondPoint = (global::Windows.UI.Xaml.Controls.TextBlock)this.FindName("RouteSecondPoint");
        }
    }
}




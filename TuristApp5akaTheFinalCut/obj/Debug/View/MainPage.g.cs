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
    partial class MainPage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 88 "..\..\View\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.LocationMenu_SelectionChanged;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 89 "..\..\View\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.LocationSubMenu_SelectionChanged;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 96 "..\..\View\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.FavouriteMenu_SelectionChanged;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 99 "..\..\View\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Button_FindLocation_Click;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 100 "..\..\View\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Button_FindRoute_OnClick;
                 #line default
                 #line hidden
                break;
            case 6:
                #line 101 "..\..\View\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Button_Favourites_Click;
                 #line default
                 #line hidden
                break;
            case 7:
                #line 84 "..\..\View\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Button_Reset_OnClick;
                 #line default
                 #line hidden
                break;
            case 8:
                #line 85 "..\..\View\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Button_Exit_Click;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}



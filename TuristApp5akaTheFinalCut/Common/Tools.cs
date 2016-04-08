using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace TuristApp5akaTheFinalCut.Common
{
    class Tools
    {
        public static T FindVisualChild<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        return (T)child;
                    }

                    T childItem = FindVisualChild<T>(child);
                    if (childItem != null) return childItem;
                }
            }
            return null;
        }

        public static PointCollection ToPointCollection(IEnumerable<Point> objList)
        {
            PointCollection tempCollection = new PointCollection();
            foreach (Point obj in objList)
            {
                tempCollection.Add(obj);
            }
            return tempCollection;
        }
    }
}

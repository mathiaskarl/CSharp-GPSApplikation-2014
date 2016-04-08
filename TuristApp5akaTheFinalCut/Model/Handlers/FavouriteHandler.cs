using System;
using System.Collections.ObjectModel;

namespace TuristApp5akaTheFinalCut.Model.Handlers
{
    class FavouriteHandler
    {
        public ObservableCollection<Object> FavLocations = new ObservableCollection<Object>();

        public void AddFavourite(Object location)
        {
            if (!FavLocations.Contains(location))
            {
                FavLocations.Add(location);
            }
        }

        public void RemoveFavourite(Object location)
        {
            if (FavLocations.Contains(location))
            {
                FavLocations.Remove(location);
            }
        }
    }
}

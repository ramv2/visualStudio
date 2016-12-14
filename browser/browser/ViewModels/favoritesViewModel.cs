using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace browser
{
    public class favoritesViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Instantiates the favoritesViewModel class.
        /// </summary>
        public favoritesViewModel()
        {            
            favorites = new ObservableCollection<favoritesModel>();
            if (Properties.Settings.Default.Favorites != null)
            {
                populateFavorites(Properties.Settings.Default.Favorites);
            }            
        }

        private void populateFavorites(ArrayList a1)
        {
            favorites.Clear();
            for (int i=0; i<a1.Count; i++)
            {
                favoritesModel fav_i = new favoritesModel();
                string str = a1[i] as string;
                int index = str.IndexOf(';');
                fav_i.title = str.Substring(0, index);
                fav_i.url = str.Substring(index+1);
                favorites.Add(fav_i);
            }
        }
        
        public ObservableCollection<favoritesModel> favorites { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

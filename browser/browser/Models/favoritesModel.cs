using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace browser
{
    public class favoritesModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Instantiates the favoritesModel class
        /// </summary>
        public favoritesModel()
        {

        }

        string _title, _url;
        public string title
        {
            get { return _title; }
            set
            {
                if (_title == value)
                    return;
                _title = value;
                OnPropertyChanged("title");
            }
        }
        public string url
        {
            get { return _url; }
            set
            {
                if (_url == value)
                    return;
                _url = value;
                OnPropertyChanged("url");
            }
        }

        public override string ToString()
        {
            return title.Length > 15 ? title.Substring(0, 15) : title;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
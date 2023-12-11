using System.ComponentModel;

namespace SiloInventoryManagement.Model
{
    public class LocationModel : INotifyPropertyChanged
    {
        private int _locationID;
        private string _locationDescription;

        public LocationModel() 
        {
            LocationID = -1;
            LocationDescription = "";
        }
        public LocationModel(Location location)
        {
            LocationID = location.LocationID;
            LocationDescription = location.LocationDescription;
        }

        public int LocationID
        {
            get { return _locationID; }
            set
            {
                if (_locationID != value)
                {
                    _locationID = value;
                    OnPropertyChanged("LocationID");
                }
            }
        }
        public string LocationDescription
        {
            get { return _locationDescription; }
            set
            {
                if (_locationDescription != value)
                {
                    _locationDescription = value;
                    OnPropertyChanged("LocationDescription");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }
    }
}


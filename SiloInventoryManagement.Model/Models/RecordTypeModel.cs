using System.ComponentModel;

namespace SiloInventoryManagement.Model
{
    public class RecordTypeModel : INotifyPropertyChanged
    {
        private int _recordTypeID;
        private string _recordTypeDescription;

        public RecordTypeModel() 
        {
            RecordTypeID = -1;
            RecordTypeDescription = "";
        }
        public RecordTypeModel(RecordType recordType) 
        {
            RecordTypeID = recordType.RecordTypeID;
            RecordTypeDescription = recordType.RecordTypeDescription;
        }

        public int RecordTypeID
        {
            get { return _recordTypeID; }
            set
            {
                if (_recordTypeID != value)
                {
                    _recordTypeID = value;
                    OnPropertyChanged("RecordTypeID");
                }

            }
        }
        public string RecordTypeDescription
        {
            get { return _recordTypeDescription; }
            set
            {
                if (_recordTypeDescription != value)
                {
                    _recordTypeDescription = value;
                    OnPropertyChanged("RecordTypeDescription");
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

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SiloInventoryManagement.Model
{
    public class SiloInventoryModel : INotifyPropertyChanged
    {
        private int _siloInventoryID;
        private int _recordTypeID;
        private DateTime _date;
        private int _locationID;
        private decimal _value;
        private string _comment;
        private string _enteredBy;
        private DateTime _dateModified;

        public SiloInventoryModel() 
        {
            DateTime now = DateTime.Now;

            SiloInventoryID = -1;
            RecordTypeID = -1;
            LocationID = -1;
            Date = new DateTime(now.Year, now.Month, now.Day);
            Value = 0;
            Comment = "";
            EnteredBy = Environment.UserName;
            DateModified = Date = new DateTime(now.Year, now.Month, now.Day);
        }

        public SiloInventoryModel(SiloInventory inventory) 
        {
            SiloInventoryID = inventory.SiloInventoryID;
            RecordTypeID = inventory.RecordTypeID;
            LocationID = inventory.LocationID;
            Date = inventory.Date;
            Value = inventory.Value;
            Comment = inventory.Comment;
            EnteredBy = inventory.EnteredBy;
            DateModified = inventory.DateModified;
        }

        public int SiloInventoryID
        {
            get { return _siloInventoryID; }
            set
            {
                if (_siloInventoryID != value)
                {
                    _siloInventoryID = value;
                    OnPropertyChanged("SiloInventoryID");
                }

            }
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
        public System.DateTime Date
        {
            get { return _date; }
            set
            {
                if (_date != value)
                {
                    _date = value;
                    OnPropertyChanged("Date");
                }

            }
        }
        public decimal Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    OnPropertyChanged("Value");
                }

            }
        }
        public string Comment
        {
            get { return _comment; }
            set
            {
                if (_comment != value)
                {
                    _comment = value == null ? "" :  value.Trim();
                    OnPropertyChanged("Comment");
                }

            }
        }
        public string EnteredBy
        {
            get { return _enteredBy; }
            set
            {
                if (_enteredBy != value)
                {
                    _enteredBy = value.Trim();
                    OnPropertyChanged("EnteredBy");
                }

            }
        }
        public System.DateTime DateModified
        {
            get { return _dateModified; }
            set
            {
                if (_dateModified != value)
                {
                    _dateModified = value;
                    OnPropertyChanged("DateModified");
                }

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            //this.VerifyPropertyName(propertyName);

            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }
    }
}

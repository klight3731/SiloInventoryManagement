using SiloInventoryManagement.DAL;
using SiloInventoryManagement.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace SiloInventoryManagement.UI
{
    /// <summary>
    /// Interaction logic for frmEditSiloInventory.xaml
    /// </summary>
    public partial class frmEditSiloInventory : Window, INotifyPropertyChanged
    {
        private Cursor _windowCursor;
        private SiloInventoryModel _siloInventory;
        private ObservableCollection<RecordTypeModel> _recordTypes;
        private ObservableCollection<LocationModel> _locations;
        private SiloInventoryDBA _siloInventoryDBA;

        public RelayCommand OKCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        public frmEditSiloInventory()
        {
            InitializeComponent();
            DataContext = this;
        }

        public frmEditSiloInventory(SiloInventoryModel siloInventory
                                    , ObservableCollection<LocationModel> locations
                                    , ObservableCollection<RecordTypeModel> recordTypes)
        {
            InitializeComponent();
            DataContext = this;

            _siloInventoryDBA = new SiloInventoryDBA();
            OKCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);

            SiloInventory = new SiloInventoryModel()
            {
                Comment = siloInventory.Comment,
                Date = siloInventory.Date,
                DateModified = siloInventory.DateModified,
                EnteredBy = siloInventory.EnteredBy,
                LocationID = siloInventory.LocationID,
                RecordTypeID = siloInventory.RecordTypeID,
                SiloInventoryID = siloInventory.SiloInventoryID,
                Value = siloInventory.Value
            };

            Locations = locations;
            RecordTypes = recordTypes;
            WindowCursor = Cursors.Arrow;
        }
        public SiloInventoryModel SiloInventory
        {
            get { return _siloInventory; }
            set
            {
                _siloInventory = value;
                OnPropertyChanged("SiloInventory");
            }
        }
        public ObservableCollection<RecordTypeModel> RecordTypes
        {
            get { return _recordTypes; }
            set
            {
                _recordTypes = value;
                OnPropertyChanged("RecordTypes");
            }
        }

        public ObservableCollection<LocationModel> Locations
        {
            get { return _locations; }
            set
            {
                _locations = value;
                OnPropertyChanged("Locations");
            }
        }
        public Cursor WindowCursor
        {
            get { return _windowCursor; }
            set
            {
                _windowCursor = value;
                OnPropertyChanged("WindowCursor");
            }
        }

        public void Save(object arg)
        {
            try
            {
                WindowCursor = Cursors.Wait;



                //If this is an Adjustment then make sure the user typed in comments
                //We also need to make sure there's a daily entry before we can add an adjustment
                if (SiloInventory.RecordTypeID == 2)
                {
                    ObservableCollection<SiloInventoryModel> siloInventory = GetSiloInventoryData();

                    if (siloInventory.Count > 0)
                    {
                        bool found = false;

                        foreach (SiloInventoryModel s in siloInventory)
                        {
                            if (s.RecordTypeID == 1)
                            {
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                        {
                            System.Windows.MessageBox.Show(string.Format("Cannot create an adjustment until the daily silo inventory has been entered for {0}.", _siloInventory.Date.ToShortDateString()),
                            "Missing Daily Silo Inventory",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    else
                    {
                        System.Windows.MessageBox.Show(string.Format("Cannot create an adjustment until the daily silo inventory has been entered for {0}.", _siloInventory.Date.ToShortDateString()),
                            "Missing Daily Silo Inventory",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (SiloInventory.Comment.Trim().Length < 4)
                    {
                        System.Windows.MessageBox.Show("Please enter a comment for this adjustment",
                            "Invalid Comment",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }



                //Make sure you insert if the silo inventory id is -1
                if (SiloInventory.SiloInventoryID == -1)
                {
                    _siloInventoryDBA.Insert(_siloInventory);
                }
                else
                {
                    _siloInventoryDBA.Update(_siloInventory, Environment.UserName);
                }

                DialogResult = true;
            }
            catch (System.Exception ex)
            {
                ErrorHelper.LogAndDisplayError(ex);
            }
            finally
            {
                WindowCursor = Cursors.Arrow;
            }
        }

        public ObservableCollection<SiloInventoryModel> GetSiloInventoryData()
        {
            return _siloInventoryDBA.GetByDateRangeObservableCollection(_siloInventory.Date, _siloInventory.Date);
        }

        public void Cancel(object arg)
        {
            DialogResult = false;
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

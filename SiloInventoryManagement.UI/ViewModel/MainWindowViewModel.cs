using SiloInventoryManagement.DAL;
using SiloInventoryManagement.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace SiloInventoryManagement.UI.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public RelayCommand RefreshCommand { get; set; }
        public RelayCommand AddInventoryAdjustmentCommand { get; set; }
        public RelayCommand AddDailyInventoryCommand { get; set; }
        public RelayCommand EditInventoryCommand { get; set; }

        private BackgroundWorker _worker;
        private BackgroundWorker _getReferenceTables;
        private BackgroundWorker _getTodaysSiloInventory;

        private ObservableCollection<SiloInventoryModel> _siloInventory;
        private DateTime _startDate;
        private DateTime _endDate;
        private ObservableCollection<RecordTypeModel> _recordTypes;
        private ObservableCollection<LocationModel> _locations;
        private SiloInventoryDBA _siloInventoryDBA;
        private LocationsDBA _locationsDBA;
        private RecordTypesDBA _recordsDBA;
        private Cursor _windowCursor;
        private Visibility _showMissingSiloWarning;
        private string _missingSiloWarningMessage;

        public MainWindowViewModel() 
        {
            DateTime now = DateTime.Now;

            _startDate = new DateTime(now.AddDays(-7).Year, now.AddDays(-7).Month, now.AddDays(-7).Day);
            _endDate = new DateTime(now.Year, now.Month, now.Day);
            _showMissingSiloWarning = Visibility.Hidden;
            _missingSiloWarningMessage = "";

            RefreshCommand = new RelayCommand(RefreshSiloInventoryData);
            AddInventoryAdjustmentCommand = new RelayCommand(AddInventoryAdjustment);
            AddDailyInventoryCommand = new RelayCommand(AddDailyInventory);
            EditInventoryCommand = new RelayCommand(EditInventory);

            _siloInventoryDBA = new SiloInventoryDBA();
            _locationsDBA = new LocationsDBA();
            _recordsDBA = new RecordTypesDBA();

            _siloInventory = new ObservableCollection<SiloInventoryModel>();
            _locations = new ObservableCollection<LocationModel>();
            _recordTypes = new ObservableCollection<RecordTypeModel>();

            OnPropertyChanged("StartDate");
            OnPropertyChanged("EndDate");
            OnPropertyChanged("ShowMissingSiloWarning");
            OnPropertyChanged("MissingSiloWarningMessage");

            _worker = new BackgroundWorker();
            _worker.DoWork += GetSiloInventoryByDateRange;
            _worker.RunWorkerCompleted += GetSiloInventoryByDateRange_RunWorkerCompleted;

            _getReferenceTables = new BackgroundWorker();
            _getReferenceTables.DoWork += GetReferenceTables_DoWork;
            _getReferenceTables.RunWorkerCompleted += GetReferenceTables_RunWorkerCompleted;

            _getTodaysSiloInventory = new BackgroundWorker();
            _getTodaysSiloInventory.DoWork += GetTodaysSiloInventory_DoWork;
            _getTodaysSiloInventory.RunWorkerCompleted += GetTodaysSiloInventory_RunWorkerCompleted;

            _getReferenceTables.RunWorkerAsync();
        }

        private void GetTodaysSiloInventory_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    ErrorHelper.DisplayError(e.Error);
                }
            }
            finally
            {
                WindowCursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// Lets pull todays silo inventory data to see if it exists.  If not show the warning message.
        /// If it does hide the warning message.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetTodaysSiloInventory_DoWork(object sender, DoWorkEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                WindowCursor = Cursors.Wait;
            }));

            ObservableCollection<SiloInventoryModel> siloInventory = new ObservableCollection<SiloInventoryModel>();

            DateTime now = DateTime.Now;    

            siloInventory = _siloInventoryDBA.GetByDateRangeObservableCollection(new DateTime(now.Year, now.Month, now.Day), new DateTime(now.Year, now.Month, now.Day));

            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (siloInventory.Count == 0)
                {
                    ShowMissingSiloWarning = Visibility.Visible;
                    MissingSiloWarningMessage = string.Format("Note: Silo Inventory has not been entered for today ({0})", new DateTime(now.Year, now.Month, now.Day).ToShortDateString());
                }
                else
                {
                    ShowMissingSiloWarning = Visibility.Hidden;
                }
            }));
        }

        private void GetReferenceTables_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    ErrorHelper.DisplayError(e.Error);
                }
                else
                {
                    _worker.RunWorkerAsync();
                }
            }
            finally
            {
                WindowCursor = Cursors.Arrow;
            }
        }

        private void GetReferenceTables_DoWork(object sender, DoWorkEventArgs e)
        {
            ObservableCollection<LocationModel> locations = new ObservableCollection<LocationModel>();
            ObservableCollection<RecordTypeModel> records = new ObservableCollection<RecordTypeModel>();

            locations = _locationsDBA.GetObservableCollection();
            records = _recordsDBA.GetObservableCollection();

            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                Locations = locations;
                RecordTypes = records;
            }));
        }

        private void GetSiloInventoryByDateRange_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    ErrorHelper.DisplayError(e.Error);
                }
                else
                {
                    _getTodaysSiloInventory.RunWorkerAsync();
                }                
            }
            finally
            {
                WindowCursor = Cursors.Arrow;
            }
        }

        private void GetSiloInventoryByDateRange(object sender, DoWorkEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                WindowCursor = Cursors.Wait;
            }));

            ObservableCollection<SiloInventoryModel> siloInventory = new ObservableCollection<SiloInventoryModel>();

            siloInventory = _siloInventoryDBA.GetByDateRangeObservableCollection(_startDate, _endDate);

            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                SiloInventory = siloInventory;
            }));
        }

        public Visibility ShowMissingSiloWarning
        {
            get { return _showMissingSiloWarning; }
            set
            {
                if (_showMissingSiloWarning != value)
                {
                    _showMissingSiloWarning = value;
                    OnPropertyChanged("ShowMissingSiloWarning");
                }
            }
        }
        public string MissingSiloWarningMessage
        {
            get { return _missingSiloWarningMessage; }
            set
            {
                if (_missingSiloWarningMessage != value)
                {
                    _missingSiloWarningMessage = value;
                    OnPropertyChanged("MissingSiloWarningMessage");
                }
            }
        }

        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged("StartDate");
                }                
            }
        }
        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                if (_endDate != value)
                {
                    _endDate = value;
                    OnPropertyChanged("EndDate");
                }                
            }
        }
        public ObservableCollection<SiloInventoryModel> SiloInventory
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

        public void RefreshSiloInventoryData(object parameter)
        {
            _worker.RunWorkerAsync();
        }

        public void AddDailyInventory(object parameter)
        {
            try
            {
                SiloInventoryModel siloInventory = new SiloInventoryModel();
                siloInventory.LocationID = (int)Enums.LocationEnum.Clewiston;
                siloInventory.RecordTypeID = (int)(Enums.RecordTypeEnum.Daily);

                frmEditSiloInventory editSiloInventory = new frmEditSiloInventory(siloInventory, _locations, _recordTypes);
                bool? dialogResult = editSiloInventory.ShowDialog();

                if (dialogResult.HasValue)
                {
                    if (dialogResult.Value)
                    {
                        //Lets refresh the data
                        _worker.RunWorkerAsync();
                    }
                }
            }
            catch (System.Exception ex)
            {
                ErrorHelper.LogAndDisplayError(ex);
            }
        }

        public void AddInventoryAdjustment(object parameter)
        {
            try
            {
                SiloInventoryModel siloInventory = new SiloInventoryModel();
                siloInventory.LocationID = (int)Enums.LocationEnum.Clewiston;
                siloInventory.RecordTypeID = (int)(Enums.RecordTypeEnum.Adjustment);

                frmEditSiloInventory editSiloInventory = new frmEditSiloInventory(siloInventory, _locations, _recordTypes);
                bool? dialogResult = editSiloInventory.ShowDialog();

                if (dialogResult.HasValue)
                {
                    if (dialogResult.Value)
                    {
                        //Lets refresh the data
                        _worker.RunWorkerAsync();
                    }
                }
            }
            catch (System.Exception ex)
            {
                ErrorHelper.LogAndDisplayError(ex);
            }
        }
        public void EditInventory(object parameter)
        {
            try
            {
                SiloInventoryModel siloInventory = (SiloInventoryModel)parameter;
                if (siloInventory != null)
                {
                    frmEditSiloInventory editSiloInventory = new frmEditSiloInventory(siloInventory, _locations, _recordTypes);
                    bool? dialogResult = editSiloInventory.ShowDialog();

                    if (dialogResult.HasValue)
                    {
                        if(dialogResult.Value)
                        {
                            //Lets refresh the data
                            _worker.RunWorkerAsync();
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                ErrorHelper.LogAndDisplayError(ex);
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

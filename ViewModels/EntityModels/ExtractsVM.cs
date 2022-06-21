using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace JudicialTest
{
    public class ExtractsVM : Singleton<ExtractsVM>, IEntityVM
    {
        private Extract _selectedExtract;
        private ObservableCollection<Extract> _allExtracts;

        public Extract SelectedExtract
        {
            get { return _selectedExtract; }
            set
            {
                _selectedExtract = value;
                VisibilityButtons();
                OnPropertyChanged("SelectedExtract");
            }
        }

        public ObservableCollection<Extract> AllExtracts
        {
            get
            {
                if (_allExtracts == null)
                {
                    _allExtracts = new ObservableCollection<Extract>();
                    _allExtracts.CollectionChanged += new NotifyCollectionChangedEventHandler(AllExtracts_CollectionChanged);
                }
                return _allExtracts;
            }
            set
            {
                _allExtracts = value;
                _allExtracts.CollectionChanged += new NotifyCollectionChangedEventHandler(AllExtracts_CollectionChanged);
            }
        }

        public ICommand AddCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand RemoveCommand { get; set; }

        private ExtractsVM()
        {
            InitializeCommand();
        }

        public void InitializeCommand()
        {
            AddCommand = new RelayCommand(AddEntity);
            SaveCommand = new RelayCommand(SaveEntity);
            RemoveCommand = new RelayCommand(RemoveEntity);
        }

        public void AddEntity()
        {
            Extract extract = new Extract();

            if (PickerEntities.PickedDebtor != null)
                if (!PickerEntities.PickedDebtor.Extracts.Any(item => item.Apartment.ApartmentID == PickerEntities.PickedApartment.ApartmentID))
                {
                    PickerEntities.PickedDebtor.Extracts.Add(extract);
                    PickerEntities.PickedApartment.Extracts.Add(extract);

                    PickerEntities.RefreshTable(MainWindow.Instance.DebtorExtractsTable);
                    PickerEntities.RefreshTable(MainWindow.Instance.ApartmentExtractsTable);
                }
                else
                {
                    string msg = "Ошибка привязки, у выбранного должника уже присутствует выписка ЕГРН на это имущество";
                    MessageBox.Show(msg, Constants.CaptionMsgBox, MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            AllExtracts.Add(extract);

        }

        public void RemoveEntity()
        {
            AllExtracts.Remove(SelectedExtract);
        }

        public void SaveEntity()
        {
            MainWindow.Instance.DB.SaveChanges();
        }

        public void VisibilityButtons()
        {
            if (SelectedExtract != null)
            {
                MainWindow.Instance.HouseProperties.Visibility = Visibility.Visible;
                MainWindow.Instance.Button_Save_Extract.IsEnabled = true;
                MainWindow.Instance.Button_Remove_Extract.IsEnabled = true;
                //if (SelectedExtract.Apartment == null && SelectedExtract.Debtor == null)
                //    MainWindow.Instance.Button_Remove_Extract.IsEnabled = true;
                //else
                //    MainWindow.Instance.Button_Remove_Extract.IsEnabled = false;

            }
            else
            {
                MainWindow.Instance.HouseProperties.Visibility = Visibility.Hidden;
                MainWindow.Instance.Button_Remove_Extract.IsEnabled = false;
                MainWindow.Instance.Button_Save_Extract.IsEnabled = false;
            }
        }

        private void AllExtracts_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    SelectedExtract = e.NewItems[0] as Extract;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    SelectedExtract = AllExtracts.FirstOrDefault();
                    break;
            }
            MainWindow.Instance.DB.SaveChanges();
        }
    }
}

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace JudicialTest
{
    public class DebtorsVM : Singleton<DebtorsVM>, IEntityVM, IPickableEntityVM
    {
        private Apartment _selectedApartment;
        private Debtor _selectedDebtor;
        private ObservableCollection<Debtor> _allDebtors;

        public Apartment SelectedApartment
        {
            get { return _selectedApartment; }
            set
            {
                _selectedApartment = value;
                OnPropertyChanged("SelectedApartment");
            }
        }
        
        public Debtor SelectedDebtor
        {
            get { return _selectedDebtor; }
            set
            {
                _selectedDebtor = value;
                VisibilityButtons();
                OnPropertyChanged("SelectedDebtor");
            }
        }

        public ObservableCollection<Debtor> AllDebtors
        {
            get
            {
                if (_allDebtors == null)
                {
                    _allDebtors = new ObservableCollection<Debtor>();
                    _allDebtors.CollectionChanged += new NotifyCollectionChangedEventHandler(AllDebtors_CollectionChanged);
                }
                return _allDebtors;
            }
            set
            {
                _allDebtors = value;
                _allDebtors.CollectionChanged += new NotifyCollectionChangedEventHandler(AllDebtors_CollectionChanged);
            }
        }


        public ICommand AddCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand RemoveCommand { get; set; }
        public ICommand PickCommand { get; set; }

        public ICommand CreateLink_ApartDebtorCommand { get; set; }        

        private DebtorsVM()
        {
            InitializeCommand();
        }

        public void InitializeCommand()
        {
            AddCommand = new RelayCommand(AddEntity);
            SaveCommand = new RelayCommand(SaveEntity);
            RemoveCommand = new RelayCommand(RemoveEntity);
            PickCommand = new RelayCommand(PickEntity);

            CreateLink_ApartDebtorCommand = new RelayCommand(CreateLink_ApartDebtor);
        }

        public void AddEntity()
        {
            if (PickerEntities.PickedApartment != null)
            {
                Debtor debtor = new Debtor();
                PickerEntities.PickedApartment.Debtors.Add(debtor);
                AllDebtors.Add(debtor);

                SelectedDebtor = debtor;
            }
        }

        public void SaveEntity()
        {
            MainWindow.Instance.DB.SaveChanges();
        }

        public void RemoveEntity()
        {
            if (!SelectedDebtor.Arrears.Any())
            {
                EstateAddressesVM.Instance.AllEstateAddresses.Remove(SelectedDebtor.LocationAddress);
                EstateAddressesVM.Instance.AllEstateAddresses.Remove(SelectedDebtor.RegisterAddress);
                EstateAddressesVM.Instance.AllEstateAddresses.Remove(SelectedDebtor.SectorAddress);
                AllDebtors.Remove(SelectedDebtor);
            }
        }

        public void PickEntity()
        {
            SaveEntity();
            PickerEntities.Pick(SelectedDebtor);
            ShowView(MainWindow.Instance.ExtractsForm);
            ArrearsVM.Instance.RefreshListArrearExceptions();
        }

        private void CreateLink_ApartDebtor()
        {
            CreateLink_ApartDebtorVM.Instance.Init();
            CreateLinkWindow.Instance.ShowDialog();
        }





        private void RefreshApartCB(Debtor debtor)
        {
            switch (debtor.Apartments.Count)
            {
                case 0:
                    MainWindow.Instance.CB_ListApartment.IsEnabled = true;
                    break;
                case 1:
                    MainWindow.Instance.CB_ListApartment.ItemsSource = debtor.Apartments;
                    
                    if (PickerEntities.PickedApartment == null)
                        SelectedApartment = debtor.Apartments.First();
                    else
                        SelectedApartment = PickerEntities.PickedApartment;

                    MainWindow.Instance.CB_ListApartment.IsEnabled = true;
                    break;
                default:
                    MainWindow.Instance.CB_ListApartment.ItemsSource = debtor.Apartments;

                    if (PickerEntities.PickedApartment == null)
                        SelectedApartment = debtor.Apartments.First();
                    else
                        SelectedApartment = PickerEntities.PickedApartment;

                    MainWindow.Instance.CB_ListApartment.IsEnabled = true;
                    break;
            }
            MainWindow.Instance.CB_ListApartment.Items.Refresh();
        }


        public void VisibilityButtons()
        {
            if (SelectedDebtor != null)
            {
                MainWindow.Instance.DebtorProperties.Visibility = Visibility.Visible;
                MainWindow.Instance.Button_Save_Debtor.IsEnabled = true;
                MainWindow.Instance.Button_Pick_Debtor.IsEnabled = true;
                MainWindow.Instance.Button_CreateLink_ApartDebtor.IsEnabled = true;
                
                RefreshApartCB(SelectedDebtor);

                if(SelectedApartment != null && !SelectedDebtor.Arrears.Any(item => item.Apartment.ApartmentID == SelectedApartment.ApartmentID))
                    MainWindow.Instance.Button_Remove_Debtor.IsEnabled = true;
                else
                    MainWindow.Instance.Button_Remove_Debtor.IsEnabled = false;
            }
            else
            {
                MainWindow.Instance.DebtorProperties.Visibility = Visibility.Hidden;
                MainWindow.Instance.Button_Save_Debtor.IsEnabled = false;
                MainWindow.Instance.Button_Remove_Debtor.IsEnabled = false;
                MainWindow.Instance.Button_Pick_Debtor.IsEnabled = false;
                MainWindow.Instance.Button_CreateLink_ApartDebtor.IsEnabled = false;
            }
            
            if (PickerEntities.PickedApartment != null)
                MainWindow.Instance.Button_Add_Debtor.IsEnabled = true;
            else
                MainWindow.Instance.Button_Add_Debtor.IsEnabled = false;
        }

        private void AllDebtors_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {            
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    SelectedDebtor = e.NewItems[0] as Debtor;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems[0] == PickerEntities.PickedDebtor)
                        PickerEntities.PickedDebtor = null;

                    if (PickerEntities.PickedApartment == null)
                        SelectedApartment = SelectedDebtor.Apartments.FirstOrDefault();
                    else
                        SelectedApartment = PickerEntities.PickedApartment;

                    SelectedDebtor = AllDebtors.FirstOrDefault();
                    break;
            }
            MainWindow.Instance.DB.SaveChanges(); 
        }
    }
}

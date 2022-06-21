using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using System.Windows;
using System.Collections.Specialized;

namespace JudicialTest
{
    public class ApartmentsVM : Singleton<ApartmentsVM>, IEntityVM, IPickableEntityVM
    {
        private Apartment _selectedApartment;
        public Apartment SelectedApartment
        {
            get { return _selectedApartment; }
            set
            {
                _selectedApartment = value;
                VisibilityButtons();
                OnPropertyChanged("SelectedApartment");
            }
        }

        private ObservableCollection<Apartment> _allApartments;
        public ObservableCollection<Apartment> AllApartments
        {
            get
            {
                if (_allApartments == null)
                {
                    _allApartments = new ObservableCollection<Apartment>();
                    _allApartments.CollectionChanged += new NotifyCollectionChangedEventHandler(AllApartments_CollectionChanged);
                }
                return _allApartments;
            }
            set
            {
                _allApartments = value;
                _allApartments.CollectionChanged += new NotifyCollectionChangedEventHandler(AllApartments_CollectionChanged);
            }
        }

        public ICommand AddCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand RemoveCommand { get; set; }
        public ICommand PickCommand { get; set; }

        private ApartmentsVM()
        {
            InitializeCommand();
        }

        public void InitializeCommand()
        {
            AddCommand = new RelayCommand(AddEntity);
            SaveCommand = new RelayCommand(SaveEntity);
            RemoveCommand = new RelayCommand(RemoveEntity);
            PickCommand = new RelayCommand(PickEntity);
        }

        public void AddEntity()
        {
            if (PickerEntities.PickedHouse != null)
            {
                Apartment apartment = new Apartment();                
                PickerEntities.PickedHouse.Apartments.Add(apartment);
                AllApartments.Add(apartment);
            }
        }

        public void SaveEntity()
        {
            MainWindow.Instance.DB.SaveChanges();
        }

        public void RemoveEntity()
        {
            if(!SelectedApartment.Debtors.Any())
            {
                AllApartments.Remove(SelectedApartment);
            }            
        }
        public void PickEntity()
        {
            SaveEntity();
            PickerEntities.Pick(SelectedApartment);        
            ShowView(MainWindow.Instance.DebtorsForm);
        }

        public void VisibilityButtons()
        {
            if (SelectedApartment != null)
            {
                MainWindow.Instance.ApartmentProperties.Visibility = Visibility.Visible;
                MainWindow.Instance.Button_Save_Apartment.IsEnabled = true;
                MainWindow.Instance.Button_Pick_Apartment.IsEnabled = true;

                if (!SelectedApartment.Debtors.Any())
                    MainWindow.Instance.Button_Remove_Apartment.IsEnabled = true;
                else
                    MainWindow.Instance.Button_Remove_Apartment.IsEnabled = false;
            }
            else
            {
                MainWindow.Instance.ApartmentProperties.Visibility = Visibility.Hidden;
                MainWindow.Instance.Button_Save_Apartment.IsEnabled = false;
                MainWindow.Instance.Button_Remove_Apartment.IsEnabled = false;
                MainWindow.Instance.Button_Pick_Apartment.IsEnabled = false;
            }

            if (PickerEntities.PickedHouse != null)
                MainWindow.Instance.Button_Add_Apartment.IsEnabled = true;
            else
                MainWindow.Instance.Button_Add_Apartment.IsEnabled = false;
        }

        private void AllApartments_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {            
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    SelectedApartment = e.NewItems[0] as Apartment;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (SelectedApartment == PickerEntities.PickedApartment)
                        PickerEntities.PickedApartment = null;
                    break;
            }
            MainWindow.Instance.DB.SaveChanges();
        }
    }
}

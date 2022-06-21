using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace JudicialTest
{
    public class HousesVM : Singleton<HousesVM>, IEntityVM, IPickableEntityVM
    {
        private House _selectedHouse;
        private ObservableCollection<House> _allHouses;

        public House SelectedHouse
        {
            get { return _selectedHouse; }
            set
            {
                _selectedHouse = value;
                VisibilityButtons();
                OnPropertyChanged("SelectedHouse");
            }
        }

        public ObservableCollection<House> AllHouses
        {
            get
            {
                if (_allHouses == null)
                {
                    _allHouses = new ObservableCollection<House>();
                    _allHouses.CollectionChanged += new NotifyCollectionChangedEventHandler(AllHouses_CollectionChanged);
                }
                return _allHouses;
            }
            set
            {
                _allHouses = value;
                _allHouses.CollectionChanged += new NotifyCollectionChangedEventHandler(AllHouses_CollectionChanged);
            }
        }

        public ICommand AddCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand RemoveCommand { get; set; }
        public ICommand PickCommand { get; set; }

        private HousesVM()
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
            House house = new House();
            AllHouses.Add(house);
        }

        public void SaveEntity()
        {
            MainWindow.Instance.DB.SaveChanges();
        }

        public void RemoveEntity()
        {
            if (!SelectedHouse.Apartments.Any())
            {
                AllHouses.Remove(SelectedHouse);
            }
        }
        public void PickEntity()
        {
            SaveEntity();
            PickerEntities.Pick(SelectedHouse);
            ShowView(MainWindow.Instance.ApartmentsForm);
        }

        public void VisibilityButtons()
        {
            if (SelectedHouse != null)
            {
                MainWindow.Instance.HouseProperties.Visibility = Visibility.Visible;
                MainWindow.Instance.Button_Save_House.IsEnabled = true;
                MainWindow.Instance.Button_Pick_House.IsEnabled = true;

                if (!SelectedHouse.Apartments.Any())
                    MainWindow.Instance.Button_Remove_House.IsEnabled = true;
                else
                    MainWindow.Instance.Button_Remove_House.IsEnabled = false;
            }
            else
            {
                MainWindow.Instance.HouseProperties.Visibility = Visibility.Hidden;
                MainWindow.Instance.Button_Remove_House.IsEnabled = false;
                MainWindow.Instance.Button_Save_House.IsEnabled = false;
                MainWindow.Instance.Button_Pick_House.IsEnabled = false;
            }
        }



        private void AllHouses_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {            
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    SelectedHouse = e.NewItems[0] as House;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems[0] == PickerEntities.PickedHouse)
                        PickerEntities.PickedHouse = null;

                    SelectedHouse = AllHouses.FirstOrDefault();
                    break;
            }
            MainWindow.Instance.DB.SaveChanges();
        }
    }
}

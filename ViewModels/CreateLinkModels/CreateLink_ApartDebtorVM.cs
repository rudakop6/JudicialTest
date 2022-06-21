using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

namespace JudicialTest
{
    public class CreateLink_ApartDebtorVM : Singleton<CreateLink_ApartDebtorVM>
    {
        public ObservableCollection<House> Houses { get; set; } = new ObservableCollection<House>();
        public ObservableCollection<Apartment> FilterApartments { get; set; } = new ObservableCollection<Apartment>();

        private House _selectedHouse;
        public House SelectedHouse
        {
            get { return _selectedHouse; }
            set
            {
                _selectedHouse = value;
                OnPropertyChanged("SelectedHouse");
            }
        }
        private Apartment _selectedApartment;
        public Apartment SelectedApartment
        {
            get { return _selectedApartment; }
            set
            {
                _selectedApartment = value;
                OnPropertyChanged("SelectedApartment");
            }
        }
        public Debtor SelectedDebtor { get; set; }


        public ICommand AddApartmentCommand { get; set; }
        public ICommand ReturnCommand { get; set; }

        public void CB_HouseSelectionChanged()
        {
            FilterApart();
        }

        private CreateLink_ApartDebtorVM()
        {
            AddApartmentCommand = new RelayCommand(AddApartment);
            ReturnCommand = new RelayCommand(Return);

            Houses = HousesVM.Instance.AllHouses;
            SelectedHouse = Houses.First();
            FilterApartments = ApartmentsVM.Instance.AllApartments;
        }
        public void Init()
        {
            SelectedDebtor = DebtorsVM.Instance.SelectedDebtor;
            CreateLinkWindow.Instance.TB_SelectedDebtor.Text = SelectedDebtor.LastName + " " +
                SelectedDebtor.FirstName + " " + SelectedDebtor.MiddleName;

            FilterApart();
            CreateLinkWindow.Instance.CB_AllApartments.Items.Refresh();
        }

        private void FilterApart()
        {
            if (SelectedHouse != null)
                CreateLinkWindow.Instance.CB_AllApartments.Items.Filter = (apart => (apart as Apartment).House.HouseID == SelectedHouse.HouseID &&
                                             !SelectedDebtor.Apartments.Any(apartDebtor => apartDebtor.ApartmentID == (apart as Apartment).ApartmentID));
        }


        private void AddApartment()
        {
            if (SelectedApartment != null)
            {
                SelectedDebtor.Apartments.Add(SelectedApartment);

                DebtorsVM.Instance.VisibilityButtons();
                MainWindow.Instance.DB.SaveChanges();
                SelectedApartment = null;
                CreateLinkWindow.Instance.Hide();
            }
        }
        private void Return()
        {
            CreateLinkWindow.Instance.Hide();
        }
    }
}

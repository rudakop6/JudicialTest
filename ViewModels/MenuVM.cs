using System.Windows.Input;

namespace JudicialTest
{
    public class MenuVM : Singleton<MenuVM>
    {
        public ICommand HouseCommand { get; set; }
        public ICommand ApartmentCommand { get; set; }
        public ICommand DebtorCommand { get; set; }
        public ICommand ArrearCommand { get; set; }
        public ICommand ExtractCommand { get; set; }
        public ICommand SettingsCommand { get; set; }
        public ICommand WipeCommand { get; set; }

        private MenuVM()
        {
            HouseCommand = new RelayCommand(PickHouse);
            ApartmentCommand = new RelayCommand(PickApartment);
            DebtorCommand = new RelayCommand(PickDebtor);
            ArrearCommand = new RelayCommand(PickArrear);
            ExtractCommand = new RelayCommand(PickExtract);
            SettingsCommand = new RelayCommand(Settings);
            WipeCommand = new RelayCommand(WipeFilters);
        }


        private void PickHouse()
        {
            ShowView(MainWindow.Instance.HousesForm);
        }
        private void PickApartment()
        {
            ShowView(MainWindow.Instance.ApartmentsForm);
        }
        private void PickDebtor()
        {
            ShowView(MainWindow.Instance.DebtorsForm);         
        }
        private void PickArrear()
        {
            ShowView(MainWindow.Instance.ArrearsForm);
        }
        private void PickExtract()
        {
            ShowView(MainWindow.Instance.ExtractsForm);
        }
        private void Settings()
        {
            ShowView(MainWindow.Instance.SettingsForm);
        }

        private void WipeFilters()
        {
            PickerEntities.WipePickedEntities();
            ShowView(MainWindow.Instance.HousesForm);
        }                
    }
}

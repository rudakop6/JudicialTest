using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace JudicialTest
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void ShowView(Grid viewModel)
        {
            ClearHUD();
            viewModel.Visibility = Visibility.Visible;
            ReselectEntitiesTable();
        }

        private void ClearHUD()
        {
            MainWindow.Instance.HousesForm.Visibility = Visibility.Hidden;
            MainWindow.Instance.ApartmentsForm.Visibility = Visibility.Hidden;
            MainWindow.Instance.DebtorsForm.Visibility = Visibility.Hidden;
            MainWindow.Instance.ArrearsForm.Visibility = Visibility.Hidden;
            MainWindow.Instance.ExtractsForm.Visibility = Visibility.Hidden;
            MainWindow.Instance.SettingsForm.Visibility = Visibility.Hidden;
        }

        private void ReselectEntitiesTable()
        {
            HousesVM.Instance.SelectedHouse = PickerEntities.PickedHouse;
            ApartmentsVM.Instance.SelectedApartment = PickerEntities.PickedApartment;
            DebtorsVM.Instance.SelectedDebtor = PickerEntities.PickedDebtor;
            ArrearsVM.Instance.SelectedArrear = PickerEntities.PickedArrear;

            if (PickerEntities.PickedDebtor != null)
                ExtractsVM.Instance.SelectedExtract = PickerEntities.PickedDebtor.Extracts.Where(extract =>
                                    extract.Apartment.ApartmentID == PickerEntities.PickedApartment.ApartmentID &&
                                    extract.Debtor.DebtorID == PickerEntities.PickedDebtor.DebtorID).FirstOrDefault();
        }
    }
}

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace JudicialTest
{
    public static class PickerEntities
    {
        private static House _pickedHouse = null;
        private static Apartment _pickedApartment = null;
        private static Debtor _pickedDebtor = null;
        private static Arrear _pickedArrear = null;

        public static House PickedHouse
        {
            get { return _pickedHouse; }
            set
            {
                if (_pickedHouse != null)
                    _pickedHouse.PickStatus = StatusPickedEnum.NotPicked;

                MainWindow.Instance.TB_SelectedHouse.Text = string.Empty;
                if (value != null)
                {
                    MainWindow.Instance.TB_SelectedHouse.Text = "Выбран дом : " + value.GetFullAddress();
                    value.PickStatus = StatusPickedEnum.Picked;
                    MainWindow.Instance.ApartmentsTable.Items.Filter = item => (item as Apartment).House.HouseID == value.HouseID;
                    MainWindow.Instance.DebtorsTable.Items.Filter = item => (item as Debtor).Apartments.Any(apart => apart.House.HouseID == value.HouseID);
                }
                else
                {
                    PickedApartment = null;
                    MainWindow.Instance.ApartmentsTable.Items.Filter = null;
                    MainWindow.Instance.DebtorsTable.Items.Filter = null;
                }
                RefreshDisplay();
                _pickedHouse = value;
            }
        }
        public static Apartment PickedApartment
        {
            get { return _pickedApartment; }
            set
            {
                if (_pickedApartment != null)
                    _pickedApartment.PickStatus = StatusPickedEnum.NotPicked;

                MainWindow.Instance.TB_SelectedApartment.Text = string.Empty;
                if (value != null)
                {
                    MainWindow.Instance.TB_SelectedApartment.Text = "Выбрана квартира : " + value.EstateNumber;
                    value.PickStatus = StatusPickedEnum.Picked;
                    MainWindow.Instance.DebtorsTable.Items.Filter = item => (item as Debtor).Apartments.
                                               Any(apart => apart.ApartmentID == value.ApartmentID);
                }
                else
                {
                    PickedDebtor = null;
                    if(PickedHouse == null)
                        MainWindow.Instance.DebtorsTable.Items.Filter = null;
                }
                RefreshDisplay();
                _pickedApartment = value;
            }
        }
        public static Debtor PickedDebtor
        {
            get { return _pickedDebtor; }
            set
            {
                if (_pickedDebtor != null)
                    _pickedDebtor.PickStatus = StatusPickedEnum.NotPicked;

                MainWindow.Instance.TB_SelectedDebtor.Text = string.Empty;
                if (value != null)
                {
                    MainWindow.Instance.TB_SelectedDebtor.Text = "Выбран должник : " + value.LastName;
                    value.PickStatus = StatusPickedEnum.Picked;
                    MainWindow.Instance.ArrearsTable.Items.Filter = item => (item as Arrear).Debtor.DebtorID == value.DebtorID && 
                                                                            (item as Arrear).Apartment.ApartmentID == PickedApartment.ApartmentID;

                    MainWindow.Instance.ExtractsTable.Items.Filter = item => (item as Extract).Debtor?.DebtorID == value.DebtorID &&
                                                                            (item as Extract).Apartment.ApartmentID == PickedApartment.ApartmentID;
                    MainWindow.Instance.Button_Arrears.IsEnabled = true;
                }
                else
                {
                    PickedArrear = null;
                    MainWindow.Instance.ArrearsTable.Items.Filter = null;
                    MainWindow.Instance.ExtractsTable.Items.Filter = null;
                    MainWindow.Instance.Button_Arrears.IsEnabled = false;
                }
                RefreshDisplay();
                _pickedDebtor = value;
            }
        }
        public static Arrear PickedArrear
        {
            get { return _pickedArrear; }
            set
            {
                if (_pickedArrear != null)
                    _pickedArrear.PickStatus = StatusPickedEnum.NotPicked;

                MainWindow.Instance.TB_SelectedArrear.Text = string.Empty;
                if (value != null)
                {
                    MainWindow.Instance.TB_SelectedArrear.Text = 
                            "Выбрана задолженность с " + GetDateString(value.StartDate) + 
                            " по " + GetDateString(value.EndDate);

                    value.PickStatus = StatusPickedEnum.Picked;

                    MainWindow.Instance.Button_CreateSP.Visibility = Visibility.Visible;
                }
                else
                {
                    MainWindow.Instance.Button_CreateSP.Visibility = Visibility.Hidden;
                }
                RefreshDisplay();
                _pickedArrear = value;
            }
        }

        public static string GetDateString(PeriodDateArrear periodDate)
        {
            string strDate = periodDate.Day + "." + periodDate.Month + "." + periodDate.Year;
            DateTime.TryParse(strDate, out DateTime date);
            return date.ToShortDateString();
        }

        public static void Pick(House pickedEntity)
        {
            PickedHouse = pickedEntity;
            PickedApartment = null;
        }
        public static void Pick(Apartment pickedEntity)
        {
            PickedHouse = pickedEntity.House;
            PickedApartment = pickedEntity;            
            PickedDebtor = null;
        }
        public static void Pick(Debtor pickedEntity)
        {
            if (DebtorsVM.Instance.SelectedApartment != null)
            {
                PickedHouse = DebtorsVM.Instance.SelectedApartment.House;
                PickedApartment = DebtorsVM.Instance.SelectedApartment;
                PickedDebtor = pickedEntity;
                PickedArrear = null;
            }
        }
        public static void Pick(Arrear pickedEntity)
        {
            PickedHouse = pickedEntity.Apartment.House;
            PickedApartment = pickedEntity.Apartment;
            PickedDebtor = pickedEntity.Debtor;
            PickedArrear = pickedEntity;
        }

        public static void WipePickedEntities()
        {
            PickedHouse = null;
        }

        public static void RefreshDisplay()
        {
            RefreshTable(MainWindow.Instance.HousesTable);
            RefreshTable(MainWindow.Instance.ApartmentsTable);
            RefreshTable(MainWindow.Instance.DebtorsTable);
            RefreshTable(MainWindow.Instance.ArrearsTable);
        }
        public static void RefreshTable(DataGrid dataBaseView)
        {
            CollectionViewSource.GetDefaultView(dataBaseView.ItemsSource).Refresh();
        }
    }
}

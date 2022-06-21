using System;
using System.ComponentModel;
using System.Data.Entity;
using System.Windows;
using System.Windows.Input;
using TextBox = System.Windows.Controls.TextBox;

namespace JudicialTest
{
    public partial class MainWindow : Window
    {
        //Реализация синглтона со статической ссылкой на объект
        private static MainWindow _instance;
        public static MainWindow Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MainWindow();

                return _instance;
            }
        }
        //Получение доступа к базе данных        
        public readonly ModelDB DB = new ModelDB(InitializePaths.Instance.GetConnectionString());

        public MainWindow()
        {
            _instance = this;
            InitializeComponent();

            //Первоначальные настройки
            Dictionaries.Instance.InitComboBoxes();
            this.Closing += Window_Closing;
            this.Closed += Window_Closed;

            //Настройка контекста данных для привязок отображения
            MainMenuForm.DataContext = MenuVM.Instance;
            HousesForm.DataContext = HousesVM.Instance;
            ApartmentsForm.DataContext = ApartmentsVM.Instance;
            DebtorsForm.DataContext = DebtorsVM.Instance;
            ArrearsForm.DataContext = ArrearsVM.Instance;
            ExtractsForm.DataContext = ExtractsVM.Instance;
            SettingsForm.DataContext = SettingsVM.Instance;

            SettingsVM.Instance.InitPathList();

            LoadAllTablesDB();
        }

        //Привязка таблиц из базы данных к коллекциям модели отображения
        public void LoadAllTablesDB()
        {
            DB.EstateAddresses_DB.Load();
            EstateAddressesVM.Instance.AllEstateAddresses = DB.EstateAddresses_DB.Local;

            DB.Houses_DB.Load();
            HousesVM.Instance.AllHouses = DB.Houses_DB.Local;
            
            DB.Apartments_DB.Load();
            ApartmentsVM.Instance.AllApartments = DB.Apartments_DB.Local;

            DB.Debtors_DB.Load();
            DebtorsVM.Instance.AllDebtors = DB.Debtors_DB.Local;

            DB.Arrears_DB.Load();
            ArrearsVM.Instance.AllArrears = DB.Arrears_DB.Local;

            DB.Extracts_DB.Load();
            ExtractsVM.Instance.AllExtracts = DB.Extracts_DB.Local;

            EstateAddressesVM.Instance.SelectedEstateAddress = null;
            HousesVM.Instance.SelectedHouse = null;
            ApartmentsVM.Instance.SelectedApartment = null;
            DebtorsVM.Instance.SelectedDebtor = null;
            ArrearsVM.Instance.SelectedArrear = null;
            ExtractsVM.Instance.SelectedExtract = null;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            string msg = "Закрыть?";
            MessageBoxResult result =
              MessageBox.Show(msg, "Подтверждение закрытия",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.No)
                e.Cancel = true;
        }
        private void Window_Closed(object sender, EventArgs e) => App.Current.Shutdown();


        private void TB_Double_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (char.IsDigit(e.Text, 0))
                return;

            if (e.Text.Contains(",") || e.Text.Contains("."))
            {
                TextBox textBox = (TextBox)sender;
                if (textBox.Text.Contains(","))
                {
                    e.Handled = true;
                    return;
                }

                if (textBox.Text.Length == 0)
                    textBox.Text = 0.ToString();

                textBox.Text += ",";
                textBox.Select(textBox.Text.Length, 0);

            }
            e.Handled = true;
        }

        private void TB_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        private void TB_Double_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (double.TryParse((textBox.Text), out double value))
            {
                textBox.Text = value.ToString();
            }
            else
            {
                textBox.Text = 0.ToString();
                textBox.Select(textBox.Text.Length, 0);
            }
        }

        private void TB_Date_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (char.IsDigit(e.Text, 0))
            {
                TextBox textBox = (TextBox)sender;
                if (textBox.Text.Length > 9)
                {
                    e.Handled = true;
                }
                if (textBox.Text.Length == 2 || textBox.Text.Length == 5)
                {
                    textBox.Text += ".";
                    textBox.Select(textBox.Text.Length, 0);
                }
                if (textBox.Text.Length > 2 && textBox.Text[2] != '.')
                {
                    textBox.Text = textBox.Text.Insert(2, ".");
                }
                if (textBox.Text.Length > 5 && textBox.Text[5] != '.')
                {
                    textBox.Text = textBox.Text.Insert(5, ".");
                }
            }
            else
            {
                e.Handled = true;
            }
        }

        private void TB_Date_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (DateTime.TryParse((textBox.Text), out DateTime value))
                textBox.Text = value.ToShortDateString();
            else
                textBox.Text = string.Empty;
        }

        private void CB_ArrearYear_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ArrearsVM.Instance.RefreshFilterCB();
        }
    }
}
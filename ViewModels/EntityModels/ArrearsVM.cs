using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace JudicialTest
{
    public class ArrearsVM : Singleton<ArrearsVM>, IEntityVM, IPickableEntityVM
    {
        private Arrear _selectedArrear;
        private ObservableCollection<Arrear> _allArrears;
        private readonly List<ArrearExceptions> _listArrearExceptions = new List<ArrearExceptions>();

        public Arrear SelectedArrear
        {
            get { return _selectedArrear; }
            set
            {
                _selectedArrear = value;
                VisibilityButtons();
                RefreshFilterCB();
                OnPropertyChanged("SelectedArrear");
            }
        }

        public ObservableCollection<Arrear> AllArrears
        {
            get
            {
                if (_allArrears == null)
                {
                    _allArrears = new ObservableCollection<Arrear>();
                    _allArrears.CollectionChanged += new NotifyCollectionChangedEventHandler(AllArrears_CollectionChanged);
                }
                return _allArrears;
            }
            set
            {
                _allArrears = value;
                _allArrears.CollectionChanged += new NotifyCollectionChangedEventHandler(AllArrears_CollectionChanged);
            }
        }

        public ICommand AddCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand RemoveCommand { get; set; }
        public ICommand PickCommand { get; set; }

        public ICommand CreateIPCommand { get; set; }
        public ICommand CreateSPCommand { get; set; }

        private ArrearsVM()
        {
            InitializeCommand();
        }

        public void InitializeCommand()
        {
            AddCommand = new RelayCommand(AddEntity);
            SaveCommand = new RelayCommand(SaveEntity);
            RemoveCommand = new RelayCommand(RemoveEntity);
            PickCommand = new RelayCommand(PickEntity);

            CreateIPCommand = new RelayCommand(CreateIP);
            CreateSPCommand = new RelayCommand(CreateSP);
        }

        public void AddEntity()
        {
            if (PickerEntities.PickedDebtor != null)
            {
                Arrear arrear = new Arrear()
                {
                    //исправить на расчетные, исходя из уже сформированных задолженностей
                    StartDate = new PeriodDateArrear()
                    {
                        Year = DateTime.Now.Year,
                        Month = 1,
                        Day = 1
                    },
                    EndDate = new PeriodDateArrear()
                    {
                        Year = DateTime.Now.Year,
                        Month = 12,
                        Day = DateTime.DaysInMonth(DateTime.Now.Year, 12)
                    },
                    NumberArrear = PickerEntities.PickedApartment.Arrears.Count() + 1
                };
                PickerEntities.PickedDebtor.Arrears.Add(arrear);
                PickerEntities.PickedApartment.Arrears.Add(arrear);
                AllArrears.Add(arrear);
            }
        }

        public void SaveEntity()
        {
            //if(Checker())
            //{
            SelectedArrear.EndDate.Day = DateTime.DaysInMonth(SelectedArrear.EndDate.Year, SelectedArrear.EndDate.Month);
            MainWindow.Instance.DB.SaveChanges();
            //RefreshListArrearExceptions();
            //}
            //else
            //{
            //    string msg = "Ошибка сохранения";
            //    MessageBox.Show(msg, Constants.CaptionMsgBox, MessageBoxButton.OK, MessageBoxImage.Warning);
            //}
        }

        private bool Checker()
        {
            return !_listArrearExceptions.Where(item => item.Arrear.ArrearID != SelectedArrear.ArrearID).Any(a =>
                                            SelectedArrear.StartDate.Month == a.Month &&
                                            SelectedArrear.StartDate.Year == a.Year);
        }

        public void RemoveEntity()
        {
            AllArrears.Remove(SelectedArrear);
        }

        public void PickEntity()
        {
            SaveEntity();
            PickerEntities.Pick(SelectedArrear);
            ShowView(MainWindow.Instance.ArrearsForm);
        }

        public void RefreshFilterCB()
        {
            //if (SelectedArrear != null)
            //{
            //    string str = string.Empty;
            //    foreach (var item in _listArrearExceptions.Where(item => item.Arrear.ArrearID != SelectedArrear.ArrearID))
            //    {
            //        str += "п " + item.Arrear.DutyPeny + " г " + item.Year + " м " + item.Month + "\n";
            //    }
            //    MainWindow.Instance.DEBUG.Text = str;

            //    MainWindow.Instance.CB_ArrearStartMonth.Items.Filter = null;
            //    MainWindow.Instance.CB_ArrearStartMonth.Items.Filter = month => !_listArrearExceptions.Where(
            //                                    item => item.Arrear.ArrearID != SelectedArrear.ArrearID).Any(
            //                                    a => (string)month == Dictionaries.Instance.GetMonth(a.Month) &&
            //                                    SelectedArrear.StartDate.Year == a.Year);

            //    MainWindow.Instance.CB_ArrearEndMonth.Items.Filter = month => !_listArrearExceptions.Where(
            //                                    item => item.Arrear.ArrearID != SelectedArrear.ArrearID).Any(
            //                                    a => (string)month == Dictionaries.Instance.GetMonth(a.Month) &&
            //                                    SelectedArrear.EndDate.Year == a.Year);

            //    MainWindow.Instance.CB_ArrearStartMonth.SelectedItem = SelectedArrear.StartDate.Year;
            //    MainWindow.Instance.CB_ArrearEndMonth.SelectedItem = SelectedArrear.EndDate.Year;
            //}
        }


        public void RefreshListArrearExceptions()
        {
            List<Arrear> filterList = new List<Arrear>();
            filterList = PickerEntities.PickedDebtor.Arrears.ToList().Where(item =>
                                        item.Apartment.ApartmentID == PickerEntities.PickedApartment.ApartmentID).ToList();

            _listArrearExceptions.Clear();
            foreach (var item in filterList)
            {
                for (int year = item.StartDate.Year; year <= item.EndDate.Year; year++)
                {
                    int startMonth = 0;
                    int endMonth = 0;

                    if (year == item.StartDate.Year)
                        startMonth = item.StartDate.Month;
                    else
                        startMonth = Constants.StartMonth;

                    if (year == item.EndDate.Year)
                        endMonth = item.EndDate.Month;
                    else
                        endMonth = Constants.EndMonth;

                    List<int> listMonth = new List<int>();
                    for (int month = startMonth; month <= endMonth; month++)
                    {
                        ArrearExceptions arrearException = new ArrearExceptions(item, year, month);
                        _listArrearExceptions.Add(arrearException);
                    }
                }
            }
        }

        private void CreateIP()
        {
            RefreshListArrearExceptions();
        }
        private void CreateSP()
        {
            RefreshListArrearExceptions();
            if (PickerEntities.PickedArrear != null)
            {
                ShablonSP.CreateCopySP(PickerEntities.PickedArrear);
            }
        }

        public void VisibilityButtons()
        {
            if (SelectedArrear != null)
            {
                MainWindow.Instance.ArrearProperties.Visibility = Visibility.Visible;
                MainWindow.Instance.Button_Save_Arrear.IsEnabled = true;
                MainWindow.Instance.Button_Remove_Arrear.IsEnabled = true;
                MainWindow.Instance.Button_Pick_Arrear.IsEnabled = true;
            }
            else
            {
                MainWindow.Instance.ArrearProperties.Visibility = Visibility.Hidden;
                MainWindow.Instance.Button_Save_Arrear.IsEnabled = false;
                MainWindow.Instance.Button_Remove_Arrear.IsEnabled = false;
                MainWindow.Instance.Button_Pick_Arrear.IsEnabled = false;
            }

            if (PickerEntities.PickedDebtor != null)
                MainWindow.Instance.Button_Add_Arrear.IsEnabled = true;
            else
                MainWindow.Instance.Button_Add_Arrear.IsEnabled = false;
        }

        private void AllArrears_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    SelectedArrear = e.NewItems[0] as Arrear;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems[0] == PickerEntities.PickedArrear)
                        PickerEntities.PickedArrear = null;
                    break;
            }
            MainWindow.Instance.DB.SaveChanges();
            RefreshListArrearExceptions();
        }
    }
}

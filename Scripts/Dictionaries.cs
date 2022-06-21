using System;
using System.Collections.Generic;

namespace JudicialTest
{
    public class Dictionaries : Singleton<Dictionaries>
    {
        public readonly Dictionary<MonthsOfYearEnum, string> MonthsDictionary = new Dictionary<MonthsOfYearEnum, string>();
        public readonly HashSet<int> YearsDictionary = new HashSet<int>();

        private readonly Dictionary<EstateTypeEnum, string> _estateTypeDictionary = new Dictionary<EstateTypeEnum, string>();
        private readonly Dictionary<GenderTypeEnum, string> _genderTypeDictionary = new Dictionary<GenderTypeEnum, string>();
        private readonly Dictionary<LifeCycleArrearEnum, string> _lifeCycleDictionary = new Dictionary<LifeCycleArrearEnum, string>();

        private Dictionaries() {}


        public void InitComboBoxes()
        {
            InitEstateTypesCB();
            InitGenderTypesCB();
            InitLyfeCycleArrearsCB();
            InitMonthsCB();
            InitYearsCB();
        }
        public string GetEstateType(EstateTypeEnum type)
        {
            _estateTypeDictionary.TryGetValue(type, out string value);
            return value;
        }
        public string GetGenderType(GenderTypeEnum type)
        {
            _genderTypeDictionary.TryGetValue(type, out string value);
            return value;
        }
        public string GetLifeCycleArrear(LifeCycleArrearEnum lifeCycle)
        {
            _lifeCycleDictionary.TryGetValue(lifeCycle, out string value);
            return value;
        }
        public string GetMonth(MonthsOfYearEnum month)
        {
            MonthsDictionary.TryGetValue(month, out string value);
            return value;
        }
        public string GetMonth(int month)
        {
            MonthsDictionary.TryGetValue((MonthsOfYearEnum)month, out string value);
            return value;
        }
        
        private void InitEstateTypesCB()
        {
            _estateTypeDictionary.Add(EstateTypeEnum.Kvartira, Constants.Kvartira);
            _estateTypeDictionary.Add(EstateTypeEnum.Pomeshenie, Constants.Pomeshenie);

            MainWindow.Instance.CB_ApartmentType.ItemsSource = _estateTypeDictionary.Values;
        }
        private void InitGenderTypesCB()
        {
            _genderTypeDictionary.Add(GenderTypeEnum.Male, Constants.Male);
            _genderTypeDictionary.Add(GenderTypeEnum.Female, Constants.Female);

            MainWindow.Instance.CB_GenderType.ItemsSource = _genderTypeDictionary.Values;
        }
        private void InitLyfeCycleArrearsCB()
        {
            _lifeCycleDictionary.Add(LifeCycleArrearEnum.Created, Constants.Created);
            _lifeCycleDictionary.Add(LifeCycleArrearEnum.SendedDocCourtOrder, Constants.SendedDocCourtOrder);
            _lifeCycleDictionary.Add(LifeCycleArrearEnum.RecievedCourtOrder, Constants.RecievedCourtOrder);
            _lifeCycleDictionary.Add(LifeCycleArrearEnum.IssuedJudicialDecision, Constants.IssuedJudicialDecision);
            _lifeCycleDictionary.Add(LifeCycleArrearEnum.RecievedExecutiveDoc, Constants.RecievedExecutiveDoc);
            _lifeCycleDictionary.Add(LifeCycleArrearEnum.SendedDocExecutionProcess, Constants.SendedDocExecutionProcess);
            _lifeCycleDictionary.Add(LifeCycleArrearEnum.Finished, Constants.Finished);

            MainWindow.Instance.CB_LifeCycleArrear.ItemsSource = _lifeCycleDictionary.Values;
        }
        private void InitMonthsCB()
        {
            MonthsDictionary.Add(MonthsOfYearEnum.January, Constants.January);
            MonthsDictionary.Add(MonthsOfYearEnum.February, Constants.February);
            MonthsDictionary.Add(MonthsOfYearEnum.March, Constants.March);
            MonthsDictionary.Add(MonthsOfYearEnum.April, Constants.April);
            MonthsDictionary.Add(MonthsOfYearEnum.May, Constants.May);
            MonthsDictionary.Add(MonthsOfYearEnum.June, Constants.June);

            MonthsDictionary.Add(MonthsOfYearEnum.July, Constants.July);
            MonthsDictionary.Add(MonthsOfYearEnum.August, Constants.August);
            MonthsDictionary.Add(MonthsOfYearEnum.September, Constants.September);
            MonthsDictionary.Add(MonthsOfYearEnum.October, Constants.October);
            MonthsDictionary.Add(MonthsOfYearEnum.November, Constants.November);
            MonthsDictionary.Add(MonthsOfYearEnum.December, Constants.December);

            MainWindow.Instance.CB_ArrearStartMonth.ItemsSource = MonthsDictionary.Values;
            MainWindow.Instance.CB_ArrearEndMonth.ItemsSource = MonthsDictionary.Values;
        }
        private void InitYearsCB()
        {
            for (int i = Constants.StartYear; i < DateTime.Now.Year + 1; i++)
            {
                YearsDictionary.Add(i);
            }

            MainWindow.Instance.CB_ArrearStartYear.ItemsSource = YearsDictionary;
            MainWindow.Instance.CB_ArrearEndYear.ItemsSource = YearsDictionary;
        }
    }
}
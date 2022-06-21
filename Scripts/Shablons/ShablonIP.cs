//using System.Threading.Tasks;
//using Excel = Microsoft.Office.Interop.Excel;
//namespace JudicialTest
//{
//    public static class ShablonIP
//    {
//        public static int FullNameString1 = 19;
//        public static string FullNameColumn1 = "A";

//        public static int FullNameString2 = 31;
//        public static string FullNameColumn2 = "D";

//        public static int PassportIDString = 25;
//        public static string PassportIDColumn = "C";

//        public static int PassportDateString = 25;
//        public static string PassportDateColumn = "F";

//        public static int PassportIssueByString = 26;
//        public static string PassportIssueByColumn = "A";

//        public static int BirthDateString = 23;
//        public static string BirthDateColumn = "C";

//        public static int BirthPlaceString = 24;
//        public static string BirthPlaceColumn = "C";

//        public static int EGRNString = 28;
//        public static string EGRNColumn = "C";

//        public static int EstateTypeString1 = 20;
//        public static string EstateTypeColumn1 = "D";
//        public static int EstateTypeString2 = 27;
//        public static string EstateTypeColumn2 = "C";

//        public static int EstateNumberString1 = 20;
//        public static string EstateNumberColumn1 = "F";
//        public static int EstateNumberString2 = 27;
//        public static string EstateNumberColumn2 = "E";

//        public static int DutyStateString = 18;
//        public static string DutyStateColumn = "B";

//        public static int DutyPeriodString = 17;
//        public static string DutyPeriodColumn = "A";

//        public static int DutyPenyString = 17;
//        public static string DutyPenyColumn = "E";

//        public static int DutyGeneralString = 15;
//        public static string DutyGeneralColumn = "G";

//        public static int PeriodString = 16;
//        public static string PeriodColumn = "H";

//        public static int OrderDateString1 = 14;
//        public static string OrderDateColumn1 = "D";
//        public static int OrderDateString2 = 51;
//        public static string OrderDateColumn2 = "D";

//        public static int OrderNumberString1 = 14;
//        public static string OrderNumberColumn1 = "G";
//        public static int OrderNumberString2 = 51;
//        public static string OrderNumberColumn2 = "F";

//        public static int DateCreateString = 56;
//        public static string DateCreateColumn = "C";

//        public static void CreateCopyIP(Apartment person)
//        {
//            AsyncIP(person);
//        }

//        private static void StartAsync(MainWindow window)
//        {
//            window.StartFormIP.Visibility = System.Windows.Visibility.Hidden;
//            window.StartFormSP.Visibility = System.Windows.Visibility.Hidden;
//            window.ButtonSettings_DataBase.Visibility = System.Windows.Visibility.Hidden;
//            window.ProgressGrid.Visibility = System.Windows.Visibility.Visible;
//        }
//        private static void EndAsync(MainWindow window)
//        {
//            window.StartFormIP.Visibility = System.Windows.Visibility.Visible;
//            window.StartFormSP.Visibility = System.Windows.Visibility.Visible;
//            window.ButtonSettings_DataBase.Visibility = System.Windows.Visibility.Visible;
//            window.ProgressGrid.Visibility = System.Windows.Visibility.Hidden;
//            window.ProgressInfo.Value = 0;
//        }

//        private async static void AsyncIP(Apartment person)
//        {
//            MainWindow window = (MainWindow)App.Current.MainWindow;
//            StartAsync(window);
//            WindowsHandlers.Instance.PathList.TryGetValue(PathEnum.NameFileShablonIP, out string pathExcel);

//            Excel.Application xlApp = new Excel.Application();
//            Excel.Workbook xlWB;
//            Excel.Worksheet list;
//            xlWB = xlApp.Workbooks.Open(pathExcel); //открытие файла, расположенного по пути
//            list = xlWB.Worksheets["Лист1"]; //открытие листа в этом файле    

//            window.ProgressInfo.Value = 20;
//            await Task.Run(() => FillingDate_Order_EGRN(list, person));
//            window.ProgressInfo.Value = 40;
//            await Task.Run(() => FillingPeriod_Duty(list, person));
//            window.ProgressInfo.Value = 60;
//            await Task.Run(() => FillingName_Estate(list, person));
//            window.ProgressInfo.Value = 80;
//            await Task.Run(() => FillingPassportData(list, person));
//            window.ProgressInfo.Value = 100;

//            xlWB.SaveCopyAs(GeneratedName(person.EstateNumber, person.FullName, person.VersionDoc));
//            xlWB.Saved = true;
//            xlApp.Workbooks.Close();
//            xlApp.Quit();
//            window.ProgressInfo.Value = 100;
//            EndAsync(window);
//        }

//        //public static void CreateCopyIP(ref Excel.Worksheet list, ref Person person)
//        //{
//        //    FillingDate_Order_EGRN(list, person);
//        //    FillingPeriod_Duty(list, person);
//        //    FillingName_Estate(list, person);
//        //    FillingPassportData(list, person);
//        //}
//        public static string GeneratedName(string number, string name, int version)
//        {
//            WindowsHandlers.Instance.PathList.TryGetValue(PathEnum.NameFolderResultIP, out string path);
//            var str = name.Split(' ');
//            string name1 = "\\" + number + "_" + str[0] + "_ИП" + version.ToString() + ".xlsx";
//            return path + name1;
//        }
//        public static void FillingDate_Order_EGRN(Excel.Worksheet list, Apartment person)
//        {
//            string orderDate = person.OrderDate;
//            string orderNumber = person.OrderNumber;
//            string egrn = person.EGRN + ".";
//            string dateCreate = person.DateCreate;

//            //дата создания документа
//            list.Cells[DateCreateString, DateCreateColumn] = dateCreate;
//            list.Cells[DateCreateString, DateCreateColumn].HorizontalAlignment = Excel.Constants.xlCenter;
//            list.Cells[DateCreateString, DateCreateColumn].Font.Bold = true;

//            //дата судебного приказа
//            list.Cells[OrderDateString1, OrderDateColumn1] = orderDate;
//            list.Cells[OrderDateString1, OrderDateColumn1].HorizontalAlignment = Excel.Constants.xlCenter;
//            list.Cells[OrderDateString1, OrderDateColumn1].Font.Bold = true;

//            list.Cells[OrderDateString2, OrderDateColumn2] = orderDate;
//            list.Cells[OrderDateString2, OrderDateColumn2].HorizontalAlignment = Excel.Constants.xlCenter;
//            list.Cells[OrderDateString2, OrderDateColumn2].Font.Bold = true;
//            //номер судебного приказа
//            list.Cells[OrderNumberString1, OrderNumberColumn1] = orderNumber;
//            list.Cells[OrderNumberString1, OrderNumberColumn1].HorizontalAlignment = Excel.Constants.xlCenter;
//            list.Cells[OrderNumberString1, OrderNumberColumn1].Font.Bold = true;

//            list.Cells[OrderNumberString2, OrderNumberColumn2] = orderNumber + ",";
//            list.Cells[OrderNumberString2, OrderNumberColumn2].HorizontalAlignment = Excel.Constants.xlCenter;
//            list.Cells[OrderNumberString2, OrderNumberColumn2].Font.Bold = true;
//            //егрн
//            list.Cells[EGRNString, EGRNColumn] = egrn;
//            list.Cells[EGRNString, EGRNColumn].HorizontalAlignment = Excel.Constants.xlLeft;
//            list.Cells[EGRNString, EGRNColumn].Font.Bold = true;
//        }

//        public static void FillingPeriod_Duty(Excel.Worksheet list, Apartment person)
//        {
//            string periodStart = person.PeriodStart;
//            string periodEnd = person.PeriodEnd;
//            string period = "c " + periodStart + " по " + periodEnd;
//            float dutyState = person.DutyState;
//            float dutyPeny = person.DutyPeny;
//            float dutyPeriod = person.DutyPeriod;
//            float dutyGeneral = dutyPeny + dutyPeriod + dutyState;

//            //период
//            list.Cells[PeriodString, PeriodColumn] = period;
//            list.Cells[PeriodString, PeriodColumn].HorizontalAlignment = Excel.Constants.xlCenter;
//            list.Cells[PeriodString, PeriodColumn].Font.Bold = true;
//            //госпошлина
//            list.Cells[DutyStateString, DutyStateColumn] = dutyState.ToString();
//            list.Cells[DutyStateString, DutyStateColumn].HorizontalAlignment = Excel.Constants.xlCenter;
//            list.Cells[DutyStateString, DutyStateColumn].Font.Bold = true;
//            //долг за период
//            list.Cells[DutyPeriodString, DutyPeriodColumn] = dutyPeriod.ToString();
//            list.Cells[DutyPeriodString, DutyPeriodColumn].HorizontalAlignment = Excel.Constants.xlCenter;
//            list.Cells[DutyPeriodString, DutyPeriodColumn].Font.Bold = true;
//            //пени
//            list.Cells[DutyPenyString, DutyPenyColumn] = dutyPeny.ToString();
//            list.Cells[DutyPenyString, DutyPenyColumn].HorizontalAlignment = Excel.Constants.xlCenter;
//            list.Cells[DutyPenyString, DutyPenyColumn].Font.Bold = true;
//            //общий долг
//            list.Cells[DutyGeneralString, DutyGeneralColumn] = dutyGeneral.ToString();
//            list.Cells[DutyGeneralString, DutyGeneralColumn].HorizontalAlignment = Excel.Constants.xlCenter;
//            list.Cells[DutyGeneralString, DutyGeneralColumn].Font.Bold = true;
//        }
//        public static void FillingName_Estate(Excel.Worksheet list, Apartment person)
//        {
//            string fullName = person.FullName;
//            string estateType = person.EstateType;
//            string estateNumber = person.EstateNumber;

//            //ФИО должника
//            list.Cells[FullNameString1, FullNameColumn1] = fullName + ",";
//            list.Cells[FullNameString1, FullNameColumn1].HorizontalAlignment = Excel.Constants.xlCenter;
//            list.Cells[FullNameString1, FullNameColumn1].Font.Bold = true;

//            list.Cells[FullNameString2, FullNameColumn2] = fullName;
//            list.Cells[FullNameString2, FullNameColumn2].HorizontalAlignment = Excel.Constants.xlCenter;
//            list.Cells[FullNameString2, FullNameColumn2].Font.Bold = true;

//            //тип имущества
//            list.Cells[EstateTypeString1, EstateTypeColumn1] = estateType;
//            list.Cells[EstateTypeString1, EstateTypeColumn1].HorizontalAlignment = Excel.Constants.xlCenter;
//            list.Cells[EstateTypeString1, EstateTypeColumn1].Font.Bold = true;

//            list.Cells[EstateTypeString2, EstateTypeColumn2] = estateType;
//            list.Cells[EstateTypeString2, EstateTypeColumn2].HorizontalAlignment = Excel.Constants.xlCenter;
//            list.Cells[EstateTypeString2, EstateTypeColumn2].Font.Bold = true;
//            //номер имущества
//            list.Cells[EstateNumberString1, EstateNumberColumn1] = estateNumber;
//            list.Cells[EstateNumberString1, EstateNumberColumn1].HorizontalAlignment = Excel.Constants.xlCenter;
//            list.Cells[EstateNumberString1, EstateNumberColumn1].Font.Bold = true;

//            list.Cells[EstateNumberString2, EstateNumberColumn2] = estateNumber;
//            list.Cells[EstateNumberString2, EstateNumberColumn2].HorizontalAlignment = Excel.Constants.xlCenter;
//            list.Cells[EstateNumberString2, EstateNumberColumn2].Font.Bold = true;
//        }
//        public static void FillingPassportData(Excel.Worksheet list, Apartment person)
//        {
//            string birthDate = person.BirthDate + ";";
//            string birthPlace = person.BirthPlace + ";";
//            string passportID = person.PassportID + ",";
//            string passportDate = person.PassportDate;
//            string passportIssueBy = person.PassportIssueBy + ";";

//            //день рождения
//            list.Cells[BirthDateString, BirthDateColumn] = birthDate;
//            list.Cells[BirthDateString, BirthDateColumn].HorizontalAlignment = Excel.Constants.xlCenter;
//            list.Cells[BirthDateString, BirthDateColumn].Font.Bold = true;
//            //место рождения
//            list.Cells[BirthPlaceString, BirthPlaceColumn] = birthPlace;
//            list.Cells[BirthPlaceString, BirthPlaceColumn].HorizontalAlignment = Excel.Constants.xlLeft;
//            list.Cells[BirthPlaceString, BirthPlaceColumn].Font.Bold = true;
//            //номер паспорта
//            list.Cells[PassportIDString, PassportIDColumn] = passportID;
//            list.Cells[PassportIDString, PassportIDColumn].HorizontalAlignment = Excel.Constants.xlCenter;
//            list.Cells[PassportIDString, PassportIDColumn].Font.Bold = true;
//            //дата выдачи паспорта
//            list.Cells[PassportDateString, PassportDateColumn] = passportDate;
//            list.Cells[PassportDateString, PassportDateColumn].HorizontalAlignment = Excel.Constants.xlLeft;
//            list.Cells[PassportDateString, PassportDateColumn].Font.Bold = true;
//            //кем выдан паспорт
//            list.Cells[PassportIssueByString, PassportIssueByColumn] = passportIssueBy;
//            list.Cells[PassportIssueByString, PassportIssueByColumn].HorizontalAlignment = Excel.Constants.xlLeft;
//            list.Cells[PassportIssueByString, PassportIssueByColumn].Font.Bold = true;
//        }
//    }
//}
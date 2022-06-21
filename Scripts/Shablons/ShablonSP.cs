using System.Linq;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace JudicialTest
{
    public static class ShablonSP
    {
        public static int WhichJudgeString = 1;
        public static string WhichJudgeColumn = "H";
        public static int JudicialSectorString = 2;
        public static string JudicialSectorColumn = "H";
        public static int AddressJudgeString = 3;
        public static string AddressJudgeColumn = "H";

        public static int FullNameString1 = 11;
        public static string FullNameColumn1 = "I";
        public static int FullNameString2 = 31;
        public static string FullNameColumn2 = "B";
        public static int FullNameString3 = 68;
        public static string FullNameColumn3 = "H";

        public static int BirthDateString = 12;
        public static string BirthDateColumn = "J";

        public static int BirthPlaceString1 = 13;
        public static string BirthPlaceColumn1 = "J";
        public static int BirthPlaceString2 = 14;
        public static string BirthPlaceColumn2 = "H";

        public static int PassportIDString = 15;
        public static string PassportIDColumn = "J";

        public static int AddressEstateString1 = 16;
        public static string AddressEstateColumn1 = "I";
        public static int AddressEstateString2 = 17;
        public static string AddressEstateColumn2 = "H";

        public static int EstateTypeString = 32;
        public static string EstateTypeColumn = "A";
        public static int EstateNumberString = 32;
        public static string EstateNumberColumn = "B";
        public static int EstateType_NumberString = 17;
        public static string EstateType_NumberColumn = "L";
        public static int EstateLivingString = 31; // жилое/не жилое
        public static string EstateLivingColumn = "K";

        public static int EGRNString = 33;
        public static string EGRNColumn = "A";
        public static int EGRNShareOfOwnershipString = 33; //доля в собственности
        public static string EGRNShareOfOwnershipColumn = "H"; //доля в собственности

        public static int PeriodStartString1 = 43; //начало периода
        public static string PeriodStartColumn1 = "D";
        public static int PeriodEndString1 = 43; //конец периода
        public static string PeriodEndColumn1 = "G";
        public static int PeriodStartString2 = 70; //начало периода
        public static string PeriodStartColumn2 = "C";
        public static int PeriodEndString2 = 70; //конец периода
        public static string PeriodEndColumn2 = "F";

        public static int DutyStateString1 = 20;//госпошлина
        public static string DutyStateColumn1 = "K";
        public static int DutyStateString2 = 71;//госпошлина
        public static string DutyStateColumn2 = "K";

        public static int DutyPeriodString1 = 44;//долг за период
        public static string DutyPeriodColumn1 = "H";
        public static int DutyPeriodString2 = 70;//долг за период
        public static string DutyPeriodColumn2 = "I";

        public static int DutyPenyString1 = 56; //пени
        public static string DutyPenyColumn1 = "A";
        public static int DutyPenyString2 = 71; //пени
        public static string DutyPenyColumn2 = "C";

        public static int DutyGeneralString = 19;//пени + долг за период
        public static string DutyGeneralColumn = "K";

        public static void CreateCopySP(Arrear arrear)
        {
            AsyncSP(arrear);
        }

        private static void StartAsync()
        {
            MainWindow.Instance.Button_CreateSP.IsEnabled = false;
            MainWindow.Instance.ProgressGrid.Visibility = System.Windows.Visibility.Visible;
        }
        private static void EndAsync()
        {
            MainWindow.Instance.Button_CreateSP.IsEnabled = true;
            MainWindow.Instance.ProgressGrid.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.Instance.ProgressInfo.Value = 0;
        }

        private async static void AsyncSP(Arrear arrear)
        {
            StartAsync();
            SettingsVM.Instance.PathList.TryGetValue(PathEnum.NameFileShablonSP, out string pathExcel);

            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWB;
            Excel.Worksheet list;
            xlWB = xlApp.Workbooks.Open(pathExcel); //открытие файла, расположенного по пути
            list = xlWB.Worksheets["Лист1"]; //открытие листа в этом файле    

            MainWindow.Instance.ProgressInfo.Value = 20;
            await Task.Run(() => FillingEGRN_Address(list, arrear));
            MainWindow.Instance.ProgressInfo.Value = 40;
            await Task.Run(() => FillingPeriod_Duty(list, arrear));
            MainWindow.Instance.ProgressInfo.Value = 60;
            await Task.Run(() => FillingName_Estate(list, arrear));
            MainWindow.Instance.ProgressInfo.Value = 80;
            await Task.Run(() => FillingPassportData(list, arrear));
            MainWindow.Instance.ProgressInfo.Value = 100;

            string fullName = arrear.Debtor.LastName + " " + arrear.Debtor.FirstName + " " + arrear.Debtor.MiddleName;
            xlWB.SaveCopyAs(GeneratedName(arrear.Apartment.EstateNumber, fullName, arrear.NumberArrear));
            xlWB.Saved = true;
            xlApp.Workbooks.Close();
            xlApp.Quit();
            MainWindow.Instance.ProgressInfo.Value = 100;
            EndAsync();
        }



        public static string GeneratedName(string number, string name, int counterArrears)
        {
            SettingsVM.Instance.PathList.TryGetValue(PathEnum.NameFolderResultSP, out string path);
            var str = name.Split(' ');
            string name1 = "\\" + number + "_" + str[0] + "_СП" + counterArrears + ".xlsx";
            return path + name1;
        }
        public static void FillingEGRN_Address(Excel.Worksheet list, Arrear arrear)
        {
            string whichJudge = arrear.Debtor.SectorAddress.GetFullAddress();//SectorFullAddress;
            //string judicialSector = ;
            //string addressJudge = person.AddressJudge;


            string addressEstate1 = arrear.Apartment.House.GetIndex_City();
            string addressEstate2 = arrear.Apartment.House.GetStreet_House();


            string egrn = arrear.Apartment.Extracts.FirstOrDefault(item => item.Debtor.DebtorID == arrear.Debtor.DebtorID)?.NumberExtractEGRN;
            if(egrn == null)
                egrn = string.Empty;
            
            //string egrnShareOfOwnership = person.EGRNShareOfOwnership;
            //if (person.EGRNShareOfOwnership == string.Empty)
            //{
            //    egrn += ".";
            //}

            //какому судье
            list.Cells[WhichJudgeString, WhichJudgeColumn] = whichJudge;
            list.Cells[WhichJudgeString, WhichJudgeColumn].HorizontalAlignment = Excel.Constants.xlRight;
            list.Cells[WhichJudgeString, WhichJudgeColumn].Font.Bold = true;
            ////судебный участок
            //list.Cells[JudicialSectorString, JudicialSectorColumn] = judicialSector;
            //list.Cells[JudicialSectorString, JudicialSectorColumn].HorizontalAlignment = Excel.Constants.xlRight;
            //list.Cells[JudicialSectorString, JudicialSectorColumn].Font.Bold = true;
            ////адрес судьи
            //list.Cells[AddressJudgeString, AddressJudgeColumn] = addressJudge;
            //list.Cells[AddressJudgeString, AddressJudgeColumn].HorizontalAlignment = Excel.Constants.xlRight;

            //адрес должника
            list.Cells[AddressEstateString1, AddressEstateColumn1] = addressEstate1;
            list.Cells[AddressEstateString1, AddressEstateColumn1].HorizontalAlignment = Excel.Constants.xlRight;

            list.Cells[AddressEstateString2, AddressEstateColumn2] = addressEstate2;
            list.Cells[AddressEstateString2, AddressEstateColumn2].HorizontalAlignment = Excel.Constants.xlRight;
            //егрн
            list.Cells[EGRNString, EGRNColumn] = egrn;
            list.Cells[EGRNString, EGRNColumn].HorizontalAlignment = Excel.Constants.xlLeft;
            list.Cells[EGRNString, EGRNColumn].Font.Bold = true;
            //доля собственности егрн
            //list.Cells[EGRNShareOfOwnershipString, EGRNShareOfOwnershipColumn] = egrnShareOfOwnership;
            //list.Cells[EGRNShareOfOwnershipString, EGRNShareOfOwnershipColumn].HorizontalAlignment = Excel.Constants.xlLeft;


        }
        public static void FillingPeriod_Duty(Excel.Worksheet list, Arrear arrear)
        {
            string periodStart = PickerEntities.GetDateString(arrear.StartDate);
            string periodEnd = PickerEntities.GetDateString(arrear.EndDate);

            float dutyState = arrear.DutyGovernment;
            string dutyPeny = arrear.DutyPeny + " руб.";
            float dutyPeriod = arrear.DutyPeriod;
            float dutyGeneral = arrear.DutyPeny + arrear.DutyPeriod;

            //период
            //начало
            list.Cells[PeriodStartString1, PeriodStartColumn1] = periodStart;
            list.Cells[PeriodStartString1, PeriodStartColumn1].HorizontalAlignment = Excel.Constants.xlCenter;
            list.Cells[PeriodStartString1, PeriodStartColumn1].Font.Bold = true;
            list.Cells[PeriodStartString2, PeriodStartColumn2] = periodStart;
            list.Cells[PeriodStartString2, PeriodStartColumn2].HorizontalAlignment = Excel.Constants.xlCenter;
            list.Cells[PeriodStartString2, PeriodStartColumn2].Font.Bold = true;
            //конец
            list.Cells[PeriodEndString1, PeriodEndColumn1] = periodEnd;
            list.Cells[PeriodEndString1, PeriodEndColumn1].HorizontalAlignment = Excel.Constants.xlCenter;
            list.Cells[PeriodEndString1, PeriodEndColumn1].Font.Bold = true;
            list.Cells[PeriodEndString2, PeriodEndColumn2] = periodEnd;
            list.Cells[PeriodEndString2, PeriodEndColumn2].HorizontalAlignment = Excel.Constants.xlCenter;
            list.Cells[PeriodEndString2, PeriodEndColumn2].Font.Bold = true;
            //госпошлина
            list.Cells[DutyStateString1, DutyStateColumn1] = dutyState.ToString();
            list.Cells[DutyStateString1, DutyStateColumn1].HorizontalAlignment = Excel.Constants.xlRight;
            list.Cells[DutyStateString1, DutyStateColumn1].Font.Bold = true;
            list.Cells[DutyStateString2, DutyStateColumn2] = dutyState.ToString();
            list.Cells[DutyStateString2, DutyStateColumn2].HorizontalAlignment = Excel.Constants.xlCenter;
            list.Cells[DutyStateString2, DutyStateColumn2].Font.Bold = true;
            //долг за период
            list.Cells[DutyPeriodString1, DutyPeriodColumn1] = dutyPeriod.ToString();
            list.Cells[DutyPeriodString1, DutyPeriodColumn1].HorizontalAlignment = Excel.Constants.xlCenter;
            list.Cells[DutyPeriodString1, DutyPeriodColumn1].Font.Bold = true;
            list.Cells[DutyPeriodString2, DutyPeriodColumn2] = dutyPeriod.ToString();
            list.Cells[DutyPeriodString2, DutyPeriodColumn2].HorizontalAlignment = Excel.Constants.xlCenter;
            list.Cells[DutyPeriodString2, DutyPeriodColumn2].Font.Bold = true;
            //пени
            list.Cells[DutyPenyString1, DutyPenyColumn1] = dutyPeny.ToString();
            list.Cells[DutyPenyString1, DutyPenyColumn1].HorizontalAlignment = Excel.Constants.xlLeft;
            list.Cells[DutyPenyString1, DutyPenyColumn1].Font.Bold = true;
            list.Cells[DutyPenyString2, DutyPenyColumn2] = dutyPeny.ToString();
            list.Cells[DutyPenyString2, DutyPenyColumn2].HorizontalAlignment = Excel.Constants.xlCenter;
            list.Cells[DutyPenyString2, DutyPenyColumn2].Font.Bold = true;
            //долг за период + пени
            list.Cells[DutyGeneralString, DutyGeneralColumn] = dutyGeneral.ToString();
            list.Cells[DutyGeneralString, DutyGeneralColumn].HorizontalAlignment = Excel.Constants.xlRight;
            list.Cells[DutyGeneralString, DutyGeneralColumn].Font.Bold = true;
        }
        public static void FillingName_Estate(Excel.Worksheet list, Arrear arrear)
        {
            string fullName = arrear.Debtor.LastName + " " + arrear.Debtor.FirstName + " " + arrear.Debtor.MiddleName;
            string estateNumber = arrear.Apartment.EstateNumber;
            string estateType;
            string estateType_Number;
            string estateLiving;
            if (arrear.Apartment.Type == Dictionaries.Instance.GetEstateType(EstateTypeEnum.Kvartira))
            {
                estateType = "кв.";
                estateType_Number = "кв. " + estateNumber;
                estateLiving = "жилого";
            }
            else
            {
                estateType = "пом.";
                estateType_Number = "пом. " + estateNumber;
                estateLiving = "нежилого";
            }
            //ФИО должника
            list.Cells[FullNameString1, FullNameColumn1] = fullName;
            list.Cells[FullNameString1, FullNameColumn1].HorizontalAlignment = Excel.Constants.xlRight;
            list.Cells[FullNameString1, FullNameColumn1].Font.Bold = true;

            list.Cells[FullNameString2, FullNameColumn2] = fullName + ",";
            list.Cells[FullNameString2, FullNameColumn2].HorizontalAlignment = Excel.Constants.xlCenter;
            list.Cells[FullNameString2, FullNameColumn2].Font.Bold = true;

            list.Cells[FullNameString3, FullNameColumn3] = fullName;
            list.Cells[FullNameString3, FullNameColumn3].HorizontalAlignment = Excel.Constants.xlCenter;
            list.Cells[FullNameString3, FullNameColumn3].Font.Bold = true;

            //тип имущества
            list.Cells[EstateTypeString, EstateTypeColumn] = estateType;
            list.Cells[EstateTypeString, EstateTypeColumn].HorizontalAlignment = Excel.Constants.xlLeft;
            list.Cells[EstateTypeString, EstateTypeColumn].Font.Bold = true;
            //номер имущества
            list.Cells[EstateNumberString, EstateNumberColumn] = estateNumber;
            list.Cells[EstateNumberString, EstateNumberColumn].HorizontalAlignment = Excel.Constants.xlLeft;
            list.Cells[EstateNumberString, EstateNumberColumn].Font.Bold = true;
            //тип + номер
            list.Cells[EstateType_NumberString, EstateType_NumberColumn] = estateType_Number;
            list.Cells[EstateType_NumberString, EstateType_NumberColumn].HorizontalAlignment = Excel.Constants.xlCenter;
            list.Cells[EstateType_NumberString, EstateType_NumberColumn].Font.Bold = true;
            //жилое / нежилое
            list.Cells[EstateLivingString, EstateLivingColumn] = estateLiving;
            list.Cells[EstateLivingString, EstateLivingColumn].HorizontalAlignment = Excel.Constants.xlCenter;
            list.Cells[EstateLivingString, EstateLivingColumn].Font.Bold = true;
        }
        public static void FillingPassportData(Excel.Worksheet list, Arrear arrear)
        {
            string birthDate =  arrear.Debtor.BirthDate;
            string birthPlace = arrear.Debtor.BirthPlace;
            string PassportNumber = arrear.Debtor.PassportSerial + " " + arrear.Debtor.PassportNumber;

            //день рождения
            list.Cells[BirthDateString, BirthDateColumn] = birthDate;
            list.Cells[BirthDateString, BirthDateColumn].HorizontalAlignment = Excel.Constants.xlRight;
            list.Cells[BirthDateString, BirthDateColumn].Font.Bold = true;
            //место рождения
            if (list.Cells[BirthPlaceString1, BirthPlaceColumn1].ShrinkToFit == true)
            {
                list.Cells[BirthPlaceString1, BirthPlaceColumn1] = birthPlace;
                list.Cells[BirthPlaceString1, BirthPlaceColumn1].HorizontalAlignment = Excel.Constants.xlRight;
                list.Cells[BirthPlaceString1, BirthPlaceColumn1].Font.Bold = true;
            }
            else
            {
                list.Cells[BirthPlaceString2, BirthPlaceColumn2] = birthPlace;
                list.Cells[BirthPlaceString2, BirthPlaceColumn2].HorizontalAlignment = Excel.Constants.xlRight;
                list.Cells[BirthPlaceString2, BirthPlaceColumn2].Font.Bold = true;
            }

            //номер паспорта
            list.Cells[PassportIDString, PassportIDColumn] = PassportNumber;
            list.Cells[PassportIDString, PassportIDColumn].HorizontalAlignment = Excel.Constants.xlRight;
            list.Cells[PassportIDString, PassportIDColumn].Font.Bold = true;
        }
    }
}
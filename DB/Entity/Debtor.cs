using System;
using System.Collections.Generic;

namespace JudicialTest
{
    public class Debtor : Entity
    {
        public Guid DebtorID { get; set; } //ID должника в БД
        public virtual ICollection<Apartment> Apartments { get; set; }
        public virtual ICollection<Arrear> Arrears { get; set; }
        public virtual ICollection<Extract> Extracts { get; set; }

        public string LastName { get; set; } //Фамилия должника
        public string FirstName { get; set; } //Имя должника       
        public string MiddleName { get; set; } //Отчество должника
        public string Gender { get; set; } //Пол должника
        public string BirthDate { get; set; } //дата рождения
        public string BirthPlace { get; set; } //место рождения
        public string PassportSerial { get; set; } //серия паспорта //маска - 5 символов "XX XX" X - цифры
        public string PassportNumber { get; set; } //номер паспорта //маска - "XXXXXX" 6 цифр
        public string PassportDate { get; set; } //дата выдачи паспорта
        public string PassportIssueBy { get; set; } //кем выдан паспорт
        public string Phone { get; set; } //Телефон
        public string Email { get; set; } //Электронная почта
        public EstateAddress RegisterAddress { get; set; } //Адрес регистрации
        public EstateAddress LocationAddress { get; set; } //Адрес места жительства
        public string SNILS { get; set; } //номер СНИЛС маска - "XXX-XXX-XXX YY" (X, Y - цифры)
        public string INN { get; set; } //номер ИНН маска - "XXXXXXXXXXXX" (12 цифр) 
        public string SeriesDriverLicense { get; set; } //маска - 5 символов "XX XX" X - цифры
        public string NumberDriverLicense { get; set; } //маска - "XXXXXX" 6 цифр
        public string SeriesSTS { get; set; } //маска - 5 символов "XX YY" X - цифры, Y - буквы
        public string NumberSTS { get; set; } //маска - "XXXXXX" 6 цифр
        public string SectorNumber { get; set; }
        public EstateAddress SectorAddress { get; set; } //Мировому судье судебного участка №... "ФИО"

        public Debtor()
        {            
            InitBaseProperties();

            Apartments = new List<Apartment>();
            Arrears = new List<Arrear>();
            Extracts = new List<Extract>();

            LastName = string.Empty;
            FirstName = string.Empty;
            MiddleName = string.Empty;
            Gender = Dictionaries.Instance.GetGenderType(GenderTypeEnum.Male);
            BirthDate = string.Empty;
            BirthPlace = string.Empty;
            PassportSerial = string.Empty;
            PassportNumber = string.Empty;
            PassportDate = string.Empty;
            PassportIssueBy = string.Empty;
            Phone = string.Empty;
            Email = string.Empty;
            RegisterAddress = new EstateAddress();
            LocationAddress = new EstateAddress();
            SNILS = string.Empty;
            INN = string.Empty;
            SeriesDriverLicense = string.Empty;
            NumberDriverLicense = string.Empty;
            SeriesSTS = string.Empty;
            NumberSTS = string.Empty;
            SectorNumber = string.Empty;
            SectorAddress = new EstateAddress();
        }
    }
}

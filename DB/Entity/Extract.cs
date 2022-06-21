using System;

namespace JudicialTest
{
    public class Extract : Entity
    {
        public Guid ExtractID { get; set; } //уникальный ключ в таблице БД
        public Debtor Debtor { get; set; } //ссылка на должника в БД
        public Apartment Apartment { get; set; } //ссылка на дом в БД

        public string NumberExtractEGRN { get; set; } //Номер выписки ЕГРН
        public string LastNameOwner { get; set; } //Фамилия владельца
        public string FirstNameOwner { get; set; } //Имя владельца       
        public string MiddleNameOwner { get; set; } //Отчество владельца
        public string NumberApartment { get; set; } //Номер владения

        public Extract()
        {
            InitBaseProperties();
            NumberExtractEGRN = string.Empty;
            LastNameOwner = string.Empty;
            FirstNameOwner = string.Empty;       
            MiddleNameOwner = string.Empty; 
            NumberApartment = string.Empty;
        }
    }
}

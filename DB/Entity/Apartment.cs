using System;
using System.Collections.Generic;

namespace JudicialTest
{
    public class Apartment : Entity
    {
        public Guid ApartmentID { get; set; } //уникальный ключ в таблице БД
        public virtual ICollection<Debtor> Debtors { get; set; }
        public virtual ICollection<Arrear> Arrears { get; set; }
        public virtual ICollection<Extract> Extracts { get; set; }
        public House House { get; set; } //ссылка на дом в бд

        public string Type { get; set; }
        public string EstateNumber { get; set; }

        public Apartment()
        {
            InitBaseProperties();
            Debtors = new List<Debtor>();
            Arrears = new List<Arrear>();
            Extracts = new List<Extract>();

            Type = Dictionaries.Instance.GetEstateType(EstateTypeEnum.Kvartira);
            EstateNumber = string.Empty;
        }
    }
}

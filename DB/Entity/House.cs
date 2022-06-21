using System;
using System.Collections.Generic;

namespace JudicialTest
{
    public class House : Entity
    {
        public Guid HouseID { get; set; } //ID позиции в таблице БД
        public virtual ICollection<Apartment> Apartments { get; set; } // список связанных квартир в доме

        public string Index { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string NumberHouse { get; set; }
        public string Corpus { get; set; }

        public House()
        {
            InitBaseProperties();
            Apartments = new List<Apartment>();
            Index = string.Empty;
            City = string.Empty;
            Street = string.Empty;
            NumberHouse = string.Empty;
            Corpus = string.Empty;
        }

        public string GetFullAddress()
        {
            string str = string.Empty;

            if (!string.IsNullOrWhiteSpace(Index))
                str += Index + ", ";
            if (!string.IsNullOrWhiteSpace(City))
                str += "г. " + City + ", ";
            if (!string.IsNullOrWhiteSpace(Street))
                str += Street + ", ";
            if (!string.IsNullOrWhiteSpace(NumberHouse))
                str += "д. " + NumberHouse + ", ";
            if (!string.IsNullOrWhiteSpace(Corpus))
                str += "к. " + Corpus;

            return str;
        }

        public string GetIndex_City()
        {
            string str = string.Empty;

            if (!string.IsNullOrWhiteSpace(Index))
                str += Index + ", ";
            if (!string.IsNullOrWhiteSpace(City))
                str += "г. " + City + ",";

            return str;
        }
        public string GetStreet_House()
        {
            string str = string.Empty;

            if (!string.IsNullOrWhiteSpace(Street))
                str += Street + ", ";
            if (!string.IsNullOrWhiteSpace(NumberHouse))
                str += "д. " + NumberHouse + ", ";
            if (!string.IsNullOrWhiteSpace(Corpus))
                str += "к. " + Corpus;

            return str;
        }
    }
}

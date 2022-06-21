using System;

namespace JudicialTest
{
    public class EstateAddress
    {
        public Guid EstateAddressID { get; set; }
        public Debtor DebtorReg { get; set; }
        public Debtor DebtorLoc { get; set; }
        public Debtor DebtorSec { get; set; }

        public string Index { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string NumberHouse { get; set; }
        public string Corpus { get; set; }
        public string NumberApartment { get; set; }

        public EstateAddress()
        {
            Index = string.Empty;
            City = string.Empty;
            Street = string.Empty;
            NumberHouse = string.Empty;
            Corpus = string.Empty;
            NumberApartment = string.Empty;
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
                str += "к. " + Corpus + ", ";
            if (!string.IsNullOrWhiteSpace(NumberApartment))
                str += NumberApartment;

            return str;
        }
    }
}

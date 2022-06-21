namespace JudicialTest
{
    public class ArrearExceptions
    {
        public Arrear Arrear { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }

        public ArrearExceptions(Arrear arrear, int year, int month)
        {
            Arrear = arrear;
            Year = year;
            Month = month;
        }
    }
}

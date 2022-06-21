using System;

namespace JudicialTest
{
    public class Arrear : Entity
    {
        public Guid ArrearID { get; set; } //уникальный ключ в таблице БД
        public Debtor Debtor { get; set; } //ссылка на должника в БД
        public Apartment Apartment { get; set; } //ссылка на квартиру в БД

        public PeriodDateArrear StartDate { get; set; }
        public PeriodDateArrear EndDate { get; set; }

        public float DutyPeriod { get; set; } //штраф за период 
        public float DutyGovernment { get; set; } //госпошлина
        public float DutyPeny { get; set; } //пени
        public float DutyDelegateServices { get; set; } //услуги представителя

        public int NumberArrear { get; set; } //номер задолженности

        public string LifeCycle { get; set; } //жизненный цикл (на каком этапе находится задолженность)

        public Arrear()
        {
            InitBaseProperties();

            DutyPeriod = 0f;
            DutyGovernment = 0f;
            DutyPeny = 0f;
            DutyDelegateServices = 0f;
            LifeCycle = Dictionaries.Instance.GetLifeCycleArrear(LifeCycleArrearEnum.Created);
            NumberArrear = 0;
        }
    }
}

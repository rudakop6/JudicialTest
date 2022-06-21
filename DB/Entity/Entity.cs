using System;

namespace JudicialTest
{
    public class Entity : ICloneable
    {
        public StatusPickedEnum PickStatus { get; set; } //статус в базе
        public StatusEntityEnum Status { get; set; } //статус в базе
        public DateTime DateCreate { get; set; } //дата создания
        public DateTime DateLastEdit { get; set; } //дата последнего изменения

        public object Clone()
        {
            return MemberwiseClone();
        }
        public void InitBaseProperties()
        {
            Status = StatusEntityEnum.NewNoSave;
            DateCreate = DateTime.Now;
            DateLastEdit = DateTime.Now;
            PickStatus = StatusPickedEnum.NotPicked;
        }
    }
}

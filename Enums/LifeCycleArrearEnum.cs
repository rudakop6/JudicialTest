namespace JudicialTest
{
    public enum LifeCycleArrearEnum
    {
        Created = 0, //Создано
        SendedDocCourtOrder = 1, //Отправлено заявление на вынесение судебного приказа
        RecievedCourtOrder = 2, //Получен судебный приказ
        IssuedJudicialDecision = 3, //Вынесено судебное решение
        RecievedExecutiveDoc = 4, //Получен исполнительный документ
        SendedDocExecutionProcess = 5, //Отправлено заявление на исполнительное производство
        Finished = 6 //Завершена работа с задолженностью
    }
}


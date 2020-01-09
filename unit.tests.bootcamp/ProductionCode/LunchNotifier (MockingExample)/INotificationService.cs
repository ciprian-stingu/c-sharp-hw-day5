namespace ProductionCode.MockingExample
{
    public interface INotificationService
    {
        void SendEmail(IEmployee employee, string templateToUse);
        void SendSlackMessage(IEmployee employee, string templateToUse);
    }
}

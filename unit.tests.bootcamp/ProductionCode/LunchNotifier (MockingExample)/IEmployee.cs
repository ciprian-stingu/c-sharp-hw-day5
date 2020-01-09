namespace ProductionCode.MockingExample
{
    using System;

    public interface IEmployee
    {
        bool IsWorkingOnDate(DateTime date);
        LunchNotifier.NotificationType GetNotificationPreference();
    }
}

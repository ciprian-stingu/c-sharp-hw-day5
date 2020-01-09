namespace ProductionCode.MockingExample
{
    using System;

    public class LunchNotifier
    {
        public enum NotificationType
        {
            Email,
            Slack
        }


        public const string RegularLunchTemplate = "It's Lunchtime, come eat!";
        public const string LateLunchTemplate = "It's Lunchtime -- Sorry it's late!";
        private readonly IEmployeeService _employeeService;
        private readonly ILogger _logger;

        private readonly INotificationService _notificationService;

        public LunchNotifier(INotificationService notifySrv, IEmployeeService employeeSrv, ILogger logger)
        {
            this._notificationService = notifySrv;
            this._employeeService = employeeSrv;
            this._logger = logger;
        }

        /// <summary>
        ///     Determines which employees are currently working in the New York office and
        ///     sends notifications to their preferred notification platform.  If lunch is
        ///     late (after 1pm), a "Late Lunch" notification template is used instead of
        ///     the typical template.
        /// </summary>
        public void SendLunchtimeNotifications()
        {
            var now = DateTime.Now;
            var templateToUse = now.Hour > 12 ? LateLunchTemplate : RegularLunchTemplate;
            this._logger.Write($"Using template: {templateToUse}");

            var nycEmployees = this._employeeService.GetEmployeesInNewYorkOffice();

            foreach (var employee in nycEmployees)
            {
                if (!employee.IsWorkingOnDate(now.Date))
                {
                    // no need to notify employees that are out sick
                    // or on vacation today.
                    this._logger.Debug("Skipping employe {employee}");
                    continue;
                }

                try
                {
                    var notificationType = employee.GetNotificationPreference();
                    switch (notificationType)
                    {
                        case NotificationType.Email:
                            this._notificationService.SendEmail(employee, templateToUse);
                            break;
                        case NotificationType.Slack:
                            this._notificationService.SendSlackMessage(employee, templateToUse);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    this._logger.Error(ex);
                }
            }
        }
    }
}

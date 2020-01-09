namespace ProductionCode.MockingExample
{
    using System;

    public class LunchNotifier_SeamAndExplicitInterface : IDateTimeOverridable
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger _logger;

        private readonly INotificationService _notificationService;

        public LunchNotifier_SeamAndExplicitInterface(INotificationService notifySrv, IEmployeeService employeeSrv,
            ILogger logger)
        {
            this._notificationService = notifySrv;
            this._employeeService = employeeSrv;
            this._logger = logger;
        }


        DateTime IDateTimeOverridable.GetDateTime()
        {
            return DateTime.Now;
        }

        /// <summary>
        ///     This method is identical to SendLunchtimeNotification, except that
        ///     it extracts the use of System.DateTime.Now into a seperate method
        ///     that explicitly implements an interface, so that constrained frameworks
        ///     (Moq, RhinoMocks, NSubstitute) can mock the "current" time, but the
        ///     method remains hidden from typical use.
        /// </summary>
        public void SendLunchtimeNotifications()
        {
            var now = ((IDateTimeOverridable) this).GetDateTime();
            var templateToUse = now.Hour > 12 ? LunchNotifier.LateLunchTemplate : LunchNotifier.RegularLunchTemplate;
            this._logger.Write($"Using template: {templateToUse}");

            var nycEmployees = this._employeeService.GetEmployeesInNewYorkOffice();

            foreach (var employee in nycEmployees)
            {
                if (!employee.IsWorkingOnDate(now.Date))
                {
                    this._logger.Debug("Skipping employe {employee}");
                    continue;
                }

                try
                {
                    var notificationType = employee.GetNotificationPreference();
                    switch (notificationType)
                    {
                        case LunchNotifier.NotificationType.Email:
                            this._notificationService.SendEmail(employee, templateToUse);
                            break;
                        case LunchNotifier.NotificationType.Slack:
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

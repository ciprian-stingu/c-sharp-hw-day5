namespace xUnit.NetCore.Mocks
{
    using Moq;
    using ProductionCode.MockingExample;
    using System;
    using System.Collections.Generic;
    using Xunit;


    public class LunchNotifierTests
    {
        [Theory]
        [InlineData("2017-01-01 13:00:00", LunchNotifier.LateLunchTemplate)]
        [InlineData("2017-01-01 12:59:59", LunchNotifier.RegularLunchTemplate)]
        public void Test_CorrectTemplateIsUsed_LateLunch_Seam(string currentTime, string expectedTemplate)
        {
            //
            // Create mocks:
            //
            var loggerMock = new Mock<ILogger>();

            var bobMock = new Mock<IEmployee>();
            /*
            * Configure mock so that employee is considered working today and gets notifications via email
            *
            */
            bobMock.Setup(o => o.IsWorkingOnDate(It.IsAny<DateTime>())).Returns(true);


            var employeeServiceMock = new Mock<IEmployeeService>();
            /*
            * Configure mock so to return employee from above
            *
            */
            employeeServiceMock.Setup(o => o.GetEmployeesInNewYorkOffice()).Returns(new List<IEmployee> { bobMock.Object });

            var notificationServiceMock = new Mock<INotificationService>();


            //
            // Create instance of class I'm testing:
            //
            Mock<LunchNotifier_UsingSeam> classUnderTest = new Mock<LunchNotifier_UsingSeam>(notificationServiceMock.Object, employeeServiceMock.Object, loggerMock.Object);
            /*
             * Create a partial mock of the LunchNotifier_UsingSeam class and change the GetDateTime() behavior to return DateTime.Parse(currentTime)
             *
             */
            classUnderTest.Setup(o => o.GetDateTime()).Returns(DateTime.Parse(currentTime));

            //
            // Run some logic to test:
            //
            classUnderTest.Object.SendLunchtimeNotifications();

            //
            // Check the results:
            //
            notificationServiceMock.Verify(x => x.SendEmail(It.IsAny<IEmployee>(), expectedTemplate));
        }

        [Fact]
        public void Test_EmployeeInOfficeGetsNotified()
        {
            //
            // Create mocks:
            //
            var loggerMock = new Mock<ILogger>();

            var bobMock = new Mock<IEmployee>();
            /*
             * Configure mock so that employee is considered working today and gets notifications via email
             *
             */
            bobMock.Setup(o => o.IsWorkingOnDate(It.IsAny<DateTime>())).Returns(true);
            bobMock.Setup(o => o.GetNotificationPreference()).Returns(LunchNotifier.NotificationType.Email);

            var employeeServiceMock = new Mock<IEmployeeService>();
            /*
             * Configure mock so to return employee from above
             *
             */
            employeeServiceMock.Setup(o => o.GetEmployeesInNewYorkOffice()).Returns(new List<IEmployee> { bobMock.Object });


            var notificationServiceMock = new Mock<INotificationService>();
            /*
            * Configure mock so that you can verify a notification was sent via email
            *
            */
            var emailCalls = 0;
            notificationServiceMock.Setup(o => o.SendEmail(It.IsAny<IEmployee>(), It.IsAny<string>())).Callback(() => emailCalls++);

            //
            // Create instance of class I'm testing:
            //
            var classUnderTest = new LunchNotifier(notificationServiceMock.Object, employeeServiceMock.Object, loggerMock.Object);

            //
            // Run some logic to test:
            //
            classUnderTest.SendLunchtimeNotifications();

            //
            // Check the results:
            //

            /*
            * Add verifications to prove emails notification was sent
            *
            */
            notificationServiceMock.Verify(o => o.SendEmail(It.IsAny<IEmployee>(), It.IsAny<string>()), Times.Once);
            Assert.Equal(1, emailCalls);
        }


        [Fact]
        public void Test_ExceptionDoesNotStopNotifications()
        {
            //
            // Create mocks:
            //
            var loggerMock = new Mock<ILogger>();
            /*
            * Configure mock so that you can verify a error was logged
            *
            */
            var exceptionsLogged = 0;
            loggerMock.Setup(o => o.Error(It.IsAny<Exception>())).Callback(() => exceptionsLogged++);

            var bobMock = new Mock<IEmployee>();
            /*
             * Configure mock so that employee is considered working today and gets notifications via email
             *
             */
            bobMock.Setup(o => o.IsWorkingOnDate(It.IsAny<DateTime>())).Returns(true);
            bobMock.Setup(o => o.GetNotificationPreference()).Returns(LunchNotifier.NotificationType.Email);

            var marthaMock = new Mock<IEmployee>();
            /*
             * Configure mock so that employee is considered working today and gets notifications via email
             *
             */
            marthaMock.Setup(o => o.IsWorkingOnDate(It.IsAny<DateTime>())).Returns(true);
            marthaMock.Setup(o => o.GetNotificationPreference()).Returns(LunchNotifier.NotificationType.Email);


            var employeeServiceMock = new Mock<IEmployeeService>();
            /*
             * Configure mock so to return both employees from above
             *
             */
            employeeServiceMock.Setup(o => o.GetEmployeesInNewYorkOffice()).Returns(new List<IEmployee> { bobMock.Object, marthaMock.Object });

            var notificationServiceMock = new Mock<INotificationService>();
            /*
             * Configure mock to throw an exception when attempting to send notification via email
             *
             */
            notificationServiceMock.Setup(o => o.SendEmail(It.IsAny<IEmployee>(), It.IsAny<string>())).Throws(new Exception("Email service not valid."));

            //
            // Create instance of class I'm testing:
            //
            var classUnderTest = new LunchNotifier(notificationServiceMock.Object, employeeServiceMock.Object, loggerMock.Object);

            //
            // Run some logic to test:
            //
            classUnderTest.SendLunchtimeNotifications();

            //
            // Check the results:
            //

            /*
             * Add verifications to prove emails notification were attempted twice
             *
             * Add verification that error logger was called
             *
             */
            notificationServiceMock.Verify(o => o.SendEmail(It.IsAny<IEmployee>(), It.IsAny<string>()), Times.Exactly(2));
            Assert.Equal(2, exceptionsLogged);
        }
    }
}

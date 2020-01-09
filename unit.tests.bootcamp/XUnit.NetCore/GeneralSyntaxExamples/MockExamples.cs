namespace xUnit.NetCore
{
    using System;
    using Moq;
    using ProductionCode.MockingExample;
    using Xunit;

    public class MockExamples
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
            bobMock.Setup(x => x.IsWorkingOnDate(It.IsAny<DateTime>()))
                .Returns(true);

            var employeeServiceMock = new Mock<IEmployeeService>();
            employeeServiceMock.Setup(x => x.GetEmployeesInNewYorkOffice())
                .Returns(new[] {bobMock.Object});

            var notificationServiceMock = new Mock<INotificationService>();


            //
            // Create instance of class I'm testing:
            //
            var classUnderTest = new Mock<LunchNotifier_UsingSeam>(notificationServiceMock.Object,
                    employeeServiceMock.Object, loggerMock.Object)
                {CallBase = true};
            classUnderTest.Setup(x => x.GetDateTime())
                .Returns(DateTime.Parse(currentTime));

            //
            // Run some logic to test:
            //
            classUnderTest.Object.SendLunchtimeNotifications();

            //
            // Check the results:
            //
            notificationServiceMock.Verify(x => x.SendEmail(It.IsAny<IEmployee>(), expectedTemplate));
        }


        [Theory]
        [InlineData("2017-01-01 13:00:00", LunchNotifier.LateLunchTemplate)]
        [InlineData("2017-01-01 12:59:59", LunchNotifier.RegularLunchTemplate)]
        public void Test_CorrectTemplateIsUsed_LateLunch_ExplicitInterface(string currentTime, string expectedTemplate)
        {
            //
            // Create mocks:
            //
            var loggerMock = new Mock<ILogger>();

            var bobMock = new Mock<IEmployee>();
            bobMock.Setup(x => x.IsWorkingOnDate(It.IsAny<DateTime>()))
                .Returns(true);

            var employeeServiceMock = new Mock<IEmployeeService>();
            employeeServiceMock.Setup(x => x.GetEmployeesInNewYorkOffice())
                .Returns(new[] {bobMock.Object});

            var notificationServiceMock = new Mock<INotificationService>();


            //
            // Create instance of class I'm testing:
            //
            var classUnderTest = new Mock<LunchNotifier_SeamAndExplicitInterface>(notificationServiceMock.Object,
                    employeeServiceMock.Object, loggerMock.Object)
                {CallBase = true};
            classUnderTest.As<IDateTimeOverridable>().Setup(x => x.GetDateTime())
                .Returns(DateTime.Parse(currentTime));

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
            bobMock.Setup(x => x.IsWorkingOnDate(It.IsAny<DateTime>()))
                .Returns(true);
            bobMock.Setup(x => x.GetNotificationPreference())
                .Returns(LunchNotifier.NotificationType.Email);

            var employeeServiceMock = new Mock<IEmployeeService>();
            employeeServiceMock.Setup(x => x.GetEmployeesInNewYorkOffice())
                .Returns(new[] {bobMock.Object});

            var notificationServiceMock = new Mock<INotificationService>();
            notificationServiceMock.Setup(x => x.SendEmail(bobMock.Object, It.IsAny<string>()));

            //
            // Create instance of class I'm testing:
            //
            var classUnderTest = new LunchNotifier(notificationServiceMock.Object, employeeServiceMock.Object,
                loggerMock.Object);

            //
            // Run some logic to test:
            //
            classUnderTest.SendLunchtimeNotifications();

            //
            // Check the results:
            //
            notificationServiceMock.Verify(x => x.SendEmail(bobMock.Object, It.IsAny<string>()), Times.Once);
            notificationServiceMock.Verify(x => x.SendSlackMessage(bobMock.Object, It.IsAny<string>()), Times.Never);
        }


        [Fact]
        public void Test_EmployeeInOfficeGetsNotified_UsingStrictMock()
        {
            //
            // Create mocks:
            //
            var loggerMock = new Mock<ILogger>();

            var bobMock = new Mock<IEmployee>();
            bobMock.Setup(x => x.IsWorkingOnDate(It.IsAny<DateTime>()))
                .Returns(true);
            bobMock.Setup(x => x.GetNotificationPreference())
                .Returns(LunchNotifier.NotificationType.Email);

            var employeeServiceMock = new Mock<IEmployeeService>();
            employeeServiceMock.Setup(x => x.GetEmployeesInNewYorkOffice())
                .Returns(new[] {bobMock.Object});

            var notificationServiceMock = new Mock<INotificationService>(MockBehavior.Strict);
            notificationServiceMock.Setup(x => x.SendEmail(bobMock.Object, It.IsAny<string>()))
                .Verifiable();

            //
            // Create instance of class I'm testing:
            //
            var classUnderTest = new LunchNotifier(notificationServiceMock.Object, employeeServiceMock.Object,
                loggerMock.Object);

            //
            // Run some logic to test:
            //
            classUnderTest.SendLunchtimeNotifications();

            //
            // Check the results:
            //

            notificationServiceMock.Verify();
        }


        [Fact]
        public void Test_ExceptionDoesNotStopNotifications()
        {
            //
            // Create mocks:
            //
            var loggerMock = new Mock<ILogger>();
            loggerMock.Setup(x => x.Error(It.IsAny<Exception>()));

            var bobMock = new Mock<IEmployee>();
            bobMock.Setup(x => x.IsWorkingOnDate(It.IsAny<DateTime>()))
                .Returns(true);
            bobMock.Setup(x => x.GetNotificationPreference())
                .Returns(LunchNotifier.NotificationType.Email);

            var marthaMock = new Mock<IEmployee>();
            marthaMock.Setup(x => x.IsWorkingOnDate(It.IsAny<DateTime>()))
                .Returns(true);
            marthaMock.Setup(x => x.GetNotificationPreference())
                .Returns(LunchNotifier.NotificationType.Email);

            DateTime? incomingDate;
            marthaMock.Setup(x => x.IsWorkingOnDate(It.IsAny<DateTime>()))
                .Returns((Func<DateTime, bool>) (input =>
                {
                    incomingDate = input;
                    return input.DayOfWeek != DayOfWeek.Sunday;
                }));

            var employeeServiceMock = new Mock<IEmployeeService>();
            employeeServiceMock.Setup(x => x.GetEmployeesInNewYorkOffice())
                .Returns(new[] {bobMock.Object, marthaMock.Object});


            var notificationServiceMock = new Mock<INotificationService>(MockBehavior.Strict);
            notificationServiceMock
                .Setup(x => x.SendEmail(It.IsAny<IEmployee>(), It.IsAny<string>()))
                .Throws<Exception>();


            //
            // Create instance of class I'm testing:
            //
            var classUnderTest = new LunchNotifier(notificationServiceMock.Object, employeeServiceMock.Object,
                loggerMock.Object);

            //
            // Run some logic to test:
            //
            classUnderTest.SendLunchtimeNotifications();

            //
            // Check the results:
            //

            notificationServiceMock.Verify(x => x.SendEmail(It.IsAny<IEmployee>(), It.IsAny<string>()),
                Times.Exactly(2));

            loggerMock.Verify(x => x.Error(It.IsAny<Exception>()), Times.Exactly(2));
        }

        [Fact]
        public void Test_ExceptionDoesNotStopNotifications_UsingWhenConstraint()
        {
            //
            // Create mocks:
            //
            var loggerMock = new Mock<ILogger>();
            loggerMock.Setup(x => x.Error(It.IsAny<Exception>()));

            var bobMock = new Mock<IEmployee>();
            bobMock.Setup(x => x.IsWorkingOnDate(It.IsAny<DateTime>()))
                .Returns(true);
            bobMock.Setup(x => x.GetNotificationPreference())
                .Returns(LunchNotifier.NotificationType.Email);

            var marthaMock = new Mock<IEmployee>();
            marthaMock.Setup(x => x.IsWorkingOnDate(It.IsAny<DateTime>()))
                .Returns(true);
            marthaMock.Setup(x => x.GetNotificationPreference())
                .Returns(LunchNotifier.NotificationType.Email);

            DateTime? incomingDate;
            marthaMock.Setup(x => x.IsWorkingOnDate(It.IsAny<DateTime>()))
                .Returns((Func<DateTime, bool>) (input =>
                {
                    incomingDate = input;
                    return input.DayOfWeek != DayOfWeek.Sunday;
                }));

            var employeeServiceMock = new Mock<IEmployeeService>();
            employeeServiceMock.Setup(x => x.GetEmployeesInNewYorkOffice())
                .Returns(new[] {bobMock.Object, marthaMock.Object});

            var isFirstCall = true;

            var notificationServiceMock = new Mock<INotificationService>(MockBehavior.Strict);
            notificationServiceMock
                .When(() => isFirstCall)
                .Setup(x => x.SendEmail(It.IsAny<IEmployee>(), It.IsAny<string>()))
                .Callback(() => isFirstCall = false)
                .Throws<Exception>();
            ;

            notificationServiceMock
                .When(() => !isFirstCall)
                .Setup(x => x.SendEmail(It.IsAny<IEmployee>(), It.IsAny<string>()));


            //
            // Create instance of class I'm testing:
            //
            var classUnderTest = new LunchNotifier(notificationServiceMock.Object, employeeServiceMock.Object,
                loggerMock.Object);

            //
            // Run some logic to test:
            //
            classUnderTest.SendLunchtimeNotifications();

            //
            // Check the results:
            //

            notificationServiceMock.Verify(x => x.SendEmail(It.IsAny<IEmployee>(), It.IsAny<string>()),
                Times.Exactly(2));

            loggerMock.Verify(x => x.Error(It.IsAny<Exception>()), Times.Once);
        }
    }
}

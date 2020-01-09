namespace xUnit.NetCore
{
    using System;
    using System.Collections.Generic;
    using ProductionCode;
    using Xunit;

    public class TestExamples
    {
        [Theory]
        [InlineData("-12345", false)]
        [InlineData("123456", false)]
        [InlineData("asdf", false)]
        [InlineData("12345", true)]
        public void TestValidateUsingTestCases(string inputString, bool expectedResult)
        {
            // Act
            var zipCodeValidator = new SimpleZipCodeValidator();
            var actualResult = zipCodeValidator.Validate(inputString);

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData("2018/01/01 12:32:00")]
        [InlineData("2019/11/11 1:32:00")]
        public void TestCaseWithDateTime(string inputDateTimeString)
        {
            var actualDateTime = DateTime.Parse(inputDateTimeString);

            MethodTakingADateTime(actualDateTime);

            void MethodTakingADateTime(DateTime input)
            {
            }
        }


        [Theory]
        [MemberData(nameof(InputStringTestCases))]
        public void TestValidate(string inputString, bool expectedResult)
        {
            // Act
            var zipCodeValidator = new SimpleZipCodeValidator();
            var actualResult = zipCodeValidator.Validate(inputString);

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }

        public static IEnumerable<object[]> InputStringTestCases()
        {
            yield return new object[] {"-12345", false};
            yield return new object[] {"123456", false};
            yield return new object[] {"asdf", false};
            yield return new object[] {"12345", true};
        }

        [Fact]
        public void TestValidateWithAlphaCharsReturnsFalse()
        {
            // Arrange
            var inputString = "asdf";
            var expectedResult = false;

            // Act
            var zipCodeValidator = new SimpleZipCodeValidator();
            var actualResult = zipCodeValidator.Validate(inputString);

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }


        [Fact]
        public void TestValidateWithNegativeNumberReturnsFalse()
        {
            // Arrange
            var inputString = "-12345";
            var expectedResult = false;

            // Act
            var zipCodeValidator = new SimpleZipCodeValidator();
            var actualResult = zipCodeValidator.Validate(inputString);

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }


        [Fact]
        public void TestValidateWithSixCharStringReturnsFalse()
        {
            // Arrange
            var inputString = "123456";
            var expectedResult = false;

            // Act
            var zipCodeValidator = new SimpleZipCodeValidator();
            var actualResult = zipCodeValidator.Validate(inputString);

            // Assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void TestValidateWithValidCodeReturnsTrue()
        {
            //
            // Arrange
            //
            var inputString = "12345";
            var expectedResult = true;

            //
            // Act
            //
            var zipCodeValidator = new SimpleZipCodeValidator();
            var actualResult = zipCodeValidator.Validate(inputString);

            //
            // Assert
            //
            Assert.Equal(expectedResult, actualResult);
        }
    }
}

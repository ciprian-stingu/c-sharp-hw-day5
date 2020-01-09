namespace xUnit.NetCore
{
    using System;
    using Xunit;

    /* no need for a class-level assembly */
    public class SyntaxExamples_NoSharedData : IDisposable
    {
        /* Per-test setup method is not supported, BUT you can use a paramerter-less constructor for the same purpose.
         * "We believe that use of [SetUp] is generally bad. However, you can implement a parameterless constructor as a direct replacement."
         *
         * From: https://xunit.github.io/docs/comparisons#note2
         * "The xUnit.net team feels that per-test setup and teardown creates difficult-to-follow and debug testing code,
         *  often causing unnecessary code to run before every single test is run.
         *  For more information, see http://jamesnewkirk.typepad.com/posts/2007/09/why-you-should-.html."
         */
        //public void PerTestSetup(){}

        /* Per-test cleanup method is not supported, BUT you can use the IDisposable interface for the same purpose.
        * "We believe that use of [TearDown] is generally bad. However, you can implement IDisposable.Dispose as a direct replacement."
        *
        * From: https://xunit.github.io/docs/comparisons#note2
        * "The xUnit.net team feels that per-test setup and teardown creates difficult-to-follow and debug testing code,
        *  often causing unnecessary code to run before every single test is run.
        *  For more information, see http://jamesnewkirk.typepad.com/posts/2007/09/why-you-should-.html."
        */
        //public void PerTestCleanup() { }
        void IDisposable.Dispose()
        {
        }

        /*
        * A "Theory" is the actual test logic, allowing parameters
        * to be passed in. The parameters of the InlineData attribute must
        * match the type and order of the method's parameters.
        * You can have as many of these in a class as you want.
        */
        [Theory]
        [InlineData(1, "a")]
        [InlineData(2, "b")]
        [InlineData(-1, "")]
        public void TestMethodWithInputs(int a, string b)
        {
        }

        /*
        * The "Fact" is the actual test logic. You can have as many
        * of these in a class as you want.
        */
        [Fact]
        public void TestMethod1()
        {
        }
    }
}

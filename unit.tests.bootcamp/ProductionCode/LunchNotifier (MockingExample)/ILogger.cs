namespace ProductionCode.MockingExample
{
    using System;

    public interface ILogger
    {
        void Write(string s);
        void Debug(string s);
        void Error(Exception exception);
    }
}

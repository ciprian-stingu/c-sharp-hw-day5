namespace ProductionCode.MockingExample
{
    using System.Collections.Generic;

    public interface IEmployeeService
    {
        IEnumerable<IEmployee> GetEmployeesInNewYorkOffice();
    }
}

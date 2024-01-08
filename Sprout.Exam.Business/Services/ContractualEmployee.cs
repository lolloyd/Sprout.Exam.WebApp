using System;

namespace Sprout.Exam.Business.Services
{
    public class ContractualEmployee : IEmployeeType
    {
        public decimal CalculateSalary(decimal monthlySalary, decimal days)
        {
            return Math.Round(monthlySalary * days, 2);
        }
    }
}

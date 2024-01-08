using Sprout.Exam.Business.Services;
using System;
using System.Collections.Generic;
using System.Text;

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

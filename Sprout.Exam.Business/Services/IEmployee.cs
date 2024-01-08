using System;
using System.Collections.Generic;
using System.Text;

namespace Sprout.Exam.Business.Services
{
    public interface IEmployeeType
    {
        decimal CalculateSalary(decimal monthlySalary, decimal days);

    }
}

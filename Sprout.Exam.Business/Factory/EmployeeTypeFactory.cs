using Sprout.Exam.Business.Services;
using Sprout.Exam.Common.Enums;
using System;

namespace Sprout.Exam.Business.Factory
{
    public class EmployeeTypeFactory
    {
        public IEmployeeType CreateEmployee(EmployeeType type)
        {
            switch (type)
            {
                case EmployeeType.Regular:
                    return new RegularEmployee();
                case EmployeeType.Contractual:
                    return new ContractualEmployee();
                default:
                    throw new ArgumentException($"Employee Type : {type} not found");
            }
        }
    }
}

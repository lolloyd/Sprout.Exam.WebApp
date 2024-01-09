using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.DataAccess.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprout.Exam.UnitTests
{
    public static  class TestHelper
    {
        public static CreateEmployeeDto NewCreateEmployeeDto()
        { 
            return new CreateEmployeeDto
            {
                Tin = "206-256-348",
                FullName = "Lloyd Miguel",
                Birthdate = DateTime.Parse("1986-06-01")
            };

        }

        public static EditEmployeeDto NewEditEmployeeDto() {
        
            return new EditEmployeeDto
            {
                Id = 1,
                FullName = "Lloyd Joseph Miguel",
                Birthdate = DateTime.Parse("1986-06-01"),
                Tin = "206-256-348",
                TypeId = 1,
                Salary = 20000m
            };
        }


        public static Employee NewEmployee()
        {
            return new Employee {
                TIN = "206-256-348",
                FullName = "Lloyd Miguel",
                Birthdate = DateTime.Parse("1986-06-01")
            };
        }
        public static Employee NewEmployee2()
        {
            return new Employee
            {
                TIN = "348-256-206",
                FullName = "Ayeth Miguel",
                Birthdate = DateTime.Parse("1986-12-12")
            };
        }
    }
}

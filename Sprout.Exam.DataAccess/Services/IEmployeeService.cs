using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Common.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sprout.Exam.DataAccess.Services
{
    public interface IEmployeeService
    {
        Task<Response> AddEmployee(CreateEmployeeDto dto);
        Task<EmployeeDto> UpdateEmployee(EditEmployeeDto dto);
        Task<Response> DeleteEmployee(int id);
        Task<EmployeeDto> GetEmployeeById(int id);
        Task<Response> ValidateExistingEmployee(CreateEmployeeDto dto);
        Task<Response> ValidateExistingTin(string tin);
        Task<List<EmployeeDto>> GetAllEmployee();
    }
}

using AutoMapper;
using Microsoft.Extensions.Logging;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Common.Model;
using Sprout.Exam.DataAccess.Extensions;
using Sprout.Exam.DataAccess.Repository;
using Sprout.Exam.DataAccess.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sprout.Exam.DataAccess.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ILogger<EmployeeService> _logger;
        private readonly IDbContextWrapper _dbContextWrapper;
        private readonly IMapper _mapper;
        public EmployeeService(ILogger<EmployeeService> logger, IMapper mapper, IDbContextWrapper dbContextWrapper)
        {
            _dbContextWrapper = dbContextWrapper;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Response> AddEmployee(CreateEmployeeDto dto)
        {
            try
            {
                Employee employee = new Employee();
                var result = await _dbContextWrapper.AddAsync(employee.CopyFromDto(dto));
                return new Response() { Id = result.Id, IsSuccess = true, Message = "Employee added successfully" };
            }
            catch (Exception e)
            {
                _logger.LogError($"AddEmployee {e.Message}");
                return new Response() { IsSuccess = false, Message = $"AddEmployee {e.Message}" };
            }

        }

        public async Task<Response> ValidateExistingEmployee(CreateEmployeeDto dto)
        {
            try
            {
                Employee employee = new Employee();
                Expression<Func<Employee, bool>> predicate = employee => (employee.TIN == dto.Tin && employee.FullName == dto.FullName && employee.Birthdate == dto.Birthdate);

                var result = _dbContextWrapper.Find(predicate);
                if (result.Any())
                    return new Response() { IsSuccess = false, Message = "Employee is found in record" };
                return new Response() { IsSuccess = true };
            }
            catch (Exception e)
            {
                _logger.LogError($"AddEmployee {e.Message}");
                return new Response() { IsSuccess = false, Message = $"ValidateExistingEmployee {e.Message}" };
            }
        }

        public async Task<Response> ValidateExistingTin(string tin)
        {
            try
            {
                Employee employee = new Employee();
                Expression<Func<Employee, bool>> predicate = employee => employee.TIN == tin;

                var result = _dbContextWrapper.Find(predicate);
                if (result.Any())
                    return new Response() { IsSuccess = false, Message = "TIN is found in record" };
                return new Response() { IsSuccess = true };
            }
            catch (Exception e)
            {
                _logger.LogError($"AddEmployee {e.Message}");
                return new Response() { IsSuccess = false, Message = $"ValidateExistingTin {e.Message}" };
            }
        }

        public async Task<Response> DeleteEmployee(int id)
        {
            try
            {
                Employee employee = new Employee();
                employee = await _dbContextWrapper.GetByIdAsync<Employee>(id);

                if (employee == null)
                {
                    _logger.LogError("Employee not found");
                    return new Response() { Id = id, IsSuccess = false, Message = "Employee not found" };
                }

                await _dbContextWrapper.DeleteAsync(employee);
                return new Response() { Id = id, IsSuccess = true, Message = "Employee deleted successfully" };
            }
            catch (Exception e)
            {
                _logger.LogError($"DeleteEmployee {e.Message}");
                return new Response() { Id = id, IsSuccess = false, Message = $"DeleteEmployee {e.Message}" };
            }

        }

        public async Task<List<EmployeeDto>> GetAllEmployee()
        {
            try
            {
                var employee = _dbContextWrapper.Set<Employee>();
                return _mapper.Map<List<EmployeeDto>>(employee.ToList());
            }
            catch (Exception e)
            {
                _logger.LogError($"GetAllEmployee {e.Message}");
                throw e;
            }
        }

        public async Task<EmployeeDto> GetEmployeeById(int id)
        {
            try
            {
                return _mapper.Map<EmployeeDto>(await _dbContextWrapper.GetByIdAsync<Employee>(id));
            }
            catch (Exception e)
            {
                _logger.LogError($"GetEmployeeById {e.Message}");
                throw e;
            }
        }

        public async Task<EmployeeDto> UpdateEmployee(EditEmployeeDto dto)
        {
            try
            {
                EmployeeDto employeeDto = new EmployeeDto();
                Employee employee = new Employee();
                employee = await _dbContextWrapper.GetByIdAsync<Employee>(dto.Id);

                if (employee == null)
                {
                    _logger.LogError("Employee not found");
                    return employeeDto;
                }
                employee = employee.CopyFromDto(dto);
                _mapper.Map<EmployeeDto>(await _dbContextWrapper.UpdateAsync<Employee>(employee));

                return employeeDto;
            }
            catch (Exception e)
            {
                _logger.LogError($"UpdateEmployee {e.Message}");
                throw e;
            }
        }
    }
}

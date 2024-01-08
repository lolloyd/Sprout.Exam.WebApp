using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Common.Constants;
using Sprout.Exam.Common.Model;
using Sprout.Exam.DataAccess.Extensions;
using System;
using System.Globalization;

namespace Sprout.Exam.WebApp.Helpers
{
    public static class Validation
    {
        public static Response CalculateParams(decimal daysAbsent, decimal daysWorked)
        {
            try
            {
                if (daysAbsent > Constants.DaysInMonth || daysAbsent < 0 || daysWorked < 0)
                    return new Response() { IsSuccess = false, Message = "Invalid work info detected" };

            }
            catch (Exception e)
            {
                return new Response() { IsSuccess = false, Message = $"Error whwn validating calculate paramters {e.Message}" };
            }
            return new Response() { IsSuccess = true };
        }

        public static Response CreateEmployeeParams(CreateEmployeeDto dto)
        {
            var employeeDto = new EmployeeDto();
            return EmployeeParams(employeeDto.CopyToEmployeeDto(dto));
        }

        public static Response EditEmployeeParams(EditEmployeeDto dto)
        {

            var employeeDto = new EmployeeDto();
            return EmployeeParams(employeeDto.CopyToEmployeeDto(dto));
        }

        public static Response EmployeeParams(EmployeeDto employee)
        {
            try
            {

                if (string.IsNullOrEmpty(employee.FullName))
                {
                    return new Response() { IsSuccess = false, Message = "FullName is required" };
                }


                if (!DateTime.TryParse(employee.Birthdate.ToLongDateString(), out DateTime tempDoB))
                {
                    return new Response() { IsSuccess = false, Message = "Invalid Birthdate format" };
                }

                if (!double.TryParse(employee.Salary.ToString(), NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"), out double money))
                {
                    return new Response() { IsSuccess = false, Message = "Invalid Salary ammount" };
                }


            }
            catch (Exception e)
            {
                return new Response() { IsSuccess = false, Message = $"Error whwn validating calculate paramters {e.Message}" };
            }

            return new Response() { IsSuccess = true };
        }
    }
}

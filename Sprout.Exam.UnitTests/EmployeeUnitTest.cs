using NUnit.Framework;
using Sprout.Exam.Common.Enums;
using Sprout.Exam.Business.Factory;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.DataAccess.Repository.Models;
using Sprout.Exam.DataAccess.Repository;
using Sprout.Exam.DataAccess.Services;
using Moq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace Sprout.Exam.UnitTests
{
    public class EmployeeUnitTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CreateEmployee_RegularEmployee_Pass()
        {

            //Arrange 
            decimal salary = 20000m;
            decimal daysAbsent = 1m;

            //Act
            var testFactory = new EmployeeTypeFactory();
            var employee = testFactory.CreateEmployee(EmployeeType.Regular);
            var result = employee.CalculateSalary(salary, daysAbsent);

            //Assert
            Assert.AreEqual(16690.91m, result);
        }

        [Test]
        public void CreateEmployee_ContractualEmployee_Pass()
        {

            //Arrange 
            decimal salary = 500m;
            decimal daysWorked = 15.5m;

            //Act
            var testFactory = new EmployeeTypeFactory();
            var employee = testFactory.CreateEmployee(EmployeeType.Contractual);
            var result = employee.CalculateSalary(salary, daysWorked);

            //Assert
            Assert.AreEqual(7750m, result);
        }

        [Test]
        public async Task EmployeeService_AddEmployee_Pass()
        {
            // Arrange
            var dbContextWrapperMock = new Mock<IDbContextWrapper>();
            var loggerMock = new Mock<ILogger<EmployeeService>>();
            var configuration = new MapperConfiguration(_ =>
            {
                _.CreateMap<Employee, EmployeeDto>().ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.EmployeeTypeId));
            });
            var mapper = new Mapper(configuration);

            var employeeService = new EmployeeService(loggerMock.Object, mapper, dbContextWrapperMock.Object);

            var createEmployeeDto = new CreateEmployeeDto(); // Assuming you have this defined for test data

            dbContextWrapperMock.Setup(mock => mock.AddAsync(It.IsAny<Employee>())).ReturnsAsync(new Employee { Id = 1 }); // Mocking the AddAsync method to return a dummy Employee with ID

            // Act
            var result = await employeeService.AddEmployee(createEmployeeDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.AreEqual(1, result.Id);
        }

        [Test]
        public async Task EmployeeService_AddEmployee_Fail()
        {
            // Arrange
            var dbContextWrapperMock = new Mock<IDbContextWrapper>();
            var loggerMock = new Mock<ILogger<EmployeeService>>();

            var configuration = new MapperConfiguration(_ =>
            {
                _.CreateMap<Employee, EmployeeDto>().ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.EmployeeTypeId));
            });
            var mapper = new Mapper(configuration);
            var employeeService = new EmployeeService(loggerMock.Object, mapper, dbContextWrapperMock.Object);
            var createEmployeeDto = new CreateEmployeeDto();
            dbContextWrapperMock.Setup(mock => mock.AddAsync(It.IsAny<Employee>())).ThrowsAsync(new Exception("AddEmployee"));


            // Act
            var result = await employeeService.AddEmployee(createEmployeeDto);

            // Assert
            Assert.False(result.IsSuccess); 
            Assert.IsTrue(result.Message.Contains("AddEmployee")); 
        }


        [Test]
        public async Task EmployeeService_ValidateExistingEmployee_Exists()
        {
            // Arrange
            var dbContextWrapperMock = new Mock<IDbContextWrapper>();
            var loggerMock = new Mock<ILogger<EmployeeService>>();
            var configuration = new MapperConfiguration(_ =>
            {
                _.CreateMap<Employee, EmployeeDto>().ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.EmployeeTypeId));
            });
            var mapper = new Mapper(configuration);
            var employeeService = new EmployeeService(loggerMock.Object, mapper, dbContextWrapperMock.Object);
            var createEmployeeDto = TestHelper.NewCreateEmployeeDto();
            var employees = new List<Employee>() { TestHelper.NewEmployee() };
            dbContextWrapperMock.Setup(mock => mock.Find(It.IsAny<Expression<Func<Employee, bool>>>()))
                .Returns<Expression<Func<Employee, bool>>>(predicate => employees.AsQueryable().Where(predicate));

            // Act
            var result = await employeeService.ValidateExistingEmployee(createEmployeeDto);

            // Assert
            Assert.False(result.IsSuccess); 
            Assert.AreEqual("Employee is found in record", result.Message);
        }

        [Test]
        public async Task EmployeeService_ValidateExistingEmployee_NotExists()
        {
            // Arrange
            var dbContextWrapperMock = new Mock<IDbContextWrapper>();
            var loggerMock = new Mock<ILogger<EmployeeService>>();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Employee, EmployeeDto>().ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.EmployeeTypeId));
            });
            var mapper = new Mapper(configuration);
            var employeeService = new EmployeeService(loggerMock.Object, mapper, dbContextWrapperMock.Object);
            var createEmployeeDto = TestHelper.NewCreateEmployeeDto();
            var employees = new List<Employee>() { TestHelper.NewEmployee2() };

            dbContextWrapperMock.Setup(mock => mock.Find(It.IsAny<Expression<Func<Employee, bool>>>()))
                .Returns<Expression<Func<Employee, bool>>>(predicate => employees.AsQueryable().Where(predicate));

            // Act
            var result = await employeeService.ValidateExistingEmployee(createEmployeeDto);

            // Assert
            Assert.True(result.IsSuccess); // Check if the employee does not exist
        }

        [Test]
        public async Task EmployeeService_ValidateExistingTin_Exists()
        {
            // Arrange
            var dbContextWrapperMock = new Mock<IDbContextWrapper>();
            var loggerMock = new Mock<ILogger<EmployeeService>>();

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Employee, EmployeeDto>().ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.EmployeeTypeId));
            });
            var mapper = new Mapper(configuration);

            var employeeService = new EmployeeService(loggerMock.Object, mapper, dbContextWrapperMock.Object);

            var existingTin = TestHelper.NewEmployee().TIN;

            var employees = new List<Employee>
            {
                TestHelper.NewEmployee(),TestHelper.NewEmployee2()
            };

            dbContextWrapperMock.Setup(mock => mock.Find(It.IsAny<Expression<Func<Employee, bool>>>()))
                .Returns<Expression<Func<Employee, bool>>>(predicate => employees.AsQueryable().Where(predicate));

            // Act
            var result = await employeeService.ValidateExistingTin(existingTin);

            // Assert
            Assert.False(result.IsSuccess); 
            Assert.AreEqual("TIN is found in record", result.Message); 
        }

        [Test]
        public async Task EmployeeService_ValidateExistingTin_NotExists()
        {
            // Arrange
            var dbContextWrapperMock = new Mock<IDbContextWrapper>();
            var loggerMock = new Mock<ILogger<EmployeeService>>();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Employee, EmployeeDto>().ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.EmployeeTypeId));
            });
            var mapper = new Mapper(configuration);
            var employeeService = new EmployeeService(loggerMock.Object, mapper, dbContextWrapperMock.Object);
            var notexistingTin = "123-256-348";
            var employees = new List<Employee>
            {
                TestHelper.NewEmployee(),TestHelper.NewEmployee2()
            };

            dbContextWrapperMock.Setup(mock => mock.Find(It.IsAny<Expression<Func<Employee, bool>>>()))
                .Returns<Expression<Func<Employee, bool>>>(predicate => employees.AsQueryable().Where(predicate));

            // Act
            var result = await employeeService.ValidateExistingTin(notexistingTin);

            // Assert
            Assert.True(result.IsSuccess); 
        }
    }
}
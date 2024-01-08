using AutoMapper;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.DataAccess.Repository.Models;

namespace Sprout.Exam.DataAccess.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() => CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.EmployeeTypeId));

    }
}

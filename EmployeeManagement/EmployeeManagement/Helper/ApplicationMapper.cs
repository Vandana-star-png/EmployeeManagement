using AutoMapper;
using EmployeeManagement.Data;
using EmployeeManagement.Models;

namespace EmployeeManagement.Helper
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<Employees, EmployeeModel>().ReverseMap();
        }
    }
}

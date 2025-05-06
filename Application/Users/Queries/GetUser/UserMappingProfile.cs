using AutoMapper;
using TasksManagement.Domain.Users;
using TasksManagement.Application.Works.Queries;

namespace TasksManagement.Application.Users.Queries.GetUser
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.AssignedWorks, opt => opt.MapFrom(src => src.AssignedWork));
        }
    }
}
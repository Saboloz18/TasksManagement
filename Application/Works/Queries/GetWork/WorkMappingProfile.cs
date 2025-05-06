using AutoMapper;
using TasksManagement.Domain.Works;
using TasksManagement.Application.WorkAssignments.Queries;
using TasksManagement.Application.Works.Queries.GetWork;

namespace TasksManagement.Application.Works.Queries
{
    public class WorkMappingProfile : Profile
    {
        public WorkMappingProfile()
        {
            CreateMap<Work, WorkResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State.ToString()))
            .ForMember(dest => dest.CurrentUserName, opt => opt.MapFrom(src => src.CurrentUser != null ? src.CurrentUser.Name : null))
            .ForMember(dest => dest.AssignmentHistory, opt => opt.MapFrom(src => src.AssignmentHistory));
        }
    }
}
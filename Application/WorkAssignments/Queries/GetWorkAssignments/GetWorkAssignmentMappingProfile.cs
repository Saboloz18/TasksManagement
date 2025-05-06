using AutoMapper;
using TasksManagement.Application.WorkAssignments.Queries.GetWorkAssignments;
using TasksManagement.Domain.WorkAssignments;

namespace TasksManagement.Application.WorkAssignments.Queries
{
    public class WorkAssignmentMappingProfile : Profile
    {
        public WorkAssignmentMappingProfile()
        {
            CreateMap<WorkAssignment, WorkAssignmentResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.WorkId, opt => opt.MapFrom(src => src.WorkId))
            .ForMember(dest => dest.WorkTitle, opt => opt.MapFrom(src => src.Work.Title))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.Name : null))
            .ForMember(dest => dest.Cycle, opt => opt.MapFrom(src => src.Cycle));
        }
    }
}
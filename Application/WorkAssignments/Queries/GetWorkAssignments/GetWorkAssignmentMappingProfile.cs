using AutoMapper;
using TasksManagement.Domain.WorkAssignments;

namespace TasksManagement.Application.WorkAssignments.Queries.GetWorkAssignments
{
    public class GetWorkAssignmentMappingProfile : Profile
    {
        public GetWorkAssignmentMappingProfile()
        {
            CreateMap<WorkAssignment, WorkAssignmentResponse>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name));
        }
    }
}

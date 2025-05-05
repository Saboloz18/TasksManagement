using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManagement.Domain.Works;

namespace TasksManagement.Application.Works.Queries.GetWork
{
    public class WorkMappingProfile : Profile
    {
        public WorkMappingProfile()
        {
            CreateMap<Work, WorkResponse>()
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State.ToString()))
                .ForMember(dest => dest.CurrentUserName, opt => opt.MapFrom(src => src.CurrentUser != null ? src.CurrentUser.Name : null));
        }
    }
}

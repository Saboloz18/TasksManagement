using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManagement.Application.Works.Queries.GetWork;
using TasksManagement.Persistance.Repositories.Works;

namespace TasksManagement.Application.Works.Queries.GetWorks
{
    public class GetWorksQueryHandler : IRequestHandler<GetWorksQuery, List<WorkResponse>>
    {
        private readonly IWorkRepository _workRepository;
        private readonly IMapper _mapper;

        public GetWorksQueryHandler(IWorkRepository workRepository, IMapper mapper)
        {
            _workRepository = workRepository;
            _mapper = mapper;
        }

        public async Task<List<WorkResponse>> Handle(GetWorksQuery request, CancellationToken cancellationToken)
        {
            var works = await _workRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<List<WorkResponse>>(works);
        }
    }
}

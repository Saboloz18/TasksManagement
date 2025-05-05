using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManagement.Persistance.Repositories.Works;

namespace TasksManagement.Application.Works.Queries.GetWork
{
    public class GetWorkByIdQueryHandler : IRequestHandler<GetWorkQuery, WorkResponse?>
    {
        private readonly IWorkRepository _workRepository;
        private readonly IMapper _mapper;

        public GetWorkByIdQueryHandler(IWorkRepository workRepository, IMapper mapper)
        {
            _workRepository = workRepository;
            _mapper = mapper;
        }

        public async Task<WorkResponse?> Handle(GetWorkQuery request, CancellationToken cancellationToken)
        {
            var work = await _workRepository.GetByIdAsync(request.Id, cancellationToken);
            return work != null ? _mapper.Map<WorkResponse>(work) : null;
        }
    }
}

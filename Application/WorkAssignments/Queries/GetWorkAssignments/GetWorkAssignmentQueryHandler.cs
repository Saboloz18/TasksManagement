using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManagement.Persistance.Repositories.WorkAssignments;

namespace TasksManagement.Application.WorkAssignments.Queries.GetWorkAssignments
{
    public class GetAssignedWorksQueryHandler : IRequestHandler<GetWorkAssignmentQuery, List<WorkAssignmentResponse>>
    {
        private readonly IWorkAssignmentRepository _workAssignmentRepository;
        private readonly IMapper _mapper;

        public GetAssignedWorksQueryHandler(IWorkAssignmentRepository workAssignmentRepository, IMapper mapper)
        {
            _workAssignmentRepository = workAssignmentRepository;
            _mapper = mapper;
        }

        public async Task<List<WorkAssignmentResponse>> Handle(GetWorkAssignmentQuery request, CancellationToken cancellationToken)
        {
            var workAssignments = await _workAssignmentRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<List<WorkAssignmentResponse>>(workAssignments);
        }
    }
}

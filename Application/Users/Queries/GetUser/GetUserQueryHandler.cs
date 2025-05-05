using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManagement.Persistance.Repositories.Users;

namespace TasksManagement.Application.Users.Queries.GetUser
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserQuery, UserResponse?>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserByIdQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserResponse?> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
            return user != null ? _mapper.Map<UserResponse>(user) : null;
        }
    }
}

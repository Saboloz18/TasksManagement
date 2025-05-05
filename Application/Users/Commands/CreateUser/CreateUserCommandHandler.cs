using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManagement.Domain.Users;
using TasksManagement.Infrastructure.Exceptions;
using TasksManagement.Persistance.Repositories.Users;

namespace TasksManagement.Application.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
    {
        private readonly IUserRepository _userRepository;   
        public CreateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (await _userRepository.ExistsByNameAsync(request.Name, cancellationToken))
            {
                throw new AlreadyExistsException($"User with name: {request.Name} already exists");
            }

            var user = new User { Name = request.Name };
            var createdUser = await _userRepository.AddAsync(user, cancellationToken);
            return createdUser.Id;
        }
    }
}

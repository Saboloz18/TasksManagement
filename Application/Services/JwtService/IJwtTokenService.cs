using TasksManagement.Domain.SystemUsers;

namespace TasksManagement.Application.Services.JwtService {
    public interface IJwtTokenService { 
        string GenerateToken(SystemUser user, string role); 
    } 
}
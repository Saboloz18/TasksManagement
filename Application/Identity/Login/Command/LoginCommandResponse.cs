using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksManagement.Application.Identity.Login.Command
{
    public record LoginCommandResponse(string Token, string Username, string Role);
}

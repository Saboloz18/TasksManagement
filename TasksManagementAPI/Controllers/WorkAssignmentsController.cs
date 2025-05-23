﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TasksManagement.Application.WorkAssignments.Queries.GetWorkAssignments;

namespace TasksManagement.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,User")]
    public class AssignedWorksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AssignedWorksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get a list of currently assigned works and who they are assigned to
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAssignedWorks(CancellationToken cancellationToken)
        {
            var assignedWorks = await _mediator.Send(new GetWorkAssignmentQuery(), cancellationToken);
            return Ok(assignedWorks);
        }
    }
}
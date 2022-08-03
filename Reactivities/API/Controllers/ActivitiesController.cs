using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Domain;
using MediatR;
using Application.Activities;
using Microsoft.AspNetCore.Authorization;
// using Application.Activities;


namespace API.Controllers
{
    public class ActivitiesController : BaseApiController
    {
        // private readonly DataContext _context;
        // public IMediator _mediator { get; }
        // public ActivitiesController(IMediator mediator)
        // {
        //     _mediator = mediator;
        //     // _context = context;
        // }

        [HttpGet]
        public async Task<IActionResult> GetActivities()
        {
            // return await Mediator.Send(new List.Query());
            return HandleResult(await Mediator.Send(new List.Query()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetActivity(Guid id)
        {
            // var activity = await Mediator.Send(new Details.Query { Id = id });

            // if (activity == null)
            // {
            //     return NotFound();
            // }

            // return activity;

            // return await Mediator.Send(new Details.Query { Id = id });

            // var result = await Mediator.Send(new Details.Query { Id = id });

            // if (result.IsSuccess && result.Value != null)
            // {
            //     return Ok(result.Value);
            // }
            // if(result.IsSuccess && result.Value == null)
            // {
            //     return NotFound();
            // }
            // return BadRequest(result.Error);

            // return HandleResult(result);

            return HandleResult(await Mediator.Send(new Details.Query { Id = id }));


        }

        [HttpPost]
        public async Task<IActionResult> CreateActivity(Activity activity)
        {
            return HandleResult(await Mediator.Send(new Create.Command { Activity = activity }));
        }

        [Authorize(Policy = "IsActivityHost")]
        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> EditActivity(Guid id, Activity activity)
        {
            activity.Id = id;
            return HandleResult(await Mediator.Send(new Edit.Command { Activity = activity }));
        }

        [Authorize(Policy = "IsActivityHost")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(Guid id)
        {
            return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
        }

        [HttpPost("{id}/attend")]
        public async Task<IActionResult> Attend(Guid id)
        {
            return HandleResult(await Mediator.Send(new UpdateAttendence.Command { Id = id }));
        }
    }
        
    
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using static Application.Activities.UpdateAttendence;

namespace Application.Activities
{
    public class UpdateAttendence
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
        }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly IUserAccessor _userAccesor;
        private readonly DataContext _context;
        public Handler(DataContext context, IUserAccessor userAccesor)
        {
            _context = context;
            _userAccesor = userAccesor;
        }
        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = await _context.Activities
                .Include(a => a.Attendees).ThenInclude(U => U.AppUser)
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if(activity == null)
            {
                return Result<Unit>.Failure("Could not find activity");
            }
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.UserName == _userAccesor.GetUsername());

            if(user == null)
            {
                return Result<Unit>.Failure("Could not find user");
            }

            var HostUsername = activity.Attendees
                .FirstOrDefault(x => x.IsHost)?.AppUser.UserName;

            var attendance = activity.Attendees
                .FirstOrDefault(x => x.AppUser.UserName == user.UserName);

            if(attendance != null && HostUsername == user.UserName)
                activity.IsCancelled = !activity.IsCancelled;

            if(attendance != null && HostUsername != user.UserName)
                activity.Attendees.Remove(attendance);

            if(attendance == null)
            {
                attendance = new ActivityAttendee
                {
                    AppUser = user,
                    IsHost = false,
                    Activity = activity
                };
                activity.Attendees.Add(attendance);
            }

            var result = await _context.SaveChangesAsync() > 0;

            return result ? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Problem updating Attendance!");
        }
    }
}
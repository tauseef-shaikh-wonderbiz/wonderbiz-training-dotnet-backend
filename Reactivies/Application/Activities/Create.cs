using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Activity Activity { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
                // RuleFor(x => x..Title).NotEmpty();
                // RuleFor(x => x.Description).NotEmpty();
                // RuleFor(x => x.Category).NotEmpty();
                // RuleFor(x => x.Date).NotEmpty();
                // RuleFor(x => x.City).NotEmpty();
                // RuleFor(x => x.Venue).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                _context.Activities.Add(request.Activity);

                // await _context.SaveChangesAsync();

                // return Unit.Value;

                var result = await _context.SaveChangesAsync() > 0;

                if(!result)
                {
                    return Result<Unit>.Failure("Failed to create Activity");
                }
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
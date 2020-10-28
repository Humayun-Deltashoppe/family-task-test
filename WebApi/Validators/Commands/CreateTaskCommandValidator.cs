using Domain.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.Validators.Commands
{
    public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
    {
        public CreateTaskCommandValidator()
        {
            RuleFor(x => x.Subject).NotNull().NotEmpty();
            RuleFor(x => x.IsComplete).Must(x => x == false ).WithMessage("isComplete must be false when adding new task."); 
            
        }
    }
   
}

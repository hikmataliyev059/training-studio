using FluentValidation;
using training_studio.Areas.Manage.Helpers.DTOs.Agents;

namespace training_studio.Areas.Manage.Helpers.Validators.Agents;

public class CreateAgentDtoValidator : AbstractValidator<CreateAgentDto>
{
    public CreateAgentDtoValidator()
    {
        RuleFor(x => x.Name)
           .NotEmpty()
           .NotNull()
           .WithMessage("Name empty ve ya null ola bilmez")
           .MinimumLength(3)
           .WithMessage("Minimum 3 simvol daxil edin")
           .MaximumLength(30)
           .WithMessage("Maximum 30 simvol daxil edin");

        RuleFor(x => x.PositionId)
          .NotEmpty()
          .NotNull()
          .WithMessage("Position duzgun daxil edin");

        RuleFor(x => x.File)
          .NotEmpty()
          .NotNull()
          .WithMessage("File duzgun daxil edin");
    }
}

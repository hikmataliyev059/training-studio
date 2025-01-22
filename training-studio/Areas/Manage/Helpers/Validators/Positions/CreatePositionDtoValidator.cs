using FluentValidation;
using training_studio.Areas.Manage.Helpers.DTOs.Positions;

namespace training_studio.Areas.Manage.Helpers.Validators.Positions;

public class CreatePositionDtoValidator : AbstractValidator<CreatePositionDto>
{
    public CreatePositionDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .NotNull()
            .WithMessage("Name empty ve ya null ola bilmez")
            .MinimumLength(3)
            .WithMessage("Minimum 3 simvol daxil edin")
            .MaximumLength(30)
            .WithMessage("Maximum 30 simvol daxil edin");
    }
}

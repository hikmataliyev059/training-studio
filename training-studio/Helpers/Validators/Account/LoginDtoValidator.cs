using FluentValidation;
using training_studio.Helpers.DTOs.Account;

namespace training_studio.Helpers.Validators.Account;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.EmailOrUsername)
          .NotEmpty()
          .NotNull()
          .WithMessage("Name empty ve ya null ola bilmez")
          .MinimumLength(3)
          .WithMessage("Minimum 3 simvol daxil edin")
          .MaximumLength(30)
          .WithMessage("Maximum 30 simvol daxil edin");

        RuleFor(x => x.Password)
        .NotEmpty()
        .NotNull()
        .WithMessage("Password empty ve ya null ola bilmez")
        .MinimumLength(3)
        .WithMessage("Minimum 3 simvol daxil edin")
        .Matches("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$")
        .WithMessage("Password tipini duzgun daxil edin");
    }
}

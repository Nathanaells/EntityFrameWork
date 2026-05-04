using FluentValidation;
public class UpdateUserDTOValidator : AbstractValidator<UpdateUserDTO>
{
    public UpdateUserDTOValidator()
    {
        RuleFor(x => x.Username)
            .MinimumLength(5)
            .WithMessage("Username must be at least 5 characters long.")
            .NotEmpty()
            .WithMessage("Username cannot be empty.")
            .When(x => !string.IsNullOrWhiteSpace(x.Username));

        RuleFor(x => x.Password)
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters long.")
            .NotEmpty()
            .WithMessage("Password cannot be empty.")
            .Matches(@"[A-Z]+").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]+").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"[0-9]+").WithMessage("Password must contain at least one digit.")
            .Matches(@"[\W]+").WithMessage("Password must contain at least one special character.")
            .When(x => !string.IsNullOrWhiteSpace(x.Password));


    }
}
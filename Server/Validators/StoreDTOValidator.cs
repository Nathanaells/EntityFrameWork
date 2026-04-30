using FluentValidation;

namespace Implemented_MVC.Validators;

public class StoreDTOValidator : AbstractValidator<StoreDTO>
{
    public StoreDTOValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Store name is required.")
            .MinimumLength(3)
            .WithMessage("Store name must be at least 3 characters long.");

        RuleFor(x => x.Location)
            .NotEmpty()
            .WithMessage("Store location is required.")
            .MinimumLength(5)
            .WithMessage("Store location must be at least 5 characters long.");
    }
}

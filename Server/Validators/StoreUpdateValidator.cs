using FluentValidation;

namespace Implemented_MVC.Validators;

public class StoreUpdateValidator : AbstractValidator<UpdateStoreDTO>
{
    public StoreUpdateValidator()
    {
        RuleFor(x => x.Name)
            .MinimumLength(3)
            .WithMessage("Store name must be at least 3 characters long.")
            .When(x => !string.IsNullOrWhiteSpace(x.Name))
            .When(x => x.Name != null);

        RuleFor(x => x.Location)
            .MinimumLength(5)
            .WithMessage("Store location must be at least 5 characters long.")
            .When(x => !string.IsNullOrWhiteSpace(x.Location))
            .When(x => x.Location != null);
    }
}
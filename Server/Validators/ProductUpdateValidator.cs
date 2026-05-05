using FluentValidation;

namespace Implemented_MVC.Validators;

public class ProductUpdateValidator : AbstractValidator<UpdateProductDTO>
{
    public ProductUpdateValidator()
    {
        RuleFor(x => x.Name)
            .MinimumLength(3)
            .WithMessage("Product name must be at least 3 characters long.")
            .When(x => !string.IsNullOrWhiteSpace(x.Name))
            .When(x => x.Name != null);

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than zero.")
            .When(x => x.Price.HasValue)
            .When(x => x.Price != null);
    }
}

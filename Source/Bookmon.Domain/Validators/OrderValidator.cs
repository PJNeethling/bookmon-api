using Bookmon.Domain.Entities;
using Bookmon.Domain.Validators.Constants;
using FluentValidation;

namespace Bookmon.Domain.Validators;

public class OrderValidator : AbstractValidator<Order>
{
    public OrderValidator()
    {
        RuleFor(x => x.Books)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(ValidationMessages.IsRequired)
                .ForEach(item => item
                    .NotEmpty().WithMessage(ValidationMessages.IsRequired));
    }
}
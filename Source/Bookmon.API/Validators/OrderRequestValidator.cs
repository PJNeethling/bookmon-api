using Bookmon.API.Models.Requests;
using Bookmon.Domain.Validators.Constants;
using FluentValidation;

namespace Bookmon.API.Validators;

public class OrderRequestValidator : AbstractValidator<OrderRequest>
{
    public OrderRequestValidator()
    {
        RuleFor(x => x.Books)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(ValidationMessages.IsRequired)
            .ForEach(item => item
                .NotEmpty().WithMessage(ValidationMessages.IsRequired));
    }
}
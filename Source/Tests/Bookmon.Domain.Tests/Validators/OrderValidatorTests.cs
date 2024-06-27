using AutoFixture;
using AutoFixture.Xunit2;
using Bookmon.Domain.Entities;
using Bookmon.Domain.Validators;
using Bookmon.Domain.Validators.Constants;
using FluentValidation.TestHelper;

namespace Bookmon.Domain.Tests.Validators;

public sealed class OrderValidatorTests
{
    private readonly OrderValidator _validator;

    public OrderValidatorTests()
    {
        _validator = new OrderValidator();
    }

    [Theory, AutoData]
    public async Task OrderValidator_DoesNotThrow_WhenValid(IFixture fixture)
    {
        var request = fixture.Build<Order>()
            .With(x => x.Id, Guid.NewGuid)
            .With(x => x.UserId, Guid.NewGuid)
            .With(x => x.CreatedDate, DateTime.Now)
            .With(x => x.Books, new List<Guid> { Guid.Parse("c6267e70-dff0-40ee-9605-cff9e8049344"), Guid.Parse("f1cd9619-c825-4bcb-a678-ce9282ea84ac") })
            .With(x => x.ModifiedDate, DateTime.Now)
            .Create();

        var result = await _validator.TestValidateAsync(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory, AutoData]
    public async Task OrderValidator_ThrowsBadRequest_WithMissingRequiredFields(IFixture fixture)
    {
        var request = fixture.Build<Order>()
            .With(x => x.Books, (List<Guid>)null)
            .Create();

        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor("Books")
            .WithErrorMessage(ValidationMessages.IsRequired);
    }
}
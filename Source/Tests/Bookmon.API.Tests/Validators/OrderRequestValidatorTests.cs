using AutoFixture;
using AutoFixture.Xunit2;
using Bookmon.API.Models.Requests;
using Bookmon.API.Validators;
using Bookmon.Domain.Validators.Constants;
using FluentValidation.TestHelper;

namespace Bookmon.API.Tests.Validators;

public sealed class OrderRequestValidatorTests
{
    private readonly OrderRequestValidator _validator;

    public OrderRequestValidatorTests()
    {
        _validator = new OrderRequestValidator();
    }

    [Theory, AutoData]
    public async Task OrderRequestValidator_DoesNotThrow_WhenValid(IFixture fixture)
    {
        var request = fixture.Build<OrderRequest>()
            .With(x => x.Books, new List<Guid> { Guid.Parse("c6267e70-dff0-40ee-9605-cff9e8049344"), Guid.Parse("f1cd9619-c825-4bcb-a678-ce9282ea84ac") })
            .Create();

        var result = await _validator.TestValidateAsync(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory, AutoData]
    public async Task OrderRequestValidator_ThrowsBadRequest_WithMissingRequiredFields(IFixture fixture)
    {
        var request = fixture.Build<OrderRequest>()
            .With(x => x.Books, (List<Guid>)null)
            .Create();

        var result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor("Books")
            .WithErrorMessage(ValidationMessages.IsRequired);
    }
}
namespace Bookmon.Domain.Validators.Constants;

public static class ValidationMessages
{
    public static string IsInvalid { get; } = "is invalid";
    public static string IsRequired { get; } = "is required";
    public static string ShouldNotExceed { get; } = "should not exceed {0} characters";
    public static string ShouldNotBeMoreRecords { get; } = "should not be more than {0} records";
}
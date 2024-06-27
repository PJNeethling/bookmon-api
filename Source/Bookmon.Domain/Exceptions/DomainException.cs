using Bookmon.Domain.Enums;
using System.Runtime.Serialization;

namespace Bookmon.Domain.Exceptions;

public sealed class DomainException : Exception
{
    private DomainException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public DomainException(DomainExceptionCodes domainExceptionCode) : base(domainExceptionCode.ToString())
    {
        DomainExceptionCode = domainExceptionCode;
    }

    public DomainException(DomainExceptionCodes domainExceptionCode, string message) : base(message)
    {
        DomainExceptionCode = domainExceptionCode;
    }

    public DomainExceptionCodes DomainExceptionCode { get; }
}
namespace Bookmon.Infrastructure.Models;

public sealed class OrderDto
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public IList<string> Books { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
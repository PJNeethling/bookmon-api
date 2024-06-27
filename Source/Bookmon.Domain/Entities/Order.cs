namespace Bookmon.Domain.Entities;

public sealed class Order
{
    public Order()
    {
    }

    public Order(Guid userId, IList<Guid> books)
    {
        Id = Guid.NewGuid();

        UserId = userId;

        Books = books;
    }

    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public IList<Guid> Books { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
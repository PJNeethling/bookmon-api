using Bookmon.Domain.Enums;

namespace Bookmon.Infrastructure.Models;

public sealed class BookDto
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Author { get; set; }

    public string Genre { get; set; }

    public DateTime PublicationDate { get; set; }

    public string Publisher { get; set; }

    public double PageCount { get; set; }

    public double Price { get; set; }

    public string Language { get; set; }

    public BookFormat Format { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
using Bookmon.Domain.Enums;
using Bookmon.Infrastructure.EntityFramework;
using Bookmon.Infrastructure.Models;

namespace Bookmon.Infrastructure.Extensions;

public static class SeedDataExtensions
{
    public static void SeedData(this CosmosDbContext context)
    {
        // Define static books
        var books = new[]
        {
            new BookDto
            {
                Id = Guid.Parse("0c09512b-0caa-455b-b491-b98ba28b703a"),
                Title = "The Great Gatsby",
                Author = "F. Scott Fitzgerald",
                Genre = "Classic Fiction",
                PublicationDate = new DateTime(1925, 4, 10),
                Publisher = "Charles Scribner's Sons",
                PageCount = 180,
                Price = 9.99,
                Language = "English",
                Format = BookFormat.Paperback,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null
            },

            new BookDto
            {
                Id = Guid.Parse("3c84d5eb-6c17-476e-a980-cd11245b786d"),
                Title = "To Kill a Mockingbird",
                Author = "Harper Lee",
                Genre = "Classic Fiction",
                PublicationDate = new DateTime(1960, 7, 11),
                Publisher = "J. B. Lippincott & Co.",
                PageCount = 281,
                Price = 12.49,
                Language = "English",
                Format = BookFormat.Hardcover,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null
            },

            new BookDto
            {
                Id = Guid.Parse("6b19e99b-50a9-4330-8bf3-a7080b44b9e6"),
                Title = "1984",
                Author = "George Orwell",
                Genre = "Dystopian Fiction",
                PublicationDate = new DateTime(1949, 6, 8),
                Publisher = "Secker & Warburg",
                PageCount = 328,
                Price = 14.99,
                Language = "English",
                Format = BookFormat.Ebook,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null
            },

            new BookDto
            {
                Id = Guid.Parse("869d0985-fbef-4dd1-847c-1136325bd98b"),
                Title = "Pride and Prejudice",
                Author = "Jane Austen",
                Genre = "Romance",
                PublicationDate = new DateTime(1813, 1, 28),
                Publisher = "T. Egerton, Whitehall",
                PageCount = 432,
                Price = 11.99,
                Language = "English",
                Format = BookFormat.Paperback,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null
            },

            new BookDto
            {
                Id = Guid.Parse("04ea714f-e398-4cbd-8e5a-905c60c0c49a"),
                Title = "The Catcher in the Rye",
                Author = "J.D. Salinger",
                Genre = "Coming-of-Age Fiction",
                PublicationDate = new DateTime(1951, 7, 16),
                Publisher = "Little, Brown and Company",
                PageCount = 224,
                Price = 10.99,
                Language = "English",
                Format = BookFormat.AudioBook,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null
            },

            new BookDto
            {
                Id = Guid.Parse("1ce5fa76-a79d-461e-961b-631fde35cb07"),
                Title = "Harry Potter and the Sorcerer's Stone",
                Author = "J.K. Rowling",
                Genre = "Fantasy",
                PublicationDate = new DateTime(1997, 6, 26),
                Publisher = "Bloomsbury Publishing",
                PageCount = 320,
                Price = 17.99,
                Language = "English",
                Format = BookFormat.Hardcover,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null
            },

            new BookDto
            {
                Id = Guid.Parse("69c4094e-3882-4201-8e38-5a03efb82018"),
                Title = "The Hobbit",
                Author = "J.R.R. Tolkien",
                Genre = "Fantasy",
                PublicationDate = new DateTime(1937, 9, 21),
                Publisher = "George Allen & Unwin",
                PageCount = 310,
                Price = 13.99,
                Language = "English",
                Format = BookFormat.Ebook,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null
            },

            new BookDto
            {
                Id = Guid.Parse("60eeebc8-76f1-478d-af03-f4d79c916c87"),
                Title = "The Lord of the Rings",
                Author = "J.R.R. Tolkien",
                Genre = "Fantasy",
                PublicationDate = new DateTime(1954, 7, 29),
                Publisher = "George Allen & Unwin",
                PageCount = 1178,
                Price = 29.99,
                Language = "English",
                Format = BookFormat.Paperback,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null
            },

            new BookDto
            {
                Id = Guid.Parse("1466bb66-8943-4286-ac9c-afc5d68f53d5"),
                Title = "The Chronicles of Narnia",
                Author = "C.S. Lewis",
                Genre = "Fantasy",
                PublicationDate = new DateTime(1950, 10, 16),
                Publisher = "Geoffrey Bles",
                PageCount = 767,
                Price = 25.49,
                Language = "English",
                Format = BookFormat.Hardcover,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null
            },

            new BookDto
            {
                Id = Guid.Parse("835ca6be-aa33-4661-95f6-051ccd28e68a"),
                Title = "Moby-Dick",
                Author = "Herman Melville",
                Genre = "Adventure",
                PublicationDate = new DateTime(1851, 10, 18),
                Publisher = "Richard Bentley (UK), Harper & Brothers (US)",
                PageCount = 635,
                Price = 15.99,
                Language = "English",
                Format = BookFormat.Paperback,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null
            },
            new BookDto
            {
                Id = Guid.Parse("6c3b0ee3-728b-4fcf-8437-cfb23d7b4380"),
                Title = "The Picture of Dorian Gray",
                Author = "Oscar Wilde",
                Genre = "Gothic Fiction",
                PublicationDate = new DateTime(1890, 7, 20),
                Publisher = "Lippincott's Monthly Magazine (UK), Ward, Lock and Company (US)",
                PageCount = 254,
                Price = 11.79,
                Language = "English",
                Format = BookFormat.Ebook,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null
            },

            new BookDto
            {
                Id = Guid.Parse("4b56a5da-88d1-4343-a054-3f9be8b6430e"),
                Title = "Frankenstein",
                Author = "Mary Shelley",
                Genre = "Gothic Fiction",
                PublicationDate = new DateTime(1818, 1, 1),
                Publisher = "Lackington, Hughes, Harding, Mavor, & Jones",
                PageCount = 280,
                Price = 10.29,
                Language = "English",
                Format = BookFormat.Paperback,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null
            },

            new BookDto
            {
                Id = Guid.Parse("db8f0428-5a91-47d3-b8e4-64e0b4a2bda9"),
                Title = "Dracula",
                Author = "Bram Stoker",
                Genre = "Gothic Horror",
                PublicationDate = new DateTime(1897, 5, 26),
                Publisher = "Archibald Constable and Company (UK), Grosset & Dunlap (US)",
                PageCount = 418,
                Price = 13.99,
                Language = "English",
                Format = BookFormat.AudioBook,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null
            },

            new BookDto
            {
                Id = Guid.Parse("cc4fba36-5cfc-40a1-a389-25d632ddbc87"),
                Title = "Alice's Adventures in Wonderland",
                Author = "Lewis Carroll",
                Genre = "Fantasy",
                PublicationDate = new DateTime(1865, 11, 26),
                Publisher = "Macmillan",
                PageCount = 272,
                Price = 8.99,
                Language = "English",
                Format = BookFormat.Hardcover,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null
            },

            new BookDto
            {
                Id = Guid.Parse("2a7994f2-86b9-481b-9317-3bd5c5b5a7a5"),
                Title = "The War of the Worlds",
                Author = "H.G. Wells",
                Genre = "Science Fiction",
                PublicationDate = new DateTime(1898, 5, 7),
                Publisher = "William Heinemann",
                PageCount = 288,
                Price = 9.49,
                Language = "English",
                Format = BookFormat.Paperback,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null
            },

            new BookDto
            {
                Id = Guid.Parse("4f785fb6-651f-47f1-a285-0f7d52c62b7c"),
                Title = "The Adventures of Sherlock Holmes",
                Author = "Arthur Conan Doyle",
                Genre = "Mystery",
                PublicationDate = new DateTime(1892, 10, 14),
                Publisher = "George Newnes Ltd",
                PageCount = 307,
                Price = 12.99,
                Language = "English",
                Format = BookFormat.Ebook,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = null
            }
        };

        // Add books to the context
        context.AddRange(books);
        context.SaveChanges();
    }
}
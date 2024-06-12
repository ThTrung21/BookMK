using BookMK.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookMK.Commands.DummyData
{
    public class AddBookData : AsyncCommandBase
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(AddBookData));

        public override async Task ExecuteAsync(object parameter)
        {
            try
            {
                List<string> genre1 = new List<string> { "Fantasy" };
                List<string> genre2 = new List<string> { "Mystery", "Thriller" };
                List<string> genre3 = new List<string> { "Sci-fi" };
                List<string> genre4 = new List<string> { "Fantasy", "Sci-Fi", "Romance" };
                List<string> genre5 = new List<string> { "Education" };

                var books = new List<Book>
                {
                    new Book
                    {
                        ID = 1,
                        Cover = "1.png",
                        SellPrice = 12,
                        ReleaseYear = "2015",
                        Genre = genre1,
                        Title = "The Old Man and the Sea",
                        AuthorName = "Test",
                        Stock = 12
                    },
                    new Book
                    {
                        ID = 2,
                        Cover = "2.png",
                        SellPrice = 20,
                        ReleaseYear = "2005",
                        Genre = genre2,
                        Title = "Harry Potter and The Lost Child",
                        AuthorName = "J.K. Rowling",
                        Stock = 123
                    },
                    new Book
                    {
                        ID = 3,
                        Cover = "3.png",
                        SellPrice = 51,
                        ReleaseYear = "2015",
                        Genre = genre3,
                        Title = "The Book of Art",
                        AuthorName = "Test",
                        Stock = 0
                    },
                    new Book
                    {
                        ID = 4,
                        Cover = "4.png",
                        SellPrice = 26,
                        ReleaseYear = "2022",
                        Genre = genre4,
                        Title = "The Other Side of the Mountain",
                        AuthorName = "J.K. Rowling",
                        Stock = 54
                    },
                    new Book
                    {
                        ID = 5,
                        Cover = "5.png",
                        SellPrice = 30,
                        ReleaseYear = "1920",
                        Genre = genre1,
                        Title = "A Million to One",
                        AuthorName = "Test",
                        Stock = 58
                    }
                };

                DataProvider<Book> db = new DataProvider<Book>(Book.Collection);
                foreach (var book in books)
                {
                    await db.InsertOneAsync(book);
                }

                _logger.Information("Dummy book data added.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while adding dummy book data.");
            }
        }
    }
}

using BookMK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BookMK.Commands.DummyData
{
    public class AddBookData : AsyncCommandBase
    {
        public override async Task ExecuteAsync(object parameter)
        {
            try
            {
                List<string> genre1 = new List<string>
                {
                    "Fantasy"
                };
                List<string> genre2 = new List<string>
                {
                    "Mystery",
                    "Thriller"
                };
                List<string> genre3 = new List<string>
                {
                    "Sci-fi"
                };
                List<string> genre4 = new List<string>
                {
                    "Fantasy",
                    "Sci-Fi",
                    "Romance"
                };
                List<string> genre5 = new List<string>
                {
                    "Education"
                };

                Book b1 = new Book()
                {
                    ID = 1,
                    Cover = "1.png",
                    SellPrice = 12,
                    ReleaseYear = "2015",
                    Genre =genre1,
                    Title="The old man and the sea",
                    AuthorName= "Test",
                    Stock=12
                };
                Book b2 = new Book()
                {
                    ID = 2,
                    Cover = "2.png",
                    SellPrice = 20,
                    ReleaseYear = "2005",
                    Genre = genre2,
                    Title = "Harry Potter and The lost child",
                    AuthorName = "J.K.Rowling",
                    Stock = 123
                };
                Book b3 = new Book()
                {
                    ID = 3,
                    Cover = "3.png",
                    SellPrice = 51,
                    ReleaseYear = "2015",
                    Genre = genre3,
                    Title = "The book of art",
                    AuthorName = "Test",
                    Stock = 0
                };
                Book b4 = new Book()
                {
                    ID = 4,
                    Cover = "4.png",
                    SellPrice = 26,
                    ReleaseYear = "2022",
                    Genre = genre4,
                    Title = "The other side of mountain",
                    AuthorName = "J.K.Rowling",
                    Stock = 54
                };
                Book b5 = new Book()
                {
                    ID = 5,
                    Cover = "5.png",
                    SellPrice = 30,
                    ReleaseYear = "1920",
                    Genre = genre1,
                    Title = "A million to one",
                    AuthorName = "Test",
                    Stock = 58
                };
                DataProvider<Book> db = new DataProvider<Book>(Book.Collection);
                await db.InsertOneAsync(b1);
                await db.InsertOneAsync(b2);
                await db.InsertOneAsync(b3);
                await db.InsertOneAsync(b4);
                await db.InsertOneAsync(b5);


            }
            catch
            {

            }
        }
    }
}

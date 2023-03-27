using OfflineTest.Data;
using OfflineTest.Models;
using OfflineTest.Services.Contracts;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace OfflineTest.Services.ContractsImplementation
{
    public class BookBus : IBookBus
    {
        private readonly ApplicationDbContext _context;

        string filePath;
        string driverName = @"E:\";
        string fileName = "booksFile.json";

        public BookBus(ApplicationDbContext context)
        {
            _context = context;
            filePath = Path.Combine(driverName, fileName);
            if (!File.Exists(filePath))
            {
                File.Create(filePath);
            }
        }

        public bool checkInternetConnection()
        {
            return NetworkInterface.GetIsNetworkAvailable();
        }
        public async Task addBook(Book book)
        {
            List<Book> books = new List<Book>();
            string updatedJson;
            if (checkInternetConnection())
            {
                _context.Books.Add(book);
                await _context.SaveChangesAsync();
                books = await _context.Books?.ToListAsync();
            }
            else
            {
                string jsonBooks = File.ReadAllText(filePath);
                books = JsonConvert.DeserializeObject<List<Book>>(jsonBooks);
                Book lastBook = books.OrderBy(b => b.BookId).Last();
                book.BookId = lastBook.BookId + 1;
                books.Add(book);
            }
            updatedJson = JsonConvert.SerializeObject(books, Formatting.Indented);
            File.WriteAllText(filePath, updatedJson);
        }

        public async Task<List<Book>> getAllBooks()
        {
            List<Book> books = new List<Book>();
            if (checkInternetConnection())
            {
                books = await _context.Books?.ToListAsync();
                string updatedJson = JsonConvert.SerializeObject(books, Formatting.Indented);
                File.WriteAllText(filePath, updatedJson);
            }
            else
            {
                string jsonBooks = File.ReadAllText(filePath);
                books = JsonConvert.DeserializeObject<List<Book>>(jsonBooks);
            }

            return books;
        }

        public async Task syncData()
        {
            string json = File.ReadAllText(filePath);
            List<Book> jsonBooks = JsonConvert.DeserializeObject<List<Book>>(json);
            List<Book> onlineBooks = await _context.Books?.ToListAsync();

            if(jsonBooks != null)
            {
                foreach (Book book in jsonBooks)
                {
                    if (!onlineBooks.Any(b => b.BookId == book.BookId))
                    {
                        book.BookId = 0;
                        _context.Books.Add(book);
                        await _context.SaveChangesAsync();
                    }
                }
            }
        }
    }
}

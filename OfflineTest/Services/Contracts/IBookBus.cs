using OfflineTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OfflineTest.Services.Contracts
{
    public interface IBookBus
    {
        Task syncData();
        Task<List<Book>> getAllBooks();
        Task addBook(Book book);

        bool checkInternetConnection();

    }
}

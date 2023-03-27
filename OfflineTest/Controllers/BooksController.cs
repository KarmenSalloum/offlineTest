using Microsoft.AspNetCore.Mvc;
using OfflineTest.Models;
using OfflineTest.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OfflineTest.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookBus _bookBus;
        public BooksController(IBookBus bookBus)
        {
            _bookBus = bookBus;
        }
        public async Task<IActionResult> Index()
        {
            if (_bookBus.checkInternetConnection())
            {
               await _bookBus.syncData();
                TempData["syncData"] = "yes";
            }
            List<Book> books = await _bookBus.getAllBooks();
            return View(books);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Book book)
        {
            await _bookBus.addBook(book);
            TempData["addData"] = "success";
            return RedirectToAction(nameof(Index));
        }

    }
}

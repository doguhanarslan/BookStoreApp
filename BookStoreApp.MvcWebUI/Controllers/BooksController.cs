using System.Text.Json.Serialization;
using BookStoreApp.Business.Abstract;
using BookStoreApp.Entities.Concrete;
using BookStoreApp.MvcWebUI.Models;
using BookStoreApp.MvcWebUI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Newtonsoft.Json;
namespace BookStoreApp.MvcWebUI.Controllers
{
    public class BooksController : Controller
    {
        private HttpClient _httpClient;
        ApiService _apiService;
        public BooksController(HttpClient httpClient, ApiService apiService)
        {
            _httpClient = httpClient;
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var books = await _apiService.GetBooksAsync();
                if (books == null || !books.Any())
                {
                    return View("Error", "Kitap bulunamadı.");
                }
                return View(books);
            }
            catch (Exception ex)
            {
                // Hata detayını loglayın veya View'e gönderin
                return View("Error", ex.Message);
            }
        }
    }
}

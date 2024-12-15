using BookStoreApp.Entities.Concrete;
using Newtonsoft.Json;

namespace BookStoreApp.MvcWebUI.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5004/"); // Web API'nin URL'sini yazın
        }

        public async Task<List<Book>> GetBooksAsync()
        {
            var response = await _httpClient.GetAsync("api/books");
            response.EnsureSuccessStatusCode(); // Hata durumlarını kontrol et

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Book>>(json);
        }
    }
}

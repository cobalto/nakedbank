using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NakedBank.Front.Services
{
    public interface IHttpService
    {
        Task<T> Get<T>(string uri, string token = null);
        Task<T> Post<T>(string uri, object value, string token = null);
    }

    public class HttpService : IHttpService
    {
        private HttpClient _httpClient;

        public HttpService(
            HttpClient httpClient
        )
        {
            _httpClient = httpClient;
        }

        public async Task<T> Get<T>(string uri, string token = null)
        {
            _httpClient.DefaultRequestHeaders.Clear();

            if (token != null)
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync(uri);

            var result = System.Text.Json.JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync(),
                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result;
        }

        public async Task<T> Post<T>(string uri, object value, string token = null)
        {
            _httpClient.DefaultRequestHeaders.Clear();

            if(token != null)
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var content = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(uri, content);

            var result = System.Text.Json.JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync(),
                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result;
        }
    }
}
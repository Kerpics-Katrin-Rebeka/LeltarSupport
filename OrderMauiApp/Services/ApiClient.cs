using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LeltarSupportMauiApp.Services
{
    internal sealed class ApiClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private bool _disposed;

#if ANDROID
        private const string DefaultBaseUrl = "http://10.0.2.2:8000/";
#else
        private const string DefaultBaseUrl = "http://127.0.0.1:8000/";
#endif

        public ApiClient(HttpClient? client = null)
        {
            _httpClient = client ?? new HttpClient { BaseAddress = new Uri(DefaultBaseUrl) };
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void SetBaseAddress(string baseAddress) => _httpClient.BaseAddress = new Uri(baseAddress.TrimEnd('/') + "/");

        public void SetBearerToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                ClearAuthorization();
                return;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public void ClearAuthorization() => _httpClient.DefaultRequestHeaders.Authorization = null;

        public async Task<T?> GetAsync<T>(string route)
        {
            var resp = await _httpClient.GetAsync(route).ConfigureAwait(false);
            var body = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!resp.IsSuccessStatusCode)
                throw new Exception($"API Error: {resp.StatusCode} - {body}");

            if (string.IsNullOrWhiteSpace(body)) return default;
            return JsonConvert.DeserializeObject<T>(body);
        }

        public async Task<IEnumerable<T>> GetListAsync<T>(string route)
        {
            var resp = await _httpClient.GetAsync(route).ConfigureAwait(false);
            var body = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!resp.IsSuccessStatusCode)
                throw new Exception($"API Error: {resp.StatusCode} - {body}");

            if (string.IsNullOrWhiteSpace(body)) return Array.Empty<T>();
            var list = JsonConvert.DeserializeObject<List<T>>(body);
            return list ?? new List<T>();
        }

        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string route, TRequest item)
        {
            var jsonReq = JsonConvert.SerializeObject(item);
            // Log request so you can compare with Postman
            Console.WriteLine($"API POST -> {route}");
            Console.WriteLine($"Request JSON: {jsonReq}");
            Console.WriteLine($"Authorization header: {_httpClient.DefaultRequestHeaders.Authorization?.ToString() ?? "<none>"}");

            using var content = new StringContent(jsonReq, Encoding.UTF8, "application/json");

            var resp = await _httpClient.PostAsync(route, content).ConfigureAwait(false);
            var body = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);

            Console.WriteLine($"API POST <- {route}");
            Console.WriteLine($"Status: {resp.StatusCode}");
            Console.WriteLine($"Response Body: {body}");

            if (!resp.IsSuccessStatusCode)
                throw new Exception($"API Error: {resp.StatusCode} - {body}\nRequest: {jsonReq}");

            if (string.IsNullOrWhiteSpace(body)) return default;
            return JsonConvert.DeserializeObject<TResponse>(body);
        }

        public async Task<TResponse?> PutAsync<TRequest, TResponse>(string route, TRequest item)
        {
            var jsonReq = JsonConvert.SerializeObject(item);
            using var content = new StringContent(jsonReq, Encoding.UTF8, "application/json");

            var resp = await _httpClient.PutAsync(route, content).ConfigureAwait(false);
            var body = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!resp.IsSuccessStatusCode)
                throw new Exception($"API Error: {resp.StatusCode} - {body}");

            if (string.IsNullOrWhiteSpace(body)) return default;
            return JsonConvert.DeserializeObject<TResponse>(body);
        }

        public async Task DeleteAsync(string route)
        {
            var resp = await _httpClient.DeleteAsync(route).ConfigureAwait(false);
            var body = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!resp.IsSuccessStatusCode)
                throw new Exception($"API Error: {resp.StatusCode} - {body}");
        }

        public void Dispose()
        {
            if (_disposed) return;
            _httpClient.Dispose();
            _disposed = true;
        }
    }
}

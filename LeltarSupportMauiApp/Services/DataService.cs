using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LeltarSupportMauiApp.Services
{
    internal static class DataService
    {
        private static readonly HttpClient _client = new HttpClient { BaseAddress = new Uri("http://localhost:3000") };

        public static async Task<IEnumerable<T>> SelectAsync<T>(string route)
        {
            var result = await _client.GetStringAsync(route).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<List<T>>(result);
        }

        public static async Task<T?> PostAsync<T>(string route, T item)
        {
            var json = JsonConvert.SerializeObject(item);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");
            var resp = await _client.PostAsync(route, content).ConfigureAwait(false);
            resp.EnsureSuccessStatusCode();
            var respJson = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(respJson))
                return default;

            return JsonConvert.DeserializeObject<T>(respJson);
        }

        public static async Task<IEnumerable<object>> SelectByRouteAsync(string route, Dictionary<string, Type> map)
        {
            var result = await _client.GetStringAsync(route).ConfigureAwait(false);
            if (!map.TryGetValue(route, out var targetType))
                throw new InvalidOperationException($"No mapping found for route '{route}'.");

            var listType = typeof(List<>).MakeGenericType(targetType);
            var deserialized = JsonConvert.DeserializeObject(result, listType) as IEnumerable;
            return deserialized?.Cast<object>() ?? Enumerable.Empty<object>();
        }
    }
}

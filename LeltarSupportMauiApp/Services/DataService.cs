using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeltarSupportMauiApp.Services
{
    internal static class DataService
    {
        private static readonly ApiClient _client = new ApiClient();

        public static void SetBaseAddress(string baseAddress) => _client.SetBaseAddress(baseAddress);

        public static void SetBearerToken(string token) => _client.SetBearerToken(token);

        public static void ClearAuthorization() => _client.ClearAuthorization();

        public static Task<T?> SelectSingleAsync<T>(string route)
            => _client.GetAsync<T>(Normalize(route));

        public static Task<IEnumerable<T>> SelectAsync<T>(string route)
            => _client.GetListAsync<T>(Normalize(route));

        public static Task<TResponse?> PostAsync<TRequest, TResponse>(string route, TRequest item)
            => _client.PostAsync<TRequest, TResponse>(Normalize(route), item);

        public static Task<TResponse?> PutAsync<TRequest, TResponse>(string route, TRequest item)
            => _client.PutAsync<TRequest, TResponse>(Normalize(route), item);

        public static Task DeleteAsync(string route)
            => _client.DeleteAsync(Normalize(route));

        private static string Normalize(string route)
        {
            if (string.IsNullOrWhiteSpace(route)) return string.Empty;
            return route.TrimStart('/');
        }
    }
}

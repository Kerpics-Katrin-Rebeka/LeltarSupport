using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeltarSupportMauiApp.Services
{
    internal static class DataService
    {
        private static readonly ApiClient _client = new ApiClient();
        private const string BuyerSessionKey = nameof(BuyerSessionKey);
        private const string AccessTokenKey = "access_token";
        private const string LoginRoute = "api/auth/login";

        public static void SetBaseAddress(string baseAddress) => _client.SetBaseAddress(baseAddress);

        public static async Task<BuyerLoginResponse?> AuthenticateBuyerAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required.", nameof(email));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password is required.", nameof(password));

            var request = new BuyerLoginRequest
            {
                Email = email,
                Password = password
            };

            var serverResponse = await _client.PostAsync<BuyerLoginRequest, ServerResponse<LoginData>>(
                Normalize(LoginRoute),
                request);

            if (serverResponse is null || serverResponse.Data is null || string.IsNullOrWhiteSpace(serverResponse.Data.Token))
                return null;

            var result = new BuyerLoginResponse
            {
                User = serverResponse.Data.User,
                AccessToken = serverResponse.Data.Token,
                TokenType = serverResponse.Data.TokenType
            };

            await LoginBuyerAsync(result.AccessToken);
            return result;
        }

        public static async Task LoginBuyerAsync(string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
                throw new ArgumentException("Access token is required.", nameof(accessToken));

            await SetBearerTokenAsync(accessToken);
            Preferences.Default.Set(BuyerSessionKey, true);
        }

        public static bool IsBuyerLoggedIn()
        {
            return Preferences.Default.Get(BuyerSessionKey, false);
        }

        public static void LogoutBuyer()
        {
            Preferences.Default.Remove(BuyerSessionKey);
            ClearAuthorization();
            RemoveAccessToken();
        }

        public static async Task SetBearerTokenAsync(string token)
        {
            _client.SetBearerToken(token);
            await SecureStorage.Default.SetAsync(AccessTokenKey, token);
        }

        public static async Task<string?> GetBearerTokenAsync()
        {
            return await SecureStorage.Default.GetAsync(AccessTokenKey);
        }

        public static void RemoveAccessToken()
        {
            SecureStorage.Default.Remove(AccessTokenKey);
        }

        public static async Task RestoreAuthorizationAsync()
        {
            var token = await GetBearerTokenAsync();

            if (!string.IsNullOrWhiteSpace(token))
                _client.SetBearerToken(token);
        }

        public static void ClearAuthorization() => _client.ClearAuthorization();

        public static Task<T?> SelectSingleAsync<T>(string route)
            => _client.GetAsync<T>(Normalize(route));

        public static Task<IEnumerable<T>> SelectAsync<T>(string route)
            => _client.GetListAsync<T>(Normalize(route));

        // New helper: use when the API wraps collection in { success, message, data: [...] }
        public static async Task<IEnumerable<T>> SelectWrappedListAsync<T>(string route)
        {
            var wrapper = await _client.GetAsync<ServerResponse<List<T>>>(Normalize(route)).ConfigureAwait(false);
            if (wrapper is null || wrapper.Data is null) return Array.Empty<T>();
            return wrapper.Data;
        }

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

        internal sealed class BuyerLoginRequest
        {
            [JsonProperty("email")]
            public string Email { get; set; } = string.Empty;

            [JsonProperty("password")]
            public string Password { get; set; } = string.Empty;
        }

        internal sealed class BuyerLoginResponse
        {
            [JsonProperty("user")]
            public BuyerUser? User { get; set; }

            [JsonProperty("access_token")]
            public string? AccessToken { get; set; }

            [JsonProperty("token_type")]
            public string? TokenType { get; set; }
        }

        internal sealed class BuyerUser
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("name")]
            public string? Name { get; set; }

            [JsonProperty("email")]
            public string? Email { get; set; }
        }

        internal sealed class ServerResponse<T>
        {
            [JsonProperty("success")]
            public bool Success { get; set; }

            [JsonProperty("message")]
            public string? Message { get; set; }

            [JsonProperty("data")]
            public T? Data { get; set; }
        }

        internal sealed class LoginData
        {
            [JsonProperty("user")]
            public BuyerUser? User { get; set; }

            [JsonProperty("token")]
            public string? Token { get; set; }

            [JsonProperty("token_type")]
            public string? TokenType { get; set; }
        }

        public static string? GetAuthorizationHeader() => _client.GetAuthorizationHeader();
    }
}

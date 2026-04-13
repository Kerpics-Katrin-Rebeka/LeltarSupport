using LeltarSupportMauiApp.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderMauiApp.Services
{
    internal static class DataService
    {
        private static readonly ApiClient _client = new ApiClient();
        private const string BuyerSessionKey = nameof(BuyerSessionKey);
        private const string AccessTokenKey = "access_token";
        private const string RefreshTokenKey = "refresh_token";
        private const string LoginRoute = "api/login";

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

            var response = await _client.PostAsync<BuyerLoginRequest, BuyerLoginResponse>(
                Normalize(LoginRoute),
                request);

            if (response is null || string.IsNullOrWhiteSpace(response.AccessToken))
                return null;

            await LoginBuyerAsync(response.AccessToken, response.RefreshToken);
            return response;
        }

        public static async Task LoginBuyerAsync(string accessToken, string? refreshToken = null)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
                throw new ArgumentException("Access token is required.", nameof(accessToken));

            await SetBearerTokenAsync(accessToken);

            if (!string.IsNullOrWhiteSpace(refreshToken))
                await SetRefreshToken(refreshToken);

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
            RemoveRefreshToken();
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

        public static async Task SetRefreshToken(string token)
        {
            await SecureStorage.Default.SetAsync(RefreshTokenKey, token);
        }

        public static async Task<string?> GetRefreshToken()
        {
            return await SecureStorage.Default.GetAsync(RefreshTokenKey);
        }

        public static void RemoveRefreshToken()
        {
            SecureStorage.Default.Remove(RefreshTokenKey);
        }

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

            [JsonProperty("refresh_token")]
            public string? RefreshToken { get; set; }

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

    }
}

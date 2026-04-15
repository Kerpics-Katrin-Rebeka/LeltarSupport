using LeltarSupportMauiApp.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderMauiApp.Services
{
    internal static class DataService
    {
        private static readonly ApiClient _client = new ApiClient();
        private const string AccessTokenKey = "access_token";
        private const string RefreshTokenKey = "refresh_token";
        private const string LoginRoute = "api/login";

        public static void SetBaseAddress(string baseAddress)
        {
            if (string.IsNullOrWhiteSpace(baseAddress))
                throw new ArgumentException("Base address is required.", nameof(baseAddress));

            _client.SetBaseAddress(baseAddress);
        }

        public static async Task<AdministratorLoginResponse> AuthenticateAdministratorAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required.", nameof(email));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password is required.", nameof(password));

            var request = new AdministratorLoginRequest
            {
                Email = email,
                Password = password
            };
            var response = default(AdministratorLoginResponse);
            try
            {
                response = await _client.PostAsync<AdministratorLoginRequest, AdministratorLoginResponse>(
                Normalize(LoginRoute),
                request);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }

            if (response is null)
                throw new InvalidOperationException("Login failed: no response was returned by the server.");

            if (string.IsNullOrWhiteSpace(response.AccessToken))
                throw new InvalidOperationException("Login failed: the server did not return an access token.");

            await LoginAdministratorAsync(response.AccessToken, response.RefreshToken);
            return response;
        }

        public static async Task LoginAdministratorAsync(string accessToken, string? refreshToken = null)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
                throw new ArgumentException("Access token is required.", nameof(accessToken));

            await SetBearerTokenAsync(accessToken);

            if (string.IsNullOrWhiteSpace(refreshToken))
                RemoveRefreshToken();
            else
                await SetRefreshToken(refreshToken);
        }

        public static async Task<bool> IsAdministratorLoggedInAsync()
        {
            var token = await GetBearerTokenAsync();

            if (string.IsNullOrWhiteSpace(token))
            {
                ClearAuthorization();
                return false;
            }

            _client.SetBearerToken(token);
            return true;
        }

        public static void LogoutAdministrator()
        {
            ClearAuthorization();
            RemoveAccessToken();
            RemoveRefreshToken();
        }

        public static async Task SetBearerTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Access token is required.", nameof(token));

            _client.SetBearerToken(token);
            await SecureStorage.Default.SetAsync(AccessTokenKey, token);
        }

        public static Task<string?> GetBearerTokenAsync()
        {
            return SecureStorage.Default.GetAsync(AccessTokenKey);
        }

        public static void RemoveAccessToken()
        {
            SecureStorage.Default.Remove(AccessTokenKey);
        }

        public static async Task<bool> RestoreAuthorizationAsync()
        {
            var token = await GetBearerTokenAsync();

            if (string.IsNullOrWhiteSpace(token))
            {
                ClearAuthorization();
                return false;
            }

            _client.SetBearerToken(token);
            return true;
        }

        public static async Task SetRefreshToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Refresh token is required.", nameof(token));

            await SecureStorage.Default.SetAsync(RefreshTokenKey, token);
        }

        public static Task<string?> GetRefreshToken()
        {
            return SecureStorage.Default.GetAsync(RefreshTokenKey);
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
            if (string.IsNullOrWhiteSpace(route))
                throw new ArgumentException("Route is required.", nameof(route));

            return route.Trim().TrimStart('/');
        }

        internal sealed class AdministratorLoginRequest
        {
            [JsonProperty("email")]
            public string Email { get; set; } = string.Empty;

            [JsonProperty("password")]
            public string Password { get; set; } = string.Empty;
        }

        internal sealed class AdministratorLoginResponse
        {
            [JsonProperty("user")]
            public AdministratorUser? User { get; set; }

            [JsonProperty("access_token")]
            public string? AccessToken { get; set; }

            [JsonProperty("refresh_token")]
            public string? RefreshToken { get; set; }

            [JsonProperty("token_type")]
            public string? TokenType { get; set; }
        }

        internal sealed class AdministratorUser
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

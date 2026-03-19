using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LeltarSupportMauiApp.Models;

namespace LeltarSupportMauiApp.Services
{
    internal class LoginService
    {
        private readonly ApiClient _apiClient;

        public LoginService(ApiClient? apiClient = null)
        {
            _apiClient = apiClient ?? new ApiClient();
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            try
            {
                // Optional: Validate inputs before sending request
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                    throw new ArgumentException("Email and password cannot be empty.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LoginAsync validation error: {ex.Message}");
                return string.Empty;
            }
            var credentials = new { email, password };
            var tokenResponse = await _apiClient.PostAsync<object, TokenResponse>("api/login", credentials).ConfigureAwait(false);
            return tokenResponse?.Token ?? string.Empty;
        }

        public void SetToken(string token) => _apiClient.SetBearerToken(token);
        public void Logout() => _apiClient.ClearAuthorization();
    }
}

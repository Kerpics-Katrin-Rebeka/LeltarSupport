using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LeltarSupportMauiApp.Services;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;

namespace LeltarSupportMauiApp.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly LoginService _loginService = new();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanLogin))]
        private string email = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanLogin))]
        private string password = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanLogin))]
        private bool isBusy;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasError))]
        private string? errorMessage;

        // Computed property used by XAML for IsEnabled
        public bool CanLogin => !IsBusy && !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password);

        // Computed property used by XAML for error visibility
        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

        // Generates LoginCommand via CommunityToolkit source generator
        [RelayCommand]
        private async Task LoginAsync()
        {
            if (!CanLogin) return;

            ErrorMessage = null;
            IsBusy = true;

            try
            {
                var token = await _loginService.LoginAsync(Email, Password).ConfigureAwait(false);

                if (string.IsNullOrWhiteSpace(token))
                {
                    ErrorMessage = "Login failed: invalid credentials or empty response.";
                    return;
                }

                // Store token on shared data client so all services use it
                DataService.SetBearerToken(token);

                // Clear sensitive data
                Password = string.Empty;

                // Navigate to products page using registered Shell route
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await Shell.Current.GoToAsync("products").ConfigureAwait(false);
                }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Login error: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
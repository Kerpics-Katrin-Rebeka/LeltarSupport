using LeltarSupportMauiApp.Models;
using LeltarSupportMauiApp.Services;
using LeltarSupportMauiApp.ViewModels;
using System.Timers;

namespace LeltarSupportMauiApp.Views
{
    public partial class ProductListView : ContentPage
    {
        private const string KioskBuyerEmail = "admin@test.com";
        private const string KioskBuyerPassword = "123456";
        private bool _gestureAdded = false;

        private System.Timers.Timer _timer;
        private System.Timers.Timer _graceTimer;
        private int TimeoutMinutes = 1;

        public ProductListView(ProductListViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            cbProductList.IsVisible = false;
            if (!_gestureAdded)
            {
                MainGrid.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(() => ResetTimer())
                });

                _gestureAdded = true;
            }

            StartOverlay.IsVisible = true;
            return;

        }

        private async void OnStartOrderClicked(object? sender, EventArgs e)
        {
            ResetTimer();
            var loginResult = await DataService.AuthenticateBuyerAsync(KioskBuyerEmail, KioskBuyerPassword);

            if (loginResult is null)
                throw new InvalidOperationException("Buyer authentication failed.");

            await DataService.RestoreAuthorizationAsync();

            StartOverlay.IsVisible = false;
            cbProductList.IsVisible = true;
            if (BindingContext is ProductListViewModel vm)
            {
                await vm.LoadProductsCommand.ExecuteAsync(null);
                SetupTimer();
            }

        }

        private void OnAddToCartClicked(object? sender, EventArgs e)
        {
            ResetTimer();

            if (BindingContext is not ProductListViewModel vm) return;
            if (sender is not Button button) return;
            if (button.BindingContext is not Product product) return;

            vm.AddToCartExecute(product);
        }

        private void SetupTimer()
        {
            _timer = new System.Timers.Timer(TimeoutMinutes * 60 * 1000);
            _timer.Elapsed += OnSessionTimeout;
            _timer.AutoReset = false;
            _timer.Start();

            _graceTimer = new System.Timers.Timer(10000);
            _graceTimer.Elapsed += OnFinalTimeout;
            _graceTimer.AutoReset = false;
        }

        public void ResetTimer()
        {
            if(_timer == null || _graceTimer == null) return;
            _timer.Stop();
            _timer.Start();

            _graceTimer.Stop();
        }

        private async void OnSessionTimeout(object sender, ElapsedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                IsInactiveFor1MinutesPopUp.Opacity = 0;
                IsInactiveFor1MinutesPopUp.IsVisible = true;

                IsInactiveFor1MinutesPopUp.FadeTo(1, 250);
            });

            _graceTimer.Start();
        }

        private async void OnFinalTimeout(object sender, ElapsedEventArgs e)
        {
            CancelOrder();

        }

        private void ContinuePopUpButton_Clicked(object sender, EventArgs e)
        {
            IsInactiveFor1MinutesPopUp.IsVisible = false;
            _graceTimer.Stop();
            ResetTimer();
        }

        private void CancelOrderButton_Clicked(object sender, EventArgs e)
        {
            CancelOrder();
        }

        private void CancelOrder()
        {
            _timer?.Stop();
            _graceTimer?.Stop();
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (BindingContext is not ProductListViewModel vm) return;
                vm.CleanCart();
                IsInactiveFor1MinutesPopUp.IsVisible = false;
                DataService.LogoutBuyer();
                StartOverlay.IsVisible = true;
                StartOverlay.Opacity = 0;
            });
        }
    }
}
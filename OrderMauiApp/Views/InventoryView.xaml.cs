using OrderMauiApp.Services;
using OrderMauiApp.ViewModels;
using System.Threading.Tasks;

namespace OrderMauiApp.Views;

public partial class InventoryView : ContentPage
{
    private const string KioskAdminEmail = "admin@test.com";
    private const string KioskAdminPassword = "123456";
    private readonly InventoryViewModel _vm;

    public InventoryView(InventoryViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }

    private async Task LoginAdministrator()
    {
        var result = await DataService.AuthenticateAdminAsync(KioskAdminEmail, KioskAdminPassword);

        if (result == null)
        {
            Console.WriteLine("Login failed");
        }
        else
        {
            Console.WriteLine("Administrator logged in.");
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await LoginAdministrator();

        await DataService.RestoreAuthorizationAsync();

        var token = await DataService.GetBearerTokenAsync();
        if (string.IsNullOrWhiteSpace(token))
        {
            DataService.LogoutAdmin();
            return;
        }

        if (_vm.InventoryList.Count == 0)
        {
            await _vm.LoadInventoryCommand.ExecuteAsync(null);
        }
    }
}
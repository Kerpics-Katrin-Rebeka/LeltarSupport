using LeltarSupportMauiApp.ViewModels;

namespace LeltarSupportMauiApp.Views;

public partial class CartView : ContentPage
{
	CartViewModel _viewModel;
	public CartView(CartViewModel vm)
	{
		InitializeComponent();
		_viewModel = vm;
        BindingContext = vm;
	}

	protected override void OnAppearing()
	{
		try
		{
			if (BindingContext is CartViewModel cvm && CartListView != null)
			{
				CartListView.ItemsSource = null;
				CartListView.ItemsSource = cvm.OrderItems;
			}
		}
		catch
		{
			// ignore refresh errors
		}
	}

    private async void PurchaseButton_Clicked(object sender, EventArgs e)
    {
		_viewModel.PurchaseCommand.Execute(null);
        PurchasedPopUpOverlay.Opacity = 0;
        PurchasedPopUpOverlay.IsVisible = true;
        await PurchasedPopUpOverlay.FadeTo(1, 250);
        await Task.Delay(10000);
        PurchasedPopUpOverlay.IsVisible = false;
    }

    private void ClosePopUpButton_Clicked(object sender, EventArgs e)
    {
        PurchasedPopUpOverlay.IsVisible = false;
    }
}
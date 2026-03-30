using System.Diagnostics;
using LeltarSupportMauiApp.ViewModels;

namespace LeltarSupportMauiApp.Views;

public partial class CartView : ContentPage
{
	public CartView(CartViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		Debug.WriteLine("CartView OnAppearing");
		// Force a refresh of the ListView ItemsSource to avoid visual duplicate cells
		try
		{
			if (BindingContext is CartViewModel cvm && CartListView != null)
			{
				Debug.WriteLine($"CartView OnAppearing: CartItems.Count={cvm.CartItems?.Count}");
				CartListView.ItemsSource = null;
				CartListView.ItemsSource = cvm.CartItems;
			}
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"CartView refresh error: {ex.Message}");
		}
	}

	protected override void OnDisappearing()
	{
		base.OnDisappearing();
		Debug.WriteLine("CartView OnDisappearing");
	}
}
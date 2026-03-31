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
		try
		{
			if (BindingContext is CartViewModel cvm && CartListView != null)
			{
				CartListView.ItemsSource = null;
				CartListView.ItemsSource = cvm.CartItems;
			}
		}
		catch
		{
			// ignore refresh errors
		}
	}
}
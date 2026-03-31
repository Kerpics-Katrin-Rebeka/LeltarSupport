using System;
using LeltarSupportMauiApp.ViewModels;
using LeltarSupportMauiApp.Models;

namespace LeltarSupportMauiApp.Views;

public partial class ProductListView : ContentPage
{
	public ProductListView(ProductListViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

	private void OnAddToCartClicked(object? sender, EventArgs e)
	{
		if (BindingContext is not ProductListViewModel vm) return;
		if (sender is not Button button) return;
		if (button.BindingContext is not Product product) return;
		vm.AddToCartExecute(product);
	}
}
using System;
using System.Diagnostics;
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

	protected override void OnAppearing()
	{
		base.OnAppearing();
		Debug.WriteLine("ProductListView OnAppearing");
	}

	protected override void OnDisappearing()
	{
		base.OnDisappearing();
		Debug.WriteLine("ProductListView OnDisappearing");
	}

	private void OnAddToCartClicked(object? sender, EventArgs e)
	{
		Debug.WriteLine("OnAddToCartClicked invoked");
		if (BindingContext is not ProductListViewModel vm) return;
		if (sender is not Button button) return;
		if (button.BindingContext is not Product product) return;
		Debug.WriteLine($"Product tapped: {product?.Name}");
		vm.AddToCartExecute(product);
	}
}
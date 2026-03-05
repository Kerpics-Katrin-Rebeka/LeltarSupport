using LeltarSupportMauiApp.ViewModels;

namespace LeltarSupportMauiApp.Views;

public partial class ProductListView : ContentPage
{
	public ProductListView(ProductListViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
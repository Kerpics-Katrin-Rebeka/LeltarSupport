using InventoryApp.ViewModels;

namespace InventoryApp.Views;

public partial class MainMenuView : ContentPage
{
	public MainMenuView(MainMenuViewModel vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
	}
}
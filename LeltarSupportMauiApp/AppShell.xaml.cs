using LeltarSupportMauiApp.Views;

namespace LeltarSupportMauiApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("details", typeof(ProductDetailsView));
        }
    }
}

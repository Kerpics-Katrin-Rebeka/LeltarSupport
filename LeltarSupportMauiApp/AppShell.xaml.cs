using LeltarSupportMauiApp.Views;

namespace LeltarSupportMauiApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("products", typeof(ProductListView));
            Routing.RegisterRoute("details", typeof(ProductDetailsView));
            Routing.RegisterRoute("login", typeof(LoginView));
        }
    }
}

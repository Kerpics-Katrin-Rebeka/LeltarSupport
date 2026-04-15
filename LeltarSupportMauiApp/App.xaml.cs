namespace LeltarSupportMauiApp
{
    using LeltarSupportMauiApp.Services;

    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
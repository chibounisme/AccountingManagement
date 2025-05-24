namespace AccountingManagement
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage(); // Or AppShell if using MAUI Shell
        }

        // You could initialize services or load initial state here if needed
        protected override async void OnStart()
        {
            base.OnStart();
            // Example: Initialize auth state if it involves async operations best done here
            // var authService = IPlatformApplication.Current?.Services.GetService<Services.AuthService>();
            // if (authService != null)
            // {
            //     await authService.InitializeAuthStateAsync();
            // }
        }
    }
}
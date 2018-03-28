using Xamarin.Forms;

namespace MEPSLog_Forms
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            NavigationPage NavPage = new NavigationPage(new MEPSLog_FormsPage());
            NavPage.BarBackgroundColor = Color.FromHex("#0d4816");
            NavPage.BarTextColor = Color.White;

            ToolbarItem helpButton = new ToolbarItem("Help", null, async () =>
            {
                System.Diagnostics.Debug.WriteLine("Help Pressed");
                var helpPage = new Help_FormsPage();
                await NavPage.PushAsync(helpPage);
            }
                                                     , ToolbarItemOrder.Default, 0);
            NavPage.ToolbarItems.Add(helpButton);

            MainPage = NavPage;

            switch(Device.RuntimePlatform) {
                case "iOS":
                    System.Diagnostics.Debug.WriteLine("Running on iOS");
                    break;

                case "Android":
                    System.Diagnostics.Debug.WriteLine("Running on Android");
                    break;
            }


        }

        protected override void OnStart()
        {
            // Handle when your app starts

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

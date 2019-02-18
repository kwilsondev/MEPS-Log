using Xamarin.Forms;

namespace MEPSLog_Forms
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //create MEPS navigation Page
            NavigationPage mepsNavPage = new NavigationPage(new MEPSLog_FormsPage());
            mepsNavPage.BarBackgroundColor = Color.FromHex("#0d4816");
            mepsNavPage.BarTextColor = Color.White;
            mepsNavPage.Title = "MEPS";
            mepsNavPage.Icon = "triangleIcon.png";

            ToolbarItem mepsHelpButton = new ToolbarItem("Help", null, async () =>
            {
                System.Diagnostics.Debug.WriteLine("MEPS Help Pressed");
                var mepsHelpPage = new Help_FormsPage();
                await mepsNavPage.PushAsync(mepsHelpPage);
            }, ToolbarItemOrder.Default, 0);
            mepsNavPage.ToolbarItems.Add(mepsHelpButton);

            //create traits navigation Page
            NavigationPage traitsNavPage = new NavigationPage(new LeadershipTraits_FormsPage());
            traitsNavPage.BarBackgroundColor = Color.FromHex("#0d4816");
            traitsNavPage.BarTextColor = Color.White;
            traitsNavPage.Title = "Leadership Traits";
            traitsNavPage.Icon = "tetradecagon.png";

            ToolbarItem traitsHelpButton = new ToolbarItem("Help", null, async () =>
            {
                System.Diagnostics.Debug.WriteLine("Traits Help Pressed");
                var traitsHelpPage = new Help_FormsPage();
                await mepsNavPage.PushAsync(traitsHelpPage);

            }, ToolbarItemOrder.Default, 0);
            traitsNavPage.ToolbarItems.Add(traitsHelpButton);

            //create Tabbed Page
            TabbedPage tabbedPage = new TabbedPage();
            tabbedPage.BarTextColor = Color.FromHex("#0d4816");
            tabbedPage.BarBackgroundColor = Color.FromHex("#0d4816");
            tabbedPage.Title = "Traits";

            tabbedPage.Children.Add(mepsNavPage);
            tabbedPage.Children.Add(traitsNavPage);

            //Use this line for both meps and leadership traits on tabs
            //MainPage = tabbedPage;
            MainPage = mepsNavPage;

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

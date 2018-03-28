using System;
using Xamarin.Forms;
//using System.Diagnostics;
using PCLStorage;
using System.Threading.Tasks;
using System.ComponentModel;
//using Plugin.Notifications;

namespace MEPSLog_Forms
{
    public partial class MEPSLog_FormsPage : ContentPage, INotifyPropertyChanged
    {
        public MEPSLog_FormsPage()
        {
            InitializeComponent();

            Task.Run( () => { FileManager.Instance.GetAveragesForLabels(averageMentalLabel,
                                                                        averageEmotionalLabel,
                                                                        averagePhysicalLabel,
                                                                        averageSpiritualLabel,
                                                                        averageTotalLabel); });
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => {
                Device.OpenUri(new Uri("https://allpoetry.com/The-Man-in-the-Glass"));
            };
            manInTheGlassLabel.GestureRecognizers.Add(tapGestureRecognizer);

        }

		protected override void OnAppearing()
		{
			base.OnAppearing();
            Task.Run(() => {
                FileManager.Instance.GetAveragesForLabels(averageMentalLabel,
                                                          averageEmotionalLabel,
                                                          averagePhysicalLabel,
                                                          averageSpiritualLabel,
                                                          averageTotalLabel);
            });

            //var result = CrossNotifications.Current.RequestPermission();
		}

		async void SaveClicked(object sender, System.EventArgs evnt)
        {
            int.TryParse(mentalEntry.Text, out int m);
            int.TryParse(emotionalEntry.Text, out int e);
            int.TryParse(physicalEntry.Text, out int p);
            int.TryParse(spiritualEntry.Text, out int s);
            DateTime date = DateTime.Now;

            if (e == 7416)
            {
                await DisplayAlert("3 Most Freckles!", "You'll forever be my only one<3", "3 Most Kev!");

            }
            if ((m <= 0 || m > 10) ||
                (e <= 0 || e > 10) ||
                (p <= 0 || p > 10) ||
                (s <= 0 || s > 10))
                
            {
               await DisplayAlert("Invalid Input", "Make sure that all values are between 0 and 10 before pressing save.", "OK");
            }
            else
            {
                MepsRun newRun = new MepsRun(m, e, p, s, date);
                //Debug.WriteLine(newRun);

                FileManager.Instance.SaveRun(newRun,
                                             averageMentalLabel,
                                             averageEmotionalLabel,
                                             averagePhysicalLabel,
                                             averageSpiritualLabel,
                                             averageTotalLabel);

                mentalEntry.Text = "";
                emotionalEntry.Text = "";
                physicalEntry.Text = "";
                spiritualEntry.Text = "";

                //await CrossNotifications.Current.Send(new Notification
                //{
                //    Title = "Time for a MEPS run!",
                //    Id = 1,
                //    Message = "Don't try to fool the man in the glass.",
                //    Vibrate = true,
                //    When = TimeSpan.FromSeconds(5)
                //});

            }
        }
        async void HistoryClicked(object sender, System.EventArgs e)
        {
            var historyPage = new History_FormsPage();
            await Navigation.PushAsync(historyPage);

        }
    }
}

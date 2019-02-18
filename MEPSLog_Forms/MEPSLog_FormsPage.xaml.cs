using System;
using Xamarin.Forms;
//using System.Diagnostics;
using PCLStorage;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Diagnostics;
using SkiaSharp.Views.Forms;
using SkiaSharp;
using Plugin.LocalNotifications;
//using Plugin.Notifications;

namespace MEPSLog_Forms
{
    public partial class MEPSLog_FormsPage : ContentPage, INotifyPropertyChanged
    {

        public int mental = 10;
        public int emotional = 10;
        public int physical = 10;
        public int spiritual = 10;

        public int avgMental;
        public int avgPhysical;
        public int avgSpiritual;

        public MEPSLog_FormsPage()
        {
            InitializeComponent();

            Task.Run(async () => { FileManager.Instance.GetAveragesForLabels(averageMentalLabel,
                                                                        averageEmotionalLabel,
                                                                        averagePhysicalLabel,
                                                                        averageSpiritualLabel,
                                                                        averageTotalLabel,
                                                                        triangleCanvas);

                MepsRun avgs = await FileManager.Instance.GetAveragesForTriangle();
                avgMental = avgs.mental;
                avgPhysical = avgs.physical;
                avgSpiritual = avgs.spiritual;
            
            });
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => {
                Device.OpenUri(new Uri("https://allpoetry.com/The-Man-in-the-Glass"));
            };
            manInTheGlassLabel.GestureRecognizers.Add(tapGestureRecognizer);

        }

		protected override void OnAppearing()
		{
			base.OnAppearing();
            Task.Run(async () => {
                FileManager.Instance.GetAveragesForLabels(averageMentalLabel,
                                                          averageEmotionalLabel,
                                                          averagePhysicalLabel,
                                                          averageSpiritualLabel,
                                                          averageTotalLabel,
                                                          triangleCanvas);
                MepsRun avgs = await FileManager.Instance.GetAveragesForTriangle();
                avgMental = avgs.mental;
                avgPhysical = avgs.physical;
                avgSpiritual = avgs.spiritual;

            });

            //var result = CrossNotifications.Current.RequestPermission();
		}

		async void SaveClicked(object sender, System.EventArgs evnt)
        {
            Boolean shouldAlert = true;

            int.TryParse(mentalEntry.Text, out int m);
            int.TryParse(emotionalEntry.Text, out int e);
            int.TryParse(physicalEntry.Text, out int p);
            int.TryParse(spiritualEntry.Text, out int s);
            DateTime date = DateTime.Now;

            if (e == 7416)
            {
                await DisplayAlert("3 Most Freckles!", "You'll forever be my only one<3", "3 Most Kev!");
                shouldAlert = false;

            }

            else if (m == 76){
                
                var leadershipPage = new LeadershipTraits_FormsPage();
                await Navigation.PushAsync(leadershipPage);
                shouldAlert = false;

            }
            else if (((m <= 0 || m > 10) ||
                (e <= 0 || e > 10) ||
                (p <= 0 || p > 10) ||
                (s <= 0 || s > 10)) && shouldAlert)
                
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
                                             averageTotalLabel,
                                             triangleCanvas);

                mentalEntry.Text = "";
                emotionalEntry.Text = "";
                physicalEntry.Text = "";
                spiritualEntry.Text = "";




                    MepsRun avgs = await FileManager.Instance.GetAveragesForTriangle();

                    avgMental = avgs.mental;
                    avgPhysical = avgs.physical;
                    avgSpiritual = avgs.spiritual;

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

            CrossLocalNotifications.Current.Show("title", "body", 101, DateTime.Now.AddSeconds(5));


        }

        public double ConvertToDegrees(double angle)
        {
            return angle * (180.0 / Math.PI);
        }

        private void OnPaint(object sender, SKPaintSurfaceEventArgs e)
        {
            Debug.WriteLine("PAINTING");

            int.TryParse(averageMentalLabel.Text, out avgMental);
            int.TryParse(averagePhysicalLabel.Text, out avgPhysical);
            int.TryParse(averageSpiritualLabel.Text, out avgSpiritual);

            // get the current surface from the event args
            var surface = e.Surface;

            // get the canvas that we can draw on
            var canvas = surface.Canvas;

            //get the canvas info
            SKImageInfo info = e.Info;

            // clear the canvas / view
            canvas.Clear(SKColors.Transparent);

            // create the paint for drawing the triangle
            var trianglePaint = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                Color = SKColors.DarkRed,
                StrokeWidth = 50
            };

            // create the paint for drawing the debug lines
            var linePaint = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Black,
                StrokeWidth = 4
            };

            int centerX = info.Width / 2;
            int centerY = info.Height / 2;
            int maxwidth = info.Width - 200;

            //calculate maximum point locations
            SKPoint maxMP = new SKPoint(centerX, centerY - maxwidth / 2);
            SKPoint maxSP = new SKPoint(centerX + (maxwidth / 2), centerY + (maxwidth / 2 * (float)Math.Tan(Math.PI / 6)));
            SKPoint maxMS = new SKPoint(centerX - (maxwidth / 2), centerY + (maxwidth / 2 * (float)Math.Tan(Math.PI / 6)));


            //adjust canvas size
            //triangleCanvas.HeightRequest = maxwidth / 2.5;

            //calculate point offsets based on MEPS inputs
            float mpOffset = (avgMental + avgPhysical) / 20f;
            float spOffset = (avgSpiritual + avgPhysical) / 20f;
            float msOffset = (avgMental + avgSpiritual) / 20f;


            //calculate actual point locations
            SKPoint actualMP = new SKPoint(centerX, centerY - (maxwidth * mpOffset) / 2);
            SKPoint actualSP = new SKPoint((centerX + (maxwidth * spOffset / 2)), (centerY + (maxwidth * spOffset / 2 * (float)Math.Tan(Math.PI / 6))));
            SKPoint actualMS = new SKPoint((centerX - (maxwidth * msOffset / 2)), (centerY + (maxwidth * msOffset / 2 * (float)Math.Tan(Math.PI / 6))));

            //create paths
            SKPath guidePath = new SKPath();
            SKPath trianglePath = new SKPath();

            //path out guidePath
            guidePath.MoveTo(centerX, centerY);
            guidePath.LineTo(maxMP);
            guidePath.LineTo(maxMS);
            guidePath.LineTo(centerX, centerY);
            guidePath.MoveTo(maxMS);
            guidePath.LineTo(maxSP);
            guidePath.LineTo(centerX, centerY);
            guidePath.MoveTo(maxSP);
            guidePath.LineTo(maxMP);
            guidePath.Close();

            //path out trianglePath
            trianglePath.MoveTo(actualMS);
            trianglePath.LineTo(actualMP);
            trianglePath.LineTo(actualSP);
            trianglePath.LineTo(actualMS);
            trianglePath.Close();

            //draw both paths
            canvas.DrawPath(guidePath, linePaint);
            canvas.DrawPath(trianglePath, trianglePaint);


        }

    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MEPSLog_Forms
{
    public partial class History_FormsPage : ContentPage
    {
       
        public History_FormsPage()
        {
            InitializeComponent();

            Task.Run( () => { FileManager.Instance.GetRuns(historyList); });

            historyList.ItemSelected += (sender, e) => {
                ((ListView)sender).SelectedItem = null;
            };

        }

        public void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            FileManager.Instance.DeleteRun((string)mi.CommandParameter, historyList);
        }
    }
}

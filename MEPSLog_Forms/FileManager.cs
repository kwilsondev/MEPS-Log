using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PCLStorage;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace MEPSLog_Forms
{
    public class FileManager
    {
        
        private static FileManager _instance = null;

        IFolder rootFolder = FileSystem.Current.LocalStorage;

        public static FileManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new FileManager();
                }
                return _instance;
            }
        }

        public FileManager()
        {
            
        }

        public async void DeleteRun(String dateIn, ListView historyList) {
            List<MepsRun> runList;
            MepsRun runToDelete = new MepsRun();
            MepsRun averageData;

            //Create/Open folder and files
            IFolder mepsFolder = await rootFolder.CreateFolderAsync("MEPS_Data",
                CreationCollisionOption.OpenIfExists);
            IFile historyFile = await mepsFolder.CreateFileAsync("Run_History.txt",
                CreationCollisionOption.OpenIfExists);
            IFile averageFile = await mepsFolder.CreateFileAsync("Averages.txt",
                CreationCollisionOption.OpenIfExists);
            Debug.WriteLine(historyFile.Path);

            //Retrieve old data and create a list of runs from it
            String previousJSON = await historyFile.ReadAllTextAsync();
            runList = JsonConvert.DeserializeObject<List<MepsRun>>(previousJSON);

            for (int i = runList.Count - 1; i >= 0; i--)
            {
                if (dateIn == runList[i].date.ToString())
                {
                    runToDelete = runList[i];
                    runList.RemoveAt(i);
                }
            }

            //Read from average file into temp average run
            String averageJSON = await averageFile.ReadAllTextAsync();
            if (averageJSON != "")
            {
                averageData = JsonConvert.DeserializeObject<MepsRun>(averageJSON);
            }
            else
            {
                averageData = new MepsRun(0, 0, 0, 0, DateTime.Now);
            }

            //subtract values from average data
            averageData.totalMental -= runToDelete.mental;
            averageData.totalEmotional -= runToDelete.emotional;
            averageData.totalPhysical -= runToDelete.physical;
            averageData.totalSpiritual -= runToDelete.spiritual;

            //calculate average from all entries
            if (runList.Count != 0){
                averageData.mental = averageData.totalMental / runList.Count;
                averageData.emotional = averageData.totalEmotional / runList.Count;
                averageData.physical = averageData.totalPhysical / runList.Count;
                averageData.spiritual = averageData.totalSpiritual / runList.Count;
                averageData.total = averageData.mental * averageData.emotional *
                                            averageData.physical * averageData.spiritual;
                averageData.date = DateTime.Now; 
            } else {
                averageData.mental = 0;
                averageData.emotional = 0;
                averageData.physical = 0;
                averageData.spiritual = 0;
                averageData.total = 0;
                averageData.date = DateTime.Now; 
            }

            //write new list and averages to file
            String newAveragesJSON = JsonConvert.SerializeObject(averageData);
            await averageFile.WriteAllTextAsync(newAveragesJSON);
            String newRunListJSON = JsonConvert.SerializeObject(runList);
            await historyFile.WriteAllTextAsync(newRunListJSON);

            //update listView
            GetRuns(historyList);
        }



        public async void SaveRun (MepsRun runIN, Label mLabel, Label eLabel, Label pLabel, Label sLabel, Label totalLabel, SKCanvasView triangleCanvas) {

            List<MepsRun> runList;
            MepsRun averageData;

            //Create/Open folder and files
            IFolder mepsFolder = await rootFolder.CreateFolderAsync("MEPS_Data",
                CreationCollisionOption.OpenIfExists);
            IFile historyFile = await mepsFolder.CreateFileAsync("Run_History.txt",
                CreationCollisionOption.OpenIfExists);
            IFile averageFile = await mepsFolder.CreateFileAsync("Averages.txt",
                CreationCollisionOption.OpenIfExists);
            Debug.WriteLine(historyFile.Path);

            //Retrieve old data and create a list of runs from it
            String previousJSON = await historyFile.ReadAllTextAsync();

            if (previousJSON != "") {
                runList = JsonConvert.DeserializeObject<List<MepsRun>>(previousJSON);
            } else {
                runList = new List<MepsRun>();
            }

            //add new run to list
            runList.Reverse();
            runList.Add(runIN);
            runList.Reverse();

            Debug.WriteLine("Run Count: " + runList.Count);

            //Read from average file into temp average run
            String averageJSON = await averageFile.ReadAllTextAsync();
            if (averageJSON != "") {
                averageData = JsonConvert.DeserializeObject<MepsRun>(averageJSON);
            } else {
                averageData = new MepsRun(0,0,0,0,DateTime.Now);
            }

            //Add new values to average totals
            averageData.totalMental += runIN.mental;
            averageData.totalEmotional += runIN.emotional;
            averageData.totalPhysical += runIN.physical;
            averageData.totalSpiritual += runIN.spiritual;

            //calculate average from all entries
            averageData.mental = averageData.totalMental / runList.Count;
            averageData.emotional = averageData.totalEmotional / runList.Count;
            averageData.physical = averageData.totalPhysical / runList.Count;
            averageData.spiritual = averageData.totalSpiritual / runList.Count;
            averageData.total = averageData.mental * averageData.emotional *
                                        averageData.physical * averageData.spiritual;
            averageData.date = DateTime.Now;

            //Write new data to history and average files
            String newAveragesJSON = JsonConvert.SerializeObject(averageData);
            await averageFile.WriteAllTextAsync(newAveragesJSON);
            String newRunListJSON = JsonConvert.SerializeObject(runList);
            await historyFile.WriteAllTextAsync(newRunListJSON);

            GetAveragesForLabels(mLabel, eLabel, pLabel, sLabel, totalLabel, triangleCanvas);



            //Debug.WriteLine(previousJSON);
            //Debug.WriteLine(newRunListJSON);

        }

        public async void GetAveragesForLabels(Label mLabel, Label eLabel, Label pLabel, Label sLabel, Label totalLabel, SKCanvasView triangleCanvas) {

            MepsRun averageRun;

            //Open Files
            IFolder mepsFolder = await rootFolder.CreateFolderAsync("MEPS_Data",
                CreationCollisionOption.OpenIfExists);
            IFile historyFile = await mepsFolder.CreateFileAsync("Run_History.txt",
                CreationCollisionOption.OpenIfExists);
            IFile averageFile = await mepsFolder.CreateFileAsync("Averages.txt",
                CreationCollisionOption.OpenIfExists);
            Debug.WriteLine(historyFile.Path);

            //Get average file JSON and create current average object
            String averageJSON =  await averageFile.ReadAllTextAsync();

            if (averageJSON != "")
            {
                averageRun = JsonConvert.DeserializeObject<MepsRun>(averageJSON);
            }
            else
            {
                averageRun = new MepsRun(0, 0, 0, 0, DateTime.Now);
            }

            Device.BeginInvokeOnMainThread( () => {
                mLabel.Text = averageRun.mental.ToString();
                eLabel.Text = averageRun.emotional.ToString();
                pLabel.Text = averageRun.physical.ToString();
                sLabel.Text = averageRun.spiritual.ToString();
                totalLabel.Text = averageRun.total.ToString();

                triangleCanvas.InvalidateSurface();
            });
            //return averageRun;

        }

        public async Task<MepsRun> GetAveragesForTriangle()
        {

            MepsRun averageRun;

            //Open Files
            IFolder mepsFolder = await rootFolder.CreateFolderAsync("MEPS_Data",
                CreationCollisionOption.OpenIfExists);
            IFile historyFile = await mepsFolder.CreateFileAsync("Run_History.txt",
                CreationCollisionOption.OpenIfExists);
            IFile averageFile = await mepsFolder.CreateFileAsync("Averages.txt",
                CreationCollisionOption.OpenIfExists);
            Debug.WriteLine(historyFile.Path);

            //Get average file JSON and create current average object
            String averageJSON = await averageFile.ReadAllTextAsync();

            if (averageJSON != "")
            {
                averageRun = JsonConvert.DeserializeObject<MepsRun>(averageJSON);
            }
            else
            {
                averageRun = new MepsRun(0, 0, 0, 0, DateTime.Now);
            }
            

            return averageRun;

        }

        public async void GetRuns(ListView historyList) {

            List<MepsRun> runList;

            //Create/Open folder and files
            IFolder mepsFolder = await rootFolder.CreateFolderAsync("MEPS_Data",
                CreationCollisionOption.OpenIfExists);
            IFile historyFile = await mepsFolder.CreateFileAsync("Run_History.txt",
                CreationCollisionOption.OpenIfExists);
            IFile averageFile = await mepsFolder.CreateFileAsync("Averages.txt",
                CreationCollisionOption.OpenIfExists);
            Debug.WriteLine(historyFile.Path);

            //Retrieve old data and create a list of runs from it
            String previousJSON = await historyFile.ReadAllTextAsync();

            if (previousJSON != "")
            {
                runList = JsonConvert.DeserializeObject<List<MepsRun>>(previousJSON);
            }
            else
            {
                runList = new List<MepsRun>();
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                historyList.ItemsSource = runList;
            });

        }

    }
}

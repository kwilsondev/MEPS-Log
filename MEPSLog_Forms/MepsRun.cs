using System;
using Newtonsoft.Json;
namespace MEPSLog_Forms
{
    
    public class MepsRun
    {
        public int mental;
        public int emotional;
        public int physical;
        public int spiritual;
        public int total;

        public int totalMental;
        public int totalEmotional;
        public int totalPhysical;
        public int totalSpiritual;

        public DateTime date;

        public string RunAsString {
            get{
                return " M: " + mental + " E: " + emotional + " P: " + physical + " S: " + spiritual + " Total: " + total;
            }
        }

        public string DateAsString {
            get {
                return date.Month + "/" + date.Day + "/" + date.Year;
            }
        }

        public string FullDateAsString
        {
            get
            {
                return date.ToString();
            }
        }

        public string MentalAsString {
            get {
                return mental.ToString();
            }
        }

        public string EmotionalAsString
        {
            get
            {
                return emotional.ToString();
            }
        }

        public string PhysicalAsString
        {
            get
            {
                return physical.ToString();
            }
        }

        public string SpiritualAsString
        {
            get
            {
                return spiritual.ToString();
            }
        }

        public string TotalAsString
        {
            get
            {
                return total.ToString();
            }
        }



        public MepsRun()
        {
        }

        public MepsRun(int m, int e, int p, int s, DateTime dateIn)
        {
            mental = m;
            emotional = e;
            physical = p;
            spiritual = s;
            date = dateIn;

            total = mental * emotional * physical * spiritual;
        }

        public String ToJSON() {
            String runJSON = JsonConvert.SerializeObject(this);
            return runJSON;
        }

        public override string ToString()
        {
            
            return " M: " + mental + " E: " + emotional+ " P: " + physical + " S: " + spiritual + " Total: " + total;
        }
      
    }
}

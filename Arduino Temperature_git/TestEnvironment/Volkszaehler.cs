using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace Arduino_Temperature_Retrofit
{
    public class PushDataAnswer : EventArgs
    {
        public PushDataAnswer(string answer, DateTime timepoint)
        {
            this.Answer = answer;
            this.Timepoint = timepoint;
        }
        public string Answer { get; set; }
        public DateTime Timepoint { get; set; }
    }

    static class Volkszaehler
    {
        
        public static string GUID { get; set; }

        public static string URL { get; set; }

        private static readonly HttpClient client = new HttpClient();

        public static event EventHandler<PushDataAnswer> EvtPushDataAnswer;

        public static bool RunTest(double value)
        {
            Dictionary<string, string> values = new Dictionary<string, string>
            {
               { "operation", "add" },
               { "value", Convert.ToString(value) }
            };

            if (null == GUID)
            {
                return false;
            }

            if (null == URL)
            {
                return false;
            }

            if (values.Count < 1)
            {
                return false;
            }
            PushData(values);
            return true;

        }

        private static async void PushData(Dictionary<string, string> values)
        {
            

            FormUrlEncodedContent content = new FormUrlEncodedContent(values);
            
            string postAddress = string.Format("{0}/{1}.json", URL, GUID);

            HttpResponseMessage response = await client.PostAsync(postAddress, content);

            var responseString = await response.Content.ReadAsStringAsync();
            EvtPushDataAnswer?.Invoke(typeof(Volkszaehler), new PushDataAnswer(responseString, DateTime.Now));
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation.Runspaces;
using System.Management.Automation;
using System.Web;
using System.Net;
using System.Net.Http;

namespace Arduino_Temperature_Retrofit
{
    
    class Program
    {
                
        static WindowsIdentity _currentUser = WindowsIdentity.GetCurrent();
        static WindowsPrincipal _currentPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
        //static string url = "http://demo.volkszaehler.org/middleware.php/data";
        //static string guid = "3997b980-36d2-11e7-9b67-a9b45d523c05";

        static int Main(string[] args)
        {
            /*
            Volkszaehler.EvtPushDataAnswer += Volkszaehler_EvtPushDataAnswer;
            Volkszaehler.GUID = guid;
            Volkszaehler.URL = url;
            for (int u =0;u<=10;u++)
            {
                Random rnd = new Random();
                double val = rnd.NextDouble() * 30;
                Console.WriteLine("Fuege Wert hinzu: {0}", val);
                if (!Volkszaehler.runTest(val))
                    Console.WriteLine("Fehler beim Aufruf");
                
                Console.ReadKey();
            }
            */
            Task<string> ret = DownloadPage("asf");
            int p = 1;
            foreach(string l in ret.Result.Split('\n'))
            {
                if (l.Contains("Salzburg"))
                    Console.WriteLine("Line : {0} - " + l, p++);
            }
            Console.ReadKey();
            runXMLText();
            Console.ReadKey();
            
            List<double> lst = new List<double>();
            for (int x = 1; x <= 10; x++)
                lst.Add(x);

            foreach (double t in lst)
                Console.WriteLine("Wert von t: " + t);
            Console.WriteLine("\nSubset\n");

            List<double> sub = getSubset(lst, 5);
            foreach (double t in sub)
                Console.WriteLine("Wert von t: " + t);

            Console.ReadKey();

            DataObject dojb = new DataObject();

            Console.WriteLine(dojb.TemperatureDetail.MinValue);

            htmlconvert();
            Console.ReadKey();

            if (HasAccess(new FileInfo(@"C:\inetpub\wwwroot\temp.html"), FileSystemRights.WriteData))
                Console.WriteLine("HTML: Has access");
            else
                Console.WriteLine("HTML: Has _NO_ access");

            if (HasAccess(new FileInfo(@"C:\Temp\log\temp_COM11.log"), FileSystemRights.WriteData))
                Console.WriteLine("LOG: Has access");
            else
                Console.WriteLine("LOG: Has _NO_ access");

            Console.WriteLine();
            Console.ReadKey();
            return 0;
        }

        static async Task<string> DownloadPage(string url)
        {
            using (var client = new HttpClient())
            {
                using (var r = await client.GetAsync(new Uri("http://www.zamg.ac.at/ogd/")))
                {
                    string result = await r.Content.ReadAsStringAsync();
                    return result;
                }
            }
        }

        private static void Volkszaehler_EvtPushDataAnswer(object sender, pushDataAnswer e)
        {
            Console.WriteLine(e.Answer);
        }

        public static void runXMLText()
        {
            try
            {
                List<XMLSensorObject> senLst = new List<XMLSensorObject>();
                senLst = XML.getSensorItemsFromXML();

                Console.ReadKey();

                //public static string Title { get { return getValue("/root/titel"); } set { setValue("/root/titel", value); } }
                //public static string HtmlFile { get { return getValue("/root/HTML/FileHTML"); } set { setValue("/root/HTML/FileHTML", value); } }
                //public static string HtmlHeadText { get { return getValue("/root/HTML/HTMLHEAD"); } set { setValue("/root/HTML/HTMLHEAD", value); } }
                //public static bool HtmlEnabled { get { return checkBool(getValue("/root/HTML/Enabled")); } set { setValueBool("/root/HTML/HTMLHEAD", value); } }
                string oldTitle = XML.Title;
                string oldHtmlFile = XML.HtmlFile;
                string oldHtmlHeadText = XML.HtmlHeadText;
                bool oldHtmlEnabled = XML.HtmlEnabled;

                Console.WriteLine("************************************************");
                Console.WriteLine("Original Values:");
                Console.WriteLine("Title:          " + XML.Title);
                Console.WriteLine("HTML File:      " + XML.HtmlFile);
                Console.WriteLine("HTML Head Text: " + XML.HtmlHeadText);
                Console.WriteLine("HTML Enabled:   " + XML.HtmlEnabled.ToString());
                Console.WriteLine("************************************************");

                Console.ReadKey();
                XML.Title = "Html Changed Value";
                XML.HtmlFile = @"C:\Temp\doesnotexit.html";
                XML.HtmlHeadText = "This is a new Head-Text";
                XML.HtmlEnabled = !oldHtmlEnabled;

                Console.WriteLine("************************************************");
                Console.WriteLine("Changed Values:");
                Console.WriteLine("Title:          " + XML.Title);
                Console.WriteLine("HTML File:      " + XML.HtmlFile);
                Console.WriteLine("HTML Head Text: " + XML.HtmlHeadText);
                Console.WriteLine("HTML Enabled:   " + XML.HtmlEnabled.ToString());
                Console.WriteLine("************************************************");
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            
        }
        public static void htmlconvert()
        {
            String myString;
            Console.WriteLine("Enter a string having '&' or '\"'  in it: ");
            myString = Console.ReadLine();
            String myEncodedString;
            // Encode the string.
            myEncodedString = HttpUtility.HtmlEncode(myString);
            Console.WriteLine("HTML Encoded string is " + myEncodedString);
            StringWriter myWriter = new StringWriter();
            // Decode the encoded string.
            HttpUtility.HtmlDecode(myEncodedString, myWriter);
            Console.Write("Decoded string of the above encoded string is " +
                           myWriter.ToString());
        }

        static private List<double> getSubset(List<double> values, int count)
        {
            if (values.Count <= count)
                return values;

            int start = values.Count - count;

            return values.GetRange(start, count);
        }

        public static bool trycreatefile(FileInfo file)
        {
            if (!file.Exists)
            {
                try
                {
                    file.Create();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool trycreatedir(DirectoryInfo directory)
        {
            if (!directory.Exists)
            {
                try
                {
                    directory.Create();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        private static string getExistingPartOfPath(string testDir)
        {
            int lst = 0;
            int last = 0;

            while (!(lst == testDir.LastIndexOf("\\")))
            {
                lst = testDir.IndexOf("\\", ++lst);
                if (!System.IO.Directory.Exists(testDir.Substring(0, lst)))
                    break;

                Console.WriteLine(testDir.Substring(0, lst));
                last = lst;
            }

            return testDir.Substring(0, last);
        }

        public static bool HasAccess(FileInfo file, FileSystemRights right)
        {
            string directoryName = file.DirectoryName;
            string existingPath = getExistingPartOfPath(directoryName);

            if (directoryName.Substring(directoryName.Length - 1) == "\\")
                directoryName = directoryName.Substring(directoryName.Length - 1);

            if (existingPath.Substring(existingPath.Length - 1) == "\\")
                existingPath = existingPath.Substring(existingPath.Length - 1);
            
            if (existingPath != directoryName)
            {
                return HasWritePermissionOnDir(existingPath);
            }
            
            // Get the collection of authorization rules that apply to the file.
            AuthorizationRuleCollection acl = file.GetAccessControl().GetAccessRules(true, true, typeof(SecurityIdentifier));
            return HasFileOrDirectoryAccess(right, acl);
        }

        private static bool HasFileOrDirectoryAccess(FileSystemRights right,
                                              AuthorizationRuleCollection acl)
        {
            bool allow = false;
            bool inheritedAllow = false;
            bool inheritedDeny = false;

            for (int i = 0; i < acl.Count; i++)
            {
                FileSystemAccessRule currentRule = (FileSystemAccessRule)acl[i];
                // If the current rule applies to the current user.
                if (_currentUser.User.Equals(currentRule.IdentityReference) ||
                    _currentPrincipal.IsInRole(
                                    (SecurityIdentifier)currentRule.IdentityReference))
                {

                    if (currentRule.AccessControlType.Equals(AccessControlType.Deny))
                    {
                        if ((currentRule.FileSystemRights & right) == right)
                        {
                            if (currentRule.IsInherited)
                            {
                                inheritedDeny = true;
                            }
                            else
                            { // Non inherited "deny" takes overall precedence.
                                return false;
                            }
                        }
                    }
                    else if (currentRule.AccessControlType
                                                    .Equals(AccessControlType.Allow))
                    {
                        if ((currentRule.FileSystemRights & right) == right)
                        {
                            if (currentRule.IsInherited)
                            {
                                inheritedAllow = true;
                            }
                            else
                            {
                                allow = true;
                            }
                        }
                    }
                }
            }

            if (allow)
            { // Non inherited "allow" takes precedence over inherited rules.
                return true;
            }
            return inheritedAllow && !inheritedDeny;
        }

        public static bool HasWritePermissionOnDir(string path)
        {
            var writeAllow = false;
            var writeDeny = false;
            var accessControlList = Directory.GetAccessControl(path);
            if (accessControlList == null)
                return false;
            var accessRules = accessControlList.GetAccessRules(true, true,
                                        typeof(System.Security.Principal.SecurityIdentifier));
            if (accessRules == null)
                return false;

            foreach (FileSystemAccessRule rule in accessRules)
            {
                if ((FileSystemRights.Write & rule.FileSystemRights) != FileSystemRights.Write)
                    continue;

                if (rule.AccessControlType == AccessControlType.Allow)
                    writeAllow = true;
                else if (rule.AccessControlType == AccessControlType.Deny)
                    writeDeny = true;
            }

            return writeAllow && !writeDeny;
        }


        public static bool IsSigned(string filepath)

        {
            var runspaceConfiguration = RunspaceConfiguration.Create();

            using (var runspace = RunspaceFactory.CreateRunspace(runspaceConfiguration))

            {

                runspace.Open();

                using (var pipeline = runspace.CreatePipeline())

                {

                    pipeline.Commands.AddScript("Get-AuthenticodeSignature \"" + filepath + "\"");

                    var results = pipeline.Invoke();

                    runspace.Close();


                    foreach (PSObject p in results)
                    {
                        foreach(PSProperty x in p.Properties)
                        {
                            if (x.Value != null)
                                Console.WriteLine(x.Name.ToString() + "\t" + x.Value.ToString());
                        }
                    }

                    var signature = results[0].BaseObject as Signature;

                    if (signature != null)
                    {
                        Signature sig = (Signature)signature;
                        Console.WriteLine("Status = " + sig.Status.ToString());

                    }
                    return signature == null || signature.SignerCertificate == null ?

                           false : (signature.Status != SignatureStatus.NotSigned);

                }

            }

        }
    }
}

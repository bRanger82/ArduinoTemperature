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


namespace TestEnvironment
{
    class Program
    {
                
        static WindowsIdentity _currentUser = WindowsIdentity.GetCurrent();
        static WindowsPrincipal _currentPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());

        static int Main(string[] args)
        {
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

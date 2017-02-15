using System;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.Security.AccessControl;
using System.Xml;
using System.Text;
using System.Globalization;

namespace Arduino_Temperature
{
    public partial class frmMain : Form
    {
        private enum dataSource
        {
            Boden = 0, 
            Tisch = 1
        }

        private SerialPort spTisch = new SerialPort();
        private SerialPort spBoden = new SerialPort();
        private bool isConnected = false;
        private string tempDataTisch = string.Empty;
        private string tempDataBoden = string.Empty;
        private DateTime timeStampLastUpdateTisch = new DateTime();
        private DateTime timeStampLastUpdateBoden = new DateTime();
        private System.Timers.Timer tmr = new System.Timers.Timer();
        private const int maxConnRetries = 10;
        private const int maxConnRetriesInitValue = 0;
        private static string strPortTisch;
        private static string strPortBoden;
        private static int maxTimeDifferenceReadData; 
        private static long maxLogFileSize;
        private static string logPathTisch;
        private static string logPathBoden;
        private static string PathHTML;
        private static bool bodenAktiv;
        private static bool tischAktiv;
        private static List<DataObject> dataBoden = new List<DataObject>();
        private static List<DataObject> dataTisch = new List<DataObject>();
        private const int maxLenDataObjects = 4;

        private Dictionary<string, int> di = new Dictionary<string, int>(); //Anzahl Verbindungsversuche pro Port

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

            try
            {
                if (!XML.XMLExists)
                {
                    MessageBox.Show("Die Konfigurationsdatei '" + XML.XMLFileName + "' existiert nicht, Programm kann nicht geladen werden!",
                                    "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    this.Close();
                    return;
                }

                loadSetttingsFromXML();

                lblHTMLNumEntriesHist.Text = "Anzahl Einträge: " + this.numMaxEntries.Value.ToString();
                setLabelFormat(lblTempTisch, lblTableLastUpdated);
                setLabelFormat(lblTempBoden, lblBottomLastUpdated);

                dataBoden.Clear();
                dataTisch.Clear();

                connectToDevices();

                isConnected = ((tischAktiv) ? spTisch.IsOpen : true && (bodenAktiv) ? spBoden.IsOpen : true);

                if (isConnected)
                {
                    tmr.Interval = 10000;
                    tmr.Elapsed += Tmr_Elapsed;
                    tmr.Enabled = true;
                    tmr.Start();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isConnected = false;
                spTisch.DataReceived -= Sp_DataReceived;
                spBoden.DataReceived -= Sp_DataReceived;
                tmr.Enabled = false;
                tmr.Stop();
            }
        }

        private void connectToDevices()
        {

            this.chkLogEnabled.Checked = false;
            this.chkHTML.Checked = false;

            FileInfo pathTable = new FileInfo(logPathTisch);
            FileInfo pathBottom = new FileInfo(logPathBoden);
            FileInfo pathHTML = new FileInfo(PathHTML);

            this.chkLogEnabled.Enabled = false;

            if (HasAccess(pathTable, FileSystemRights.WriteData) &&
                HasAccess(pathBottom, FileSystemRights.WriteData))
                this.chkLogEnabled.Enabled = true;

            this.chkHTML.Enabled = HasAccess(new FileInfo(PathHTML), FileSystemRights.WriteData);

            if (tischAktiv)
            {
                lblTableLastUpdated.Text = "Verbindungsaufbau ...";
                if (tryConnect(ref spTisch, strPortTisch))
                    lblTableLastUpdated.Text = "Warte auf Daten ...";
                else
                    lblTableLastUpdated.Text = "Verbindungsaufbau fehlgeschlagen";
            }
            else
            {
                lblTableLastUpdated.Text = "Nicht verwendet (in der Konfiguration inaktiv gesetzt)";
            }

            if (bodenAktiv)
            {
                lblBottomLastUpdated.Text = "Verbindungsaufbau ...";
                if (tryConnect(ref spBoden, strPortBoden))
                    lblBottomLastUpdated.Text = "Warte auf Daten ...";
                else
                    lblBottomLastUpdated.Text = "Verbindungsaufbau fehlgeschlagen";
            }
            else
            {
                lblBottomLastUpdated.Text = "Nicht verwendet (in der Konfiguration inaktiv gesetzt)";
            }

        }

        private void setLabelFormat(Label lblParent, Label lblUpdate)
        {
            System.Drawing.Point pos = this.PointToScreen(lblUpdate.Location);
            pos = lblParent.PointToClient(pos);
            lblUpdate.Parent = lblParent;
            lblUpdate.Location = pos;
            lblUpdate.BackColor = System.Drawing.Color.Transparent;
        }

        private void addDataset(dataSource ds, DataObject dobj)
        {
            switch(ds)
            {
                case dataSource.Boden: genericAddDataToList(dataBoden, dobj, numMaxEntries.Value); break;
                case dataSource.Tisch: genericAddDataToList(dataTisch, dobj, numMaxEntries.Value); break;
            }
        }

        private void genericAddDataToList(List<DataObject> lst, DataObject dobj, int maxLen)
        {
            if (maxLen < 1) return;
            
            lst.Add(dobj);

            while (lst.Count > maxLen)
            {
                lst.RemoveAt(0);
            }
            
        }

        private void loadSetttingsFromXML()
        {
            strPortTisch = XML.TischPort;
            strPortBoden = XML.BodenPort;
            maxTimeDifferenceReadData = XML.maxTimeDifferenceReadData; 
            maxLogFileSize = XML.maxLogFileSize;
            logPathTisch = XML.TischLogfile;
            logPathBoden = XML.BodenLogfile;
            PathHTML = XML.FileHTML;
            tischAktiv = XML.TischAktiv;
            bodenAktiv = XML.BodenAktiv;
            this.Text = XML.Title;
        }

        private string reconnectText(string portName)
        {
            if (!di.ContainsKey(portName))
                return string.Empty;
            if ((int)di[portName] > maxConnRetries)
                return "Keine Verbindung nach " + di[portName].ToString() + " Versuchen.";
            else
                return "Verbindung verloren! \n" + di[portName].ToString() + ". Verbindungsversuch von " + maxConnRetries.ToString();
        }

        private void Tmr_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!tryConnect(ref spTisch, strPortTisch))
                lblTempTisch.Text = reconnectText(strPortTisch);

            //if (!tryConnect(ref spBoden, strPortBoden))
            //    lblTempTisch.Text = reconnectText(strPortBoden);

            writeToHTML();
        }

        private void Sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (sender is SerialPort)
            {
                SerialPort spTemp = (SerialPort)sender;
                string received = spTemp.ReadLine();
                string portName = spTemp.PortName;
                this.BeginInvoke(new LineReceivedEvent(LineReceived), received, portName);
            }
        }

        private delegate void LineReceivedEvent(string line, string comPort);

        private string getLineFromData(string line, out DataObject dobj)
        {
            dobj = new DataObject();
            dobj.DataAvailable = false;
            dobj.Protocol = DataObjectProtocol.NONE;
            dobj.Timepoint = DateTime.Now;

            string returnValue = string.Empty;

            if (line.Contains("|"))
            {
                string[] values = line.Split('|');
                //Format: START|humidity|temperature|heatindex|EOF
                //This format was defined within the Arduino Sketch
                //  e.g. (random numbers): START|40.30|25.50|26.54|EOF 
                // START: Indicates a Start of a data-frame
                //  40.30 --> 40.30% Humidity (relative)
                //  25.50 --> 25.50°C Temperature
                //  26.54 --> 26.54°C Temperature/Heat-Index
                // EOF: End of Frame
                string len = values[1];

                if (len.Contains(":"))
                {
                    Console.WriteLine("Laenge: '{0}'", len.Substring(len.IndexOf(":") + 1));
                    int graphLen;
                    if (int.TryParse(len.Substring(len.IndexOf(":") + 1), out graphLen))
                    {

                    }
                }

                if (values.Length == 7 && values[0].StartsWith("START") && values[6].StartsWith("EOF")) //Protocoll second version
                {
                    
                    dobj.AirPressure = values[5].ToString();
                    dobj.HeatIndex = values[4].ToString();
                    dobj.Humidity = values[2].ToString();
                    dobj.Temperature = values[3].ToString();
                    dobj.Timepoint = DateTime.Now;
                    dobj.DataAvailable = true;
                    dobj.AdditionalInformation = "-";
                    dobj.Protocol = DataObjectProtocol.PROTOCOL_TWO;

                    returnValue = "Luftfeuchtigkeit: " + values[2].ToString() + " %\n" +
                           "Temperatur: " + values[3].ToString() + " °C\n" +
                           "'Heat Index': " + values[4].ToString() + " °C\n" +
                           "Luftdruck: " + values[5].ToString() + " mb\n"; 

                } else if (values.Length == 5 && values[0].StartsWith("START") && values[4].StartsWith("EOF")) //Protocol first version
                {
                    
                    dobj.AirPressure = string.Empty;
                    dobj.HeatIndex = values[3].ToString();
                    dobj.Humidity = values[1].ToString();
                    dobj.Temperature = values[2].ToString();
                    dobj.Timepoint = DateTime.Now;
                    dobj.DataAvailable = true;
                    dobj.AdditionalInformation = "-";
                    dobj.Protocol = DataObjectProtocol.PROTOCOL_ONE;

                    returnValue = "Luftfeuchtigkeit: " + values[1].ToString() + " %\n" +
                           "Temperatur: " + values[2].ToString() + " °C\n" +
                           "'Heat Index': " + values[3].ToString() + " °C\n";
                }
                else
                {
                    dobj.AirPressure = string.Empty;
                    dobj.HeatIndex = string.Empty;
                    dobj.Humidity = string.Empty;
                    dobj.Temperature = string.Empty;
                    dobj.Timepoint = DateTime.Now;
                    dobj.DataAvailable = false;
                    dobj.AdditionalInformation = "-";
                    dobj.Protocol = DataObjectProtocol.NONE;

                    returnValue = "Fehler beim Lesen der Daten";
                }
            }
            else
            {
                dobj.AirPressure = string.Empty;
                dobj.HeatIndex = string.Empty;
                dobj.Humidity = string.Empty;
                dobj.Temperature = string.Empty;
                dobj.Timepoint = DateTime.Now;
                dobj.DataAvailable = false;
                dobj.AdditionalInformation = "-";
                dobj.Protocol = DataObjectProtocol.NONE;

                returnValue = line;
            }

            return returnValue;
        }

        private void LineReceived(string newline, string comPort)
        {

            DataObject dobj = new DataObject();

            string line = getLineFromData(newline, out dobj);
            
            if (comPort == strPortTisch)
            {
                timeStampLastUpdateTisch = DateTime.Now;
                lblTempTisch.Text = XML.TischBezeichnung + " (" + strPortTisch + ")\n" + line;
                lblTableLastUpdated.Text = "Aktualisiert: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
                tempDataTisch = "TISCH</br>" + line.Replace("\n", "</br>");
                writeToLog(getCurrentDateTimeFormatted() + "\t" + line.Replace(".", ","), logPathTisch);
                addDataset(dataSource.Tisch, dobj);
            } else if (comPort == strPortBoden)
            {
                timeStampLastUpdateBoden = DateTime.Now;
                lblTempBoden.Text = XML.BodenBezeichnung + " (" + strPortBoden + ")\n" + line;
                lblBottomLastUpdated.Text = "Aktualisiert: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
                tempDataBoden = "BODEN</br>" + line.Replace("\n", "</br>");
                writeToLog(getCurrentDateTimeFormatted() + "\t" + line.Replace(".", ","), logPathBoden);
                addDataset(dataSource.Boden, dobj);
            }

            checkTimeSpan();
        }

        static public string getCurrentDateTimeFormatted()
        {
            return DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
        }

        private bool checkReconnectionTries (ref SerialPort SerPort, string portName)
        {
            if (!di.ContainsKey(portName))
                di.Add(portName, maxConnRetriesInitValue); //Wenn nicht nicht existiert in Liste eintragen

            if (SerPort.IsOpen)
            {
                di[portName] = maxConnRetriesInitValue; //Anzahl erfolglose Verbindungsversuche zurück setzen
                return true;
            }
            else
            {
                if (di[portName] > maxConnRetries)
                {
                    return false;
                }
                else
                {
                    ++di[portName];
                    return true;
                }
            }
            
        }

        private bool tryConnect(ref SerialPort SerPort, string portName)
        {
            if (!checkReconnectionTries(ref SerPort, portName)) return false;
            
            try
            {
                if (SerPort.IsOpen) return true;
                // if Ser.IsOpen will be handled within checkReconnectionTries (see above)
                // --> return true: proceed, open SerPort (even it is already opened)
                // --> return false: stop and report that the conenction was not successful
                //TODO: change logic!

                SerPort.StopBits = COMSettings.DefaultStopBits;
                SerPort.DataBits = COMSettings.DefaultDataBits;
                SerPort.BaudRate = COMSettings.DefaultBaudRate;
                SerPort.DtrEnable = COMSettings.DefaultDtrEnable;
                SerPort.PortName = portName;
                SerPort.DataReceived -= Sp_DataReceived;
                SerPort.DataReceived += Sp_DataReceived;
                SerPort.Open();
                Thread.Sleep(250);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fehler beim Verbindungsaufbau: " + ex.Message);
            }
            
            return SerPort.IsOpen;
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            tmr.Enabled = false;
            tmr.Stop();

            try
            {
                spTisch.DataReceived -= Sp_DataReceived;
                spBoden.DataReceived -= Sp_DataReceived;

                Application.DoEvents();
                if (spBoden.IsOpen)
                    spBoden.Close();

                Application.DoEvents();
                if (spTisch.IsOpen)
                    spTisch.Close();

                Application.DoEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Schließen der COM Verbindungen: " + ex.Message, 
                                "Fehler", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Error);
            }
        }

        private void checkTimeSpan()
        {
            //if one of them is not active return as a compare is not possible/reasonable
            if (!tischAktiv || !bodenAktiv) return;

            TimeSpan ts = timeStampLastUpdateBoden.Subtract(timeStampLastUpdateTisch);
            double diff = ts.TotalSeconds;
            if (diff < 0) diff *= -1; //must be a positiv number, if A<B than A-B could be -60 so -60 * -1 = 60
            if (diff > maxTimeDifferenceReadData)            //if there is a greater difference then notify user
            {
                lblTempTisch.ForeColor = System.Drawing.Color.Red;
                lblTempBoden.ForeColor = System.Drawing.Color.Red;
            } else
            {
                lblTempTisch.ForeColor = System.Drawing.Color.Black;
                lblTempBoden.ForeColor = System.Drawing.Color.Black;
            }
            
        }

        private void chkTopMost_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = (chkTopMost.Checked);
        }

        private void lblTempTisch_DoubleClick(object sender, EventArgs e)
        {
            changeLabelFont(ref lblTempTisch);
        }

        private void changeLabelFont(ref Label lbl)
        {
            using (FontDialog fd = new FontDialog())
            {
                fd.Font = lbl.Font;
                if (fd.ShowDialog() == DialogResult.OK)
                    lbl.Font = fd.Font;
            }
        }

        WindowsIdentity _currentUser = WindowsIdentity.GetCurrent();
        WindowsPrincipal _currentPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());

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

        public bool HasAccess(DirectoryInfo directory, FileSystemRights right)
        {
            // Get the collection of authorization rules that apply to the directory.
            AuthorizationRuleCollection acl = directory.GetAccessControl()
                .GetAccessRules(true, true, typeof(SecurityIdentifier));
            return HasFileOrDirectoryAccess(right, acl);
        }

        public bool HasAccess(FileInfo file, FileSystemRights right)
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

        private bool HasFileOrDirectoryAccess(FileSystemRights right,
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

        private void writeToLog(string text, string path)
        {
            Console.WriteLine("writeToLog called");
            if (!this.chkLogEnabled.Checked)
            {
                Console.WriteLine("writeToLog - logEnabled is not checked, exit ...");
                return;
            }

            FileInfo fi = new FileInfo(path);

            if (!HasAccess(fi, FileSystemRights.WriteData))
            {
                Console.WriteLine("writeToLog - Has no access to directory '" + path + "' ");
                return;
            }
            
            if (!Directory.Exists(path.Substring(0, path.LastIndexOf("\\"))))
                Directory.CreateDirectory(path.Substring(0, path.LastIndexOf("\\")));

            if (!File.Exists(path))
                File.Create(path).Close();
            
            try
            {
                if (fi.Length > maxLogFileSize)
                    fi.Delete();
            }
            catch (Exception ex)
            {
                Console.WriteLine("writeToLog - Cannot delete File, Error: " + ex.Message);
            }

            using (StreamWriter sw = new StreamWriter(path, true))
            {
                sw.WriteLine(text.Replace("\n", "\t").Replace(": ", ":\t"));
            }
        }

        private string getHTMLBody()
        {
            return Properties.Resources.html_temperature_main_template; 
        }

        private void trackToDatabase()
        {
            AccessDatabase.TrackEnvironment("Testeintrag", (double)10.10, (double)20.20, (double)30.30, (double)40.40);
        }

        private void writeToHTML()
        {
            if (!chkHTML.Checked)
                return;
            
            if ((string.IsNullOrEmpty(tempDataTisch) && tischAktiv) || (string.IsNullOrEmpty(tempDataBoden) && bodenAktiv))
                return;

            if (!HasAccess(new FileInfo(PathHTML), FileSystemRights.WriteData))
                return;

            try
            {
                lblHTMLUpdated.Invoke((MethodInvoker)(() => lblHTMLUpdated.ForeColor = System.Drawing.Color.Black));
                using (StreamWriter sw = new StreamWriter(PathHTML))
                {
                    string ret = Properties.Resources.html_temperature_main_template;
                    if (tischAktiv)
                    {
                        ret = ret.Replace("&TEMP1", tempDataTisch);
                        ret = ret.Replace("&TABLE_TISCH", createHTMLTableString(dataTisch, "Daten Tisch:"));
                    }
                    else
                    {
                        ret = ret.Replace("&TEMP1", "");
                        ret = ret.Replace("&TABLE_TISCH", "");
                    }
                    if (bodenAktiv)
                    {
                        ret = ret.Replace("&TEMP2", tempDataBoden);
                        ret = ret.Replace("&TABLE_BODEN", createHTMLTableString(dataBoden, "Daten Boden:"));
                    }
                    else
                    {
                        ret = ret.Replace("&TEMP2", "Sensor 2 nicht aktiv");
                        ret = ret.Replace("&TABLE_BODEN", "Keine Daten fuer Sensor 2, nicht aktiv");
                    }
                    
                    ret = ret.Replace("&LASTUPDATE", getCurrentDateTimeFormatted());
                    ret = ret.Replace("&HTML_HEAD", XML.HTMLHead);
                    ret = ret.Replace("°", "&deg;");
                    sw.WriteLine(ret);
                }
            }
            catch (Exception ex)
            {
                chkHTML.Checked = false;
                lblHTMLUpdated.Invoke((MethodInvoker)(() => lblHTMLUpdated.ForeColor = System.Drawing.Color.Red));
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
            }
            lblHTMLUpdated.Invoke((MethodInvoker)(() => lblHTMLUpdated.Text = getCurrentDateTimeFormatted()));
        }

        private string createHTMLTableString(List<DataObject> lDojb, string title)
        {
            if (lDojb.Count < 1)
                return "";

            string decPoint = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            StringBuilder sb = new StringBuilder();
            sb.Clear();
            sb.AppendLine("</br><h3>" + title + "</h3>");
            sb.AppendLine(@"<table style=""width:100%"">");
            sb.AppendLine("  <tr>");
            sb.AppendLine("    <th>Datum und Uhrzeit</th>");
            sb.AppendLine("    <th>Temperatur (°C)</th>");
            sb.AppendLine("    <th>Luftfeuchtigkeit (%)</th>");
            sb.AppendLine("    <th>Heat Index (°C)</th>");
            sb.AppendLine("    <th>Luftdruck (mb)</th>");
            sb.AppendLine("    <th>Zusatz-Info</th>");
            sb.AppendLine("  </tr>");

            foreach(DataObject dobj in lDojb)
            {
                sb.AppendLine("  <tr>");
                sb.AppendLine("    <td>" + dobj.Timepoint.ToShortDateString() + " " + dobj.Timepoint.ToLongTimeString() + "</td>");

                if (dobj.DataAvailable)
                {
                    sb.AppendLine("    <td>" + ((string.IsNullOrEmpty(dobj.Temperature)) ? "No data" : dobj.Temperature.Replace(".", decPoint)) + "</td>");
                    sb.AppendLine("    <td>" + ((string.IsNullOrEmpty(dobj.Humidity)) ? "No data" : dobj.Humidity.Replace(".", decPoint)) + "</td>");
                    sb.AppendLine("    <td>" + ((string.IsNullOrEmpty(dobj.HeatIndex)) ? "No data" : dobj.HeatIndex.Replace(".", decPoint)) + "</td>");
                    sb.AppendLine("    <td>" + ((string.IsNullOrEmpty(dobj.AirPressure)) ? "No data" : dobj.AirPressure.Replace(".", decPoint)) + "</td>");
                    sb.AppendLine("    <td>" + ((string.IsNullOrEmpty(dobj.AdditionalInformation)) ? "No data" : dobj.AdditionalInformation.Replace(".", decPoint)) + "</td>");
                }
                else
                {
                    sb.AppendLine("    <td>No data</td>");
                    sb.AppendLine("    <td>No data</td>");
                    sb.AppendLine("    <td>No data</td>");
                    sb.AppendLine("    <td>No data</td>");
                    sb.AppendLine("    <td></td>");
                }

                sb.AppendLine("  </tr>");
            }

            sb.AppendLine("  </table> ");

            sb.AppendLine("</br>");
            return sb.ToString();
        }

        private void lblTempBoden_DoubleClick(object sender, EventArgs e)
        {
            changeLabelFont(ref lblTempBoden);
        }

        private void frmMain_DoubleClick(object sender, EventArgs e)
        {
            trackToDatabase();
        }

        private void chkTopMost_CheckedChanged_1(object sender, EventArgs e)
        {
            this.TopMost = chkTopMost.Checked;
        }

        private void numMaxEntries_Scroll(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(numMaxEntries, numMaxEntries.Value.ToString());
            lblHTMLNumEntriesHist.Text = "Anzahl Einträge: " + this.numMaxEntries.Value.ToString();
        }
    }

    sealed class XML
    {
        private static XmlDocument xDoc = new XmlDocument();
        private static bool XMLLoaded = false;
        public static string XMLFileName { get { return System.IO.Path.Combine(Application.StartupPath, "Temperature.xml"); } }
        public static bool XMLExists { get { return System.IO.File.Exists(XMLFileName); } }

        public static string Title { get { return getValue("/root/titel"); } }
        public static string FileHTML { get { return getValue("/root/FileHTML"); } }
        public static string HTMLHead { get { return getValue("/root/HTMLHEAD"); } }
        public static string TischPort { get { return getValue("/root/Tisch/Port"); } }
        public static string TischBezeichnung { get { return getValue("/root/Tisch/Bezeichnung"); } }
        public static string TischLogfile { get { return getValue("/root/Tisch/LogFile"); } }
        public static string BodenPort { get { return getValue("/root/Boden/Port"); } }
        public static string BodenBezeichnung { get { return getValue("/root/Boden/Bezeichnung"); } }
        public static string BodenLogfile { get { return getValue("/root/Boden/LogFile"); } }
        public static bool TischAktiv { get { return getValue("/root/Tisch/Aktiv").ToUpper() == "Y"; } }
        public static bool BodenAktiv { get { System.Diagnostics.Debug.Print("getValue('/root/Boden/Aktiv') = " + getValue("/root/Boden/Aktiv")); return getValue("/root/Boden/Aktiv").ToUpper() == "Y"; } }

        public static long maxLogFileSize
        {
            get
            {
                long fSize = 0;
                if (long.TryParse(getValue("/root/Boden/maxLogFileSize"), out fSize))
                    return fSize;
                else
                    return 1024 * 1024;
            }
        }

        public static int maxTimeDifferenceReadData
        {
            get
            {
                int iDiff = 0;
                if (int.TryParse(getValue("/root/Boden/maxTimeDifferenceReadData"), out iDiff))
                    return iDiff;
                else
                    return 60;
            }
        }

        private static string getValue(string nodePath)
        {
            try
            {
                if (!XMLLoaded) loadXML();
                XMLLoaded = true;
                XmlNode t = xDoc.SelectSingleNode(nodePath); 
                if (t == null)
                    return String.Empty;
                else
                    return t.InnerText.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return String.Empty;
            }
        }

        private static bool loadXML()
        {
            if (!XMLExists)
                return false;

            xDoc.Load(@"Temperature.xml");

            return true;
        }
    }
    
    public static class COMSettings
    {
        public static StopBits DefaultStopBits { get { return StopBits.One; } }
        public static int DefaultDataBits = 8;
        public static int DefaultBaudRate = 9600;
        public static bool DefaultDtrEnable = true;
    }

    public class DataObject
    {
        public string Temperature { get; set; }
        public string HeatIndex { get; set; }
        public string Humidity { get; set; }
        public DateTime Timepoint { get; set; }
        public string AirPressure { get; set; }
        public bool DataAvailable { get; set; }
        public string AdditionalInformation { get; set; }
        public DataObjectProtocol Protocol { get; set; }
    }

    public enum DataObjectProtocol
    {
        NONE = 0, 
        PROTOCOL_ONE = 1, //Luftfeuchtigkeit, Heat Index, Temperatur
        PROTOCOL_TWO = 2  //Luftfeuchtigkeit, Heat Index, Temperatur, Luftdruck
    }
}









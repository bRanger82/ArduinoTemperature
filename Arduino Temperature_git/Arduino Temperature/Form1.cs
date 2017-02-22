using System;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Web;

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
        private DataObjectExt dObjTisch = new DataObjectExt();
        private DataObjectExt dObjBoden = new DataObjectExt();
        private Dictionary<string, int> di = new Dictionary<string, int>(); //Anzahl Verbindungsversuche pro Port
        private DataObjectDetails dobjDetail = new DataObjectDetails();

        public frmMain()
        {
            InitializeComponent();
        }

        private void init()
        {
            loadSetttingsFromXML();

            lblHTMLNumEntriesHist.Text = "Anzahl Einträge: " + this.numMaxEntries.Value.ToString();
            setLabelFormat(lblTempTisch, lblTableLastUpdated);
            setLabelFormat(lblTempBoden, lblBottomLastUpdated);

            dataBoden.Clear();
            dataTisch.Clear();
        }

        
        private void initNewFromObjects()
        {
            lblSensorOne.Text = XML.TischBezeichnung;

            dObjTisch.Name = XML.TischBezeichnung;
            dObjTisch.Active = XML.TischAktiv;
            dObjTisch.Items.Clear();
            dObjTisch.DataAvailable = false;
            dObjBoden.Name = XML.BodenBezeichnung;
            dObjBoden.Active = XML.BodenAktiv;
            dObjBoden.Items.Clear();
            dObjBoden.DataAvailable = false;

        }

        private void checkForConnection()
        {
            isConnected = ((tischAktiv) ? spTisch.IsOpen : true && (bodenAktiv) ? spBoden.IsOpen : true);

            if (isConnected)
            {
                tmr.Interval = 10000;
                tmr.Elapsed += Tmr_Elapsed;
                tmr.Enabled = true;
                tmr.Start();
            }
        }

        private void loadSensorToComboBox()
        {
            if (XML.TischAktiv)
                this.cboSensors.Items.Add(XML.TischBezeichnung);

            if (XML.BodenAktiv)
                this.cboSensors.Items.Add(XML.BodenBezeichnung);

            if (this.cboSensors.Items.Count > 0)
            {
                this.cboSensors.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("Es sind keine Sensoren aktiv gesetzt!", "Konfigurationsfehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

                init();
                initNewFromObjects();
                checkAccessRights();
                connectToDevices();
                loadSensorToComboBox();
                checkForConnection();
            }
            catch (Exception ex)
            {
                isConnected = false;
                spTisch.DataReceived -= Sp_DataReceived;
                spBoden.DataReceived -= Sp_DataReceived;
                tmr.Enabled = false;
                tmr.Stop();
                MessageBox.Show(ex.Message, "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkAccessRights()
        {
            this.chkLogEnabled.Checked = false;
            this.chkHTML.Checked = false;

            FileInfo pathTable = new FileInfo(logPathTisch);
            FileInfo pathBottom = new FileInfo(logPathBoden);
            FileInfo pathHTML = new FileInfo(PathHTML);

            this.chkLogEnabled.Enabled = false;

            if (Permission.HasAccess(pathTable, FileSystemRights.WriteData) &&
                Permission.HasAccess(pathBottom, FileSystemRights.WriteData))
                this.chkLogEnabled.Enabled = true;

            this.chkHTML.Enabled = Permission.HasAccess(new FileInfo(PathHTML), FileSystemRights.WriteData);
        }

        private void connectToDevices()
        {
            connectDevice(ref spTisch, ref lblTableLastUpdated, strPortTisch, tischAktiv);
            connectDevice(ref spBoden, ref lblBottomLastUpdated, strPortBoden, bodenAktiv);
        }

        private void connectDevice(ref SerialPort comPort, ref Label lblUpdate, string strPort, bool active)
        {
            if (!active)
            {
                lblUpdate.Text = "Nicht verwendet (in der Konfiguration inaktiv gesetzt)";
                return;
            }

            lblUpdate.Text = "Verbindungsaufbau ...";

            if (tryConnect(ref comPort, strPort))
                lblUpdate.Text = "Warte auf Daten ...";
            else
                lblUpdate.Text = "Verbindungsaufbau fehlgeschlagen";

        }

        private void setLabelFormat(Control Parent, Label lblUpdate)
        {
            System.Drawing.Point pos = this.PointToScreen(lblUpdate.Location);
            pos = Parent.PointToClient(pos);
            lblUpdate.Parent = Parent;
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
            if (tischAktiv)
                if (!tryConnect(ref spTisch, strPortTisch))
                    lblTempTisch.Text = reconnectText(strPortTisch);

            if (bodenAktiv)
                if (!tryConnect(ref spBoden, strPortBoden))
                    lblTempBoden.Text = reconnectText(strPortBoden);

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

        private string getLineFromDataExt(string line, ref DataObjectExt dobj)
        {
            dobj.DataAvailable = false;
            dobj.Protocol = DataObjectProtocol.NONE;
            string returnValue = string.Empty;

            if (line.Contains("|"))
            {
                string[] values = line.Split('|');
                //Format: START|humidity|temperature|heatindex|....|EOF
                //This format was defined within the Arduino Sketch
                //  e.g. (random numbers): START|40.30|25.50|26.54|EOF 
                // START: Indicates a Start of a data-frame
                //  40.30 --> 40.30% Humidity (relative)
                //  25.50 --> 25.50°C Temperature
                //  26.54 --> 26.54°C Temperature/Heat-Index
                // EOF: End of Frame

                if (values.Length == 7 && values[0].StartsWith("START") && values[6].StartsWith("EOF")) //Protocoll second version
                {
                    dobj.addDataItem(DataObjectCategory.Humidity.Value, double.Parse(Common.replaceDecPoint(values[2].ToString())), DataObjectCategory.Humidity, Common.SensorValueType.Humidity);
                    dobj.addDataItem(DataObjectCategory.Temperature.Value, double.Parse(Common.replaceDecPoint(values[3].ToString())), DataObjectCategory.Temperature, Common.SensorValueType.Temperature);
                    dobj.addDataItem(DataObjectCategory.HeatIndex.Value, double.Parse(Common.replaceDecPoint(values[4].ToString())), DataObjectCategory.HeatIndex, Common.SensorValueType.Temperature);
                    dobj.addDataItem(DataObjectCategory.AirPressure.Value, double.Parse(Common.replaceDecPoint(values[5].ToString())), DataObjectCategory.AirPressure, Common.SensorValueType.AirPressure);
                    dobj.LastUpdated = DateTime.Now;
                    dobj.DataAvailable = true;
                    dobj.AdditionalInformation = "-";
                    dobj.Protocol = DataObjectProtocol.PROTOCOL_TWO;

                    returnValue = "Luftfeuchtigkeit: " + values[2].ToString() + " %\n" +
                           "Temperatur: " + values[3].ToString() + " °C\n" +
                           "'Heat Index': " + values[4].ToString() + " °C\n" +
                           "Luftdruck: " + values[5].ToString() + " mb\n";

                }
                else if (values.Length == 5 && values[0].StartsWith("START") && values[4].StartsWith("EOF")) //Protocol first version
                {
                    dobj.addDataItem(DataObjectCategory.Humidity.Value, double.Parse(Common.replaceDecPoint(values[1].ToString())), DataObjectCategory.Humidity, Common.SensorValueType.Humidity);
                    dobj.addDataItem(DataObjectCategory.Temperature.Value, double.Parse(Common.replaceDecPoint(values[2].ToString())), DataObjectCategory.Temperature, Common.SensorValueType.Temperature);
                    dobj.addDataItem(DataObjectCategory.HeatIndex.Value, double.Parse(Common.replaceDecPoint(values[3].ToString())), DataObjectCategory.HeatIndex, Common.SensorValueType.Temperature);
                    dobj.LastUpdated = DateTime.Now;
                    dobj.DataAvailable = true;
                    dobj.AdditionalInformation = "-";
                    dobj.Protocol = DataObjectProtocol.PROTOCOL_ONE;

                    returnValue = "Luftfeuchtigkeit: " + values[1].ToString() + " %\n" +
                           "Temperatur: " + values[2].ToString() + " °C\n" +
                           "'Heat Index': " + values[3].ToString() + " °C\n";
                }
                else if (values.Length == 8 && values[0].StartsWith("START") && values[7].StartsWith("EOF")) //Protocol third version
                {
                    dobj.addDataItem(DataObjectCategory.Humidity.Value, double.Parse(Common.replaceDecPoint(values[2].ToString())), DataObjectCategory.Humidity, Common.SensorValueType.Humidity);
                    dobj.addDataItem(DataObjectCategory.Temperature.Value, double.Parse(Common.replaceDecPoint(values[3].ToString())), DataObjectCategory.Temperature, Common.SensorValueType.Temperature);
                    dobj.addDataItem(DataObjectCategory.HeatIndex.Value, double.Parse(Common.replaceDecPoint(values[4].ToString())), DataObjectCategory.HeatIndex, Common.SensorValueType.Temperature);
                    dobj.addDataItem(DataObjectCategory.AirPressure.Value, double.Parse(Common.replaceDecPoint(values[5].ToString())), DataObjectCategory.AirPressure, Common.SensorValueType.AirPressure);
                    dobj.addDataItem(DataObjectCategory.LUX.Value, double.Parse(Common.replaceDecPoint(values[6].ToString())), DataObjectCategory.LUX, Common.SensorValueType.LUX);

                    dobj.LastUpdated = DateTime.Now;
                    dobj.DataAvailable = true;
                    dobj.AdditionalInformation = "-";
                    dobj.Protocol = DataObjectProtocol.PROTOCOL_THREE;
                    returnValue = "Luftfeuchtigkeit: " + values[2].ToString() + " %\n" +
                           "Temperatur: " + values[3].ToString() + " °C\n" +
                           "'Heat Index': " + values[4].ToString() + " °C\n" +
                           "Luftdruck: " + values[5].ToString() + " mb\n" +
                           "LUX: " + values[6].ToString() + " lux\n";

                }
                else
                {
                    dobj.LastUpdated = DateTime.Now;
                    dobj.DataAvailable = false;
                    dobj.AdditionalInformation = "-";
                    dobj.Protocol = DataObjectProtocol.NONE;

                    returnValue = "Fehler beim Lesen der Daten";
                }
            }
            else
            {
                dobj.DataAvailable = false;
                dobj.AdditionalInformation = "-";
                dobj.Protocol = DataObjectProtocol.NONE;

                returnValue = line;
            }

            return returnValue;
        }

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
                    dobj.LUX = string.Empty;
                    dobj.Humidity = Common.replaceDecPoint(values[2].ToString());
                    dobj.Temperature = Common.replaceDecPoint(values[3].ToString());
                    dobj.HeatIndex = Common.replaceDecPoint(values[4].ToString());
                    dobj.AirPressure = Common.replaceDecPoint(values[5].ToString());
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
                    dobj.LUX = string.Empty;
                    dobj.Humidity = Common.replaceDecPoint(values[1].ToString());
                    dobj.Temperature = Common.replaceDecPoint(values[2].ToString());
                    dobj.HeatIndex = Common.replaceDecPoint(values[3].ToString());
                    dobj.Timepoint = DateTime.Now;
                    dobj.DataAvailable = true;
                    dobj.AdditionalInformation = "-";
                    dobj.Protocol = DataObjectProtocol.PROTOCOL_ONE;

                    returnValue = "Luftfeuchtigkeit: " + values[1].ToString() + " %\n" +
                           "Temperatur: " + values[2].ToString() + " °C\n" +
                           "'Heat Index': " + values[3].ToString() + " °C\n";
                } else if (values.Length == 8 && values[0].StartsWith("START") && values[7].StartsWith("EOF")) //Protocol third version
                {
                    dobj.Humidity = Common.replaceDecPoint(values[2].ToString());
                    dobj.Temperature = Common.replaceDecPoint(values[3].ToString());
                    dobj.HeatIndex = Common.replaceDecPoint(values[4].ToString());
                    dobj.AirPressure = Common.replaceDecPoint(values[5].ToString());
                    dobj.LUX = Common.replaceDecPoint(values[6].ToString());
                    
                    dobj.Timepoint = DateTime.Now;
                    dobj.DataAvailable = true;
                    dobj.AdditionalInformation = "-";
                    dobj.Protocol = DataObjectProtocol.PROTOCOL_THREE;
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

        private void loopGrpBox(GroupBox grp)
        {
            foreach(Control ctr in grp.Controls)
            {
                Console.WriteLine("GrpBox.Name :: Control.Name {0} {1}", grp.Name, ctr.Name);
                if (ctr is GroupBox)
                    loopGrpBox((GroupBox)ctr);
            }
        }

        private void test(Label lblValue, Label lblMin, Label lblMax, string value, string unit)
        {
            double val;
            lblValue.Text = value + unit;
            if (double.TryParse(value, out val))
            {
                if (val < dobjDetail.HumidityDetail.MinValue)
                {
                    dobjDetail.HumidityDetail.MinValue = val;
                    dobjDetail.HumidityDetail.MinTimepoint = DateTime.Now;
                    lblSensorOneHumidityValueMin.Text = dobjDetail.HumidityDetail.MinValue.ToString("#.00") + Common.getSensorValueUnit(Common.SensorValueType.Humidity);
                    lblSensorOneHumidityValueMinTime.Text = Common.getCurrentDateTimeFormattedNoSec(dobjDetail.HumidityDetail.MinTimepoint);
                }
                if (val > dobjDetail.HumidityDetail.MaxValue)
                {
                    dobjDetail.HumidityDetail.MaxValue = val;
                    dobjDetail.HumidityDetail.MinTimepoint = DateTime.Now;
                    lblSensorOneHumidityValueMax.Text = dobjDetail.HumidityDetail.MaxValue.ToString("#.00") + Common.getSensorValueUnit(Common.SensorValueType.Humidity);
                    lblSensorOneHumidityValueMaxTime.Text = Common.getCurrentDateTimeFormattedNoSec(dobjDetail.HumidityDetail.MaxTimepoint);
                }

            }
        }

        private void setLabelInformation(Label lblValue, Label lblMinValue, Label lblMaxValue, Label lblMinTime, Label lblMaxTime, DataObjectExt dObjExt, DataObjectCategory dobjcat)
        {
            if (dObjExt.ItemExists(dobjcat) && DataObjectCapabilities.HasCapability(dObjExt.Items[dobjcat.Value].DataObjCategory, dObjExt.Protocol))
            {
                string unit = Common.getSensorValueUnit(dObjExt.Items[dobjcat.Value].SensorType);

                lblValue.Text = dObjExt.Items[dobjcat.Value].Value.ToString("#.#0") + unit;
                lblMinValue.Text = dObjExt.Items[dobjcat.Value].MinValue.ToString("#.#0") + unit;
                lblMaxValue.Text = dObjExt.Items[dobjcat.Value].MaxValue.ToString("#.#0") + unit;
                lblMinTime.Text = Common.getCurrentDateTimeFormattedNoSec(dObjExt.Items[dobjcat.Value].MinTimepoint);
                lblMaxTime.Text = Common.getCurrentDateTimeFormattedNoSec(dObjExt.Items[dobjcat.Value].MaxTimepoint);
            }
        }


        private void LineReceived(string newline, string comPort)
        {

            DataObject dobj = new DataObject();
            string line = getLineFromData(newline, out dobj);
            
            if (comPort == strPortTisch)
            {
                line = getLineFromDataExt(newline, ref dObjTisch);
                timeStampLastUpdateTisch = DateTime.Now;
                lblTempTisch.Text = XML.TischBezeichnung + " (" + strPortTisch + ")\n" + line;
                lblTableLastUpdated.Text = "Aktualisiert: " + Common.getCurrentDateTimeFormatted();
                tempDataTisch = lblTempTisch.Text;
                tempDataTisch= tempDataTisch.Replace("\n", ".br.");
                tempDataTisch = HttpUtility.HtmlEncode(tempDataTisch); // "TISCH</br>" + line.Replace("\n", "</br>");
                tempDataTisch = tempDataTisch.Replace(".br.", "</br>");
                writeToLog(Common.getCurrentDateTimeFormatted() + "\t" + line.Replace(".", ","), logPathTisch);
                addDataset(dataSource.Tisch, dobj);
                if (dobj.DataAvailable)
                {
                    setLabelInformation(lblSensorOneTempValue, lblSensorOneTempMin, lblSensorOneTempMax, lblSensorOneTempMinTime, lblSensorOneTempMaxTime, dObjTisch, DataObjectCategory.Temperature);
                    setLabelInformation(lblSensorOneLuxValue, lblSensorOneLuxMin, lblSensorOneLuxMax, lblSensorOneLuxMinTime, lblSensorOneLuxMaxTime, dObjTisch, DataObjectCategory.LUX);
                    setLabelInformation(lblSensorOneHumidityValue, lblSensorOneHumidityValueMin, lblSensorOneHumidityValueMax, lblSensorOneHumidityValueMinTime, lblSensorOneHumidityValueMaxTime, dObjTisch, DataObjectCategory.Humidity);
                    setLabelInformation(lblSensorOnePressureValue, lblSensorOnePressureMin, lblSensorOnePressureMax, lblSensorOnePressureMinTime, lblSensorOnePressureMaxTime, dObjTisch, DataObjectCategory.AirPressure);
                    setLabelInformation(lblSensorHeatIndexValue, lblSensorHeatIndexMin, lblSensorHeatIndexMax, lblSensorHeatIndexMinTime, lblSensorHeatIndexMaxTime, dObjTisch, DataObjectCategory.HeatIndex);
                }
                else
                {
                    lblSensorOneHumidityValue.Text = "Fehler";
                    lblSensorOneTempValue.Text = "Fehler";
                    if (dobj.Protocol == DataObjectProtocol.PROTOCOL_ONE)
                    {
                        lblSensorOneLuxValue.Text = "N/A";
                        lblSensorOnePressureValue.Text = "N/A";
                    }
                    else if (dobj.Protocol == DataObjectProtocol.PROTOCOL_TWO)
                    {
                        lblSensorOneLuxValue.Text = "N/A";
                        lblSensorOnePressureValue.Text = "Fehler";
                    }
                    else if (dobj.Protocol == DataObjectProtocol.PROTOCOL_THREE)
                    {
                        lblSensorOneLuxValue.Text = "Fehler";
                        lblSensorOnePressureValue.Text = "Fehler";
                    }
                }

                lblSensorOneLastUpdated.Text = "Aktualisiert: " + Common.getCurrentDateTimeFormatted();

            } else if (comPort == strPortBoden)
            {
                line = getLineFromDataExt(newline, ref dObjBoden);
                timeStampLastUpdateBoden = DateTime.Now;
                lblTempBoden.Text = XML.BodenBezeichnung + " (" + strPortBoden + ")\n" + line;
                lblBottomLastUpdated.Text = "Aktualisiert: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
                tempDataBoden = lblTempBoden.Text;
                tempDataBoden = tempDataBoden.Replace("\n", ".br.");
                tempDataBoden = HttpUtility.HtmlEncode(tempDataBoden); // "TISCH</br>" + line.Replace("\n", "</br>");
                tempDataBoden = tempDataBoden.Replace(".br.", "</br>");
                writeToLog(Common.getCurrentDateTimeFormatted() + "\t" + line.Replace(".", ","), logPathBoden);
                addDataset(dataSource.Boden, dobj);
            }

            checkTimeSpan();
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

                SerPort.StopBits = Common.COMSettings.DefaultStopBits;
                SerPort.DataBits = Common.COMSettings.DefaultDataBits;
                SerPort.BaudRate = Common.COMSettings.DefaultBaudRate;
                SerPort.DtrEnable = Common.COMSettings.DefaultDtrEnable;
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

        private void writeToLog(string text, string path)
        {
            Console.WriteLine("writeToLog called");
            if (!this.chkLogEnabled.Checked)
            {
                Console.WriteLine("writeToLog - logEnabled is not checked, exit ...");
                return;
            }

            FileInfo fi = new FileInfo(path);

            if (!Permission.HasAccess(fi, FileSystemRights.WriteData))
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


        private void writeToHTML()
        {
            if (!chkHTML.Checked)
                return;
            
            if ((string.IsNullOrEmpty(tempDataTisch) && tischAktiv) || (string.IsNullOrEmpty(tempDataBoden) && bodenAktiv))
                return;

            if (!Permission.HasAccess(new FileInfo(PathHTML), FileSystemRights.WriteData))
                return;

            try
            {
                lblHTMLUpdated.Invoke((MethodInvoker)(() => lblHTMLUpdated.ForeColor = System.Drawing.Color.Black));

                HTML.writeHTMLFile(PathHTML, tischAktiv, tempDataTisch, dataTisch, bodenAktiv, tempDataBoden, dataBoden);
            }
            catch (Exception ex)
            {
                chkHTML.Checked = false;
                lblHTMLUpdated.Invoke((MethodInvoker)(() => lblHTMLUpdated.ForeColor = System.Drawing.Color.Red));
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
            }
            lblHTMLUpdated.Invoke((MethodInvoker)(() => lblHTMLUpdated.Text = Common.getCurrentDateTimeFormatted()));
        }
        
        private void lblTempBoden_DoubleClick(object sender, EventArgs e)
        {
            changeLabelFont(ref lblTempBoden);
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

    
    



}









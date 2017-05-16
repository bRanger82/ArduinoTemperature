using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Arduino_Temperature_Retrofit
{
    public partial class frmMain : Form
    {

        Dictionary<string, DataObject> dataObjs = new Dictionary<string, DataObject>();
        HTMLSettings htmlSettings = new HTMLSettings();

        Timer tmrCheckConnStatus = new Timer();
        Timer tmrFileWriter = new Timer(); 

        public frmMain()
        {
            InitializeComponent();
        }
        
        public void loadHtmlSettings()
        {
            htmlSettings.Enabled = XML.HtmlEnabled;
            htmlSettings.Filename = XML.HtmlFile;
            htmlSettings.HeadText = XML.HtmlHeadText;
            htmlSettings.UpdateFrequency = XML.HttpUpdateFrequency();
            htmlSettings.LastRun = DateTime.Now.AddYears(-1); //initial value
        }

        private void LoadDataObjects()
        {
            List<XMLSensorObject> xmlSensors = XML.getSensorItemsFromXML();
            foreach (XMLSensorObject xmlSensor in xmlSensors)
            {
                DataObject dobj = new DataObject();
                
                dobj.Name = xmlSensor.Name;
                dobj.Active = xmlSensor.Active;
                
                dobj.MaxHistoryItemsSet = xmlSensor.numLogEntries;
                dobj.LoggingEnabled = xmlSensor.LogEnabled;
                dobj.LogPath = xmlSensor.LogFilePath;
                dobj.maxLogFileSize = xmlSensor.maxLogFileSize;
                dobj.HTMLEnabled = xmlSensor.HTMLEnabled;
                dobj.DataInterfaceType = xmlSensor.Protocol; // Determinate if HTTP or COM is used

                if (dobj.DataInterfaceType == XMLProtocol.COM) //if COM hook up a listener
                {
                    dobj.PortName = xmlSensor.Port;
                    dobj.BaudRate = xmlSensor.Baudrate;
                    dobj.DataBits = Common.COMSettings.DefaultDataBits;
                    dobj.DtrEnable = xmlSensor.DtrEnabled;
                    dobj.StopBits = Common.COMSettings.DefaultStopBits;
                    if (dobj.Active)
                    {
                        dobj.DataReceived += Dobj_DataReceived;
                        dobj.Open();
                    }
                } else if (dobj.DataInterfaceType == XMLProtocol.HTTP)
                {
                    dobj.URL = xmlSensor.URL;
                }
                dataObjs.Add(dobj.Name, dobj);
            }
        }

        private void getHTTPData(string url, DataObject dobj)
        {
            try
            {

                Task<string> result = HTML.DownloadPage(url);

                foreach (string l in result.Result.Split('\n'))
                {
                    if (l.Contains("START") && l.EndsWith("EOF"))
                    {
                        DataReceived(l, dobj.Name);
                        break;
                    } else if (l.Contains("Salzburg"))
                    {
                        MessageBox.Show(l);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception: getHTTPData: " + ex.Message);
            }

        }

        private void Dobj_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (sender is DataObject)
            {
                DataObject spTemp = (DataObject)sender;
                string received = spTemp.ReadLine();
                this.BeginInvoke(new DataReceiveEvent(DataReceived), received, spTemp.Name);
            }
        }

        private delegate void DataReceiveEvent(string information, string Name);
        
        private void DataReceived(string information, string name)
        {
            DataObject dobj = dataObjs[name];
            if (information.Contains("|"))
            {
                try
                {
                    string[] values = information.Split('|');
                    if (values.Length == 5 && values[0].StartsWith("START") && values[4].StartsWith("EOF")) //Protocol first version
                    {
                        processDataProtocolV1(values, ref dobj);
                    } else if (values.Length == 7 && values[0].StartsWith("START") && values[6].StartsWith("EOF")) //Protocoll second version
                    {
                        processDataProtocolV2(values, ref dobj);
                    } else if (values.Length == 8 && values[0].StartsWith("START") && values[7].StartsWith("EOF")) //Protocol third version
                    {
                        processDataProtocolV3(values, ref dobj);
                    } else if (values[0].StartsWith("REPLY") && values[values.Length - 1].StartsWith("EOF")) // returns all commands which can be handled by the arduino
                    {
                        parseArduinoReply(dobj, values);
                        return;
                    } else
                    {
                        dobj.DataAvailable = false;
                        dobj.LastUpdated = DateTime.Now;
                        dobj.Protocol = DataObjectProtocol.NONE;
                        dobj.AdditionalInformation = "Daten empfangen: Datenprotokoll unbekannt";
                        Console.WriteLine(information);
                    }
                }
                catch (Exception)
                {
                    dobj.DataAvailable = false;
                    dobj.LastUpdated = DateTime.Now;
                    dobj.Protocol = DataObjectProtocol.NONE;
                    dobj.AdditionalInformation = "Fehler bei der Verarbeitung, warte auf nächste Datensatz.";
                }
            } else
            {
                dobj.DataAvailable = false;
                dobj.LastUpdated = DateTime.Now;
                dobj.Protocol = DataObjectProtocol.NONE;
                dobj.AdditionalInformation = information;
            }

            if (dobj.DataAvailable)
                processIncomingDataSet(ref dobj, name);

        }

        private void parseArduinoReply(DataObject dobj, string[] items)
        {
            string message = string.Empty;
            for (int pos = 1; pos < items.Length - 1; pos++)
                message += "\n" + items[pos];
            Console.WriteLine(message);

            MessageBox.Show("Received reply:\n" + message);
        }

        private string getToolTip(DataObject dobj)
        {
            if (dobj.DataInterfaceType == XMLProtocol.COM)
            {
                return "Name: \t" + dobj.Name + "\n" +
                       "Aktiv: \t" + ((dobj.Active) ? "Ja" : "Nein") + "\n" +
                       "Protokoll:\t" + Enum.GetName(typeof(XMLProtocol), dobj.DataInterfaceType) + "\n" + 
                       "Port: \t" + dobj.PortName + "\n" +
                       "Baud-Rate: \t" + dobj.BaudRate + "\n" + 
                       "Log aktiviert: \t" + ((dobj.LoggingEnabled) ? "Ja" : "Nein") + "\n" +
                       "HTML aktiviert: \t" + ((dobj.HTMLEnabled) ? "Ja" : "Nein") + "\n";
            } else if (dobj.DataInterfaceType == XMLProtocol.HTTP)
            {
                return "Name: \t" + dobj.Name + "\n" +
                       "Aktiv: \t" + ((dobj.Active) ? "Ja" : "Nein") + "\n" +
                       "Protokoll:\t" + Enum.GetName(typeof(XMLProtocol), dobj.DataInterfaceType) + "\n" +
                       "URL: \t" + dobj.URL+ "\n" +
                       "Log aktiviert: \t" + ((dobj.LoggingEnabled) ? "Ja" : "Nein") + "\n" +
                       "HTML aktiviert: \t" + ((dobj.HTMLEnabled) ? "Ja" : "Nein") + "\n";
            } else
            {
                return "ERROR: Nicht definiert!";
            }
            
        }

        private void UpdateStatus(DataObject dobj)
        {
            this.picConnStatus.BackColor = System.Windows.Forms.Control.DefaultBackColor; 
            Color col = System.Windows.Forms.Control.DefaultBackColor;
            if (dobj.DataInterfaceType == XMLProtocol.COM)
            {
                if (dobj.Active)
                {
                    if (dobj.IsOpen)
                    {
                        col = System.Drawing.Color.Green;
                        this.toolTip1.SetToolTip(picConnStatus, "STATUS: Verbunden\n" + getToolTip(dobj));
                    }
                    else
                    {
                        col = System.Drawing.Color.Red;
                        this.toolTip1.SetToolTip(picConnStatus, "STATUS: KEINE VERBINDUNG\n" + getToolTip(dobj));
                    }
                }
                else
                {
                    col = System.Drawing.Color.LightBlue;
                    this.toolTip1.SetToolTip(picConnStatus, "STATUS: Nicht Aktiv\n" + getToolTip(dobj));
                }
            } else if (dobj.DataInterfaceType == XMLProtocol.HTTP)
            {
                col = System.Drawing.Color.DarkBlue;
                this.toolTip1.SetToolTip(picConnStatus, "STATUS: HTTP wird verwendet\n" + getToolTip(dobj));
            }
            
            SolidBrush myBrush = new SolidBrush(col);
            
            Graphics g = picConnStatus.CreateGraphics();

            g.FillEllipse(myBrush, new Rectangle(0, 0, picConnStatus.Size.Width, picConnStatus.Size.Height));
            
        }

        private void writeCommandToArduino(DataObject dobj, string command)
        {
            //Has to be implemented together with ARDUINO and the C# method parseArduinoReply
            if (dobj.IsOpen)
            {
                dobj.Write(command);
            }
        }

        private void processIncomingDataSet(ref DataObject dobj, string name)
        {
            if (dobj.Protocol != DataObjectProtocol.NONE)
            {
                if (name == this.cboSensors.GetItemText(this.cboSensors.SelectedItem) && dobj.FirstData)
                {
                    if (dobj.FirstData && cboChartSelection.Items.Count == 0)
                    {
                        addChartPossibilities();
                    }

                    showData(dobj);
                    updateChart(dobj);
                    lblSensorLastUpdated.ForeColor = SystemColors.ControlText;
                }
            }
            else
            {
                if (dobj.FirstData)
                {
                    lblSensorLastUpdated.ForeColor = Color.DarkRed;
                    lblSensorLastUpdated.Text = dobj.AdditionalInformation;
                }
                else
                {
                    lblSensorLastUpdated.Text = "Warte auf Daten";
                }
            }
        }

        private void processDataProtocolV1(string [] data, ref DataObject dobj)
        {
            dobj.addDataItem(DataObjectCategory.Luftfeuchtigkeit.Value, double.Parse(Common.replaceDecPoint(data[1].ToString())), DataObjectCategory.Luftfeuchtigkeit);
            dobj.addDataItem(DataObjectCategory.Temperatur.Value, double.Parse(Common.replaceDecPoint(data[2].ToString())), DataObjectCategory.Temperatur);
            dobj.addDataItem(DataObjectCategory.HeatIndex.Value, double.Parse(Common.replaceDecPoint(data[3].ToString())), DataObjectCategory.HeatIndex);
            dobj.LastUpdated = DateTime.Now;
            dobj.DataAvailable = true;
            dobj.AdditionalInformation = "-";
            dobj.Protocol = DataObjectProtocol.PROTOCOL_ONE;
            dobj.FirstData = true;
        }

        private void processDataProtocolV2(string[] data, ref DataObject dobj)
        {
            dobj.addDataItem(DataObjectCategory.Luftfeuchtigkeit.Value, double.Parse(Common.replaceDecPoint(data[2].ToString())), DataObjectCategory.Luftfeuchtigkeit);
            dobj.addDataItem(DataObjectCategory.Temperatur.Value, double.Parse(Common.replaceDecPoint(data[3].ToString())), DataObjectCategory.Temperatur);
            dobj.addDataItem(DataObjectCategory.HeatIndex.Value, double.Parse(Common.replaceDecPoint(data[4].ToString())), DataObjectCategory.HeatIndex);
            dobj.addDataItem(DataObjectCategory.Luftdruck.Value, double.Parse(Common.replaceDecPoint(data[5].ToString())), DataObjectCategory.Luftdruck);
            dobj.LastUpdated = DateTime.Now;
            dobj.DataAvailable = true;
            dobj.AdditionalInformation = "-";
            dobj.Protocol = DataObjectProtocol.PROTOCOL_TWO;
            dobj.FirstData = true;
        }

        private void processDataProtocolV3(string[] data, ref DataObject dobj)
        {
            dobj.addDataItem(DataObjectCategory.Luftfeuchtigkeit.Value, double.Parse(Common.replaceDecPoint(data[2].ToString())), DataObjectCategory.Luftfeuchtigkeit);
            dobj.addDataItem(DataObjectCategory.Temperatur.Value, double.Parse(Common.replaceDecPoint(data[3].ToString())), DataObjectCategory.Temperatur);
            dobj.addDataItem(DataObjectCategory.HeatIndex.Value, double.Parse(Common.replaceDecPoint(data[4].ToString())), DataObjectCategory.HeatIndex);
            dobj.addDataItem(DataObjectCategory.Luftdruck.Value, double.Parse(Common.replaceDecPoint(data[5].ToString())), DataObjectCategory.Luftdruck);
            dobj.addDataItem(DataObjectCategory.Lichtwert.Value, double.Parse(Common.replaceDecPoint(data[6].ToString())), DataObjectCategory.Lichtwert);

            dobj.LastUpdated = DateTime.Now; 
            dobj.DataAvailable = true;
            dobj.AdditionalInformation = "-";
            dobj.Protocol = DataObjectProtocol.PROTOCOL_THREE;
            dobj.FirstData = true;
        }

        private void showData(DataObject dobj)
        {
            if (dobj.DataAvailable)
            {
                setLabelInformation(lblSensorTempValue, lblSensorTempMin, lblSensorTempMax, lblSensorTempMinTime, lblSensorTempMaxTime, dobj, DataObjectCategory.Temperatur, picTrendTemp);
                setLabelInformation(lblSensorLuxValue, lblSensorLuxMin, lblSensorLuxMax, lblSensorLuxMinTime, lblSensorLuxMaxTime, dobj, DataObjectCategory.Lichtwert, picTrendLUX);
                setLabelInformation(lblSensorHumidityValue, lblSensorHumidityValueMin, lblSensorHumidityValueMax, lblSensorHumidityValueMinTime, lblSensorHumidityValueMaxTime, dobj, DataObjectCategory.Luftfeuchtigkeit, picTrendHumidity);
                setLabelInformation(lblSensorPressureValue, lblSensorPressureMin, lblSensorPressureMax, lblSensorPressureMinTime, lblSensorPressureMaxTime, dobj, DataObjectCategory.Luftdruck, picTrendAirPressure);
                setLabelInformation(lblSensorHeatIndexValue, lblSensorHeatIndexMin, lblSensorHeatIndexMax, lblSensorHeatIndexMinTime, lblSensorHeatIndexMaxTime, dobj, DataObjectCategory.HeatIndex, picTrendHeatIndex);
                lblSensorLastUpdated.Text = "Zuletzt aktualisiert: " + Common.getCurrentDateTimeFormatted();
            }
        }

        private Image getTrend(DataObject dobj, DataObjectCategory dobjCat, out string trendInfo)
        {
            Image img = null;
            Trend trend = dobj.getTrend(dobjCat);
            string lblTrend = string.Empty;
            trendInfo = string.Empty;

            switch (trend)
            {
                case Trend.CONSTANT: img = new Bitmap(Properties.Resources.Trend_Same); trendInfo = "Trend: gleichbleibend"; break;
                case Trend.DOWN: img = new Bitmap(Properties.Resources.Trend_Down); trendInfo = "Trend: fallend";  break;
                case Trend.UP: img = new Bitmap(Properties.Resources.Trend_UP); trendInfo = "Trend: steigend"; break;
            }
            return img;
        }

        private void setLabelInformation(Label lblValue, Label lblMinValue, Label lblMaxValue, Label lblMinTime, Label lblMaxTime, DataObject dObjExt, DataObjectCategory dobjcat, PictureBox picTrend)
        {
            if (dObjExt.ItemExists(dobjcat) && DataObjectCategory.HasCapability(dObjExt.Items[dobjcat.Value].DataObjCategory, dObjExt.Protocol))
            {
                string unit = DataObjectCategory.getSensorValueUnit(dobjcat);

                lblValue.Text = dObjExt.Items[dobjcat.Value].Value.ToString("F") + unit;
                lblMinValue.Text = dObjExt.Items[dobjcat.Value].MinValue.ToString("F") + unit;
                lblMaxValue.Text = dObjExt.Items[dobjcat.Value].MaxValue.ToString("F") + unit;
                lblMinTime.Text = Common.getCurrentDateTimeFormattedNoSec(dObjExt.Items[dobjcat.Value].MinTimepoint);
                lblMaxTime.Text = Common.getCurrentDateTimeFormattedNoSec(dObjExt.Items[dobjcat.Value].MaxTimepoint);
                string trendInfo;
                picTrend.Image = getTrend(dObjExt, dobjcat, out trendInfo);
                toolTip1.SetToolTip(picTrend, trendInfo);
                lblValue.Parent.Enabled = true;
            }
            else
            {
                lblValue.Text = " --- ";
                lblMinValue.Text = " --- ";
                lblMaxValue.Text = " --- ";
                lblMinTime.Text = " --- ";
                lblMaxTime.Text = " --- ";
                picTrend.Image = null;
                picTrend.BackColor = SystemColors.Control;
                lblValue.Parent.Enabled = false;
            }
        }

        private Color getChartColor(DataObjectCategory dobjCat)
        {
            if (dobjCat.Value == DataObjectCategory.HeatIndex.Value)
                return picColHeatIndex.BackColor;
            else if (dobjCat.Value == DataObjectCategory.Temperatur.Value)
                return picColTemp.BackColor;
            else if (dobjCat.Value == DataObjectCategory.Lichtwert.Value)
                return picColLUX.BackColor;
            else if (dobjCat.Value == DataObjectCategory.Luftfeuchtigkeit.Value)
                return picColHumidity.BackColor;
            else if (dobjCat.Value == DataObjectCategory.Luftdruck.Value)
                return picColAirPressure.BackColor;
            else
                return Color.Red;
        }

        private void setTimerFileWriter(bool enabled)
        {
            if (!enabled)
            {
                tmrFileWriter.Enabled = false;
                tmrFileWriter.Stop();
                return;
            }

            tmrFileWriter.Interval = 1000;
            tmrFileWriter.Enabled = true;
            tmrFileWriter.Tick -= TmrFileWriter_Tick;
            tmrFileWriter.Tick += TmrFileWriter_Tick;
            tmrFileWriter.Start();
        }

        private void writeHTML()
        {
            try
            {
                if (htmlSettings.Enabled)
                {
                    HTML.writeHTMLFile(htmlSettings.Filename, dataObjs, htmlSettings.HeadText);
                    htmlSettings.LastRun = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                //Inform the user? What if the programm runs in during overnight? show message-boxes every run? ;)
                Console.WriteLine(ex.Message);
            }
        }

        private void TmrFileWriter_Tick(object sender, EventArgs e)
        {
            //html
            if (DateTime.Now.Subtract(htmlSettings.LastRun).TotalSeconds > htmlSettings.UpdateFrequency)
            {
                Console.WriteLine("writeHTML");
                writeHTML();
            }
            Console.WriteLine("TmrFileWriter_Tick");
        }

        private void connectionCheck(bool enabled)
        {
            if (!enabled)
            {
                tmrCheckConnStatus.Enabled = false;
                tmrCheckConnStatus.Stop();
                return;
            }
            tmrCheckConnStatus.Interval = 2000;
            tmrCheckConnStatus.Enabled = true;
            tmrCheckConnStatus.Tick -= TmrCheckConnStatus_Tick;
            tmrCheckConnStatus.Tick += TmrCheckConnStatus_Tick;
            tmrCheckConnStatus.Start();
        }

        private void TmrCheckConnStatus_Tick(object sender, EventArgs e)
        {
            if (cboSensors.Items.Count < 1)
                return;

            DataObject dobj = getAcutalDataObject();
            int maxConnectionRetries = 10;

            if (dobj == null)
                return;

            UpdateStatus(dobj);

            if (dobj.DataInterfaceType == XMLProtocol.HTTP)
            {
                string showURL = dobj.URL;
                if (dobj.URL.Length > 25)
                    showURL = dobj.URL.Substring(0, 25) + "...";
                lblSensorLastUpdated.Text = "HTTP URL: " + showURL;
                return;
            }
            if (dobj.Active && !dobj.IsOpen)
            {
                lblSensorLastUpdated.ForeColor = Color.DarkRed;

                if (dobj.ConnectionRetries >= maxConnectionRetries)
                {
                    lblSensorLastUpdated.Text = "Fehler: Keine Verbindung nach 10 Versuchen!";
                    return;
                }

                lblSensorLastUpdated.Text = "Fehler: Keine Verbindung, Verbindungsversuch ...";
                dobj.increaseConnectionRetry();

                try
                {
                    dobj.Open();
                }
                catch (Exception ex)
                {
                    lblSensorLastUpdated.Text = "Verbindungsfehler: " + ex.Message;
                }

            }
        }

        private void initToolTip(ToolTip tp)
        {
            tp.UseFading = true;
            tp.UseAnimation = true;
            tp.IsBalloon = true;
            tp.ShowAlways = true;
            tp.AutoPopDelay = 5000;
            tp.InitialDelay = 1000;
            tp.ReshowDelay = 500;
        }

        private void initFormSettings()
        {
            this.TopMost = XML.getTopMost;
            this.Text = XML.Title;
        }

        private void setDefaultTrend()
        {
            picTrendTemp.Image = picTrendSame.Image;
            picTrendHeatIndex.Image = picTrendSame.Image;
            picTrendAirPressure.Image = picTrendSame.Image;
            picTrendHumidity.Image = picTrendSame.Image;
            picTrendLUX.Image = picTrendSame.Image;
            
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                LoadDataObjects();
                loadHtmlSettings();
                UpdateSensorCbo();
                initToolTip(toolTip1);
                initFormSettings();
                setDefaultTrend();
                connectionCheck(true);
                setTimerFileWriter(true);
                if (cboChartSelection.Items.Count > 0 && cboChartSelection.SelectedIndex > -1)
                {
                    Console.WriteLine("Get first dataset from selected combobox entry");
                    System.Threading.Thread.Sleep(250);
                    writeCommandToArduino(getAcutalDataObject(), "DATA");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ein Fehler ist aufgetreten:\n" + ex.Message, "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally
            {
                this.detailierteInformationenToolStripMenuItem.Enabled = (cboChartSelection.Items.Count > 0);

            }
        }

        private void UpdateSensorCbo()
        {
            cboSensors.Items.Clear();

            foreach(KeyValuePair<string, DataObject> kvp in dataObjs)
            {
                DataObject dobj = (DataObject)kvp.Value;
                if (dobj.Active)
                    cboSensors.Items.Add(dobj.Name);
            }

            if (cboSensors.Items.Count > 0)
                cboSensors.SelectedIndex = 0;

        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {

            tmrCheckConnStatus.Stop();
            tmrCheckConnStatus.Dispose();
            tmrFileWriter.Stop();
            tmrFileWriter.Dispose();
            foreach (KeyValuePair<string, DataObject> kvp in dataObjs)
            {
                DataObject dobj = (DataObject)kvp.Value;
                if (dobj.IsOpen)
                {
                    dobj.DataReceived -= Dobj_DataReceived;
                    dobj.Close();
                }
            }
        }

        private void cboSensors_SelectedIndexChanged(object sender, EventArgs e)
        {
            addChartPossibilities();
            DataObject dojb = getAcutalDataObject();
            showData(dojb);
            UpdateStatus(dojb);
            this.detailierteInformationenToolStripMenuItem.Enabled = (cboChartSelection.Items.Count > 0);
        }

        private DataObject getAcutalDataObject()
        {
            if (this.cboSensors.Items.Count < 1)
                return null;

            return dataObjs[this.cboSensors.GetItemText(this.cboSensors.SelectedItem)];
        }

        private void addChartPossibilities()
        {
            this.cboChartSelection.SelectedIndexChanged -= cboChartSelection_SelectedIndexChanged;

            List<string> capabaleItems = DataObjectCategory.getCapableItems(getAcutalDataObject().Protocol);

            cboChartSelection.Items.Clear();

            cboChartSelection.Items.AddRange(capabaleItems.ToArray());

            if (cboChartSelection.Items.Count > 0)
                cboChartSelection.SelectedIndex = 0;

            this.cboChartSelection.SelectedIndexChanged += cboChartSelection_SelectedIndexChanged;

            cboChartSelection_SelectedIndexChanged(this, EventArgs.Empty);

        }

        public void changeColor(PictureBox pic)
        {
            ColorDialog colDlg = new ColorDialog();
            colDlg.Color = pic.BackColor;
            if (colDlg.ShowDialog(this) == DialogResult.OK)
            {
                pic.BackColor = colDlg.Color;
                updateChart(getAcutalDataObject());
            }
            colDlg.Dispose();
        }

        public double MilliTimeStamp(DateTime TheDate)
        {  
             DateTime d1 = new DateTime(1970, 1, 1);  
             DateTime d2 = DateTime.SpecifyKind(TheDate, DateTimeKind.Utc);  
             TimeSpan ts = new TimeSpan(d2.Ticks - d1.Ticks);  
             return ts.TotalMilliseconds;  
        }

        private void addChartSerie(List<double> values, List<double> dt, string name, Color color, DateTime minDate, DateTime maxDate, double minY = double.MinValue, double maxY = double.MaxValue)
        {
            if (chartValues.Series.IndexOf(name) < 0)
            {
                chartValues.Series.Add(name);
                chartValues.Series[name].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            }
            
            //chartValues.Titles[0].Text = "Testtitle";
            
            chartValues.Series[0].XValueType = ChartValueType.DateTime;
            chartValues.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";
            double diff = maxDate.Subtract(minDate).TotalSeconds;

            if (diff < 300)
            {
                chartValues.ChartAreas[0].AxisX.Interval = 1;
                chartValues.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Minutes;
                chartValues.ChartAreas[0].AxisX.IntervalOffset = 1;
            }
            else if (diff >= 300 && diff < 600)
            {
                chartValues.ChartAreas[0].AxisX.Interval = 2;
                chartValues.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Minutes;
                chartValues.ChartAreas[0].AxisX.IntervalOffset = 2;
            } else if (diff >= 600 && diff < 1200)
            {
                chartValues.ChartAreas[0].AxisX.Interval = 5;
                chartValues.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Minutes;
                chartValues.ChartAreas[0].AxisX.IntervalOffset = 5;
            } else if (diff >=1200 && diff < 1800)
            {
                chartValues.ChartAreas[0].AxisX.Interval = 10;
                chartValues.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Minutes;
                chartValues.ChartAreas[0].AxisX.IntervalOffset = 10;
            } else if (diff >=1800 && diff < 3600)
            {
                chartValues.ChartAreas[0].AxisX.Interval = 15;
                chartValues.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Minutes;
                chartValues.ChartAreas[0].AxisX.IntervalOffset = 15;
            } else if (diff >=3600 && diff < 7200)
            {
                chartValues.ChartAreas[0].AxisX.Interval = 30;
                chartValues.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Minutes;
                chartValues.ChartAreas[0].AxisX.IntervalOffset = 30;
            }
            else if (diff >= 7200 && diff < 28800)
            {
                chartValues.ChartAreas[0].AxisX.Interval = 1;
                chartValues.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Hours;
                chartValues.ChartAreas[0].AxisX.IntervalOffset = 1;
            }
            else
            {
                chartValues.ChartAreas[0].AxisX.Interval = 4;
                chartValues.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Hours;
                chartValues.ChartAreas[0].AxisX.IntervalOffset = 4;
            }

            chartValues.ChartAreas[0].AxisY.Minimum = minY;
            chartValues.ChartAreas[0].AxisY.Maximum = maxY;

            chartValues.ChartAreas[0].AxisX.Minimum = minDate.AddMinutes(-1).ToOADate();
            chartValues.ChartAreas[0].AxisX.Maximum = maxDate.AddMinutes(1).ToOADate();
            chartValues.Series[0].Points.DataBindXY(dt.ToArray(), values.ToArray());
            
            chartValues.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.DashDotDot;
            chartValues.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.DashDotDot;

            chartValues.Series[name].Color = color;

        }

        private void updateChart(DataObjectCategory dbo, DataObject dObjExt, Color lineColor)
        {
            chartValues.Series.Clear();

            if (DataObjectCategory.HasCapability(dbo, dObjExt.Protocol) && dObjExt.DataAvailable)
            {
                double min = dObjExt.getHistoryItemMinValue(dbo);
                double max = dObjExt.getHistoryItemMaxValue(dbo);

                if ((max - min) < 4)
                {
                    min -= 2;
                    max += 2;
                }
                else
                {
                    min -= 5;
                    max += 5;
                }
                    

                //Set minimum Value to 0 evept for Temperature values (HeatIndex and Temperature -> it can be colder than 0 degrees ;) )
                if (min < 0 && !(dbo.Value == DataObjectCategory.HeatIndex.Value || dbo.Value == DataObjectCategory.Temperatur.Value))
                    min = 0;

                Console.WriteLine("Min " + min.ToString() + " - Max: " + max.ToString());
                List<double> dt = new List<double>();
                List<double> values = new List<double>();

                DateTime minDate = dObjExt.getLogItems(dbo)[0].Timepoint;
                DateTime maxDate = dObjExt.getLogItems(dbo)[dObjExt.getLogItems(dbo).Count - 1].Timepoint;

                foreach (logItem li in dObjExt.getLogItems(dbo))
                {
                    dt.Add(li.Timepoint.ToOADate());
                    values.Add(li.Value);
                }
                
                addChartSerie(values, dt, dbo.Value.ToString(), lineColor, minDate, maxDate, min, max);
                
                lblNumLogEntries.Text = "Datensätze: " + values.Count.ToString() + " (max: " + dObjExt.MaxHistoryItemsSet + ")";

                if (detailierteInformationenToolStripMenuItem.Checked)
                {
                    updateListView();
                }
            }

            if (chartValues.Series.Count > 0)
            {
                chartValues.DataBind();
                chartValues.Update();
            }
        }

        public DataObjectCategory getActualDataObjectCategory()
        {
            string selected = this.cboChartSelection.GetItemText(this.cboChartSelection.SelectedItem);
            return DataObjectCategory.getObjectCategory(selected);
        }

        private void updateChart(DataObject dobj)
        {
            DataObjectCategory dobjCat = getActualDataObjectCategory();

            if (dobjCat != null)
            {
                if (dobj.getHistoryItemCount(dobjCat) > 0)
                {
                    Color lineColor = getChartColor(dobjCat);
                    updateChart(dobjCat, dobj, lineColor);
                }
            }
        }

        private void cboChartSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateChart(getAcutalDataObject());
        }

        private void tsmEnd_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tsmOptions_Click(object sender, EventArgs e)
        {
            optionProperties Options = new optionProperties();
            Options.propTopMost = this.TopMost;
            Options.propWriteHTML = XML.HtmlEnabled;
            frmOptions fOpt = new frmOptions(Options);

            fOpt.Top = (this.Top + (this.Height / 2)) - (fOpt.Height / 2);
            fOpt.Left = (this.Left + (this.Width / 2)) - (fOpt.Width / 2);
            
            fOpt.ShowDialog(this);

            //exit if canceled
            if (fOpt.Cancel == true)
                return;

            this.TopMost = fOpt.OptionProp.propTopMost;
        }

        private void blauAnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            writeCommandToArduino(getAcutalDataObject(), "LED");
        }

        private void blauStatusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            writeCommandToArduino(getAcutalDataObject(), "STATUSLED");
        }

        private void picColTemp_Click(object sender, EventArgs e)
        {
            changeColor(picColTemp);
        }

        private void picColHeatIndex_Click(object sender, EventArgs e)
        {
            changeColor(picColHeatIndex);
        }

        private void picColAirPressure_Click(object sender, EventArgs e)
        {
            changeColor(picColAirPressure);
        }

        private void picColHumidity_Click(object sender, EventArgs e)
        {
            changeColor(picColHumidity);
        }

        private void picColLUX_Click(object sender, EventArgs e)
        {
            changeColor(picColLUX);
        }

        private void updateListView()
        {
            DataObject dobj = getAcutalDataObject();
            lstViewDetail.Clear();
            lstViewDetail.View = View.Details;
            lstViewDetail.MultiSelect = true;
            lstViewDetail.FullRowSelect = true;
            lstViewDetail.Columns.Add("Datum/Uhrzeit");
            lstViewDetail.Columns.Add("Wert");
            foreach(ColumnHeader ch in lstViewDetail.Columns)
            {
                ch.Width = (lstViewDetail.Width / lstViewDetail.Columns.Count) - 5;
            }
            
            if (!dobj.DataAvailable)
                return;

            DataObjectCategory cat = getActualDataObjectCategory();
            lstViewDetail.BeginUpdate();

            try
            {
                foreach (logItem li in dobj.getLogItems(cat))
                {
                    ListViewItem lvItem = new ListViewItem();
                    lvItem.Text = li.Timepoint.ToString("dd.MM.yyyy hh:mm:ss");
                    lvItem.SubItems.Add(li.Value.ToString("F") + DataObjectCategory.getSensorValueUnit(li.DataObjectCat));
                    lstViewDetail.Items.Add(lvItem);
                }

                lstViewDetail.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                lstViewDetail.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
            finally
            {
                lstViewDetail.EndUpdate();
            }
        }

        private void detailierteInformationenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            detailierteInformationenToolStripMenuItem.Checked = !detailierteInformationenToolStripMenuItem.Checked;

            if (detailierteInformationenToolStripMenuItem.Checked)
            {
                this.Width = lstViewDetail.Left + lstViewDetail.Width + 30;
                updateListView();
            }
            else
            {
                this.Width = grpBoxSensor.Left + grpBoxSensor.Width + 30;
            }
            

        }

        Point? prevPosition = null;
        ToolTip tooltip = new ToolTip();

        private void chartValues_MouseMove(object sender, MouseEventArgs e)
        {
            Point pos = e.Location;
            if (prevPosition.HasValue && pos == prevPosition.Value)
                return;
            tooltip.RemoveAll();
            prevPosition = pos;
            HitTestResult [] results = chartValues.HitTest(pos.X, pos.Y, false, ChartElementType.DataPoint);
            foreach (HitTestResult result in results)
            {
                if (result.ChartElementType == ChartElementType.DataPoint)
                {
                    var prop = result.Object as DataPoint;
                    if (prop != null)
                    {
                        var pointXPixel = result.ChartArea.AxisX.ValueToPixelPosition(prop.XValue);
                        var pointYPixel = result.ChartArea.AxisY.ValueToPixelPosition(prop.YValues[0]);
                        
                        // check if the cursor is really close to the point (2 pixels around the point)
                        if (Math.Abs(pos.X - pointXPixel) < 5 &&
                            Math.Abs(pos.Y - pointYPixel) < 5)
                        {
                            tooltip.Show("X=" + DateTime.FromOADate(prop.XValue).ToString("dd.MM.yyyy hh:mm:ss") + ", Y=" + prop.YValues[0], this.chartValues,
                                            pos.X, pos.Y - 15);
                        }
                    }
                }
            }
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            writeCommandToArduino(getAcutalDataObject(), "HELP");
        }

        private void getVersionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            writeCommandToArduino(getAcutalDataObject(), "PROTOCOL_VERSION");
        }

        private void getActualDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            writeCommandToArduino(getAcutalDataObject(), "DATA");
        }

        private void testInvalidesKommandoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            writeCommandToArduino(getAcutalDataObject(), "InvalidBlaBla");
        }

        private void getHTTPDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataObject dObj = getAcutalDataObject();
            if (dObj.DataInterfaceType == XMLProtocol.HTTP)
            {
                getHTTPData(dObj.URL, dObj);
            } else
            {
                MessageBox.Show("Kein URL Sensor!");
            }
        }
    }
}

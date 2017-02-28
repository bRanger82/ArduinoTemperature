using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Arduino_Temperature_Retrofit
{
    public partial class frmMain : Form
    {

        Dictionary<string, DataObject> dataObjs = new Dictionary<string, DataObject>();

        public frmMain()
        {
            InitializeComponent();
        }

        private void LoadDataObjects()
        {
            List<XMLSensorObject> xmlSensors = clsXML.test();
            foreach (XMLSensorObject xmlSensor in xmlSensors)
            {
                DataObject dobj = new DataObject();
                dobj.LogPath = xmlSensor.LogFilePath;
                dobj.Name = xmlSensor.Name;
                dobj.Active = xmlSensor.Active;
                dobj.PortName = xmlSensor.Port;
                dobj.BaudRate = Common.COMSettings.DefaultBaudRate;
                dobj.DataBits = Common.COMSettings.DefaultDataBits;
                dobj.DtrEnable = Common.COMSettings.DefaultDtrEnable;
                dobj.StopBits = Common.COMSettings.DefaultStopBits;
                dobj.DataReceived += Dobj_DataReceived;
                dobj.Open();
                dataObjs.Add(dobj.Name, dobj);
            }
        }

        private void Dobj_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (sender is DataObject)
            {
                DataObject spTemp = (DataObject)sender;
                string received = spTemp.ReadLine();
                string portName = spTemp.PortName;
                this.BeginInvoke(new DataReceiveEvent(DataReceived), received, portName, spTemp.Name);
            }
        }

        private delegate void DataReceiveEvent(string information, string comPort, string Name);

        private void DataReceived(string information, string comPort, string name)
        {
            DataObject dobj = dataObjs[name];
            if (information.Contains("|"))
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
                } else
                {
                    dobj.DataAvailable = false;
                    dobj.LastUpdated = DateTime.Now;
                    dobj.Protocol = DataObjectProtocol.NONE;
                    dobj.AdditionalInformation = "Datenprotokoll unbekannt";
                }
            } else
            {
                dobj.DataAvailable = false;
                dobj.LastUpdated = DateTime.Now;
                dobj.Protocol = DataObjectProtocol.NONE;
                dobj.AdditionalInformation = information;
            }

            if (name == this.cboSensors.GetItemText(this.cboSensors.SelectedItem))
            {
                showData(dobj);
                updateChart(dobj);
            }
        }

        private void processDataProtocolV1(string [] data, ref DataObject dobj)
        {
            dobj.addDataItem(DataObjectCategory.Humidity.Value, double.Parse(Common.replaceDecPoint(data[1].ToString())), DataObjectCategory.Humidity, Common.SensorValueType.Humidity);
            dobj.addDataItem(DataObjectCategory.Temperature.Value, double.Parse(Common.replaceDecPoint(data[2].ToString())), DataObjectCategory.Temperature, Common.SensorValueType.Temperature);
            dobj.addDataItem(DataObjectCategory.HeatIndex.Value, double.Parse(Common.replaceDecPoint(data[3].ToString())), DataObjectCategory.HeatIndex, Common.SensorValueType.Temperature);
            dobj.LastUpdated = DateTime.Now;
            dobj.DataAvailable = true;
            dobj.AdditionalInformation = "-";
            dobj.Protocol = DataObjectProtocol.PROTOCOL_ONE;
        }

        private void processDataProtocolV2(string[] data, ref DataObject dobj)
        {
            dobj.addDataItem(DataObjectCategory.Humidity.Value, double.Parse(Common.replaceDecPoint(data[2].ToString())), DataObjectCategory.Humidity, Common.SensorValueType.Humidity);
            dobj.addDataItem(DataObjectCategory.Temperature.Value, double.Parse(Common.replaceDecPoint(data[3].ToString())), DataObjectCategory.Temperature, Common.SensorValueType.Temperature);
            dobj.addDataItem(DataObjectCategory.HeatIndex.Value, double.Parse(Common.replaceDecPoint(data[4].ToString())), DataObjectCategory.HeatIndex, Common.SensorValueType.Temperature);
            dobj.addDataItem(DataObjectCategory.AirPressure.Value, double.Parse(Common.replaceDecPoint(data[5].ToString())), DataObjectCategory.AirPressure, Common.SensorValueType.AirPressure);
            dobj.LastUpdated = DateTime.Now;
            dobj.DataAvailable = true;
            dobj.AdditionalInformation = "-";
            dobj.Protocol = DataObjectProtocol.PROTOCOL_TWO;
        }

        private void processDataProtocolV3(string[] data, ref DataObject dobj)
        {
            dobj.addDataItem(DataObjectCategory.Humidity.Value, double.Parse(Common.replaceDecPoint(data[2].ToString())), DataObjectCategory.Humidity, Common.SensorValueType.Humidity);
            dobj.addDataItem(DataObjectCategory.Temperature.Value, double.Parse(Common.replaceDecPoint(data[3].ToString())), DataObjectCategory.Temperature, Common.SensorValueType.Temperature);
            dobj.addDataItem(DataObjectCategory.HeatIndex.Value, double.Parse(Common.replaceDecPoint(data[4].ToString())), DataObjectCategory.HeatIndex, Common.SensorValueType.Temperature);
            dobj.addDataItem(DataObjectCategory.AirPressure.Value, double.Parse(Common.replaceDecPoint(data[5].ToString())), DataObjectCategory.AirPressure, Common.SensorValueType.AirPressure);
            dobj.addDataItem(DataObjectCategory.LUX.Value, double.Parse(Common.replaceDecPoint(data[6].ToString())), DataObjectCategory.LUX, Common.SensorValueType.LUX);

            dobj.LastUpdated = DateTime.Now; 
            dobj.DataAvailable = true;
            dobj.AdditionalInformation = "-";
            dobj.Protocol = DataObjectProtocol.PROTOCOL_THREE;
        }

        private void showData(DataObject dobj)
        {
            if (dobj.DataAvailable)
            {
                setLabelInformation(lblSensorTempValue, lblSensorTempMin, lblSensorTempMax, lblSensorTempMinTime, lblSensorTempMaxTime, dobj, DataObjectCategory.Temperature);
                setLabelInformation(lblSensorLuxValue, lblSensorLuxMin, lblSensorLuxMax, lblSensorLuxMinTime, lblSensorLuxMaxTime, dobj, DataObjectCategory.LUX);
                setLabelInformation(lblSensorHumidityValue, lblSensorHumidityValueMin, lblSensorHumidityValueMax, lblSensorHumidityValueMinTime, lblSensorHumidityValueMaxTime, dobj, DataObjectCategory.Humidity);
                setLabelInformation(lblSensorPressureValue, lblSensorPressureMin, lblSensorPressureMax, lblSensorPressureMinTime, lblSensorPressureMaxTime, dobj, DataObjectCategory.AirPressure);
                setLabelInformation(lblSensorHeatIndexValue, lblSensorHeatIndexMin, lblSensorHeatIndexMax, lblSensorHeatIndexMinTime, lblSensorHeatIndexMaxTime, dobj, DataObjectCategory.HeatIndex);
            }
            else
            {
                lblSensorHumidityValue.Text = "Fehler";
                lblSensorTempValue.Text = "Fehler";
                if (dobj.Protocol == DataObjectProtocol.PROTOCOL_ONE)
                {
                    lblSensorLuxValue.Text = "N/A";
                    lblSensorPressureValue.Text = "N/A";
                }
                else if (dobj.Protocol == DataObjectProtocol.PROTOCOL_TWO)
                {
                    lblSensorLuxValue.Text = "N/A";
                    lblSensorPressureValue.Text = "Fehler";
                }
                else if (dobj.Protocol == DataObjectProtocol.PROTOCOL_THREE)
                {
                    lblSensorLuxValue.Text = "Fehler";
                    lblSensorPressureValue.Text = "Fehler";
                }
            }

            lblSensorLastUpdated.Text = "Aktualisiert: " + Common.getCurrentDateTimeFormatted();
        }

        private void setLabelInformation(Label lblValue, Label lblMinValue, Label lblMaxValue, Label lblMinTime, Label lblMaxTime, DataObject dObjExt, DataObjectCategory dobjcat)
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
            else
            {
                lblValue.Text = " --- ";
                lblMinValue.Text = " --- ";
                lblMaxValue.Text = " --- ";
                lblMinTime.Text = " --- ";
                lblMaxTime.Text = " --- ";
            }
        }

        private Color getChartColor(DataObjectCategory dobjCat)
        {
            if (dobjCat.Value == DataObjectCategory.HeatIndex.Value)
                return picColHeatIndex.BackColor;
            else if (dobjCat.Value == DataObjectCategory.Temperature.Value)
                return picColTemp.BackColor;
            else if (dobjCat.Value == DataObjectCategory.LUX.Value)
                return picColLUX.BackColor;
            else if (dobjCat.Value == DataObjectCategory.Humidity.Value)
                return picColHumidity.BackColor;
            else if (dobjCat.Value == DataObjectCategory.AirPressure.Value)
                return picColAirPressure.BackColor;
            else
                return Color.Red;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                LoadDataObjects();
                UpdateSensorCbo();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ein Fehler ist aufgetreten:\n" + ex.Message, "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            string name = this.cboSensors.GetItemText(this.cboSensors.SelectedItem);
            addChartPossibilities();
            showData(dataObjs[name]);
        }

        private void addChartPossibilities()
        {
            cboChartSelection.Items.AddRange(DataObjectCategory.Items.ToArray());
            if (cboChartSelection.Items.Count > 0)
                cboChartSelection.SelectedIndex = 0;
        }

        private void addChartSerie(List<double> values, string name, Color color, double min = double.MinValue, double max = double.MaxValue)
        {
            if (chartValues.Series.IndexOf(name) < 0)
            {
                chartValues.Series.Add(name);
                chartValues.Series[name].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            }
            chartValues.Series[name].Points.DataBindY(values.ToArray());

            chartValues.ChartAreas[0].AxisY.Minimum = min;
            chartValues.ChartAreas[0].AxisY.Maximum = max;

            chartValues.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.DashDotDot;
            chartValues.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.DashDotDot;

            chartValues.Series[name].Color = color;

        }

        private void updateChart(DataObjectCategory dbo, DataObject dObjExt, Color lineColor)
        {
            chartValues.Series.Clear();

            if (DataObjectCapabilities.HasCapability(dbo, dObjExt.Protocol))
            {
                double min = dObjExt.getLogItemMinValue(dbo);
                double max = dObjExt.getLogItemMaxValue(dbo);

                if ((max - min) < 10)
                {
                    min -= 5;
                    max += 5;
                }

                Console.WriteLine("Min " + min.ToString() + " - Max: " + max.ToString());
                addChartSerie(dObjExt.getLogItems(dbo), dbo.Value.ToString(), lineColor, min, max);
            }

            if (chartValues.Series.Count > 0)
            {
                chartValues.DataBind();
                chartValues.Update();
            }
        }

        private void updateChart(DataObject dobj)
        {
            string selected = this.cboChartSelection.GetItemText(this.cboChartSelection.SelectedItem);
            DataObjectCategory dobjCat = DataObjectCategory.getObjectCategory(selected);

            if (dobjCat != null)
            {
                if (dobj.getLogItemCount(dobjCat) > 0)
                {
                    Color lineColor = getChartColor(dobjCat);
                    updateChart(dobjCat, dobj, lineColor);
                }
            }
        }

        private void cboChartSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            string name = this.cboSensors.GetItemText(this.cboSensors.SelectedItem);
            updateChart(dataObjs[name]);
        }
    }
}

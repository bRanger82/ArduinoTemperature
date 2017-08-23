namespace Arduino_Temperature_Retrofit
{
    partial class frmMain
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.lblChartSelection = new System.Windows.Forms.Label();
            this.cboChartSelection = new System.Windows.Forms.ComboBox();
            this.grpBoxSensor = new System.Windows.Forms.GroupBox();
            this.cboSensors = new System.Windows.Forms.ComboBox();
            this.grpBoxHeatIndex = new System.Windows.Forms.GroupBox();
            this.lblSensorHeatIndexName = new System.Windows.Forms.Label();
            this.lblSensorHeatIndexMaxTime = new System.Windows.Forms.Label();
            this.lblSensorHeatIndexMinTime = new System.Windows.Forms.Label();
            this.lblSensorHeatIndexMax = new System.Windows.Forms.Label();
            this.lblSensorHeatIndexMin = new System.Windows.Forms.Label();
            this.lblSensorHeatIndexMaxName = new System.Windows.Forms.Label();
            this.lblSensorHeatIndexMinName = new System.Windows.Forms.Label();
            this.lblSensorHeatIndexValue = new System.Windows.Forms.Label();
            this.lblSensorLastUpdated = new System.Windows.Forms.Label();
            this.lblSensor = new System.Windows.Forms.Label();
            this.grpBoxLUX = new System.Windows.Forms.GroupBox();
            this.lblSensorLuxName = new System.Windows.Forms.Label();
            this.lblSensorLuxMaxTime = new System.Windows.Forms.Label();
            this.lblSensorLuxMinTime = new System.Windows.Forms.Label();
            this.lblSensorLuxMax = new System.Windows.Forms.Label();
            this.lblSensorLuxMin = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.lblSensorLuxValue = new System.Windows.Forms.Label();
            this.grpBoxAirPressure = new System.Windows.Forms.GroupBox();
            this.lblSensorPressureName = new System.Windows.Forms.Label();
            this.lblSensorPressureMaxTime = new System.Windows.Forms.Label();
            this.lblSensorPressureMinTime = new System.Windows.Forms.Label();
            this.lblSensorPressureMax = new System.Windows.Forms.Label();
            this.lblSensorPressureMin = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.lblSensorPressureValue = new System.Windows.Forms.Label();
            this.grpBoxHumidity = new System.Windows.Forms.GroupBox();
            this.lblSensorHumidityName = new System.Windows.Forms.Label();
            this.lblSensorHumidityValueMaxTime = new System.Windows.Forms.Label();
            this.lblSensorHumidityValueMinTime = new System.Windows.Forms.Label();
            this.lblSensorHumidityValueMax = new System.Windows.Forms.Label();
            this.lblSensorHumidityValueMin = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.lblSensorHumidityValue = new System.Windows.Forms.Label();
            this.grpBoxTemperature = new System.Windows.Forms.GroupBox();
            this.lblSensorTempName = new System.Windows.Forms.Label();
            this.lblSensorTempMaxTime = new System.Windows.Forms.Label();
            this.lblSensorTempMinTime = new System.Windows.Forms.Label();
            this.lblSensorTempMax = new System.Windows.Forms.Label();
            this.lblSensorTempMin = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblSensorTempValue = new System.Windows.Forms.Label();
            this.chartValues = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menüToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmEnd = new System.Windows.Forms.ToolStripMenuItem();
            this.kommandosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blauAnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blauStatusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getVersionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getActualDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testInvalidesKommandoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getHTTPDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sQLTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zusatzToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.detailierteInformationenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.frmMainToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.lblNumLogEntries = new System.Windows.Forms.Label();
            this.lstViewDetail = new System.Windows.Forms.ListView();
            this.lblHistoryDataLV = new System.Windows.Forms.Label();
            this.picTrendError = new System.Windows.Forms.PictureBox();
            this.picTrendSame = new System.Windows.Forms.PictureBox();
            this.picConnStatus = new System.Windows.Forms.PictureBox();
            this.picTrendDown = new System.Windows.Forms.PictureBox();
            this.picTrendUp = new System.Windows.Forms.PictureBox();
            this.picTrendHeatIndex = new System.Windows.Forms.PictureBox();
            this.picColHeatIndex = new System.Windows.Forms.PictureBox();
            this.picTrendLUX = new System.Windows.Forms.PictureBox();
            this.picColLUX = new System.Windows.Forms.PictureBox();
            this.picTrendAirPressure = new System.Windows.Forms.PictureBox();
            this.picColAirPressure = new System.Windows.Forms.PictureBox();
            this.picTrendHumidity = new System.Windows.Forms.PictureBox();
            this.picColHumidity = new System.Windows.Forms.PictureBox();
            this.picTrendTemp = new System.Windows.Forms.PictureBox();
            this.picColTemp = new System.Windows.Forms.PictureBox();
            this.grpBoxSensor.SuspendLayout();
            this.grpBoxHeatIndex.SuspendLayout();
            this.grpBoxLUX.SuspendLayout();
            this.grpBoxAirPressure.SuspendLayout();
            this.grpBoxHumidity.SuspendLayout();
            this.grpBoxTemperature.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartValues)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picTrendError)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTrendSame)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picConnStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTrendDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTrendUp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTrendHeatIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picColHeatIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTrendLUX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picColLUX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTrendAirPressure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picColAirPressure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTrendHumidity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picColHumidity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTrendTemp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picColTemp)).BeginInit();
            this.SuspendLayout();
            // 
            // lblChartSelection
            // 
            this.lblChartSelection.AutoSize = true;
            this.lblChartSelection.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChartSelection.Location = new System.Drawing.Point(328, 489);
            this.lblChartSelection.Name = "lblChartSelection";
            this.lblChartSelection.Size = new System.Drawing.Size(109, 16);
            this.lblChartSelection.TabIndex = 35;
            this.lblChartSelection.Text = "Datenauswahl:";
            // 
            // cboChartSelection
            // 
            this.cboChartSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboChartSelection.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboChartSelection.FormattingEnabled = true;
            this.cboChartSelection.Location = new System.Drawing.Point(450, 486);
            this.cboChartSelection.Name = "cboChartSelection";
            this.cboChartSelection.Size = new System.Drawing.Size(169, 24);
            this.cboChartSelection.TabIndex = 34;
            this.cboChartSelection.SelectedIndexChanged += new System.EventHandler(this.cboChartSelection_SelectedIndexChanged);
            // 
            // grpBoxSensor
            // 
            this.grpBoxSensor.Controls.Add(this.picTrendError);
            this.grpBoxSensor.Controls.Add(this.picTrendSame);
            this.grpBoxSensor.Controls.Add(this.picConnStatus);
            this.grpBoxSensor.Controls.Add(this.picTrendDown);
            this.grpBoxSensor.Controls.Add(this.picTrendUp);
            this.grpBoxSensor.Controls.Add(this.cboSensors);
            this.grpBoxSensor.Controls.Add(this.grpBoxHeatIndex);
            this.grpBoxSensor.Controls.Add(this.lblSensorLastUpdated);
            this.grpBoxSensor.Controls.Add(this.lblSensor);
            this.grpBoxSensor.Controls.Add(this.grpBoxLUX);
            this.grpBoxSensor.Controls.Add(this.grpBoxAirPressure);
            this.grpBoxSensor.Controls.Add(this.grpBoxHumidity);
            this.grpBoxSensor.Controls.Add(this.grpBoxTemperature);
            this.grpBoxSensor.Location = new System.Drawing.Point(10, 25);
            this.grpBoxSensor.Name = "grpBoxSensor";
            this.grpBoxSensor.Size = new System.Drawing.Size(610, 450);
            this.grpBoxSensor.TabIndex = 33;
            this.grpBoxSensor.TabStop = false;
            // 
            // cboSensors
            // 
            this.cboSensors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSensors.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboSensors.FormattingEnabled = true;
            this.cboSensors.Location = new System.Drawing.Point(319, 19);
            this.cboSensors.Name = "cboSensors";
            this.cboSensors.Size = new System.Drawing.Size(285, 33);
            this.cboSensors.TabIndex = 35;
            this.cboSensors.SelectedIndexChanged += new System.EventHandler(this.cboSensors_SelectedIndexChanged);
            // 
            // grpBoxHeatIndex
            // 
            this.grpBoxHeatIndex.Controls.Add(this.picTrendHeatIndex);
            this.grpBoxHeatIndex.Controls.Add(this.lblSensorHeatIndexName);
            this.grpBoxHeatIndex.Controls.Add(this.lblSensorHeatIndexMaxTime);
            this.grpBoxHeatIndex.Controls.Add(this.lblSensorHeatIndexMinTime);
            this.grpBoxHeatIndex.Controls.Add(this.lblSensorHeatIndexMax);
            this.grpBoxHeatIndex.Controls.Add(this.lblSensorHeatIndexMin);
            this.grpBoxHeatIndex.Controls.Add(this.lblSensorHeatIndexMaxName);
            this.grpBoxHeatIndex.Controls.Add(this.lblSensorHeatIndexMinName);
            this.grpBoxHeatIndex.Controls.Add(this.lblSensorHeatIndexValue);
            this.grpBoxHeatIndex.Controls.Add(this.picColHeatIndex);
            this.grpBoxHeatIndex.Enabled = false;
            this.grpBoxHeatIndex.Location = new System.Drawing.Point(319, 69);
            this.grpBoxHeatIndex.Name = "grpBoxHeatIndex";
            this.grpBoxHeatIndex.Size = new System.Drawing.Size(285, 110);
            this.grpBoxHeatIndex.TabIndex = 30;
            this.grpBoxHeatIndex.TabStop = false;
            // 
            // lblSensorHeatIndexName
            // 
            this.lblSensorHeatIndexName.AutoSize = true;
            this.lblSensorHeatIndexName.BackColor = System.Drawing.Color.Transparent;
            this.lblSensorHeatIndexName.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSensorHeatIndexName.Location = new System.Drawing.Point(2, 6);
            this.lblSensorHeatIndexName.Name = "lblSensorHeatIndexName";
            this.lblSensorHeatIndexName.Size = new System.Drawing.Size(139, 29);
            this.lblSensorHeatIndexName.TabIndex = 23;
            this.lblSensorHeatIndexName.Text = "Heat-Index";
            // 
            // lblSensorHeatIndexMaxTime
            // 
            this.lblSensorHeatIndexMaxTime.AutoSize = true;
            this.lblSensorHeatIndexMaxTime.Location = new System.Drawing.Point(119, 89);
            this.lblSensorHeatIndexMaxTime.Name = "lblSensorHeatIndexMaxTime";
            this.lblSensorHeatIndexMaxTime.Size = new System.Drawing.Size(54, 13);
            this.lblSensorHeatIndexMaxTime.TabIndex = 22;
            this.lblSensorHeatIndexMaxTime.Text = "Kein Wert";
            // 
            // lblSensorHeatIndexMinTime
            // 
            this.lblSensorHeatIndexMinTime.AutoSize = true;
            this.lblSensorHeatIndexMinTime.Location = new System.Drawing.Point(119, 69);
            this.lblSensorHeatIndexMinTime.Name = "lblSensorHeatIndexMinTime";
            this.lblSensorHeatIndexMinTime.Size = new System.Drawing.Size(54, 13);
            this.lblSensorHeatIndexMinTime.TabIndex = 21;
            this.lblSensorHeatIndexMinTime.Text = "Kein Wert";
            // 
            // lblSensorHeatIndexMax
            // 
            this.lblSensorHeatIndexMax.AutoSize = true;
            this.lblSensorHeatIndexMax.Location = new System.Drawing.Point(59, 89);
            this.lblSensorHeatIndexMax.Name = "lblSensorHeatIndexMax";
            this.lblSensorHeatIndexMax.Size = new System.Drawing.Size(54, 13);
            this.lblSensorHeatIndexMax.TabIndex = 20;
            this.lblSensorHeatIndexMax.Text = "Kein Wert";
            // 
            // lblSensorHeatIndexMin
            // 
            this.lblSensorHeatIndexMin.AutoSize = true;
            this.lblSensorHeatIndexMin.Location = new System.Drawing.Point(59, 69);
            this.lblSensorHeatIndexMin.Name = "lblSensorHeatIndexMin";
            this.lblSensorHeatIndexMin.Size = new System.Drawing.Size(54, 13);
            this.lblSensorHeatIndexMin.TabIndex = 19;
            this.lblSensorHeatIndexMin.Text = "Kein Wert";
            // 
            // lblSensorHeatIndexMaxName
            // 
            this.lblSensorHeatIndexMaxName.AutoSize = true;
            this.lblSensorHeatIndexMaxName.Location = new System.Drawing.Point(10, 89);
            this.lblSensorHeatIndexMaxName.Name = "lblSensorHeatIndexMaxName";
            this.lblSensorHeatIndexMaxName.Size = new System.Drawing.Size(27, 13);
            this.lblSensorHeatIndexMaxName.TabIndex = 18;
            this.lblSensorHeatIndexMaxName.Text = "Max";
            // 
            // lblSensorHeatIndexMinName
            // 
            this.lblSensorHeatIndexMinName.AutoSize = true;
            this.lblSensorHeatIndexMinName.Location = new System.Drawing.Point(10, 69);
            this.lblSensorHeatIndexMinName.Name = "lblSensorHeatIndexMinName";
            this.lblSensorHeatIndexMinName.Size = new System.Drawing.Size(24, 13);
            this.lblSensorHeatIndexMinName.TabIndex = 17;
            this.lblSensorHeatIndexMinName.Text = "Min";
            // 
            // lblSensorHeatIndexValue
            // 
            this.lblSensorHeatIndexValue.AutoSize = true;
            this.lblSensorHeatIndexValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSensorHeatIndexValue.Location = new System.Drawing.Point(57, 36);
            this.lblSensorHeatIndexValue.Name = "lblSensorHeatIndexValue";
            this.lblSensorHeatIndexValue.Size = new System.Drawing.Size(118, 29);
            this.lblSensorHeatIndexValue.TabIndex = 1;
            this.lblSensorHeatIndexValue.Text = "Kein Wert";
            // 
            // lblSensorLastUpdated
            // 
            this.lblSensorLastUpdated.AutoSize = true;
            this.lblSensorLastUpdated.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSensorLastUpdated.Location = new System.Drawing.Point(3, 421);
            this.lblSensorLastUpdated.Name = "lblSensorLastUpdated";
            this.lblSensorLastUpdated.Size = new System.Drawing.Size(204, 16);
            this.lblSensorLastUpdated.TabIndex = 34;
            this.lblSensorLastUpdated.Text = "Bitte warten, initialisierung ...";
            // 
            // lblSensor
            // 
            this.lblSensor.AutoSize = true;
            this.lblSensor.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSensor.Location = new System.Drawing.Point(107, 22);
            this.lblSensor.Name = "lblSensor";
            this.lblSensor.Size = new System.Drawing.Size(189, 25);
            this.lblSensor.TabIndex = 31;
            this.lblSensor.Text = "Sensor-Auswahl:";
            // 
            // grpBoxLUX
            // 
            this.grpBoxLUX.Controls.Add(this.picTrendLUX);
            this.grpBoxLUX.Controls.Add(this.lblSensorLuxName);
            this.grpBoxLUX.Controls.Add(this.lblSensorLuxMaxTime);
            this.grpBoxLUX.Controls.Add(this.lblSensorLuxMinTime);
            this.grpBoxLUX.Controls.Add(this.lblSensorLuxMax);
            this.grpBoxLUX.Controls.Add(this.lblSensorLuxMin);
            this.grpBoxLUX.Controls.Add(this.label22);
            this.grpBoxLUX.Controls.Add(this.label23);
            this.grpBoxLUX.Controls.Add(this.lblSensorLuxValue);
            this.grpBoxLUX.Controls.Add(this.picColLUX);
            this.grpBoxLUX.Enabled = false;
            this.grpBoxLUX.Location = new System.Drawing.Point(6, 301);
            this.grpBoxLUX.Name = "grpBoxLUX";
            this.grpBoxLUX.Size = new System.Drawing.Size(285, 110);
            this.grpBoxLUX.TabIndex = 30;
            this.grpBoxLUX.TabStop = false;
            // 
            // lblSensorLuxName
            // 
            this.lblSensorLuxName.AutoSize = true;
            this.lblSensorLuxName.BackColor = System.Drawing.Color.Transparent;
            this.lblSensorLuxName.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSensorLuxName.Location = new System.Drawing.Point(2, 6);
            this.lblSensorLuxName.Name = "lblSensorLuxName";
            this.lblSensorLuxName.Size = new System.Drawing.Size(118, 29);
            this.lblSensorLuxName.TabIndex = 23;
            this.lblSensorLuxName.Text = "Lichtwert";
            // 
            // lblSensorLuxMaxTime
            // 
            this.lblSensorLuxMaxTime.AutoSize = true;
            this.lblSensorLuxMaxTime.Location = new System.Drawing.Point(119, 89);
            this.lblSensorLuxMaxTime.Name = "lblSensorLuxMaxTime";
            this.lblSensorLuxMaxTime.Size = new System.Drawing.Size(54, 13);
            this.lblSensorLuxMaxTime.TabIndex = 22;
            this.lblSensorLuxMaxTime.Text = "Kein Wert";
            // 
            // lblSensorLuxMinTime
            // 
            this.lblSensorLuxMinTime.AutoSize = true;
            this.lblSensorLuxMinTime.Location = new System.Drawing.Point(119, 69);
            this.lblSensorLuxMinTime.Name = "lblSensorLuxMinTime";
            this.lblSensorLuxMinTime.Size = new System.Drawing.Size(54, 13);
            this.lblSensorLuxMinTime.TabIndex = 21;
            this.lblSensorLuxMinTime.Text = "Kein Wert";
            // 
            // lblSensorLuxMax
            // 
            this.lblSensorLuxMax.AutoSize = true;
            this.lblSensorLuxMax.Location = new System.Drawing.Point(59, 89);
            this.lblSensorLuxMax.Name = "lblSensorLuxMax";
            this.lblSensorLuxMax.Size = new System.Drawing.Size(54, 13);
            this.lblSensorLuxMax.TabIndex = 20;
            this.lblSensorLuxMax.Text = "Kein Wert";
            // 
            // lblSensorLuxMin
            // 
            this.lblSensorLuxMin.AutoSize = true;
            this.lblSensorLuxMin.Location = new System.Drawing.Point(59, 69);
            this.lblSensorLuxMin.Name = "lblSensorLuxMin";
            this.lblSensorLuxMin.Size = new System.Drawing.Size(54, 13);
            this.lblSensorLuxMin.TabIndex = 19;
            this.lblSensorLuxMin.Text = "Kein Wert";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(10, 89);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(27, 13);
            this.label22.TabIndex = 18;
            this.label22.Text = "Max";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(10, 69);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(24, 13);
            this.label23.TabIndex = 17;
            this.label23.Text = "Min";
            // 
            // lblSensorLuxValue
            // 
            this.lblSensorLuxValue.AutoSize = true;
            this.lblSensorLuxValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSensorLuxValue.Location = new System.Drawing.Point(57, 36);
            this.lblSensorLuxValue.Name = "lblSensorLuxValue";
            this.lblSensorLuxValue.Size = new System.Drawing.Size(118, 29);
            this.lblSensorLuxValue.TabIndex = 1;
            this.lblSensorLuxValue.Text = "Kein Wert";
            // 
            // grpBoxAirPressure
            // 
            this.grpBoxAirPressure.Controls.Add(this.picTrendAirPressure);
            this.grpBoxAirPressure.Controls.Add(this.lblSensorPressureName);
            this.grpBoxAirPressure.Controls.Add(this.lblSensorPressureMaxTime);
            this.grpBoxAirPressure.Controls.Add(this.lblSensorPressureMinTime);
            this.grpBoxAirPressure.Controls.Add(this.lblSensorPressureMax);
            this.grpBoxAirPressure.Controls.Add(this.lblSensorPressureMin);
            this.grpBoxAirPressure.Controls.Add(this.label30);
            this.grpBoxAirPressure.Controls.Add(this.label31);
            this.grpBoxAirPressure.Controls.Add(this.lblSensorPressureValue);
            this.grpBoxAirPressure.Controls.Add(this.picColAirPressure);
            this.grpBoxAirPressure.Enabled = false;
            this.grpBoxAirPressure.Location = new System.Drawing.Point(6, 185);
            this.grpBoxAirPressure.Name = "grpBoxAirPressure";
            this.grpBoxAirPressure.Size = new System.Drawing.Size(285, 110);
            this.grpBoxAirPressure.TabIndex = 29;
            this.grpBoxAirPressure.TabStop = false;
            // 
            // lblSensorPressureName
            // 
            this.lblSensorPressureName.AutoSize = true;
            this.lblSensorPressureName.BackColor = System.Drawing.Color.Transparent;
            this.lblSensorPressureName.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSensorPressureName.Location = new System.Drawing.Point(2, 6);
            this.lblSensorPressureName.Name = "lblSensorPressureName";
            this.lblSensorPressureName.Size = new System.Drawing.Size(135, 29);
            this.lblSensorPressureName.TabIndex = 23;
            this.lblSensorPressureName.Text = "Barometer";
            // 
            // lblSensorPressureMaxTime
            // 
            this.lblSensorPressureMaxTime.AutoSize = true;
            this.lblSensorPressureMaxTime.Location = new System.Drawing.Point(119, 89);
            this.lblSensorPressureMaxTime.Name = "lblSensorPressureMaxTime";
            this.lblSensorPressureMaxTime.Size = new System.Drawing.Size(54, 13);
            this.lblSensorPressureMaxTime.TabIndex = 22;
            this.lblSensorPressureMaxTime.Text = "Kein Wert";
            // 
            // lblSensorPressureMinTime
            // 
            this.lblSensorPressureMinTime.AutoSize = true;
            this.lblSensorPressureMinTime.Location = new System.Drawing.Point(119, 69);
            this.lblSensorPressureMinTime.Name = "lblSensorPressureMinTime";
            this.lblSensorPressureMinTime.Size = new System.Drawing.Size(54, 13);
            this.lblSensorPressureMinTime.TabIndex = 21;
            this.lblSensorPressureMinTime.Text = "Kein Wert";
            // 
            // lblSensorPressureMax
            // 
            this.lblSensorPressureMax.AutoSize = true;
            this.lblSensorPressureMax.Location = new System.Drawing.Point(59, 89);
            this.lblSensorPressureMax.Name = "lblSensorPressureMax";
            this.lblSensorPressureMax.Size = new System.Drawing.Size(54, 13);
            this.lblSensorPressureMax.TabIndex = 20;
            this.lblSensorPressureMax.Text = "Kein Wert";
            // 
            // lblSensorPressureMin
            // 
            this.lblSensorPressureMin.AutoSize = true;
            this.lblSensorPressureMin.Location = new System.Drawing.Point(59, 69);
            this.lblSensorPressureMin.Name = "lblSensorPressureMin";
            this.lblSensorPressureMin.Size = new System.Drawing.Size(54, 13);
            this.lblSensorPressureMin.TabIndex = 19;
            this.lblSensorPressureMin.Text = "Kein Wert";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(10, 89);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(27, 13);
            this.label30.TabIndex = 18;
            this.label30.Text = "Max";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(10, 69);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(24, 13);
            this.label31.TabIndex = 17;
            this.label31.Text = "Min";
            // 
            // lblSensorPressureValue
            // 
            this.lblSensorPressureValue.AutoSize = true;
            this.lblSensorPressureValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSensorPressureValue.Location = new System.Drawing.Point(57, 36);
            this.lblSensorPressureValue.Name = "lblSensorPressureValue";
            this.lblSensorPressureValue.Size = new System.Drawing.Size(118, 29);
            this.lblSensorPressureValue.TabIndex = 1;
            this.lblSensorPressureValue.Text = "Kein Wert";
            // 
            // grpBoxHumidity
            // 
            this.grpBoxHumidity.Controls.Add(this.picTrendHumidity);
            this.grpBoxHumidity.Controls.Add(this.lblSensorHumidityName);
            this.grpBoxHumidity.Controls.Add(this.lblSensorHumidityValueMaxTime);
            this.grpBoxHumidity.Controls.Add(this.lblSensorHumidityValueMinTime);
            this.grpBoxHumidity.Controls.Add(this.lblSensorHumidityValueMax);
            this.grpBoxHumidity.Controls.Add(this.lblSensorHumidityValueMin);
            this.grpBoxHumidity.Controls.Add(this.label14);
            this.grpBoxHumidity.Controls.Add(this.label15);
            this.grpBoxHumidity.Controls.Add(this.lblSensorHumidityValue);
            this.grpBoxHumidity.Controls.Add(this.picColHumidity);
            this.grpBoxHumidity.Enabled = false;
            this.grpBoxHumidity.Location = new System.Drawing.Point(319, 185);
            this.grpBoxHumidity.Name = "grpBoxHumidity";
            this.grpBoxHumidity.Size = new System.Drawing.Size(285, 110);
            this.grpBoxHumidity.TabIndex = 28;
            this.grpBoxHumidity.TabStop = false;
            // 
            // lblSensorHumidityName
            // 
            this.lblSensorHumidityName.AutoSize = true;
            this.lblSensorHumidityName.BackColor = System.Drawing.Color.Transparent;
            this.lblSensorHumidityName.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSensorHumidityName.Location = new System.Drawing.Point(2, 6);
            this.lblSensorHumidityName.Name = "lblSensorHumidityName";
            this.lblSensorHumidityName.Size = new System.Drawing.Size(189, 29);
            this.lblSensorHumidityName.TabIndex = 23;
            this.lblSensorHumidityName.Text = "Luftfeuchtigkeit";
            // 
            // lblSensorHumidityValueMaxTime
            // 
            this.lblSensorHumidityValueMaxTime.AutoSize = true;
            this.lblSensorHumidityValueMaxTime.Location = new System.Drawing.Point(119, 89);
            this.lblSensorHumidityValueMaxTime.Name = "lblSensorHumidityValueMaxTime";
            this.lblSensorHumidityValueMaxTime.Size = new System.Drawing.Size(54, 13);
            this.lblSensorHumidityValueMaxTime.TabIndex = 22;
            this.lblSensorHumidityValueMaxTime.Text = "Kein Wert";
            // 
            // lblSensorHumidityValueMinTime
            // 
            this.lblSensorHumidityValueMinTime.AutoSize = true;
            this.lblSensorHumidityValueMinTime.Location = new System.Drawing.Point(119, 69);
            this.lblSensorHumidityValueMinTime.Name = "lblSensorHumidityValueMinTime";
            this.lblSensorHumidityValueMinTime.Size = new System.Drawing.Size(54, 13);
            this.lblSensorHumidityValueMinTime.TabIndex = 21;
            this.lblSensorHumidityValueMinTime.Text = "Kein Wert";
            // 
            // lblSensorHumidityValueMax
            // 
            this.lblSensorHumidityValueMax.AutoSize = true;
            this.lblSensorHumidityValueMax.Location = new System.Drawing.Point(59, 89);
            this.lblSensorHumidityValueMax.Name = "lblSensorHumidityValueMax";
            this.lblSensorHumidityValueMax.Size = new System.Drawing.Size(54, 13);
            this.lblSensorHumidityValueMax.TabIndex = 20;
            this.lblSensorHumidityValueMax.Text = "Kein Wert";
            // 
            // lblSensorHumidityValueMin
            // 
            this.lblSensorHumidityValueMin.AutoSize = true;
            this.lblSensorHumidityValueMin.Location = new System.Drawing.Point(59, 69);
            this.lblSensorHumidityValueMin.Name = "lblSensorHumidityValueMin";
            this.lblSensorHumidityValueMin.Size = new System.Drawing.Size(54, 13);
            this.lblSensorHumidityValueMin.TabIndex = 19;
            this.lblSensorHumidityValueMin.Text = "Kein Wert";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(10, 89);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(27, 13);
            this.label14.TabIndex = 18;
            this.label14.Text = "Max";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(10, 69);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(24, 13);
            this.label15.TabIndex = 17;
            this.label15.Text = "Min";
            // 
            // lblSensorHumidityValue
            // 
            this.lblSensorHumidityValue.AutoSize = true;
            this.lblSensorHumidityValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSensorHumidityValue.Location = new System.Drawing.Point(57, 36);
            this.lblSensorHumidityValue.Name = "lblSensorHumidityValue";
            this.lblSensorHumidityValue.Size = new System.Drawing.Size(118, 29);
            this.lblSensorHumidityValue.TabIndex = 1;
            this.lblSensorHumidityValue.Text = "Kein Wert";
            // 
            // grpBoxTemperature
            // 
            this.grpBoxTemperature.Controls.Add(this.picTrendTemp);
            this.grpBoxTemperature.Controls.Add(this.lblSensorTempName);
            this.grpBoxTemperature.Controls.Add(this.lblSensorTempMaxTime);
            this.grpBoxTemperature.Controls.Add(this.lblSensorTempMinTime);
            this.grpBoxTemperature.Controls.Add(this.lblSensorTempMax);
            this.grpBoxTemperature.Controls.Add(this.lblSensorTempMin);
            this.grpBoxTemperature.Controls.Add(this.label4);
            this.grpBoxTemperature.Controls.Add(this.label3);
            this.grpBoxTemperature.Controls.Add(this.lblSensorTempValue);
            this.grpBoxTemperature.Controls.Add(this.picColTemp);
            this.grpBoxTemperature.Enabled = false;
            this.grpBoxTemperature.Location = new System.Drawing.Point(6, 69);
            this.grpBoxTemperature.Name = "grpBoxTemperature";
            this.grpBoxTemperature.Size = new System.Drawing.Size(285, 110);
            this.grpBoxTemperature.TabIndex = 27;
            this.grpBoxTemperature.TabStop = false;
            // 
            // lblSensorTempName
            // 
            this.lblSensorTempName.AutoSize = true;
            this.lblSensorTempName.BackColor = System.Drawing.Color.Transparent;
            this.lblSensorTempName.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSensorTempName.Location = new System.Drawing.Point(2, 6);
            this.lblSensorTempName.Name = "lblSensorTempName";
            this.lblSensorTempName.Size = new System.Drawing.Size(149, 29);
            this.lblSensorTempName.TabIndex = 23;
            this.lblSensorTempName.Text = "Temperatur";
            // 
            // lblSensorTempMaxTime
            // 
            this.lblSensorTempMaxTime.AutoSize = true;
            this.lblSensorTempMaxTime.Location = new System.Drawing.Point(119, 89);
            this.lblSensorTempMaxTime.Name = "lblSensorTempMaxTime";
            this.lblSensorTempMaxTime.Size = new System.Drawing.Size(54, 13);
            this.lblSensorTempMaxTime.TabIndex = 22;
            this.lblSensorTempMaxTime.Text = "Kein Wert";
            // 
            // lblSensorTempMinTime
            // 
            this.lblSensorTempMinTime.AutoSize = true;
            this.lblSensorTempMinTime.Location = new System.Drawing.Point(119, 69);
            this.lblSensorTempMinTime.Name = "lblSensorTempMinTime";
            this.lblSensorTempMinTime.Size = new System.Drawing.Size(54, 13);
            this.lblSensorTempMinTime.TabIndex = 21;
            this.lblSensorTempMinTime.Text = "Kein Wert";
            // 
            // lblSensorTempMax
            // 
            this.lblSensorTempMax.AutoSize = true;
            this.lblSensorTempMax.Location = new System.Drawing.Point(59, 89);
            this.lblSensorTempMax.Name = "lblSensorTempMax";
            this.lblSensorTempMax.Size = new System.Drawing.Size(54, 13);
            this.lblSensorTempMax.TabIndex = 20;
            this.lblSensorTempMax.Text = "Kein Wert";
            // 
            // lblSensorTempMin
            // 
            this.lblSensorTempMin.AutoSize = true;
            this.lblSensorTempMin.Location = new System.Drawing.Point(59, 69);
            this.lblSensorTempMin.Name = "lblSensorTempMin";
            this.lblSensorTempMin.Size = new System.Drawing.Size(54, 13);
            this.lblSensorTempMin.TabIndex = 19;
            this.lblSensorTempMin.Text = "Kein Wert";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 89);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(27, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Max";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(24, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Min";
            // 
            // lblSensorTempValue
            // 
            this.lblSensorTempValue.AutoSize = true;
            this.lblSensorTempValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSensorTempValue.Location = new System.Drawing.Point(57, 36);
            this.lblSensorTempValue.Name = "lblSensorTempValue";
            this.lblSensorTempValue.Size = new System.Drawing.Size(118, 29);
            this.lblSensorTempValue.TabIndex = 1;
            this.lblSensorTempValue.Text = "Kein Wert";
            // 
            // chartValues
            // 
            chartArea1.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea1.Name = "ChartArea1";
            this.chartValues.ChartAreas.Add(chartArea1);
            this.chartValues.Location = new System.Drawing.Point(10, 516);
            this.chartValues.Name = "chartValues";
            this.chartValues.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.IsVisibleInLegend = false;
            series1.Name = "Temperature";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series2.IsVisibleInLegend = false;
            series2.Name = "HeatIndex";
            this.chartValues.Series.Add(series1);
            this.chartValues.Series.Add(series2);
            this.chartValues.Size = new System.Drawing.Size(609, 173);
            this.chartValues.TabIndex = 36;
            this.chartValues.Text = "chart1";
            this.chartValues.Click += new System.EventHandler(this.chartValues_Click);
            this.chartValues.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chartValues_MouseMove);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menüToolStripMenuItem,
            this.kommandosToolStripMenuItem,
            this.zusatzToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(634, 24);
            this.menuStrip1.TabIndex = 37;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menüToolStripMenuItem
            // 
            this.menüToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmOptions,
            this.toolStripMenuItem1,
            this.tsmEnd});
            this.menüToolStripMenuItem.Name = "menüToolStripMenuItem";
            this.menüToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.menüToolStripMenuItem.Text = "Menü";
            // 
            // tsmOptions
            // 
            this.tsmOptions.Name = "tsmOptions";
            this.tsmOptions.Size = new System.Drawing.Size(124, 22);
            this.tsmOptions.Text = "Optionen";
            this.tsmOptions.Click += new System.EventHandler(this.tsmOptions_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(121, 6);
            // 
            // tsmEnd
            // 
            this.tsmEnd.Name = "tsmEnd";
            this.tsmEnd.Size = new System.Drawing.Size(124, 22);
            this.tsmEnd.Text = "Beenden";
            this.tsmEnd.Click += new System.EventHandler(this.tsmEnd_Click);
            // 
            // kommandosToolStripMenuItem
            // 
            this.kommandosToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.blauAnToolStripMenuItem,
            this.blauStatusToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.getVersionToolStripMenuItem,
            this.getActualDataToolStripMenuItem,
            this.testInvalidesKommandoToolStripMenuItem,
            this.getHTTPDataToolStripMenuItem,
            this.sQLTestToolStripMenuItem});
            this.kommandosToolStripMenuItem.Name = "kommandosToolStripMenuItem";
            this.kommandosToolStripMenuItem.Size = new System.Drawing.Size(87, 20);
            this.kommandosToolStripMenuItem.Text = "Kommandos";
            // 
            // blauAnToolStripMenuItem
            // 
            this.blauAnToolStripMenuItem.Name = "blauAnToolStripMenuItem";
            this.blauAnToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.blauAnToolStripMenuItem.Text = "Blau ändern";
            this.blauAnToolStripMenuItem.Click += new System.EventHandler(this.blauAnToolStripMenuItem_Click);
            // 
            // blauStatusToolStripMenuItem
            // 
            this.blauStatusToolStripMenuItem.Name = "blauStatusToolStripMenuItem";
            this.blauStatusToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.blauStatusToolStripMenuItem.Text = "Blau Status";
            this.blauStatusToolStripMenuItem.Click += new System.EventHandler(this.blauStatusToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // getVersionToolStripMenuItem
            // 
            this.getVersionToolStripMenuItem.Name = "getVersionToolStripMenuItem";
            this.getVersionToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.getVersionToolStripMenuItem.Text = "Get Version";
            this.getVersionToolStripMenuItem.Click += new System.EventHandler(this.getVersionToolStripMenuItem_Click);
            // 
            // getActualDataToolStripMenuItem
            // 
            this.getActualDataToolStripMenuItem.Name = "getActualDataToolStripMenuItem";
            this.getActualDataToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.getActualDataToolStripMenuItem.Text = "Get latest data";
            this.getActualDataToolStripMenuItem.Click += new System.EventHandler(this.getActualDataToolStripMenuItem_Click);
            // 
            // testInvalidesKommandoToolStripMenuItem
            // 
            this.testInvalidesKommandoToolStripMenuItem.Name = "testInvalidesKommandoToolStripMenuItem";
            this.testInvalidesKommandoToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.testInvalidesKommandoToolStripMenuItem.Text = "Test invalides cmd";
            this.testInvalidesKommandoToolStripMenuItem.Click += new System.EventHandler(this.testInvalidesKommandoToolStripMenuItem_Click);
            // 
            // getHTTPDataToolStripMenuItem
            // 
            this.getHTTPDataToolStripMenuItem.Name = "getHTTPDataToolStripMenuItem";
            this.getHTTPDataToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.getHTTPDataToolStripMenuItem.Text = "Get HTTP Data";
            this.getHTTPDataToolStripMenuItem.Click += new System.EventHandler(this.getHTTPDataToolStripMenuItem_Click);
            // 
            // sQLTestToolStripMenuItem
            // 
            this.sQLTestToolStripMenuItem.Name = "sQLTestToolStripMenuItem";
            this.sQLTestToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.sQLTestToolStripMenuItem.Text = "SQL Test";
            this.sQLTestToolStripMenuItem.Click += new System.EventHandler(this.sQLTestToolStripMenuItem_Click);
            // 
            // zusatzToolStripMenuItem
            // 
            this.zusatzToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.detailierteInformationenToolStripMenuItem});
            this.zusatzToolStripMenuItem.Name = "zusatzToolStripMenuItem";
            this.zusatzToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.zusatzToolStripMenuItem.Text = "Zusatz";
            // 
            // detailierteInformationenToolStripMenuItem
            // 
            this.detailierteInformationenToolStripMenuItem.Name = "detailierteInformationenToolStripMenuItem";
            this.detailierteInformationenToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.detailierteInformationenToolStripMenuItem.Text = "Detailierte Informationen";
            this.detailierteInformationenToolStripMenuItem.Click += new System.EventHandler(this.detailierteInformationenToolStripMenuItem_Click);
            // 
            // lblNumLogEntries
            // 
            this.lblNumLogEntries.AutoSize = true;
            this.lblNumLogEntries.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumLogEntries.Location = new System.Drawing.Point(7, 489);
            this.lblNumLogEntries.Name = "lblNumLogEntries";
            this.lblNumLogEntries.Size = new System.Drawing.Size(136, 16);
            this.lblNumLogEntries.TabIndex = 38;
            this.lblNumLogEntries.Text = "Datensätze: <N/A>";
            // 
            // lstViewDetail
            // 
            this.lstViewDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstViewDetail.Location = new System.Drawing.Point(638, 94);
            this.lstViewDetail.Name = "lstViewDetail";
            this.lstViewDetail.Size = new System.Drawing.Size(284, 595);
            this.lstViewDetail.TabIndex = 39;
            this.lstViewDetail.UseCompatibleStateImageBehavior = false;
            // 
            // lblHistoryDataLV
            // 
            this.lblHistoryDataLV.AutoSize = true;
            this.lblHistoryDataLV.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHistoryDataLV.Location = new System.Drawing.Point(632, 44);
            this.lblHistoryDataLV.Name = "lblHistoryDataLV";
            this.lblHistoryDataLV.Size = new System.Drawing.Size(256, 31);
            this.lblHistoryDataLV.TabIndex = 40;
            this.lblHistoryDataLV.Text = "Historische Daten:";
            // 
            // picTrendError
            // 
            this.picTrendError.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.picTrendError.Image = global::Arduino_Temperature_Retrofit.Properties.Resources.Error;
            this.picTrendError.Location = new System.Drawing.Point(444, 353);
            this.picTrendError.Name = "picTrendError";
            this.picTrendError.Size = new System.Drawing.Size(30, 30);
            this.picTrendError.TabIndex = 43;
            this.picTrendError.TabStop = false;
            this.picTrendError.Visible = false;
            // 
            // picTrendSame
            // 
            this.picTrendSame.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.picTrendSame.Image = global::Arduino_Temperature_Retrofit.Properties.Resources.Trend_Same;
            this.picTrendSame.Location = new System.Drawing.Point(408, 353);
            this.picTrendSame.Name = "picTrendSame";
            this.picTrendSame.Size = new System.Drawing.Size(30, 30);
            this.picTrendSame.TabIndex = 42;
            this.picTrendSame.TabStop = false;
            this.picTrendSame.Visible = false;
            // 
            // picConnStatus
            // 
            this.picConnStatus.BackColor = System.Drawing.SystemColors.Control;
            this.picConnStatus.Location = new System.Drawing.Point(68, 22);
            this.picConnStatus.Name = "picConnStatus";
            this.picConnStatus.Size = new System.Drawing.Size(25, 25);
            this.picConnStatus.TabIndex = 36;
            this.picConnStatus.TabStop = false;
            // 
            // picTrendDown
            // 
            this.picTrendDown.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.picTrendDown.Image = global::Arduino_Temperature_Retrofit.Properties.Resources.Trend_Down;
            this.picTrendDown.Location = new System.Drawing.Point(408, 317);
            this.picTrendDown.Name = "picTrendDown";
            this.picTrendDown.Size = new System.Drawing.Size(30, 30);
            this.picTrendDown.TabIndex = 41;
            this.picTrendDown.TabStop = false;
            this.picTrendDown.Visible = false;
            // 
            // picTrendUp
            // 
            this.picTrendUp.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.picTrendUp.Image = global::Arduino_Temperature_Retrofit.Properties.Resources.Trend_UP;
            this.picTrendUp.Location = new System.Drawing.Point(444, 317);
            this.picTrendUp.Name = "picTrendUp";
            this.picTrendUp.Size = new System.Drawing.Size(30, 30);
            this.picTrendUp.TabIndex = 40;
            this.picTrendUp.TabStop = false;
            this.picTrendUp.Visible = false;
            // 
            // picTrendHeatIndex
            // 
            this.picTrendHeatIndex.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.picTrendHeatIndex.Location = new System.Drawing.Point(213, 35);
            this.picTrendHeatIndex.Name = "picTrendHeatIndex";
            this.picTrendHeatIndex.Size = new System.Drawing.Size(30, 30);
            this.picTrendHeatIndex.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picTrendHeatIndex.TabIndex = 40;
            this.picTrendHeatIndex.TabStop = false;
            // 
            // picColHeatIndex
            // 
            this.picColHeatIndex.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.picColHeatIndex.Location = new System.Drawing.Point(0, 0);
            this.picColHeatIndex.Name = "picColHeatIndex";
            this.picColHeatIndex.Size = new System.Drawing.Size(290, 5);
            this.picColHeatIndex.TabIndex = 0;
            this.picColHeatIndex.TabStop = false;
            this.picColHeatIndex.Click += new System.EventHandler(this.picColHeatIndex_Click);
            // 
            // picTrendLUX
            // 
            this.picTrendLUX.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.picTrendLUX.Location = new System.Drawing.Point(213, 35);
            this.picTrendLUX.Name = "picTrendLUX";
            this.picTrendLUX.Size = new System.Drawing.Size(30, 30);
            this.picTrendLUX.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picTrendLUX.TabIndex = 45;
            this.picTrendLUX.TabStop = false;
            // 
            // picColLUX
            // 
            this.picColLUX.BackColor = System.Drawing.Color.Aqua;
            this.picColLUX.Location = new System.Drawing.Point(0, 0);
            this.picColLUX.Name = "picColLUX";
            this.picColLUX.Size = new System.Drawing.Size(290, 5);
            this.picColLUX.TabIndex = 0;
            this.picColLUX.TabStop = false;
            this.picColLUX.Click += new System.EventHandler(this.picColLUX_Click);
            // 
            // picTrendAirPressure
            // 
            this.picTrendAirPressure.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.picTrendAirPressure.Location = new System.Drawing.Point(213, 36);
            this.picTrendAirPressure.Name = "picTrendAirPressure";
            this.picTrendAirPressure.Size = new System.Drawing.Size(30, 30);
            this.picTrendAirPressure.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picTrendAirPressure.TabIndex = 43;
            this.picTrendAirPressure.TabStop = false;
            // 
            // picColAirPressure
            // 
            this.picColAirPressure.BackColor = System.Drawing.Color.Blue;
            this.picColAirPressure.Location = new System.Drawing.Point(0, 0);
            this.picColAirPressure.Name = "picColAirPressure";
            this.picColAirPressure.Size = new System.Drawing.Size(290, 5);
            this.picColAirPressure.TabIndex = 0;
            this.picColAirPressure.TabStop = false;
            this.picColAirPressure.Click += new System.EventHandler(this.picColAirPressure_Click);
            // 
            // picTrendHumidity
            // 
            this.picTrendHumidity.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.picTrendHumidity.Location = new System.Drawing.Point(214, 36);
            this.picTrendHumidity.Name = "picTrendHumidity";
            this.picTrendHumidity.Size = new System.Drawing.Size(30, 30);
            this.picTrendHumidity.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picTrendHumidity.TabIndex = 44;
            this.picTrendHumidity.TabStop = false;
            // 
            // picColHumidity
            // 
            this.picColHumidity.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.picColHumidity.Location = new System.Drawing.Point(0, 0);
            this.picColHumidity.Name = "picColHumidity";
            this.picColHumidity.Size = new System.Drawing.Size(279, 5);
            this.picColHumidity.TabIndex = 0;
            this.picColHumidity.TabStop = false;
            this.picColHumidity.Click += new System.EventHandler(this.picColHumidity_Click);
            // 
            // picTrendTemp
            // 
            this.picTrendTemp.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.picTrendTemp.Location = new System.Drawing.Point(213, 35);
            this.picTrendTemp.Name = "picTrendTemp";
            this.picTrendTemp.Size = new System.Drawing.Size(30, 30);
            this.picTrendTemp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picTrendTemp.TabIndex = 39;
            this.picTrendTemp.TabStop = false;
            // 
            // picColTemp
            // 
            this.picColTemp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.picColTemp.Location = new System.Drawing.Point(0, -5);
            this.picColTemp.Name = "picColTemp";
            this.picColTemp.Size = new System.Drawing.Size(290, 10);
            this.picColTemp.TabIndex = 0;
            this.picColTemp.TabStop = false;
            this.picColTemp.Click += new System.EventHandler(this.picColTemp_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 701);
            this.Controls.Add(this.lblHistoryDataLV);
            this.Controls.Add(this.lstViewDetail);
            this.Controls.Add(this.lblNumLogEntries);
            this.Controls.Add(this.chartValues);
            this.Controls.Add(this.lblChartSelection);
            this.Controls.Add(this.cboChartSelection);
            this.Controls.Add(this.grpBoxSensor);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.Text = "Arduino Temperature";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMain_KeyDown);
            this.grpBoxSensor.ResumeLayout(false);
            this.grpBoxSensor.PerformLayout();
            this.grpBoxHeatIndex.ResumeLayout(false);
            this.grpBoxHeatIndex.PerformLayout();
            this.grpBoxLUX.ResumeLayout(false);
            this.grpBoxLUX.PerformLayout();
            this.grpBoxAirPressure.ResumeLayout(false);
            this.grpBoxAirPressure.PerformLayout();
            this.grpBoxHumidity.ResumeLayout(false);
            this.grpBoxHumidity.PerformLayout();
            this.grpBoxTemperature.ResumeLayout(false);
            this.grpBoxTemperature.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartValues)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picTrendError)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTrendSame)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picConnStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTrendDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTrendUp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTrendHeatIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picColHeatIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTrendLUX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picColLUX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTrendAirPressure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picColAirPressure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTrendHumidity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picColHumidity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTrendTemp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picColTemp)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblChartSelection;
        private System.Windows.Forms.ComboBox cboChartSelection;
        private System.Windows.Forms.GroupBox grpBoxSensor;
        private System.Windows.Forms.ComboBox cboSensors;
        private System.Windows.Forms.GroupBox grpBoxHeatIndex;
        private System.Windows.Forms.Label lblSensorHeatIndexName;
        private System.Windows.Forms.Label lblSensorHeatIndexMaxTime;
        private System.Windows.Forms.Label lblSensorHeatIndexMinTime;
        private System.Windows.Forms.Label lblSensorHeatIndexMax;
        private System.Windows.Forms.Label lblSensorHeatIndexMin;
        private System.Windows.Forms.Label lblSensorHeatIndexMaxName;
        private System.Windows.Forms.Label lblSensorHeatIndexMinName;
        private System.Windows.Forms.Label lblSensorHeatIndexValue;
        private System.Windows.Forms.PictureBox picColHeatIndex;
        private System.Windows.Forms.Label lblSensorLastUpdated;
        private System.Windows.Forms.Label lblSensor;
        private System.Windows.Forms.GroupBox grpBoxLUX;
        private System.Windows.Forms.Label lblSensorLuxName;
        private System.Windows.Forms.Label lblSensorLuxMaxTime;
        private System.Windows.Forms.Label lblSensorLuxMinTime;
        private System.Windows.Forms.Label lblSensorLuxMax;
        private System.Windows.Forms.Label lblSensorLuxMin;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label lblSensorLuxValue;
        private System.Windows.Forms.PictureBox picColLUX;
        private System.Windows.Forms.GroupBox grpBoxAirPressure;
        private System.Windows.Forms.Label lblSensorPressureName;
        private System.Windows.Forms.Label lblSensorPressureMaxTime;
        private System.Windows.Forms.Label lblSensorPressureMinTime;
        private System.Windows.Forms.Label lblSensorPressureMax;
        private System.Windows.Forms.Label lblSensorPressureMin;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label lblSensorPressureValue;
        private System.Windows.Forms.PictureBox picColAirPressure;
        private System.Windows.Forms.GroupBox grpBoxHumidity;
        private System.Windows.Forms.Label lblSensorHumidityName;
        private System.Windows.Forms.Label lblSensorHumidityValueMaxTime;
        private System.Windows.Forms.Label lblSensorHumidityValueMinTime;
        private System.Windows.Forms.Label lblSensorHumidityValueMax;
        private System.Windows.Forms.Label lblSensorHumidityValueMin;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label lblSensorHumidityValue;
        private System.Windows.Forms.PictureBox picColHumidity;
        private System.Windows.Forms.GroupBox grpBoxTemperature;
        private System.Windows.Forms.Label lblSensorTempName;
        private System.Windows.Forms.Label lblSensorTempMaxTime;
        private System.Windows.Forms.Label lblSensorTempMinTime;
        private System.Windows.Forms.Label lblSensorTempMax;
        private System.Windows.Forms.Label lblSensorTempMin;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblSensorTempValue;
        private System.Windows.Forms.PictureBox picColTemp;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartValues;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menüToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmOptions;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem tsmEnd;
        private System.Windows.Forms.ToolStripMenuItem kommandosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem blauAnToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem blauStatusToolStripMenuItem;
        private System.Windows.Forms.PictureBox picConnStatus;
        private System.Windows.Forms.ToolTip frmMainToolTip;
        private System.Windows.Forms.Label lblNumLogEntries;
        private System.Windows.Forms.PictureBox picTrendTemp;
        private System.Windows.Forms.PictureBox picTrendUp;
        private System.Windows.Forms.PictureBox picTrendDown;
        private System.Windows.Forms.PictureBox picTrendSame;
        private System.Windows.Forms.PictureBox picTrendHeatIndex;
        private System.Windows.Forms.PictureBox picTrendLUX;
        private System.Windows.Forms.PictureBox picTrendAirPressure;
        private System.Windows.Forms.PictureBox picTrendHumidity;
        private System.Windows.Forms.ToolStripMenuItem zusatzToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem detailierteInformationenToolStripMenuItem;
        private System.Windows.Forms.ListView lstViewDetail;
        private System.Windows.Forms.Label lblHistoryDataLV;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getVersionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getActualDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testInvalidesKommandoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getHTTPDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sQLTestToolStripMenuItem;
        private System.Windows.Forms.PictureBox picTrendError;
    }
}


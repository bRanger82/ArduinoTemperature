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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.lblChartSelection = new System.Windows.Forms.Label();
            this.cboChartSelection = new System.Windows.Forms.ComboBox();
            this.grpBoxSensorOne = new System.Windows.Forms.GroupBox();
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
            this.picColHeatIndex = new System.Windows.Forms.PictureBox();
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
            this.picColLUX = new System.Windows.Forms.PictureBox();
            this.grpBoxAirPressure = new System.Windows.Forms.GroupBox();
            this.lblSensorPressureName = new System.Windows.Forms.Label();
            this.lblSensorPressureMaxTime = new System.Windows.Forms.Label();
            this.lblSensorPressureMinTime = new System.Windows.Forms.Label();
            this.lblSensorPressureMax = new System.Windows.Forms.Label();
            this.lblSensorPressureMin = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.lblSensorPressureValue = new System.Windows.Forms.Label();
            this.picColAirPressure = new System.Windows.Forms.PictureBox();
            this.grpBoxHumidity = new System.Windows.Forms.GroupBox();
            this.lblSensorHumidityName = new System.Windows.Forms.Label();
            this.lblSensorHumidityValueMaxTime = new System.Windows.Forms.Label();
            this.lblSensorHumidityValueMinTime = new System.Windows.Forms.Label();
            this.lblSensorHumidityValueMax = new System.Windows.Forms.Label();
            this.lblSensorHumidityValueMin = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.lblSensorHumidityValue = new System.Windows.Forms.Label();
            this.picColHumidity = new System.Windows.Forms.PictureBox();
            this.grpBoxTemperature = new System.Windows.Forms.GroupBox();
            this.lblSensorTempName = new System.Windows.Forms.Label();
            this.lblSensorTempMaxTime = new System.Windows.Forms.Label();
            this.lblSensorTempMinTime = new System.Windows.Forms.Label();
            this.lblSensorTempMax = new System.Windows.Forms.Label();
            this.lblSensorTempMin = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblSensorTempValue = new System.Windows.Forms.Label();
            this.picColTemp = new System.Windows.Forms.PictureBox();
            this.chartValues = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menüToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmEnd = new System.Windows.Forms.ToolStripMenuItem();
            this.grpBoxSensorOne.SuspendLayout();
            this.grpBoxHeatIndex.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picColHeatIndex)).BeginInit();
            this.grpBoxLUX.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picColLUX)).BeginInit();
            this.grpBoxAirPressure.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picColAirPressure)).BeginInit();
            this.grpBoxHumidity.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picColHumidity)).BeginInit();
            this.grpBoxTemperature.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picColTemp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartValues)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblChartSelection
            // 
            this.lblChartSelection.AutoSize = true;
            this.lblChartSelection.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChartSelection.Location = new System.Drawing.Point(221, 484);
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
            this.cboChartSelection.Location = new System.Drawing.Point(336, 481);
            this.cboChartSelection.Name = "cboChartSelection";
            this.cboChartSelection.Size = new System.Drawing.Size(136, 24);
            this.cboChartSelection.TabIndex = 34;
            this.cboChartSelection.SelectedIndexChanged += new System.EventHandler(this.cboChartSelection_SelectedIndexChanged);
            // 
            // grpBoxSensorOne
            // 
            this.grpBoxSensorOne.Controls.Add(this.cboSensors);
            this.grpBoxSensorOne.Controls.Add(this.grpBoxHeatIndex);
            this.grpBoxSensorOne.Controls.Add(this.lblSensorLastUpdated);
            this.grpBoxSensorOne.Controls.Add(this.lblSensor);
            this.grpBoxSensorOne.Controls.Add(this.grpBoxLUX);
            this.grpBoxSensorOne.Controls.Add(this.grpBoxAirPressure);
            this.grpBoxSensorOne.Controls.Add(this.grpBoxHumidity);
            this.grpBoxSensorOne.Controls.Add(this.grpBoxTemperature);
            this.grpBoxSensorOne.Location = new System.Drawing.Point(12, 25);
            this.grpBoxSensorOne.Name = "grpBoxSensorOne";
            this.grpBoxSensorOne.Size = new System.Drawing.Size(463, 447);
            this.grpBoxSensorOne.TabIndex = 33;
            this.grpBoxSensorOne.TabStop = false;
            // 
            // cboSensors
            // 
            this.cboSensors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSensors.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboSensors.FormattingEnabled = true;
            this.cboSensors.Location = new System.Drawing.Point(238, 19);
            this.cboSensors.Name = "cboSensors";
            this.cboSensors.Size = new System.Drawing.Size(222, 33);
            this.cboSensors.TabIndex = 35;
            this.cboSensors.SelectedIndexChanged += new System.EventHandler(this.cboSensors_SelectedIndexChanged);
            // 
            // grpBoxHeatIndex
            // 
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
            this.grpBoxHeatIndex.Location = new System.Drawing.Point(238, 69);
            this.grpBoxHeatIndex.Name = "grpBoxHeatIndex";
            this.grpBoxHeatIndex.Size = new System.Drawing.Size(225, 110);
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
            // picColHeatIndex
            // 
            this.picColHeatIndex.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.picColHeatIndex.Location = new System.Drawing.Point(0, 0);
            this.picColHeatIndex.Name = "picColHeatIndex";
            this.picColHeatIndex.Size = new System.Drawing.Size(225, 5);
            this.picColHeatIndex.TabIndex = 0;
            this.picColHeatIndex.TabStop = false;
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
            this.lblSensor.Location = new System.Drawing.Point(47, 22);
            this.lblSensor.Name = "lblSensor";
            this.lblSensor.Size = new System.Drawing.Size(189, 25);
            this.lblSensor.TabIndex = 31;
            this.lblSensor.Text = "Sensor-Auswahl:";
            // 
            // grpBoxLUX
            // 
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
            this.grpBoxLUX.Size = new System.Drawing.Size(225, 110);
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
            // picColLUX
            // 
            this.picColLUX.BackColor = System.Drawing.Color.Aqua;
            this.picColLUX.Location = new System.Drawing.Point(0, 0);
            this.picColLUX.Name = "picColLUX";
            this.picColLUX.Size = new System.Drawing.Size(225, 5);
            this.picColLUX.TabIndex = 0;
            this.picColLUX.TabStop = false;
            // 
            // grpBoxAirPressure
            // 
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
            this.grpBoxAirPressure.Size = new System.Drawing.Size(225, 110);
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
            // picColAirPressure
            // 
            this.picColAirPressure.BackColor = System.Drawing.Color.Blue;
            this.picColAirPressure.Location = new System.Drawing.Point(0, 0);
            this.picColAirPressure.Name = "picColAirPressure";
            this.picColAirPressure.Size = new System.Drawing.Size(225, 5);
            this.picColAirPressure.TabIndex = 0;
            this.picColAirPressure.TabStop = false;
            // 
            // grpBoxHumidity
            // 
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
            this.grpBoxHumidity.Location = new System.Drawing.Point(237, 185);
            this.grpBoxHumidity.Name = "grpBoxHumidity";
            this.grpBoxHumidity.Size = new System.Drawing.Size(225, 110);
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
            // picColHumidity
            // 
            this.picColHumidity.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.picColHumidity.Location = new System.Drawing.Point(0, 0);
            this.picColHumidity.Name = "picColHumidity";
            this.picColHumidity.Size = new System.Drawing.Size(225, 5);
            this.picColHumidity.TabIndex = 0;
            this.picColHumidity.TabStop = false;
            // 
            // grpBoxTemperature
            // 
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
            this.grpBoxTemperature.Size = new System.Drawing.Size(225, 110);
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
            // picColTemp
            // 
            this.picColTemp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.picColTemp.Location = new System.Drawing.Point(0, 0);
            this.picColTemp.Name = "picColTemp";
            this.picColTemp.Size = new System.Drawing.Size(225, 5);
            this.picColTemp.TabIndex = 0;
            this.picColTemp.TabStop = false;
            // 
            // chartValues
            // 
            chartArea1.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea1.Name = "ChartArea1";
            this.chartValues.ChartAreas.Add(chartArea1);
            this.chartValues.Location = new System.Drawing.Point(12, 511);
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
            this.chartValues.Size = new System.Drawing.Size(460, 147);
            this.chartValues.TabIndex = 36;
            this.chartValues.Text = "chart1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menüToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(489, 24);
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
            this.tsmOptions.Size = new System.Drawing.Size(152, 22);
            this.tsmOptions.Text = "Optionen";
            this.tsmOptions.Click += new System.EventHandler(this.tsmOptions_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(149, 6);
            // 
            // tsmEnd
            // 
            this.tsmEnd.Name = "tsmEnd";
            this.tsmEnd.Size = new System.Drawing.Size(152, 22);
            this.tsmEnd.Text = "Beenden";
            this.tsmEnd.Click += new System.EventHandler(this.tsmEnd_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 671);
            this.Controls.Add(this.chartValues);
            this.Controls.Add(this.lblChartSelection);
            this.Controls.Add(this.cboChartSelection);
            this.Controls.Add(this.grpBoxSensorOne);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.Text = "Arduino Temperature";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.grpBoxSensorOne.ResumeLayout(false);
            this.grpBoxSensorOne.PerformLayout();
            this.grpBoxHeatIndex.ResumeLayout(false);
            this.grpBoxHeatIndex.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picColHeatIndex)).EndInit();
            this.grpBoxLUX.ResumeLayout(false);
            this.grpBoxLUX.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picColLUX)).EndInit();
            this.grpBoxAirPressure.ResumeLayout(false);
            this.grpBoxAirPressure.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picColAirPressure)).EndInit();
            this.grpBoxHumidity.ResumeLayout(false);
            this.grpBoxHumidity.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picColHumidity)).EndInit();
            this.grpBoxTemperature.ResumeLayout(false);
            this.grpBoxTemperature.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picColTemp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartValues)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblChartSelection;
        private System.Windows.Forms.ComboBox cboChartSelection;
        private System.Windows.Forms.GroupBox grpBoxSensorOne;
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
    }
}


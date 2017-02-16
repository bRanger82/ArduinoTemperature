namespace Arduino_Temperature
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.lblTempTisch = new System.Windows.Forms.Label();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.lblTempBoden = new System.Windows.Forms.Label();
            this.grpSettings = new System.Windows.Forms.GroupBox();
            this.lblHTMLNumEntriesHist = new System.Windows.Forms.Label();
            this.numMaxEntries = new System.Windows.Forms.TrackBar();
            this.chkHTML = new System.Windows.Forms.CheckBox();
            this.lblHTMLUpdated = new System.Windows.Forms.Label();
            this.chkLogEnabled = new System.Windows.Forms.CheckBox();
            this.chkTopMost = new System.Windows.Forms.CheckBox();
            this.lblTableLastUpdated = new System.Windows.Forms.Label();
            this.lblBottomLastUpdated = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.grpSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxEntries)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTempTisch
            // 
            this.lblTempTisch.BackColor = System.Drawing.SystemColors.ControlLight;
            this.lblTempTisch.Font = new System.Drawing.Font("Nirmala UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTempTisch.Location = new System.Drawing.Point(16, 7);
            this.lblTempTisch.Name = "lblTempTisch";
            this.lblTempTisch.Size = new System.Drawing.Size(410, 180);
            this.lblTempTisch.TabIndex = 0;
            this.lblTempTisch.DoubleClick += new System.EventHandler(this.lblTempTisch_DoubleClick);
            // 
            // lblTempBoden
            // 
            this.lblTempBoden.BackColor = System.Drawing.SystemColors.ControlLight;
            this.lblTempBoden.Font = new System.Drawing.Font("Nirmala UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTempBoden.Location = new System.Drawing.Point(16, 210);
            this.lblTempBoden.Name = "lblTempBoden";
            this.lblTempBoden.Size = new System.Drawing.Size(410, 180);
            this.lblTempBoden.TabIndex = 4;
            this.lblTempBoden.DoubleClick += new System.EventHandler(this.lblTempBoden_DoubleClick);
            // 
            // grpSettings
            // 
            this.grpSettings.Controls.Add(this.lblHTMLNumEntriesHist);
            this.grpSettings.Controls.Add(this.numMaxEntries);
            this.grpSettings.Controls.Add(this.chkHTML);
            this.grpSettings.Controls.Add(this.lblHTMLUpdated);
            this.grpSettings.Controls.Add(this.chkLogEnabled);
            this.grpSettings.Controls.Add(this.chkTopMost);
            this.grpSettings.Location = new System.Drawing.Point(16, 411);
            this.grpSettings.Name = "grpSettings";
            this.grpSettings.Size = new System.Drawing.Size(410, 144);
            this.grpSettings.TabIndex = 7;
            this.grpSettings.TabStop = false;
            // 
            // lblHTMLNumEntriesHist
            // 
            this.lblHTMLNumEntriesHist.AutoSize = true;
            this.lblHTMLNumEntriesHist.Location = new System.Drawing.Point(290, 63);
            this.lblHTMLNumEntriesHist.Name = "lblHTMLNumEntriesHist";
            this.lblHTMLNumEntriesHist.Size = new System.Drawing.Size(93, 13);
            this.lblHTMLNumEntriesHist.TabIndex = 12;
            this.lblHTMLNumEntriesHist.Text = "Anzahl Einträge: 0";
            // 
            // numMaxEntries
            // 
            this.numMaxEntries.Location = new System.Drawing.Point(6, 91);
            this.numMaxEntries.Maximum = 100;
            this.numMaxEntries.Minimum = 5;
            this.numMaxEntries.Name = "numMaxEntries";
            this.numMaxEntries.Size = new System.Drawing.Size(392, 45);
            this.numMaxEntries.TabIndex = 11;
            this.numMaxEntries.Value = 5;
            this.numMaxEntries.Scroll += new System.EventHandler(this.numMaxEntries_Scroll);
            // 
            // chkHTML
            // 
            this.chkHTML.AutoSize = true;
            this.chkHTML.Location = new System.Drawing.Point(9, 62);
            this.chkHTML.Name = "chkHTML";
            this.chkHTML.Size = new System.Drawing.Size(56, 17);
            this.chkHTML.TabIndex = 10;
            this.chkHTML.Text = "HTML";
            this.chkHTML.UseVisualStyleBackColor = true;
            // 
            // lblHTMLUpdated
            // 
            this.lblHTMLUpdated.AutoSize = true;
            this.lblHTMLUpdated.Location = new System.Drawing.Point(80, 63);
            this.lblHTMLUpdated.Name = "lblHTMLUpdated";
            this.lblHTMLUpdated.Size = new System.Drawing.Size(108, 13);
            this.lblHTMLUpdated.TabIndex = 9;
            this.lblHTMLUpdated.Text = "HTML letztes Update";
            // 
            // chkLogEnabled
            // 
            this.chkLogEnabled.AutoSize = true;
            this.chkLogEnabled.Location = new System.Drawing.Point(9, 39);
            this.chkLogEnabled.Name = "chkLogEnabled";
            this.chkLogEnabled.Size = new System.Drawing.Size(75, 17);
            this.chkLogEnabled.TabIndex = 8;
            this.chkLogEnabled.Text = "Log to File";
            this.chkLogEnabled.UseVisualStyleBackColor = true;
            // 
            // chkTopMost
            // 
            this.chkTopMost.AutoSize = true;
            this.chkTopMost.Location = new System.Drawing.Point(9, 16);
            this.chkTopMost.Name = "chkTopMost";
            this.chkTopMost.Size = new System.Drawing.Size(71, 17);
            this.chkTopMost.TabIndex = 7;
            this.chkTopMost.Text = "Top-Most";
            this.chkTopMost.UseVisualStyleBackColor = true;
            this.chkTopMost.CheckedChanged += new System.EventHandler(this.chkTopMost_CheckedChanged_1);
            // 
            // lblTableLastUpdated
            // 
            this.lblTableLastUpdated.AutoSize = true;
            this.lblTableLastUpdated.BackColor = System.Drawing.Color.Transparent;
            this.lblTableLastUpdated.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTableLastUpdated.Location = new System.Drawing.Point(14, 175);
            this.lblTableLastUpdated.Name = "lblTableLastUpdated";
            this.lblTableLastUpdated.Size = new System.Drawing.Size(292, 12);
            this.lblTableLastUpdated.TabIndex = 8;
            this.lblTableLastUpdated.Text = "Zuletzt aktualisiert: dd.mm.yyyy hh:mm:ss";
            // 
            // lblBottomLastUpdated
            // 
            this.lblBottomLastUpdated.AutoSize = true;
            this.lblBottomLastUpdated.BackColor = System.Drawing.Color.Transparent;
            this.lblBottomLastUpdated.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBottomLastUpdated.Location = new System.Drawing.Point(14, 378);
            this.lblBottomLastUpdated.Name = "lblBottomLastUpdated";
            this.lblBottomLastUpdated.Size = new System.Drawing.Size(292, 12);
            this.lblBottomLastUpdated.TabIndex = 9;
            this.lblBottomLastUpdated.Text = "Zuletzt aktualisiert: dd.mm.yyyy hh:mm:ss";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 561);
            this.Controls.Add(this.lblBottomLastUpdated);
            this.Controls.Add(this.lblTableLastUpdated);
            this.Controls.Add(this.grpSettings);
            this.Controls.Add(this.lblTempBoden);
            this.Controls.Add(this.lblTempTisch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.Text = "Arduino Temperature Watcher";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.grpSettings.ResumeLayout(false);
            this.grpSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxEntries)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTempTisch;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.Label lblTempBoden;
        private System.Windows.Forms.GroupBox grpSettings;
        private System.Windows.Forms.CheckBox chkHTML;
        private System.Windows.Forms.Label lblHTMLUpdated;
        private System.Windows.Forms.CheckBox chkLogEnabled;
        private System.Windows.Forms.CheckBox chkTopMost;
        private System.Windows.Forms.Label lblTableLastUpdated;
        private System.Windows.Forms.Label lblBottomLastUpdated;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TrackBar numMaxEntries;
        private System.Windows.Forms.Label lblHTMLNumEntriesHist;
    }
}


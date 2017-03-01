namespace Arduino_Temperature_Retrofit
{
    partial class frmOptions
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
            this.grpSettings = new System.Windows.Forms.GroupBox();
            this.chkHTML = new System.Windows.Forms.CheckBox();
            this.chkTopMost = new System.Windows.Forms.CheckBox();
            this.btnOptionsOK = new System.Windows.Forms.Button();
            this.btnOptionsCancel = new System.Windows.Forms.Button();
            this.grpSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpSettings
            // 
            this.grpSettings.Controls.Add(this.chkHTML);
            this.grpSettings.Controls.Add(this.chkTopMost);
            this.grpSettings.Location = new System.Drawing.Point(12, 12);
            this.grpSettings.Name = "grpSettings";
            this.grpSettings.Size = new System.Drawing.Size(256, 146);
            this.grpSettings.TabIndex = 8;
            this.grpSettings.TabStop = false;
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
            // chkTopMost
            // 
            this.chkTopMost.AutoSize = true;
            this.chkTopMost.Location = new System.Drawing.Point(9, 16);
            this.chkTopMost.Name = "chkTopMost";
            this.chkTopMost.Size = new System.Drawing.Size(71, 17);
            this.chkTopMost.TabIndex = 7;
            this.chkTopMost.Text = "Top-Most";
            this.chkTopMost.UseVisualStyleBackColor = true;
            // 
            // btnOptionsOK
            // 
            this.btnOptionsOK.Location = new System.Drawing.Point(102, 164);
            this.btnOptionsOK.Name = "btnOptionsOK";
            this.btnOptionsOK.Size = new System.Drawing.Size(75, 23);
            this.btnOptionsOK.TabIndex = 9;
            this.btnOptionsOK.Text = "OK";
            this.btnOptionsOK.UseVisualStyleBackColor = true;
            this.btnOptionsOK.Click += new System.EventHandler(this.btnOptionsOK_Click);
            // 
            // btnOptionsCancel
            // 
            this.btnOptionsCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOptionsCancel.Location = new System.Drawing.Point(193, 164);
            this.btnOptionsCancel.Name = "btnOptionsCancel";
            this.btnOptionsCancel.Size = new System.Drawing.Size(75, 23);
            this.btnOptionsCancel.TabIndex = 10;
            this.btnOptionsCancel.Text = "Abbrechen";
            this.btnOptionsCancel.UseVisualStyleBackColor = true;
            this.btnOptionsCancel.Click += new System.EventHandler(this.btnOptionsCancel_Click);
            // 
            // frmOptions
            // 
            this.AcceptButton = this.btnOptionsOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnOptionsCancel;
            this.ClientSize = new System.Drawing.Size(284, 204);
            this.Controls.Add(this.btnOptionsCancel);
            this.Controls.Add(this.btnOptionsOK);
            this.Controls.Add(this.grpSettings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmOptions";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Optionen";
            this.Load += new System.EventHandler(this.frmOptions_Load);
            this.grpSettings.ResumeLayout(false);
            this.grpSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSettings;
        private System.Windows.Forms.CheckBox chkHTML;
        private System.Windows.Forms.CheckBox chkTopMost;
        private System.Windows.Forms.Button btnOptionsOK;
        private System.Windows.Forms.Button btnOptionsCancel;
    }
}
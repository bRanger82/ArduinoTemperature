using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Arduino_Temperature
{
    public partial class frmOptions : Form
    {
        public frmOptions(OptionProperties opt)
        {
            InitializeComponent();
            OptionProp = opt;
        }

        private bool _cancel = false;
        public bool Cancel{ get { return _cancel; } }

        public OptionProperties OptionProp { get; set; }

        private void btnOptionsOK_Click(object sender, EventArgs e)
        {
            OptionProp.PropWriteHTML = this.chkHTML.Checked;
            OptionProp.PropLogToFile = this.chkLogEnabled.Checked;
            OptionProp.PropTopMost = this.chkTopMost.Checked;
            OptionProp.NumEntries = this.numMaxEntries.Value;

            _cancel = false;

            this.Close();
        }

        private void btnOptionsCancel_Click(object sender, EventArgs e)
        {
            _cancel = true;
            this.Close();
        }

        private void frmOptions_Load(object sender, EventArgs e)
        {
            this.chkHTML.Checked = OptionProp.PropWriteHTML;
            this.chkLogEnabled.Checked = OptionProp.PropLogToFile;
            this.chkTopMost.Checked = OptionProp.PropTopMost;
            this.numMaxEntries.Value = OptionProp.NumEntries;
            this.lblHTMLNumEntriesHist.Text = "Anzahl Einträge: " + this.numMaxEntries.Value.ToString();

        }

        private void numMaxEntries_Scroll(object sender, EventArgs e)
        {
            this.lblHTMLNumEntriesHist.Text = "Anzahl Einträge: " + this.numMaxEntries.Value.ToString();
        }
    }

    public class OptionProperties
    {
        public bool PropTopMost { get; set; } = false;
        public bool PropLogToFile { get; set; } = false;
        public bool PropWriteHTML { get; set; } = false;
        public int NumEntries { get; set; } = 5;
    }
}

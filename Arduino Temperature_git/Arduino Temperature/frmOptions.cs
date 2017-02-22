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
        public frmOptions(optionProperties opt)
        {
            InitializeComponent();
            OptionProp = opt;
        }

        private bool _cancel = false;
        public bool Cancel{ get { return _cancel; } }

        public optionProperties OptionProp { get; set; }

        private void btnOptionsOK_Click(object sender, EventArgs e)
        {
            OptionProp.propWriteHTML = this.chkHTML.Checked;
            OptionProp.propLogToFile = this.chkLogEnabled.Checked;
            OptionProp.propTopMost = this.chkTopMost.Checked;
            OptionProp.numEntries = this.numMaxEntries.Value;

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
            this.chkHTML.Checked = OptionProp.propWriteHTML;
            this.chkLogEnabled.Checked = OptionProp.propLogToFile;
            this.chkTopMost.Checked = OptionProp.propTopMost;
            this.numMaxEntries.Value = OptionProp.numEntries;
            this.lblHTMLNumEntriesHist.Text = "Anzahl Einträge: " + this.numMaxEntries.Value.ToString();

        }

        private void numMaxEntries_Scroll(object sender, EventArgs e)
        {
            this.lblHTMLNumEntriesHist.Text = "Anzahl Einträge: " + this.numMaxEntries.Value.ToString();
        }
    }

    public class optionProperties
    {
        public bool propTopMost { get; set; }
        public bool propLogToFile { get; set; }
        public bool propWriteHTML { get; set; }
        public int numEntries { get; set; }
    }
}

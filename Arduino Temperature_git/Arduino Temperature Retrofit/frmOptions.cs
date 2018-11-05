using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Arduino_Temperature_Retrofit
{
    public partial class FrmOptions : Form
    {
        public FrmOptions(OptionProperties opt)
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
            OptionProp.PropTopMost = this.chkTopMost.Checked;

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
            this.chkTopMost.Checked = OptionProp.PropTopMost;
        }
        
    }

    /// <summary>
    /// Verwaltet die Einstellungen
    /// </summary>
    public class OptionProperties
    {
        public bool PropTopMost { get; set; } = false;
        public bool PropWriteHTML { get; set; } = false;
    }
}

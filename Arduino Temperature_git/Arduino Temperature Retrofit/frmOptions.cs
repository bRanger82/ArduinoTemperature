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
            OptionProp.propTopMost = this.chkTopMost.Checked;

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
            this.chkTopMost.Checked = OptionProp.propTopMost;
        }
        
    }

    public class optionProperties
    {
        public bool propTopMost { get; set; } = false;
        public bool propWriteHTML { get; set; } = false;
    }
}

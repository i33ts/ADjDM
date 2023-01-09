using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ADjDM
{
    public partial class ReportForm : Form
    {
        public string ReportTextbox;
        public string ReportTitle;
        public ReportForm()
        {
            InitializeComponent();
        }

        private void ReportForm_Load(object sender, EventArgs e)
        {
            this.reportBox.Text = ReportTextbox;
            this.Text = ReportTitle;
        }

    }
}

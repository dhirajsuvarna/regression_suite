using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace regression_test_suite
{
    public partial class PreferenceForm : Form
    {
        public PreferenceForm()
        {
            InitializeComponent();
        }

        private void PreferenceForm_Load(object sender, EventArgs e)
        {
            user_PropertyGrid.SelectedObject = Properties.Regression.Default;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Regression.Default.Save();
            MessageBox.Show("User Preference Saved!!!", this.Text);
        }
    }
}

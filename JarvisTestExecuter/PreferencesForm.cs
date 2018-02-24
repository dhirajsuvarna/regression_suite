using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace testing_cost
{
    public partial class PreferencesForm : Form
    {
        public PreferencesForm()
        {
            InitializeComponent();
        }

        private void PreferencesForm_Load(object sender, EventArgs e)
        {
            user_PropertyGrid.SelectedObject = Properties.Location.Default;
        }

        private void save_Button_Click(object sender, EventArgs e)
        {
            Properties.Location.Default.Save();
            MessageBox.Show("User Preferences Saved!!!", this.Text);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ISCTraining
{
    public partial class Dashboard : Form
    {
        String username;
        public Dashboard(String username)
        {
            InitializeComponent();
            this.username = username;
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            label1.Text = $"Hi {username}";
        }
    }
}

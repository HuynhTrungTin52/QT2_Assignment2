using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ECommerceGUI
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnOpenAgent_Click(object sender, EventArgs e)
        {
            frmAgent agentForm = new frmAgent();
            agentForm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmItem f = new frmItem();
            f.Show();
        }

        private void btnOpenOrder_Click(object sender, EventArgs e)
        {
            frmOrder orderForm = new frmOrder();
            orderForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmFilter f = new frmFilter();
            f.Show();
        }
    }
}

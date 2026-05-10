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
    public partial class frmAgent : Form
    {
        DataTable dtAgent = new DataTable();
        public frmAgent()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void frmAgent_Load(object sender, EventArgs e)
        {
            dtAgent.Columns.Add("AgentID");
            dtAgent.Columns.Add("AgentName");
            dtAgent.Columns.Add("Address");

            dataGridView1.DataSource = dtAgent;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string id = txtAgentID.Text;
            string name = txtAgentName.Text;
            string address = txtAddress.Text;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(address))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Tên và Địa chỉ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }

            dtAgent.Rows.Add(id, name, address);

            txtAgentID.Clear();
            txtAgentName.Clear();
            txtAddress.Clear();

            txtAgentName.Focus();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                dataGridView1.CurrentRow.Cells["AgentID"].Value = txtAgentID.Text;
                dataGridView1.CurrentRow.Cells["AgentName"].Value = txtAgentName.Text;
                dataGridView1.CurrentRow.Cells["Address"].Value = txtAddress.Text;

                txtAgentID.Clear();
                txtAgentName.Clear();
                txtAddress.Clear();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null && !dataGridView1.CurrentRow.IsNewRow)
            {
                
                DataRowView rowView = dataGridView1.CurrentRow.DataBoundItem as DataRowView;

                if (rowView != null)
                {
                    rowView.Row.Delete();

                    txtAgentID.Clear();
                    txtAgentName.Clear();
                    txtAddress.Clear();
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];

                txtAgentID.Text = row.Cells["AgentID"].Value.ToString();
                txtAgentName.Text = row.Cells["AgentName"].Value.ToString();
                txtAddress.Text = row.Cells["Address"].Value.ToString();
            }
        }
    }
}

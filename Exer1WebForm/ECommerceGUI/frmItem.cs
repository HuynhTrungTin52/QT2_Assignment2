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
    public partial class frmItem : Form
    {
        Database db = new Database();
        public frmItem()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dgvItem.CurrentRow != null && !dgvItem.CurrentRow.IsNewRow)
            {
                DataGridViewRow row = dgvItem.CurrentRow;

                row.Cells["ItemID"].Value = txtItemID.Text;
                row.Cells["ItemName"].Value = txtItemName.Text;
                row.Cells["Size"].Value = txtSize.Text;
                row.Cells["Price"].Value = txtPrice.Text;

                MessageBox.Show("Đã cập nhật sản phẩm thành công!");
                ClearText();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm trong bảng để sửa!");
            }
        }

        private void frmItem_Load(object sender, EventArgs e)
        {
            string sql = "SELECT * FROM Item";
            dgvItem.DataSource = db.GetDataTable(sql);

            dgvItem.Columns["ItemID"].HeaderText = "Mã sản phẩm";
            dgvItem.Columns["ItemName"].HeaderText = "Tên sản phẩm";
            dgvItem.Columns["Price"].HeaderText = "Đơn giá";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtItemID.Text) || string.IsNullOrEmpty(txtItemName.Text))
            {
                MessageBox.Show("Nhập đủ Mã và Tên sản phẩm!");
                return;
            }

            if (double.TryParse(txtPrice.Text, out double price))
            {
                try
                {
                    string sql = $"INSERT INTO Item (ItemID, ItemName, Size, Price) " +
                                 $"VALUES ('{txtItemID.Text}', N'{txtItemName.Text}', N'{txtSize.Text}', {price})";

                   
                    db.ExecuteNonQuery(sql);

                    MessageBox.Show("Add successful!");

                    dgvItem.DataSource = db.GetDataTable("SELECT * FROM Item");

                    ClearText();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Price must be a number ");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dgvItem.CurrentRow != null && !dgvItem.CurrentRow.IsNewRow)
            {
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    DataRowView rowView = dgvItem.CurrentRow.DataBoundItem as DataRowView;
                    if (rowView != null)
                    {
                        rowView.Row.Delete();
                        ClearText();
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa!");
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void lblSize_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgvItem_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvItem.Rows[e.RowIndex];
                txtItemID.Text = row.Cells["ItemID"].Value.ToString();
                txtItemName.Text = row.Cells["ItemName"].Value.ToString();
                txtSize.Text = row.Cells["Size"].Value.ToString();
                txtPrice.Text = row.Cells["Price"].Value.ToString();
            }
        }
        private void ClearText()
        {
            txtItemID.Clear();
            txtItemName.Clear();
            txtSize.Clear();
            txtPrice.Clear();
            txtItemID.Focus();
        }
    }
}

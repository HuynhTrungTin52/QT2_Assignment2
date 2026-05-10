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
    public partial class frmOrder : Form
    {
        Database db = new Database();
        public frmOrder()
        {
            InitializeComponent();
        }

        private void frmOrder_Load(object sender, EventArgs e)
        {
            cboItem.DataSource = db.GetDataTable("SELECT * FROM Agent");
            cboItem.DisplayMember = "AgentName";
            cboItem.ValueMember = "AgentID";

            cboItem.DataSource = db.GetDataTable("SELECT * FROM Item");
            cboItem.DisplayMember = "ItemName";
            cboItem.ValueMember = "ItemID";
        }

        private void dgvOrderDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1 || e.ColumnIndex == 2)
            {
                DataGridViewRow row = dgvOrderDetail.Rows[e.RowIndex];

                double qty = Convert.ToDouble(row.Cells["colQty"].Value ?? 0);
                double price = Convert.ToDouble(row.Cells["colPrice"].Value ?? 0);

                row.Cells["colSubTotal"].Value = qty * price;

                double total = 0;
                foreach (DataGridViewRow r in dgvOrderDetail.Rows)
                {
                    total += Convert.ToDouble(r.Cells["colSubTotal"].Value ?? 0);
                }
                txtPrice.Text = total.ToString();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtOrderId.Text) || cboItem.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng nhập đủ Mã hóa đơn và chọn Đại lý!");
                return;
            }
            MessageBox.Show("Đã lưu hóa đơn " + txtOrderId.Text + " thành công vào Database!", "Thông báo");

            
            dgvOrderDetail.Rows.Clear();
            txtPrice.Clear();
            txtOrderId.Clear();
        }

        private void btnInHoaDon_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtOrderId.Text))
            {
                MessageBox.Show("Vui lòng nhập Mã hóa đơn trước khi in!", "Thông báo");
                return;
            }

            frmPrintOrder fPrint = new frmPrintOrder();

            fPrint.GenerateInvoice(
                txtOrderId.Text,  
                cboItem.Text,    
                dtpDate.Text,      
                txtPrice.Text,      
                dgvOrderDetail     
            );

            fPrint.ShowDialog();
        }

        private void cboItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboItem.SelectedValue != null && cboItem.SelectedValue is string)
            {
                string id = cboItem.SelectedValue.ToString();
                DataTable dt = db.GetDataTable($"SELECT Price FROM Item WHERE ItemID = '{id}'");
                if (dt.Rows.Count > 0)
                {
                    txtPrice.Text = dt.Rows[0]["Price"].ToString();
                }
            }
        }

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            string ngay = dtpDate.Value.ToString("yyyy-MM-dd"); 
        }
    }
}

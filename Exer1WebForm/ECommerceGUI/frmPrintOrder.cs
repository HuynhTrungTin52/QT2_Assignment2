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
    public partial class frmPrintOrder : Form
    {
        public frmPrintOrder()
        {
            InitializeComponent();
        }

        private void rtbInvoice_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            PrintDialog printDlg = new PrintDialog();
            if (printDlg.ShowDialog() == DialogResult.OK)
            {
     
                MessageBox.Show("Hệ thống đang gửi dữ liệu đến máy in...", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close(); 
            }
        }
        public void GenerateInvoice(string orderID, string agentName, string date, string total, DataGridView detailGrid)
        {
            rtbInvoice.Clear();

            rtbInvoice.SelectionAlignment = HorizontalAlignment.Center;
            rtbInvoice.SelectionFont = new Font(rtbInvoice.Font, FontStyle.Bold);
            rtbInvoice.AppendText("SALES INVOICE\n");
            rtbInvoice.AppendText("E-Commerce System: Hong Bao Nhi\n");
            rtbInvoice.AppendText("------------------------------------------\n\n");

            rtbInvoice.SelectionAlignment = HorizontalAlignment.Left;
            rtbInvoice.SelectionFont = new Font(rtbInvoice.Font, FontStyle.Regular);
            rtbInvoice.AppendText($"Order ID:   {orderID}\n");
            rtbInvoice.AppendText($"Customer:   {agentName}\n");
            rtbInvoice.AppendText($"Date:       {date}\n");
            rtbInvoice.AppendText("------------------------------------------\n");

            rtbInvoice.AppendText(string.Format("{0,-20} {1,5} {2,10}\n", "Item Name", "Qty", "Amount"));
            rtbInvoice.AppendText("------------------------------------------\n");

  
            foreach (DataGridViewRow row in detailGrid.Rows)
            {
                if (row.Cells["colItem"].Value != null)
                {
                    string itemName = row.Cells["colItem"].Value.ToString();
                    string qty = row.Cells["colQty"].Value.ToString();
                    string subTotal = row.Cells["colSubTotal"].Value.ToString();
                    rtbInvoice.AppendText(string.Format("{0,-20} {1,5} {2,10}\n", itemName, qty, subTotal));
                }
            }


            rtbInvoice.AppendText("------------------------------------------\n");
            rtbInvoice.SelectionFont = new Font(rtbInvoice.Font, FontStyle.Bold);
            rtbInvoice.AppendText($"TOTAL AMOUNT: {total} VNĐ\n");
            rtbInvoice.AppendText("\n\tThank you for your business!");
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Đang in hóa đơn...");
        }
    }

}

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
    public partial class frmFilter : Form
    {
        public frmFilter()
        {
            InitializeComponent();
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();

            if (cboFilterType.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn một tiêu chí để lọc!");
                return;
            }
            if (cboFilterType.SelectedIndex == 0)
            {
                dt.Columns.Add("Tên sản phẩm");
                dt.Columns.Add("Số lượng đã bán");
                dt.Rows.Add("Laptop Lenovo IdeaPad", "25");
                dt.Rows.Add("Chuột Logitech G304", "48");
                dt.Rows.Add("Bàn phím cơ Akko", "12");
            }
            else if (cboFilterType.SelectedIndex == 1)
            {
                dt.Columns.Add("Tên Đại lý");
                dt.Columns.Add("Các sản phẩm đã lấy");
                dt.Rows.Add("Đại lý Bảo Nhi", "Laptop, Chuột, Bàn phím");
                dt.Rows.Add("Đại lý Nghĩa", "Chuột, Bàn phím");
                dt.Rows.Add("Đại lý TDTU", "Laptop");
            }
            else
            {
                dt.Columns.Add("Tên Đại lý");
                dt.Columns.Add("Tổng tiền tích lũy");
                dt.Rows.Add("Đại lý Bảo Nhi", "150.000.000 VNĐ");
                dt.Rows.Add("Đại lý Như", "100.000.000 VNĐ");
                dt.Rows.Add("Đại lý Nghĩa", "75.000.000 VNĐ");
                dt.Rows.Add("Đại lý Tín", "55.000.000 VNĐ");
            }

            dgvFilterResult.DataSource = dt;
        }
        

        private void dgvFilterResult_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

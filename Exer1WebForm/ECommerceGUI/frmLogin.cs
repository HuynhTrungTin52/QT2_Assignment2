using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;

namespace ECommerceGUI
{
    public partial class frmLogin : Form
    {
        Database db = new Database();
        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string user = txtUserName.Text.Trim();
            string pass = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Nhập đầy đủ tài khoản và mật khẩu đã nhé!", "Thông báo");
                return;
            }

            try
            {
                string sql = $"SELECT * FROM Users WHERE UserName='{user}' AND Password='{pass}'";

                DataTable dt = db.GetDataTable(sql);

                if (dt != null && dt.Rows.Count > 0)
                {
                    string fullName = dt.Rows[0]["Email"].ToString().Split('-')[0].Trim();

                    MessageBox.Show($"Chào Admin {fullName}! Đăng nhập thành công rồi nha.", "Thành công");

                    this.Hide(); 
                    frmMain main = new frmMain();
                    main.Show();
                }
                else
                {
                    MessageBox.Show("Sai tài khoản hoặc mật khẩu.", "Lỗi đăng nhập");

                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
       
                MessageBox.Show("Connection Error: " + ex.Message, "Lỗi hệ thống");
            }
        }

    }
}

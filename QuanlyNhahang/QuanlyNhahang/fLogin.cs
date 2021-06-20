using QuanlyNhahang.DAO;
using QuanlyNhahang.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanlyNhahang
{
    public partial class fLogin : Form
    {
        public fLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txbUsername.Text;
            string password = txbPassword.Text;
            if (Login(username, password))
            {
                Account loginAccount = AccountDAO.Instance.GetAccountByUserName(username);
                fTableManager f = new fTableManager(loginAccount);
                this.Hide();
                f.ShowDialog();
                this.Show();
            }
            else
            {
                MessageBox.Show("Sai tên tài khoản hoặc mật khẩu!");
            }

        }

        bool Login(string username, string password)
        {
            return AccountDAO.Instance.Login(username, password);
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void fLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có thật sự muốn thoát chương trình?", "Thông báo", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
            {
                e.Cancel = true;
            }
        }

        private void chb_ShowPass_CheckedChanged(object sender, EventArgs e)
        {
            if (chb_ShowPass.Checked)
            {
                txbPassword.UseSystemPasswordChar = false;
            }
            else
            {
                txbPassword.UseSystemPasswordChar = true;
            }
        }

        //private void txbUsername_Enter(object sender, EventArgs e)
        //{
        //    if (txbUsername.Text == "Nhập tên đăng nhập")
        //    {
        //        txbUsername.Text = "";
        //        txbUsername.ForeColor = Color.Black;
        //    }
        //}

        //private void txbUsername_Leave(object sender, EventArgs e)
        //{
        //    if (txbUsername.Text == "")
        //    {
        //        txbUsername.Text = "Nhập tên đăng nhập";
        //        txbUsername.ForeColor = Color.LightGreen;
        //    }
        //}

        //private void txbPassword_Enter(object sender, EventArgs e)
        //{
        //    if (txbPassword.Text == "Nhập mật khẩu")
        //    {
        //        txbPassword.Text = "";
        //        txbPassword.ForeColor = Color.Black;
        //    }
        //}

        //private void txbPassword_Leave(object sender, EventArgs e)
        //{
        //    if (txbPassword.Text == "")
        //    {
        //        txbPassword.Text = "Nhập mật khẩu";
        //        txbPassword.ForeColor = Color.LightGreen;
        //    }
        //}

       
    }
}

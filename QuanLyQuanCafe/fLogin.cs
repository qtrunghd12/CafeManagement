using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
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
                Account loginAccount = AccountDAO.Instance.GetAccountByUsername(username);
                fTableManager fTableManager = new fTableManager(loginAccount);
                this.Hide();
                fTableManager.ShowDialog();
                this.Show();
            }
            else
            {
                MessageBox.Show("Sai tên tài khoản hoặc mật khẩu!");
            }
        }
        bool Login (string username, string password)
        {
            return AccountDAO.Instance.Login(username, password);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void fLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Thoát chương trình?", "Thông báo", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
            {
                e.Cancel = true;
            }
        }

        private void fLogin_Load(object sender, EventArgs e)
        {
            label5.Parent = pbBackground;
            label5.BackColor = Color.Transparent;
            label6.Parent = pbBackground;
            label6.BackColor = Color.Transparent;
            label7.Parent = pbBackground;
            label7.BackColor = Color.Transparent;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            fRegister fRegister = new fRegister();
            this.Hide();
            fRegister.ShowDialog();
        }
    }
}

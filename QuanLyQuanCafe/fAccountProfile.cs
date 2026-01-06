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
    public partial class fAccountProfile : Form
    {
        private Account loginAccount;
        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount); }
        }
        public fAccountProfile(Account account)
        {
            InitializeComponent();
            LoginAccount = account;
        }
        void ChangeAccount(Account account)
        {
            txbUsername.Text = LoginAccount.Username;
            txbDisplayName.Text = LoginAccount.DisplayName;
        }

        void UpdateAccountInfo()
        {
            string displayName = txbDisplayName.Text;
            string password = txbPassword.Text;
            string newPassword = txbNewPassword.Text;
            string passwordConfirm = txbPasswordConfirm.Text;

            if (!newPassword.Equals(passwordConfirm))
            {
                MessageBox.Show("Vui lòng nhập lại mật khẩu đúng với mật khẩu mới!");
            }
            else
            {
                if (AccountDAO.Instance.UpdateAccount(LoginAccount.Username, displayName, password, newPassword))
                {
                    MessageBox.Show("Cập nhật thành công!");
                    if (updateAccount != null)
                        updateAccount(this, new AccountEvent(AccountDAO.Instance.GetAccountByUsername(LoginAccount.Username)));
                }
                else
                {
                    MessageBox.Show("Vui lòng điền đúng mật khẩu!");
                }
            }
        }

        private event EventHandler<AccountEvent> updateAccount;
        public event EventHandler<AccountEvent> UpdateAccount
        {
            add { updateAccount += value; }
            remove { updateAccount -= value; }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateAccountInfo();
        }
    }

    public class AccountEvent : EventArgs
    {
        private Account account;

        public Account Account
        {
            get { return account; }
            set { account = value; }
        }

        public AccountEvent(Account account)
        {
            this.Account = account;
        }
    }
}

using QuanLyQuanCafe.DAO;
using System;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class fRegister : Form
    {
        public fRegister()
        {
            InitializeComponent();
        }

        private void fRegister_Load(object sender, EventArgs e)
        {
            label5.Parent = pbBackground;
            label5.BackColor = System.Drawing.Color.Transparent;
            label6.Parent = pbBackground;
            label6.BackColor = System.Drawing.Color.Transparent;
            label7.Parent = pbBackground;
            label7.BackColor = System.Drawing.Color.Transparent;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            fLogin loginForm = new fLogin();
        }

        private void fRegister_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Thoát chương trình?", "Thông báo", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
            {
                e.Cancel = true;
            }
        }

        // Register button click handler
        private void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txbUsername.Text.Trim();
            string password = txbPassword.Text;
            string passwordConfirm = txbPasswordConfirm.Text;
            string displayName = txbDisplayName.Text.Trim();

            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txbUsername.Focus();
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txbPassword.Focus();
                return;
            }

            if (password != passwordConfirm)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txbPasswordConfirm.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(displayName))
            {
                // fallback: use username as display name
                displayName = username;
            }

            int role = 0; // default user

            try
            {
                bool created = AccountDAO.Instance.RegisterAccount(username, password, displayName, role);
                if (created)
                {
                    MessageBox.Show("Đăng ký thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Tên đăng nhập đã tồn tại. Vui lòng chọn tên khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txbUsername.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đăng ký: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            fLogin fLogin = new fLogin();
            this.Hide();
            fLogin.ShowDialog();
        }
    }
}

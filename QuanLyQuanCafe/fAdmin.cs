using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;

namespace QuanLyQuanCafe
{
    public partial class fAdmin : Form
    {
        BindingSource foodList = new BindingSource();
        BindingSource categoryList = new BindingSource();
        BindingSource accountList = new BindingSource();

        public Account loginAccount;

        public fAdmin()
        {
            InitializeComponent();
            Load();
        }

        #region Methods
        void Load()
        {
            dgvFood.DataSource = foodList;
            dgvCategory.DataSource = categoryList;
            dgvAccount.DataSource = accountList;

            LoadRevenueByDate(DateTime.Now, DateTime.Now);
            LoadDateTimePicker();
            LoadFoodList();
            LoadCategoryList();
            AddFoodBinding();
            AddCategoryBinding();
            AddAccountBinding();
            LoadAccount();
            LoadCategoryIntoCombobox(cbCategory);
        }

        void AddAccountBinding()
        {
            txbUsername.DataBindings.Add(new Binding("Text", accountList, "Username", true, DataSourceUpdateMode.Never));
            txbDisplayName.DataBindings.Add(new Binding("Text", accountList, "DisplayName", true, DataSourceUpdateMode.Never));
            nmAccountRole.DataBindings.Add(new Binding("Value", accountList, "Role", true, DataSourceUpdateMode.Never));
        }

        void LoadDateTimePicker()
        {
            DateTime today = DateTime.Now;
            dtpFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpToDate.Value = dtpFromDate.Value.AddMonths(1).AddDays(-1);
        }
        void LoadRevenueByDate(DateTime checkIn, DateTime checkOut)
        {
            dgvRevenue.DataSource = BillDAO.Instance.GetBillListByDate(checkIn, checkOut);
        }

        void LoadFoodList()
        {
            foodList.DataSource = FoodDAO.Instance.GetFoodList();
        }

        void LoadCategoryList()
        {
            categoryList.DataSource = CategoryDAO.Instance.GetListCategory();
        }

        List<Food> SearchFoodByName(string name)
        {
            List<Food> listFood = FoodDAO.Instance.SearchFoodByName(name);

            return listFood;
        }

        List<Account> SearchAccountByUsername(string username)
        {
            List<Account> listAccount = AccountDAO.Instance.SearchAccountByUsername(username);
            return listAccount;
        }

        void LoadAccount()
        {
            accountList.DataSource = AccountDAO.Instance.GetListAccount();
        }

        void AddFoodBinding()
        {
            txbFoodName.DataBindings.Add(new Binding("Text", dgvFood.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txbFoodID.DataBindings.Add(new Binding("Text", dgvFood.DataSource, "ID", true, DataSourceUpdateMode.Never));
            nmFoodPrice.DataBindings.Add(new Binding("Value", dgvFood.DataSource, "Price", true, DataSourceUpdateMode.Never));
        }

        void AddCategoryBinding()
        {
            txbCategory.DataBindings.Add(new Binding("Text", dgvCategory.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txbCategoryID.DataBindings.Add(new Binding("Text", dgvCategory.DataSource, "ID", true, DataSourceUpdateMode.Never));
        }

        void LoadCategoryIntoCombobox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";
        }
        void AddAccount(string username, string displayName, int type)
        {
            try
            {
                username = username?.Trim();
                displayName = displayName?.Trim();

                if (string.IsNullOrWhiteSpace(username))
                {
                    MessageBox.Show("Tên đăng nhập không được rỗng");
                    return;
                }

                if (string.IsNullOrWhiteSpace(displayName))
                {
                    displayName = username;
                }

                var existingAccounts = AccountDAO.Instance.GetListAccount();
                bool exists = false;
                if (existingAccounts != null)
                {
                    foreach (DataRow row in existingAccounts.Rows)
                    {
                        if (string.Equals(row["Username"].ToString(), username, StringComparison.OrdinalIgnoreCase))
                        {
                            exists = true;
                            break;
                        }
                    }
                }

                if (exists)
                {
                    MessageBox.Show("Tài khoản đã tồn tại");
                    return;
                }

                if (AccountDAO.Instance.InsertAccount(username, displayName, type))
                {
                    MessageBox.Show("Thêm tài khoản thành công");
                }
                else
                {
                    MessageBox.Show("Thêm tài khoản thất bại");
                }

                LoadAccount();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi thêm tài khoản: " + ex.Message);
            }
        }

        void EditAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.UpdateAccount(userName, displayName, type))
            {
                MessageBox.Show("Cập nhật tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Cập nhật tài khoản thất bại");
            }

            LoadAccount();
        }

        void DeleteAccount(string userName)
        {
            if (loginAccount.Username.Equals(userName))
            {
                MessageBox.Show("Vui lòng đừng xóa chính tài khoản của bạn");
                return;
            }
            if (AccountDAO.Instance.DeleteAccount(userName))
            {
                MessageBox.Show("Xóa tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Xóa tài khoản thất bại");
            }

            LoadAccount();
        }

        void ResetPass(string userName)
        {
            if (AccountDAO.Instance.ResetPassword(userName))
            {
                MessageBox.Show("Đặt lại mật khẩu thành công");
            }
            else
            {
                MessageBox.Show("Đặt lại mật khẩu thất bại");
            }
        }


        #endregion

        #region Events

        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUsername.Text;
            string displayName = txbDisplayName.Text;
            int type = (int)nmAccountRole.Value;

            AddAccount(userName, displayName, type);
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUsername.Text;

            DeleteAccount(userName);
        }

        private void btnEditAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUsername.Text;
            string displayName = txbDisplayName.Text;
            int type = (int)nmAccountRole.Value;

            EditAccount(userName, displayName, type);
        }


        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            string userName = txbUsername.Text;

            ResetPass(userName);
        }

        private void btnShowAccount_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }

        private void btnRevenue_Click(object sender, EventArgs e)
        {
            LoadRevenueByDate(dtpFromDate.Value, dtpToDate.Value);
        }

        private void btnShowFood_Click(object sender, EventArgs e)
        {
            LoadFoodList();
        }

        private void btnShowCategory_Click(object sender, EventArgs e)
        {
            LoadCategoryList();
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int categoryID = (cbCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;

            if (FoodDAO.Instance.InsertFood(name, categoryID, price))
            {
                MessageBox.Show("Thêm món thành công");
                LoadFoodList();
                if (insertFood != null)
                    insertFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm thức ăn");
            }
        }

        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }

        private void btnEditFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int categoryID = (cbCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;
            int id = Convert.ToInt32(txbFoodID.Text);

            if (FoodDAO.Instance.UpdateFood(id, name, categoryID, price))
            {
                MessageBox.Show("Sửa món thành công");
                LoadFoodList();
                if (updateFood != null)
                    updateFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa thức ăn");
            }
        }
        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }

        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbFoodID.Text);

            if (FoodDAO.Instance.DeleteFood(id))
            {
                MessageBox.Show("Xóa món thành công");
                LoadFoodList();
                if (deleteFood != null)
                    deleteFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa thức ăn");
            }
        }
        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }

        // Category event handlers
        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            string name = txbCategory.Text?.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Tên danh mục không được rỗng");
                return;
            }

            if (CategoryDAO.Instance.InsertCategory(name))
            {
                MessageBox.Show("Thêm danh mục thành công");
                LoadCategoryList();
                LoadCategoryIntoCombobox(cbCategory);
                if (insertCategory != null) insertCategory(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm danh mục");
            }
        }
        private event EventHandler insertCategory;
        public event EventHandler InsertCategory
        {
            add { insertCategory += value; }
            remove { insertCategory -= value; }
        }

        private void btnEditCategory_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txbCategoryID.Text, out int id))
            {
                MessageBox.Show("ID danh mục không hợp lệ");
                return;
            }
            string name = txbCategory.Text?.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Tên danh mục không được rỗng");
                return;
            }

            if (CategoryDAO.Instance.UpdateCategory(id, name))
            {
                MessageBox.Show("Sửa danh mục thành công");
                LoadCategoryList();
                LoadCategoryIntoCombobox(cbCategory);
                if (updateCategory != null) updateCategory(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa danh mục");
            }
        }
        private event EventHandler updateCategory;
        public event EventHandler UpdateCategory
        {
            add { updateCategory += value; }
            remove { updateCategory -= value; }
        }

        private void btnDeleteCategory_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txbCategoryID.Text, out int id))
            {
                MessageBox.Show("ID danh mục không hợp lệ");
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa danh mục này?", "Xác nhận", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (CategoryDAO.Instance.DeleteCategory(id))
                {
                    MessageBox.Show("Xóa danh mục thành công");
                    LoadCategoryList();
                    LoadCategoryIntoCombobox(cbCategory);
                    if (deleteCategory != null) deleteCategory(this, new EventArgs());
                }
                else
                {
                    MessageBox.Show("Có lỗi khi xóa danh mục (có thể còn món thuộc danh mục này)");
                }
            }
        }
        private event EventHandler deleteCategory;
        public event EventHandler DeleteCategory
        {
            add { deleteCategory += value; }
            remove { deleteCategory -= value; }
        }
        private void btnSearchFood_Click(object sender, EventArgs e)
        {
            foodList.DataSource = SearchFoodByName(txbFoodSearch.Text);
        }

        private void btnSearchAccount_Click(object sender, EventArgs e)
        {
            accountList.DataSource = SearchAccountByUsername(txbAccountSearch.Text);
        }

        #endregion

        private void txbFoodID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvFood.SelectedCells.Count > 0)
                {
                    int id = (int)dgvFood.SelectedCells[0].OwningRow.Cells["CategoryID"].Value;

                    Category cateogory = CategoryDAO.Instance.GetCategoryByID(id);

                    cbCategory.SelectedItem = cateogory;

                    int index = -1;
                    int i = 0;
                    foreach (Category item in cbCategory.Items)
                    {
                        if (item.ID == cateogory.ID)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }

                    cbCategory.SelectedIndex = index;
                }
            }
            catch { }
        }

       
    }
}

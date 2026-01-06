using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Menu = QuanLyQuanCafe.DTO.Menu;

namespace QuanLyQuanCafe
{
    public partial class fTableManager : Form
    {
        private Account loginAccount;
        public Account LoginAccount 
        {
            get { return loginAccount; }
            set { loginAccount = value; ShowAccountUser(loginAccount.Role, loginAccount.DisplayName); }
        }

        public fTableManager(Account account)
        {
            InitializeComponent();
            this.LoginAccount = account;
            LoadTable();
            LoadCategory();
        }

        #region Methods
        void ShowAccountUser(int role, string DisplayName)
        {
            adminToolStripMenuItem.Enabled = role != 0;
            txbHello.Text = "Chào mừng, " + DisplayName + " !";
        }
        void LoadCategory()
        {
            List<Category> listCategory = CategoryDAO.Instance.GetListCategory();
            cbCategory.DataSource = listCategory;
            cbCategory.DisplayMember = "Name";
        }
        void LoadFoodListByCategoryID(int id)
        {
            List<Food> listFood = FoodDAO.Instance.GetFoodByCategoryID(id);
            cbFood.DataSource = listFood;
            cbFood.DisplayMember = "Name";
        }
        void LoadTable()
        {
            flpTable.Controls.Clear();
            List<Table> tableList = TableDAO.Instance.LoadTableList();
            foreach (Table item in tableList)
            {
                Button btn = new Button() { Width = TableDAO.TableWidth, Height = TableDAO.TableHeight };
                btn.Text = item.Name + Environment.NewLine + item.Status;
                btn.Click += btn_Click;
                btn.Tag = item;
                flpTable.Controls.Add(btn);
                ApplyButtonStyle(btn, item.Status);
            }
        }

        void UpdateTableButton(int tableID, string newStatus)
        {
            var btn = flpTable.Controls.OfType<Button>().FirstOrDefault(b => (b.Tag as Table)?.ID == tableID);
            if (btn == null) return;

            var table = btn.Tag as Table;
            if (table == null) return;

            table.Status = newStatus;
            btn.Text = table.Name + Environment.NewLine + table.Status;
            ApplyButtonStyle(btn, table.Status);
        }

        void ApplyButtonStyle(Button btn, string status)
        {
            switch (status)
            {
                case "Trống":
                    btn.BackColor = Color.Beige;
                    btn.ForeColor = Color.Black;
                    btn.Font = new Font("Arial", 12.0f, FontStyle.Bold);
                    break;
                default:
                    btn.BackColor = Color.DarkOliveGreen;
                    btn.ForeColor = Color.White;
                    btn.Font = new Font("Arial", 12.0f, FontStyle.Bold);
                    break;
            }
        }

        void showBill(int id)
        {
            lsvBill.Items.Clear();
            List<Menu> listBillInfo = MenuDAO.Instance.GetListMenuByTable(id);
            CultureInfo culture = new CultureInfo("vi-VN");
            Thread.CurrentThread.CurrentCulture = culture;
            float totalPrice = 0;
            foreach (Menu item in listBillInfo)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Quantity.ToString());
                lsvItem.SubItems.Add(item.Price.ToString("c"));
                lsvItem.SubItems.Add(item.TotalPrice.ToString("c"));
                totalPrice += item.TotalPrice;
                lsvBill.Items.Add(lsvItem);
            }

            txbTotalPrice.Text = totalPrice.ToString("c");
        }
        #endregion

        #region Events
        void btn_Click(object sender, EventArgs e)
        {
            Table table = (sender as Button)?.Tag as Table;
            if (table == null)
                return;
            txbTable.Text = table.Name;
            lsvBill.Tag = table;
            showBill(table.ID);
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAccountProfile f = new fAccountProfile(LoginAccount);
            f.UpdateAccount += f_UpdateAccount;
            f.ShowDialog();
        }

        private void f_UpdateAccount(object sender, AccountEvent e)
        {
            txbHello.Text = "Chào mừng, " + e.Account.DisplayName + " !";
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin f = new fAdmin();
            f.loginAccount = LoginAccount;
            f.InsertFood += F_InsertFood;
            f.UpdateFood += F_UpdateFood;
            f.DeleteFood += F_DeleteFood;
            f.ShowDialog();
        }

        private void F_DeleteFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                showBill((lsvBill.Tag as Table).ID);
            LoadTable();
        }

        private void F_UpdateFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                showBill((lsvBill.Tag as Table).ID);
        }

        private void F_InsertFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                showBill((lsvBill.Tag as Table).ID);
        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;
            ComboBox cb = sender as ComboBox;
            if (cb.SelectedItem == null)
                return;
            Category selected = cb.SelectedItem as Category;
            id = selected.ID;
            LoadFoodListByCategoryID(id);
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            if (table == null)
            {
                MessageBox.Show("Hãy chọn bàn trước khi thêm món");
                return;
            }
            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            int idFood = (cbFood.SelectedItem as Food).ID;
            int quantity = (int)nmFoodQuantity.Value;

            if (quantity == 0)
            {
                MessageBox.Show("Số lượng phải khác 0.");
                return;
            }

            if (quantity < 0)
            {
                // Kiểm tra món hiện có số lượng bao nhiêu
                int currentQty = MenuDAO.Instance.GetFoodQuantityInBill(idBill, idFood);

                if (currentQty + quantity < 0) // VD: hiện có 1 món, quantity = -2 → còn -1 (sai)
                {
                    MessageBox.Show("Không thể giảm món dưới 0!");
                    return;
                }
            }

            if (idBill == -1) // Nếu chưa có hóa đơn cho bàn này
            {
                BillDAO.Instance.InsertBill(table.ID);
                BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxIDBill(), idFood, quantity);
            }
            else // Nếu đã có hóa đơn rồi
            {
                BillInfoDAO.Instance.InsertBillInfo(idBill, idFood, quantity);
            }
            showBill(table.ID);

            UpdateTableButton(table.ID, "Có người");
        }

        #endregion

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            if (table == null) return;

            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            // Parse chính xác theo format tiền Việt Nam
            string rawText = txbTotalPrice.Text.Replace("₫", "").Trim();
            double totalPrice = double.Parse(rawText, NumberStyles.Any, new CultureInfo("vi-VN"));

            if (idBill != -1)
            {
                if (MessageBox.Show(string.Format("Bạn có chắc thanh toán hóa đơn cho {0}\nTổng tiền - {1}", table.Name, txbTotalPrice.Text), "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    BillDAO.Instance.CheckOut(idBill, (float)totalPrice);
                    showBill(table.ID);

                    // Update only the affected table button to empty (avoid reloading all)
                    UpdateTableButton(table.ID, "Trống");
                }
            }
        }
    }
}
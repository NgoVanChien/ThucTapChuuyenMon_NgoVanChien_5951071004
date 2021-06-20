using QuanlyNhahang.DAO;
using QuanlyNhahang.DTO;
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
using System.Security.Cryptography;
using Microsoft.Reporting.WinForms;

namespace QuanlyNhahang
{
    public partial class fAdmin : Form
    {
        BindingSource foodList = new BindingSource();
        BindingSource categorylist = new BindingSource();
        BindingSource tableList = new BindingSource();
        BindingSource accountList = new BindingSource();

        public Account loginAccount;
        public fAdmin()
        {
            InitializeComponent();
            Load();
        }

       new void Load()
        {
            dtgvFood.DataSource = foodList;
            dtgvCategory.DataSource = categorylist;
            dtgvTableFood.DataSource = tableList;
            dtgvAccount.DataSource = accountList;

            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
            LoadDateTimePickerBill();
            LoadListFood();
            LoadCategory();
            LoadTable();
            LoadAccount();

            LoadCategoryIntoCombobox(cbFoodCategory);
            AddFoodBindding();
            AddCategoryBinding();
            AddTableBinding();
            AddAccountBinding();
            LoadStatusIntoCombobox(cbTableStatus);
        }


        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            dtgvBill.DataSource = BillDAO.Instace.GetBillListByDate(checkIn, checkOut);
        }
        List<Food> SearchFoodByName(string name)
        {
            List<Food> listFood = FoodDAO.Instace.SearchFoodByName(name);
            return listFood;
        }      

        void AddAccountBinding()
        {
            txbUserName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "UserName", true, DataSourceUpdateMode.Never));
            txbDisplayName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));
            nmUpdownAccountType.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "Type", true, DataSourceUpdateMode.Never));
        }

        void LoadAccount()
        {
            accountList.DataSource = AccountDAO.Instance.GetListAccount();
        }

        void AddFoodBindding()
        {
            txbFoodName.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txbFoodID.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "ID", true, DataSourceUpdateMode.Never));
            nmFoodPrice.DataBindings.Add(new Binding("Value", dtgvFood.DataSource, "price", true, DataSourceUpdateMode.Never));
        }
        void LoadCategoryIntoCombobox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instace.GetListCategory();
            cb.DisplayMember = "Name";
        }
        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpkFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpkToDate.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1);

        }
        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }

        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }

        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }
        void LoadListFood()
        {
            foodList.DataSource = FoodDAO.Instace.GetListFood();
        }
        void LoadCategory()
        {
            categorylist.DataSource = CategoryDAO.Instace.GetListCategory();
        }

        void AddCategoryBinding()
        {
            txbCategoryID.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "ID", true, DataSourceUpdateMode.Never));
            txbCategoryName.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "Name", true, DataSourceUpdateMode.Never));
        }

        void LoadTable()
        {
            tableList.DataSource = TableDAO.Instance.GetTableFoodList();
        }
        void AddTableBinding()
        {
            txbTableID.DataBindings.Add(new Binding("Text", dtgvTableFood.DataSource, "ID", true, DataSourceUpdateMode.Never));
            txbTableName.DataBindings.Add(new Binding("Text", dtgvTableFood.DataSource, "Name", true, DataSourceUpdateMode.Never));
            cbTableStatus.DataBindings.Add(new Binding("Text", dtgvTableFood.DataSource, "Status", true, DataSourceUpdateMode.Never));
        }
        void LoadStatusIntoCombobox(ComboBox b)
        {
            b.DataSource = TableDAO.Instance.GetTableFoodList();
            b.DisplayMember = "status";
        }
        void AddAccount(string username, string displayname, int type)
        {
            if(AccountDAO.Instance.InsertAccount(username, displayname,  type))
            {
                MessageBox.Show("Bạn đã thêm tài khoản thành công ");
            }
            else
            {
                MessageBox.Show("Thêm tài khoản thất bại");
            }
            LoadAccount();
        }

        void DeleteAccount(string username)
        {
            if (loginAccount.UserName.Equals(username))
            {
                MessageBox.Show("Vui lòng đừng xóa tài khoản của bạn chứ !");
                return;
            }
            if (AccountDAO.Instance.DeleteAccount(username  ))
            {
                MessageBox.Show(" Bạn đã xóa tài khoản thành công !");
            }
            else
            {
                MessageBox.Show("Xóa tài khoản thất bại !");
            }
            LoadAccount();
        }
        void EditAccount(string username, string displayname, int type)
        {
            if (AccountDAO.Instance.EditAccount(username, displayname, type))
            {
                MessageBox.Show(" Bạn đã cập nhật tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Cập nhật tài khoản thất bại");
            }
            LoadAccount();
        }

        void ResetPassword(string username)
        {
            string userName = txbUserName.Text;
            if (AccountDAO.Instance.ResetPassword(userName))
            {
                MessageBox.Show("Bạn đã làm mới mật khẩu thành công");
            }
            else
            {
                MessageBox.Show("Làm mới mật khẩu thất bại");
            }
        }


        private void btnViewbill_Click_1(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
        }

        private void btnShowFood_Click_1(object sender, EventArgs e)
        {
            LoadListFood();
        }

        
        // Thêm món 
        private void btnAddFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int categoryID = (cbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;

            if (FoodDAO.Instace.InsertFood(name, categoryID, price))
            {
                MessageBox.Show("Thêm món thành công");
                LoadListFood();
                if (insertFood != null)
                    insertFood(this, new EventArgs());

            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm món");
            }
        }
        // Xóa món
        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbFoodID.Text);

            if (FoodDAO.Instace.DeletetFood(id))
            {
                MessageBox.Show("Xóa món thành công");
                LoadListFood();
                if (deleteFood != null)
                    deleteFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa món");
            }
        }
        // Sửa món
        private void btnEditFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int categoryID = (cbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;
            int id = Convert.ToInt32(txbFoodID.Text);

            if (FoodDAO.Instace.UpdatetFood(id, name, categoryID, price))
            {
                MessageBox.Show("Sửa món thành công");
                LoadListFood();
                if (updateFood != null)
                    updateFood(this, new EventArgs());

            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa món ");
            }
        }
        private void txbFoodID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtgvFood.SelectedCells.Count > 0)
                {
                    int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["CategoryID"].Value;
                    Category category = CategoryDAO.Instace.GetCategoryById(id);
                    cbFoodCategory.SelectedItem = category;
                    int index = -1;
                    int i = 0;
                    foreach (Category item in cbFoodCategory.Items)
                    {
                        if (item.ID == category.ID)
                        {
                            index = i;
                            break;
                        }
                        i++;

                    }
                    cbFoodCategory.SelectedIndex = index;
                }


            }
            catch { }
        }


        private void btnSearchFood_Click_1(object sender, EventArgs e)
        {
            foodList.DataSource = SearchFoodByName(txbSearchFoodName.Text);
        }

        private void btnShowAccount_Click_1(object sender, EventArgs e)
        {
            LoadAccount();
        }
        // Thêm tài khoản
        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            string username = txbUserName.Text;
            string displayname = txbDisplayName.Text;
            int type = (int)nmUpdownAccountType.Value;

            AddAccount(username, displayname, type);
        }
        //  Xóa tài khoản
        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            string username = txbUserName.Text;

            DeleteAccount(username);
        }
        // Sửa  tài khoản
        private void btnEditAccount_Click(object sender, EventArgs e)
        {
            string username = txbUserName.Text;
            string displayname = txbDisplayName.Text;
            int type = (int)nmUpdownAccountType.Value;

            EditAccount(username, displayname, type);
        }

        // Làm mới mật khẩu
        private void btnResetPassWord_Click(object sender, EventArgs e)
        {
            string username = txbUserName.Text;

            ResetPassword(username);
        }

        private void btnShowCategory_Click(object sender, EventArgs e)
        {
            LoadCategory();
        }

        // Thêm danh mục
        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            string name = txbCategoryName.Text;


            if (CategoryDAO.Instace.InsertCategory(name))
            {
                MessageBox.Show("Thêm danh mục thành công");
                LoadCategory();

            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm danh mục");
            }
        }

        // Xóa  danh mục
        private void btnDeleteCategory_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbCategoryID.Text);

            if (CategoryDAO.Instace.DeleteCategory(id))
            {
                MessageBox.Show("Xóa danh mục thành công");
                LoadCategory();

            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa danh mục ");
            }
        }
        // Sửa  danh mục
        private void btnEditCategory_Click(object sender, EventArgs e)
        {
            string name = txbCategoryName.Text;

            int id = Convert.ToInt32(txbCategoryID.Text);

            if (CategoryDAO.Instace.UpdateCategory(name, id))
            {
                MessageBox.Show("Sửa danh mục thành công");
                LoadCategory();

            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa danh mục món ăn");
            }
        }

        private void btnShowTable_Click(object sender, EventArgs e)
        {
            LoadTable();
        }

        // Thêm Bàn ăn
        private void btnAddTable_Click(object sender, EventArgs e)
        {
            string name = txbTableName.Text;


            if (TableDAO.Instance.InsertTable(name))
            {
                MessageBox.Show("Thêm bàn ăn thành công");
                LoadTable();

            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm bàn ăn");
            }
        }

        // Xóa Bàn ăn
        private void btnDeleteTable_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbTableID.Text);

            if (TableDAO.Instance.DeleteTable(id))
            {
                MessageBox.Show("Xóa bàn ăn thành công");
                LoadTable();

            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa bàn ăn");
            }
        }

        // Sửa  Bàn ăn
        private void btnEditTable_Click(object sender, EventArgs e)
        {
            string name = txbTableName.Text;

            int id = Convert.ToInt32(txbTableID.Text);

            if (TableDAO.Instance.UpdateTable(name, id))
            {
                MessageBox.Show("Sửa bàn ăn thành công");
                LoadTable();

            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa bàn ăn");
            }
        }

        private void btnFirstBillPage_Click_1(object sender, EventArgs e)
        {
            txbPageBill.Text = "1";
        }

        private void btnPreviousBillPage1_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txbPageBill.Text);
            if (page > 1)
                page--;
            txbPageBill.Text = page.ToString();
        }

        private void txbPageBill_TextChanged_1(object sender, EventArgs e)
        {
            dtgvBill.DataSource = BillDAO.Instace.GetBillListByDateAndPage(dtpkFromDate.Value, dtpkToDate.Value, Convert.ToInt32(txbPageBill.Text));
        }

        private void btnnNextBillPage_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txbPageBill.Text);
            int sumRecord = BillDAO.Instace.GetNumBillListByDate(dtpkFromDate.Value, dtpkToDate.Value);

            if (page < sumRecord)
                page++;
            txbPageBill.Text = page.ToString();
        }
        private void btnLastBillPage_Click_1(object sender, EventArgs e)
        {
                int sumRecord = BillDAO.Instace.GetNumBillListByDate(dtpkFromDate.Value, dtpkToDate.Value);
                int lastPage = sumRecord / 10;
                if (sumRecord % 10 != 0)
                    lastPage++;
                txbPageBill.Text = lastPage.ToString();
        }

        MD5 md = MD5.Create();
        private void btnMahoa_Click(object sender, EventArgs e)
        {
            byte[] inputStr = System.Text.Encoding.ASCII.GetBytes(txbMahoa.Text);
            byte[] hash = md.ComputeHash(inputStr);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            lbMahoa.Text = sb.ToString();
        }

        private void txbMahoa_Enter(object sender, EventArgs e)
        {
            if (txbMahoa.Text == "Nhập mật khẩu cần mã hóa")
            {
                txbMahoa.Text = "";
                txbMahoa.ForeColor = Color.Black;
            }
        }

        private void txbMahoa_Leave(object sender, EventArgs e)
        {
            if (txbMahoa.Text == "")
            {
                txbMahoa.Text = "Nhập mật khẩu cần mã hóa";
                txbMahoa.ForeColor = Color.LightGreen;
            }
        }

        private void fAdmin_Load(object sender, EventArgs e)
        {

            this.rpViewer.RefreshReport();
        }

        private void fAdmin_Load_1(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
            this.reportViewer2.RefreshReport();
        }

        private void reportViewer2_Load(object sender, EventArgs e)
        {
            DataTable data = Dataprovider.Instance.ExcuteQuery("SELECT * FROM Bill");
            ReportDataSource rds = new ReportDataSource("BillReport", data);
            this.reportViewer2.LocalReport.DataSources.Clear();
            this.reportViewer2.LocalReport.DataSources.Add(rds);
            this.reportViewer2.RefreshReport();
        }

        private void fAdmin_Load_2(object sender, EventArgs e)
        {
        
           
        }

        private void reportViewer3_Load(object sender, EventArgs e)
        {
            DataTable data = Dataprovider.Instance.ExcuteQuery("SELECT * FROM Bill");
            ReportDataSource rds = new ReportDataSource("DataSet2", data);
            this.reportViewer3.LocalReport.DataSources.Clear();
            this.reportViewer3.LocalReport.DataSources.Add(rds);
            this.reportViewer3.RefreshReport();
        }

        private void fAdmin_Load_3(object sender, EventArgs e)
        {

        }

       
    }
    }


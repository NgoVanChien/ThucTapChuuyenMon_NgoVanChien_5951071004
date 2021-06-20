using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanlyNhahang.DTO;

namespace QuanlyNhahang.DAO
{
    public class CategoryDAO
    {
        private static CategoryDAO instace;

        public static CategoryDAO Instace
        {
            get { if (instace == null) instace = new CategoryDAO(); return CategoryDAO.instace; }

            private set { CategoryDAO.instace = value; }

        }
        private CategoryDAO() { }

        public List<Category> GetListCategory()
        {
            List<Category> list = new List<Category>();

            string query = "SELECT * FROM FoodCategory";

            DataTable data = Dataprovider.Instance.ExcuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Category category = new Category(item);
                list.Add(category);
            }

            return list;
        }
        public Category GetCategoryById(int id)
        {
            Category category = null;
            string query = "SELECT * FROM FoodCategory WHERE id = " + id;

            DataTable data = Dataprovider.Instance.ExcuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                category = new Category(item);
                return category;
            }
            return category;

        }
        // thêm danh mục
        public bool InsertCategory(string name)
        {
            string query = string.Format("INSERT FoodCategory ( name ) VALUES  ( N'{0}')", name);
            int result = Dataprovider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        // sửa danh mục
        public bool UpdateCategory(string name, int id)
        {
            string query = string.Format("UPDATE FoodCategory SET name = N'{0}' WHERE id = {1}", name, id);
            int result = Dataprovider.Instance.ExecuteNonQuery(query);

            return result > 0;

        }
        // xóa danh mục
        public bool DeleteCategory(int id)
        {
            //BillinfoDAO.Instance.DeleteBillInfoByFoodID(idFood);

            string query = string.Format("DELETE FoodCategory WHERE id = {0}", id);
            int result = Dataprovider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
    }
}

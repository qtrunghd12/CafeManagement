using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyQuanCafe.DTO;

namespace QuanLyQuanCafe.DAO
{
    public class CategoryDAO
    {
        private static CategoryDAO instance;
        public static CategoryDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new CategoryDAO();
                return instance;
            }
            private set { instance = value; }
        }
        private CategoryDAO() { }

        public List<Category> GetListCategory()
        {
            List<Category> list = new List<Category>();
            string query = "SELECT * FROM FoodCategory";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                Category category = new Category(item);
                list.Add(category);
            }
            return list;
        }

        public Category GetCategoryByID(int id)
        {
            Category category = null;

            string query = "select * from FoodCategory where id = " + id;

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                category = new Category(item);
                return category;
            }

            return category;
        }

        // New CRUD methods
        public bool InsertCategory(string name)
        {
            try
            {
                string query = "INSERT INTO dbo.FoodCategory ( name ) VALUES ( @name )";
                int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { name });
                return result > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateCategory(int id, string name)
        {
            try
            {
                string query = "UPDATE dbo.FoodCategory SET name = @name WHERE id = @id";
                int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { name, id });
                return result > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteCategory(int id)
        {
            try
            {
                string query = "DELETE FROM dbo.FoodCategory WHERE id = @id";
                int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { id });
                return result > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}

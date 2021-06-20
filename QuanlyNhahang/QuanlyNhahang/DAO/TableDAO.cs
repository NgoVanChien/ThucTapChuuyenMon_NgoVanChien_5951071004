using QuanlyNhahang.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanlyNhahang.DAO
{
    public class TableDAO
    {
        private static TableDAO instance;

        public static TableDAO Instance
        {
            get { if (instance == null) instance = new TableDAO(); return TableDAO.instance; }
            private set { TableDAO.instance = value; }
        }
        public static int TableWidth = 90;
        public static int TableHeight = 90;
        private TableDAO() { }

        public void SwitchTable(int id1, int id2)
        {
            Dataprovider.Instance.ExcuteQuery("USP_SwitchTable @idTable1 , @idTable2", new object[] { id1, id2 });
        }

        public List<Table> LoadTableList()
        {

            List<Table> tableList = new List<Table>();
            DataTable data = Dataprovider.Instance.ExcuteQuery("EXEC USP_GetTableList");
            foreach (DataRow item in data.Rows)
            {
                Table table = new Table(item);
                tableList.Add(table);

            }
            return tableList;
        }

        public List<Table> GetTableFoodList()
        {
            List<Table> tableList = new List<Table>();
            DataTable data = Dataprovider.Instance.ExcuteQuery("select * from TableFood");
            foreach (DataRow item in data.Rows)
            {
                Table table = new Table(item);
                tableList.Add(table);
            }
            return tableList;
        }
        public bool InsertTable(string name)
        {
            string query = string.Format("INSERT TableFood ( name , status ) VALUES  ( N'{0}' , N'Trống')", name);
            int result = Dataprovider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        public bool UpdateTable(string name, int id)
        {
            string query = string.Format("UPDATE TableFood SET name = N'{0}' WHERE id = {1}", name, id);
            int result = Dataprovider.Instance.ExecuteNonQuery(query);

            return result > 0;

        }

        public bool DeleteTable(int id)
        {

            string query = string.Format("Delete TableFood WHERE id = {0}", id);
            int result = Dataprovider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
    }
}

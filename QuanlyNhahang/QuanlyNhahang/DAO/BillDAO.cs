using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanlyNhahang.DTO;

namespace QuanlyNhahang.DAO
{
    public class BillDAO
    {
        private static BillDAO instace;

        public static BillDAO Instace
        {
            get { if (instace == null) instace = new BillDAO(); return BillDAO.instace; }
            private set { instace = value; }
        }
        private BillDAO() { }
        public int GetUncheckBillIDByTableID(int id)
        {
            DataTable data = Dataprovider.Instance.ExcuteQuery("SELECT * FROM Bill WHERE idTable=" + id + " AND status = 0");
            if (data.Rows.Count > 0)
            {
                Bill bill = new Bill(data.Rows[0]);

                return bill.ID;

            }
            return -1;
        }
      
        public void CheckOut(int id, int discount, float totalPrice)
        {
            string query = "UPDATE Bill SET dateCheckOut = GETDATE(), status = 1," + "discount = " + discount + ", totalPrice = " + totalPrice + " WHERE id = " + id;
            Dataprovider.Instance.ExecuteNonQuery(query);
        }
        public void InsertBill(int id)
        {
            Dataprovider.Instance.ExcuteQuery("exec USP_InsertBill @idTable", new object[] { id });
        }

        public DataTable GetBillListByDate(DateTime checkIn, DateTime checkOut)
        {
            return Dataprovider.Instance.ExcuteQuery("exec USP_GetListBillByDate @checkIn , @checkOut", new object[] { checkIn, checkOut });
        }
        public DataTable GetBillListByDateAndPage(DateTime checkIn, DateTime checkOut, int pageNum)
        {
            return Dataprovider.Instance.ExcuteQuery("exec USP_GetListBillByDateAndPage @checkIn , @checkOut , @page", new object[] { checkIn, checkOut, pageNum });
        }

        public int GetNumBillListByDate(DateTime checkIn, DateTime checkOut)
        {
            return (int)Dataprovider.Instance.ExecuteScalar("exec USP_GetNumBillByDate @checkIn , @checkOut", new object[] { checkIn, checkOut });
        }

        public int GetMaxIDBill()
        {
            try
            {
                return (int)Dataprovider.Instance.ExecuteScalar("SELECT MAX(id) FROM Bill");
            }
            catch
            {
                return 1;
            }
        }




    }
}


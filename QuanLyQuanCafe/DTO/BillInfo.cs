using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class BillInfo
    {
        public BillInfo(int id, int billID, int foodID, int quantity)
        {
            this.ID = id;
            this.IdBill = billID;
            this.IdFood = foodID;
            this.Quantity = quantity;
        }
        public BillInfo(DataRow row) 
        {
            this.ID = (int)row["id"];
            this.IdBill = (int)row["idBill"];
            this.IdFood = (int)row["idFood"];
            this.Quantity = (int)row["quantity"];
        }

        private int iD;
        public int ID { get => iD; set => iD = value; }
        private int idBill;
        public int IdBill { get => idBill; set => idBill = value; }
        private int idFood;
        public int IdFood { get => idFood; set => idFood = value; }
        private int quantity;
        public int Quantity { get => quantity; set => quantity = value; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class Menu
    {
        private string foodName;
        public string FoodName { get => foodName; set => foodName = value; }
        private int quantity;
        public int Quantity { get => quantity; set => quantity = value; }
        private float price;
        public float Price { get => price; set => price = value; }
        private float totalPrice;
        public float TotalPrice { get => totalPrice; set => totalPrice = value; }

        
        public Menu() { }
        public Menu(string foodName, int quantity, float price, float totalPrice = 0)
        {
            this.FoodName = foodName;
            this.Quantity = quantity;
            this.Price = price;
            this.TotalPrice = totalPrice;
        }
        public Menu(System.Data.DataRow row)
        {
            this.FoodName = row["name"].ToString();
            this.Quantity = (int)row["quantity"];
            this.Price = (float)Convert.ToDouble(row["price"].ToString());
            this.TotalPrice = (float)Convert.ToDouble(row["totalPrice"].ToString());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class Bill
    {    
        public Bill(int id, int status, DateTime? dateCheckIn, DateTime? dateCheckOut)
        {
            this.ID = id;
            this.Status = status;
            this.DateCheckIn = dateCheckIn;
            this.DateCheckOut = dateCheckOut;
        }

        public Bill(DataRow row) 
        { 
            this.ID = (int)row["id"];
            this.Status = (int)row["status"];
            var dateCheckInTemp = row["dateCheckIn"];
            if (dateCheckInTemp.ToString() != "")
                this.DateCheckIn = (DateTime?)dateCheckInTemp;
            var dateCheckOutTemp = row["dateCheckOut"];
            if (dateCheckOutTemp.ToString() != "")
                this.DateCheckOut = (DateTime?)dateCheckOutTemp;
        }

        private int iD;
        public int ID 
        { 
            get => iD; 
            set => iD = value; 
        }
        private int status;
        public int Status 
        {
            get => status;
            set => status = value; 
        }
        private DateTime? dateCheckIn;
        public DateTime? DateCheckIn 
        { 
            get => dateCheckIn; 
            set => dateCheckIn = value; 
        }
        private DateTime? dateCheckOut;
        public DateTime? DateCheckOut 
        { 
            get => dateCheckOut; 
            set => dateCheckOut = value; 
        }
    }
}

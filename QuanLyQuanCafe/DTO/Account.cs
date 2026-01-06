using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class Account
    {
        private string username;
        private string displayName;
        private int role;
        private string password;

        public string Username { get => username; set => username = value; }
        public string DisplayName { get => displayName; set => displayName = value; }
        public int Role { get => role; set => role = value; }
        public string Password { get => password; set => password = value; }

        public Account(string username, string displayName, int role, string password = null)
        {
            this.Username = username;
            this.DisplayName = displayName;
            this.Role = role;
            this.Password = password;
        }
        public Account(DataRow row) 
        {
            this.Username = row["username"].ToString();
            this.DisplayName = row["displayname"].ToString();
            this.Role = (int)row["role"];
            this.Password = row["password"].ToString();
        }
    }
}

using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class AccountDAO
    {
        private static AccountDAO instance;

        public static AccountDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new AccountDAO();
                return instance;
            }
            private set { instance = value; }
        }
        private AccountDAO() { }
        public bool Login (string username, string password)
        {
            string query = "USP_Login @username , @password";
            DataTable result = DataProvider.Instance.ExecuteQuery(query, new object[] {username, password});
            return result.Rows.Count > 0;
        }

        public List<Account> SearchAccountByUsername(string username)
        {

            List<Account> list = new List<Account>();

            string query = string.Format("SELECT * FROM dbo.Account WHERE dbo.fuConvertToUnsign1(username) LIKE N'%' + dbo.fuConvertToUnsign1(N'{0}') + '%'", username);

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Account account = new Account(item);
                list.Add(account);
            }

            return list;
        }

        public Account GetAccountByUsername(string username)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM Account WHERE Username = '" + username + "'");
            foreach (DataRow item in data.Rows)
            { 
                return new Account(item);
            }
            return null;
        }
        public bool UpdateAccount (string username, string displayname, string password, string newpassword)
        {
            int result = DataProvider.Instance.ExecuteNonQuery("EXEC USP_UpdateAccount @username , @displayname , @password , @newpassword", new object[] { username, displayname, password, newpassword });
            return result > 0; 
        }
        public DataTable GetListAccount()
        {
            return DataProvider.Instance.ExecuteQuery("SELECT Username, DisplayName, Role FROM dbo.Account");
        }

        public bool InsertAccount(string name, string displayName, int role)
        {
            string query = string.Format("INSERT dbo.Account ( Username, DisplayName, Role, password )VALUES  ( N'{0}', N'{1}', {2}, N'{3}')", name, displayName, role, "1");
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        // Register function: inserts a new account with provided username, password, displayName and role.
        // Returns false if the username already exists.
        public bool RegisterAccount(string username, string password, string displayName, int role)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("username must be provided", nameof(username));
            if (password == null)
                throw new ArgumentNullException(nameof(password));
            if (GetAccountByUsername(username) != null)
                return false; // username already taken

            // Store password as provided
            string query = "INSERT INTO dbo.Account (Username, DisplayName, Role, Password) VALUES ( @username , @displayname , @role , @password )";
            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { username, displayName, role, password });

            return result > 0;
        }

        public bool UpdateAccount(string name, string displayName, int role)
        {
            string query = string.Format("UPDATE dbo.Account SET DisplayName = N'{1}', Role = {2} WHERE UserName = N'{0}'", name, displayName, role);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool DeleteAccount(string name)
        {
            string query = string.Format("Delete Account where Username = N'{0}'", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool ResetPassword(string name)
        {
            string query = string.Format("update account set password = N'1' where Username = N'{0}'", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
    }
}

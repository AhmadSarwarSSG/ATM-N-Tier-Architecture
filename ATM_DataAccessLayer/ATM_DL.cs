using System;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATM_BusinessObject;

namespace ATM_DataAccessLayer
{
    public class ATM_DL
    {//There no such function that is specifically designed for either customer or admin so, all  the function are contained in one class
        public bool doTransaction(ATM_BO BusinessObject, int amount)
        {
            string connection = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=ATM_DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            string query = "insert into Transactions(Type, UserID, Date, Amount) values(@t, @u, @d, @a)";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter TransactionType = new SqlParameter("t", BusinessObject.TransactionType);
            SqlParameter date = new SqlParameter("d", BusinessObject.Date);
            SqlParameter userID = new SqlParameter("u", BusinessObject.UserID);
            SqlParameter Amount = new SqlParameter("a", amount);
            cmd.Parameters.Add(TransactionType);
            cmd.Parameters.Add(date);
            cmd.Parameters.Add(userID);
            cmd.Parameters.Add(Amount);
            int rows = cmd.ExecuteNonQuery();
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool checkCredentials(ATM_BO BusinessObj)
        {
            string connection = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=ATM_DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            string query = "select * from Login where UserID=@u AND password=@p";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter userID = new SqlParameter("u", BusinessObj.UserID);
            SqlParameter password = new SqlParameter("p", BusinessObj.password);
            cmd.Parameters.Add(userID);
            cmd.Parameters.Add(password);
            SqlDataReader dr = cmd.ExecuteReader();
            if(dr.HasRows)
            {
                con.Close();
                return true;
            }
            else
            {
                con.Close();
                return false;
            }
        }
        public ATM_BO getDetails(ATM_BO BusinessObject)
        {
            string connection = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=ATM_DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            string query = "select * from Customer inner join Login on Customer.UserID=Login.UserID where Customer.UserID=@u";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter userID = new SqlParameter("u", BusinessObject.UserID);
            cmd.Parameters.Add(userID);
            SqlDataReader dr = cmd.ExecuteReader();
            while(dr.Read())
            {
                BusinessObject.AccountNO = int.Parse(dr[0].ToString());
                BusinessObject.Name = dr[1].ToString();
                BusinessObject.AccountType = dr[2].ToString();
                BusinessObject.Status = dr[4].ToString();
                BusinessObject.balance = int.Parse(dr[3].ToString());
                BusinessObject.AccountHolder = dr[9].ToString();
            }
            return BusinessObject;
        }
        public bool depositeCash(ATM_BO BusinessObject)
        {
            string connection = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=ATM_DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            string query = "";
            if (BusinessObject.TransactionType!= "Cash WithDrawl")
            {
                query = "update Customer set balance=@b where AccountNo=@a";
            }
            else
            {
                query = "update Customer set balance=@b, Date=@d where AccountNo=@a";
            }
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter balance = new SqlParameter("b", BusinessObject.balance);
            SqlParameter AccountNo = new SqlParameter("a", BusinessObject.AccountNO);
            SqlParameter Amount = new SqlParameter("c", BusinessObject.TransactionAmount);
            SqlParameter Date = new SqlParameter("d", BusinessObject.Date);
            cmd.Parameters.Add(balance);
            cmd.Parameters.Add(AccountNo);
            cmd.Parameters.Add(Amount);
            cmd.Parameters.Add(Date);
            int row = cmd.ExecuteNonQuery();
            if(row>0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public  (bool, ATM_BO) getDetailsbyAccountNo(ATM_BO BusinessObject)
        {
            string connection = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=ATM_DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            string query = "select * from Customer Customer where AccountNo=@a";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter AccNo = new SqlParameter("a", BusinessObject.AccountNO);
            cmd.Parameters.Add(AccNo);
            SqlDataReader dr = cmd.ExecuteReader();
            if(dr.HasRows)
            {
                while (dr.Read())
                {
                    BusinessObject.AccountNO = int.Parse(dr[0].ToString());
                    BusinessObject.Name = dr[1].ToString();
                    BusinessObject.AccountType = dr[2].ToString();
                    BusinessObject.Status = dr[4].ToString();
                    BusinessObject.Date = dr[5].ToString().Split(" ")[0];
                    BusinessObject.balance = int.Parse(dr[3].ToString());
                    BusinessObject.UserID = dr[6].ToString();                
                }
                return (true, BusinessObject);
            }
            else
            {
                return (false, BusinessObject);
            }
        }
        public ATM_BO getAccountType(ATM_BO BusinessObject)
        {
            string connection = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=ATM_DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            string query = "select * from Login where UserID=@u";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter userID = new SqlParameter("u", BusinessObject.UserID);
            cmd.Parameters.Add(userID);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    BusinessObject.password = dr[1].ToString();
                    BusinessObject.AccountHolder=dr[2].ToString();
                }
                return BusinessObject;
            }
            else
            {
                return BusinessObject;
            }
        }
        public bool insertInCustomer(ATM_BO BusinessObject)
        {
            string connection = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=ATM_DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            string query = "insert into Customer(Name, Type, Balance, Status, UserID) values(@N, @t, @b, @s, @u)";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter Name = new SqlParameter("N", BusinessObject.Name);
            SqlParameter type = new SqlParameter("t", BusinessObject.AccountType);
            SqlParameter balacne = new SqlParameter("b", BusinessObject.balance);
            SqlParameter status = new SqlParameter("s", BusinessObject.Status);
            SqlParameter userID = new SqlParameter("u", BusinessObject.UserID);
            cmd.Parameters.Add(Name);
            cmd.Parameters.Add(type);
            cmd.Parameters.Add(balacne);
            cmd.Parameters.Add(status);
            cmd.Parameters.Add(userID);
            int rows = cmd.ExecuteNonQuery();
            if(rows>0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool insertInLogin(ATM_BO BusinessObject)
        {
            string connection = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=ATM_DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            string query = "insert into Login(UserID, Password, Type) values(@u, @p, 'Customer')";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter userID = new SqlParameter("u", BusinessObject.UserID);
            SqlParameter password = new SqlParameter("p", BusinessObject.password) ;
            cmd.Parameters.Add(userID);
            cmd.Parameters.Add(password);
            int rows = cmd.ExecuteNonQuery();
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool deletefromLogin(ATM_BO BusinessObject)
        {
            string connection = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=ATM_DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            string query = "delete from Login where UserID=@u";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter userID = new SqlParameter("u", BusinessObject.UserID);
            cmd.Parameters.Add(userID);
            int rows = cmd.ExecuteNonQuery();
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool deletefromCustomer(ATM_BO BusinessObject)
        {
            string connection = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=ATM_DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            string query = "delete from Customer where UserID=@u";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter userID = new SqlParameter("u", BusinessObject.UserID);
            cmd.Parameters.Add(userID);
            int rows = cmd.ExecuteNonQuery();
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool deletefromTransactions(ATM_BO BusinessObject)
        {
            string connection = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=ATM_DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            string query = "delete from Transactions where UserID=@u";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter userID = new SqlParameter("u", BusinessObject.UserID);
            cmd.Parameters.Add(userID);
            int rows = cmd.ExecuteNonQuery();
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool updateAccountinCustomer(ATM_BO BusinessObject)
        {
            string connection = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=ATM_DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            string query = "update Customer set Name=@n, Status=@s where AccountNo=@a;";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter userID = new SqlParameter("u", BusinessObject.UserID);
            SqlParameter Name = new SqlParameter("n", BusinessObject.Name);
            SqlParameter status = new SqlParameter("s", BusinessObject.Status);
            SqlParameter AccountNo = new SqlParameter("a", BusinessObject.AccountNO);
            cmd.Parameters.Add(userID);
            cmd.Parameters.Add(Name);
            cmd.Parameters.Add(status);
            cmd.Parameters.Add(AccountNo);
            int row = cmd.ExecuteNonQuery();
            if (row > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool updateAccountinLogin(ATM_BO BusinessObject, ATM_BO OldObj)
        {
            string connection = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=ATM_DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            string query = "update Login set Login.UserID=@u, Login.Password=@p where Login.UserID=@o";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter password = new SqlParameter("p", BusinessObject.password);
            SqlParameter userID = new SqlParameter("u", BusinessObject.UserID);
            SqlParameter OldID = new SqlParameter("o", OldObj.UserID);
            cmd.Parameters.Add(userID);
            cmd.Parameters.Add(password);
            cmd.Parameters.Add(OldID);
            int row = cmd.ExecuteNonQuery();
            if (row > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<ATM_BO> getSearch(ATM_BO BusinessObject)
        {
            bool second = false;
            List<ATM_BO> SearchResult = new List<ATM_BO>();
            string connection = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=ATM_DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            string query = "select AccountNo, UserID, Name, Type, Balance, Status from Customer where ";
            if(BusinessObject.AccountNO!=0)
            {
                if(second)
                {
                    query += " AND ";
                }
                second = true;
                query += "AccountN=@a";
            }
            if(BusinessObject.UserID!="")
            {
                if (second)
                {
                    query += " AND ";
                }
                second = true;
                query += "UserID=@u";
            }
            if (BusinessObject.balance != 0)
            {
                if (second)
                {
                    query += " AND ";
                }
                second = true;
                query += "Balance=@b";
            }
            if (BusinessObject.Name != "")
            {
                if (second)
                {
                    query += " AND ";
                }
                second = true;
                query += "Name=@n";
            }
            if (BusinessObject.AccountType != "")
            {
                if (second)
                {
                    query += " AND ";
                }
                second = true;
                query += "Type=@t";
            }
            if (BusinessObject.Status != "")
            {
                if (second)
                {
                    query += " AND ";
                }
                second = true;
                query += "Status=@s";
            }
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter AccountNo = new SqlParameter("a", BusinessObject.AccountNO);
            SqlParameter Name = new SqlParameter("n", BusinessObject.Name);
            SqlParameter type = new SqlParameter("t", BusinessObject.AccountType);
            SqlParameter balacne = new SqlParameter("b", BusinessObject.balance);
            SqlParameter status = new SqlParameter("s", BusinessObject.Status);
            SqlParameter userID = new SqlParameter("u", BusinessObject.UserID);
            cmd.Parameters.Add(Name);
            cmd.Parameters.Add(AccountNo);
            cmd.Parameters.Add(type);
            cmd.Parameters.Add(balacne);
            cmd.Parameters.Add(status);
            cmd.Parameters.Add(userID);
            SqlDataReader dr = cmd.ExecuteReader();
            if(dr.HasRows)
            {
                while(dr.Read())
                {
                    ATM_BO Bo = new ATM_BO();
                    Bo.AccountNO = int.Parse(dr[0].ToString());
                    Bo.UserID = dr[1].ToString();
                    Bo.Name = dr[2].ToString();
                    Bo.AccountType = dr[3].ToString();
                    Bo.balance = int.Parse(dr[4].ToString());
                    Bo.Status = dr[5].ToString();
                    SearchResult.Add(Bo);
                }
                return SearchResult;
            }
            else
            {
                return null;
            }
        }
        public List<ATM_BO> getReportByAmount(int min, int max)
        {
            bool second = false;
            List<ATM_BO> SearchResult = new List<ATM_BO>();
            string connection = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=ATM_DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            string query = "select AccountNo, UserID, Name, Type, Balance, Status from Customer Where Balance BETWEEN @L AND @H";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter minimum = new SqlParameter("L", min);
            SqlParameter maximum = new SqlParameter("H", max);
            cmd.Parameters.Add(minimum);
            cmd.Parameters.Add(maximum);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    ATM_BO Bo = new ATM_BO();
                    Bo.AccountNO = int.Parse(dr[0].ToString());
                    Bo.UserID = dr[1].ToString();
                    Bo.Name = dr[2].ToString();
                    Bo.AccountType = dr[3].ToString();
                    Bo.balance = int.Parse(dr[4].ToString());
                    Bo.Status = dr[5].ToString();
                    SearchResult.Add(Bo);
                }
                return SearchResult;
            }
            else
            {
                return null;
            }
        }
        public List<ATM_BO> getReportByDate(string min, string max)
        {
            List<ATM_BO> SearchResult = new List<ATM_BO>();
            string connection = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=ATM_DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            string query = "select * from Transactions Where Date BETWEEN @L AND @H";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter minimum = new SqlParameter("L", min);
            SqlParameter maximum = new SqlParameter("H", max);
            cmd.Parameters.Add(minimum);
            cmd.Parameters.Add(maximum);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    ATM_BO Bo = new ATM_BO();
                    Bo.TransactionType = dr[1].ToString();
                    Bo.UserID = dr[3].ToString();
                    Bo.Date = dr[2].ToString();
                    Bo.TransactionAmount = int.Parse(dr[4].ToString());
                    SearchResult.Add(Bo);
                }
                return SearchResult;
            }
            else
            {
                return null;
            }
        }
        public ATM_BO getOneDayAmmount(ATM_BO BusinessObject)
        {
            string connection = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=ATM_DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            string query = "select sum(Amount) from Transactions Where Date='" + DateTime.Today.Year + "-" + DateTime.Today.Month + "-" + DateTime.Today.Day + "' AND UserID=@u AND Type='Cash WithDrawl'";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter ID = new SqlParameter("u", BusinessObject.UserID);
            cmd.Parameters.Add(ID);
            SqlDataReader dr = cmd.ExecuteReader();
            if(dr.HasRows)
            {
                dr.Read();
                string amount = dr[0].ToString();
                if(amount=="")
                {
                    BusinessObject.OneDayAmount = 0;
                }
                else
                {
                    BusinessObject.OneDayAmount = int.Parse(amount);
                }
                return BusinessObject;
            }
            else
            {
                return BusinessObject;
            }
        }
        public bool UpdateStatus(ATM_BO BusinessObject)
        {
            string connection = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=ATM_DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            string query = "update Customer set Status='Inactive' where UserID=@u";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlParameter ID = new SqlParameter("u", BusinessObject.UserID);
            cmd.Parameters.Add(ID);
            int i = cmd.ExecuteNonQuery();
            if (i>0)
            { 
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

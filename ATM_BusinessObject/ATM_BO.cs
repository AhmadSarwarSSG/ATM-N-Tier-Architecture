using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM_BusinessObject
{
    public class ATM_BO
    {
        public string UserID { get; set; }
        public string password { get; set; }
        public string Name { get; set; }
        public int AccountNO { get; set; }
        public int balance { get; set; }
        public string AccountType { get; set; }
        public string AccountHolder { get; set; }
        public string Date { get; set; }
        public string Status { get; set; }
        public string TransactionType { get; set; }
        public int TransactionAmount { get; set; }
        public int OneDayAmount { get; set; }
        public int Attempts { get; set; }
    }
}

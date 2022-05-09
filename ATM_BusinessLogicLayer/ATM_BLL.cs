using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATM_BusinessObject;
using ATM_DataAccessLayer;

namespace ATM_BusinessLogicLayer
{
    public class ATM_BLL
    {//There no such function that is specifically designed for either customer or admin so, all  the function are contained in one class
        ATM_DL DL = new ATM_DL();
        public List<ATM_BO> getReportByDate(string start, string end)
        {
            List<ATM_BO> SearchResult = DL.getReportByDate(start, end);
            if (SearchResult != null)
            {
                for (int i = 0; i < SearchResult.Count() || SearchResult == null; i++)
                {
                    //Getting detail of each customer and decrypting it
                    SearchResult[i] = DL.getDetails(SearchResult[i]);
                    SearchResult[i] = DL.getAccountType(SearchResult[i]);
                    SearchResult[i].UserID = encryption_decryption(SearchResult[i].UserID);
                    SearchResult[i].password = encryption_decryption(SearchResult[i].password);
                }
            }
            return SearchResult;
        }
        public List<ATM_BO> getReportByBalance(int min, int max)
        {
            List<ATM_BO> SearchResult = DL.getReportByAmount(min, max);
            if (SearchResult != null)
            {
                for (int i = 0; i < SearchResult.Count() || SearchResult == null; i++)
                {
                    //Getting account tpye and decrypting userId and Password
                    SearchResult[i] = DL.getAccountType(SearchResult[i]);
                    SearchResult[i].UserID = encryption_decryption(SearchResult[i].UserID);
                    SearchResult[i].password = encryption_decryption(SearchResult[i].password);
                }
            }
            return SearchResult;
        }
        public List<ATM_BO> Search(ATM_BO BusinessObject)
        {
            List<ATM_BO> searchResult = DL.getSearch(BusinessObject);
            if (searchResult != null)
            {
                for (int i = 0; i < searchResult.Count() || searchResult == null; i++)
                {
                    //Getting account tpye and decrypting userId and Password
                    searchResult[i] = DL.getAccountType(searchResult[i]);
                    searchResult[i].UserID = encryption_decryption(searchResult[i].UserID);
                    searchResult[i].password = encryption_decryption(searchResult[i].password);
                }
            }
            return searchResult;
        }
        public bool updateAccount(ATM_BO Old_Data, ATM_BO Updated_Data)
        {
            //If the user do not want to update a field just put old data in that field
            if (Updated_Data.UserID == "")
            {
                Updated_Data.UserID = Old_Data.UserID;
            }
            if (Updated_Data.password == "")
            {
                Updated_Data.password = Old_Data.password;
            }
            if (Updated_Data.Name == "")
            {
                Updated_Data.Name = Old_Data.Name;
            }
            if (Updated_Data.Status == "")
            {
                Updated_Data.Status = Old_Data.Status;
            }
            //Saving all old data to perivious data
            Updated_Data.AccountHolder = Old_Data.AccountHolder;
            Updated_Data.AccountNO = Old_Data.AccountNO;
            Updated_Data.AccountType = Old_Data.AccountType;
            Updated_Data.UserID = encryption_decryption(Updated_Data.UserID);
            Updated_Data.password = encryption_decryption(Updated_Data.password);
            Old_Data.UserID = encryption_decryption(Old_Data.UserID);
            Old_Data.password = encryption_decryption(Old_Data.password);
            bool isUpdatedinLogin = DL.updateAccountinLogin(Updated_Data, Old_Data);
            bool isUpdatedinCustomer = DL.updateAccountinCustomer(Updated_Data);
            return isUpdatedinCustomer && isUpdatedinLogin;
        }

        public bool deleteAccount(ATM_BO BusinessObject)
        {
            //Deleting customer according to Account no from all the tables
            BusinessObject.UserID = encryption_decryption(BusinessObject.UserID);
            BusinessObject.password = encryption_decryption(BusinessObject.password);
            bool isdeletedFromCustomer = DL.deletefromCustomer(BusinessObject);
            bool isdeletedFromTransactions = DL.deletefromTransactions(BusinessObject);
            bool isdeletedFromLogin = DL.deletefromLogin(BusinessObject);
            return isdeletedFromCustomer && isdeletedFromLogin;
        }
        public (bool, ATM_BO) addCustomer(ATM_BO BusinessObject)
        {
            //Encypting and inserting user data
            BusinessObject.UserID = encryption_decryption(BusinessObject.UserID);
            BusinessObject.password = encryption_decryption(BusinessObject.password);
            bool isStoredinLogin = DL.insertInLogin(BusinessObject);
            bool isStoredinCustomer = DL.insertInCustomer(BusinessObject);
            BusinessObject = DL.getDetails(BusinessObject);
            BusinessObject = DL.getDetailsbyAccountNo(BusinessObject).Item2;
            BusinessObject.UserID = encryption_decryption(BusinessObject.UserID);
            BusinessObject.password = encryption_decryption(BusinessObject.password);
            return (isStoredinCustomer && isStoredinLogin, BusinessObject);
        }
        public (bool, ATM_BO) TransferCash(int amount, ATM_BO Sender, ATM_BO Receiver)
        {
            Sender.balance -= amount;
            Receiver.balance += amount;
            bool senderCheck = DL.depositeCash(Sender);//Deduting cash from sender
            bool ReceoverCheck = DL.depositeCash(Receiver);//Depositing cash to receriver
            Sender.TransactionType = "Cash Transfer";
            Receiver.TransactionType = "Cash Transfer";
            Sender.UserID = encryption_decryption(Sender.UserID);
            Receiver.UserID = encryption_decryption(Receiver.UserID);
            DL.doTransaction(Sender, amount);
            DL.doTransaction(Receiver, amount);
            return (senderCheck && ReceoverCheck, Sender);
        }
        public (bool, ATM_BO) validateAndGetAccountInfo(ATM_BO Receiver)
        {
            //To validting the if the user exits or not
            var ReceiverData = DL.getDetailsbyAccountNo(Receiver);
            ATM_BO Receriver = ReceiverData.Item2;
            bool suucessStatus = ReceiverData.Item1;
            if(Receiver.UserID!=null || Receiver.password!=null)
            {
                Receiver = DL.getAccountType(Receriver);
                Receiver.UserID = encryption_decryption(Receiver.UserID);
                Receiver.password = encryption_decryption(Receiver.password);
            }
            return (suucessStatus, Receiver);
        }
        public (bool, ATM_BO) DepositeCash(int amount, ATM_BO BusinessObject)
        {
            BusinessObject.UserID = encryption_decryption(BusinessObject.UserID);
            BusinessObject.password = encryption_decryption(BusinessObject.password);
            BusinessObject.balance += amount;//Increasing the balacne
            bool successStatus = DL.depositeCash(BusinessObject);
            BusinessObject.TransactionType = "Cash Deposit";
            DL.doTransaction(BusinessObject, amount);//Saving transaction
            BusinessObject.UserID = encryption_decryption(BusinessObject.UserID);
            BusinessObject.password = encryption_decryption(BusinessObject.password);
            return (successStatus, BusinessObject);
        }
        public (bool, ATM_BO) DeductCash(int amount, ATM_BO BusinessObject)
        {
            BusinessObject.UserID = encryption_decryption(BusinessObject.UserID);
            BusinessObject.password = encryption_decryption(BusinessObject.password);
            BusinessObject = DL.getDetailsbyAccountNo(BusinessObject).Item2;
            BusinessObject = DL.getOneDayAmmount(BusinessObject);
            BusinessObject.OneDayAmount += amount;
            if (BusinessObject.OneDayAmount <= 20000)//Checking if the cash if exceeding the 20000 in one day
            {
                BusinessObject.balance -= amount;
                BusinessObject.TransactionType = "Cash WithDrawl";//Setting transaction type
                BusinessObject.TransactionAmount = amount;
                BusinessObject.Date = DateTime.Today.Year + "-" + DateTime.Today.Month + "-" + DateTime.Today.Day;
                bool successStatus = DL.depositeCash(BusinessObject);
                DL.doTransaction(BusinessObject, amount);
                BusinessObject.UserID = encryption_decryption(BusinessObject.UserID);
                BusinessObject.password = encryption_decryption(BusinessObject.password);
                return (successStatus, BusinessObject);
            }
            else
            {
                BusinessObject.UserID = encryption_decryption(BusinessObject.UserID);
                BusinessObject.password = encryption_decryption(BusinessObject.password);
                return (false, BusinessObject);
            }
        }
        public (bool, ATM_BO) checkCredentials(ATM_BO BusinessObject)
        {
            BusinessObject.UserID = encryption_decryption(BusinessObject.UserID);
            BusinessObject.password = encryption_decryption(BusinessObject.password);
            if (DL.checkCredentials(BusinessObject))//Checking if the account the accout exit
            {
                BusinessObject = DL.getDetails(BusinessObject);//Getting account detail
                BusinessObject = DL.getAccountType(BusinessObject);//Getting account type
                BusinessObject = DL.getDetailsbyAccountNo(BusinessObject).Item2;//getting account Info
                BusinessObject.UserID = encryption_decryption(BusinessObject.UserID);
                BusinessObject.password = encryption_decryption(BusinessObject.password);
                if(BusinessObject.Status=="Active" || BusinessObject.AccountHolder=="Admin")//Checking if it is admin or account is active
                {
                    return (true, BusinessObject);
                }
                else
                {
                    return (false, BusinessObject);
                }
            }
            else
            {

                BusinessObject.UserID = encryption_decryption(BusinessObject.UserID);
                BusinessObject.password = encryption_decryption(BusinessObject.password);
                return (false, BusinessObject);
            }
        }
        public bool menuSelection(string option)
        {
            //Checking customer menu selection
            if (option == "")
            {
                return false;
            }
            else
            {
                int Option = int.Parse(option);
                if (Option > 6 || Option < 1)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public int checkAmount(int amount, ATM_BO BusinessObj)
        {
            if (BusinessObj.balance >= amount)
            {
                //If the balacne is more than amount return 2
                return 2;
            }
            else
            {
                if (BusinessObj.balance < 500)
                {
                    //If the balacne is less than 500
                    return 0;
                }
                else
                {
                    //If the balacne is more than amount is less than given amount
                    return 1;
                }
            }
        }
        public bool isMultipleOf500(int amount)
        {
            //Checking if the number is multiple of 500
            if (amount % 500 == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool receiptAgreement(string opt)
        {
            //Validating receipt input
            if (opt == "y" || opt == "Y" || opt == "n" || opt == "N")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool fastcashOptions(int opt)
        {
            //lvalidating fast cash input
            if (opt < 1 || opt > 7)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool adminMenuCheck(string option)
        {
            //Validing Admin menu input
            if (option == "")
            {
                return true;
            }
            else
            {
                int Option = int.Parse(option);
                if (Option < 1 && Option > 7)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public string encryption_decryption(string s)
        {
            char[] arr = s.ToArray<char>();
            string encrypted = "";
            foreach (char c in arr)
            {
                if (System.Convert.ToInt32(c) >= 65 && System.Convert.ToInt32(c) <= 90)
                {
                    //If there is some capital digit if will be subtracted from 155
                    char New = System.Convert.ToChar(155 - System.Convert.ToInt32(c));
                    encrypted += New;
                }
                if (System.Convert.ToInt32(c) >= 97 && System.Convert.ToInt32(c) <= 122)
                {
                    //If there is some Small digit if will be subtracted from 219
                    char New = System.Convert.ToChar(219 - System.Convert.ToInt32(c));
                    encrypted += New;
                }
                if (System.Convert.ToInt32(c) >= 48 && System.Convert.ToInt32(c) <= 57)
                {
                    //If there is some number if will be subtracted from 105
                    char New = System.Convert.ToChar(105 - System.Convert.ToInt32(c));
                    encrypted += New;
                }
            }
            return encrypted;
        }
        public bool changeStatus(ATM_BO BusinessObject)
        {
            BusinessObject.UserID = encryption_decryption(BusinessObject.UserID);//Encrypting data username for database
            return DL.UpdateStatus(BusinessObject);
        }
    }
}

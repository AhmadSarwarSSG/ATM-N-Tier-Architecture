using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATM_BusinessLogicLayer;
using ATM_BusinessObject;

namespace ATM_PresentationLayer
{
    public class Customer_PL
    {
        ATM_BLL BLL = new ATM_BLL();
        ATM_BO BO=new ATM_BO();
        public Customer_PL(ATM_BO BusinessObject, ATM_BLL bll)
        {   //To get same user that logged in
            BLL = bll;
            BO = BusinessObject;
        }
        public void customerMenu()
        {
            Console.WriteLine("~~~~~~~~~~ WELCOME TO ATM ~~~~~~~~~~");
            Console.WriteLine("1-----WithDraw Cash");
            Console.WriteLine("2-----Cash Transfer");
            Console.WriteLine("3-----Deposite Cash");
            Console.WriteLine("4-----Display Balance");
            Console.WriteLine("5-----Exit");
            Console.ForegroundColor = ConsoleColor.Green;
            string choice = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;
            while (!BLL.menuSelection(choice))// Checking if the Option is betweeen 1 and 5
            {
                Console.WriteLine("You have selected Wrong Option kindly Select again");
                Console.WriteLine("1-----WithDraw Cash");
                Console.WriteLine("2-----Cash Transfer");
                Console.WriteLine("3-----Deposite Cash");
                Console.WriteLine("4-----Display Balance");
                Console.WriteLine("5-----Exit");
                Console.ForegroundColor = ConsoleColor.Green;
                choice = Console.ReadLine();    //Getting Input again
                Console.ForegroundColor = ConsoleColor.White;
            }
            int Choice = int.Parse(choice);
            if (Choice == 1)
            {
                cashWithDraw();
            }
            else if (Choice == 2)
            {
                cashTransfer();
            }
            else if (Choice == 3)
            {
                CashDeposit();
            }
            else if (Choice == 4)
            {
                displayBalance();
            }
            else if (Choice == 5)
            {
                //exit
            }

        }
        public void cashWithDraw()
        {
            Console.WriteLine("a) Fast Cash");
            Console.WriteLine("b) Normal Cash");
            Console.Write("Please select one of the above options: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string option = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;
            if (option == "a" || option == "A")
            {
                fastcash();
            }
            else if(option=="b" || option=="B")
            {
                normalWithDrawl();
            }
            else
            {
                Console.WriteLine("Wrong Input");
                cashWithDraw();
            }
        }
        public void cashTransfer()
        {
            int amount;
            Console.Write("Enter amount in multiples of 500: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string Amount = Console.ReadLine();
            if(Amount=="")
            {
                amount = 0;
            }
            else
            {
                amount = int.Parse(Amount);
            }
            Console.ForegroundColor = ConsoleColor.White;
            while (BLL.isMultipleOf500(amount))//Checking if the amoount is multiple of 500
            {
                Console.Write("Kindly Re-Enter amount in the multiples of 500: ");
                Console.ForegroundColor = ConsoleColor.Green;
                Amount = Console.ReadLine();
                if (Amount == "")
                {
                    amount = 0;
                }
                else
                {
                    amount = int.Parse(Amount);
                }
                Console.ForegroundColor = ConsoleColor.White;
            }
            if (BLL.checkAmount(amount, BO) == 1)
            {
                Console.WriteLine("Sorry you can not Transfer money you have less then 500");
                customerMenu();
            }
            while (BLL.checkAmount(amount, BO) != 2)
            {
                Console.WriteLine("Entered Amount is Greater than your current Balance kindly re-enter amount: ");
                Console.ForegroundColor = ConsoleColor.Green;
                Amount = Console.ReadLine();
                if (Amount == "")
                {
                    amount = 0;
                }
                else
                {
                    amount = int.Parse(Amount);
                }
                Console.ForegroundColor = ConsoleColor.White;

            }
            Console.Write("Enter the account number to which you want to transfer: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string account = Console.ReadLine();
            int accountNO;
            if(account=="")
            {
                accountNO = -1;
            }
            else
            {
                accountNO = int.Parse(account);
            }
            Console.ForegroundColor = ConsoleColor.White;
            ATM_BO Receiver = new ATM_BO();
            Receiver.AccountNO = accountNO;
            var receriverCheck = BLL.validateAndGetAccountInfo(Receiver); //Validating if the customer exits or not
            while (!receriverCheck.Item1)
            {
                Console.Write("You have entered wrong Account Number Kindly Re-Enter: ");
                Console.ForegroundColor = ConsoleColor.Green;
                account = Console.ReadLine();
                if (account == "")//If customer just press enter
                {
                    accountNO = -1;
                }
                else
                {
                    accountNO = int.Parse(account);
                }
                Console.ForegroundColor = ConsoleColor.White;
                Receiver.AccountNO = accountNO;
                receriverCheck = BLL.validateAndGetAccountInfo(Receiver);
            }
            Receiver = receriverCheck.Item2;
            string accountantName = Receiver.Name;
            Console.Write("You wish to deposit Rs " + amount + " in account held by " + accountantName + " If this information is correct please re - enter the account number: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string RecheckAccNO = Console.ReadLine();
            int recheckAccNO;
            if (RecheckAccNO=="")
            {
                recheckAccNO = 0;
            }
            else
            {
                recheckAccNO = int.Parse(RecheckAccNO);
            }
            Console.ForegroundColor = ConsoleColor.White;
            while (recheckAccNO != accountNO)
            {
                Console.WriteLine("You have entered Wrong account number Kindly re-enter Account No: ");
                Console.ForegroundColor = ConsoleColor.Green;
                RecheckAccNO = Console.ReadLine();
                if (RecheckAccNO == "")
                {
                    recheckAccNO = 0;
                }
                else
                {
                    recheckAccNO = int.Parse(RecheckAccNO);
                }
                Console.ForegroundColor = ConsoleColor.White;
            }
            BO.Date = DateTime.Today.Year + "-" + DateTime.Today.Month + "-" + DateTime.Today.Day;
            Receiver.Date = DateTime.Today.Year + "-" + DateTime.Today.Month + "-" + DateTime.Today.Day;
            var TransferStatus = BLL.TransferCash(amount, BO, Receiver);
            if (TransferStatus.Item1)
            {
                BO = TransferStatus.Item2;
                Console.WriteLine("Transaction Confirmed");
                Console.WriteLine("Do you wish to print a receipt (Y/N)?");
                Console.ForegroundColor = ConsoleColor.Green;
                string agreement = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.White;
                while (BLL.receiptAgreement(agreement))
                {
                    Console.Write("You have entered wrong option Kindly input again: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    agreement = Console.ReadLine();
                    Console.ForegroundColor = ConsoleColor.White;
                }
                if (agreement == "y" || agreement == "Y")
                {
                    displayBalance(2, amount); //Getting the Transfer receipt
                }
                else
                {
                    Console.WriteLine("Thank you for Using our ATM Services");
                }
            }
            else
            {
                Console.WriteLine("Sorry Transaction Unsuccessfull");
            }
        }
        public void CashDeposit()
        {
            Console.Write("Enter the cash amount to deposit: ");
            Console.ForegroundColor = ConsoleColor.Green;
            int amount;
            string Amount = Console.ReadLine();
            if (Amount == "")   //If the user simply press enter
            {
                amount = 0;
            }
            else
            {
                amount = int.Parse(Amount);
            }
            Console.ForegroundColor = ConsoleColor.White;
            BO.Date = DateTime.Today.Year + "-" + DateTime.Today.Month + "-" + DateTime.Today.Day;//Setting up the date
            var checkDeposite = BLL.DepositeCash(amount, BO);
            BO = checkDeposite.Item2;
            if (checkDeposite.Item1)
            {
                Console.WriteLine("Cash deposited Successfully");
                Console.WriteLine("Do you wish to print a receipt (Y/N)?");
                Console.ForegroundColor = ConsoleColor.Green;
                string agreement = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.White;
                while (BLL.receiptAgreement(agreement))
                {
                    Console.Write("You have entered wrong option Kindly input again: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    agreement = Console.ReadLine();
                    Console.ForegroundColor = ConsoleColor.White;
                }
                if (agreement == "y" || agreement == "Y")
                {
                    displayBalance(3, amount); //Getting deposite slip
                }
                else
                {
                    Console.WriteLine("Thank you for Using our ATM Services");
                }
            }
            else
            {
                Console.WriteLine("Sorry Cash was not deposited");
            }
        }
        public void displayBalance(int type = 0, int amount = 0)
        {
            string[] date;
            Console.WriteLine("Account#" + BO.AccountNO);
            Console.WriteLine("Dated: " + DateTime.Today.Day + "/" + DateTime.Today.Month + "/" + DateTime.Today.Year);
            if (type == 1)
            {
                Console.WriteLine("Withdrawn: " + amount);// Display when cash is with drawn
            }
            else if (type == 2)
            {
                Console.WriteLine("Amount Transfered: " + amount);// Display when cash is Transfered
            }
            else if (type == 3)
            {
                Console.WriteLine("Deposited: " + amount);// Display when cash is Deposited
            }
            Console.WriteLine("Balacne: " + BO.balance);
        }
        public void fastcash()
        {
            int amount = 0;
            bool validation = true;
            while (validation)
            {
                Console.Write("1----500\n2----1000\n3----2000\n4----5000\n5----10000\n6----15000\n7----20000\n");
                Console.Write("Select one of the denominations of money: ");
                Console.ForegroundColor = ConsoleColor.Green;
                int option;
                string Option = Console.ReadLine();
                if (Option == "")//If user simply press enter
                {
                    option = 0;
                }
                else
                {
                    option= int.Parse(Option);
                }
                Console.ForegroundColor = ConsoleColor.White;
                while (BLL.fastcashOptions(option))
                {
                    Console.Write("You have entered wrong option kindly Re-Enter: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Option = Console.ReadLine();
                    if (Option == "")
                    {
                        option = 0;
                    }
                    else
                    {
                        option = int.Parse(Option);
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                }

                if (option == 1)
                {
                    validation = fastCashOption(500);
                    amount = 500;
                }
                if (option == 2)
                {
                    validation = fastCashOption(1000);
                    amount = 1000;
                }
                if (option == 3)
                {
                    validation = fastCashOption(2000);
                    amount = 2000;
                }
                if (option == 4)
                {
                    validation = fastCashOption(5000);
                    amount = 5000;
                }
                if (option == 5)
                {
                    validation = fastCashOption(10000);
                    amount = 10000;
                }
                if (option == 6)
                {
                    validation = fastCashOption(15000);
                    amount = 15000;
                }
                if (option == 7)
                {
                    validation = fastCashOption(20000);
                    amount = 20000;
                }
            }
            BO.Date = DateTime.Today.Year + "-" + DateTime.Today.Month + "-" + DateTime.Today.Day;
            var withdraw = BLL.DeductCash(amount, BO);
            if (withdraw.Item1)//Checking if the cash is withdrawn or not
            {
                BO = withdraw.Item2;
                Console.WriteLine("Do you wish to print a receipt (Y/N)?");
                Console.ForegroundColor = ConsoleColor.Green;
                string reciept_agreement = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.White;
                while (BLL.receiptAgreement(reciept_agreement))
                {
                    Console.Write("You have entered wrong option Kindly input again: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    reciept_agreement = Console.ReadLine();
                    Console.ForegroundColor = ConsoleColor.White;
                }
                if (reciept_agreement == "y" || reciept_agreement == "Y")
                {
                    displayBalance(1, amount);// DisPlay withdraw receipt
                }
                else
                {
                    Console.WriteLine("Thank you for Using our ATM Services");
                }
            }
            else
            {
                Console.WriteLine("Sorry You can not withdraw at this moment");
            }
        }
        public bool fastCashOption(int Amount)
        {
            int amount = 0;
            Console.WriteLine("Are you sure you want to withdraw Rs." + Amount + " (Y/N)");
            Console.ForegroundColor = ConsoleColor.Green;
            string agreement = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;
            while (BLL.receiptAgreement(agreement))
            {
                Console.Write("You have entered wrong option Kindly input again: ");
                Console.ForegroundColor = ConsoleColor.Green;
                agreement = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.White;
            }
            if (agreement == "y" || agreement == "Y")
            {
                amount = Amount;
                if (BLL.checkAmount(amount, BO) == 2)//Checking if there is enough amount in account
                {
                    return false;
                }
                else if (BLL.checkAmount(amount, BO) == 0)//If the balance is less than 500
                {
                    Console.WriteLine("Your Current Balance is less than 500 you can not use fast cash");
                    customerMenu();
                }
                else if (BLL.checkAmount(amount, BO) == 1)
                {
                    return true;
                }
            }
            else
            {
                Console.WriteLine("Thank you for Using our ATM Services");
                customerMenu();
            }
            return true;
        }
        public void normalWithDrawl()
        {
            Console.Write("Enter the withdrawal amount: ");
            Console.ForegroundColor = ConsoleColor.Green;
            int amount;
            string Amount = Console.ReadLine();
            if (Amount == "")
            {
                amount = 0;
            }
            else
            {
                amount = int.Parse(Amount);
            }
            Console.ForegroundColor = ConsoleColor.White;
            while (BLL.checkAmount(amount, BO) != 2)//Checking if there is enough amount
            {
                Console.Write("Invalid amount kindly Re-Enter the withdrawal amount: ");
                Console.ForegroundColor = ConsoleColor.Green;
                Amount = Console.ReadLine();
                if (Amount == "")
                {
                    amount = 0;
                }
                else
                {
                    amount = int.Parse(Amount);
                }
                Console.ForegroundColor = ConsoleColor.White;
            }
            BO.Date = DateTime.Today.Year + "-" + DateTime.Today.Month + "-" + DateTime.Today.Day;
            var withdraw = BLL.DeductCash(amount, BO);
            if (withdraw.Item1)
            {
                BO = withdraw.Item2;
                Console.WriteLine("Cash Successfully Withdrawn!");
                Console.WriteLine("Do you wish to print a receipt (Y/N)?");
                Console.ForegroundColor = ConsoleColor.Green;
                string reciept_agreement = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.White;
                while (BLL.receiptAgreement(reciept_agreement))
                {
                    Console.Write("You have entered wrong option Kindly input again: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    reciept_agreement = Console.ReadLine();
                    Console.ForegroundColor = ConsoleColor.White;
                }
                if (reciept_agreement == "y" || reciept_agreement == "Y")
                {
                    displayBalance(1, amount);//Display withdraw receipt
                }
                else
                {
                    Console.WriteLine("Thank you for Using our ATM Services");
                }
            }
            else
            {
                Console.WriteLine("Sorry we Cash can not be drawn at this moment");
            }

        }
    }
    public class Admin_PL
    {
        ATM_BLL BLL = new ATM_BLL();
        ATM_BO BO = new ATM_BO();
        public Admin_PL(ATM_BO BusinessObject, ATM_BLL bll)
        {
            BLL = bll;
            BO = BusinessObject;
        }
        public void AdminMenu()
        {
            Console.Write("1----Create New Account.\n2----Delete Existing Account.\n3----Update Account Information.\n4----Search for Account.\n5----View Reports\n6----Exit\n");
            Console.ForegroundColor = ConsoleColor.Green;
            string choice = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;
            while (BLL.adminMenuCheck(choice))//Checking if  the user have entered betweeen 1 and 7
            {
                Console.Write("You have entered wrong input kindly re-enter the value: ");
                Console.ForegroundColor = ConsoleColor.Green;
                choice = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.White;
            }
            int Choice = int.Parse(choice);
            if (Choice == 1)//Chosing option according to menu
            {
                createAccount();
            }
            if (Choice == 2)
            {
                deleteExisting();
            }
            if (Choice == 3)
            {
                updateAccount();
            }
            if (Choice == 4)
            {
                searchAccount();
            }
            if (Choice == 5)
            {
                Report();
            }
        }
        public void createAccount()
        {   //Getting Customer details
            Console.Write("Login: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string username = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Pin: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string password = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Acccount Holder Name: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string name = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Type(Saving, Current): ");
            Console.ForegroundColor = ConsoleColor.Green;
            string type = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Starting Balance: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string Balance= Console.ReadLine();
            int balance;
            if (Balance=="")
            {
                balance = 0;
            }
            else
            {
                balance = int.Parse(Balance);
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Status: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string status = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;
            BO.Name = name;
            BO.AccountType = type;
            BO.balance = balance;
            BO.UserID = username;
            BO.password = password;
            BO.Status = status;
            var businessobj = BLL.addCustomer(BO);//Adding the customer
            if (businessobj.Item1)
            {
                BO = businessobj.Item2;
                Console.WriteLine("Account Created Successfully - Your Account NO is: " + BO.AccountNO);
            }
            else
            {
                Console.WriteLine("Sorry Your Account is not created");
            }
        }
        public void deleteExisting()
        {
            ATM_BO BusinessObject = new ATM_BO();
            Console.Write("Enter the account number to which you want to delete: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string AccountNo = Console.ReadLine();
            int accountNo;
            if(AccountNo=="")//Checking if the user haved just pressed enter
            {
                accountNo = 0;
            }
            else
            {
                accountNo = int.Parse(AccountNo);
            }
            Console.ForegroundColor = ConsoleColor.White;
            BusinessObject.AccountNO = accountNo;
            var BusinessObj = BLL.validateAndGetAccountInfo(BusinessObject);
            BusinessObject = BusinessObj.Item2;
            if(BusinessObj.Item1)
            {
                Console.Write("You wish to delete the account held by " + BusinessObject.Name + "; If this information is correct please re - enter the account number: ");
                Console.ForegroundColor = ConsoleColor.Green;
                string Re_accountNo= Console.ReadLine();
                int re_accountNo;
                if (Re_accountNo == "")
                {
                    re_accountNo = 0;
                }
                else
                {
                    re_accountNo = int.Parse(Re_accountNo);
                }
                Console.ForegroundColor = ConsoleColor.White;
                if (accountNo == re_accountNo)
                {
                    if (BLL.deleteAccount(BusinessObject))//Checking if the Account is deleted or not
                    {
                        Console.WriteLine("Account Deleted Successfully");
                    }
                    else
                    {
                        Console.WriteLine("Sorry we can not delete this account");
                    }
                }
                else
                {
                    Console.WriteLine("Account Number Miss match");
                }
            }
            else
            {
                Console.WriteLine("This Account does not exit");
            }
        }
        public void updateAccount()
        {
            ATM_BO BusinessObject = new ATM_BO();
            Console.Write("Enter the Account Number: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string AccountNo = Console.ReadLine();
            int accountNo;
            if (AccountNo == "")//Checking if the user just pressed enter and store -1 n account no
            {
                accountNo = -1;
            }
            else
            {
                accountNo = int.Parse(AccountNo);
            }
            Console.ForegroundColor = ConsoleColor.White;
            BusinessObject.AccountNO = accountNo;
            var BusinessObj = BLL.validateAndGetAccountInfo(BusinessObject);
            BusinessObject = BusinessObj.Item2;
            if(BusinessObj.Item1)
            {
                //Displaing data and getting update if the account exits
                Console.WriteLine("Account# " + BusinessObject.AccountNO);
                Console.WriteLine("Type: " + BusinessObject.AccountType);
                Console.WriteLine("Holder: " + BusinessObject.Name);
                Console.WriteLine("Balance: " + BusinessObject.balance);
                Console.WriteLine("Status: " + BusinessObject.Status);
                Console.WriteLine("Please enter in the fields you wish to update (leave blank otherwise):");
                Console.Write("Login: ");
                Console.ForegroundColor = ConsoleColor.Green;
                string username = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Pin: ");
                Console.ForegroundColor = ConsoleColor.Green;
                string password = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Acccount Holder Name: ");
                Console.ForegroundColor = ConsoleColor.Green;
                string name = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Status: ");
                Console.ForegroundColor = ConsoleColor.Green;
                string status = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.White;
                ATM_BO updatedObject = new ATM_BO();
                updatedObject.UserID = username;
                updatedObject.password = password;
                updatedObject.Name = name;
                updatedObject.Status = status;
                if (BLL.updateAccount(BusinessObject, updatedObject))
                {
                    Console.WriteLine("Your account has been successfully been updated.");
                }
                else
                {
                    Console.WriteLine("Sorry we cannot update your account");
                }
            }
            else
            {
                Console.WriteLine("This Account Doesn't Exits");
            }
        }
        public void searchAccount()
        {
            int AccountNo;
            int balance;
            ATM_BO BusinessObject = new ATM_BO();
            Console.WriteLine("Please enter in the fields you wish to Seaarch (leave blank otherwise):");
            Console.Write("User ID: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string username = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Account No: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string Account = Console.ReadLine();
            if(Account=="")
            {
                AccountNo = 0;
            }
            else
            {
                AccountNo = int.Parse(Account);
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Acccount Holder Name: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string name = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Type(Saving, Current): ");
            Console.ForegroundColor = ConsoleColor.Green;
            string type = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Starting Balance: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string Balance = Console.ReadLine();
            if (Balance == "")//Checking if the user jsut missed Balacned field empty
            {
                balance = 0;
            }
            else
            {
                balance = int.Parse(Balance);
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Status: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string status = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;
            BusinessObject.UserID = username;
            BusinessObject.AccountNO = AccountNo;
            BusinessObject.Name = name;
            BusinessObject.AccountType = type;
            BusinessObject.balance = balance;
            BusinessObject.Status = status;
            Console.WriteLine("==== SEARCH RESULTS ======");
            string present = string.Format("{0,-20}{1,-20}{2,-20}{3,-20}{4,-20}{5,-20}", "AccountNO", "UserID", "Name", "Type", "Balance", "Status");
            foreach (ATM_BO Bo in BLL.Search(BusinessObject))
            {   //Displaying formated results
                string present_ = string.Format("{0,-20}{1,-20}{2,-20}{3,-20}{4,-20}{5,-20}", Bo.AccountNO, Bo.UserID, Bo.Name, Bo.AccountType, Bo.balance, Bo.Status);
                Console.WriteLine(present_);
            }

        }
        public void Report()
        {   ///Displaying report menu
            Console.Write("1---Accounts By Amount\n2---Accounts By Date\n");
            Console.ForegroundColor = ConsoleColor.Green;
            int option = int.Parse(Console.ReadLine());
            Console.ForegroundColor = ConsoleColor.White;
            if (option == 1)
            {
                reportByAmount();
            }
            else if (option == 2)
            {
                reportByDate();
            }
            else
            {
                Console.WriteLine("Wrong Input");
                Report();
            }
        }
        public void reportByAmount()
        {
            int min, max;
            Console.Write("Enter the minimum amount: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string Min = Console.ReadLine();
            if(Min=="")//Taking input and validaing amount
            {
                min = 0;
            }
            else
            {
                min = int.Parse(Min);
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Enter the maximum amount: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string Max = Console.ReadLine();
            if (Max == "")
            {
                max = 0;
            }
            else
            {
                max = int.Parse(Max);
            }
            Console.ForegroundColor = ConsoleColor.White;
            List<ATM_BO> List = BLL.getReportByBalance(min, max);
            Console.WriteLine("==== SEARCH RESULTS ======");
            string present = string.Format("{0,-20}{1,-20}{2,-20}{3,-20}{4,-20}{5,-20}", "AccountNO", "UserID", "Name", "Type", "Balance", "Status");
            Console.WriteLine(present);
            if (List != null)
            {   //Displaying result if the list is not null
                foreach (ATM_BO Bo in List)
                {
                    string present_ = string.Format("{0,-20}{1,-20}{2,-20}{3,-20}{4,-20}{5,-20}", Bo.AccountNO, Bo.UserID, Bo.Name, Bo.AccountType, Bo.balance, Bo.Status);
                    Console.WriteLine(present_);
                }
            }
            else
            {
                Console.WriteLine("NO Result Found");
            }
        }
        public void reportByDate()
        {
            Console.Write("Enter the Starting Date: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string min = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Enter the Ending Date: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string max = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;                                                                                        
            //Foramtting date according the dates of SQL to be compared
            string start = min.Split("/")[2] + "-" + min.Split("/")[1] + "-" + min.Split("/")[0];
            string ending = max.Split("/")[2] + "-" + max.Split("/")[1] + "-" + max.Split("/")[0];
            List<ATM_BO> List = BLL.getReportByDate(start, ending);
            Console.WriteLine("==== SEARCH RESULTS ======");
            string present = string.Format("{0,-20}{1,-20}{2,-20}{3,-20}{4,-20}", "Transaction Type", "UserID", "Name", "Amount", "Date");
            Console.WriteLine(present);
            if (List != null)
            {
                foreach (ATM_BO Bo in List)
                {
                    //Foramted Output
                    string present_ = string.Format("{0,-20}{1,-20}{2,-20}{3,-20}{4,-20}", Bo.TransactionType, Bo.UserID, Bo.Name, Bo.TransactionAmount, Bo.Date);
                    Console.WriteLine(present_);
                }
            }
            else
            {
                Console.WriteLine("NO Result Found");
            }

        }
    }
    public class ATM_PL
    {
        ATM_BLL BLL = new ATM_BLL();
        ATM_BO BO = new ATM_BO();
        public void getLogin()
        {
            int wrongCount = 0;
            Console.Write("Enter Username: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string userName = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Enter Password: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string password = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;
            BO.UserID = userName;
            BO.password = password;
            BO = BLL.checkCredentials(BO).Item2;
            string previosUsername = userName;
            while(!BLL.checkCredentials(BO).Item1)//Validting the accounf
            {
                if(BO.Status=="Inactive")
                {
                    Console.WriteLine("Your Account is currently Inactive Kindly Contact the Admin");
                    BO.AccountHolder = "";
                    break;
                }
                if(previosUsername==userName)//Checking if the same user have entered wrong password
                {
                    wrongCount++;
                }
                else
                {
                    previosUsername = userName;
                    wrongCount = 1; //If there is somenew username with wrong password the count will again start from 1
                }
                if(wrongCount==3)
                {
                    //If couny become 3 the we'll change the status to inactive
                    BO.Status = "Inactive";
                    BLL.changeStatus(BO);
                    Console.WriteLine("Too mainy Wrong Attempts your account is being blocked kindly Contact your Admin");
                    break;
                }
                Console.WriteLine("~~ Either your Username or Password is incoorect ~~");
                Console.Write("Enter Username: ");
                Console.ForegroundColor = ConsoleColor.Green;
                userName = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Enter Password: ");
                Console.ForegroundColor = ConsoleColor.Green;
                password = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.White;
                BO.UserID = userName;
                BO.password = password;
            }
            if(BO.AccountHolder=="Customer")
            {
                Customer_PL CPL = new Customer_PL(BO, BLL);
                CPL.customerMenu();
            }
            if(BO.AccountHolder=="Admin")
            {
                Admin_PL APL = new Admin_PL(BO, BLL);
                APL.AdminMenu();
            }
        }
        
    }
}

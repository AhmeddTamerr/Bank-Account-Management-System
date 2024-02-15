using System;
using System.Collections.Generic;

public interface IInterestEarning
{
    void ApplyInterest();
    decimal CalculateInterest();
    void CalculateInterestBetweenDates();
}

public static class InterestExtensions
{
    public static void CalculateTotalInterest(this IInterestEarning interest, DateTime startDate, DateTime endDate)
    {
        double months = (int)endDate.Subtract(startDate).Days / (365.25 / 12);
        int averageMonths = (int)months;
        Console.Write($"Total interest earned in {averageMonths} months: ");
        Console.WriteLine($"E£{(interest.CalculateInterest() * averageMonths):F2}");
    }
}

internal class Program
{
    class BankAccount : IInterestEarning
    {
        public string AccountNumber { get; init; }
        public string AccountHolderName { get; set; }
        public Decimal Balance { get; set; }

        public BankAccount(string name, decimal initialBalance)
        {
            AccountHolderName = name;
            Balance = initialBalance;
            AccountNumber = GenerateAccountNumber();
        }

        private string GenerateAccountNumber()
        {
            Random random = new Random();
            string accountTypePrefix = GetAccountTypePrefix();
            string randomDigits = random.Next(1000000, 9999999).ToString();
            return $"{accountTypePrefix}{randomDigits}";
        }

        private string GetAccountTypePrefix()
        {
            switch (this.GetType().Name)
            {
                case nameof(SavingsAccount):
                    return "SAV";
                case nameof(InvestmentAccount):
                    return "INV";
                case nameof(CheckingAccount):
                    return "CHK";
                case nameof(CreditAccount):
                    return "CRE";
                default:
                    return "BNK";
            }
        }



        public virtual void Deposit(decimal value)
        {
            if (value >= 0)
            {
                Balance += value;
                Console.WriteLine($"Deposited E£{value:F2} into account {AccountNumber}. New Balance: E£{Balance:F2}");
            }
            else
                Console.WriteLine("you can't deposit a negative number");
        }

        public virtual void Withdraw(Decimal value)
        {
            if (value > 0 && Balance >= value)
            {
                Balance -= value;
                Console.WriteLine($"Withdrawn E£{value:F2} from account {AccountNumber}. New Balance: E£{Balance:F2} ");
            }
            else
                Console.WriteLine($"Insufficient funds in account {AccountNumber}.");
        }

        public virtual void CheckBalance()
        {
            Console.WriteLine($"Account {AccountNumber} balance: E£{Balance:F2}");
        }

        public void ApplyInterest()
        {
            // Placeholder for applying interest, not implemented in this example
        }

        public decimal CalculateInterest()
        {
            // Placeholder for calculating interest, not implemented in this example
            return 0;
        }

        public void CalculateInterestBetweenDates()
        {
            // Placeholder for CalculateInterestBetweenDates, not implemented in this example
        }
    }
    class InvestmentAccount : BankAccount, IInterestEarning
    {
        public decimal monthlyinterest;
        private decimal Interestrate = 0.05m; // 5%
        private decimal interestamount = 0;

        /* Constructor */
        public InvestmentAccount(string name, decimal initialBalance)
            : base(name, initialBalance)
        {

        }

        public override void Deposit(decimal amount)
        {
            base.Deposit(amount);
        }

        public override void Withdraw(decimal amount)
        {
            base.Withdraw(amount);
        }

        public override void CheckBalance()
        {
            Console.WriteLine($"New balance: E£{(Balance+monthlyinterest):F2}");
            Console.WriteLine($"Account {AccountNumber} balance: {(Balance+monthlyinterest):F2}");
        }

        public new void CalculateInterestBetweenDates()
        {
            Console.WriteLine("Calculating interest between two dates");
            Console.WriteLine("Please insert start date: (dd/MM/yyyy)");

            if (DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime startDate))
            {
                Console.WriteLine("Please insert end date (dd/MM/yyyy):");

                if (DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime endDate))
                {
                    double months = (int)endDate.Subtract(startDate).Days / (365.25 / 12);
                    int averageMonths = (int)months;
                    Console.Write($"Total interest earned in {averageMonths} months: ");
                    interestamount = Balance * Interestrate * averageMonths;
                    Console.WriteLine($"E£{interestamount:F2}");
                    decimal monthlyinterest = interestamount / averageMonths;
                    Console.WriteLine($"Interest earned on investment account {AccountNumber}:E£{monthlyinterest:F2}. New balance:E£{(Balance+monthlyinterest):F2}");
                    Console.WriteLine($"Account {AccountNumber} balance:E£{(Balance+monthlyinterest):F2}");
                }
                else
                {
                    Console.WriteLine("Invalid end date format. Exiting interest calculation.");
                }
            }
            else
            {
                Console.WriteLine("Invalid start date format. Exiting interest calculation.");
            }
        }

        public void ApplyInterest()
        {
            Console.WriteLine($"Interest earned on investment account {AccountNumber}: E£{monthlyinterest:F2}");
        }

        public decimal CalculateInterest()
        {
            // Placeholder for calculating interest, not implemented in this example
            return 0;
        }
    }

    class CheckingAccount : BankAccount
    {
        /* Constructor */
        public CheckingAccount(string name, decimal initialBalance)
            : base(name, initialBalance)
        {

        }

        public override void Deposit(decimal amount)
        {
            base.Deposit(amount);
        }

        public override void Withdraw(decimal amount)
        {
            base.Withdraw(amount);
        }

        public override void CheckBalance()
        {
            base.CheckBalance();
        }
    }

    class CreditAccount : BankAccount
    {
        private Decimal CreditLimit;

        /* Constructor */
        public CreditAccount(string name, decimal initialBalance, Decimal creditlimit)
            : base(name, initialBalance)
        {
            CreditLimit = creditlimit;
        }

        public override void Deposit(decimal amount)
        {
            base.Deposit(amount);
        }

        public override void Withdraw(decimal amount)
        {
            if (amount > 0)
            {
                if (Balance > amount)
                {
                    Balance -= amount;
                    Console.WriteLine($"Withdrawn E£{amount:F2} from account {AccountNumber} using Balance. Balance: E£{Balance:F2}, Credit: E£{CreditLimit:F2} ");
                }
                else if (amount > Balance)
                {
                    if (amount < CreditLimit)
                    {
                        CreditLimit -= amount;
                        Console.WriteLine($"Withdrawn E£{amount:F2} from account {AccountNumber} using credit. Balance: E£{Balance:F2}, Credit: E£{CreditLimit:F2} ");
                    }
                    else
                        Console.WriteLine("ERROR, invalid transaction");
                }
            }
            else
                Console.WriteLine("ERROR, please enter a valid amount");
        }

        public override void CheckBalance()
        {
            base.CheckBalance();
        }
    }

    class SavingsAccount : BankAccount, IInterestEarning
    {
        public decimal Interestrate = 0.03m; // 3%
        public decimal interestamount = 0;

        /* Constructor */
        public SavingsAccount(string name, decimal initialBalance)
            : base(name, initialBalance)
        {

        }
        public override void Deposit(decimal amount)
        {
            base.Deposit(amount);
        }

        public override void Withdraw(decimal amount)
        {
            base.Withdraw(amount);
        }

        public override void CheckBalance()
        {
            base.CheckBalance();
        }

        public void ApplyInterest()
        {
            Console.WriteLine($"Interest earned on investment account {AccountNumber} :{interestamount:F2} New balance: {Balance:F2}"); //000000
        }

        public decimal CalculateInterest()
        {
            interestamount = Balance * Interestrate;
            return interestamount;
        }

        public void CalculateInterestBetweenDates()
        {
            Console.WriteLine("Calculating interest between two dates");
            Console.WriteLine("Please insert start date: (dd/MM/yyyy)");

            if (DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime startDate))
            {
                Console.WriteLine("Please insert end date: (dd/MM/yyyy)");

                if (DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime endDate))
                {
                    double months = (int)endDate.Subtract(startDate).Days / (365.25 / 12);
                    int averageMonths = (int)months;
                    Console.Write($"Total interest earned in {averageMonths} months: ");
                    interestamount = Balance * Interestrate * averageMonths;
                    Console.WriteLine($"E£{interestamount:F2}");
                }
                else
                {
                    Console.WriteLine("Invalid end date format. Exiting interest calculation.");
                }
            }
            else
            {
                Console.WriteLine("Invalid start date format. Exiting interest calculation.");
            }
        }
    }

    public static void Main()
    {
        List<BankAccount> accounts = new List<BankAccount>();

        try
        {
            Console.WriteLine("Enter details for Savings Account (HolderName, Balance):");
            string holderName = Console.ReadLine();
            decimal initialBalance;
            if (decimal.TryParse(Console.ReadLine(), out initialBalance))
            {
                SavingsAccount savingsAccount = new SavingsAccount(holderName, initialBalance);
                accounts.Add(savingsAccount);
            }
            else
            {
                Console.WriteLine("Invalid input for balance. Skipping Savings Account creation.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating Savings Account: {ex.Message}");
        }

        try
        {
            Console.WriteLine("Enter details for Checking Account (HolderName, Balance):");
            string holderName = Console.ReadLine();
            decimal initialBalance;
            if (decimal.TryParse(Console.ReadLine(), out initialBalance))
            {
                CheckingAccount checkingAccount = new CheckingAccount(holderName, initialBalance);
                accounts.Add(checkingAccount);
            }
            else
            {
                Console.WriteLine("Invalid input for balance. Skipping Checking Account creation.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating Checking Account: {ex.Message}");
        }

        try
        {
            Console.WriteLine("Enter details for Investment Account (HolderName, Balance):");
            string holderName = Console.ReadLine();
            decimal initialBalance;
            if (decimal.TryParse(Console.ReadLine(), out initialBalance))
            {
                InvestmentAccount investmentAccount = new InvestmentAccount(holderName, initialBalance);
                accounts.Add(investmentAccount);
            }
            else
            {
                Console.WriteLine("Invalid input for balance. Skipping Investment Account creation.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating Investment Account: {ex.Message}");
        }

        try
        {
            Console.WriteLine("Enter details for Credit Account (HolderName, Balance, Credit Limit):");
            string holderName = Console.ReadLine();
            decimal initialBalance;
            if (decimal.TryParse(Console.ReadLine(), out initialBalance))
            {
                decimal creditLimit;
                if (decimal.TryParse(Console.ReadLine(), out creditLimit))
                {
                    CreditAccount creditAccount = new CreditAccount(holderName, initialBalance, creditLimit);
                    accounts.Add(creditAccount);
                }
                else
                {
                    Console.WriteLine("Invalid input for credit limit. Skipping Credit Account creation.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input for balance. Skipping Credit Account creation.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating Credit Account: {ex.Message}");
        }

        foreach (var account in accounts)
        {
            Console.WriteLine($"\nAccount Type: {account.GetType().Name}");
            Console.WriteLine($"AccountHolder: {account.AccountHolderName}\n");
            Console.WriteLine($"Account {account.AccountNumber} balance: E£{account.Balance:F2}");
            Console.WriteLine("Please enter amount deposited:");
            decimal depositAmount;
            if (decimal.TryParse(Console.ReadLine(), out depositAmount))
            {
                try
                {
                    account.Deposit(depositAmount);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error depositing amount: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid input for deposit amount.");
            }

            Console.WriteLine("Please enter amount withdrawn:");
            decimal withdrawAmount;
            if (decimal.TryParse(Console.ReadLine(), out withdrawAmount))
            {
                try
                {
                    account.Withdraw(withdrawAmount);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error withdrawing amount: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid input for withdrawal amount.");
            }

            if (account is IInterestEarning interestAccount)
            {
                interestAccount.CalculateInterestBetweenDates();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Commercial_System
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Initialize DB before launching the form
            DatabaseInitializer.Initialize();

            Application.Run(new CustomerTransactionForm());
        }

        public static class DatabaseInitializer
        {
            private static string connectionString = "Data Source=.;Initial Catalog=CommercialSystemDb;Integrated Security=True";

            public static void Initialize()
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    using (SqlTransaction transaction = con.BeginTransaction())
                    {
                        try
                        {
                            string createCustomerDetails = @"
                            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='CustomerDetails' AND xtype='U')
                            CREATE TABLE CustomerDetails (
                                AccountNumber NVARCHAR(50) PRIMARY KEY,
                                CustomerName NVARCHAR(100),
                                Balance DECIMAL(18,2)
                            )";
                            using (SqlCommand cmd = new SqlCommand(createCustomerDetails, con, transaction))
                                cmd.ExecuteNonQuery();

                            string createTransactions = @"
                            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='CustomerTransactions' AND xtype='U')
                            CREATE TABLE CustomerTransactions (
                                TransactionID INT IDENTITY(1,1) PRIMARY KEY,
                                AccountNumber NVARCHAR(50),
                                TransactionDate DATETIME,
                                Reference NVARCHAR(100),
                                Amount DECIMAL(18,2),
                                DebitCreditIndicator NVARCHAR(10),
                                FOREIGN KEY (AccountNumber) REFERENCES CustomerDetails(AccountNumber)
                            )";
                            using (SqlCommand cmd = new SqlCommand(createTransactions, con, transaction))
                                cmd.ExecuteNonQuery();

                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
        }

    }
}

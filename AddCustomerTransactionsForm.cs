using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Commercial_System
{
    public partial class AddCustomerTransactionsForm : Form
    {
        private string accountNumber; // the customer to add transaction for
        private string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

        public AddCustomerTransactionsForm(string selectedAccountNumber)
        {
            InitializeComponent();
            accountNumber = selectedAccountNumber;
        }

        private void AddCustomerTransactionsForm_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Today;
            rbDebit.Checked = true; // default selection
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTransactionReference.Text))
            {
                MessageBox.Show("Please enter a Transaction Reference.");
                return;
            }

            decimal amount = numTransactionAmount.Value;
            if (amount <= 0)
            {
                MessageBox.Show("Transaction Amount must be greater than zero.");
                return;
            }

            string debitCredit = rbDebit.Checked ? "D" : "C";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Insert into CustomerTransactions
                    string insertSql = @"
                        INSERT INTO CustomerTransactions
                        (AccountNumber, TransactionDate, Reference, Amount, DebitCreditIndicator)
                        VALUES (@AccountNumber, @TransactionDate, @Reference, @Amount, @DebitCredit)";

                    using (SqlCommand cmdInsert = new SqlCommand(insertSql, conn, transaction))
                    {
                        cmdInsert.Parameters.AddWithValue("@AccountNumber", accountNumber);
                        cmdInsert.Parameters.AddWithValue("@TransactionDate", dateTimePicker1.Value.Date);
                        cmdInsert.Parameters.AddWithValue("@Reference", txtTransactionReference.Text.Trim());
                        cmdInsert.Parameters.AddWithValue("@Amount", amount);
                        cmdInsert.Parameters.AddWithValue("@DebitCredit", debitCredit);
                        cmdInsert.ExecuteNonQuery();
                    }

                    // Update balance in CustomerDetails (handle NULL with ISNULL)
                    string updateSql;
                    if (debitCredit == "D")
                    {
                        updateSql = @"
                            UPDATE CustomerDetails
                            SET Balance = ISNULL(Balance, 0) + @Amount
                            WHERE AccountNumber = @AccountNumber";
                    }
                    else
                    {
                        updateSql = @"
                            UPDATE CustomerDetails
                            SET Balance = ISNULL(Balance, 0) - @Amount
                            WHERE AccountNumber = @AccountNumber";
                    }

                    using (SqlCommand cmdUpdate = new SqlCommand(updateSql, conn, transaction))
                    {
                        cmdUpdate.Parameters.AddWithValue("@Amount", amount);
                        cmdUpdate.Parameters.AddWithValue("@AccountNumber", accountNumber);
                        int rowsUpdated = cmdUpdate.ExecuteNonQuery();

                        if (rowsUpdated == 0)
                        {
                            throw new Exception("Customer not found or balance update failed.");
                        }
                    }

                    transaction.Commit();
                    MessageBox.Show("Transaction saved and balance updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Error saving transaction: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}

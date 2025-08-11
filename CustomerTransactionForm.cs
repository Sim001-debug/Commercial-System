using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Commercial_System
{
    public partial class CustomerTransactionForm : Form
    {
        private string connectionString;
        private bool isEditing = false;

        public CustomerTransactionForm()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
            dataGridView1.AllowUserToOrderColumns = true;
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.Automatic;
            }
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT AccountNumber, CustomerName FROM CustomerDetails";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading customers: " + ex.Message);
                }
            }
        }

        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            isEditing = false;
            txtAccountNumber.ReadOnly = false;
        }

        private void btnEditCustomer_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                isEditing = true;

                txtAccountNumber.Text = dataGridView1.SelectedRows[0].Cells["AccountNumber"].Value.ToString();
                txtCustomerName.Text = dataGridView1.SelectedRows[0].Cells["CustomerName"].Value.ToString();

                txtAccountNumber.ReadOnly = true; // AccountNumber is key, don’t allow editing it
            }
            else
            {
                MessageBox.Show("Please select a customer to edit.");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAccountNumber.Text) || string.IsNullOrWhiteSpace(txtCustomerName.Text))
            {
                MessageBox.Show("Please fill in both Account Number and Customer Name.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    if (isEditing)
                    {
                        string updateQuery = "UPDATE CustomerDetails SET CustomerName = @CustomerName WHERE AccountNumber = @AccountNumber";
                        SqlCommand cmd = new SqlCommand(updateQuery, conn);
                        cmd.Parameters.AddWithValue("@CustomerName", txtCustomerName.Text);
                        cmd.Parameters.AddWithValue("@AccountNumber", txtAccountNumber.Text);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                            MessageBox.Show("Customer updated successfully.");
                        else
                            MessageBox.Show("No matching customer found to update.");
                    }
                    else
                    {
                        string insertQuery = "INSERT INTO CustomerDetails (AccountNumber, CustomerName) VALUES (@AccountNumber, @CustomerName)";
                        SqlCommand cmd = new SqlCommand(insertQuery, conn);
                        cmd.Parameters.AddWithValue("@AccountNumber", txtAccountNumber.Text);
                        cmd.Parameters.AddWithValue("@CustomerName", txtCustomerName.Text);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Customer added successfully.");
                    }

                    LoadCustomers();
                    ClearForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving customer: " + ex.Message);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            txtAccountNumber.Clear();
            txtCustomerName.Clear();
            txtAccountNumber.ReadOnly = false;
            isEditing = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a customer to delete.");
                return;
            }

            // Get the AccountNumber of the selected row
            string accountNumber = dataGridView1.SelectedRows[0].Cells["AccountNumber"].Value.ToString();

            // Confirm deletion with the user
            var confirmResult = MessageBox.Show($"Are you sure you want to delete customer with Account Number '{accountNumber}'?",
                                                "Confirm Delete",
                                                MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string deleteQuery = "DELETE FROM CustomerDetails WHERE AccountNumber = @AccountNumber";
                        SqlCommand cmd = new SqlCommand(deleteQuery, conn);
                        cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Customer deleted successfully.");
                            LoadCustomers();
                            ClearForm();
                        }
                        else
                        {
                            MessageBox.Show("No matching customer found to delete.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error deleting customer: " + ex.Message);
                    }
                }
            }
        }

        private void btnAddTransaction_Click(object sender, EventArgs e)
        {
            // Make sure a customer is selected
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a customer first.");
                return;
            }

            // Get selected customer's AccountNumber
            string selectedAccountNumber = dataGridView1.SelectedRows[0].Cells["AccountNumber"].Value.ToString();

            // Create and show AddCustomerTransactionsForm, passing the AccountNumber
            using (AddCustomerTransactionsForm addTransactionForm = new AddCustomerTransactionsForm(selectedAccountNumber))
            {
                if (addTransactionForm.ShowDialog() == DialogResult.OK)
                {
                    // Refresh your customer list after adding transaction to update balances
                    LoadCustomers();
                }
            }
        }

        private void btnSortCustomers_Click(object sender, EventArgs e)
        {
            SortCustomersForm sortForm = new SortCustomersForm();
            sortForm.ShowDialog();
        }

        private void btnCustomerEnquiries_Click(object sender, EventArgs e)
        {
            using (CustomerEnquiriesForm enquiriesForm = new CustomerEnquiriesForm())
            {
                enquiriesForm.ShowDialog();
            }
        }
    }
}

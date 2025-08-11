using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Commercial_System
{
    public partial class CustomerEnquiriesForm : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

        public CustomerEnquiriesForm()
        {
            InitializeComponent();
        }

        private void CustomerEnquiriesForm_Load(object sender, EventArgs e)
        {
            LoadCustomers();
        }

        private void LoadCustomers(string orderBy = "AccountNumber")
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = $"SELECT AccountNumber, CustomerName, Balance FROM CustomerDetails ORDER BY {orderBy}";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvCustomers.DataSource = dt;
                    btnViewTransactions.Enabled = false; // reset until user selects a customer
                    dgvTransactions.DataSource = null; // clear transactions grid
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading customers: " + ex.Message);
                }
            }
        }

        private void dgvCustomers_SelectionChanged(object sender, EventArgs e)
        {
            btnViewTransactions.Enabled = dgvCustomers.SelectedRows.Count > 0;
        }

        private void btnSortCustomers_Click(object sender, EventArgs e)
        {
            using (var sortForm = new SortCustomersForm())
            {
                if (sortForm.ShowDialog() == DialogResult.OK)
                {
                    LoadCustomers(sortForm.SelectedSortColumn);
                }
            }
        }

        private void btnViewTransactions_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a customer first.");
                return;
            }

            string accountNumber = dgvCustomers.SelectedRows[0].Cells["AccountNumber"].Value.ToString();
            LoadTransactions(accountNumber);
        }

        private void LoadTransactions(string accountNumber)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"
                        SELECT TransactionDate, Reference, Amount, DebitCreditIndicator 
                        FROM CustomerTransactions 
                        WHERE AccountNumber = @AccountNumber 
                        ORDER BY TransactionDate DESC";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvTransactions.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading transactions: " + ex.Message);
                }
            }
        }
    }
}

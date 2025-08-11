using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Commercial_System
{
    public partial class SortCustomersForm : Form
    {
        private string connectionString;
        public string SelectedSortColumn { get; private set; }


        public SortCustomersForm()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
        }

        private void SortCustomersForm_Load(object sender, EventArgs e)
        {
            // Add sorting options
            comboBoxSortBy.Items.Add("AccountNumber");
            comboBoxSortBy.Items.Add("CustomerName");
            comboBoxSortBy.Items.Add("Balance");

            comboBoxSortBy.SelectedIndex = 0; // Default selection

            LoadSortedData(comboBoxSortBy.SelectedItem.ToString());
        }

        private void btnSort_Click(object sender, EventArgs e)
        {
            if (comboBoxSortBy.SelectedItem != null)
            {
                SelectedSortColumn = comboBoxSortBy.SelectedItem.ToString();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void LoadSortedData(string sortField)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = $"SELECT AccountNumber, CustomerName, Balance FROM CustomerDetails ORDER BY {sortField} ASC";
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridViewCustomers.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }
    }
}

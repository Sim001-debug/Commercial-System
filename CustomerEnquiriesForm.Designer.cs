namespace Commercial_System
{
    partial class CustomerEnquiriesForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dgvCustomers;
        private System.Windows.Forms.DataGridView dgvTransactions;
        private System.Windows.Forms.Button btnSortCustomers;
        private System.Windows.Forms.Button btnViewTransactions;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dgvCustomers = new System.Windows.Forms.DataGridView();
            this.dgvTransactions = new System.Windows.Forms.DataGridView();
            this.btnSortCustomers = new System.Windows.Forms.Button();
            this.btnViewTransactions = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransactions)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvCustomers
            // 
            this.dgvCustomers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCustomers.Location = new System.Drawing.Point(50, 30);
            this.dgvCustomers.MultiSelect = false;
            this.dgvCustomers.Name = "dgvCustomers";
            this.dgvCustomers.ReadOnly = true;
            this.dgvCustomers.RowHeadersWidth = 51;
            this.dgvCustomers.RowTemplate.Height = 24;
            this.dgvCustomers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCustomers.Size = new System.Drawing.Size(450, 150);
            this.dgvCustomers.TabIndex = 0;
            this.dgvCustomers.SelectionChanged += new System.EventHandler(this.dgvCustomers_SelectionChanged);
            // 
            // dgvTransactions
            // 
            this.dgvTransactions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTransactions.Location = new System.Drawing.Point(50, 230);
            this.dgvTransactions.Name = "dgvTransactions";
            this.dgvTransactions.ReadOnly = true;
            this.dgvTransactions.RowHeadersWidth = 51;
            this.dgvTransactions.RowTemplate.Height = 24;
            this.dgvTransactions.Size = new System.Drawing.Size(700, 200);
            this.dgvTransactions.TabIndex = 1;
            // 
            // btnSortCustomers
            // 
            this.btnSortCustomers.Location = new System.Drawing.Point(520, 30);
            this.btnSortCustomers.Name = "btnSortCustomers";
            this.btnSortCustomers.Size = new System.Drawing.Size(120, 35);
            this.btnSortCustomers.TabIndex = 2;
            this.btnSortCustomers.Text = "Sort Customers";
            this.btnSortCustomers.UseVisualStyleBackColor = true;
            this.btnSortCustomers.Click += new System.EventHandler(this.btnSortCustomers_Click);
            // 
            // btnViewTransactions
            // 
            this.btnViewTransactions.Location = new System.Drawing.Point(520, 90);
            this.btnViewTransactions.Name = "btnViewTransactions";
            this.btnViewTransactions.Size = new System.Drawing.Size(120, 35);
            this.btnViewTransactions.TabIndex = 3;
            this.btnViewTransactions.Text = "View Transactions";
            this.btnViewTransactions.UseVisualStyleBackColor = true;
            this.btnViewTransactions.Enabled = false;
            this.btnViewTransactions.Click += new System.EventHandler(this.btnViewTransactions_Click);
            // 
            // CustomerEnquiriesForm
            // 
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnViewTransactions);
            this.Controls.Add(this.btnSortCustomers);
            this.Controls.Add(this.dgvTransactions);
            this.Controls.Add(this.dgvCustomers);
            this.Name = "CustomerEnquiriesForm";
            this.Text = "Customer Enquiries";
            this.Load += new System.EventHandler(this.CustomerEnquiriesForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransactions)).EndInit();
            this.ResumeLayout(false);
        }
    }
}

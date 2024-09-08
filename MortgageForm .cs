using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace UXI_Mortgage
{
    public partial class MortgageForm : Form
    {
        private Timer timer;
        public MortgageForm()
        {
            InitializeComponent();
            InitializeTimer();
            UpdateDateTime();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                // Validate Principal
                if (!double.TryParse(principalTextBox.Text, out double principal) || principal <= 0)
                {
                    MessageBox.Show("Please enter a valid positive number for the principal.", "Input Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validate Annual Rate
                if (!double.TryParse(interestRateTextBox.Text, out double annualRate) || annualRate < 0)
                {
                    MessageBox.Show("Please enter a valid non-negative number for the annual interest rate.", "Input Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validate Term Years
                if (!int.TryParse(termTextBox.Text, out int termYears) || termYears <= 0)
                {
                    MessageBox.Show("Please enter a valid positive integer for the term in years.", "Input Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                
                double monthlyPayment = MortgageCalculator.CalculateMonthlyPayment(principal, annualRate, termYears);
                double totalInterest = MortgageCalculator.CalculateTotalInterest(principal, annualRate, termYears);
                double totalAmountPaid = MortgageCalculator.CalculateTotalAmountPaid(principal, annualRate, termYears);

                monthlyPaymentLabel.Text = monthlyPayment.ToString("C2", CultureInfo.CurrentCulture);//$"Monthly Payment: {monthlyPayment:C}";
                totalInterestLabel.Text = totalInterest.ToString("C2", CultureInfo.CurrentCulture); // $"Total Interest: {totalInterest:C}";
                totalAmountPaidLabel.Text = totalAmountPaid.ToString("C2", CultureInfo.CurrentCulture); // $"Total Amount Paid: {totalAmountPaid:C}";

                // Generate amortization schedule
                var schedule = AmortizationSchedule.GenerateSchedule(principal, annualRate, termYears);
                DisplayAmortizationSchedule(schedule);
            }
            catch (Exception ex)
            {
                // Display error message
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            

        }


        private void DisplayAmortizationSchedule(List<AmortizationEntry> schedule)
        {
            // Configure DataGridView columns
            ConfigureDataGridView();

            // Add rows to DataGridView
            foreach (var entry in schedule)
            {
                dataGridView.Rows.Add(
                    entry.PaymentNumber,
                    entry.Payment,
                    entry.PrincipalPayment,
                    entry.InterestPayment,
                    entry.RemainingBalance
                );
            }
        }
        private void ConfigureDataGridView()
        {
            // Clear any existing columns and rows
            dataGridView.Columns.Clear();
            dataGridView.Rows.Clear();

            // Add columns
            dataGridView.Columns.Add("PaymentNumber", "Payment #");
            dataGridView.Columns.Add("Payment", "Payment Amount");
            dataGridView.Columns.Add("PrincipalPayment", "Principal Payment");
            dataGridView.Columns.Add("InterestPayment", "Interest Payment");
            dataGridView.Columns.Add("RemainingBalance", "Remaining Balance");

            // Set column formatting
            dataGridView.Columns["Payment"].DefaultCellStyle.Format = "C";
            dataGridView.Columns["PrincipalPayment"].DefaultCellStyle.Format = "C";
            dataGridView.Columns["InterestPayment"].DefaultCellStyle.Format = "C";
            dataGridView.Columns["RemainingBalance"].DefaultCellStyle.Format = "C";

            // Optionally, set column widths
            dataGridView.Columns["PaymentNumber"].Width = 80;
            dataGridView.Columns["Payment"].Width = 120;
            dataGridView.Columns["PrincipalPayment"].Width = 150;
            dataGridView.Columns["InterestPayment"].Width = 150;
            dataGridView.Columns["RemainingBalance"].Width = 150;

            // Set column header text alignment
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            
        }
        
        private void StyleDataGridView()
        {
            // Set header font and color
            dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;

            // Set alternating row colors
            dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.Beige;
            dataGridView.RowsDefaultCellStyle.BackColor = Color.White;

            // Set the grid color
            dataGridView.GridColor = Color.Black;
        }

        private void InitializeTimer()
        {
            timer = new Timer();
            timer.Interval = 1000; // 1 second
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateDateTime();
        }

        private void UpdateDateTime()
        {
            lblDateTime.Text = DateTime.Now.ToString("F");
        }

        private void termTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

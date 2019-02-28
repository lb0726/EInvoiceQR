using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MT.EInvoiceQR.Win
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void btnCreateQRCode_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                txtTitle.Focus();
                MessageBox.Show("请填写发票抬头");
                return;
            }
            Invoice t = new Invoice()
            {
                Title = txtTitle.Text.Trim(),
                TaxNumber = txtTaxNumber.Text.Trim(),                
                Address = txtAddress.Text.Trim(),
                Telephone = txtTelephone.Text.Trim(),
                BankName = txtBankName.Text.Trim(),
                BankAccount = txtBankAccount.Text.Trim(),             
            };
            string QRString = InvoiceQRCodeHelp.GetInvoiceQRCodeString(t);

            Image img = QRCode.GCode(QRString,3);

            picQRCode.Image = img; 
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InvoiceTotal
{
    // This is the starting point for exercise 8-1 from
    // "Murach's C# 2010" by Joel Murach
    // (c) 2010 by Mike Murach & Associates, Inc. 
    // www.murach.com

    public partial class frmInvoiceTotal : Form
	{
		public frmInvoiceTotal()
		{
			InitializeComponent();
		}

        // TODO: declare class variables for array and list here
        decimal[] totals = new decimal[5];
        int index = 0;

        private void btnCalculate_Click(object sender, EventArgs e)
		{
            try
            {

                if (txtSubtotal.Text == "")
                {
                    MessageBox.Show(
                        "Subtotal is a required field.", "Entry Error");
                }
                else
                {
                    decimal subtotal = Decimal.Parse(txtSubtotal.Text);
                    if (subtotal > 0 && subtotal < 10000)
                    {
                        decimal discountPercent = 0m;
                        if (subtotal >= 500)
                            discountPercent = .2m;
                        else if (subtotal >= 250 && subtotal < 500)
                            discountPercent = .15m;
                        else if (subtotal >= 100 && subtotal < 250)
                            discountPercent = .1m;
                        decimal discountAmount = subtotal * discountPercent;
                        decimal invoiceTotal = subtotal - discountAmount;

                        discountAmount = Math.Round(discountAmount, 2);
                        invoiceTotal = Math.Round(invoiceTotal, 2);

                        txtDiscountPercent.Text = discountPercent.ToString("p1");
                        txtDiscountAmount.Text = discountAmount.ToString();
                        txtTotal.Text = invoiceTotal.ToString();

                        // TODO:  Add invoice total to the array here
                        totals[index] = invoiceTotal;
                        index++;
                    }
                    else
                    {
                        MessageBox.Show(
                            "Subtotal must be greater than 0 and less than 10,000.",
                            "Entry Error");
                    }
                }
            }
            catch (FormatException)
            {
                MessageBox.Show(
                    "Please enter a valid number for the Subtotal field.",
                    "Entry Error");
            }
            catch (IndexOutOfRangeException)
            {
                //message used to say 3 values, but since our array allows 5 items, that preset message didn't make sense
                MessageBox.Show(
                    "You can only enter 5 values",
                    "Entry Error");
            }

            txtSubtotal.Focus();
        }

		private void btnExit_Click(object sender, EventArgs e)
		{
            // TODO: add code that displays dialog boxes here
            string totalsString = "";
            for (int i = 0; i < totals.Length; i++)
            {
                totalsString += totals[i] + "\n";
            }
            MessageBox.Show(totalsString, "Orders Totals");


            this.Close();
		}

	}
}
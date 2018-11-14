using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Web;
using System.Collections;
using System.Configuration;
using SweetSpotProShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;

namespace SweetShop
{
    public class SweetShopManager
    {
        private string connectionString;
        CustomerManager CM = new CustomerManager();
        EmployeeManager EM = new EmployeeManager();
        ItemDataUtilities idu = new ItemDataUtilities();
        LocationManager lm = new LocationManager();

        public SweetShopManager()
        {
            connectionString = ConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString;
        }
        /*******Invoice Utilities************************************************************************************/
        public Invoice getSingleInvoice(int invoiceID, int invoiceSub)
        {
            SqlConnection con = new SqlConnection(connectionString);
            InvoiceItemsManager IIM = new InvoiceItemsManager();
            InvoiceMOPsManager IMM = new InvoiceMOPsManager();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT invoiceNum, invoiceSubNum, invoiceDate, Cast(invoiceTime as DATETIME) as invoiceTime, custID, empID, locationID, subTotal, shippingAmount, "
                + "discountAmount, tradeinAmount, CASE WHEN chargeGST = 1 THEN governmentTax ELSE 0 END AS governmentTax, "
                + "CASE WHEN chargePST = 1 THEN provincialTax ELSE 0 END AS provincialTax, balanceDue, transactionType, comments FROM tbl_invoice "
                + "WHERE invoiceNum = @invoiceNum and invoiceSubNum = @invoiceSubNum";
            cmd.Parameters.AddWithValue("invoiceNum", invoiceID);
            cmd.Parameters.AddWithValue("invoiceSubNum", invoiceSub);
            cmd.Connection = con;
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            Invoice i = new Invoice();
            while (reader.Read())
            {
                i = new Invoice(Convert.ToInt32(reader["invoiceNum"]), Convert.ToInt32(reader["invoiceSubNum"]), Convert.ToDateTime(reader["invoiceDate"]),
                    Convert.ToDateTime(reader["invoiceTime"]), CM.ReturnCustomer(Convert.ToInt32(reader["custID"]))[0], EM.ReturnEmployee(Convert.ToInt32(reader["empID"]))[0],
                    lm.ReturnLocation(Convert.ToInt32(reader["locationID"]))[0], Convert.ToDouble(reader["subTotal"]), Convert.ToInt32(reader["shippingAmount"]),
                    Convert.ToDouble(reader["discountAmount"]), Convert.ToDouble(reader["tradeinAmount"]), Convert.ToDouble(reader["governmentTax"]),
                    Convert.ToDouble(reader["provincialTax"]), Convert.ToDouble(reader["balanceDue"]), IIM.ReturnInvoiceItems(reader["invoiceNum"].ToString() + "-" + reader["invoiceSubNum"].ToString()),
                    IMM.ReturnInvoiceMOPs(reader["invoiceNum"].ToString() + "-" + reader["invoiceSubNum"].ToString()), Convert.ToInt32(reader["transactionType"]),
                    reader["comments"].ToString());
            }
            con.Close();
            //Returns the invoice
            return i;
        }
        //Get Items
        public List<Cart> invoice_getItems(int invoiceNum, int invoiceSubNum, string table)
        {
            List<Cart> items = new List<Cart>();
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select * FROM " + table + " Where invoiceNum = @invoiceNum and invoiceSubNum = @invoiceSubNum";
            cmd.Parameters.AddWithValue("invoiceNum", invoiceNum);
            cmd.Parameters.AddWithValue("invoiceSubNum", invoiceSubNum);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int sku = Convert.ToInt32(reader["sku"]);
                //items.Add(new Cart(sku, getDescription(sku, getItemType(sku)),
                //    Convert.ToInt32(reader["itemQuantity"]), Convert.ToDouble(reader["itemPrice"]),
                //    Convert.ToDouble(reader["itemCost"]), Convert.ToDouble(reader["itemDiscount"]),
                //    Convert.ToBoolean(reader["percentage"]), Convert.ToDouble(reader["itemRefund"]), false, 0));
            }
            conn.Close();
            //Returns list of the items
            return items;
        }
        //Get Checkout Totals
        public CheckoutManager invoice_getCheckoutTotals(int invoiceNum, int invoiceSubNum, string table)
        {
            CheckoutManager ckm = new CheckoutManager();
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select subTotal, shippingAmount, discountAmount, tradeinAmount, "
                + "CASE WHEN chargeGST = 1 THEN governmentTax ELSE 0 END AS governmentTax, "
                + "CASE WHEN chargePST = 1 THEN provincialTax ELSE 0 END AS provincialTax "
                + "FROM " + table + " Where invoiceNum = @invoiceNum and invoiceSubNum = @invoiceSubNum";
            cmd.Parameters.AddWithValue("invoiceNum", invoiceNum);
            cmd.Parameters.AddWithValue("invoiceSubNum", invoiceSubNum);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                double gst = Convert.ToDouble(reader["governmentTax"]);
                double pst = Convert.ToDouble(reader["provincialTax"]);
                bool isGST = false;
                bool isPST = false;
                if (gst != 0)
                {
                    isGST = true;
                }
                if (pst != 0)
                {
                    isPST = true;
                }
                double shipping;
                try
                {
                    shipping = Convert.ToDouble(reader["shippingAmount"]);
                }
                catch (Exception ex)
                {
                    shipping = 0;
                }
                ckm = new CheckoutManager(Convert.ToDouble(reader["discountAmount"]), Convert.ToDouble(reader["tradeinAmount"]),
                    shipping, isGST, isPST, gst, pst, 0, Convert.ToDouble(reader["subTotal"]));
            }
            conn.Close();
            //Returns checkout totals
            return ckm;
        }
        //Get Methods of Payment
        public List<Mops> invoice_getMOP(int invoiceNum, int invoiceSubNum, string table)
        {
            List<Mops> mops = new List<Mops>();
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select mopType, amountPaid FROM " + table + " Where invoiceNum = @invoiceNum and invoiceSubNum = @invoiceSubNum";
            cmd.Parameters.AddWithValue("invoiceNum", invoiceNum);
            cmd.Parameters.AddWithValue("invoiceSubNum", invoiceSubNum);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                mops.Add(new Mops(reader["mopType"].ToString(), Convert.ToDouble(reader["amountPaid"])));
            }
            conn.Close();
            //Returns the methods of payment
            return mops;
        }
    }
}
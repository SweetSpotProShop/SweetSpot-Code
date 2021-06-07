<%@ Page Title="" Language="C#" MasterPageFile="~/ReportingMPage.Master" AutoEventWireup="true" CodeBehind="ReportsExtensiveInvoice.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.ReportsExtensiveInvoice" %>

<asp:Content ID="ReportsPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <div id="extensiveInvoice" class="yesPrint">
        <h2>Extensive Invoice Report</h2>
        <hr />
        <div>
            <asp:Label ID="lblDates" runat="server" Font-Bold="true" />
        </div>
        <hr />

        <asp:GridView ID="GrdInvoices" runat="server" AutoGenerateColumns="false" ShowFooter="true" OnRowDataBound="GrdInvoices_RowDataBound" 
			AlternatingRowStyle-BackColor="WhiteSmoke" Width="100%" OnRowCommand="GrdInvoices_RowCommand" FooterStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Center">
            <Columns>
                <asp:TemplateField HeaderText="Invoice">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnInvoiceNumber" runat="server" Text='<%#Eval("varInvoiceNumber")%>' CommandArgument='<%# Eval("intInvoiceID")%>' />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblTotals" runat="server" Text="Totals:" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Shipping">
                    <ItemTemplate>
                        <asp:Label ID="lblShipping" runat="server" Text='<%#Eval("fltShippingCharges", "{0:C}")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Trade-in Amount">
                    <ItemTemplate>
                        <asp:Label ID="lblTradeIn" runat="server" Text='<%#Eval("fltTotalTradeIn", "{0:C}")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Discount">
                    <ItemTemplate>
                        <asp:Label ID="lblDiscount" runat="server" Text='<%#Eval("fltTotalDiscount", "{0:C}")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Sub-Total">
                    <ItemTemplate>
                        <asp:Label ID="lblSubTotal" runat="server" Text='<%#Eval("fltSubTotal", "{0:C}")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
				<asp:TemplateField HeaderText="Total Sales">
                    <ItemTemplate>
                        <asp:Label ID="lblTotalSales" runat="server" Text='<%#Eval("fltTotalSales", "{0:C}")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Government Tax">
                    <ItemTemplate>
                        <asp:Label ID="lblGovernmentTax" runat="server" Text='<%#Eval("fltGovernmentTaxAmount", "{0:C}")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Provincial Tax">
                    <ItemTemplate>
                        <asp:Label ID="lblProvincialTax" runat="server" Text='<%#Eval("fltProvincialTaxAmount", "{0:C}")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
				<asp:TemplateField HeaderText="Liquor Tax">
                    <ItemTemplate>
                        <asp:Label ID="lblLiquorTax" runat="server" Text='<%#Eval("fltLiquorTaxAmount", "{0:C}")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Sales Dollars">
                    <ItemTemplate>
                        <asp:Label ID="lblSalesDollars" runat="server" Text='<%#Eval("fltSalesDollars", "{0:C}")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="COGS">
                    <ItemTemplate>
                        <asp:Label ID="lblCOGS" runat="server" Text='<%#Eval("fltCostofGoods", "{0:C}")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Revenue Earned">
                    <ItemTemplate>
                        <asp:Label ID="lblRevenue" runat="server" Text='<%#Eval("fltRevenueEarned", "{0:C}")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Profit Margin">
                    <ItemTemplate>
                        <asp:Label ID="lblProfitMargin" runat="server" Text='<%#(Convert.ToDouble(Eval("fltRevenueEarned")) / Convert.ToDouble(Eval("fltSalesDollars"))).ToString("P")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Customer Name">
                    <ItemTemplate>
                        <asp:Label ID="lblCustomer" runat="server" Text='<%#Eval("varCustomerName")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Employee Name">
                    <ItemTemplate>
                        <asp:Label ID="lblEmployee" runat="server" Text='<%#Eval("varEmployeeName")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Date">
                    <ItemTemplate>
                        <asp:Label ID="lblDate" runat="server" Text='<%#Eval("dtmInvoiceDate", "{0:dd/MMM/yy}")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <br />
    <hr />

    <asp:Button CssClass="noPrint" ID="btnPrint" runat="server" Text="Print Report" Width="200px" OnClientClick="CallPrint('extensiveInvoice');" />
    <asp:Button CssClass="noPrint" ID="BtnDownload" runat="server" Text="Download" Visible="true" Width="200px" OnClick="BtnDownload_Click" />
    <style media="print">
        .noPrint {
            display: none;
            /*margin-left: 0;*/
        }

        .yesPrint {
            display: inline-block !important;
            /* margin-right:100px;
           float: right;*/
            margin-left: 10px !important;
        }
    </style>
    <script>
        function CallPrint(strid) {
            var prtContent = document.getElementById(strid);
            var WinPrint = window.open('', '', 'letf=10,top=10,width="450",height="250",toolbar=1,scrollbars=1,status=0');
            WinPrint.document.write("<html><head><LINK rel=\"stylesheet\" type\"text/css\" href=\"css/print.css\" media=\"print\"><LINK rel=\"stylesheet\" type\"text/css\" href=\"css/print.css\" media=\"screen\"></head><body>");
            WinPrint.document.write(prtContent.innerHTML);
            WinPrint.document.write("</body></html>");
            WinPrint.document.close();
            WinPrint.focus();
            WinPrint.print();
            WinPrint.close();
            return false;
        }
    </script>
</asp:Content>

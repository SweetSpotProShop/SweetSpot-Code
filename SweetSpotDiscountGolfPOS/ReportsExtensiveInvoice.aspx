<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReportsExtensiveInvoice.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.ReportsExtensiveInvoice" %>

<asp:Content ID="ReportsPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <div id="extensiveInvoice" class="yesPrint">
        <h2>Extensive Invoice Report</h2>
        <hr />
        <div>
            <asp:Label ID="lblDates" runat="server" Font-Bold="true" />
        </div>
        <hr />

        <asp:GridView ID="grdInvoices" runat="server" AutoGenerateColumns="false" ShowFooter="true" OnRowDataBound="grdInvoices_RowDataBound" AlternatingRowStyle-BackColor="WhiteSmoke" Width="100%">
            <Columns>
                <asp:TemplateField HeaderText="Invoice">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnInvoiceNumber" runat="server" Text='<%#Eval("Invoice")%>' OnClick="lbtnInvoiceNumber_Click" />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblTotals" runat="server" Text="Totals:" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Shipping">
                    <ItemTemplate>
                        <asp:Label ID="lblShipping" runat="server" Text='<%#Eval("shippingAmount")%>' />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblShippingTotal" runat="server" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Trade-in Amount">
                    <ItemTemplate>
                        <asp:Label ID="lblTradeIn" runat="server" Text='<%#Eval("tradeinAmount")%>' />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblTradeInTotal" runat="server" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Discount">
                    <ItemTemplate>
                        <asp:Label ID="lblDiscount" runat="server" Text='<%#Eval("Total Discount")%>' />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblDiscountTotal" runat="server" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Pre-Tax">
                    <ItemTemplate>
                        <asp:Label ID="lblPreTax" runat="server" Text='<%#Eval("Pre-Tax")%>' />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblPreTaxTotal" runat="server" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Government Tax">
                    <ItemTemplate>
                        <asp:Label ID="lblGovernmentTax" runat="server" Text='<%#Eval("governmentTax")%>' />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblGovernmentTaxTotal" runat="server" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Provincial Tax">
                    <ItemTemplate>
                        <asp:Label ID="lblProvincialTax" runat="server" Text='<%#Eval("provincialTax")%>' />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblProvincialTaxTotal" runat="server" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Post-Tax">
                    <ItemTemplate>
                        <asp:Label ID="lblPostTax" runat="server" Text='<%#Eval("Post-Tax")%>' />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblPostTaxTotal" runat="server" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="COGS">
                    <ItemTemplate>
                        <asp:Label ID="lblCOGS" runat="server" Text='<%#Eval("COGS")%>' />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblCOGSTotal" runat="server" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Revenue Earned">
                    <ItemTemplate>
                        <asp:Label ID="lblRevenue" runat="server" Text='<%#Eval("Revenue Earned")%>' />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblRevenueTotal" runat="server" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Profit Margin">
                    <ItemTemplate>
                        <asp:Label ID="lblProfitMargin" runat="server" Text='<%#Eval("Profit Margin")%>' />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblProfitMarginTotal" runat="server" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Payment">
                    <ItemTemplate>
                        <asp:Label ID="lblPayment" runat="server" Text='<%#Eval("Payment")%>' />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblPaymentTotal" runat="server" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Customer Name">
                    <ItemTemplate>
                        <asp:Label ID="lblCustomer" runat="server" Text='<%#Eval("Customer Name")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Employee Name">
                    <ItemTemplate>
                        <asp:Label ID="lblEmployee" runat="server" Text='<%#Eval("Employee Name")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Date">
                    <ItemTemplate>
                        <asp:Label ID="lblDate" runat="server" Text='<%#Eval("invoiceDate") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <br />
    <hr />

    <asp:Button class="noPrint" ID="btnPrint" runat="server" Text="Print Report" Width="200px" OnClientClick="CallPrint('extensiveInvoice');" />
    <asp:Button class="noPrint" ID="btnDownload" runat="server" Text="Download" Visible="true" Width="200px" OnClick="btnDownload_Click" />
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

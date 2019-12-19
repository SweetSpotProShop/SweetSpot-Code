<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReportsStoreStats.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.ReportsStoreStats" %>
<asp:Content ID="ReportsPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <div id="print">
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

        <h2>Store Stats Report</h2>
        <hr />
        <div>
            <asp:Label ID="lblDates" runat="server" Font-Bold="true" />
        </div>
        <div>
            <asp:GridView ID="grdStats" runat="server" AutoGenerateColumns="false" ShowFooter="true" RowStyle-HorizontalAlign="Center" OnRowDataBound="grdStats_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="Grouped By">
                        <ItemTemplate>
							<asp:Label ID="lblSelection" runat="server" Text='<%#Eval("selection")%>' />
                            <%--<asp:Label ID="lblYear" runat="server" Text='<%#Eval("varMonthName") + " / " + Eval("dtmInvoiceYear")%>' Visible="false" />
							<asp:Label ID="lblSelectedDate" runat="server" Text='<%#Eval("dtmSelectedDate", "{0: dd/MMM/yy}")%>' Visible="false" />--%>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblTotals" runat="server" Text="Totals:" />
                        </FooterTemplate>
                    </asp:TemplateField>
					<asp:TemplateField HeaderText="Store">
                        <ItemTemplate>
                            <asp:Label ID="lblLocationName" runat="server" Text='<%#Eval("varLocationName")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="GST">
                        <ItemTemplate>
                            <asp:Label ID="lblGovernmentTax" runat="server" Text='<%#Eval("fltGovernmentTaxAmount", "{0:C}")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="PST">
                        <ItemTemplate>
                            <asp:Label ID="lblProvincialTax" runat="server" Text='<%#Eval("fltProvincialTaxAmount", "{0:C}")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
					<asp:TemplateField HeaderText="LCT">
                        <ItemTemplate>
                            <asp:Label ID="lblLiquorTax" runat="server" Text='<%#Eval("fltLiquorTaxAmount", "{0:C}")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Cost of Goods Sold">
                        <ItemTemplate>
                            <asp:Label ID="lblCostofGoods" runat="server" Text='<%#Eval("fltCostofGoods", "{0:C}")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
					<asp:TemplateField HeaderText="Sales Pre-Tax">
                        <ItemTemplate>
                            <asp:Label ID="lblSalesPreTax" runat="server" Text='<%#Eval("fltSubTotal", "{0:C}")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Profit Margin">
                        <ItemTemplate>
                            <asp:Label ID="lblProfitMargin" runat="server" Text='<%#Eval("fltProfitMargin", "{0:P}")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>                    
                    <asp:TemplateField HeaderText="Sales Dollars">
                        <ItemTemplate>
                            <asp:Label ID="lblSalesDollars" runat="server" Text='<%#Eval("fltSalesDollars", "{0:C}")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>        
        <hr />
        </div>
        <asp:Table runat="server">
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Button class="noPrint" ID="btnPrint" runat="server" Text="Print Report" Width="200px" OnClientClick="CallPrint('print');" />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Button class="noPrint" ID="btnDownload" runat="server" Text="Download" Visible="true" Width="200px" Onclick="btnDownload_Click"/>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
</asp:Content>
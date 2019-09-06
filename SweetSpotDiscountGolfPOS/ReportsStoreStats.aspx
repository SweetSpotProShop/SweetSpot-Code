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
                    <asp:TemplateField HeaderText="Year">
                        <ItemTemplate>
                            <asp:Label ID="lblYear" runat="server" Text='<%#Eval("dtmInvoiceYear")%>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblTotals" runat="server" Text="Totals:" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Month">
                        <ItemTemplate>
                            <asp:Label ID="lblC2" runat="server" Text='<%#Eval("varMonthName")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="C3">
                        <ItemTemplate>
                            <asp:Label ID="lblC3" runat="server" Text='<%#Eval("dtmSelectedDate", "{0: dd/MMM/yy}")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="City Name">
                        <ItemTemplate>
                            <asp:Label ID="lblCityName" runat="server" Text='<%#Eval("varCityName")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Government Tax">
                        <ItemTemplate>
                            <asp:Label ID="lblGovTax" runat="server" Text='<%#Eval("fltGovernmentTaxAmount", "{0:C}")%>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblGovTaxTotal" runat="server" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Provincial Tax">
                        <ItemTemplate>
                            <asp:Label ID="lblProvTax" runat="server" Text='<%#Eval("fltProvincialTaxAmount", "{0:C}")%>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblProvTaxTotal" runat="server" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Cost of Goods Sold">
                        <ItemTemplate>
                            <asp:Label ID="lblCOGS" runat="server" Text='<%#Eval("fltTotalCOGS", "{0:C}")%>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblCOGSTotal" runat="server" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Average Profit Margin">
                        <ItemTemplate>
                            <asp:Label ID="lblAverageProfitMargin" runat="server" Text='<%#Eval("fltAverageProfitMargin", "{0:P}")%>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblAverageProfitMarginTotal" runat="server" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Sales Pre-Tax">
                        <ItemTemplate>
                            <asp:Label ID="lblSalesPreTax" runat="server" Text='<%#Eval("fltSalesPreTax", "{0:C}")%>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblSalesPreTaxTotal" runat="server" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Sales Post-Tax">
                        <ItemTemplate>
                            <asp:Label ID="lblSalesPostTax" runat="server" Text='<%#Eval("fltSalesPostTax", "{0:C}")%>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblSalesPostTaxTotal" runat="server" />
                        </FooterTemplate>
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
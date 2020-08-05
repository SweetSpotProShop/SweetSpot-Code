<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReportsSales.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.ReportsSales" %>

<asp:Content ID="ReportsPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
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
    <div id="print">
        <h2>Sales Report By Date</h2>
        <hr />
        <div>
            <asp:Label ID="lblDates" runat="server" Font-Bold="true" />
        </div>
        <hr />
        <div>
<<<<<<< Updated upstream
            <asp:GridView ID="grdSalesByDate" runat="server" AutoGenerateColumns="false" Width="60%" RowStyle-HorizontalAlign="Center" ShowFooter="true" OnRowDataBound="grdSalesByDate_RowDataBound">
=======
            <asp:GridView ID="GrdSalesByDate" runat="server" AutoGenerateColumns="false" Width="60%" RowStyle-HorizontalAlign="Center" ShowFooter="true" 
				OnRowDataBound="GrdSalesByDate_RowDataBound" FooterStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Center">
>>>>>>> Stashed changes
                <Columns>
                    <asp:TemplateField HeaderText="Date">
                        <ItemTemplate>
                            <asp:Label ID="lblDate" runat="server" Text='<%# Eval("dtmInvoiceDate", "{0:dd/MMM/yy}")%>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblTotal" runat="server" Text="Totals:" />
                        </FooterTemplate>
                    </asp:TemplateField>
					<asp:TemplateField HeaderText="Sales Dollars">
                        <ItemTemplate>
                            <asp:Label ID="lblSalesDollars" runat="server" Text='<%# Eval("fltSalesDollars", "{0:C}") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
					<asp:TemplateField HeaderText="GST">
                        <ItemTemplate>
                            <asp:Label ID="lblGSTAmount" runat="server" Text='<%# Eval("fltGovernmentTaxAmount", "{0:C}") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
					<asp:TemplateField HeaderText="PST">
                        <ItemTemplate>
                            <asp:Label ID="lblPSTAmount" runat="server" Text='<%# Eval("fltProvincialTaxAmount", "{0:C}") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
					<asp:TemplateField HeaderText="LCT">
                        <ItemTemplate>
                            <asp:Label ID="lblLCTAmount" runat="server" Text='<%# Eval("fltLiquorTaxAmount", "{0:C}") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Total Sales">
                        <ItemTemplate>
                            <asp:Label ID="lblTotalSales" runat="server" Text='<%# Eval("fltTotalSales", "{0:C}") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <hr />
    </div>
    <asp:Table runat="server">
        <asp:TableRow>
            <asp:TableCell>
                <asp:Button class="noPrint" ID="btnPrint" runat="server" Text="Print Report" Width="200px" OnClientClick="CallPrint('print');" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:Button class="noPrint" ID="btnDownload" runat="server" Text="Download" Visible="true" Width="200px" OnClick="btnDownload_Click" />
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Content>
<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReportsCOGSvsPM.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.ReportsCOGSvsPM" %>

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
        <h2>Cost of Goods Sold and Profit Margin</h2>
        <hr />
        <asp:Label ID="lblDates" runat="server" Font-Bold="true" />
        <hr />

        <asp:GridView ID="GrdInvoiceSelection" runat="server" AutoGenerateColumns="false" ShowFooter="true" Width="100%" RowStyle-HorizontalAlign="Center" OnRowDataBound="GrdInvoiceSelection_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="Invoice Number" HeaderStyle-Width="20%">
                    <ItemTemplate>
                        <asp:LinkButton ID="LbtnInvoiceNumber" runat="server" Text='<%#Eval("invoice")%>' OnClick="LbtnInvoiceNumber_Click" />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblTotal" runat="server" Text="Totals:" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Total Price" HeaderStyle-Width="20%">
                    <ItemTemplate>
                        <asp:Label ID="lblTotalPrice" runat="server" Text='<%#Eval("totalPrice","{0:C}") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Total Cost" HeaderStyle-Width="20%">
                    <ItemTemplate>
                        <asp:Label ID="lblTotalCost" runat="server" Text='<%#Eval("totalCost","{0:C}") %>' />
                    </ItemTemplate>
                </asp:TemplateField>                
                <asp:TemplateField HeaderText="Total Discount" HeaderStyle-Width="20%">
                    <ItemTemplate>
                        <asp:Label ID="lblTotalDiscount" runat="server" Text='<%#Eval("totalDiscount") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Discount as Percent" Visible="false" HeaderStyle-Width="20%">
                    <ItemTemplate>
                        <asp:Label ID="lblPercentage" runat="server" Text='<%#Eval("percentage") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Profit Margin" HeaderStyle-Width="20%">
                    <ItemTemplate>
                        <asp:Label ID="lblTotalProfit" runat="server" Text='<%#string.Concat(Eval("totalProfit","{0:f2}"), "%") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <br />
        <hr />
    </div>
    <asp:Table runat="server">
        <asp:TableRow>
            <asp:TableCell>
                <asp:Button CssClass="noPrint" ID="btnPrint" runat="server" Text="Print Report" Width="200px" OnClientClick="CallPrint('print');" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:Button CssClass="noPrint" ID="BtnDownload" runat="server" Text="Download" Visible="true" Width="200px" OnClick="BtnDownload_Click" />
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <script>
        function printReport(printable) {
            window.print();
        }
    </script>
</asp:Content>

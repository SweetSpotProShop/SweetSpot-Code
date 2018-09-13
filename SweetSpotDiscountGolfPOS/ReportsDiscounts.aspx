<%@ Page Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReportsDiscounts.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.ReportDiscounts" %>

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
    <asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="btnPrint">
        <div id="print">
            <h2>Discount Report</h2>
            <br />
            <hr />
            <asp:Label ID="lblReportDate" runat="server" Font-Bold="true" Text="Date"></asp:Label>
            <hr />
            <asp:GridView ID="grdInvoiceDisplay" runat="server" AutoGenerateColumns="false" Width="100%" ShowFooter="true" RowStyle-HorizontalAlign="Center" OnRowDataBound="grdInvoiceDisplay_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="Invoice Number" HeaderStyle-Width="16.6666666667%">
                        <ItemTemplate>
                            <asp:Label ID="lblInvoiceNum" runat="server" Text='<%#Eval("invoiceNum") + "-" + Eval("invoiceSubNum") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblTotal" runat="server" Text="Totals:"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Invoice Date" HeaderStyle-Width="16.6666666667%">
                        <ItemTemplate>
                            <asp:Label ID="lblInvoiceDate" runat="server" Text='<%#Eval("invoiceDate","{0: dd/MMM/yy}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Customer Name" HeaderStyle-Width="16.6666666667%">
                        <ItemTemplate>
                            <asp:Label ID="lblCustomerName" runat="server" Text='<%#Eval("customerName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Discount" HeaderStyle-Width="16.6666666667%">
                        <ItemTemplate>
                            <asp:Label ID="lblDiscountAmount" runat="server" Text='<%#Eval("discountAmount","{0:C}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Balance Due" HeaderStyle-Width="16.6666666667%">
                        <ItemTemplate>
                            <asp:Label ID="lblBalanceDue" runat="server" Text='<%#Eval("balanceDue","{0:C}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Employee Name" HeaderStyle-Width="16.6666666667%">
                        <ItemTemplate>
                            <asp:Label ID="lblEmployeeName" runat="server" Text='<%#Eval("employeeName") %>'></asp:Label>
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
                    <asp:Button class="noPrint" ID="btnPrint" runat="server" Text="Print Report" Width="200px" OnClientClick="CallPrint('print');" />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Button class="noPrint" ID="btnDownload" runat="server" Text="Download" Visible="true" Width="200px" OnClick="btnDownload_Click" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </asp:Panel>
    <script>
        function printReport(printable) {
            window.print();
        }
    </script>
</asp:Content>

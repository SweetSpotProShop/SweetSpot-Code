<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReportsExtensiveInvoice.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.ReportsExtensiveinvoice" %>

<asp:Content ID="ReportsPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <div class="yesPrint">
        <h2>Extensive Invoice Report</h2>
        <hr />
        <div>
            <asp:Label ID="lblDates" runat="server" Text="lblDates" Font-Bold="true"></asp:Label>
        </div>
        <hr />
        <div>
            <asp:GridView ID="grdInvoices" runat="server" Width="75%" AutoGenerateColumns="false" ShowFooter="false" >
                <Columns>
                    <asp:BoundField HeaderText="Invoice" HeaderStyle-Width="20%" DataField="Invoice" />
                    <asp:BoundField HeaderText="Shipping" HeaderStyle-Width="20%" DataField="shippingAmount" DataFormatString="{0:d}" />
                    <asp:BoundField HeaderText="Total Discount" HeaderStyle-Width="20%" DataField="Total Discount" DataFormatString="{0:d}" />
                    <asp:BoundField HeaderText="Pre-Tax" HeaderStyle-Width="20%" DataField="Pre-Tax" DataFormatString="{0:d}" />
                    <asp:BoundField HeaderText="Government Tax" HeaderStyle-Width="20%" DataField="governmentTax" DataFormatString="{0:d}" />
                    <asp:BoundField HeaderText="Provincial Tax" HeaderStyle-Width="20%" DataField="provincialTax" DataFormatString="{0:d}" />
                    <asp:BoundField HeaderText="Post-Tax" HeaderStyle-Width="20%" DataField="Post-Tax" DataFormatString="{0:d}" />
                    <asp:BoundField HeaderText="COGS" HeaderStyle-Width="20%" DataField="COGS" DataFormatString="{0:d}" />
                    <asp:BoundField HeaderText="Revenue Earned" HeaderStyle-Width="20%" DataField="Revenue Earned" DataFormatString="{0:d}" />
                    <asp:BoundField HeaderText="Profit Margin" HeaderStyle-Width="20%" DataField="Profit Margin" />
                    <asp:BoundField HeaderText="Customer" HeaderStyle-Width="20%" DataField="Customer Name" />
                    <asp:BoundField HeaderText="Employee" HeaderStyle-Width="20%" DataField="Employee Name" />
                    <asp:BoundField HeaderText="Location" HeaderStyle-Width="20%" DataField="Location" />
                    <asp:BoundField HeaderText="Date" HeaderStyle-Width="20%" DataField="invoiceDate" />
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <hr />
    </div>
    <asp:Button class="noPrint" ID="btnPrint" runat="server" Text="Print Report" Width="200px" OnClientClick="CallPrint('Purchases');" />
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
        function printReport(printable) {
            window.print();
        }
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


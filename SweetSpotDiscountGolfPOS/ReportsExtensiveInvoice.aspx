<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReportsExtensiveInvoice.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.ReportsExtensiveInvoice" %>

<asp:Content ID="ReportsPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <div id="extensiveInvoice" class="yesPrint">
        <h2>Extensive Invoice Report</h2>
        <hr />
        <div>
            <asp:Label ID="lblDates" runat="server" Text="lblDates" Font-Bold="true"></asp:Label>
        </div>
        <hr />
        <div>
            <asp:GridView ID="grdInvoices" runat="server"  AutoGenerateColumns="false" ShowFooter="false" OnRowDataBound="grdInvoices_RowDataBound">
                <Columns>
                    <asp:BoundField HeaderText="Invoice"  DataField="Invoice" />
                    <asp:BoundField HeaderText="Shipping"  DataField="shippingAmount"  />
                    <asp:BoundField HeaderText="Total Discount"  DataField="Total Discount"  />
                    <asp:BoundField HeaderText="Pre-Tax"  DataField="Pre-Tax"  />
                    <asp:BoundField HeaderText="Government Tax"  DataField="governmentTax"  />
                    <asp:BoundField HeaderText="Provincial Tax"  DataField="provincialTax"  />
                    <asp:BoundField HeaderText="Post-Tax"  DataField="Post-Tax"  />
                    <asp:BoundField HeaderText="COGS"  DataField="COGS"  />
                    <asp:BoundField HeaderText="Revenue Earned"  DataField="Revenue Earned"  />
                    <asp:BoundField HeaderText="Profit Margin"  DataField="Profit Margin" />
                    <asp:BoundField HeaderText="Customer"  DataField="Customer Name" />
                    <asp:BoundField HeaderText="Employee"  DataField="Employee Name" />
                    <asp:BoundField HeaderText="Location"  DataField="Location" />
                    <asp:BoundField HeaderText="Date"  DataField="invoiceDate" />
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <hr />
    </div>
    <asp:Button class="noPrint" ID="btnPrint" runat="server" Text="Print Report" Width="200px" OnClientClick="CallPrint('extensiveInvoice');" />
    <%--<asp:Button class="noPrint" ID="btnDownload" runat="server" Text="Download" Visible="true" Width="200px" OnClick="btnDownload_Click" />--%>
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


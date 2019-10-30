<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReportsTaxes.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.ReportsTaxes" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SPMaster" runat="server">
</asp:Content>--%>

<asp:Content ID="ReportsTaxesPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
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
    <link href="MainStyleSheet.css" rel="stylesheet" type="text/css" />
    <div id="Taxes" class="yesPrint">
        <h2>Taxes</h2>
        <hr />
        <%--Taxes Breakdown--%>
        <asp:Label ID="lblTaxDate" Font-Bold="true" runat="server" />
        <hr />
        <h3>Sales</h3>
        <div>
            <asp:GridView ID="grdTaxesCollected" runat="server" Width="40%" AutoGenerateColumns="false" ShowFooter="true" OnRowDataBound="grdTaxesCollected_RowDataBound">
                <Columns>
                    <asp:BoundField HeaderText="Date" HeaderStyle-Width="33%" DataField="dtmInvoiceDate" FooterText="Totals:" DataFormatString="{0:d}"/>
                    <asp:BoundField HeaderText="GST" HeaderStyle-Width="33%" DataField="fltGovernmentTaxAmount" DataFormatString="{0:C}" />
                    <asp:BoundField HeaderText="PST" HeaderStyle-Width="33%" DataField="fltProvincialTaxAmount" DataFormatString="{0:C}" />
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <hr />
        <h3>Returns</h3>
        <div>
            <asp:GridView ID="grdTaxesReturned" runat="server" Width="40%" AutoGenerateColumns="false" ShowFooter="true" OnRowDataBound="grdTaxesReturned_RowDataBound">
                <Columns>
                    <asp:BoundField HeaderText="Date" DataField="dtmInvoiceDate" FooterText="Totals:" DataFormatString="{0:d}"/>
                    <asp:BoundField HeaderText="GST" DataField="fltGovernmentTaxAmount" DataFormatString="{0:C}" />
                    <asp:BoundField HeaderText="PST" DataField="fltProvincialTaxAmount" DataFormatString="{0:C}" />
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <hr />
        <h3>All Transactions</h3>
        <div>
            <asp:GridView ID="grdTaxesOverall" runat="server" Width="40%" AutoGenerateColumns="false" ShowFooter="true" OnRowDataBound="grdTaxesOverall_RowDataBound">
                <Columns>
                    <asp:BoundField HeaderText="Date" DataField="dtmInvoiceDate" FooterText="Totals:" DataFormatString="{0:d}"/>
                    <asp:BoundField HeaderText="GST" DataField="fltGovernmentTaxAmount" DataFormatString="{0:C}" />
                    <asp:BoundField HeaderText="PST" DataField="fltProvincialTaxAmount" DataFormatString="{0:C}" />
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <hr />
    </div>
    <asp:Button class="noPrint" ID="btnPrint" runat="server" Text="Print Report" Width="200px"  OnClientClick="CallPrint('Taxes');" />
    <asp:Button class="noPrint" ID="btnDownload" runat="server" Text="Download" Visible="true" Width="200px" OnClick="btnDownload_Click" />
    <script>
        function printReport(printable) {
            window.print();
        }
    </script>
</asp:Content>

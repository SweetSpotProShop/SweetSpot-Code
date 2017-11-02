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
                    <asp:BoundField HeaderText="GST" HeaderStyle-Width="33%" DataField="govTax" DataFormatString="{0:C}" />
                    <asp:BoundField HeaderText="PST" HeaderStyle-Width="33%" DataField="provTax" DataFormatString="{0:C}" />
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
                    <asp:BoundField HeaderText="GST" DataField="govTax" DataFormatString="{0:C}" />
                    <asp:BoundField HeaderText="PST" DataField="provTax" DataFormatString="{0:C}" />
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
                    <asp:BoundField HeaderText="GST" DataField="govTax" DataFormatString="{0:C}" />
                    <asp:BoundField HeaderText="PST" DataField="provTax" DataFormatString="{0:C}" />
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <hr />
    </div>
    <asp:Button class="noPrint" ID="btnPrint" runat="server" Text="Print Report" Width="200px" OnClientClick="printReport()" />
    <script>
        function printReport(printable) {
            window.print();
        }
    </script>
</asp:Content>

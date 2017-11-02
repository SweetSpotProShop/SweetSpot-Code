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
    <asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="btnPrint">
        <h2>Discount Report</h2>
        <br />
        <hr />
        <asp:Label ID="lblReportDate" runat="server" Font-Bold="true" Text="Date"></asp:Label>
        <hr />
        <asp:GridView ID="grdInvoiceDisplay" runat="server" AutoGenerateColumns="false" Width="100%" ShowFooter="true" RowStyle-HorizontalAlign="Center" OnRowDataBound="grdInvoiceDisplay_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="Invoice Number" HeaderStyle-Width="20%">
                    <ItemTemplate>
                        <asp:Label ID="lblInvoiceNum" runat="server" Text='<%#Eval("invoiceNum") + "-" + Eval("invoiceSub") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblTotal" runat="server" Text="Totals:"></asp:Label>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Invoice Date" HeaderStyle-Width="20%">
                    <ItemTemplate>
                        <asp:Label ID="lblInvoiceDate" runat="server" Text='<%#Eval("invoiceDate","{0: dd/MMM/yy}") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Customer Name" HeaderStyle-Width="20%">
                    <ItemTemplate>
                        <asp:Label ID="lblCustomerName" runat="server" Text='<%#Eval("customerName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Discount" HeaderStyle-Width="20%">
                    <ItemTemplate>
                        <asp:Label ID="lblDiscountAmount" runat="server" Text='<%#Eval("discountAmount","{0:C}") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Employee Name" HeaderStyle-Width="20%">
                    <ItemTemplate>
                        <asp:Label ID="lblEmployeeName" runat="server" Text='<%#Eval("employeeName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <br />
        <hr />
        <asp:Table runat="server">
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Button class="noPrint" ID="btnPrint" runat="server" Text="Print Report" Width="200px" OnClientClick="printReport()" />
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

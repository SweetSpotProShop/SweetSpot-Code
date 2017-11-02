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

    <h2>Cost of Goods Sold and Profit Margin</h2>
    <hr />
    <asp:Label ID="lblDates" runat="server" Font-Bold="true" Text="lblDates"></asp:Label>
    <hr />

    <asp:GridView ID="grdInvoiceSelection" runat="server" AutoGenerateColumns="false" ShowFooter="true" Width="100%" RowStyle-HorizontalAlign="Center" OnRowDataBound="grdInvoiceSelection_RowDataBound">
        <Columns>
            <asp:TemplateField HeaderText="Invoice Number" HeaderStyle-Width="20%">
                <ItemTemplate>
                    <asp:LinkButton ID="lbtnInvoiceNumber" runat="server" Text='<%#Eval("invoice")%>' OnClick="lbtnInvoiceNumber_Click"></asp:LinkButton>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:Label ID="lblTotal" runat="server" Text="Totals:"></asp:Label>
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Total Cost" HeaderStyle-Width="20%">
                <ItemTemplate>
                    <asp:Label ID="lblTotalCost" runat="server" Text='<%#Eval("totalCost","{0:C}") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Total Price" HeaderStyle-Width="20%">
                <ItemTemplate>
                    <asp:Label ID="lblTotalPrice" runat="server" Text='<%#Eval("balanceDue","{0:C}") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Total Discount" HeaderStyle-Width="20%">
                <ItemTemplate>
                    <asp:Label ID="lblTotalDiscount" runat="server" Text='<%#Eval("discountAmount") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Discount as Percent" Visible="false" HeaderStyle-Width="20%">
                <ItemTemplate>
                    <asp:Label ID="lblPercentage" runat="server" Text='<%#Eval("percentage") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Total Profit" HeaderStyle-Width="20%">
                <ItemTemplate>
                    <asp:Label ID="lblTotalProfit" runat="server" Text='<%#Eval("totalProfit","{0:C}") %>'></asp:Label>
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
            <asp:TableCell>
                <asp:Button class="noPrint" ID="btnDownload" runat="server" Text="Download" Visible="false" Width="200px" OnClick="btnDownload_Click" />
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <script>
        function printReport(printable) {
            window.print();
        }
    </script>
</asp:Content>

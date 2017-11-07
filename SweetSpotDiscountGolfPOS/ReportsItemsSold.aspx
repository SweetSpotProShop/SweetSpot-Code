<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReportsItemsSold.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.ReportsItemsSold" %>

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
    <h2>Items Sold</h2>
    <hr />
    <div>
        <asp:Label ID="lblDates" runat="server" Font-Bold="true" Text="lblDates"></asp:Label>
    </div>
    <hr />
    <div>
        <asp:GridView ID="grdItems" runat="server" AutoGenerateColumns="false" Width="100%" RowStyle-HorizontalAlign="Center" ShowFooter="true" OnRowDataBound="grdItems_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="Invoice Number">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnInvoiceNumber" runat="server" Text='<%#Eval("invoice")%>' OnClick="lbtnInvoiceNumber_Click"></asp:LinkButton>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblTotal" runat="server" Text="Totals:"></asp:Label>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="SKU">
                    <ItemTemplate>
                        <asp:Label ID="lblSku" runat="server" Text='<%#Eval("sku")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Item Cost">
                    <ItemTemplate>
                        <asp:Label ID="lblItemCost" runat="server" Text='<%#Eval("cost","{0:C}") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Item Price">
                    <ItemTemplate>
                        <asp:Label ID="lblItemPrice" runat="server" Text='<%#Eval("price","{0:C}") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Item Discount">
                    <ItemTemplate>
                        <asp:Label ID="lblTotalDiscount" runat="server" Text='<%#Eval("discount") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Discount as Percent" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblPercentage" runat="server" Text='<%#Eval("percent") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Item Profit">
                    <ItemTemplate>
                        <asp:Label ID="lblTotalProfit" runat="server" Text='<%#Eval("difference","{0:C}") %>'></asp:Label>
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
        </asp:TableRow>
    </asp:Table>
    <%--<script>
        function printReport(printable) {
            window.print();
        }
    </script>--%>
</asp:Content>


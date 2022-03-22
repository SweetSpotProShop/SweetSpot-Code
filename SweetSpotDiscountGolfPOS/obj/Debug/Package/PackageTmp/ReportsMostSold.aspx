<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReportsMostSold.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.ReportsMSI" %>

<asp:Content ID="ReportsPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
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


        <h2>Top Selling Items Report</h2>
        <hr />
        <div>
            <asp:Label ID="lblDates" runat="server" Font-Bold="true" />
        </div>
        <hr />
        <h3>Most Sold Items</h3>
        <div>
            <asp:GridView ID="grdItems" runat="server" AutoGenerateColumns="false" RowStyle-HorizontalAlign="Center">
                <Columns>
                    <asp:TemplateField HeaderText="SKU" ControlStyle-Width="300px">
                        <ItemTemplate>
                            <asp:Label ID="lblSku" runat="server" Text='<%#Eval("sku")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Amount Sold" ControlStyle-Width="300px">
                        <ItemTemplate>
                            <asp:Label ID="lblAmountSold" runat="server" Text='<%#Eval("amountSold")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <hr />
        <h3>Most Sold Brands</h3>
        <div>
            <asp:GridView ID="grdBrands" runat="server" AutoGenerateColumns="false" RowStyle-HorizontalAlign="Center">
                <Columns>
                    <asp:TemplateField HeaderText="Brand" ControlStyle-Width="300px">
                        <ItemTemplate>
                            <asp:Label ID="lblBrand" runat="server" Text='<%#Eval("brand")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Times Sold" ControlStyle-Width="300px">
                        <ItemTemplate>
                            <asp:Label ID="lblTimesSold" runat="server" Text='<%#Eval("amountSold")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <hr />
        <h3>Most Sold Models</h3>
        <div>
            <asp:GridView ID="grdModels" runat="server" AutoGenerateColumns="false" RowStyle-HorizontalAlign="Center">
                <Columns>
                    <asp:TemplateField HeaderText="Model" ControlStyle-Width="300px">
                        <ItemTemplate>
                            <asp:Label ID="lblModel" runat="server" Text='<%#Eval("model")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Times Sold" ControlStyle-Width="300px">
                        <ItemTemplate>
                            <asp:Label ID="lblTimesSold" runat="server" Text='<%#Eval("amountSold")%>' />
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
                    <asp:Button CssClass="noPrint" ID="btnPrint" runat="server" Text="Print Report" Width="200px" OnClientClick="CallPrint('print');" />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Button CssClass="noPrint" ID="BtnDownload" runat="server" Text="Download" Visible="true" Width="200px" OnClick="BtnDownload_Click" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
</asp:Content>
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
    <h2>Marketing: Most Sold Report</h2>
    <hr />
    <div>
        <asp:Label ID="lblDates" runat="server" Text="lblDates" font-bold="true"></asp:Label>
    </div>
    <hr />
    <h2>Most Sold Items</h2>
    <div>
        <asp:GridView ID="grdItems" runat="server" AutoGenerateColumns="false" RowStyle-HorizontalAlign="Center">
            <Columns>
                <asp:TemplateField HeaderText="SKU" ControlStyle-Width="300px">
                    <ItemTemplate>
                        <asp:Label ID="lblSku" runat="server" Text='<%#Eval("sku")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Amount Sold" ControlStyle-Width="300px">
                    <ItemTemplate>
                        <asp:Label ID="lblAmountSold" runat="server" Text='<%#Eval("amountSold")%>'></asp:Label>
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
                        <asp:Label ID="lblBrand" runat="server" Text='<%#Eval("description")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Times Sold" ControlStyle-Width="300px">
                    <ItemTemplate>
                        <asp:Label ID="lblTimesSold" runat="server" Text='<%#Eval("amountSold")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                </Columns>
            </asp:GridView>
    </div>
     <hr />
    <h3>Most Sold Models</h3>
    <div>
        <asp:GridView ID="grdModels" runat="server" AutoGenerateColumns="false"  RowStyle-HorizontalAlign="Center">
            <Columns>
                <asp:TemplateField HeaderText="Model" ControlStyle-Width="300px">
                    <ItemTemplate>
                        <asp:Label ID="lblModel" runat="server" Text='<%#Eval("description")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Times Sold" ControlStyle-Width="300px">
                    <ItemTemplate>
                        <asp:Label ID="lblTimesSold" runat="server" Text='<%#Eval("amountSold")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                </Columns>
            </asp:GridView>
    </div>
    <br />
    <hr />
    















</asp:Content>


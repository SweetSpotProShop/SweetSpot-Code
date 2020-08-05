<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReportsSpecificGrip.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.ReportsSpecificGrip" %>
<asp:Content ID="ReportsPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <div id="print">
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

        <h2>Specific Grip Report</h2>
        <hr />
        <div>
            <asp:Label ID="lblDates" runat="server" Font-Bold="true" />
        </div>
        <div>
            <asp:GridView ID="GrdStats" runat="server" AutoGenerateColumns="false" ShowFooter="true" RowStyle-HorizontalAlign="Center" OnRowDataBound="GrdStats_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="Store">
                        <ItemTemplate>
                            <asp:Label ID="lblYear" runat="server" Text='<%#Eval("varLocationName")%>'/>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblTotals" runat="server" Text="Totals:" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="SKU">
                        <ItemTemplate>
                            <asp:Label ID="lblSKU" runat="server" Text='<%#Eval("varSku")%>'/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Description">
                        <ItemTemplate>
                            <asp:Label ID="lblDescription" runat="server" Text='<%#Eval("varItemDescription")%>'/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Quantity">
                        <ItemTemplate>
                            <asp:Label ID="lblQuantity" runat="server" Text='<%#Eval("intOverallQuantity")%>'/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Average Cost">
                        <ItemTemplate>
                            <asp:Label ID="lblAverageCost" runat="server" Text='<%#(Convert.ToDouble(Eval("fltOverallCost")) / Convert.ToDouble(Eval("intOverallQuantity"))).ToString("C")%>'/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Average Price">
                        <ItemTemplate>
                            <asp:Label ID="lblAveragePrice" runat="server" Text='<%#(Convert.ToDouble(Eval("fltOverallPrice")) / Convert.ToDouble(Eval("intOverallQuantity"))).ToString("C")%>'/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Cost of Goods Sold">
                        <ItemTemplate>
                            <asp:Label ID="lblCostGoodsSold" runat="server" Text='<%#(Convert.ToDouble(Eval("fltOverallCost"))).ToString("C")%>'/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Profit Margin">
                        <ItemTemplate>
                            <asp:Label ID="lblProfitMargin" runat="server" Text='<%#(((Convert.ToDouble(Eval("fltOverallPrice")) * Convert.ToDouble(Eval("intOverallQuantity"))) - (Convert.ToDouble(Eval("intOverallQuantity")) * Convert.ToDouble(Eval("fltOverallCost")))) / (Convert.ToDouble(Eval("fltOverallPrice")) * Convert.ToDouble(Eval("intOverallQuantity")))).ToString("P") %>'/>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>        
        <hr />
        </div>
        <asp:Table runat="server">
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Button CssClass="noPrint" ID="btnPrint" runat="server" Text="Print Report" Width="200px" OnClientClick="CallPrint('print');" />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Button CssClass="noPrint" ID="BtnDownload" runat="server" Text="Download" Visible="true" Width="200px" Onclick="BtnDownload_Click"/>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
</asp:Content>
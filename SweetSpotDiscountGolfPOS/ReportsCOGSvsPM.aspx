<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReportsCOGSvsPM.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.ReportsCOGSvsPM" %>

<asp:Content ID="ReportsPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">

    <h2>Cost of Goods Sold and Profit Margin</h2>
    <hr />
    <table border="1">
        <tr>
            <td colspan="5">
                <asp:Label ID="lblDates" runat="server" Text="lblDates"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:label id="lblItemsSold" runat="server" text="Items Sold:"></asp:label>
            </td>
            <td>
                <asp:label id="lblCost" runat="server" text="Total Cost:"></asp:label>
            </td>
            <td>
                <asp:label id="lblPM" runat="server" text="Total Sold:"></asp:label>
            </td>
            <td>
                <asp:label id="lblProfitMargin" runat="server" text="Profit Margin:"></asp:label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:GridView ID="grdInvoiceSelection" runat="server" AutoGenerateColumns="false" Width="100%">
                    <Columns>
                        <asp:TemplateField HeaderText="Invoice Number">
                            <ItemTemplate>
                                <asp:Label ID="lblInvoiceNum" runat="server"  Text='<%#Eval("invoiceNum") + "-" + Eval("invoiceSubNum")%>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sku">
                            <ItemTemplate>
                                <asp:Label ID="lblSku" runat="server" Text='<%#Eval("sku") %>'></asp:Label>
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
                        <asp:TemplateField HeaderText="Profit">
                            <ItemTemplate>
                                <asp:Label ID="lblItemProfit" runat="server" Text='<%#Eval("difference","{0:C}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField> 
                    </Columns>
                </asp:GridView>
            </td>
            <td style="vertical-align:top">
                <asp:label id="lblTotalCostDisplay" runat="server" text="lblTotalCostDisplay"></asp:label>
            </td>
            <td style="vertical-align:top">
                <asp:label id="lblSoldDisplay" runat="server" text="lblSoldDisplay"></asp:label>
            </td>
            <td style="vertical-align:top">
                <asp:label id="lblProfitMarginDisplay" runat="server" text="lblProfitMarginDisplay"></asp:label>
            </td>
        </tr>        
    </table>









































</asp:Content>

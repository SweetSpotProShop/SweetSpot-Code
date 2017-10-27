<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReportsCOGSvsPM.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.ReportsCOGSvsPM" %>

<asp:Content ID="ReportsPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">

    <h2>Cost of Goods Sold and Profit Margin</h2>
    <hr />
    <asp:Label ID="lblDates" runat="server" Text="lblDates"></asp:Label>
    <table border="1">        
        <tr>
            <td colspan="2">
                <asp:label id="lblItemsSold" runat="server" text="Items Sold:"></asp:label>
            </td>
            <td>
                <asp:label id="lblCost" runat="server" text="Total Cost:"></asp:label>
            </td>
            <td>
                <asp:label id="lblPM" runat="server" text="Total Price:"></asp:label>
            </td>
            <td>
                <asp:label id="lblProfitMargin" runat="server" text="Profit Margin:"></asp:label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:GridView ID="grdInvoiceSelection" runat="server" AutoGenerateColumns="false" Width="100%" RowStyle-HorizontalAlign="Center">
                    <Columns>
                        <asp:TemplateField HeaderText="Invoice Number">
                            <ItemTemplate>
                                <asp:Label ID="lblInvoiceNum" runat="server"  Text='<%#Eval("invoice")%>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total Cost">
                            <ItemTemplate>
                                <asp:Label ID="lblTotalCost" runat="server" Text='<%#Eval("totalCost","{0:C}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total Discount">
                            <ItemTemplate>
                                <asp:Label ID="lblTotalDiscount" runat="server" Text='<%#Eval("discountAmount") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Discount as Percent">
                            <ItemTemplate>
                                <asp:Label ID="lblPercentage" runat="server" Text='<%#Eval("percentage") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>       
                        <asp:TemplateField HeaderText="Total Price">
                            <ItemTemplate>
                                <asp:Label ID="lblTotalPrice" runat="server" Text='<%#Eval("balanceDue","{0:C}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="Total Profit">
                            <ItemTemplate>
                                <asp:Label ID="lblTotalProfit" runat="server" Text='<%#Eval("totalProfit","{0:C}") %>'></asp:Label>
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

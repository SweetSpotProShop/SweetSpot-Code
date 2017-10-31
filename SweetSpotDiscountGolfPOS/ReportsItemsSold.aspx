<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReportsItemsSold.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.ReportsItemsSold" %>

<asp:Content ID="ReportsPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <h2>Items Sold</h2>
    <hr />
    <div>
        <asp:Label ID="lblDates" runat="server" Text="lblDates"></asp:Label>
    </div>
    <div>
        <asp:GridView ID="grdItems" runat="server" AutoGenerateColumns="false" Width="100%" RowStyle-HorizontalAlign="Center" OnRowDataBound="grdItems_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="Invoice Number">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnInvoiceNumber" runat="server" Text='<%#Eval("invoice")%>' OnClick="lbtnInvoiceNumber_Click"></asp:LinkButton>
                    </ItemTemplate>
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














</asp:Content>


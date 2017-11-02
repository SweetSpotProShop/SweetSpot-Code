<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReportsMostSold.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.ReportsMSI" %>
<asp:Content ID="ReportsPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    
    <div>
        <asp:Label ID="lblDates" runat="server" Text="lblDates"></asp:Label>
    </div>
    <hr />
    <h2>Most Sold Items</h2>
    <hr />    
    <div>
        <asp:GridView ID="grdItems" runat="server" AutoGenerateColumns="false" Width="100%" RowStyle-HorizontalAlign="Center">
            <Columns>
                <asp:TemplateField HeaderText="SKU">
                    <ItemTemplate>
                        <asp:Label ID="lblSku" runat="server" Text='<%#Eval("sku")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Amount Sold">
                    <ItemTemplate>
                        <asp:Label ID="lblAmountSold" runat="server" Text='<%#Eval("amountSold")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                </Columns>
            </asp:GridView>
    </div>
    <hr />
    <h2>Most Sold Brands</h2>
    <hr />
    <div>
        <asp:GridView ID="grdBrands" runat="server" AutoGenerateColumns="false" Width="100%" RowStyle-HorizontalAlign="Center">
            <Columns>
                <asp:TemplateField HeaderText="Brand">
                    <ItemTemplate>
                        <asp:Label ID="lblBrand" runat="server" Text='<%#Eval("description")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Times Sold">
                    <ItemTemplate>
                        <asp:Label ID="lblTimesSold" runat="server" Text='<%#Eval("amountSold")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                </Columns>
            </asp:GridView>
    </div>
     <hr />
    <h2>Most Sold Models</h2>
    <hr />
    <div>
        <asp:GridView ID="grdModels" runat="server" AutoGenerateColumns="false" Width="100%" RowStyle-HorizontalAlign="Center">
            <Columns>
                <asp:TemplateField HeaderText="Model">
                    <ItemTemplate>
                        <asp:Label ID="lblModel" runat="server" Text='<%#Eval("description")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Times Sold">
                    <ItemTemplate>
                        <asp:Label ID="lblTimesSold" runat="server" Text='<%#Eval("amountSold")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                </Columns>
            </asp:GridView>
    </div>















</asp:Content>


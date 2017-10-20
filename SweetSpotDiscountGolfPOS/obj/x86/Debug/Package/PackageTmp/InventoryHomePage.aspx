<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="InventoryHomePage.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.InventoryHomePage" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SPMaster" runat="server">
</asp:Content>--%>

<asp:Content ID="InventoryPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <div id="Inventory">
        <asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="btnInventorySearch">
            <h2>Inventory Information</h2>
            <hr />
            <asp:Label ID="lblInventoryType" runat="server" Text="Inventory Type"></asp:Label>
            <div class="divider" />
            <asp:DropDownList ID="ddlInventoryType" runat="server" DataSourceID="sqlInventoryTypes" DataTextField="typeDescription" DataValueField="typeID"></asp:DropDownList>
            <asp:SqlDataSource ID="sqlInventoryTypes" runat="server" ConnectionString="<%$ ConnectionStrings:SweetSpotDevConnectionString %>" SelectCommand="SELECT [typeID], [typeDescription] FROM [tbl_itemType]"></asp:SqlDataSource>
            <hr />
            <%--Enter search text to find matching Inventory information--%>
            <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
            <hr />
            <asp:Button ID="btnInventorySearch" runat="server" Width="150" Text="Inventory Search" OnClick="btnInventorySearch_Click" />
            <div class="divider" />
            <asp:Button ID="btnAddNewInventory" runat="server" Width="150" Text="Add New Inventory" OnClick="btnAddNewInventory_Click" />
            <hr />
            <asp:GridView ID="grdInventorySearched" runat="server" AutoGenerateColumns="False" OnRowCommand="grdInventorySearched_RowCommand">
                <Columns>
                    <asp:TemplateField HeaderText="View Item">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtnView" CommandName="viewItem" CommandArgument='<%#Eval("sku") %>' Text="View Item" runat="server">View Item</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="SKU">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%#Eval("sku")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Description">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%#Eval("description")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Store">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%#Eval("location")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Quantity">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%#Eval("quantity")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Price">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%#Eval("price")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Cost">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%#Eval("cost")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    No current Inventory data, please search for an Inventory Item
                </EmptyDataTemplate>
            </asp:GridView>
        </asp:Panel>
    </div>
</asp:Content>

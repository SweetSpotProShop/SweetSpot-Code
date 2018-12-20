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
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Table runat="server">
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label ID="lblSearch" runat="server" Text="Enter Search Text" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:CheckBox ID="chkIncludeZero" runat="server" Text="Return Zero Quantity" TextAlign="Left" />
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:TextBox ID="txtSearch" runat="server" AutoComplete="off" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Button ID="btnInventorySearch" runat="server" Width="150" Text="Inventory Search" OnClick="btnInventorySearch_Click" />                                
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Button ID="btnAddNewInventory" runat="server" Width="150" Text="Add New Inventory" OnClick="btnAddNewInventory_Click" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Button ID="btnMakePurchase" runat="server" Width="150" Text="Make Purchase" OnClick="btnMakePurchase_Click" />
                            </asp:TableCell>

                        </asp:TableRow>

                    </asp:Table>
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                        <ProgressTemplate>
                            <div>
                                <img src="Images/ajax-loader.gif" />
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <hr />
                    <asp:GridView ID="grdInventorySearched" runat="server" AutoGenerateColumns="False" OnRowCommand="grdInventorySearched_RowCommand" AllowPaging="true" PageSize="50" OnPageIndexChanging="grdInventorySearched_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField HeaderText="View Item">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnView" CommandName="viewItem" CommandArgument='<%#Eval("sku") %>' Text="View Item" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SKU">
                                <HeaderTemplate>
                                    <asp:Button ID="btnSKU" runat="server" OnClick="lbtnSKU_Click" Width="100px" Text="SKU" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("sku")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Description">
                                <HeaderTemplate>
                                    <asp:Button ID="btnDescription" runat="server" OnClick="btnDescription_Click" Width="100px" Text="Description" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("description")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Store">
                                <HeaderTemplate>
                                    <asp:Button ID="btnStore" runat="server" OnClick="btnStore_Click" Width="100px" Text="Store" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("location")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Quantity">
                                <HeaderTemplate>
                                    <asp:Button ID="btnQuantity" runat="server" OnClick="btnQuantity_Click" Width="100px" Text="Quantity" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("quantity")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Price">
                                <HeaderTemplate>
                                    <asp:Button ID="btnPrice" runat="server" OnClick="btnPrice_Click" Width="100px" Text="Price" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("price","{0:C}")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cost">
                                <HeaderTemplate>
                                    <asp:Button ID="btnCost" runat="server" OnClick="btnCost_Click" Width="100px" Text="Cost" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("cost","{0:C}")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Comments">
                                <HeaderTemplate>
                                    <asp:Button ID="btnComments" runat="server" OnClick="btnComments_Click" Width="100px" Text="Comments" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("comments")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            No current Inventory data, please search for an Inventory Item
                        </EmptyDataTemplate>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
            <hr />
            <asp:Button class="noPrint" ID="btnDownload" runat="server" Text="Download" Visible="true" Width="200px" OnClick="btnDownload_Click" />
        </asp:Panel>
    </div>
</asp:Content>

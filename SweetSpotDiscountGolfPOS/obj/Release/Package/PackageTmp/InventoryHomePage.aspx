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
                                <asp:TextBox ID="txtSearch" runat="server" AutoCompleteType="Disabled" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Button ID="BtnInventorySearch" runat="server" Width="150" Text="Inventory Search" OnClick="BtnInventorySearch_Click" />                                
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Button ID="BtnAddNewInventory" runat="server" Width="150" Text="Add New Inventory" OnClick="BtnAddNewInventory_Click" />
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Button ID="BtnMakePurchase" runat="server" Width="150" Text="Make Purchase" OnClick="BtnMakePurchase_Click" />
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
                    <asp:GridView ID="GrdInventorySearched" runat="server" AutoGenerateColumns="False" OnRowCommand="GrdInventorySearched_RowCommand" 
						AllowPaging="True" PageSize="50" OnPageIndexChanging="GrdInventorySearched_PageIndexChanging" >
                        <Columns>
                            <asp:TemplateField HeaderText="View Item">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnView" CommandName="viewItem" CommandArgument='<%#Eval("intInventoryID") %>' Text="View Item" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SKU">
                                <HeaderTemplate>
                                    <asp:Button ID="BtnSKU" runat="server" OnClick="BtnSKU_Click" Width="100px" Text="SKU" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("varSku")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Description">
                                <HeaderTemplate>
                                    <asp:Button ID="BtnDescription" runat="server" OnClick="BtnDescription_Click" Width="100px" Text="Description" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("varItemDescription")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Store">
                                <HeaderTemplate>
                                    <asp:Button ID="BtnStore" runat="server" OnClick="BtnStore_Click" Width="100px" Text="Store" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("varLocationName")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Quantity">
                                <HeaderTemplate>
                                    <asp:Button ID="BtnQuantity" runat="server" OnClick="BtnQuantity_Click" Width="100px" Text="Quantity" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("intItemQuantity")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Price">
                                <HeaderTemplate>
                                    <asp:Button ID="BtnPrice" runat="server" OnClick="BtnPrice_Click" Width="100px" Text="Price" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("fltItemPrice","{0:C}")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cost">
                                <HeaderTemplate>
                                    <asp:Button ID="BtnCost" runat="server" OnClick="BtnCost_Click" Width="100px" Text="Cost" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("fltItemCost","{0:C}")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Comments">
                                <HeaderTemplate>
                                    <asp:Button ID="BtnComments" runat="server" OnClick="BtnComments_Click" Width="100px" Text="Comments" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("varAdditionalInformation")%>' />
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
            <asp:Button CssClass="noPrint" ID="BtnDownload" runat="server" Text="Download" Visible="true" Width="200px" OnClick="BtnDownload_Click" />
        </asp:Panel>
    </div>
</asp:Content>

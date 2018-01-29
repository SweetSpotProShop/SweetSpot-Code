<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="InventoryAddNew.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.InventoryAddNew" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SPMaster" runat="server">
</asp:Content>--%>

<asp:Content ID="InventoryAddNewPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <div id="NewInventory">
        <%--REMEMBER TO SET DEFAULT BUTTON--%>
        <asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="btnSaveItem">
            <%--Textboxes and Labels for user to enter inventory info--%>
            <h2>New Inventory Item</h2>

            <br />
            <h3>
                <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="True" Enabled="false" />
                <%--<asp:Label ID="lblTypeDisplay" runat="server" Visible="true" />--%>
            </h3>
            <asp:Table ID="Table1" runat="server" Width="100%">
                <asp:TableRow>
                    <asp:TableCell Width="25%">
                        <asp:Label ID="lblSKU" runat="server" Text="SKU:" />
                    </asp:TableCell>
                    <asp:TableCell Width="25%">
                        <asp:Label ID="lblSKUDisplay" runat="server" Text="" />
                    </asp:TableCell>
                    <asp:TableCell Width="25%">
                        <asp:Label ID="lblCost" runat="server" Text="Cost:  $" />
                    </asp:TableCell>
                    <asp:TableCell Width="25%">
                        <asp:TextBox ID="txtCost" runat="server" Enabled="false" Text="0" />
                        <%--<asp:Label ID="lblCostDisplay" runat="server" Text="" Visible="true" />--%>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblBrand" runat="server" Text="Brand Name:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:DropDownList ID="ddlBrand" runat="server" AutoPostBack="True" Enabled="false" />
                        <%--<asp:Label ID="lblBrandDisplay" runat="server" Text="" Visible="true" />--%>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblPrice" runat="server" Text="Price:  $" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtPrice" runat="server" Enabled="false" Text="0" />
                        <%--<asp:Label ID="lblPriceDisplay" runat="server" Text="" Visible="true" />--%>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblQuantity" runat="server" Text="Quantity:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtQuantity" runat="server" Enabled="false" Text="0" />
                        <%--<asp:Label ID="lblQuantityDisplay" runat="server" Text="" Visible="true" />--%>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblLocation" runat="server" Text="Location:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:DropDownList ID="ddlLocation" runat="server" AutoPostBack="True" Enabled="false" />
                        <%--<asp:Label ID="lblLocationDisplay" runat="server" Text="" Visible="true" />--%>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="4"><hr /></asp:TableCell>
                </asp:TableRow>

                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblClubType" runat="server" Text="Club Type:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtClubType" runat="server" Enabled="false" />
                        <%--<asp:Label ID="lblClubTypeDisplay" runat="server" Text="" Visible="true" />--%>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblModel" runat="server" Text="Model:" Visible="true" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:DropDownList ID="ddlModel" runat="server" AutoPostBack="True" Enabled="false" />
                        <%--<asp:Label ID="lblModelDisplay" runat="server" Text="" Visible="true" />--%>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblShaft" runat="server" Text="Shaft:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtShaft" runat="server" Enabled="false" />
                        <%--<asp:Label ID="lblShaftDisplay" runat="server" Text="" Visible="true" />--%>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblNumberofClubs" runat="server" Text="Number of Clubs:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtNumberofClubs" runat="server" Enabled="false" />
                        <%--<asp:Label ID="lblNumberofClubsDisplay" runat="server" Text="" Visible="true" />--%>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblClubSpec" runat="server" Text="Club Spec:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtClubSpec" runat="server" Enabled="false" />
                        <%--<asp:Label ID="lblClubSpecDisplay" runat="server" Text="" Visible="true" />--%>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblShaftSpec" runat="server" Text="Shaft Spec:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtShaftSpec" runat="server" Enabled="false" />
                        <%--<asp:Label ID="lblShaftSpecDisplay" runat="server" Text="" Visible="true" />--%>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblShaftFlex" runat="server" Text="ShaftFlex:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtShaftFlex" runat="server" Enabled="false" />
                        <%--<asp:Label ID="lblShaftFlexDisplay" runat="server" Text="" Visible="true" />--%>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblDexterity" runat="server" Text="Dexterity:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtDexterity" runat="server" Enabled="false" />
                        <%--<asp:Label ID="lblDexterityDisplay" runat="server" Text="" Visible="true" />--%>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="4"><hr /></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="2">
                        <asp:Label ID="lblComments" runat="server" Text="Comments:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:CheckBox ID="chkUsed" runat="server" Text="Used" Enabled="false" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="4">
                        <asp:TextBox Height="30px" Width="100%" ID="txtComments" runat="server" Enabled="false" />
                        <%--<asp:Label Height="30px" Width="100%" ID="lblCommentsDisplay" runat="server" Visible="true" />--%>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="4"><hr /></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Button ID="btnAddItem" runat="server" Text="Add Item" OnClick="btnAddItem_Click" Visible="false" />
                        <asp:Button ID="btnEditItem" runat="server" Text="Edit Item" OnClick="btnEditItem_Click" Visible="true" />
                        <asp:Button ID="btnSaveItem" runat="server" Text="Save Changes" OnClick="btnSaveItem_Click" Visible="false" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="btnBackToSearch" runat="server" Text="Exit Item" OnClick="btnBackToSearch_Click" Visible="true" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" Visible="false" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="btnCreateSimilar" runat="server" Text="Create Similar" OnClick="btnCreateSimilar_Click" Visible="true" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </asp:Panel>
    </div>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="InventoryAddNew.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.InventoryAddNew" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SPMaster" runat="server">
</asp:Content>--%>

<asp:Content ID="InventoryAddNewPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <div id="NewInventory">
        <%--REMEMBER TO SET DEFAULT BUTTON--%>
        <asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="btnSaveItem">
            <h2>New Inventory Item</h2>
            <br />
            <h3>
                <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="True" 
                    DataTextField="varItemTypeName" DataValueField="intItemTypeID" Enabled="false" />
            </h3>
            <asp:Table ID="Table1" runat="server" Width="100%">
                <asp:TableRow>
                    <asp:TableCell Width="16%">
                        <asp:Label ID="lblSKU" runat="server" Text="SKU:" />
                    </asp:TableCell>
                    <asp:TableCell Width="24%">
                        <asp:Label ID="lblSKUDisplay" runat="server" />
                    </asp:TableCell>
                    <asp:TableCell Width="21%">
                    </asp:TableCell>
                    <asp:TableCell Width="13%">
                        <asp:Label ID="lblCost" runat="server" Text="Cost:  $" />
                    </asp:TableCell>
                    <asp:TableCell Width="13%">
                        <asp:TextBox ID="txtCost" runat="server" AutoCompleteType="Disabled" Enabled="false" Text="0.00" />
                    </asp:TableCell>
                    <asp:TableCell Width="13%">
                        <asp:RegularExpressionValidator ID="revCost"
                                ControlToValidate="txtCost"
                                ValidationExpression="[-+]?([0-9]*\.[0-9]+|[0-9]+)"
                                Display="Static"
                                EnableClientScript="true"
                                ErrorMessage="Requires a number"
                                runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblBrand" runat="server" Text="Brand Name:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:DropDownList ID="ddlBrand" runat="server" AutoPostBack="false" 
                            DataTextField="varBrandName" DataValueField="intBrandID" Enabled="false" />
                    </asp:TableCell>
                    <asp:TableCell>

                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblPrice" runat="server" Text="Price:  $" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtPrice" runat="server" AutoCompleteType="Disabled" Enabled="false" Text="0.00" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:RegularExpressionValidator ID="revPrice"
                                ControlToValidate="txtPrice"
                                ValidationExpression="[-+]?([0-9]*\.[0-9]+|[0-9]+)"
                                Display="Static"
                                EnableClientScript="true"
                                ErrorMessage="Requires a number"
                                runat="server" />
                    </asp:TableCell>
                    
                </asp:TableRow>
                <asp:TableRow>

                    <asp:TableCell>
                        <asp:Label ID="lblLocation" runat="server" Text="Location:" />
                    </asp:TableCell>
                    <asp:TableCell  ColumnSpan="2">
                        <asp:DropDownList ID="ddlLocation" runat="server" AutoPostBack="false" 
                            DataTextField="varCityName" DataValueField="intLocationID" Enabled="false" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblQuantity" runat="server" Text="Quantity:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtQuantity" runat="server" AutoCompleteType="Disabled" Enabled="false" Text="0" />
                        <asp:RegularExpressionValidator ID="revQuantity"
                                ControlToValidate="txtQuantity"
                                ValidationExpression="[-+]?([0-9]*\.[0-9]+|[0-9]+)"
                                Display="Static"
                                EnableClientScript="true"
                                ErrorMessage="Requires a number"
                                runat="server" />
                    </asp:TableCell>
                    <asp:TableCell>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="6">
                        <hr />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblModel" runat="server" Text="Model:" Visible="true" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:DropDownList ID="ddlModel" runat="server" AutoPostBack="false"
                            DataTextField="varModelName" DataValueField="intModelID" Enabled="false" /> 
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblClubType" runat="server" Text="Club Type:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtClubType" runat="server" AutoCompleteType="Disabled" Enabled="false" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblDexterity" runat="server" Text="Dexterity:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtDexterity" runat="server" AutoCompleteType="Disabled" Enabled="false" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblClubSpec" runat="server" Text="Club Spec:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtClubSpec" runat="server" AutoCompleteType="Disabled" Enabled="false" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblShaftSpec" runat="server" Text="Shaft Spec:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtShaftSpec" runat="server" AutoCompleteType="Disabled" Enabled="false" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblNumberofClubs" runat="server" Text="Number of Clubs:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtNumberofClubs" runat="server" AutoCompleteType="Disabled" Enabled="false" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblShaft" runat="server" Text="Shaft:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtShaft" runat="server" AutoCompleteType="Disabled" Enabled="false" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblShaftFlex" runat="server" Text="ShaftFlex:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtShaftFlex" runat="server" AutoCompleteType="Disabled" Enabled="false" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblProdID" runat="server" Text="Prod ID:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtProdID" runat="server" AutoCompleteType="Disabled" Enabled="false" />
                    </asp:TableCell>

                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="6">
                        <hr />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="2">
                        <asp:Label ID="lblComments" runat="server" Text="Comments:" />
                    </asp:TableCell>
                    <asp:TableCell ColumnSpan="1">
                        <asp:CheckBox ID="chkUsed" runat="server" Text="Used" Enabled="false" />
                    </asp:TableCell>
					<asp:TableCell ColumnSpan="3" RowSpan="2" HorizontalAlign="Center">
						<asp:Label ID="lblTaxesChargedAtSale" runat="server" Text="Taxes Charged At Sale" Font-Bold="true" />
						<asp:GridView ID="GrdInventoryTaxes" runat="server" AutoGenerateColumns="false" Width="80%"
							RowStyle-HorizontalAlign="Center" OnRowDataBound="GrdInventoryTaxes_RowDataBound"
							OnRowCommand="GrdInventoryTaxes_RowCommand">
							<Columns>
								<asp:TemplateField>
									<ItemTemplate>
										<asp:LinkButton ID="lbtnChangeCharged" runat="server" Text="Remove" CommandArgument='<%#Eval("intTaxID") %>' />
									</ItemTemplate>
								</asp:TemplateField>
								<asp:BoundField DataField="varTaxName" ReadOnly="true" HeaderText="Tax Name" />
								<asp:BoundField DataField="fltTaxRate" ReadOnly="true" HeaderText="Tax Rate" DataFormatString="{0:P}" />
								<asp:TemplateField HeaderText="Charge">
									<ItemTemplate>
										<asp:CheckBox ID="chkChargeTax" runat="server" Text="" Checked='<%#Eval("bitChargeTax") %>' Enabled="false" />
									</ItemTemplate>
								</asp:TemplateField>
							</Columns>
						</asp:GridView>
					</asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="3">
                        <asp:TextBox Height="30px" Width="100%" ID="txtComments" runat="server" AutoCompleteType="Disabled" Enabled="false" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="6">
                        <hr />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="2">
                        <asp:Button ID="BtnAddItem" runat="server" Text="Add Item" OnClick="BtnAddItem_Click" Visible="false" />
                        <asp:Button ID="BtnEditItem" runat="server" Text="Edit Item" OnClick="BtnEditItem_Click" Visible="true" />
                        <asp:Button ID="BtnSaveItem" runat="server" Text="Save Changes" OnClick="BtnSaveItem_Click" Visible="false" />
                    </asp:TableCell>
                    <asp:TableCell ColumnSpan="2">
                        <asp:Button ID="BtnBackToSearch" runat="server" Text="Exit Item" OnClick="BtnBackToSearch_Click" Visible="true" />
                        <asp:Button ID="BtnCancel" runat="server" Text="Cancel" OnClick="BtnCancel_Click" Visible="false" CausesValidation="false"/>
                    </asp:TableCell>
                    <asp:TableCell ColumnSpan="2">
                        <asp:Button ID="BtnCreateSimilar" runat="server" Text="Create Similar" OnClick="BtnCreateSimilar_Click" Visible="true" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </asp:Panel>
    </div>
</asp:Content>

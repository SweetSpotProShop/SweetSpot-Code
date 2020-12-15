<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="SalesCart.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.SalesCart" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>--%>

<asp:Content ID="NonActive" ContentPlaceHolderID="SPMaster" runat="server">
	<style>
		.costDetail {
			display: none;
		}

		.cost:hover .costDetail {
			display: block;
			position: absolute;
			text-align: left;
			max-width: 300px;
			max-height: 300px;
			overflow: auto;
			background-color: #fff;
			border: 2px solid #bbb;
			padding: 3px;
		}
	</style>
	<style>
		.priceDetail {
			display: none;
		}

		.price:hover .priceDetail {
			display: block;
			position: absolute;
			text-align: left;
			max-width: 300px;
			max-height: 300px;
			overflow: auto;
			background-color: #fff;
			border: 2px solid #bbb;
			padding: 3px;
		}
	</style>
	<div id="menu_simple">
		<ul>
			<li><a>HOME</a></li>
			<li><a>CUSTOMERS</a></li>
			<li><a>SALES</a></li>
			<li><a>INVENTORY</a></li>
			<li><a>REPORTS</a></li>
			<li><a>SETTINGS</a></li>
		</ul>
	</div>
	<div id="image_simple">
		<img src="Images/SweetSpotLogo.jpg" />
	</div>
	<link rel="stylesheet" type="text/css" href="CSS/MainStyleSheet.css" />
</asp:Content>
<asp:Content ID="CartPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
	<div id="Cart">
		<asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="btnInventorySearch">
			<asp:Label ID="lblCustomer" runat="server" Text="Customer Name:" />
			<asp:TextBox ID="txtCustomer" runat="server" AutoCompleteType="Disabled" />
			<asp:Button ID="BtnCustomerSelect" runat="server" Text="Change Customer" OnClick="BtnCustomerSelect_Click" CausesValidation="false" />
			<div>
				<br />
				<div>
					<asp:GridView ID="GrdCustomersSearched" runat="server" AutoGenerateColumns="false" ShowFooter="true"
						OnRowCommand="GrdCustomersSearched_RowCommand" AllowPaging="True" PageSize="5"
						OnPageIndexChanging="GrdCustomersSearched_PageIndexChanging">
						<Columns>
							<asp:TemplateField HeaderText="Switch Customer">
								<ItemTemplate>
									<asp:LinkButton ID="lbtnSwitchCustomer" CommandName="SwitchCustomer" CommandArgument='<%#Eval("intCustomerID") %>' Text="Switch Customer" runat="server" />
								</ItemTemplate>
								<FooterTemplate>
									<asp:Button ID="BtnAddCustomer" runat="server" Text="Add Customer" OnClick="BtnAddCustomer_Click" />
								</FooterTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Customer Name">
								<ItemTemplate>
									<asp:Label runat="server" Text='<%#Eval("varFirstName") + " " + Eval("varLastName") %>' />
								</ItemTemplate>
								<FooterTemplate>
									<div>
										<asp:TextBox ID="txtFirstName" runat="server" AutoCompleteType="Disabled" placeholder="First Name" ToolTip="First Name" />
									</div>
									<div>
										<asp:TextBox ID="txtLastName" runat="server" AutoCompleteType="Disabled" placeholder="Last Name" ToolTip="Last Name" />
									</div>
								</FooterTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Phone Number">
								<ItemTemplate>
									<asp:Label runat="server" Text='<%#Eval("varContactNumber") %>' />
								</ItemTemplate>
								<FooterTemplate>
									<div>
										<asp:TextBox ID="txtPhoneNumber" runat="server" AutoCompleteType="Disabled" placeholder="Phone Number" ToolTip="Phone Number" />
									</div>
								</FooterTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Email Address">
								<ItemTemplate>
									<asp:Label runat="server" Text='<%#Eval("varEmailAddress") %>' />
								</ItemTemplate>
								<FooterTemplate>
									<div>
										<asp:TextBox ID="txtEmail" runat="server" AutoCompleteType="Disabled" placeholder="Email" ToolTip="Email" />
									</div>
									<div>
										<asp:CheckBox ID="chkMarketingEnrollment" runat="server" Text="Marketing Enrollment" />
									</div>
								</FooterTemplate>
							</asp:TemplateField>
						</Columns>
					</asp:GridView>
				</div>
				<br />
			</div>
			<%--//Radio button for InStore or Shipping--%>
			<asp:RadioButton ID="RdbInStorePurchase" runat="server" Text="In Store" Checked="True" GroupName="rgSales" OnCheckedChanged="RdbInStorePurchase_CheckedChanged" AutoPostBack="true" />
			<asp:RadioButton ID="RdbShipping" runat="server" Text="Shipping" GroupName="rgSales" OnCheckedChanged="RdbShipping_CheckedChanged" AutoPostBack="true" />
			<asp:Label ID="lblShipping" runat="server" Text="Amount:" />
			<asp:TextBox ID="txtShippingAmount" runat="server" AutoCompleteType="Disabled" Text="0.00" />
			<asp:DropDownList ID="DdlShippingProvince" runat="server" DataTextField="varProvinceName" DataValueField="intProvinceID" 
				Enabled="false" Visible="false" OnSelectedIndexChanged="DdlShippingProvince_SelectedIndexChanged" AutoPostBack="true" />
			<asp:Label ID="lblShippingWarning" runat="server" Visible="false" />
			<div>
				<asp:Button ID="BtnJumpToInventory" Text="Jump to Inventory" OnClick="BtnJumpToInventory_Click" runat="server" />
			</div>

			<div style="text-align: right">
				<asp:Label ID="lblInvoiceNumber" runat="server" Text="Invoice No:" />
				<asp:Label ID="lblInvoiceNumberDisplay" runat="server" />
				<br />
				<asp:Label ID="lblDate" runat="server" Text="Date:" />
				<asp:Label ID="lblDateDisplay" runat="server" />
				<hr />
			</div>
			<div>
				<asp:TextBox ID="txtSearch" runat="server" AutoCompleteType="Disabled" />
				<asp:Button ID="BtnInventorySearch" runat="server" Width="150" Text="Inventory Search" OnClick="BtnInventorySearch_Click" />
				<asp:Button ID="BtnAddTradeIn" runat="server" Text="Add Trade In" Width="150" OnClick="BtnAddTradeIn_Click" />
				<asp:Button ID="BtnClearSearch" runat="server" Width="150" Text="Clear Search Results" OnClick="BtnClearSearch_Click" />				
				<asp:Button ID="BtnRefreshCart" runat="server" Text="Refresh Cart" Width="150" OnClick="BtnRefreshCart_Click" Visible="false" />
			</div>
			<hr />
			<asp:GridView ID="GrdInventorySearched" runat="server" AutoGenerateColumns="False" OnRowCommand="GrdInventorySearched_RowCommand" RowStyle-HorizontalAlign="Center">
				<Columns>
					<asp:TemplateField HeaderText="Add Item">
						<ItemTemplate>
							<asp:LinkButton Text="Add Item" runat="server" CommandName="AddItem" CausesValidation="false" CommandArgument='<%#Eval("intInventoryID") %>' />
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="varSku" HeaderText="SKU" />
					<asp:TemplateField HeaderText="In Stock">
						<ItemTemplate>
							<div>
								<asp:TextBox ID="quantityToAdd" runat="server" AutoCompleteType="Disabled" placeholder="Enter Quantity To Add" />
							</div>
							<div>
								<asp:Label ID="QuantityInOrder" Text='<%#Eval("intItemQuantity")%>' runat="server" />
							</div>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Description">
						<ItemTemplate>
							<asp:Label ID="Description" Text='<%#Eval("varItemDescription")%>' runat="server" />
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Price" ItemStyle-Width="50px">
						<ItemTemplate>
							<div class='cost' id="divRollOverSearch" runat="server">
								<asp:Label ID="rollPrice" runat="server" Text='<%#  (Eval("fltItemPrice","{0:C}")).ToString() %>' />
								<div id="divPriceConvert" class="costDetail" runat="server">
									<asp:Label ID="rollCost" runat="server" Text='<%# Convert.ToString(Eval("fltItemCost","{0:C}")).Replace("\n","<br/>") %>' />
								</div>
							</div>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Discount">
						<ItemTemplate>
							<div>
								<asp:CheckBox ID="chkDiscountPercent" runat="server" Text="Discount by Percent" />
							</div>
							<div>
								<asp:TextBox ID="txtAmountDiscount" runat="server" AutoCompleteType="Disabled" placeholder="Enter Amount" />
							</div>
						</ItemTemplate>
					</asp:TemplateField>					
					<asp:TemplateField HeaderText="Trade In" Visible="false">
						<ItemTemplate>
							<asp:CheckBox ID="chkTradeInSearch" Checked='<%# Eval("bitIsClubTradeIn") %>' runat="server" Enabled="false" />
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Type ID" Visible="false">
						<ItemTemplate>
							<asp:Label ID="lblTypeIDSearch" Text='<%# Eval("intItemTypeID") %>' runat="server" />
						</ItemTemplate>
					</asp:TemplateField>
				</Columns>
			</asp:GridView>
			<hr />
			<h3>Cart</h3>
			<hr />
			<asp:Label ID="lblInvalidQty" runat="server" Visible="false" Text="Invalid Quantity Entered" ForeColor="Red" />
			<asp:GridView ID="GrdCartItems" EmptyDataText=" No Records Found" runat="server" AutoGenerateColumns="false"
				Style="margin-right: 0px" OnRowEditing="OnRowEditing" OnRowDataBound="GrdCartItems_RowDataBound" 
				OnRowUpdating="OnRowUpdating" OnRowCancelingEdit="ORowCanceling" OnRowDeleting="OnRowDeleting" RowStyle-HorizontalAlign="Center">
				<Columns>
					<asp:TemplateField HeaderText="Remove Item">
						<ItemTemplate>
							<asp:LinkButton Text="Remove" runat="server" CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete?');" CausesValidation="false" />
							<asp:Label ID="lblInvoiceItemID" runat="server" Text='<%#Eval("intInvoiceItemID")%>' Visible="false" />
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Edit Item">
						<ItemTemplate>
							<asp:LinkButton Text="Edit" runat="server" CommandName="Edit" CommandArgument='<%#Eval("intInvoiceItemID")%>' CausesValidation="false" />
						</ItemTemplate>
						<EditItemTemplate>
							<asp:LinkButton Text="Update" runat="server" CommandName="Update" CommandArgument='<%#Eval("intInvoiceItemID")%>' CausesValidation="false" />
							<asp:LinkButton Text="Cancel" runat="server" CommandName="Cancel" CausesValidation="false" />
						</EditItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="varSku" ReadOnly="true" HeaderText="SKU" />
					<asp:BoundField DataField="intItemQuantity" HeaderText="Quantity" />
					<asp:BoundField DataField="varItemDescription" ReadOnly="true" HeaderText="Description" />
					<asp:TemplateField HeaderText="Price" ItemStyle-Width="50px">
						<ItemTemplate>
							<div class='cost' id="divRollOverCart" runat="server">
								<asp:Label ID="price" runat="server" Text='<%#  (Eval("fltItemPrice","{0:C}")).ToString() %>' />
								<div id="divCostConvert" class="costDetail" runat="server">
									<asp:Label ID="cost" runat="server" Text='<%# Convert.ToString(Eval("fltItemCost","{0:C}")).Replace("\n","<br/>") %>' />
								</div>
							</div>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Discount Amount">
						<ItemTemplate>
							<asp:CheckBox ID="ckbPercentageDisplay" Checked='<%# Convert.ToBoolean(Eval("bitIsDiscountPercent")) %>' runat="server" Text="Discount by Percent" Enabled="false" />
							<div id="divAmountDisplay" class="txt" runat="server">
								<asp:Label ID="lblAmountDisplay" runat="server" Text='<%# Eval("fltItemDiscount") %>' Enabled="false" />
							</div>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:CheckBox ID="ckbPercentageEdit" Checked='<%# Convert.ToBoolean(Eval("bitIsDiscountPercent")) %>' runat="server" Text="Discount by Percent" Enabled="true" />
							<div id="divAmountEdit" class="txt" runat="server">
								<asp:TextBox ID="txtAmnt" runat="server" AutoCompleteType="Disabled" Text='<%# Eval("fltItemDiscount") %>' Enabled="true" />
							</div>
						</EditItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Trade In" Visible="false">
						<ItemTemplate>
							<asp:CheckBox ID="chkTradeIn" Checked='<%# Eval("bitIsClubTradeIn") %>' runat="server" Enabled="false" />
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Type ID" Visible="false">
						<ItemTemplate>
							<asp:Label ID="lblTypeID" Text='<%# Eval("intItemTypeID") %>' runat="server" />
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Taxes" Visible="true">
						<ItemTemplate>
							<asp:CheckBoxList ID="cblTaxes" runat="server" Enabled="false" DataSource='<%# Eval("invoiceItemTaxes") %>' 
								DataTextField="varTaxName" DataValueField="bitIsTaxCharged" />
						</ItemTemplate>
					</asp:TemplateField>
				</Columns>
			</asp:GridView>
			<hr />
			<asp:Label ID="lblSubtotal" runat="server" Text="Subtotal:" />
			<asp:Label ID="lblSubtotalDisplay" runat="server" />
			<hr />
			<asp:Table runat="server">
				<asp:TableRow>
					<asp:TableCell>
						<asp:Button ID="BtnCancelSale" runat="server" Text="Void Transaction" OnClick="BtnCancelSale_Click" Width="163px" CausesValidation="false" />
					</asp:TableCell>
					<asp:TableCell>
						<asp:Button ID="BtnExitSale" runat="server" Text="Hold Sale" OnClick="BtnExitSale_Click" Width="163px" CausesValidation="false" />
					</asp:TableCell>
					<asp:TableCell>
						<%--<asp:Button ID="btnLayaway" runat="server" Text="Layaway" OnClick="btnLayaway_Click" Width="163px" CausesValidation="false" Visible="false" />--%>
					</asp:TableCell>
					<asp:TableCell>
						<asp:Button ID="BtnProceedToCheckout" runat="server" Text="Checkout" OnClick="BtnProceedToCheckout_Click" Width="163px" CausesValidation="false" />
					</asp:TableCell>
				</asp:TableRow>
			</asp:Table>
		</asp:Panel>
	</div>
</asp:Content>

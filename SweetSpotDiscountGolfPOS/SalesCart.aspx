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
    <%--<asp:Label ID="lblLocationID" runat="server" Text="Temp locaiton id label"></asp:Label>--%>
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
            <asp:Label ID="lblCustomer" runat="server" Text="Customer Name:"></asp:Label>
            <asp:Label ID="lblCustomerDisplay" runat="server" Text="" Visible="false"></asp:Label>
            <asp:TextBox ID="txtCustomer" runat="server"></asp:TextBox>
            <asp:Button ID="btnCustomerSelect" runat="server" Text="Select Different Customer" OnClick="btnCustomerSelect_Click" CausesValidation="false" />
            <div>


                <%--<div>
                    <asp:TextBox ID="txtCustomerName" runat="server"></asp:TextBox>
                    <asp:Button ID="btnSearchCustomers" runat="server" Text="Search" OnClick="btnSearchCustomers_Click" />
                </div>--%>


                <div>
                    <asp:GridView ID="grdCustomersSearched" runat="server" AutoGenerateColumns="false" ShowFooter="true" OnRowCommand="grdCustomersSearched_RowCommand" AllowPaging="True" PageSize="5" OnPageIndexChanging="grdCustomersSearched_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField HeaderText="Switch Customer">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnSwitchCustomer" CommandName="SwitchCustomer" CommandArgument='<%#Eval("CustomerId") %>' Text="Switch Customer" runat="server">Switch Customer</asp:LinkButton>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Button ID="btnAddCustomer" runat="server" Text="Add Customer" OnClick="btnAddCustomer_Click" />
                                </FooterTemplate>
                            </asp:TemplateField>                            
                            <asp:TemplateField HeaderText="Customer Name">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("firstName") + " " + Eval("lastName") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <div>
                                        <asp:TextBox ID="txtFirstName" runat="server" placeholder="First Name" ToolTip="First Name"></asp:TextBox>
                                    </div>
                                    <div>
                                        <asp:TextBox ID="txtLastName" runat="server" placeholder="Last Name" ToolTip="Last Name"></asp:TextBox>
                                    </div>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Phone Number">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("primaryPhoneNumber") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtPhoneNumber" runat="server" placeholder="Phone Number" ToolTip="Phone Number"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>





            </div>
            <br />
            <br />
            <%--//Radio button for InStore or Shipping--%>
            <asp:RadioButton ID="RadioButton1" runat="server" Text="In Store" Checked="True" GroupName="rgSales" />
            <asp:RadioButton ID="RadioButton2" runat="server" Text="Shipping" GroupName="rgSales"/>
            <asp:Label ID="lblShipping" runat="server" Text="Shipping Amount:"></asp:Label>
            <asp:TextBox ID="txtShippingAmount" runat="server"></asp:TextBox>
            <div>
            <asp:Button ID="btnJumpToInventory" Text="Jump to Inventory" OnClick="btnJumpToInventory_Click" runat="server" /></div>

            <div style="text-align: right">
                <asp:Label ID="lblInvoiceNumber" runat="server" Text="Invoice No:"></asp:Label>
                <asp:Label ID="lblInvoiceNumberDisplay" runat="server"></asp:Label>
                <br />
                <asp:Label ID="lblDate" runat="server" Text="Date:"></asp:Label>
                <asp:Label ID="lblDateDisplay" runat="server" Text=""></asp:Label>
                <hr />
            </div>
            <div>
                <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
                <asp:Button ID="btnInventorySearch" runat="server" Width="150" Text="Inventory Search" OnClick="btnInventorySearch_Click" />
               
            </div>
            <hr />
            <asp:GridView ID="grdInventorySearched" runat="server" AutoGenerateColumns="False" OnRowCommand="grdInventorySearched_RowCommand">
                <Columns>
                    <asp:TemplateField HeaderText="Add Item">
                        <ItemTemplate>
                            <asp:LinkButton Text="Add Item" runat="server" CommandName="AddItem" CommandArgument='<%#Eval("sku")%>' CausesValidation="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="sku" HeaderText="SKU" />
                    <asp:TemplateField HeaderText="Quantity">
                        <ItemTemplate>
                            <asp:Label ID="QuantityInOrder" Text='<%#Eval("quantity")%>' runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Description">
                        <ItemTemplate>
                            <asp:Label ID="Description" Text='<%#Eval("description")%>' runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Price" ItemStyle-Width="50px">
                        <ItemTemplate>
                            <div class='cost' id="divRollOverSearch" runat="server">
                                <%#  (Eval("price","{0:C}")).ToString() %>
                                <div id="divPriceConvert" class="costDetail" runat="server">
                                    <%# Convert.ToString(Eval("cost","{0:C}")).Replace("\n","<br/>") %>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            
            <hr />
            <h3>Cart</h3>
            <hr />
            <asp:Label ID="lblInvalidQty" runat="server" Visible="false" Text="Label"></asp:Label>
            <asp:GridView ID="grdCartItems" EmptyDataText=" No Records Found" runat="server" AutoGenerateColumns="false" Style="margin-right: 0px" OnRowEditing="OnRowEditing" OnRowUpdating="OnRowUpdating" OnRowCancelingEdit="ORowCanceling" OnRowDeleting="OnRowDeleting">
                <Columns>
                    <%--<asp:CommandField ShowDeleteButton="True" ButtonType="Link" />--%>
                    <asp:TemplateField HeaderText="Remove Item">
                        <ItemTemplate>
                            <asp:LinkButton Text="Remove" runat="server" CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete?');" CausesValidation="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Edit Item">
                        <ItemTemplate>
                            <asp:LinkButton Text="Edit" runat="server" CommandName="Edit" CommandArgument='<%#Eval("sku")%>' CausesValidation="false" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton Text="Update" runat="server" CommandName="Update" CommandArgument='<%#Eval("sku")%>' CausesValidation="false" />
                            <asp:LinkButton Text="Cancel" runat="server" CommandName="Cancel" CausesValidation="false" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="sku" ReadOnly="true" HeaderText="SKU" />
                    <asp:BoundField DataField="quantity" HeaderText="Quantity" />
                    <asp:BoundField DataField="description" ReadOnly="true" HeaderText="Description" />
                    <asp:TemplateField HeaderText="Price" ItemStyle-Width="50px">
                        <ItemTemplate>
                            <div class='cost' id="divRollOverCart" runat="server">
                                <asp:Label ID="price" runat="server" Text='<%#  (Eval("price","{0:C}")).ToString() %>'></asp:Label>
                                <div id="divCostConvert" class="costDetail" runat="server">
                                    <asp:Label ID="cost" runat="server" Text='<%# Convert.ToString(Eval("cost","{0:C}")).Replace("\n","<br/>") %>'></asp:Label>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Discount Amount">
                        <ItemTemplate>
                            <asp:CheckBox ID="ckbPercentageDisplay" Checked='<%# Convert.ToBoolean(Eval("percentage")) %>' runat="server" Text="Discount by Percent" Enabled="false"/>
                            <div id="divAmountDisplay" class="txt" runat="server">
                                <asp:Label ID="lblAmountDisplay" runat="server" Text='<%# Eval("discount") %>' Enabled="false"></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:CheckBox ID="ckbPercentageEdit" Checked='<%# Convert.ToBoolean(Eval("percentage")) %>' runat="server" Text="Discount by Percent" Enabled="true"/>
                            <div id="divAmountEdit" class="txt" runat="server">
                                <asp:TextBox ID="txtAmnt" runat="server" Text='<%# Eval("discount") %>' Enabled="true"></asp:TextBox>
                            </div>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Trade In" Visible="false">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkTradeIn" Checked='<%# Eval("tradeIn") %>' runat="server" Enabled="false"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Type ID" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblTypeID" Text='<%# Eval("typeID") %>' runat="server"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <hr />
            <asp:Label ID="lblSubtotal" runat="server" Text="Subtotal:"></asp:Label>
            <asp:Label ID="lblSubtotalDisplay" runat="server" Text=""></asp:Label>
            <hr />
            <asp:Button ID="btnCancelSale" runat="server" Text="Cancel Sale" OnClick="btnCancelSale_Click" CausesValidation="false" />
            <asp:Button ID="btnProceedToCheckout" runat="server" Text="Proceed to Checkout" OnClick="btnProceedToCheckout_Click" CausesValidation="false" />
        </asp:Panel>
    </div>
</asp:Content>

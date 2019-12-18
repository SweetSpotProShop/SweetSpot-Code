<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReturnsCart.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.ReturnsCart" %>

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
<asp:Content ID="ReturnsCartPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <div id="ReturnCart">
        <asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="btnProceedToReturnCheckout">
            <asp:Label ID="lblCustomer" runat="server" Text="Customer Name:" />
            <asp:Label ID="lblCustomerDisplay" runat="server" />
            <br />
            <br />
            <div style="text-align: right">
                <asp:Label ID="lblInvoiceNumber" runat="server" Text="Invoice No:" />
                <asp:Label ID="lblInvoiceNumberDisplay" runat="server" />
                <br />
                <asp:Label ID="lblDate" runat="server" Text="Date:" />
                <asp:Label ID="lblDateDisplay" runat="server" />
                <hr />
            </div>
            <h3>Available Items</h3>
            <hr />
            <asp:Label ID="lblInvalidQty" runat="server" Visible="false" Text="Invalid Quantity Entered" ForeColor="Red" />
            <asp:GridView ID="grdInvoicedItems" runat="server" AutoGenerateColumns="false" OnRowDeleting="grdInvoicedItems_RowDeleting" RowStyle-HorizontalAlign="Center" >
                <Columns>
                    <asp:TemplateField HeaderText="Return Item">
                        <ItemTemplate>
                            <asp:LinkButton ID="lkbReturnItem" Text="Return Item" CommandName="Delete" CommandArgument='<%#Eval("intInvoiceItemID") %>' runat="server" CausesValidation="false" />
							<asp:Label ID="lblInvoiceItemID" Text='<%#Eval("intInvoiceItemID") %>' runat="server" Visible="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="varSku" ReadOnly="true" HeaderText="SKU" />
                    <asp:TemplateField HeaderText="Quantity">
                        <ItemTemplate>
                            <div>
                                <asp:Label ID="quantitySold" runat="server" Text='<%#Eval("intItemQuantity") %>' />
                            </div>
                            <div>
                                <asp:TextBox ID="quantityToReturn" runat="server" AutoComplete="off" placeholder="Enter Quantity To Return" />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="varItemDescription" ReadOnly="true" HeaderText="Description" />
                    <asp:TemplateField HeaderText="Paid">
                        <ItemTemplate>
                            <%# Convert.ToBoolean(Eval("bitIsDiscountPercent")) == false ? ((Convert.ToDouble(Eval("fltItemPrice")))-(Convert.ToDouble(Eval("fltItemDiscount")))).ToString("C") : ((Convert.ToDouble(Eval("fltItemPrice")) - ((Convert.ToDouble(Eval("fltItemDiscount")) / 100) * Convert.ToDouble(Eval("fltItemPrice"))))).ToString("C") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Discount Applied" Visible="false">
                        <ItemTemplate>
                            <asp:CheckBox ID="ckbPercentage" Checked='<%# Convert.ToBoolean(Eval("bitIsDiscountPercent")) %>' runat="server" Text="Discount by Percent" Enabled="false" />
                            <div id="divReturnAmountDiscount" class="txt" runat="server">
                                <asp:Label ID="lblReturnAmountDisplay" runat="server" Text='<%# Eval("fltItemDiscount") %>' Enabled="false" />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Type ID" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblReturnTypeID" Text='<%# Eval("intItemTypeID") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Amount to Refund">
                        <ItemTemplate>
                            <asp:TextBox ID="txtReturnAmount" runat="server" AutoComplete="off" Text='<%# Convert.ToBoolean(Eval("bitIsDiscountPercent")) == false ? ((Convert.ToDouble(Eval("fltItemPrice")))-(Convert.ToDouble(Eval("fltItemDiscount")))).ToString("#0.00") : ((Convert.ToDouble(Eval("fltItemPrice")) - ((Convert.ToDouble(Eval("fltItemDiscount")) / 100) * Convert.ToDouble(Eval("fltItemPrice"))))).ToString("#0.00") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <hr />
            <h3>Return Cart</h3>
            <hr />
            <asp:GridView ID="grdReturningItems" runat="server" AutoGenerateColumns="false" OnRowDeleting="grdReturningItems_RowDeleting" RowStyle-HorizontalAlign="Center" >
                <Columns>
                    <asp:TemplateField HeaderText="Cancel Return">
                        <ItemTemplate>
                            <asp:LinkButton ID="lkbCancelItem" Text="Cancel Return" CommandName="Delete" CommandArgument='<%#Eval("intInvoiceItemID") %>' runat="server" CausesValidation="false" />
							<asp:Label ID="lblInvoiceItemReturnID" Text='<%#Eval("intInvoiceItemID") %>' runat="server" Visible="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="varSku" ReadOnly="true" HeaderText="SKU" />
                    <asp:BoundField DataField="intItemQuantity" ReadOnly="true" HeaderText="Quantity" />
                    <asp:BoundField DataField="varItemDescription" ReadOnly="true" HeaderText="Description" />
                    <asp:BoundField DataField="fltItemRefund" ReadOnly="true" HeaderText="Refund Amount" DataFormatString="{0:C}" />
                    <asp:TemplateField HeaderText="Discount Applied" Visible="false">
                        <ItemTemplate>
                            <asp:CheckBox ID="ckbRIPercentage" Checked='<%# Convert.ToBoolean(Eval("bitIsDiscountPercent")) %>' runat="server" Text="Discount by Percent" Enabled="false" />
                            <div id="divRIReturnAmountDiscount" class="txt" runat="server">
                                <asp:Label ID="lblRIReturnAmountDisplay" runat="server" Text='<%# Eval("fltItemDiscount") %>' Enabled="false" />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Type ID" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblReturnedTypeID" Text='<%# Eval("intItemTypeID") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <hr />
            <asp:Label ID="lblReturnSubtotal" runat="server" Text="Return Subtotal:" />
            <asp:Label ID="lblReturnSubtotalDisplay" runat="server" />
            <hr />
            <asp:Button ID="btnCancelReturn" runat="server" Text="Void Transaction" OnClick="btnCancelReturn_Click" CausesValidation="false" />
            <asp:Button ID="btnProceedToReturnCheckout" runat="server" Text="Checkout" OnClick="btnProceedToReturnCheckout_Click" CausesValidation="false" />
        </asp:Panel>
    </div>
</asp:Content>
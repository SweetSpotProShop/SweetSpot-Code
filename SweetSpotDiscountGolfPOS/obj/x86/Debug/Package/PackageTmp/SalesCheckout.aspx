<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="SalesCheckout.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.SalesCheckout" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>--%>

<asp:Content ID="NonActive" ContentPlaceHolderID="SPMaster" runat="server">
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
<asp:Content ID="CheckoutPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <h3>Transaction Details</h3>
    <div>
        <%--REMEMBER TO SET DEFAULT BUTTON--%>
        <asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="mopCash">
            <table>
                <tr>
                    <td colspan="2" class="auto-style1">
                        <table>
                            <tr>
                                <td colspan="2" style="text-align: center">Methods of Payment</td>
                            </tr>
                            <tr>
                                <td>
                                    <%--<asp:Button ID="mopAmericanExpress" runat="server" Text="American Express" OnClick="mopAmericanExpress_Click" Width="163px" OnClientClick="return confirm('Confirm American Express');" />--%>
                                    <asp:Button ID="mopCash" runat="server" Text="Cash" OnClick="mopCash_Click" Width="163px" OnClientClick="return confirm('Confirm Cash');" />
                                    
                                </td>
                                <td>
                                    <asp:Button ID="mopVisa" runat="server" Text="Visa" OnClick="mopVisa_Click" Width="163px" OnClientClick="return confirm('Confirm Visa');" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="mopMasterCard" runat="server" Text="MasterCard" OnClick="mopMasterCard_Click" Width="163px" OnClientClick="return confirm('Confirm MasterCard');" />
                                </td>
                                <td>
                                    <%--<asp:Button ID="mopCheque" runat="server" Text="Cheque" OnClick="mopCheque_Click" Width="163px" OnClientClick="return confirm('Confirm Cheque');" />--%>
                                    <asp:Button ID="mopDebit" runat="server" Text="Debit" OnClick="mopDebit_Click" Width="163px" OnClientClick="return confirm('Confirm Debit');" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="mopGiftCard" runat="server" Text="Gift Card" OnClick="mopGiftCard_Click" Width="163px" OnClientClick="return confirm('Confirm Gift Card');" />
                                </td>
                                <td>
                                    
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    
                                    <%--<asp:Button ID="mopOnAccount" runat="server" Text="Charge to Account" OnClick="mopOnAccount_Click" Width="163px" />--%>
                                </td>
                                <td>
                                   <%-- <asp:Button ID="mopGiftCard" runat="server" Text="Gift Card" OnClick="mopGiftCard_Click" Width="163px" OnClientClick="return confirm('Confirm Gift Card');" />--%>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblAmountPaid" runat="server" Text="Owing:" Width="163px"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAmountPaying" runat="server" Width="159px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>


                    </td>
                    <td colspan="2" class="auto-style1">
                        <asp:Table ID="tblTotals" runat="server">
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblTotalInCart" runat="server" Text="Total In Cart:"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblTotalInCartAmount" runat="server" Text=""></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblTotalInDiscounts" runat="server" Text="Total Discounts:"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblTotalInDiscountsAmount" runat="server" Text=""></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblTradeIns" runat="server" Text="Trade-Ins:"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblTradeInsAmount" runat="server" Text=""></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblShipping" runat="server" Text="Shipping:"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblShippingAmount" runat="server" Text=""></asp:Label>
                                    <%--<asp:TextBox ID="txtShippingAmount" runat="server" Text="" AutoPostBack="True"></asp:TextBox>--%>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblSubTotal" runat="server" Text="Subtotal:"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblSubTotalAmount" runat="server" Text=""></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblGovernment" runat="server" Text="GST:" Visible="false"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblGovernmentAmount" runat="server" Text="" Visible="false"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Button ID="btnRemoveGov" runat="server" Text="Remove GST" Width="163px" OnClick="btnRemoveGovTax" Visible="false" />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblProvincial" runat="server" Text="PST:" Visible="false"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblProvincialAmount" runat="server" Text="" Visible="false"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Button ID="btnRemoveProv" runat="server" Text="Remove PST" Width="163px" OnClick="btnRemoveProvTax" Visible="false" />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblBalance" runat="server" Text="Balance Due:"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblBalanceAmount" runat="server" Text=""></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </td>
                </tr>



                <tr>
                    <td colspan="4">
                        <asp:GridView ID="gvCurrentMOPs" runat="server" AutoGenerateColumns="false" Width="100%" OnRowDeleting="OnRowDeleting">
                            <Columns>
                                <asp:TemplateField HeaderText="Remove">
                                    <ItemTemplate>
                                        <asp:LinkButton Text="Remove MOP" runat="server" CommandName="Delete" OnClientClick="return confirm('Are you sure you want to remove this Method of Payment?');" CausesValidation="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="methodOfPayment" ReadOnly="true" HeaderText="Payment Type" />
                                <asp:BoundField DataField="amountPaid" ReadOnly="true" HeaderText="Amount Paid" DataFormatString="{0:0.00}" />
                                <asp:TemplateField HeaderText="Table ID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTableID" Text='<%#Eval("tableID") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                        <asp:Label ID="lblRemainingBalanceDue" runat="server" Text="Remaining Balance Due"></asp:Label>
                    </td>
                    <td colspan="2">
                        <hr />
                        <asp:Label ID="lblRemainingBalanceDueDisplay" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnCancelSale" runat="server" Text="Cancel Sale" OnClick="btnCancelSale_Click" Width="163px" />
                    </td>
                    <td>
                        <asp:Button ID="btnReturnToCart" runat="server" Text="Return To Cart" OnClick="btnReturnToCart_Click" Width="163px" />
                    </td>
                    <td>
                        <asp:Button ID="btnLayaway" runat="server" Text="Layaway" OnClick="btnLayaway_Click" Width="163px" Visible="false" />
                    </td>
                    <td>
                        <asp:Button ID="btnFinalize" runat="server" Text="Process Sale" OnClick="btnFinalize_Click" Width="163px" />                        
                    </td>
                    <td>
                        <asp:CheckBox ID="chbxDoesNotEqualZero" Text="Check box to bypass MOPs" runat="server" />
                    </td>
                </tr>
            </table>
            <p>
                Comments:
               <br />
                <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine"></asp:TextBox>
            </p>
        </asp:Panel>
    </div>
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .auto-style1 {
            height: 152px;
        }
    </style>
</asp:Content>
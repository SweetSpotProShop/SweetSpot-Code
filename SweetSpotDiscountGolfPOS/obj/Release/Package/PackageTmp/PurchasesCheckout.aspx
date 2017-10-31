<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="PurchasesCheckout.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.PurchasesCheckout" %>

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
<asp:Content ID="PurchaseCheckOutPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
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
                                    <asp:Button ID="mopCash" runat="server" Text="Cash" OnClick="mopCash_Click" Width="163px" OnClientClick="return confirm('Confirm Cash');" />

                                </td>
                                <td>
                                    <asp:Button ID="mopCheque" runat="server" Text="Cheque" OnClick="mopCheque_Click" Width="163px" OnClientClick="return confirm('Confirm Cheque');" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="mopDebit" runat="server" Text="Debit" OnClick="mopDebit_Click" Width="163px" OnClientClick="return confirm('Confirm Debit');" />
                                </td>
                                <td>
                                    <asp:Button ID="mopGiftCard" runat="server" Text="Gift Card" OnClick="mopGiftCard_Click" Width="163px" OnClientClick="return confirm('Confirm Gift Card');" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblPurchaseAmount" runat="server" Text="Purchase Amount:" Width="163px"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPurchaseAmount" runat="server" Width="159px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td colspan="2" class="auto-style1">
                        <asp:Table ID="tblTotals" runat="server">
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblChequeNumber" runat="server" Text="Enter Cheque Number:"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="txtChequeNumber" runat="server" Text="0000" />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblTotalPurchase" runat="server" Text="Total Purchases:"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblTotalPurchaseAmount" runat="server" Text=""></asp:Label>
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
                                <asp:BoundField DataField="amountPaid" ReadOnly="true" HeaderText="Amount Paid" DataFormatString="{0:C}" />
                                <asp:BoundField DataField="chequeNum" ReadOnly="true" HeaderText="Cheque Number" />
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
                        <asp:Label ID="lblRemainingPurchaseDue" runat="server" Text="Remaining Purchase Due"></asp:Label>
                    </td>
                    <td colspan="2">
                        <hr />
                        <asp:Label ID="lblRemainingPurchaseDueDisplay" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnCancelPurchase" runat="server" Text="Cancel Purchase" OnClick="btnCancelPurchase_Click" Width="163px" />
                    </td>
                    <td>
                        <asp:Button ID="btnReturnToPurchaseCart" runat="server" Text="Return To Purchases" OnClick="btnReturnToPurchaseCart_Click" Width="163px" />
                    </td>
                    <td>
                        <asp:Button ID="btnFinalizePurchase" runat="server" Text="Process Purchase" OnClick="btnFinalizePurchase_Click" Width="163px" />
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
<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReturnsCheckout.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.ReturnsCheckout" %>

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
<asp:Content ID="ReturnsCheckoutPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <h3>Transaction Details</h3>
    <div>
        <%--REMEMBER TO SET DEFAULT BUTTON--%>
        <asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="mopCash">
            <asp:Table runat="server">
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="2" CSSClass="auto-style1">
                        <asp:Table runat="server">
                            <asp:TableRow>
                                <asp:TableCell ColumnSpan="2" Style="text-align: center">Methods For Refund</asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Button ID="mopCash" runat="server" Text="Cash" OnClick="mopCash_Click" Width="163px" OnClientClick="return confirm('Confirm Cash');" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Button ID="mopVisa" runat="server" Text="Visa" OnClick="mopVisa_Click" Width="163px" OnClientClick="return confirm('Confirm Visa');" />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Button ID="mopMasterCard" runat="server" Text="MasterCard" OnClick="mopMasterCard_Click" Width="163px" OnClientClick="return confirm('Confirm MasterCard');" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Button ID="mopDebit" runat="server" Text="Debit" OnClick="mopDebit_Click" Width="163px" OnClientClick="return confirm('Confirm Debit');" />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Button ID="mopGiftCard" runat="server" Text="Gift Card" OnClick="mopGiftCard_Click" Width="163px" OnClientClick="return confirm('Confirm Gift Card');" />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell ColumnSpan="2">
                                    <hr />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblRefundAmount" runat="server" Text="Refund Amount:" Width="163px" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="txtAmountRefunding" runat="server" Width="159px" />
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </asp:TableCell>
                    <asp:TableCell ColumnSpan="2" CSSClass="auto-style1">
                        <asp:Table ID="tblTotals" runat="server">
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblRefundSubTotal" runat="server" Text="Refund Subtotal:" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblRefundSubTotalAmount" runat="server" Text="" />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblGovernment" runat="server" Text="GST:" Visible="false" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblGovernmentAmount" runat="server" Text="" Visible="false" />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblProvincial" runat="server" Text="PST:" Visible="false" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblProvincialAmount" runat="server" Text="" Visible="false" />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblRefundBalance" runat="server" Text="Total Refund Amount:" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblRefundBalanceAmount" runat="server" Text="" />
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="4">
                        <asp:GridView ID="gvCurrentMOPs" runat="server" AutoGenerateColumns="false" Width="100%" OnRowDeleting="OnRowDeleting" RowStyle-HorizontalAlign="Center" >
                            <Columns>
                                <asp:TemplateField HeaderText="Remove">
                                    <ItemTemplate>
                                        <asp:LinkButton Text="Remove Refund Method" runat="server" CommandName="Delete" OnClientClick="return confirm('Are you sure you want to remove this Method of Payment?');" CausesValidation="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="mopType" ReadOnly="true" HeaderText="Refund Type" />
                                <asp:BoundField DataField="amountPaid" ReadOnly="true" HeaderText="Refund Amount" DataFormatString="{0:C}" />
                                <asp:TemplateField HeaderText="Table ID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTableID" Text='<%#Eval("id") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="2">
                        <hr />
                        <asp:Label ID="lblRemainingRefund" runat="server" Text="Remaining Refund:" />
                    </asp:TableCell>
                    <asp:TableCell ColumnSpan="2">
                        <hr />
                        <asp:Label ID="lblRemainingRefundDisplay" runat="server" Text=""></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Button ID="btnCancelReturn" runat="server" Text="Cancel Return" OnClick="btnCancelReturn_Click" Width="163px" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="btnReturnToCart" runat="server" Text="Return To Cart" OnClick="btnReturnToCart_Click" Width="163px" />
                    </asp:TableCell>
                    <asp:TableCell>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="btnFinalize" runat="server" Text="Process Refund" OnClick="btnFinalize_Click" Width="163px" />                        
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <p>
                Comments:
               <br />
                <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" />
            </p>
        </asp:Panel>
    </div>
    <script>
        function userInput(owing) {
            var given = prompt("Enter the amount of cash", "");
            var change = owing - given;
            if (change < 0) {
                var give = String(change.toFixed(2));
                alert("Change: " + give);
            }
            else if (change >= 0) {   
            }
        }
    </script>
</asp:Content>
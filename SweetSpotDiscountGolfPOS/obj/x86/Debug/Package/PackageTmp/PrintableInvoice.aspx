<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="PrintableInvoice.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.PrintableInvoice" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>--%>
<asp:Content ID="NonActive" ContentPlaceHolderID="SPMaster" runat="server" >
    <style media="print">
		.noPrint {
			display: none;
			margin-left: 0;
		}
		.yesPrint {
			display: inline-block !important;
			/* margin-right:100px;
           float: right;*/
			margin-left: 10px !important;
		}
	</style>
    <div id="menu_simple" class="noPrint">
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
        <img src="Images/combinedLogo.jpg" />
    </div>
    <link rel="stylesheet" type="text/css" href="CSS/MainStyleSheet.css" />
</asp:Content>
<asp:Content ID="printableInvoiceDisplay" ContentPlaceHolderID="IndividualPageContent" runat="server" >

    <script>
        function printReport(printable) {
            window.print();
        }
    </script>
    <link rel="stylesheet" type="text/css" href="CSS/displayPrintableInvoice.css" />
    <div id="printable" runat="server">
        <div id="Invoice" class="yesPrint">
            <h3><b>Invoice: </b>
                <asp:Label ID="lblinvoiceNum" runat="server" Text=""></asp:Label></h3>
            <p>
                Tax Number:
                <asp:Label ID="lblTaxNum" runat="server" Text=""></asp:Label>
            </p>
            <p>
                Date:
                <asp:Label ID="lblDate" runat="server" Text=""></asp:Label>
            </p>
            <hr />
        </div>
        <br />
        <div id="finalInvoice" class="yesPrint">
            <asp:Table ID="tblPartiesInvolved" runat="server" Width="100%">
                <asp:TableRow>
                    <asp:TableCell CssClass="leftSide">
                        <h3>
                            <asp:Label ID="lblCustomerName" runat="server" Text=""></asp:Label></h3>
                    </asp:TableCell>
                    <asp:TableCell CssClass="rightSide">
                        <h3>
                            <asp:Label ID="lblSweetShopName" runat="server" Text="Sweet Spot Discount Golf"></asp:Label></h3>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="leftSide">
                        <asp:Label ID="lblStreetAddress" runat="server" Text=""></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell CssClass="rightSide">
                        <asp:Label ID="lblSweetShopStreetAddress" runat="server" Text="644 Main St. N"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="leftSide">
                        <asp:Label ID="lblPostalAddress" runat="server" Text=""></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell CssClass="rightSide">
                        <asp:Label ID="lblSweetShopPostalAddress" runat="server" Text="Moose Jaw, Saskatchewan S6H 3K4"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="leftSide">
                        <asp:Label ID="lblPhone" runat="server" Text=""></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell CssClass="rightSide">
                        <asp:Label ID="lblSweetShopPhone" runat="server" Text="(306) 692-8337"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <hr />

            <asp:GridView ID="grdItemsSoldList" runat="server" CellPadding="4" Width="70%" AutoGenerateColumns="False">
                <Columns>
                    <asp:TemplateField HeaderText="SKU #">
                        <ItemTemplate>
                            <asp:Label ID="sku" Text='<%#Eval("SKU")%>' runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Description">
                        <ItemTemplate>
                            <asp:Label ID="description" Text='<%#Eval("description")%>' runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Quantity">
                        <ItemTemplate>
                            <asp:Label ID="quantity" Text='<%#Eval("quantity")%>' runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Retail Price">
                        <ItemTemplate>
                            <asp:Label ID="retail" Text='<%#Eval("price","{0:.00}")%>' runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Discounts/Bonus Applied">
                        <ItemTemplate>
                            <%# Convert.ToBoolean(Eval("percentage")) == false ? (Eval("discount","{0:.00}")).ToString() : ((Convert.ToDouble(Eval("discount")) / 100) * Convert.ToDouble(Eval("price"))).ToString("#0.00") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Extended Price">
                        <ItemTemplate>
                            <%# Convert.ToBoolean(Eval("percentage")) == false ? ((Convert.ToDouble(Eval("price")))-(Convert.ToDouble(Eval("discount")))).ToString("#0.00") : ((Convert.ToDouble(Eval("price")) - ((Convert.ToDouble(Eval("discount")) / 100) * Convert.ToDouble(Eval("price"))))).ToString("#0.00") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <hr />
        </div>
        <div id="paymentDetails" class="yesPrint">
            <h3>Payment Details</h3>

            <asp:Table ID="tblSummary" runat="server" Width="70%">
                <asp:TableRow>
                    <asp:TableCell CssClass="leftFirst">
                        <asp:Label ID="lblDiscounts" runat="server" Text="Discounts:"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell CssClass="leftSecond">
                        <asp:Label ID="lblDiscountsDisplay" runat="server" Text=""></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell CssClass="rightFirst">
                        <asp:Label ID="lblBlank" runat="server" Text=""></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell CssClass="rightSecond">
                        <asp:Label ID="lblBlankDisplay" runat="server" Text=""></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="leftFirst">
                        <asp:Label ID="lblTradeIns" runat="server" Text="Trade-Ins:"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell CssClass="leftSecond">
                        <asp:Label ID="lblTradeInsDisplay" runat="server" Text=""></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell CssClass="rightFirst">
                        <asp:Label ID="lblGST" runat="server" Text="GST:"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell CssClass="rightSecond">
                        <asp:Label ID="lblGSTDisplay" runat="server" Text=""></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="leftFirst">
                        <asp:Label ID="lblShipping" runat="server" Text="Shipping:"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell CssClass="leftSecond">
                        <asp:Label ID="lblShippingDisplay" runat="server" Text=""></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell CssClass="rightFirst">
                        <asp:Label ID="lblPST" runat="server" Text="PST:"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell CssClass="rightSecond">
                        <asp:Label ID="lblPSTDisplay" runat="server" Text=""></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="leftFirst">
                        <asp:Label ID="lblSubtotal" runat="server" Text="Subtotal:"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell CssClass="leftSecond">
                        <asp:Label ID="lblSubtotalDisplay" runat="server" Text=""></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell CssClass="rightFirst">
                        <asp:Label ID="lblTotalPaid" runat="server" Text="Total Paid:"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell CssClass="rightSecond">
                        <asp:Label ID="lblTotalPaidDisplay" runat="server" Text=""></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <p>
                <asp:GridView ID="grdMOPS" runat="server" CellPadding="4" Width="70%" AutoGenerateColumns="false">
                    <Columns>
                        <asp:BoundField DataField="methodOfPayment" ReadOnly="true" HeaderText="Payment Type" />
                        <asp:BoundField DataField="amountPaid" ReadOnly="true" HeaderText="Amount Paid" DataFormatString="{0:0.00}" />
                    </Columns>
                </asp:GridView>
            </p>
            <br />
            <div class="noPrint">
                <%--added a cssclass here for testing--%>
                <asp:Button ID="btnPrint" CssClass="noPrint" runat="server" Text="Print" Width="100px" OnClientClick="printReport()" />
                <br />
                <asp:Button ID="btnHome" runat="server" Text="Home" Width="100px" OnClick="btnHome_Click" />
                <br />
                <%--<p><b>PLEASE NOTE: </b>All used equipment is sold as is and it is understood that its' condition</p>
                <p>and usability may reflect prior use. The Sweet Spot Discount Golf assumes no responsibility</p>
                <p>beyond the point of sale. <b>ALL SALES FINAL</b> Thank you for shopping at the Sweet Spot.</p>--%>
            </div>
            <p><b>PLEASE NOTE: </b>All used equipment is sold as is and it is understood that its' condition</p>
                <p>and usability may reflect prior use. The Sweet Spot Discount Golf assumes no responsibility</p>
                <p>beyond the point of sale. <b>ALL SALES FINAL</b> Thank you for shopping at the Sweet Spot.</p>
        </div>
    </div>

</asp:Content>

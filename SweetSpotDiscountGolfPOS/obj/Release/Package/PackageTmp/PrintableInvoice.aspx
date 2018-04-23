﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="PrintableInvoice.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.PrintableInvoice" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>--%>
<asp:Content ID="NonActive" ContentPlaceHolderID="SPMaster" runat="server">
    <style media="print">
        .noPrint {
            display: none;
            /*margin-left: 0;*/
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

    <link rel="stylesheet" type="text/css" href="CSS/MainStyleSheet.css" />
</asp:Content>
<asp:Content ID="printableInvoiceDisplay" ContentPlaceHolderID="IndividualPageContent" runat="server">

    <script>
        function printReport(printable) {
            window.print();
        }

        function CallPrint(mainPage, disclaimer) {

            var prtContent = document.getElementById(mainPage);
            var prtDisclaimer = document.getElementById(disclaimer);
            var WinPrint = window.open('', '', 'letf=10,top=10,width="450",height="250",toolbar=1,scrollbars=1,status=0');

            WinPrint.document.write("<html><head><LINK rel=\"stylesheet\" type\"text/css\" href=\"css/print.css\" media=\"print\"><LINK rel=\"stylesheet\" type\"text/css\" href=\"css/print.css\" media=\"screen\"></head><body>");

            WinPrint.document.write(prtContent.innerHTML);
            WinPrint.document.write(prtDisclaimer.innerHTML);
            WinPrint.document.write("</body></html>");
            WinPrint.document.close();
            WinPrint.focus();
            WinPrint.print();
            WinPrint.close();

            return false;
        }

    </script>
    <link rel="stylesheet" type="text/css" href="CSS/displayPrintableInvoice.css" />
    <div id="print">
        <asp:Table runat="server">
            <asp:TableRow>
                <asp:TableCell CssClass="leftSide">
                    <div id="image_simple">
                        <img src="Images/combinedLogo.jpg" />
                    </div>
                </asp:TableCell>
                <asp:TableCell>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </asp:TableCell>
                <asp:TableCell CssClass="rightSide">
                    <div id="Invoice" class="yesPrint">
                        <b>Invoice: </b>
                        <asp:Label ID="lblinvoiceNum" runat="server" Text="" />
                        <br />
                        Tax Number:
                        <asp:Label ID="lblTaxNum" runat="server" Text="" />
                        <br />
                        Date:
                        <asp:Label ID="lblDate" runat="server" Text="" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblTime" runat="server" Text="" />
                    </div>
                </asp:TableCell></asp:TableRow></asp:Table><hr />
        <div id="finalInvoice" class="yesPrint">
            <asp:Table ID="tblPartiesInvolved" runat="server" Width="100%">
                <asp:TableRow>
                    <asp:TableCell CssClass="leftSide">
                        <asp:Label ID="lblCustomerName" runat="server" Text="" />
                    </asp:TableCell><asp:TableCell CssClass="rightSide">
                        <asp:Label ID="lblSweetShopName" runat="server" Text="Sweet Spot Discount Golf" />
                    </asp:TableCell></asp:TableRow><asp:TableRow>
                    <asp:TableCell CssClass="leftSide">
                        <asp:Label ID="lblStreetAddress" runat="server" Text=""></asp:Label>
                    </asp:TableCell><asp:TableCell CssClass="rightSide">
                        <asp:Label ID="lblSweetShopStreetAddress" runat="server" Text="644 Main St. N"></asp:Label>
                    </asp:TableCell></asp:TableRow><asp:TableRow>
                    <asp:TableCell CssClass="leftSide">
                        <asp:Label ID="lblPostalAddress" runat="server" Text=""></asp:Label>
                    </asp:TableCell><asp:TableCell CssClass="rightSide">
                        <asp:Label ID="lblSweetShopPostalAddress" runat="server" Text="Moose Jaw, Saskatchewan S6H 3K4"></asp:Label>
                    </asp:TableCell></asp:TableRow><asp:TableRow>
                    <asp:TableCell CssClass="leftSide">
                        <asp:Label ID="lblPhone" runat="server" Text=""></asp:Label>
                    </asp:TableCell><asp:TableCell CssClass="rightSide">
                        <asp:Label ID="lblSweetShopPhone" runat="server" Text="(306) 692-8337"></asp:Label>
                    </asp:TableCell></asp:TableRow></asp:Table><hr />

            <asp:GridView ID="grdItemsSoldList" runat="server" CellPadding="4" Width="100%" AutoGenerateColumns="False" RowStyle-HorizontalAlign="Center">
                <Columns>
                    <asp:TemplateField HeaderText="SKU #">
                        <ItemTemplate>
                            <asp:Label ID="sku" Text='<%#Eval("sku")%>' runat="server"></asp:Label></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="Description">
                        <ItemTemplate>
                            <asp:Label ID="itemDescription" Text='<%# Eval("description")%>' runat="server"></asp:Label></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="Retail Price">
                        <ItemTemplate>
                            <asp:Label ID="itemPrice" Text='<%# Convert.ToInt32(Request.QueryString["inv"].Split(Convert.ToChar("-"))[1]) == 1 ? Eval("price","{0:C}") : (Convert.ToBoolean(Eval("percentage")) == false ? ((Convert.ToDouble(Eval("price"))) - Convert.ToDouble(Eval("itemDiscount"))).ToString("C") : ((Convert.ToDouble(Eval("price")) - ((Convert.ToDouble(Eval("itemDiscount")) / 100) * Convert.ToDouble(Eval("price"))))).ToString("C")) %>' runat="server"></asp:Label></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="Discounts/Bonus Applied">
                        <ItemTemplate>
                            <asp:Label ID="discount" Text='<%# Convert.ToInt32(Request.QueryString["inv"].Split(Convert.ToChar("-"))[1]) == 1 ? Convert.ToBoolean(Eval("percentage")) == false ? (Eval("itemDiscount","{0:C}")).ToString() : ((Convert.ToDouble(Eval("itemDiscount")) / 100) * Convert.ToDouble(Eval("price"))).ToString("C") : (Convert.ToBoolean(Eval("percentage")) == false ? (((Convert.ToDouble(Eval("price")))-(Convert.ToDouble(Eval("itemDiscount")))) - Convert.ToDouble(Eval("itemRefund"))).ToString("C") : (((Convert.ToDouble(Eval("price")) - ((Convert.ToDouble(Eval("itemDiscount")) / 100) * Convert.ToDouble(Eval("price"))))) - Convert.ToDouble(Eval("itemRefund"))).ToString("C")) %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Quantity">
                        <ItemTemplate>
                            <asp:Label ID="itemQuantity" Text='<%#Eval("quantity")%>' runat="server"></asp:Label></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="Sale Price">
                        <ItemTemplate>
                            <asp:Label ID="salePrice" Text='<%# Convert.ToInt32(Request.QueryString["inv"].Split(Convert.ToChar("-"))[1]) == 1 ? Convert.ToBoolean(Eval("percentage")) == false ? ((Convert.ToDouble(Eval("price")))-(Convert.ToDouble(Eval("itemDiscount")))).ToString("C") : ((Convert.ToDouble(Eval("price")) - ((Convert.ToDouble(Eval("itemDiscount")) / 100) * Convert.ToDouble(Eval("price"))))).ToString("C") : Eval("itemRefund", "{0:C}") %>' runat="server"></asp:Label></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="Extended Price">
                        <ItemTemplate>
                            <asp:Label ID="extended" Text='<%# Convert.ToInt32(Request.QueryString["inv"].Split(Convert.ToChar("-"))[1]) == 1 ? Convert.ToBoolean(Eval("percentage")) == false ? ((Convert.ToDouble(Eval("price"))-Convert.ToDouble(Eval("itemDiscount")))*Convert.ToDouble(Eval("quantity"))).ToString("C") : ((Convert.ToDouble(Eval("price")) - ((Convert.ToDouble(Eval("itemDiscount")) / 100) * Convert.ToDouble(Eval("price"))))*Convert.ToDouble(Eval("quantity"))).ToString("C") : (Convert.ToDouble(Eval("itemRefund")) * Convert.ToDouble(Eval("quantity"))).ToString("C") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <hr />
        </div>
        <div id="paymentDetails" class="yesPrint">
            <h3>Payment Details</h3><asp:Table ID="tblSummary" runat="server" Width="70%">
                <asp:TableRow>
                    <asp:TableCell CssClass="leftFirst">
                        <asp:Label ID="lblDiscounts" runat="server" Text="Discounts:"></asp:Label>
                    </asp:TableCell><asp:TableCell CssClass="leftSecond">
                        <asp:Label ID="lblDiscountsDisplay" runat="server" Text="" DataFormatString="{0:C}"></asp:Label>
                    </asp:TableCell><asp:TableCell CssClass="rightFirst">
                        <asp:Label ID="lblBlank" runat="server" Text=""></asp:Label>
                    </asp:TableCell><asp:TableCell CssClass="rightSecond">
                        <asp:Label ID="lblBlankDisplay" runat="server" Text=""></asp:Label>
                    </asp:TableCell></asp:TableRow><asp:TableRow>
                    <asp:TableCell CssClass="leftFirst">
                        <asp:Label ID="lblTradeIns" runat="server" Text="Trade-Ins:"></asp:Label>
                    </asp:TableCell><asp:TableCell CssClass="leftSecond">
                        <asp:Label ID="lblTradeInsDisplay" runat="server" Text="" DataFormatString="{0:C}"></asp:Label>
                    </asp:TableCell><asp:TableCell CssClass="rightFirst">
                        <asp:Label ID="lblGST" runat="server" Text="GST:"></asp:Label>
                    </asp:TableCell><asp:TableCell CssClass="rightSecond">
                        <asp:Label ID="lblGSTDisplay" runat="server" Text="" DataFormatString="{0:C}"></asp:Label>
                    </asp:TableCell></asp:TableRow><asp:TableRow>
                    <asp:TableCell CssClass="leftFirst">
                        <asp:Label ID="lblShipping" runat="server" Text="Shipping:"></asp:Label>
                    </asp:TableCell><asp:TableCell CssClass="leftSecond">
                        <asp:Label ID="lblShippingDisplay" runat="server" Text="" DataFormatString="{0:C}"></asp:Label>
                    </asp:TableCell><asp:TableCell CssClass="rightFirst">
                        <asp:Label ID="lblPST" runat="server" Text="PST:"></asp:Label>
                    </asp:TableCell><asp:TableCell CssClass="rightSecond">
                        <asp:Label ID="lblPSTDisplay" runat="server" Text="" DataFormatString="{0:C}"></asp:Label>
                    </asp:TableCell></asp:TableRow><asp:TableRow>
                    <asp:TableCell CssClass="leftFirst">
                        <asp:Label ID="lblSubtotal" runat="server" Text="Subtotal:"></asp:Label>
                    </asp:TableCell><asp:TableCell CssClass="leftSecond">
                        <asp:Label ID="lblSubtotalDisplay" runat="server" Text="" DataFormatString="{0:C}"></asp:Label>
                    </asp:TableCell><asp:TableCell CssClass="rightFirst">
                        <asp:Label ID="lblTotalPaid" runat="server" Text="Total Paid:"></asp:Label>
                    </asp:TableCell><asp:TableCell CssClass="rightSecond">
                        <asp:Label ID="lblTotalPaidDisplay" runat="server" Text="" DataFormatString="{0:C}"></asp:Label>
                    </asp:TableCell></asp:TableRow></asp:Table><p>
                <asp:GridView ID="grdMOPS" runat="server" CellPadding="4" Width="70%" AutoGenerateColumns="false" RowStyle-HorizontalAlign="Center">
                    <Columns>
                        <asp:BoundField DataField="mopType" ReadOnly="true" HeaderText="Payment Type" />
                        <asp:BoundField DataField="amountPaid" ReadOnly="true" HeaderText="Amount Paid" DataFormatString="{0:C}" />
                    </Columns>
                </asp:GridView>
            </p>
        </div>
    </div>
    <div class="noPrint">
        <%--added a cssclass here for testing--%>
        <asp:Button ID="btnPrint" CssClass="noPrint" runat="server" Text="Print" Width="100px" OnClientClick="CallPrint('print', 'disclaimer');" />
        <br />
        <asp:Button ID="btnHome" runat="server" Text="Home" Width="100px" OnClick="btnHome_Click" />
        <br />
        <%--<p><b>PLEASE NOTE: </b>All used equipment is sold as is and it is understood that its' condition</p>
                <p>and usability may reflect prior use. The Sweet Spot Discount Golf assumes no responsibility</p>
                <p>beyond the point of sale. <b>ALL SALES FINAL</b> Thank you for shopping at the Sweet Spot.</p>--%>
    </div>
    <div id="disclaimer">
        <h6>
            <p>
                <b>PLEASE NOTE: </b>All used equipment is sold as is and it is understood that its' condition
        and usability may reflect prior use. The Sweet Spot Discount Golf assumes no responsibility
        beyond the point of sale. <b>ALL SALES FINAL</b> Thank you for shopping at the Sweet Spot. </p></h6></div></asp:Content>
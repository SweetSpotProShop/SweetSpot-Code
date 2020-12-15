<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.HomePage" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SPMaster" runat="server">
</asp:Content>--%>
<asp:Content ID="homePageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <h2>Today's Transactions</h2>
    <%--REMEMBER TO SET DEFAULT BUTTON--%>
    <asp:Label ID="lblLoc" runat="server" Text="Location :" />
    <asp:DropDownList ID="ddlLocation" runat="server" AutoPostBack="true"
        DataTextField="varCityName" DataValueField="intLocationID" Enabled="false" />
    <div style="text-align: right">
        <asp:Label ID="lbluser" runat="server" Visible="false" Text="You have Admin Access" />
    </div>
    <hr />
    <asp:GridView ID="GrdSameDaySales" runat="server" AutoGenerateColumns="False" Width="100%" ShowFooter="true" OnRowCommand="GrdSameDaySales_RowCommand"
		OnRowDataBound="GrdSameDaySales_RowDataBound" RowStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" FooterStyle-Font-Bold="true">
        <Columns>
            <asp:TemplateField HeaderText="Invoice Number">
                <ItemTemplate>
                    <asp:LinkButton ID="lbtnInvoiceNumber" runat="server" Text='<%#Eval("varInvoiceNumber") + "-" + Eval("intInvoiceSubNumber") %>' CommandArgument='<%#Eval("intInvoiceID") %>' />
                </ItemTemplate>
                 <FooterTemplate>
                    <asp:Label ID="lblTotals" runat="server" Text="Totals:" />
                </FooterTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="intCustomerID" ReadOnly="true" HeaderText="Customer" />
            <asp:BoundField DataField="employeeName" ReadOnly="true" HeaderText="Employee" />
            <asp:BoundField DataField="fltTotalDiscount" ReadOnly="true" HeaderText="Discount" DataFormatString="{0:C}" />
            <asp:BoundField DataField="fltTotalTradeIn" ReadOnly="true" HeaderText="Trade In" DataFormatString="{0:C}" />
            <asp:BoundField DataField="fltSubTotal" ReadOnly="true" HeaderText="Subtotal" DataFormatString="{0:C}" />
            <asp:BoundField DataField="fltGovernmentTaxAmount" ReadOnly="true" HeaderText="Government Tax" DataFormatString="{0:C}" />
            <asp:BoundField DataField="fltProvincialTaxAmount" ReadOnly="true" HeaderText="Provincial Tax" DataFormatString="{0:C}" />
            <asp:BoundField DataField="fltBalanceDue" ReadOnly="true" HeaderText="Balance Paid" DataFormatString="{0:C}" />
            <asp:BoundField DataField="varPaymentName" ReadOnly="true" HeaderText="MOP Type" />
            <asp:BoundField DataField="fltAmountPaid" ReadOnly="true" HeaderText="MOP Amount" DataFormatString="{0:C}" />
        </Columns>
    </asp:GridView>
    <div>
        <asp:HiddenField ID="hidden" runat="server" />
    </div>
</asp:Content>
<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.HomePage" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SPMaster" runat="server">
</asp:Content>--%>
<asp:Content ID="homePageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <h2>Today's Transactions</h2>
    <%--REMEMBER TO SET DEFAULT BUTTON--%>
    <asp:Label ID="lblLoc" runat="server" Text="Location :" />
    <asp:DropDownList ID="ddlLocation" runat="server" AutoPostBack="true" />
    <asp:Label ID="lblLocation" runat="server" Visible="false" Text="Loc" />
    <div style="text-align: right">
        <asp:Label ID="lbluser" runat="server" Visible="false" Text="UserName" />
    </div>
    <hr />
    <asp:GridView ID="grdSameDaySales" runat="server" AutoGenerateColumns="False" Width="100%" ShowFooter="true" OnRowDataBound="grdSameDaySales_RowDataBound" RowStyle-HorizontalAlign="Center">
        <Columns>
            <asp:TemplateField HeaderText="Invoice Number">
                <ItemTemplate>
                    <asp:LinkButton ID="lbtnInvoiceNumber" runat="server" Text='<%#Eval("invoiceNum") + "-" + Eval("invoiceSubNum") %>' OnClick="lbtnInvoiceNumber_Click" />
                </ItemTemplate>
                 <FooterTemplate>
                    <asp:Label ID="lblTotals" runat="server" Text="Totals:" />
                </FooterTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="custID" ReadOnly="true" HeaderText="Customer" />
            <asp:BoundField DataField="discountAmount" ReadOnly="true" HeaderText="Discount" DataFormatString="{0:C}" />
            <asp:BoundField DataField="tradeinAmount" ReadOnly="true" HeaderText="Trade In" DataFormatString="{0:C}" />
            <asp:BoundField DataField="subTotal" ReadOnly="true" HeaderText="Subtotal" DataFormatString="{0:C}" />
            <asp:BoundField DataField="governmentTax" ReadOnly="true" HeaderText="Government Tax" DataFormatString="{0:C}" />
            <asp:BoundField DataField="provincialTax" ReadOnly="true" HeaderText="Provincial Tax" DataFormatString="{0:C}" />
            <asp:BoundField DataField="balanceDue" ReadOnly="true" HeaderText="Balance Paid" DataFormatString="{0:C}" />
        </Columns>
    </asp:GridView>
    <div>
        <asp:HiddenField ID="hidden" runat="server" />
    </div>
</asp:Content>
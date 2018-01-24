<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReturnsHomePage.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.ReturnsHomePage" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SPMaster" runat="server">
</asp:Content>--%>

<asp:Content ID="returnsPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <div id="Sales">
        <%--REMEMBER TO SET DEFAULT BUTTON--%>
        <asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="btnSearch">
            <h2>Locate Invoice for Return</h2>
            <hr />
            <asp:Table ID="tblInvoiceSearch" runat="server" >
                <asp:TableRow>
                    <asp:TableCell RowSpan="3" Width="20%">
                        <asp:Calendar ID="calSearchDate" runat="server" BackColor="White" BorderColor="#999999" CellPadding="4" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="184px" Width="200px" >
                            <DayHeaderStyle BackColor="#5FD367" Font-Bold="True" Font-Size="7pt" />
                            <NextPrevStyle VerticalAlign="Bottom" />
                            <OtherMonthDayStyle ForeColor="#808080" />
                            <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                            <SelectorStyle BackColor="#CCCCCC" />
                            <TitleStyle BackColor="#005555" BorderColor="Black" Font-Bold="True" />
                            <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                            <WeekendDayStyle BackColor="#FFFFCC" />
                        </asp:Calendar>
                    </asp:TableCell>
                    <asp:TableCell Width="20%">
                        <asp:Label ID="lblInvoiceSearch" runat="server" Text="Enter Invoice Number, Customer Name, <br /> or Customer Phone Number:" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:TextBox ID="txtInvoiceSearch" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Button ID="btnSearch" runat="server" Width="150" Text="Search" OnClick="btnSearch_Click" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <hr />
            <div>
                <asp:GridView ID="grdInvoiceSelection" runat="server" RowStyle-HorizontalAlign="Center" AutoGenerateColumns="false" Width="100%" OnRowCommand="grdInvoiceSelection_RowCommand">
                    <Columns>
                        <asp:TemplateField HeaderText="Invoice Number">
                            <ItemTemplate>
                                <asp:LinkButton ID="lkbInvoiceNum" runat="server" CommandName="returnInvoice" CommandArgument='<%#Eval("invoiceNum") + "-" + Eval("invoiceSub")%>' Text='<%#Eval("invoiceNum") + "-" + Eval("invoiceSub") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Invoice Date">
                            <ItemTemplate>
                                <asp:Label ID="lblInvoiceDate" runat="server" Text='<%#Eval("invoiceDate","{0: MM/dd/yy}") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Customer Name">
                            <ItemTemplate>
                                <asp:Label ID="lblCustomerName" runat="server" Text='<%#Eval("customer.firstName") + " " + Eval("customer.lastName") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total">
                            <ItemTemplate>
                                <asp:Label ID="lblAmountPaid" runat="server" Text='<%#Eval("balanceDue","{0:C}") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Store">
                            <ItemTemplate>
                                <asp:Label ID="lblLocation" runat="server" Text='<%#Eval("location.locationName") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <hr />
        </asp:Panel>
    </div>
</asp:Content>
<asp:Content ID="Content1" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .auto-style1 {
            position: relative;
            left: 300px;
            top: -10px;
            width: 207px;
            height: 228px;
        }
        </style>
</asp:Content>

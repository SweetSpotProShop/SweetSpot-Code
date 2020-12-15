<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="SalesHomePage.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.SalesHomePage" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
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
<%--<asp:Content ID="Content2" ContentPlaceHolderID="SPMaster" runat="server">
</asp:Content>--%>

<asp:Content ID="salesPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <div id="Sales">
        <%--REMEMBER TO SET DEFAULT BUTTON--%>
        <asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="btnQuickSale">
            <h2>Sales</h2>
            <hr />
            <asp:Table runat="server">
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Button ID="BtnQuickSale" runat="server" Width="150" Text="Quick Sale" OnClick="BtnQuickSale_Click" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="BtnReturns" runat="server" Width="150" Text="Process Return" OnClick="BtnReturns_Click" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="BtnInvoiceSearch" runat="server" Text="Search for Invoices" OnClick="BtnInvoiceSearch_Click" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="BtnProcessCashOut" runat="server" Text="Process CashOut" OnClick="BtnProcessCashOut_Click" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Calendar ID="CalSearchDate" runat="server" BackColor="White" BorderColor="#999999" CellPadding="4" DayNameFormat="Shortest" 
                            Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="184px" Width="200px" OnSelectionChanged="CalSearchDate_SelectionChanged" >
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
                </asp:TableRow>
            </asp:Table>

            <%--<div class="divider" />--%>
            <hr />
            <h2>Current Open Sales</h2>
            <hr />
            <div>
                <asp:GridView ID="GrdCurrentOpenSales" runat="server" AutoGenerateColumns="false" Width="100%" OnRowCommand="GrdCurrentOpenSales_RowCommand" RowStyle-HorizontalAlign="Center" >
                    <Columns>
                        <asp:TemplateField HeaderText="View Invoice">
                            <ItemTemplate>
                                <asp:LinkButton ID="lkbInvoiceNum" runat="server" CommandName="returnInvoice" CommandArgument='<%#Eval("intInvoiceID")%>' Text='<%#Eval("varInvoiceNumber") + "-" + Eval("intInvoiceSubNumber") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Invoice Date">
                            <ItemTemplate>
                                <asp:Label ID="lblInvoiceDate" runat="server" Text='<%#Eval("dtmInvoiceDate","{0: dd/MMM/yy}") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Customer Name">
                            <ItemTemplate>
                                <asp:Label ID="lblCustomerName" runat="server" Text='<%#Eval("customer.varFirstName") + " " + Eval("customer.varLastName") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Discount">
                            <ItemTemplate>
                                <asp:Label ID="lblDiscountAmount" runat="server" Text='<%#Eval("fltTotalDiscount","{0:C}") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Trade In">
                            <ItemTemplate>
                                <asp:Label ID="lblTradeInAmount" runat="server" Text='<%#Eval("fltTotalTradeIn","{0:C}") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Subtotal">
                            <ItemTemplate>
                                <asp:Label ID="lblSubtotal" runat="server" Text='<%#Eval("fltSubTotal","{0:C}") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="GST">
                            <ItemTemplate>
                                <asp:Label ID="lblGSTAmount" runat="server" Text='<%#Eval("fltGovernmentTaxAmount","{0:C}") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PST">
                            <ItemTemplate>
                                <asp:Label ID="lblPSTAmount" runat="server" Text='<%#Eval("fltProvincialTaxAmount","{0:C}") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total">
                            <ItemTemplate>
                                <asp:Label ID="lblAmountPaid" runat="server" Text='<%#Eval("fltBalanceDue","{0:C}") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Employee Name">
                            <ItemTemplate>
                                <asp:Label ID="lblEmployeeName" runat="server" Text='<%#Eval("employee.varFirstName") + " " + Eval("employee.varLastName") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Type">
                            <ItemTemplate>
                                <%--<asp:Label ID="lblTransactionType" runat="server" Text='<%#Eval("varTransactionName") %>' />--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CustID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblCustID" runat="server" Text='<%#Eval("customer.intCustomerID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <hr />
        </asp:Panel>
    </div>
</asp:Content>


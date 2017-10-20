<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="SalesHomePage.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.SalesHomePage" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SPMaster" runat="server">
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
                        <asp:Button ID="btnQuickSale" runat="server" Width="150" Text="Quick Sale" OnClick="btnQuickSale_Click" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="btnReturns" runat="server" Width="150" Text="Process Return" OnClick="btnReturns_Click" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>

            <%--<div class="divider" />--%>
            <hr />
            <h2>Invoice Search</h2>
            <asp:SqlDataSource ID="SqlDSLocation" runat="server" ConnectionString="<%$ ConnectionStrings:SweetSpotDevConnectionString %>" SelectCommand="SELECT [locationID], [locationName] FROM [tbl_location]"></asp:SqlDataSource>
            <br />
            Search by using the Calendar dates or the text box for a specific invoice
            <br />
            <asp:Table runat="server">
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblSelectLocation" runat="server" Text="Select Location:" />
                    </asp:TableCell><asp:TableCell>
                        <asp:DropDownList ID="ddlLocation" runat="server" AutoPostBack="True" DataSourceID="SqlDSLocation" DataTextField="locationName" DataValueField="locationID"></asp:DropDownList>
                    </asp:TableCell><asp:TableCell>
                        <asp:Label ID="lblInvoiceNum" runat="server" Text="Enter Invoice Number:" />
                    </asp:TableCell><asp:TableCell>
                        <asp:TextBox ID="txtInvoiceNum" runat="server" Text="" />
                    </asp:TableCell><asp:TableCell>
                        <asp:Button ID="btnInvoiceSearch" runat="server" Text="Search for Invoices" OnClick="btnInvoiceSearch_Click" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <hr />
            <asp:Table ID="tblInvoiceSearch" runat="server" Width="100%">
                <asp:TableRow>
                    <asp:TableCell Width="50%">
                        <asp:Label runat="server" Text="Start Date:"></asp:Label>
                    </asp:TableCell><asp:TableCell Width="50%">
                        <asp:Label runat="server" Text="End Date:"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Calendar ID="calStartDate" runat="server" BackColor="White" BorderColor="#999999" CellPadding="4" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="184px" Width="200px" OnSelectionChanged="calStart_SelectionChanged">
                            <DayHeaderStyle BackColor="#5FD367" Font-Bold="True" Font-Size="7pt" />
                            <NextPrevStyle VerticalAlign="Bottom" />
                            <OtherMonthDayStyle ForeColor="#808080" />
                            <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                            <SelectorStyle BackColor="#CCCCCC" />
                            <TitleStyle BackColor="#005555" BorderColor="Black" Font-Bold="True" />
                            <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                            <WeekendDayStyle BackColor="#FFFFCC" />
                        </asp:Calendar>
                    </asp:TableCell><asp:TableCell>
                        <asp:Calendar ID="calEndDate" runat="server" BackColor="White" BorderColor="#999999" CellPadding="4" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="182px" Width="200px" OnSelectionChanged="calEnd_SelectionChanged">
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
            <hr />
            <div>
                <asp:GridView ID="grdInvoiceSelection" runat="server" AutoGenerateColumns="false" Width="100%" OnRowCommand="grdInvoiceSelection_RowCommand">
                    <Columns>
                        <asp:TemplateField HeaderText=" View Invoice">
                            <ItemTemplate>
                                <asp:LinkButton ID="lkbInvoiceNum" runat="server" CommandName="returnInvoice" CommandArgument='<%#Eval("invoiceNum") + "-" + Eval("invoiceSub")%>' Text='<%#Eval("invoiceNum") + "-" + Eval("invoiceSub") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Invoice Date">
                            <ItemTemplate>
                                <asp:Label ID="lblInvoiceDate" runat="server" Text='<%#Eval("invoiceDate","{0: MM/dd/yy}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Customer Name">
                            <ItemTemplate>
                                <asp:Label ID="lblCustomerName" runat="server" Text='<%#Eval("customerName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Discount">
                            <ItemTemplate>
                                <asp:Label ID="lblDiscountAmount" runat="server" Text='<%#Eval("discountAmount","{0:#0.00}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Trade In">
                            <ItemTemplate>
                                <asp:Label ID="lblTradeInAmount" runat="server" Text='<%#Eval("tradeinAmount","{0:#0.00}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Subtotal">
                            <ItemTemplate>
                                <asp:Label ID="lblSubtotal" runat="server" Text='<%#Eval("subTotal","{0:#0.00}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="GST">
                            <ItemTemplate>
                                <asp:Label ID="lblGSTAmount" runat="server" Text='<%#Eval("governmentTax","{0:#0.00}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PST">
                            <ItemTemplate>
                                <asp:Label ID="lblPSTAmount" runat="server" Text='<%#Eval("provincialTax","{0:#0.00}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total">
                            <ItemTemplate>
                                <asp:Label ID="lblAmountPaid" runat="server" Text='<%#Eval("balanceDue","{0:#0.00}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Employee Name">
                            <ItemTemplate>
                                <asp:Label ID="lblEmployeeName" runat="server" Text='<%#Eval("employeeName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <hr />
        </asp:Panel>
    </div>
</asp:Content>
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

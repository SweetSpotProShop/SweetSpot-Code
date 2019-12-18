<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="InvoiceSearch.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.InvoiceSearch" %>

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
        <asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="btnInvoiceSearch">

            <%--<div class="divider" />--%>
            <h2>Invoice Search</h2>
            <hr />
            Search by using the Calendar dates or the text box for a specific invoice
            <br />
            <asp:Table runat="server">
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblSelectLocation" runat="server" Text="Select Location:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:DropDownList ID="ddlLocation" runat="server" AutoPostBack="false"
                            DataTextField="varCityName" DataValueField="intLocationID" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblInvoiceNum" runat="server" Text="Enter Invoice Number or SKU:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtInvoiceNum" runat="server" AutoComplete="off" Text="" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="btnInvoiceSearch" runat="server" Text="Search for Invoices" OnClick="btnInvoiceSearch_Click" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <hr />
            <asp:Table ID="tblInvoiceSearch" runat="server" Width="100%">
                <asp:TableRow>
                    <asp:TableCell Width="50%">
                        <asp:Label runat="server" Text="Start Date:" />
                    </asp:TableCell>
                    <asp:TableCell Width="50%">
                        <asp:Label runat="server" Text="End Date:" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Calendar ID="calStartDate" runat="server" BackColor="White" BorderColor="#999999" CellPadding="4"
                            DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" 
                            Height="184px" Width="200px" OnSelectionChanged="calStart_SelectionChanged">
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
                    <asp:TableCell>
                        <asp:Calendar ID="calEndDate" runat="server" BackColor="White" BorderColor="#999999" CellPadding="4" 
                            DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" 
                            Height="182px" Width="200px" OnSelectionChanged="calEnd_SelectionChanged">
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
                <asp:GridView ID="grdInvoiceSelection" runat="server" AutoGenerateColumns="false" Width="100%" 
					OnRowCommand="grdInvoiceSelection_RowCommand" RowStyle-HorizontalAlign="Center"
					OnRowDataBound="grdInvoiceSelection_RowDataBound" >
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
                                <asp:Label ID="lblCustomerName" runat="server" Text='<%#Eval("customerName") %>' />
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
						<asp:TemplateField HeaderText="Payment">
                            <ItemTemplate>
                                <asp:Label ID="lblPaymentName" runat="server" Text='<%#Eval("varPaymentName") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Total">
                            <ItemTemplate>
                                <asp:Label ID="lblAmountPaid" runat="server" Text='<%#Eval("fltAmountPaid","{0:C}") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Employee Name">
                            <ItemTemplate>
                                <asp:Label ID="lblEmployeeName" runat="server" Text='<%#Eval("employeeName") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <hr />
        </asp:Panel>
    </div>
</asp:Content>

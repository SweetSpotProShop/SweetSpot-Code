<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReportsHomePage.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.ReportsHomePage" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SPMaster" runat="server">
</asp:Content>--%>

<asp:Content ID="ReportsPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <asp:SqlDataSource ID="sqlLocations" runat="server" ConnectionString="<%$ ConnectionStrings:SweetSpotDevConnectionString %>" SelectCommand="SELECT [locationID], [locationName] FROM [tbl_location] ORDER BY [locationName]"></asp:SqlDataSource>
    <div id="Reports">
        <div style="text-align: left">
            <asp:Label ID="lblReport" runat="server" Visible="false" Text="Report Access"></asp:Label>

            <asp:Label ID="lbldate" runat="server" Visible="false" Text="Select a date"></asp:Label>

        </div>
        <asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="btnRunReport">
            <h2>Reports Selection</h2>
            <br />
            <asp:Label runat="server" Text="Select Location:" />
            <asp:DropDownList ID="ddlLocation" runat="server" AutoPostBack="True" DataSourceID="sqlLocations" DataTextField="locationName" DataValueField="locationID" Visible="true"></asp:DropDownList>
            <hr />
            <%--Start Calendar--%>
            <asp:Table runat="server">
                <asp:TableRow>
                    <asp:TableCell Width="33%">
                        <asp:Label runat="server" Text="Start Date:"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell Width="33%">
                        <asp:Label runat="server" Text="End Date:"></asp:Label>
                    </asp:TableCell>
                    <%--<asp:TableCell Width="33%">
                        <asp:Label runat="server" Text="Inovice Number:"></asp:Label>
                    </asp:TableCell>--%>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:TextBox ID="txtStartDate" ReadOnly="true" Width="195px" placeholder="Please select a starting date." Text="" runat="server"></asp:TextBox>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtEndDate" ReadOnly="true" Width="195px" placeholder="Please select a ending date." Text="" runat="server"></asp:TextBox>
                    </asp:TableCell>
                    <%--<asp:TableCell>
                        <asp:TextBox ID="txtInvoiceNum" Width="195px" placeholder="Please enter an invoice number" Text="" runat="server"></asp:TextBox>
                    </asp:TableCell>--%>
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
                    </asp:TableCell>
                    <asp:TableCell>
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
                <asp:Table runat="server">
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Button ID="btnRunReport" runat="server" Text="CashOut Report" Width="200px" OnClick="btnSubmit_Click" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Button ID="btnPurchasesReport" runat="server" Text="Purchases Report" Width="200px" OnClick="btnPurchasesReport_Click" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Button ID="btnTaxReport" runat="server" Text="Tax Report" Width="200px" OnClick="btnTaxReport_Click" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Button ID="btnCOGSvsPMReport" runat="server" Text="COGS & Profit Margin Report" Width="200px" OnClick="btnCOGSvsPMReport_Click" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Button ID="btnItemsSold" runat="server" Text="Items Sold" Width="200px" Onclick="btnItemsSold_Click" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Button ID="btnDiscountReport" runat="server" Text="Discount Report" Width="200px" OnClick="btnDiscountReport_Click" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Button ID="btnSalesByDateReport" runat="server" Text="Sales By Date Report" Width="200px" OnClick="btnSalesByDate_Click" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Button ID="btnMostSold" runat="server" Text="Top Selling Items Report" Width="200px" Onclick="btnMostSold_Click" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Button ID="btnPaymentsByDateReport" runat="server" Text="Payments By Date" Width="200px" Onclick="btnPaymentsByDateReport_Click" />
                        </asp:TableCell>
                        <asp:TableCell>
                            
                        </asp:TableCell>
                        <asp:TableCell>
                            
                        </asp:TableCell>
                        <asp:TableCell>
                            
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <hr />
            <div>
            </div>
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


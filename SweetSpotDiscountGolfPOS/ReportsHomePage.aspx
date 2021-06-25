<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReportsHomePage.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.ReportsHomePage" %>

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

<asp:Content ID="ReportsPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <div id="Reports">
        <div style="text-align: left">
            <asp:Label ID="lblReport" runat="server" Visible="false" Text="Report Access" />
            <asp:Label ID="lbldate" runat="server" Visible="false" Text="Select a date" />

        </div>
        <asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="btnCashOutReport">
            <h2>Reports Selection</h2>
            <br />
            <asp:Label runat="server" Text="Select Location:" />
            <asp:DropDownList ID="ddlLocation" runat="server" AutoPostBack="false"
                DataTextField="varCityName" DataValueField="intLocationID" Visible="true" />
            <hr />
            <%--Start Calendar--%>
            <asp:Table runat="server">
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label runat="server" Text="Select date type:"/>                        
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:DropDownList ID="ddlDatePeriod" runat="server" AutoPostBack="false">
							<asp:ListItem Text="Invoice" Value="0" />
                            <asp:ListItem Text="Day" Value="1" />
                            <asp:ListItem Text="Week" Value="2" />
                            <asp:ListItem Text="Month" Value="3" />
                        </asp:DropDownList>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Calendar ID="CalStartDate" runat="server" BackColor="White" BorderColor="#999999" CellPadding="4" DayNameFormat="Shortest"
                            Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="184px" Width="200px" 
                            OnSelectionChanged="CalStart_SelectionChanged">
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
                        <asp:Calendar ID="CalEndDate" runat="server" BackColor="White" BorderColor="#999999" CellPadding="4" DayNameFormat="Shortest"
                            Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="182px" Width="200px"
                            OnSelectionChanged="CalEnd_SelectionChanged">
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
                            <asp:Button ID="BtnCashOutReport" runat="server" Text="CashOut Report" Width="200px" OnClick="BtnCashOutReport_Click" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Button ID="BtnDiscountReport" runat="server" Text="Discount Report" Width="200px" OnClick="BtnDiscountReport_Click" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Button ID="BtnTaxReport" runat="server" Text="Tax Report" Width="200px" OnClick="BtnTaxReport_Click" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Button ID="BtnSalesByDateReport" runat="server" Text="Sales By Date Report" Width="200px" OnClick="BtnSalesByDate_Click" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Button ID="BtnExtensiveInvoice" runat="server" Text="Extensive Invoice" Width="200px" Onclick="BtnExtensiveInvoice_Click" />							
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Button ID="BtnCostOfInventory" runat="server" Text="Cost of Inventory" Width="200px" OnClick="BtnCostOfInventory_Click" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Button ID="BtnStoreStatsReport" runat="server" Text="Store Stats Report" Width="200px" OnClick="BtnStoreStatsReport_Click" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Button ID="BtnInvnetoryChangeReport" runat="server" Text="Inventory Change Report" Width="200px" OnClick="BtnInventoryChangeReport_Click" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Button ID="BtnSpecificApparelReport" runat="server" Text="Specific Apparel Report" Width="200px" OnClick="BtnSpecificApparelReport_Click" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Button ID="BtnSpecificGripReport" runat="server" Text="Specific Grip Report" Width="200px" OnClick="BtnSpecificGripReport_Click" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Button ID="BtnCreatePDFFiles" runat="server" Width="200" Text="Store Daily Sales Data" OnClick="BtnCreatePDFFiles_Click" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Button ID="BtnExportInvoices" runat="server" Width="200" Text="Export Invoices" OnClick="BtnExportInvoices_Click" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
							<%--<asp:Button ID="btnUpdatingTaxes" runat="server" Text="Update Taxes between Date Range" Width="200px" OnClick="btnUpdatingTaxes_Click" />--%>
							<%--<asp:Button ID="btnItemsSold" runat="server" Text="Items Sold" Width="200px" Onclick="btnItemsSold_Click" />--%>
                        </asp:TableCell>
                        <asp:TableCell>
							<%--<asp:Button ID="btnTradeInsByDateReport" runat="server" Text="Trade Ins By Date" Width="200px" Onclick="btnTradeInsByDateReport_Click" />
							<asp:Button ID="btnPurchasesReport" runat="server" Text="Purchases Report" Width="200px" OnClick="btnPurchasesReport_Click" />--%>
                        </asp:TableCell>
                        <asp:TableCell>
							<%--<asp:Button ID="btnPaymentsByDateReport" runat="server" Text="Payments By Date" Width="200px" Onclick="btnPaymentsByDateReport_Click" />
							<asp:Button ID="btnMostSold" runat="server" Text="Top Selling Items Report" Width="200px" Onclick="btnMostSold_Click" />--%>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <hr />
        </asp:Panel>
    </div>
</asp:Content>
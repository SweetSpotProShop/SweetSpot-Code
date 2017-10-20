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
            <%--<div id="calStart"  runat="server">
                 <asp:TextBox ID="txtStartDate" ReadOnly="true" Width="195px" placeholder="Please select a starting date." Text="" runat="server"></asp:TextBox>
                <hr />
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
              
            </div>--%>



            <hr />
            <%--End Calendar--%>
            <%--<div id="calEnd" runat="server" style="margin-top: -229px;" class="auto-style1">
                <%-- <div id="calEnd" style="width:1210px;margin-top:auto;">--%>
            <%--<asp:TextBox ID="txtEndDate" ReadOnly="true" Width="195px" placeholder="Please select a ending date." Text="" runat="server"></asp:TextBox>
                <hr />
                <div style="width: 30px; margin: 0 auto">
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
                </div>
            </div>--%>

            <div>
                <asp:Table runat="server">
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Button ID="btnRunReport" runat="server" Text="CashOut Report" Width="200px" OnClick="btnSubmit_Click" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Button ID="btnPurchasesReport" runat="server" Text="Purchases Report" Width="200px" OnClick="btnPurchasesReport_Click" />
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>


            <%--<div>
                <div>
                    <asp:Label runat="server" Text="Search Between Dates For:"></asp:Label>
                </div>
                <asp:Button ID="btnInvoiceBetweenDates" runat="server" Text="Invoices" Width="200px" OnClick="btnInvoiceBetweenDates_Click" />
                <%--<asp:Button ID="btnDeletedInvoiceBetweenDates" runat="server" Text="Deleted Invoices" Width="200px" OnClick="btnDeletedInvoiceBetweenDates_Click" />
            </div>
            <asp:Button ID="btnReturnInvoice" runat="server" Text="Search For Invoice" OnClick="btnReturnInvoice_Click" />--%>
            <%--Testing--%>
            <%--<asp:Button ID="btnTesting" runat="server" Text="Test" OnClientClick="javascript:return getInput();" /><asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>--%>



            <hr />
            <%--<asp:GridView ID="grdInvoicesBetweenDates" runat="server" AutoGenerateColumns="False" Width="100%" >
                <Columns>
                    <asp:TemplateField HeaderText="Invoice Number">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtnInvoiceNumber" runat="server" Text='<%#Eval("invoiceNum") + "-" + Eval("invoiceSub") %>' OnClick="lbtnInvoiceNumber_Click"></asp:LinkButton>
                            <asp:Label ID="lblInvoiceNumber" runat="server" Text='<%#Eval("invoiceNum") + "-" + Eval("invoiceSub") %>' OnClick="lbtnInvoiceNumber_Click" Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="customerID" ReadOnly="true" HeaderText="Customer" />
                    <%--<asp:TemplateField HeaderText="Employee">
                    <asp:ItemTemplate>
                        <asp:Label ID="lblEmployee" runat="server" Text='<%#Eval("employeeID")%>'></asp:Label>
                    </asp:ItemTemplate>
                </asp:TemplateField>
                    <asp:BoundField DataField="discountAmount" ReadOnly="true" HeaderText="Discount" DataFormatString="{0:0.00}" />
                    <asp:BoundField DataField="tradeinAmount" ReadOnly="true" HeaderText="Trade In" DataFormatString="{0:0.00}" />
                    <asp:BoundField DataField="subTotal" ReadOnly="true" HeaderText="Subtotal" DataFormatString="{0:0.00}" />
                    <asp:BoundField DataField="governmentTax" ReadOnly="true" HeaderText="Government Tax" DataFormatString="{0:0.00}" />
                    <asp:BoundField DataField="provincialTax" ReadOnly="true" HeaderText="Provincial Tax" DataFormatString="{0:0.00}" />
                    <asp:BoundField DataField="balanceDue" ReadOnly="true" HeaderText="Balance Paid" DataFormatString="{0:0.00}" />
                    <%--<asp:TemplateField HeaderText="Delete Invoice">
                        <ItemTemplate>
                            <%--<asp:LinkButton Text="Delete" runat="server" CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete?');" CausesValidation="false" />
                            <asp:LinkButton Text="Delete" runat="server" CommandName="Delete" OnClientClick="userInput()" CausesValidation="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>--%>
            <div>
                <%--<asp:HiddenField ID="hidden" runat="server" />--%>
            </div>
            <%--<script>
                function userInput() {
                    var reason = prompt("Enter reason for deleting invoice", "");
                    //User pressed okay but didn't type anything
                    while (reason == "") {
                        //Keeps cycling until reason given or cancel is hit
                        reason = prompt("Enter reason for deleting invoice", "");
                    }
                    if (reason != "" && reason != "Code:CancelDelete") {
                        //User typed something and hit okay
                        document.getElementById('IndividualPageContent_hidden').value = reason.toString();
                    }
                    else {
                        //User hits cancel
                        document.getElementById('IndividualPageContent_hidden').value = "Code:CancelDelete";
                    }
                }

            </script>--%>
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


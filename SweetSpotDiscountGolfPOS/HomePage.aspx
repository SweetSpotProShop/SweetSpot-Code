<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.HomePage" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SPMaster" runat="server">
</asp:Content>--%>

<asp:Content ID="homePageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <h2>Today's Transactions</h2>
    <%--REMEMBER TO SET DEFAULT BUTTON--%>
    <%--<asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="btn">--%>
    <asp:Label ID="lblLoc" runat="server" Text="Location : "></asp:Label>
    <asp:DropDownList ID="ddlLocation" runat="server" AutoPostBack="true">
    </asp:DropDownList>
    <asp:Label ID="lblLocation" runat="server" Visible="false" Text="Loc"></asp:Label>

    <div style="text-align: right">
        <asp:Label ID="lbluser" runat="server" Visible="false" Text="UserName"></asp:Label>
    </div>

    <hr />
    <asp:GridView ID="grdSameDaySales" runat="server" AutoGenerateColumns="False" Width="100%" ShowFooter="true" OnRowDataBound="grdSameDaySales_RowDataBound" >
        <Columns>
            <asp:TemplateField HeaderText="Invoice Number">
                <ItemTemplate>
                    <asp:LinkButton ID="lbtnInvoiceNumber" runat="server" Text='<%#Eval("invoiceNum") + "-" + Eval("invoiceSub") %>' OnClick="lbtnInvoiceNumber_Click"></asp:LinkButton>
                </ItemTemplate>
                 <FooterTemplate>
                    <asp:Label ID="lblTotals" runat="server" Text="Totals" />
                </FooterTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="customerID" ReadOnly="true" HeaderText="Customer" />
            <%--<asp:TemplateField HeaderText="Employee">
                    <asp:ItemTemplate>
                        <asp:Label ID="lblEmployee" runat="server" Text='<%#Eval("employeeID")%>'></asp:Label>
                    </asp:ItemTemplate>
                </asp:TemplateField>--%>
            <asp:BoundField DataField="discountAmount" ReadOnly="true" HeaderText="Discount" DataFormatString="{0:C}" />
            <asp:BoundField DataField="tradeinAmount" ReadOnly="true" HeaderText="Trade In" DataFormatString="{0:C}" />
            <asp:BoundField DataField="subTotal" ReadOnly="true" HeaderText="Subtotal" DataFormatString="{0:C}" />
            <asp:BoundField DataField="governmentTax" ReadOnly="true" HeaderText="Government Tax" DataFormatString="{0:C}" />
            <asp:BoundField DataField="provincialTax" ReadOnly="true" HeaderText="Provincial Tax" DataFormatString="{0:C}" />
            <asp:BoundField DataField="balanceDue" ReadOnly="true" HeaderText="Balance Paid" DataFormatString="{0:C}" />
            <%--<asp:TemplateField HeaderText="Delete Invoice">
                <ItemTemplate>
                    <%--<asp:LinkButton Text="Delete" runat="server" CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete?');" CausesValidation="false" />
                    <asp:LinkButton Text="Delete" runat="server" CommandName="Delete" OnClientClick="userInput()" CausesValidation="false" />
                </ItemTemplate>
            </asp:TemplateField>--%>
        </Columns>
    </asp:GridView>

    <div>
        <asp:HiddenField ID="hidden" runat="server" />
        
    </div>
    <script>
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

    </script>
    <%--  </asp:Panel>--%>
</asp:Content>

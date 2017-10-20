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
            <asp:Button ID="btnQuickSale" runat="server" Width="150" Text="Quick Sale" OnClick="btnQuickSale_Click" />
            <div class="divider" />
            <hr />
            <h2>Locate Return Invoice</h2>
            <hr />
            <asp:Table ID="tblInvoiceSearch" runat="server" Width="100%">
                <asp:TableRow>
                    <asp:TableCell Width="15%">
                        <asp:RadioButton ID="rdbSearchByCustomer" runat="server" Text="Customer" Checked="True" GroupName="rgInvoiceSearch" />
                        <br />
                        <asp:RadioButton ID="rdbSearchByInvoiceNumber" runat="server" Text="Invoice" GroupName="rgInvoiceSearch" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblInvoiceSearch" runat="server" Text="Enter Invoice, Name, or Phone Number: "></asp:Label>
                        <br />
                        <asp:TextBox ID="txtInvoiceSearch" runat="server"></asp:TextBox>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblSearchDate" runat="server" Text="Enter Date to Search(leave blank if not known): "></asp:Label>
                        <br />
                        <asp:TextBox ID="txtSearchDate" runat="server"></asp:TextBox>
                        <asp:CompareValidator ID="cvSearchDate" ControlToValidate="txtSearchDate" Operator="DataTypeCheck" Type="Date" Text="MM/DD/YY" runat="server"></asp:CompareValidator>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="btnSearch" runat="server" Width="150" Text="Search" OnClick="btnSearch_Click" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <hr />
            <div>
                <asp:GridView ID="grdInvoiceSelection" runat="server" AutoGenerateColumns="false" Width="100%" OnRowCommand="grdInvoiceSelection_RowCommand">
                    <Columns>
                        <asp:TemplateField HeaderText="Invoice Number">
                            <ItemTemplate>
                                <asp:LinkButton ID="lkbInvoiceNum" runat="server" CommandName="returnInvoice" CommandArgument='<%#Eval("invoiceNum")%>' Text='<%#Eval("invoiceNum") + "-" + Eval("invoiceSub") %>'></asp:LinkButton>
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
                        <asp:TemplateField HeaderText="Total">
                            <ItemTemplate>
                                <asp:Label ID="lblAmountPaid" runat="server" Text='<%#Eval("balanceDue","{0:#0.00}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Store">
                            <ItemTemplate>
                                <asp:Label ID="lblLocation" runat="server" Text='<%#Eval("locationName") %>'></asp:Label>
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

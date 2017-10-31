<%@ Page Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReportsDiscounts.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.ReportDiscounts" %>




<asp:Content ID="ReportsPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="btnPrint">
        <h2>Discount Report</h2>
        <br />
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblReportDate" runat="server" Text="Date"></asp:Label>
                </td>
            </tr>
            <tr>
                <td >
                    <asp:Label ID="lblInvoices" runat="server" Text="Invoices:"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblTotals" runat="server" Text="Totals:"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="grdInvoiceDisplay" runat="server" AutoGenerateColumns="false" Width="100%">
                        <Columns>
                            <asp:TemplateField HeaderText="Invoice Number">
                                <ItemTemplate>
                                    <asp:Label ID="lblInvoiceNum" runat="server" Text='<%#Eval("invoiceNum") + "-" + Eval("invoiceSub") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Invoice Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblInvoiceDate" runat="server" Text='<%#Eval("invoiceDate","{0: dd/MMM/yy}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Customer Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblCustomerName" runat="server" Text='<%#Eval("customerName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Discount">
                                <ItemTemplate>
                                    <asp:Label ID="lblDiscountAmount" runat="server" Text='<%#Eval("discountAmount","{0:C}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Employee Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblEmployeeName" runat="server" Text='<%#Eval("employeeName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
                <td style="vertical-align:top">
                    <asp:ListBox ID="lbxTotals" runat="server" ></asp:ListBox>
                </td>


            </tr>
        </table>

        <hr />
        <asp:Button ID="btnPrint" runat="server" Text="Print Report" />
    </asp:Panel>





</asp:Content>

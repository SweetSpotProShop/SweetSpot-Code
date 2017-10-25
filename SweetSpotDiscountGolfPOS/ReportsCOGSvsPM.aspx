<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReportsCOGSvsPM.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.ReportsCOGSvsPM" %>

<asp:Content ID="ReportsPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">

    <h2>Cost of Goods Sold and Profit Margin</h2>
    <hr />
    <table>
        <tr>
            <td>
                <asp:label id="lblItemsSold" runat="server" text="Items Sold"></asp:label>
            </td>
            <td>
                <asp:label id="lblCOGS" runat="server" text="Cost of Goods Sold"></asp:label>
            </td>
            <td>
                <asp:label id="lblPM" runat="server" text="Profit Margin"></asp:label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="grdInvoiceSelection" runat="server" AutoGenerateColumns="false" Width="100%">
                    <Columns>
                        <asp:TemplateField HeaderText="Invoice Number">
                            <ItemTemplate>
                                <asp:Label ID="lblInvoiceNum" runat="server"  Text='<%#Eval("invoiceNum") + "-" + Eval("invoiceSub")%>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sku">
                            <ItemTemplate>
                                <asp:Label ID="lblSku" runat="server" Text='<%#Eval("sku") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Item Cost">
                            <ItemTemplate>
                                <asp:Label ID="lblItemCost" runat="server" Text='<%#Eval("itemCost","{0:C}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Item Price">
                            <ItemTemplate>
                                <asp:Label ID="lblItemPrice" runat="server" Text='<%#Eval("itemPrice","{0:C}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>                     
                    </Columns>
                </asp:GridView>
            </td>
            <td>
                <asp:label id="lblCOGSDisplay" runat="server" text="lblCOGSDisplay"></asp:label>
            </td>
            <td>
                <asp:label id="lblPMDisplay" runat="server" text="lblPMDisplay"></asp:label>
            </td>
        </tr>
    </table>









































</asp:Content>

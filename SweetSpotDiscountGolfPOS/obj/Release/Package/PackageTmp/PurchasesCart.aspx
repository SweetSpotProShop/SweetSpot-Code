<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="PurchasesCart.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.PurchasesCart" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>--%>

<asp:Content ID="NonActive" ContentPlaceHolderID="SPMaster" runat="server">
    <style>
        .costDetail {
            display: none;
        }

        .cost:hover .costDetail {
            display: block;
            position: absolute;
            text-align: left;
            max-width: 300px;
            max-height: 300px;
            overflow: auto;
            background-color: #fff;
            border: 2px solid #bbb;
            padding: 3px;
        }
    </style>
    <style>
        .priceDetail {
            display: none;
        }

        .price:hover .priceDetail {
            display: block;
            position: absolute;
            text-align: left;
            max-width: 300px;
            max-height: 300px;
            overflow: auto;
            background-color: #fff;
            border: 2px solid #bbb;
            padding: 3px;
        }
    </style>
    <%--<asp:Label ID="lblLocationID" runat="server" Text="Temp locaiton id label"></asp:Label>--%>
    <div id="menu_simple">
        <ul>
            <li><a>HOME</a></li>
            <li><a>CUSTOMERS</a></li>
            <li><a>SALES</a></li>
            <li><a>INVENTORY</a></li>
            <li><a>REPORTS</a></li>
            <li><a>SETTINGS</a></li>
        </ul>
    </div>
    <div id="image_simple">
        <img src="Images/SweetSpotLogo.jpg" />
    </div>
    <link rel="stylesheet" type="text/css" href="CSS/MainStyleSheet.css" />
</asp:Content>
<asp:Content ID="PurchasesPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <div id="Cart">
        <asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="btnAddPurchase">
            <asp:Label ID="lblCustomer" runat="server" Text="Customer Name:"></asp:Label>
            <asp:Label ID="lblCustomerDisplay" runat="server" Text="" Visible="false"></asp:Label>
            <asp:TextBox ID="txtCustomer" ReadOnly="true" runat="server"></asp:TextBox>
            <asp:Button ID="btnCustomerSelect" runat="server" Text="Select Different Customer" OnClick="btnCustomerSelect_Click" CausesValidation="false" />

            <br />
            <br />
            <div style="text-align: right">
                <asp:Label ID="lblReceiptNumber" runat="server" Text="Receipt No:"></asp:Label>
                <asp:Label ID="lblReceiptNumberDisplay" runat="server"></asp:Label>
                <br />
                <asp:Label ID="lblDate" runat="server" Text="Date:"></asp:Label>
                <asp:Label ID="lblDateDisplay" runat="server" Text=""></asp:Label>
                <hr />
            </div>
            <h3>Purchases</h3>
            <asp:Button ID="btnAddPurchase" runat="server" Text="Add Purchase" OnClick="btnAddPurchase_Click"/>
            <hr />
            <asp:GridView ID="grdPurchasedItems" runat="server" AutoGenerateColumns="false" Style="margin-right: 0px" OnRowEditing="OnRowEditing" OnRowUpdating="OnRowUpdating" OnRowCancelingEdit="ORowCanceling">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20%" HeaderText="Edit Item">
                        <ItemTemplate>
                            <asp:LinkButton Text="Edit" runat="server" CommandName="Edit" CommandArgument='<%#Eval("sku")%>' CausesValidation="false" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton Text="Update" runat="server" CommandName="Update" CommandArgument='<%#Eval("sku")%>' CausesValidation="false" />
                            <asp:LinkButton Text="Cancel" runat="server" CommandName="Cancel" CausesValidation="false" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="sku" ReadOnly="true" HeaderStyle-Width="20%" HeaderText="SKU" />
                    <asp:BoundField DataField="description" HeaderStyle-Width="20%" HeaderText="Description" />                    
                    <asp:BoundField DataField="cost" HeaderStyle-Width="20%" HeaderText="Cost" DataFormatString="{0:C}" />
                </Columns>
            </asp:GridView>
            <hr />
            <asp:Label ID="lblPurchaseAmount" runat="server" Text="Purchase Amount:"></asp:Label>
            <asp:Label ID="lblPurchaseAmountDisplay" runat="server" Text=""></asp:Label>
            <hr />
            <asp:Button ID="btnCancelPurchase" runat="server" Text="Cancel Purchase" OnClick="btnCancelPurchase_Click" CausesValidation="false" />
            <asp:Button ID="btnProceedToPayOut" runat="server" Text="Proceed to Pay Out" OnClick="btnProceedToPayOut_Click" CausesValidation="false" />
        </asp:Panel>
    </div>
</asp:Content>

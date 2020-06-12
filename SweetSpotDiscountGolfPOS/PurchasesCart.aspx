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
        <asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="btnAddPurchase" >
            <asp:Label ID="lblCustomer" runat="server" Text="Customer Name:" />
            <asp:TextBox ID="txtCustomer" runat="server" AutoCompleteType="Disabled" />
            <asp:Button ID="BtnCustomerSelect" runat="server" Text="Select Different Customer" OnClick="BtnCustomerSelect_Click" CausesValidation="false" />
            <div>
                <br />
                <div>
                    <asp:GridView ID="GrdCustomersSearched" runat="server" AutoGenerateColumns="false" ShowFooter="true" 
                        OnRowCommand="GrdCustomersSearched_RowCommand" AllowPaging="True" PageSize="5" 
                        OnPageIndexChanging="GrdCustomersSearched_PageIndexChanging" >
                        <Columns>
                            <asp:TemplateField HeaderText="Switch Customer">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnSwitchCustomer" CommandName="SwitchCustomer" CommandArgument='<%#Eval("CustomerId") %>' Text="Switch Customer" runat="server" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Button ID="BtnAddCustomer" runat="server" Text="Add Customer" OnClick="BtnAddCustomer_Click" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Customer Name">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("firstName") + " " + Eval("lastName") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <div>
                                        <asp:TextBox ID="txtFirstName" runat="server" AutoCompleteType="Disabled" placeholder="First Name" ToolTip="First Name" />
                                    </div>
                                    <div>
                                        <asp:TextBox ID="txtLastName" runat="server" AutoCompleteType="Disabled" placeholder="Last Name" ToolTip="Last Name" />
                                    </div>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Phone Number">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("primaryPhoneNumber") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtPhoneNumber" runat="server" AutoCompleteType="Disabled" placeholder="Phone Number" ToolTip="Phone Number" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Email Address" >
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("email") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <div>
                                        <asp:TextBox ID="txtEmail" runat="server" AutoCompleteType="Disabled" placeholder="Email" ToolTip="Email" />
                                    </div>
                                    <div>
                                        <asp:CheckBox ID="chkMarketingEnrollment" runat="server" Text="Marketing Enrollment" />
                                    </div>                                    
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <br />
            </div>
            <div style="text-align: right">
                <asp:Label ID="lblReceiptNumber" runat="server" Text="Receipt No:" />
                <asp:Label ID="lblReceiptNumberDisplay" runat="server" />
                <br />
                <asp:Label ID="lblDate" runat="server" Text="Date:" />
                <asp:Label ID="lblDateDisplay" runat="server" />
                <hr />
            </div>
            <h3>Purchases</h3>
            <asp:Button ID="BtnAddPurchase" runat="server" Text="Add Purchase" OnClick="BtnAddPurchase_Click" />
            <hr />
            <asp:GridView ID="grdPurchasedItems" runat="server" AutoGenerateColumns="false" Style="margin-right: 0px" 
                OnRowEditing="OnRowEditing" OnRowUpdating="OnRowUpdating" OnRowCancelingEdit="OnRowCanceling" >
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20%" HeaderText="Edit Item" >
                        <ItemTemplate>
                            <asp:LinkButton Text="Edit" runat="server" CommandName="Edit" CommandArgument='<%#Eval("intInvoiceItemID")%>' CausesValidation="false" />
							<asp:Label ID="lblInvoiceItemID" runat="server" Text='<%#Eval("intInvoiceItemID")%>' Visible="false" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton Text="Update" runat="server" CommandName="Update" CommandArgument='<%#Eval("intInvoiceItemID")%>' CausesValidation="false" />
                            <asp:LinkButton Text="Cancel" runat="server" CommandName="Cancel" CausesValidation="false" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="varSku" ReadOnly="true" HeaderStyle-Width="20%" HeaderText="SKU" />
                    <asp:BoundField DataField="varItemDescription" HeaderStyle-Width="20%" HeaderText="Description" />
                    <asp:BoundField DataField="varItemCost" HeaderStyle-Width="20%" HeaderText="Cost" DataFormatString="{0:C}" />
                </Columns>
            </asp:GridView>
            <hr />
            <asp:Label ID="lblPurchaseAmount" runat="server" Text="Purchase Amount:" />
            <asp:Label ID="lblPurchaseAmountDisplay" runat="server" />
            <hr />
            <asp:Button ID="BtnCancelPurchase" runat="server" Text="Cancel Purchase" OnClick="BtnCancelPurchase_Click" CausesValidation="false" />
            <asp:Button ID="BtnProceedToPayOut" runat="server" Text="Proceed to Pay Out" OnClick="BtnProceedToPayOut_Click" CausesValidation="false" />
        </asp:Panel>
    </div>
</asp:Content>

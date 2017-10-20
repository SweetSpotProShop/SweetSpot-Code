<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="CustomerHomePage.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.CustomerHomePage" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SPMaster" runat="server">
</asp:Content>--%>

<asp:Content ID="customerHomePageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <div id="Customer">
        <asp:Panel ID="custSearch" runat="server" DefaultButton="btnCustomerSearch">
            <h2>Customer Information</h2>
            <hr />
            <%--Enter search text to find matching customer information--%>
            <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
            <hr />
            <asp:Button ID="btnCustomerSearch" runat="server" Width="150" Text="Customer Search" OnClick="btnCustomerSearch_Click" />
            <%--<div class="divider" />--%>
            <asp:Button ID="btnAddNewCustomer" runat="server" Width="150" Text="Add New Customer" OnClick="btnAddNewCustomer_Click" />
            <hr />
            <asp:GridView ID="grdCustomersSearched" runat="server" AutoGenerateColumns="false" OnRowCommand="grdCustomersSearched_RowCommand">
                <Columns>
                    <asp:TemplateField HeaderText="Sale">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtnStartSale" CommandName="StartSale" CommandArgument='<%#Eval("CustomerId") %>' Text="Start Sale" runat="server">Start Sale</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="View Profile">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtnViewCustomer" CommandName="ViewProfile" CommandArgument='<%#Eval("CustomerId") %>' Text="View Profile" runat="server">View Profile</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Customer Number">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%#Eval("CustomerId") %>' ID="key"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Customer Name">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%#Eval("firstName") + " " + Eval("lastName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Customer Address">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%#Eval("primaryAddress") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Phone Number">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%#Eval("primaryPhoneNumber") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="City">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%#Eval("city") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    No current customer data, please search for a customer
                </EmptyDataTemplate>
            </asp:GridView>
        </asp:Panel>
    </div>
</asp:Content>

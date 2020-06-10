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
            <asp:Table ID="tblCustomerRow" runat="server">
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:TextBox ID="txtSearch" runat="server" AutoComplete="off" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="BtnCustomerSearch" runat="server" Width="150" Text="Customer Search" OnClick="BtnCustomerSearch_Click" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="BtnAddNewCustomer" runat="server" Width="150" Text="Add New Customer" OnClick="BtnAddNewCustomer_Click" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <hr />
            <asp:GridView ID="GrdCustomersSearched" runat="server" AutoGenerateColumns="false" OnRowCommand="GrdCustomersSearched_RowCommand" 
				AllowPaging="True" PageSize="25" OnPageIndexChanging="GrdCustomersSearched_PageIndexChanging" RowStyle-HorizontalAlign="Center">
                <Columns>
                    <asp:TemplateField HeaderText="Sale">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtnStartSale" CommandName="StartSale" CommandArgument='<%#Eval("intCustomerID") %>' Text="Start Sale" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="View Profile">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtnViewCustomer" CommandName="ViewProfile" CommandArgument='<%#Eval("intCustomerID") %>' Text="View Profile" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Customer Number">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%#Eval("intCustomerID") %>' ID="key" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Customer Name">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%#Eval("varFirstName") + " " + Eval("varLastName") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Customer Address">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%#Eval("varAddress") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Phone Number">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%#Eval("varContactNumber") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Email Address">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%#Eval("varEmailAddress") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="City">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%#Eval("varCityName") %>' />
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

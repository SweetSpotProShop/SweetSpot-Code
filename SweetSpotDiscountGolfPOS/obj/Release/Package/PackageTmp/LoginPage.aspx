<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="LoginPage.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.LoginPage" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>--%>

<asp:Content ID="NonActive" ContentPlaceHolderID="SPMaster" runat="server">
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

<asp:Content ID="LoginPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <div id="LoginPage">
        <asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="btnLogin">
            <table class="auto-style1">
                <caption class="auto-style2">
                    <strong>Login Form</strong>
                </caption>
                <tr>
                    <td class="style2"></td>
                    <td></td>
                    <td></td>
                </tr>

                <tr>
                    <td class="style2">Password:</td>
                    <td>
                        <asp:TextBox ID="txtPassword" TextMode="Password" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                            ControlToValidate="txtPassword" ErrorMessage="Please enter a password"
                            ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="style2"></td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td class="style2"></td>
                    <td>
                        <asp:Button ID="btnLogin" runat="server" Text="Log In" OnClick="btnLogin_Click" Width="128px" />
                    </td>
                    <td>
                        <asp:Label ID="lblError" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>

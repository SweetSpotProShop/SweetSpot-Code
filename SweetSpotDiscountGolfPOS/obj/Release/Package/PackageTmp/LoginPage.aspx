﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="LoginPage.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.LoginPage" %>

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
            <asp:Table ID="tblLogin" runat="server" CssClass="auto-style1" >
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblLogin" runat="server" Font-Bold="true" Text="Login Form" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblPassword" runat="server" Text="Password:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtPasswordEntry" runat="server" AutoCompleteType="Disabled" TextMode="Password" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:RequiredFieldValidator ID="rfvTxtPasswordEntry" runat="server" ControlToValidate="txtPasswordEntry" ErrorMessage="Please enter a password" ForeColor="Red" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell />
                    <asp:TableCell>
                        <asp:Button ID="BtnLogin" runat="server" Text="Log In" OnClick="BtnLogin_Click" Width="128px" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblError" runat="server" ForeColor="Red" />
						<%--<asp:Label ID="lblMaintenanceMessage" Text="This Site is currently unavailable as it is undergoing Maintenance." runat="server" ForeColor="Red" />--%>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </asp:Panel>
    </div>
</asp:Content>

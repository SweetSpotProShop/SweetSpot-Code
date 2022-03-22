<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="EmployeeAddNew.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.EmployeeAddNew" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SPMaster" runat="server">
</asp:Content>--%>

<asp:Content ID="EmployeeAddNewPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <div id="NewEmployee">
        <h2>New Employee</h2>
        <%--REMEMBER TO SET DEFAULT BUTTON--%>
        <asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="btnSaveEmployee">
            <asp:Table ID="Table1" runat="server" Width="100%">
                <asp:TableRow>
                    <asp:TableCell Width="25%">
                        <asp:Label ID="lblFirstName" runat="server" Text="First Name:" />
                    </asp:TableCell>
                    <asp:TableCell Width="25%">
                        <asp:TextBox ID="txtFirstName" runat="server" AutoComplete="off" ValidateRequestMode="Enabled" ViewStateMode="Enabled" Enabled="false" />
                    </asp:TableCell>
                    <asp:TableCell Width="25%">
                        <asp:Label ID="lblLastName" runat="server" Text="Last Name:" />
                    </asp:TableCell>
                    <asp:TableCell Width="25%">
                        <asp:TextBox ID="txtLastName" runat="server" AutoComplete="off" ValidateRequestMode="Enabled" ViewStateMode="Enabled" Enabled="false" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:RequiredFieldValidator ID="valFirstName" runat="server" ForeColor="red" ErrorMessage="Must enter a First Name" ControlToValidate="txtFirstName" />
                    </asp:TableCell>
                    <asp:TableCell>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:RequiredFieldValidator ID="valLastName" runat="server" ForeColor="red" ErrorMessage="Must enter a Last Name" ControlToValidate="txtLastName" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblJob" runat="server" Text="Job:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:DropDownList ID="ddlJob" runat="server" AutoPostBack="false" 
                            DataTextField="varJobTitle" DataValueField="intJobID" Enabled="false" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblLocation" runat="server" Text="Location:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:DropDownList ID="ddlLocation" runat="server" AutoPostBack="false"
                            DataTextField="varCityName" DataValueField="intLocationID" Enabled="false" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblEmail" runat="server" Text="Email:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtEmail" runat="server" AutoComplete="off" Enabled="false" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="4">
                        <hr />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblPrimaryPhoneNumber" runat="server" Text="Primary Phone Number:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtPrimaryPhoneNumber" runat="server" AutoComplete="off" ValidateRequestMode="Enabled" Enabled="false" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lbSecondaryPhoneNumber" runat="server" Text="Secondary Phone Number:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtSecondaryPhoneNumber" runat="server" AutoComplete="off" ValidateRequestMode="Enabled" Enabled="false" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblPrimaryAddress" runat="server" Text="Primary Address:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtPrimaryAddress" runat="server" AutoComplete="off" Enabled="false" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblSecondaryAddress" runat="server" Text="Secondary Address:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtSecondaryAddress" runat="server" AutoComplete="off" Enabled="false" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblCity" runat="server" Text="City:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtCity" runat="server" AutoComplete="off" Enabled="false" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblPostalCode" runat="server" Text="PostalCode:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtPostalCode" runat="server" AutoComplete="off" Enabled="false" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblProvince" runat="server" Text="Province:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:DropDownList ID="ddlProvince" runat="server" AutoPostBack="false" 
                            DataTextField="varProvinceName" DataValueField="intProvinceID" Enabled="false" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblCountry" runat="server" Text="Country:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:DropDownList ID="DdlCountry" runat="server" AutoPostBack="True" 
                            DataTextField="varCountryName" DataValueField="intCountryID" Enabled="false" 
                            OnSelectedIndexChanged="DdlCountry_SelectedIndexChanged" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="4">
                        <hr />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblNewPassword" runat="server" Text="Enter New Password" Visible="false" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtNewPassword" TextMode="Password" runat="server" AutoComplete="off" Visible="false" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblPasswordFormat" runat="server" Text="Passwords are only Numeric" Visible="false" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblNewPassword2" runat="server" Text="Retype New Password" Visible="false" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtNewPassword2" TextMode="Password" runat="server" AutoComplete="off" Visible="false" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="BtnSavePassword" runat="server" Text="Save New Password" Visible="false" OnClick="BtnSavePassword_Click" CausesValidation="false" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="4">
                        <hr />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Button ID="BtnAddEmployee" runat="server" Text="Add Employee" OnClick="BtnAddEmployee_Click" Visible="false" CausesValidation="true"/>
                        <asp:Button ID="BtnEditEmployee" runat="server" Text="Edit Employee" OnClick="BtnEditEmployee_Click" Visible="true" CausesValidation="false"/>
                        <asp:Button ID="BtnSaveEmployee" runat="server" Text="Save Changes" OnClick="BtnSaveEmployee_Click" Visible="false" CausesValidation="true"/>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="BtnBackToSearch" runat="server" Text="Exit Employee" OnClick="BtnBackToSearch_Click" Visible="true" CausesValidation="false"/>
                        <asp:Button ID="BtnCancel" runat="server" Text="Cancel" OnClick="BtnCancel_Click" Visible="false" CausesValidation="false"/>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </asp:Panel>
    </div>
</asp:Content>

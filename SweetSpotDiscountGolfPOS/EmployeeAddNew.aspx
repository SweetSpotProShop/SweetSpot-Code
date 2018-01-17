<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="EmployeeAddNew.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.EmployeeAddNew" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SPMaster" runat="server">
</asp:Content>--%>

<asp:Content ID="EmployeeAddNewPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <div id="NewEmployee">
        <%--Textboxes and Labels for user to enter employee info--%>
        <h2>New Employee</h2>
        <%--REMEMBER TO SET DEFAULT BUTTON--%>
        <asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="btnSaveEmployee">
            <asp:SqlDataSource ID="sqlCountrySource" runat="server" ConnectionString="<%$ ConnectionStrings:SweetSpotDevConnectionString %>" SelectCommand="SELECT * FROM [tbl_country] ORDER BY [countryDesc]"></asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlJobSource" runat="server" ConnectionString="<%$ ConnectionStrings:SweetSpotDevConnectionString %>" SelectCommand="SELECT * FROM [tbl_jobPosition] ORDER BY [title]"></asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlLocationSource" runat="server" ConnectionString="<%$ ConnectionStrings:SweetSpotDevConnectionString %>" SelectCommand="SELECT locationID, locationName FROM [tbl_location] ORDER BY [locationName]"></asp:SqlDataSource>

            <asp:Table ID="Table1" runat="server" Width="100%">
                <asp:TableRow>
                    <asp:TableCell Width="25%">
                        <asp:Label ID="lblFirstName" runat="server" Text="First Name:" />
                    </asp:TableCell>
                    <asp:TableCell Width="25%">
                        <asp:TextBox ID="txtFirstName" runat="server" ValidateRequestMode="Enabled" ViewStateMode="Enabled" Enabled="false" />
                        <%--<asp:Label ID="lblFirstNameDisplay" runat="server" Text="" Visible="true" />--%>
                    </asp:TableCell>
                    <asp:TableCell Width="25%">
                        <asp:Label ID="lblLastName" runat="server" Text="Last Name:" />
                    </asp:TableCell>
                    <asp:TableCell Width="25%">
                        <asp:TextBox ID="txtLastName" runat="server" ValidateRequestMode="Enabled" ViewStateMode="Enabled" Enabled="false" />
                        <%--<asp:Label ID="lblLastNameDisplay" runat="server" Text="" Visible="true" />--%>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell></asp:TableCell>
                    <asp:TableCell><asp:RequiredFieldValidator ID="valFirstName" runat="server" ForeColor="red" ErrorMessage="Must enter a First Name" ControlToValidate="txtFirstName" /></asp:TableCell>
                    <asp:TableCell></asp:TableCell>
                    <asp:TableCell><asp:RequiredFieldValidator ID="valLastName" runat="server" ForeColor="red" ErrorMessage="Must enter a Last Name" ControlToValidate="txtLastName" /></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblJob" runat="server" Text="Job:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:DropDownList ID="ddlJob" runat="server" AutoPostBack="True" DataSourceID="sqlJobSource" DataTextField="title" DataValueField="jobID" Enabled="false" />
                        <%--<asp:Label ID="lblJobDisplay" runat="server" Text="" Visible="true" />--%>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblLocation" runat="server" Text="Location:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:DropDownList ID="ddlLocation" runat="server" AutoPostBack="True" DataSourceID="sqlLocationSource" DataTextField="locationName" DataValueField="locationID" Enabled="false" />
                        <%--<asp:Label ID="lblLocationDisplay" runat="server" Text="" Visible="true" />--%>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblEmail" runat="server" Text="Email:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtEmail" runat="server" Enabled="false" />
                        <%--<asp:Label ID="lblEmailDisplay" runat="server" Text="" Visible="true" />--%>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="4"><hr /></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblPrimaryPhoneNumber" runat="server" Text="Primary Phone Number:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtPrimaryPhoneNumber" runat="server" ValidateRequestMode="Enabled" Enabled="false" />
                        <%--<asp:Label ID="lblPrimaryPhoneNumberDisplay" runat="server" Text="" Visible="true" />--%>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lbSecondaryPhoneNumber" runat="server" Text="Secondary Phone Number:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtSecondaryPhoneNumber" runat="server" ValidateRequestMode="Enabled" Enabled="false" />
                        <%--<asp:Label ID="lblSecondaryPhoneNumberDisplay" runat="server" Text="" Visible="true" />--%>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblPrimaryAddress" runat="server" Text="Primary Address:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtPrimaryAddress" runat="server" Enabled="false" />
                        <%--<asp:Label ID="lblPrimaryAddressDisplay" runat="server" Text="" Visible="true" />--%>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblSecondaryAddress" runat="server" Text="Secondary Address:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtSecondaryAddress" runat="server" Enabled="false" />
                        <%--<asp:Label ID="lblSecondaryAddressDisplay" runat="server" Text="" Visible="true" />--%>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblCity" runat="server" Text="City:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtCity" runat="server" Enabled="false" />
                        <%--<asp:Label ID="lblCityDisplay" runat="server" Text="" Visible="true" />--%>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblPostalCode" runat="server" Text="PostalCode:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtPostalCode" runat="server" Enabled="false" />
                        <%--<asp:Label ID="lblPostalCodeDisplay" runat="server" Text="" Visible="true" />--%>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblProvince" runat="server" Text="Province:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:DropDownList ID="ddlProvince" AutoPostBack="true" runat="server" Enabled="false" />
                        <%--<asp:Label ID="lblProvinceDisplay" runat="server" Text="" Visible="true" />--%>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblCountry" runat="server" Text="Country:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="True" DataSourceID="sqlCountrySource" DataTextField="countryDesc" DataValueField="countryID" Enabled="false" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" />
                        <%--<asp:Label ID="lblCountryDisplay" runat="server" Text="" Visible="true" />--%>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="4"><hr /></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblNewPassword" runat="server" Text="Enter New Password" Visible="false" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtNewPassword" TextMode="Password" runat="server" Text="" Visible="false" />
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
                        <asp:TextBox ID="txtNewPassword2" TextMode="Password" runat="server" Text="" Visible="false" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="btnSavePassword" runat="server" Text="Save New Password" Visible="false" OnClick="btnSavePassword_Click" CausesValidation="false" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="4"><hr /></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Button ID="btnAddEmployee" runat="server" Text="Add Employee" OnClick="btnAddEmployee_Click" Visible="false" CausesValidation="true"/>
                        <asp:Button ID="btnEditEmployee" runat="server" Text="Edit Employee" OnClick="btnEditEmployee_Click" Visible="true" CausesValidation="false"/>
                        <asp:Button ID="btnSaveEmployee" runat="server" Text="Save Changes" OnClick="btnSaveEmployee_Click" Visible="false" CausesValidation="true"/>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="btnBackToSearch" runat="server" Text="Exit Employee" OnClick="btnBackToSearch_Click" Visible="true" CausesValidation="false"/>
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" Visible="false" CausesValidation="false"/>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </asp:Panel>
    </div>
</asp:Content>

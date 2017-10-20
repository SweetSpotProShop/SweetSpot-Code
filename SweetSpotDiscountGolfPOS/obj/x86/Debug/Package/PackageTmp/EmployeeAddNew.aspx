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
            <asp:SqlDataSource ID="sqlProvinceSource" runat="server" ConnectionString="<%$ ConnectionStrings:SweetSpotDevConnectionString %>" SelectCommand="SELECT * FROM [tbl_provState] WHERE ([countryID] = @countryID) ORDER BY [provName]">
                <SelectParameters>
                    <asp:ControlParameter ControlID="ddlCountry" DefaultValue="0" Name="countryID" PropertyName="SelectedValue" Type="Int32" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlJobSource" runat="server" ConnectionString="<%$ ConnectionStrings:SweetSpotDevConnectionString %>" SelectCommand="SELECT * FROM [tbl_jobPosition] ORDER BY [title]"></asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlLocationSource" runat="server" ConnectionString="<%$ ConnectionStrings:SweetSpotDevConnectionString %>" SelectCommand="SELECT locationID, locationName FROM [tbl_location] ORDER BY [locationName]"></asp:SqlDataSource>

            <asp:Table ID="Table1" runat="server" Width="100%">
                <asp:TableRow>
                    <asp:TableCell Width="25%">
                        <asp:Label ID="lblFirstName" runat="server" Text="First Name: "></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell Width="25%">
                        <asp:TextBox ID="txtFirstName" runat="server" ValidateRequestMode="Enabled" ViewStateMode="Enabled" Visible="false"></asp:TextBox>
                        <asp:Label ID="lblFirstNameDisplay" runat="server" Text="" Visible="true"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell Width="25%">
                        <asp:Label ID="lblLastName" runat="server" Text="Last Name: "></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell Width="25%">
                        <asp:TextBox ID="txtLastName" runat="server" ValidateRequestMode="Enabled" ViewStateMode="Enabled" Visible="false"></asp:TextBox>
                        <asp:Label ID="lblLastNameDisplay" runat="server" Text="" Visible="true"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblJob" runat="server" Text="Job: "></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:DropDownList ID="ddlJob" runat="server" AutoPostBack="True" DataSourceID="sqlJobSource" DataTextField="title" DataValueField="jobID" Visible="false"></asp:DropDownList>
                        <asp:Label ID="lblJobDisplay" runat="server" Text="" Visible="true"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblLocation" runat="server" Text="Location: "></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:DropDownList ID="ddlLocation" runat="server" AutoPostBack="True" DataSourceID="sqlLocationSource" DataTextField="locationName" DataValueField="locationID" Visible="false"></asp:DropDownList>
                        <asp:Label ID="lblLocationDisplay" runat="server" Text="" Visible="true"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblEmail" runat="server" Text="Email: "></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtEmail" runat="server" Visible="false"></asp:TextBox>
                        <asp:Label ID="lblEmailDisplay" runat="server" Text="" Visible="true"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="4"><hr /></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblPrimaryPhoneNumber" runat="server" Text="Primary Phone Number: "></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtPrimaryPhoneNumber" runat="server" ValidateRequestMode="Enabled" Visible="false"></asp:TextBox>
                        <asp:Label ID="lblPrimaryPhoneNumberDisplay" runat="server" Text="" Visible="true"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lbSecondaryPhoneNumber" runat="server" Text="Secondary Phone Number: "></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtSecondaryPhoneNumber" runat="server" ValidateRequestMode="Enabled" Visible="false"></asp:TextBox>
                        <asp:Label ID="lblSecondaryPhoneNumberDisplay" runat="server" Text="" Visible="true"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblPrimaryAddress" runat="server" Text="Primary Address: "></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtPrimaryAddress" runat="server" Visible="false"></asp:TextBox>
                        <asp:Label ID="lblPrimaryAddressDisplay" runat="server" Text="" Visible="true"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblSecondaryAddress" runat="server" Text="Secondary Address: "></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtSecondaryAddress" runat="server" Visible="false"></asp:TextBox>
                        <asp:Label ID="lblSecondaryAddressDisplay" runat="server" Text="" Visible="true"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblCity" runat="server" Text="City: "></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtCity" runat="server" Visible="false"></asp:TextBox>
                        <asp:Label ID="lblCityDisplay" runat="server" Text="" Visible="true"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblPostalCode" runat="server" Text="PostalCode: "></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtPostalCode" runat="server" Visible="false"></asp:TextBox>
                        <asp:Label ID="lblPostalCodeDisplay" runat="server" Text="" Visible="true"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblProvince" runat="server" Text="Province: "></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:DropDownList ID="ddlProvince" runat="server" AutoPostBack="True" DataSourceID="sqlProvinceSource" DataTextField="provName" DataValueField="provStateID" Visible="false"></asp:DropDownList>
                        <asp:Label ID="lblProvinceDisplay" runat="server" Text="" Visible="true"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblCountry" runat="server" Text="Country: "></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="True" DataSourceID="sqlCountrySource" DataTextField="countryDesc" DataValueField="countryID" Visible="false"></asp:DropDownList>
                        <asp:Label ID="lblCountryDisplay" runat="server" Text="" Visible="true"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="4"><hr /></asp:TableCell>
                </asp:TableRow>

                <asp:TableRow>
                    <asp:TableCell ColumnSpan="4"><hr /></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Button ID="btnAddEmployee" runat="server" Text="Add Employee" OnClick="btnAddEmployee_Click" Visible="false" />
                        <asp:Button ID="btnEditEmployee" runat="server" Text="Edit Employee" OnClick="btnEditEmployee_Click" Visible="true" />
                        <asp:Button ID="btnSaveEmployee" runat="server" Text="Save Changes" OnClick="btnSaveEmployee_Click" Visible="false" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="btnBackToSearch" runat="server" Text="Exit Employee" OnClick="btnBackToSearch_Click" Visible="true" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" Visible="false" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </asp:Panel>
    </div>
</asp:Content>

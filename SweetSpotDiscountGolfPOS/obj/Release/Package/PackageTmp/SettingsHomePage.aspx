<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="SettingsHomePage.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.SettingsHomePage" Async="true" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SPMaster" runat="server">
</asp:Content>--%>

<asp:Content ID="SettingsPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <div id="Settings">
        <%--REMEMBER TO SET DEFAULT BUTTON--%>
        <asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="btnEmployeeSearch">
            <h2>Employee Management</h2>
            <hr />
            <%--Enter search text to find matching Employees information--%>
            <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
            <hr />
            <asp:Button ID="btnEmployeeSearch" runat="server" Width="150" Text="Employee Search" OnClick="btnEmployeeSearch_Click" />
            <div class="divider" />
            <asp:Button ID="btnAddNewEmployee" runat="server" Width="150" Text="Add New Employee" OnClick="btnAddNewEmployee_Click" />
            <hr />
            <asp:GridView ID="grdEmployeesSearched" AutoGenerateColumns="false" runat="server" OnRowCommand="grdEmployeesSearched_RowCommand">
                <Columns>
                    <asp:TemplateField HeaderText="View Profile">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtnViewEmployee" CommandName="ViewProfile" CommandArgument='<%#Eval("employeeID") %>' Text="View Profile" runat="server">View Profile</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Employee Number">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%#Eval("employeeID") %>' ID="key"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Employee Name">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%#Eval("firstName") + " " + Eval("lastName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Employee Address">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%#Eval("primaryAddress") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Phone Number">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%#Eval("primaryContactNumber") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="City">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%#Eval("city") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    No current employee data, please search for a employee
                </EmptyDataTemplate>
            </asp:GridView>
            <br />
            <hr />
            <h2>Taxes</h2>
            <hr />
            <div>
                <asp:SqlDataSource ID="SqlDSProvince" runat="server" ConnectionString="<%$ ConnectionStrings:SweetSpotDevConnectionString %>" SelectCommand="SELECT [provStateID], [provName] FROM [tbl_provState] WHERE ([countryID] = @countryID) ORDER BY [provName]">
                    <SelectParameters>
                        <asp:Parameter DefaultValue="0" Name="countryID" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDSTax" runat="server" ConnectionString="<%$ ConnectionStrings:SweetSpotDevConnectionString %>" SelectCommand="SELECT [tr].[taxID], [tr].[taxRate], [tbl_taxType].[taxName] FROM [tbl_taxRate] AS tr INNER JOIN [tbl_taxType] ON [tr].[taxID] = [tbl_taxType].[taxID] INNER JOIN (SELECT [taxID], MAX([taxDate]) AS MTD FROM [tbl_taxRate] WHERE ([taxDate] <= @recDate) AND (provStateID = @provStateID) GROUP BY [taxID]) AS td ON [tr].[taxID] = [td].[taxID] AND [tr].[taxDate] = [td].[MTD] WHERE ([tr].[provStateID] = @provStateID)">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="lblCurrentDate" DefaultValue="0" Name="recDate" PropertyName="Text" />
                        <asp:ControlParameter ControlID="ddlProvince" DefaultValue="1" Name="provStateID" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:Table runat="server">
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="lblProvince" runat="server" Text="Province:"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:DropDownList ID="ddlProvince" runat="server" AutoPostBack="true" DataSourceID="SqlDSProvince" DataTextField="provName" DataValueField="provStateID" ></asp:DropDownList>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="lblTax" runat="server" Text="Tax:"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:DropDownList ID="ddlTax" runat="server" AutoPostBack="true" DataSourceID="sqlDSTax" DataTextField="taxName" DataValueField="taxID" OnPreRender="ddlTax_SelectedIndexChanged"></asp:DropDownList>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="lblCurrentDate" runat="server" Text="" Visible="false"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="lblCurrent" runat="server" Text="Current Rate:"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="lblNewRate" runat="server" Text="New Rate:"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="lblAsOfDate" runat="server" Text="As of:"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="lblCurrentDisplay" runat="server" Text=""></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtNewRate" runat="server"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtDate" runat="server" Text="" ></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Button ID="btnSaveTheTax" Text="Set New Tax Rate" runat="server" OnClick="btnSaveTheTax_Click"></asp:Button>
                        </asp:TableCell>
                        <asp:TableCell>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <br />
            <hr />
            <h2>Import Files From Excel</h2>
            <hr />
            <div>
                <asp:Table runat="server" GridLines="Both" BorderStyle="Solid" BorderWidth="1px" BorderColor="Black">
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="lblproduct" runat="server" Text="Import Items"></asp:Label>
                            <div>
                                <asp:FileUpload ID="fupItemSheet" runat="server" />

                            </div>
                            <asp:Button ID="btnLoadItems" runat="server" Width="150" Text="Import Items" OnClick="btnLoadItems_Click" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="lblLoadCustomers" runat="server" Text="Import Customers"></asp:Label>
                            <div>
                                <asp:FileUpload ID="fupCustomers" runat="server" />

                            </div>
                            <asp:Button ID="btnImportCustomers" runat="server" Width="150" Text="Import Customers" onclick="btnImportCustomers_Click"/>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>

            </div>

            <%--<asp:Button ID="btnLoadCustomers" runat="server" Width="150" Text="Load Customers" OnClick="btnLoadCustomers_Click" />
            <asp:Button ID="btnLoadEmployees" runat="server" Width="150" Text="Load Employees" OnClick="btnLoadEmployee_Click" />--%>

            <br />
            <hr />
            <h2>Export Items To Excel</h2>
            <hr />
            <asp:Button ID="btnExportAll" runat="server" Width="150" Text="Export All" OnClick="btnExportAll_Click" />
            <asp:Button ID="btnExportClubs" runat="server" Width="150" Text="Export Clubs" OnClick="btnExportClubs_Click" />
            <asp:Button ID="btnExportClothing" runat="server" Width="150" Text="Export Clothing" OnClick="btnExportClothing_Click" />
            <asp:Button ID="btnExportAccessories" runat="server" Width="150" Text="Export Accessories" OnClick="btnExportAccessories_Click" />
            <asp:Button ID="btnExportInvoices" runat="server" Width="150" Text="Export Invoices" OnClick="btnExportInvoices_Click" />
            <asp:Button ID="btnExportEmails" runat="server" Width="150" Text="Export Emails" OnClick="btnExportEmails_Click" />
            <script>
                function UpdateProgressLabel()
                {
                    alert("TEST");
                    var progress = <%=this.progress%>;                    
                    document.getElementById('IndividualPageContent_Label1').value = progress;
                }
            </script>
        </asp:Panel>
    </div>
</asp:Content>
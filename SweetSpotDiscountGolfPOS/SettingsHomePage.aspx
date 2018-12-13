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
            <asp:Table ID="tblEmployee" runat="server">
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:TextBox ID="txtSearch" runat="server" AutoComplete="off" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="btnEmployeeSearch" runat="server" Width="150" Text="Employee Search" OnClick="btnEmployeeSearch_Click" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="btnAddNewEmployee" runat="server" Width="150" Text="Add New Employee" OnClick="btnAddNewEmployee_Click" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <hr />
            <asp:GridView ID="grdEmployeesSearched" AutoGenerateColumns="false" runat="server" OnRowCommand="grdEmployeesSearched_RowCommand">
                <Columns>
                    <asp:TemplateField HeaderText="View Profile">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtnViewEmployee" CommandName="ViewProfile" CommandArgument='<%#Eval("employeeID") %>' Text="View Profile" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Employee Number">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%#Eval("employeeID") %>' ID="key" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Employee Name">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%#Eval("firstName") + " " + Eval("lastName") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Employee Address">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%#Eval("primaryAddress") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Phone Number">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%#Eval("primaryContactNumber") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="City">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%#Eval("city") %>' />
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
                <asp:Table runat="server">
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="lblProvince" runat="server" Text="Province:" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:DropDownList ID="ddlProvince" runat="server" AutoPostBack="true" 
                                DataTextField="provName" DataValueField="provStateID" 
                                OnSelectedIndexChanged="ddlProvince_SelectedIndexChanged" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="lblTax" runat="server" Text="Tax:" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:DropDownList ID="ddlTax" runat="server" AutoPostBack="true" 
                                DataTextField="taxName" DataValueField="taxID" 
                                OnSelectedIndexChanged="ddlTax_SelectedIndexChanged" 
                                OnPreRender="ddlTax_SelectedIndexChanged" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="lblCurrentDate" runat="server" Visible="false" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="lblCurrent" runat="server" Text="Current Rate:" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="lblNewRate" runat="server" Text="New Rate:" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="lblAsOfDate" runat="server" Text="As of:" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="lblCurrentDisplay" runat="server" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtNewRate" runat="server" AutoComplete="off" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtDate" runat="server" AutoComplete="off" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Button ID="btnSaveTheTax" Text="Set New Tax Rate" runat="server" OnClick="btnSaveTheTax_Click" />
                        </asp:TableCell>
                        <asp:TableCell>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <br />
            <hr />
            <h2>New Model and Brand</h2>
            <hr />
            <div>
                <asp:Table runat="server" GridLines="Both" BorderStyle="Solid" BorderWidth="1px" BorderColor="Black">
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="lblModel" runat="server" Text="Model" />
                            <div>
                                <asp:TextBox ID="txtModelOne" runat="server" AutoComplete="off" placeholder="Model" />
                            </div>
                            <div>
                                <asp:TextBox ID="txtModelTwo" runat="server" AutoComplete="off" placeholder="Confirm Model" />
                            </div>
                            <asp:Button ID="btnAddModel" runat="server" Width="150" Text="Add Model" OnClick="btnAddModel_Click" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="lblBrand" runat="server" Text="Brand" />
                            <div>
                                <asp:TextBox ID="txtBrandOne" runat="server" AutoComplete="off" placeholder="Brand" />
                            </div>
                            <div>
                                <asp:TextBox ID="txtBrandTwo" runat="server"  AutoComplete="off" placeholder="Confirm Brand" />
                            </div>
                            <asp:Button ID="btnAddBrand" runat="server" Width="150" Text="Add Brand" OnClick="btnAddBrand_Click" />
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
                            <asp:Label ID="lblproduct" runat="server" Text="Import Items" />
                            <div>
                                <asp:FileUpload ID="fupItemSheet" runat="server" />
                            </div>
                            <asp:Button ID="btnLoadItems" runat="server" Width="150" Text="Import Items" OnClick="btnLoadItems_Click" />
                            <%--The actual button--%>
                            <asp:Image id="imgLoadingItemImport" ImageUrl="~/Images/ajax-loader.gif" visible="false"  runat="server"/>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="lblLoadCustomers" runat="server" Text="Import Customers" />
                            <div>
                                <asp:FileUpload ID="fupCustomers" runat="server" />
                            </div>
                            <asp:Button ID="btnImportCustomers" runat="server" Width="150" Text="Import Customers" OnClientClick="showImage" OnClick="btnImportCustomers_Click" />
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
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
                function UpdateProgressLabel() {
                    alert("TEST");
                    var progress = <%=this.progress%>;                    
                    document.getElementById('IndividualPageContent_Label1').value = progress;
                }

                function showImage() {
                    var img = document.getElementById('imgLoadingItemImport');
                    img.style.visibility = 'visible';
                }
            </script>
        </asp:Panel>
    </div>
</asp:Content>
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
                        <asp:TextBox ID="txtSearch" runat="server" AutoCompleteType="Disabled" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="BtnEmployeeSearch" runat="server" Width="150" Text="Employee Search" OnClick="BtnEmployeeSearch_Click" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="BtnAddNewEmployee" runat="server" Width="150" Text="Add New Employee" OnClick="BtnAddNewEmployee_Click" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <hr />
            <asp:GridView ID="GrdEmployeesSearched" AutoGenerateColumns="false" runat="server" OnRowCommand="GrdEmployeesSearched_RowCommand">
                <Columns>
                    <asp:TemplateField HeaderText="View Profile">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtnViewEmployee" CommandName="ViewProfile" CommandArgument='<%#Eval("intEmployeeID") %>' Text="View Profile" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Employee Number">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%#Eval("intEmployeeID") %>' ID="key" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Employee Name">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%#Eval("varFirstName") + " " + Eval("varLastName") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Employee Address">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%#Eval("varAddress") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Phone Number">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%#Eval("varContactNumber") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="City">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%#Eval("varCityName") %>' />
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
                            <asp:DropDownList ID="DdlProvince" runat="server" AutoPostBack="true" 
                                DataTextField="varProvinceName" DataValueField="intProvinceID" 
                                OnSelectedIndexChanged="DdlProvince_SelectedIndexChanged" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="lblTax" runat="server" Text="Tax:" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:DropDownList ID="DdlTax" runat="server" AutoPostBack="true" 
                                DataTextField="varTaxName" DataValueField="intTaxID" 
                                OnSelectedIndexChanged="DdlTax_SelectedIndexChanged" 
                                OnPreRender="DdlTax_SelectedIndexChanged" />
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
                            <asp:TextBox ID="txtNewRate" runat="server" AutoCompleteType="Disabled" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtDate" runat="server" AutoCompleteType="Disabled" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Button ID="BtnSaveTheTax" Text="Set New Tax Rate" runat="server" OnClick="BtnSaveTheTax_Click" />
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
                                <asp:TextBox ID="txtModelOne" runat="server" AutoCompleteType="Disabled" placeholder="Model" />
                            </div>
                            <div>
                                <asp:TextBox ID="txtModelTwo" runat="server" AutoCompleteType="Disabled" placeholder="Confirm Model" />
                            </div>
                            <asp:Button ID="BtnAddModel" runat="server" Width="150" Text="Add Model" OnClick="BtnAddModel_Click" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="lblBrand" runat="server" Text="Brand" />
                            <div>
                                <asp:TextBox ID="txtBrandOne" runat="server" AutoCompleteType="Disabled" placeholder="Brand" />
                            </div>
                            <div>
                                <asp:TextBox ID="txtBrandTwo" runat="server"  AutoCompleteType="Disabled" placeholder="Confirm Brand" />
                            </div>
                            <asp:Button ID="BtnAddBrand" runat="server" Width="150" Text="Add Brand" OnClick="BtnAddBrand_Click" />
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
                            <asp:Table runat="server">
                                <asp:TableHeaderRow>
                                    <asp:TableHeaderCell>
                                        <asp:Label ID="lblproduct" runat="server" Text="Import Items" />
                                    </asp:TableHeaderCell>                                    
                                </asp:TableHeaderRow>
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:FileUpload ID="fupItemSheet" runat="server" />
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Button ID="BtnLoadItems" runat="server" Width="150" Text="Import Items" OnClick="BtnLoadItems_Click" />
                                        <%--The actual button--%>
                                        <asp:Image id="imgLoadingItemImport" ImageUrl="~/Images/ajax-loader.gif" visible="false"  runat="server"/>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Table runat="server">
                                <asp:TableHeaderRow>
                                    <asp:TableHeaderCell>
                                        <asp:Label ID="lblSpecialUpdateTool" runat="server" Text="Special Update Tool" />
                                    </asp:TableHeaderCell>                                    
                                </asp:TableHeaderRow>
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:FileUpload ID="fupSpecialUpdate" runat="server"/>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Label ID="lblReferenceColumn" runat="server" Text="Reference Column" />
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:DropDownList ID="ddlReferenceColumn" runat="server">

                                            <asp:ListItem Enabled="true" Text="Select Subject" Value="-1"/>
                                            <asp:ListItem Text="SKU" Value="1"/>

                                        </asp:DropDownList>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Label ID="lblSpecialUpateColumn" runat="server" Text="Update Column" />
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:DropDownList ID="ddlSpecialUpdateColumn" OnSelectedIndexChanged="ddlSpecialUpdateColumn_SelectedIndexChanged" AutoPostBack="true" runat="server">
                                            
                                            <asp:ListItem Enabled="true" Text="Select Subject" Value="-1"/>
                                            <asp:ListItem Text="Location" Value="1"/>
                                            <asp:ListItem Text="Cost" Value="2"/>
                                            <asp:ListItem Text="Price" Value="3"/>
                                            <asp:ListItem Text="ProdID" Value="4"/>
                                            <asp:ListItem Text="Quantity" Value="5"/>
                                            <asp:ListItem Text="Dexterity" Value="6"/>
                                            <asp:ListItem Text="Comments" Value="7"/>

                                        </asp:DropDownList>
                                    </asp:TableCell>
                                </asp:TableRow>

                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Label ID="LblTableSelect" runat="server" Text="Select Table" />
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:DropDownList ID="DdlTableSelect" runat="server">
                                            <asp:ListItem Text="Clubs" Value="1"/>
                                            <asp:ListItem Text="Clothing" Value="2"/>
                                            <asp:ListItem Text="Accessories" Value="3"/>
                                        </asp:DropDownList>
                                    </asp:TableCell>
                                </asp:TableRow>

                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Button ID="btnSpecialUpdateTool" runat="server" Width="150" Text="Run Update Tool" OnClick="btnSpecialUpdateTool_Click" />
                                        <asp:Image id="imgLoadingItemImport2" ImageUrl="~/Images/ajax-loader.gif" visible="false"  runat="server"/>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <br />
            <hr />
            <h2>Export Items To Excel</h2>
            <hr />
            <asp:Button ID="BtnExportAll" runat="server" Width="150" Text="Export All" OnClick="BtnExportAll_Click" />
            <asp:Button ID="BtnExportClubs" runat="server" Width="150" Text="Export Clubs" OnClick="BtnExportClubs_Click" />
            <asp:Button ID="BtnExportClothing" runat="server" Width="150" Text="Export Clothing" OnClick="BtnExportClothing_Click" />
            <asp:Button ID="BtnExportAccessories" runat="server" Width="150" Text="Export Accessories" OnClick="BtnExportAccessories_Click" />
            <%--<asp:Button ID="BtnExportInvoices" runat="server" Width="150" Text="Export Invoices" OnClick="BtnExportInvoices_Click" />--%>
            <asp:Button ID="BtnExportEmails" runat="server" Width="150" Text="Export Emails" OnClick="BtnExportEmails_Click" />
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
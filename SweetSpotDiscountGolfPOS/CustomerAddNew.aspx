<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="CustomerAddNew.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.CustomerAddNew" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SPMaster" runat="server">
</asp:Content>--%>

<asp:Content ID="CustomerAddNewPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <div id="NewCustomer">
        <asp:panel id="pnlDefaultButton" runat="server" defaultbutton="btnSaveCustomer">
            <%--Textboxes and Labels for user to enter customer info--%>
            <h2>Customer Management</h2>
            <asp:SqlDataSource ID="sqlCountrySource" runat="server" ConnectionString="<%$ ConnectionStrings:SweetSpotDevConnectionString %>" SelectCommand="SELECT * FROM [tbl_country] ORDER BY [countryDesc]"></asp:SqlDataSource>
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
                    </asp:TableCell>
                    <asp:TableCell><asp:RequiredFieldValidator ID="valFirstName" runat="server" ForeColor="red" ErrorMessage="Must enter a First Name" ControlToValidate="txtFirstName"></asp:RequiredFieldValidator></asp:TableCell>
                    <asp:TableCell>
                    </asp:TableCell>
                    <asp:TableCell><asp:RequiredFieldValidator ID="valLastName" runat="server" ForeColor="red" ErrorMessage="Must enter a Last Name" ControlToValidate="txtLastName"></asp:RequiredFieldValidator></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblEmail" runat="server" Text="Email: "></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtEmail" runat="server" Visible="false"></asp:TextBox>
                        <asp:Label ID="lblEmailDisplay" runat="server" Text="" Visible="true"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:CheckBox ID="chkEmailList" runat="server" Text="Check to add to email list" Enabled="false"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="4"><hr /></asp:TableCell>
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
                        <%--<asp:Label ID="lblBillingAddress" runat="server" Text="Billing Address: "></asp:Label>--%>
                    </asp:TableCell>
                    <asp:TableCell>
                        <%--<asp:TextBox ID="txtBillingAddress" runat="server" Visible="false"></asp:TextBox>
                        <asp:Label ID="lblBillingAddressDisplay" runat="server" Text="" Visible="true"></asp:Label>--%>
                    </asp:TableCell>
                    <asp:TableCell ColumnSpan="2">
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
                        <asp:DropDownList ID="ddlProvince" runat="server" Visible="false"></asp:DropDownList>
                        <asp:Label ID="lblProvinceDisplay" runat="server" Text="" Visible="true"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblCountry" runat="server" Text="Country: "></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="True" DataSourceID="sqlCountrySource" DataTextField="countryDesc" DataValueField="countryID" Visible="false" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged"></asp:DropDownList>
                        <asp:Label ID="lblCountryDisplay" runat="server" Text="" Visible="true"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="4"><hr /></asp:TableCell>
                </asp:TableRow>
                <%--<asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblAccount" runat="server" Text="Account Number: "></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblAccountNumber" runat="server" Text=""></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblBalance" runat="server" Text="Account Balance: "></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblAccountBalance" runat="server" Text=""></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="4"><hr /></asp:TableCell>
                </asp:TableRow>--%>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Button ID="btnAddCustomer" runat="server" Text="Add Customer" OnClick="btnAddCustomer_Click" Visible="false" CausesValidation="true"/>
                        <asp:Button ID="btnEditCustomer" runat="server" Text="Edit Customer" OnClick="btnEditCustomer_Click" Visible="true" CausesValidation="false"/>
                        <asp:Button ID="btnSaveCustomer" runat="server" Text="Save Changes" OnClick="btnSaveCustomer_Click" Visible="false" CausesValidation="true"/>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="btnStartSale" runat="server" Text="Start Sale" OnClick="btnStartSale_Click" Visible="true" CausesValidation="false"/>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="btnBackToSearch" runat="server" Text="Exit Customer" OnClick="btnBackToSearch_Click" Visible="true" CausesValidation="false"/>
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" Visible="false" CausesValidation="false"/>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <hr />
            <%--Gridview for the invoices--%>
            <h2>Customer Invoices</h2>
            <div>
                <asp:GridView ID="grdInvoiceSelection" runat="server" AutoGenerateColumns="false" Width="100%" OnRowCommand="grdInvoiceSelection_RowCommand">
                    <Columns>
                        <asp:TemplateField HeaderText=" View Invoice">
                            <ItemTemplate>
                                <asp:LinkButton ID="lkbInvoiceNum" runat="server" CommandName="returnInvoice" CommandArgument='<%#Eval("invoiceNum") + "-" + Eval("invoiceSub")%>' Text='<%#Eval("invoiceNum") + "-" + Eval("invoiceSub") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Invoice Date">
                            <ItemTemplate>
                                <asp:Label ID="lblInvoiceDate" runat="server" Text='<%#Eval("invoiceDate","{0: MM/dd/yy}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Customer Name">
                            <ItemTemplate>
                                <asp:Label ID="lblCustomerName" runat="server" Text='<%#Eval("customerName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Discount">
                            <ItemTemplate>
                                <asp:Label ID="lblDiscountAmount" runat="server" Text='<%#Eval("discountAmount","{0:C}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Trade In">
                            <ItemTemplate>
                                <asp:Label ID="lblTradeInAmount" runat="server" Text='<%#Eval("tradeinAmount","{0:C}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Subtotal">
                            <ItemTemplate>
                                <asp:Label ID="lblSubtotal" runat="server" Text='<%#Eval("subTotal","{0:C}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="GST">
                            <ItemTemplate>
                                <asp:Label ID="lblGSTAmount" runat="server" Text='<%#Eval("governmentTax","{0:C}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PST">
                            <ItemTemplate>
                                <asp:Label ID="lblPSTAmount" runat="server" Text='<%#Eval("provincialTax","{0:C}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total">
                            <ItemTemplate>
                                <asp:Label ID="lblAmountPaid" runat="server" Text='<%#Eval("balanceDue","{0:C}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Employee Name">
                            <ItemTemplate>
                                <asp:Label ID="lblEmployeeName" runat="server" Text='<%#Eval("employeeName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </asp:panel>
    </div>
</asp:Content>

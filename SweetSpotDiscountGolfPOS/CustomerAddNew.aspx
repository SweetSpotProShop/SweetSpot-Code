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
                        <asp:Label ID="lblPrimaryPhoneNumber" runat="server" Text="Primary Phone Number:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtPrimaryPhoneNumber" runat="server" AutoComplete="off" ValidateRequestMode="Enabled" Enabled="false" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblSecondaryPhoneNumber" runat="server" Text="Secondary Phone Number:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtSecondaryPhoneNumber" runat="server" AutoComplete="off" ValidateRequestMode="Enabled" Enabled="false" />
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
                        <asp:Label ID="lblEmail" runat="server" Text="Email:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtEmail" runat="server" AutoComplete="off" Enabled="false" />
                    </asp:TableCell>
                    <asp:TableCell>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:CheckBox ID="chkEmailList" runat="server" Text="Marketing Enrollment" Enabled="false"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="4">
                        <hr />
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
                    </asp:TableCell>
                    <asp:TableCell>
                    </asp:TableCell>
                    <asp:TableCell ColumnSpan="2">
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
                        <asp:DropDownList ID="ddlProvince" AutoPostBack="false" runat="server"
                            DataTextField = "varProvinceName" DataValueField = "intProvinceID" Enabled="false" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblCountry" runat="server" Text="Country:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:DropDownList ID="DdlCountry" runat="server" AutoPostBack="true"
                            DataTextField = "varCountryName" DataValueField = "intCountryID" Enabled="false" 
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
                        <asp:Button ID="BtnAddCustomer" runat="server" Text="Add Customer" OnClick="BtnAddCustomer_Click" Visible="false" CausesValidation="true"/>
                        <asp:Button ID="BtnEditCustomer" runat="server" Text="Edit Customer" OnClick="BtnEditCustomer_Click" Visible="true" CausesValidation="false"/>
                        <asp:Button ID="BtnSaveCustomer" runat="server" Text="Save Changes" OnClick="BtnSaveCustomer_Click" Visible="false" CausesValidation="true"/>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="BtnStartSale" runat="server" Text="Start Sale" OnClick="BtnStartSale_Click" Visible="true" CausesValidation="false"/>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="BtnBackToSearch" runat="server" Text="Exit Customer" OnClick="BtnBackToSearch_Click" Visible="true" CausesValidation="false"/>
                        <asp:Button ID="BtnCancel" runat="server" Text="Cancel" OnClick="BtnCancel_Click" Visible="false" CausesValidation="false"/>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <hr />
            <%--Gridview for the invoices--%>
            <h2>Customer Invoices</h2>
            <div>
                <asp:GridView ID="GrdInvoiceSelection" runat="server" AutoGenerateColumns="false" Width="100%" OnRowCommand="GrdInvoiceSelection_RowCommand" RowStyle-HorizontalAlign="Center" >
                    <Columns>
                        <asp:TemplateField HeaderText=" View Invoice">
                            <ItemTemplate>
                                <asp:LinkButton ID="lkbInvoiceNum" runat="server" CommandName="returnInvoice" CommandArgument='<%#Eval("intInvoiceID")%>' Text='<%#Eval("varInvoiceNumber") + "-" + Eval("intInvoiceSubNumber") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Invoice Date">
                            <ItemTemplate>
                                <asp:Label ID="lblInvoiceDate" runat="server" Text='<%#Eval("dtmInvoiceDate","{0: dd/MMM/yy}") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Discount">
                            <ItemTemplate>
                                <asp:Label ID="lblDiscountAmount" runat="server" Text='<%#Eval("fltTotalDiscount","{0:C}") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Trade In">
                            <ItemTemplate>
                                <asp:Label ID="lblTradeInAmount" runat="server" Text='<%#Eval("fltTotalTradeIn","{0:C}") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Subtotal">
                            <ItemTemplate>
                                <asp:Label ID="lblSubtotal" runat="server" Text='<%#Eval("fltSubTotal","{0:C}") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="GST">
                            <ItemTemplate>
                                <asp:Label ID="lblGSTAmount" runat="server" Text='<%#Eval("fltGovernmentTaxAmount","{0:C}") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PST">
                            <ItemTemplate>
                                <asp:Label ID="lblPSTAmount" runat="server" Text='<%#Eval("fltProvincialTaxAmount","{0:C}") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total">
                            <ItemTemplate>
                                <asp:Label ID="lblAmountPaid" runat="server" Text='<%# Eval("fltBalanceDue", "{0:C}") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Employee Name">
                            <ItemTemplate>
                                <asp:Label ID="lblEmployeeName" runat="server" Text='<%#Eval("employee.varFirstName") +" "+Eval("employee.varLastName") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </asp:panel>
    </div>
</asp:Content>

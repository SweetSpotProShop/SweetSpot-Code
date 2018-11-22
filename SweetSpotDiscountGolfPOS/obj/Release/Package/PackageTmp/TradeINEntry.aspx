<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TradeINEntry.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.TradeINEntry" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div id="NewInventory">
                <h2>Trade-In Item</h2>
                <asp:Table ID="Table1" runat="server" Width="100%">
                    <asp:TableRow>
                        <asp:TableCell Width="25%">
                            <asp:Label ID="lblSKU" runat="server" Text="SKU:" />
                        </asp:TableCell>
                        <asp:TableCell Width="25%">
                            <asp:Label ID="lblSKUDisplay" runat="server" Text="" />
                        </asp:TableCell>
                        <asp:TableCell Width="25%">
                            <asp:Label ID="lblCost" runat="server" Text="Cost:" />
                        </asp:TableCell>
                        <asp:TableCell Width="25%">
                            <asp:TextBox ID="txtCost" runat="server" Visible="true" AutoComplete="off" Text="0" />
                            <asp:RequiredFieldValidator ID="rfvCost"
                                runat="server" ControlToValidate="txtCost"
                                ErrorMessage="Cost Required"
                                ForeColor="Red" />
                            <asp:RegularExpressionValidator ID="revCost"
                                ControlToValidate="txtCost"
                                ValidationExpression="[-+]?([0-9]*\.[0-9]+|[0-9]+)"
                                Display="Static"
                                EnableClientScript="true"
                                ErrorMessage="Requires a number"
                                runat="server" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="lblBrand" runat="server" Text="Brand Name:" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:DropDownList ID="ddlBrand" runat="server" AutoPostBack="false" Visible="true" />
                            <asp:RequiredFieldValidator ID="rfvBrand"
                                runat="server" ControlToValidate="ddlBrand"
                                ErrorMessage="Brand Required"
                                ForeColor="Red" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="lblPrice" runat="server" Text="Price:" />                            
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtPrice" runat="server" Visible="true" AutoComplete="off" Text="0" />
                            <asp:RegularExpressionValidator ID="revPrice"
                                ControlToValidate="txtPrice"
                                ValidationExpression="[-+]?([0-9]*\.[0-9]+|[0-9]+)"
                                Display="Static"
                                EnableClientScript="true"
                                ErrorMessage="Requires a number"
                                runat="server" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="lblQuantity" runat="server" Text="Quantity:" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtQuantity" runat="server" Visible="True" AutoComplete="off" Text="1" />
                            <asp:RequiredFieldValidator ID="rfvQuantity"
                                runat="server" ControlToValidate="txtQuantity"
                                ErrorMessage="Quantity Required"
                                ForeColor="Red" />
                            <asp:RegularExpressionValidator ID="revQuantity"
                                ControlToValidate="txtQuantity"
                                ValidationExpression="[-+]?([0-9]*\.[0-9]+|[0-9]+)"
                                Display="Static"
                                EnableClientScript="true"
                                ErrorMessage="Requires a number"
                                runat="server" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell ColumnSpan="4">
                            <hr />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="lblClubType" runat="server" Text="Club Type:" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:DropDownList ID="ddlClubType" runat="server" AutoPostBack="false" Visible="True" />
                            <asp:RequiredFieldValidator ID="rfvClubType"
                                runat="server" ControlToValidate="ddlClubType"
                                ErrorMessage="Club Type Required"
                                ForeColor="Red" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="lblModel" runat="server" Text="Model:" Visible="true" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:DropDownList ID="ddlModel" runat="server" AutoPostBack="false" Visible="True" />
                            <asp:RequiredFieldValidator ID="rfvModel"
                                runat="server" ControlToValidate="ddlModel"
                                ErrorMessage="Model Required"
                                ForeColor="Red" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="lblShaft" runat="server" Text="Shaft:" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtShaft" runat="server" Visible="True" AutoComplete="off" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="lblNumberofClubs" runat="server" Text="Number of Clubs:" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtNumberofClubs" runat="server" Visible="True" AutoComplete="off" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="lblClubSpec" runat="server" Text="Club Spec:" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtClubSpec" runat="server" Visible="True" AutoComplete="off" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="lblShaftSpec" runat="server" Text="Shaft Spec:" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtShaftSpec" runat="server" Visible="True" AutoComplete="off" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="lblShaftFlex" runat="server" Text="ShaftFlex:" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtShaftFlex" runat="server" Visible="True" AutoComplete="off" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="lblDexterity" runat="server" Text="Dexterity:" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtDexterity" runat="server" Visible="True" AutoComplete="off" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell ColumnSpan="4">
                            <hr />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell ColumnSpan="2">
                            <asp:Label ID="lblComments" runat="server" Text="Comments:" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:CheckBox ID="chkUsed" runat="server" Text="Used" Enabled="false" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell ColumnSpan="4">
                            <asp:TextBox Height="30px" Width="100%" ID="txtComments" runat="server" Visible="true" AutoComplete="off" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell ColumnSpan="4">
                            <hr />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Button ID="btnAddItem" runat="server" Text="Add Item" OnClick="btnAddTradeIN_Click" Visible="true" CausesValidation="True" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" Visible="true" CausesValidation="false" />
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
        </div>
    </form>
</body>
</html>

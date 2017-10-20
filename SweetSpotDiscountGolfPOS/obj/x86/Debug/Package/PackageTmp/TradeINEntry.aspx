<%@ Page Language="C#"  AutoEventWireup="true" CodeBehind="TradeINEntry.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.TradeINEntry" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>


    <div id="NewInventory">
        
            <%--Textboxes and Labels for user to enter inventory info--%>
            <h2>Trade-In Item</h2>
            <asp:Label ID="tempLocation" runat="server" Text="temp Location variable = 0"></asp:Label>


            <asp:SqlDataSource ID="sqlItemType" runat="server" ConnectionString="<%$ ConnectionStrings:SweetSpotDevConnectionString %>" SelectCommand="SELECT [typeID], [typeDescription] FROM [tbl_itemType] ORDER BY [typeDescription]"></asp:SqlDataSource>
            <asp:SqlDataSource ID="sqlBrand" runat="server" ConnectionString="<%$ ConnectionStrings:SweetSpotDevConnectionString %>" SelectCommand="SELECT [brandID], [brandName] FROM [tbl_brand] ORDER BY [brandName]"></asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlModel" runat="server" ConnectionString="<%$ ConnectionStrings:SweetSpotDevConnectionString %>" SelectCommand="SELECT [modelID], [modelName] FROM [tbl_model] ORDER BY [modelName]"></asp:SqlDataSource>
            <asp:SqlDataSource ID="sqlClubType" runat="server" ConnectionString="<%$ ConnectionStrings:SweetSpotDevConnectionString %>" SelectCommand="SELECT [typeID], [typeName] FROM [tbl_clubType] ORDER BY [typeName]"></asp:SqlDataSource>
            <br />
            <br />
            <br />

            <asp:Table ID="Table1" runat="server" Width="100%">
                <asp:TableRow>
                    <asp:TableCell Width="25%">
                        <asp:Label ID="lblSKU" runat="server" Text="SKU: "></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell Width="25%">
                        <asp:Label ID="lblSKUDisplay" runat="server" Text=""></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell Width="25%">
                        <asp:Label ID="lblCost" runat="server" Text="Cost:  $"></asp:Label>                        
                    </asp:TableCell>
                    <asp:TableCell Width="25%">
                        <asp:TextBox ID="txtCost" runat="server" Visible="true"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvCost" 
                            runat="server" ControlToValidate ="txtCost"
                            ErrorMessage="Cost Required" 
                            ForeColor="Red">
                        </asp:RequiredFieldValidator>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblBrand" runat="server" Text="Brand Name: "></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:DropDownList ID="ddlBrand" runat="server" AutoPostBack="True" DataSourceID="sqlBrand" DataTextField="brandName" DataValueField="brandID" Visible="true"></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvBrand" 
                            runat="server" ControlToValidate ="ddlBrand"
                            ErrorMessage="Brand Required" 
                            ForeColor="Red">
                        </asp:RequiredFieldValidator>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblPrice" runat="server" Text="Price:  $"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtPrice" runat="server" Visible="true"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblQuantity" runat="server" Text="Quantity: "></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtQuantity" runat="server" Visible="True"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvQuantity" 
                            runat="server" ControlToValidate ="txtQuantity"
                            ErrorMessage="Quantity Required" 
                            ForeColor="Red">
                        </asp:RequiredFieldValidator>
                    </asp:TableCell>                    
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="4"><hr /></asp:TableCell>
                </asp:TableRow>

                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblClubType" runat="server" Text="Club Type: "></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <%--<asp:TextBox ID="txtClubType" runat="server" Visible="True"></asp:TextBox>--%>
                        <asp:DropDownList ID="ddlClubType" runat="server" AutoPostBack="True" DataSourceID="sqlClubType" DataTextField="typeName" DataValueField="typeID" Visible="True"></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvClubType" 
                            runat="server" ControlToValidate ="ddlClubType"
                            ErrorMessage="Club Type Required" 
                            ForeColor="Red">
                        </asp:RequiredFieldValidator>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblModel" runat="server" Text="Model: " Visible="true"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:DropDownList ID="ddlModel" runat="server" AutoPostBack="True" DataSourceID="sqlModel" DataTextField="modelName" DataValueField="modelID" Visible="True"></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvModel" 
                            runat="server" ControlToValidate ="ddlModel"
                            ErrorMessage="Model Required" 
                            ForeColor="Red">
                        </asp:RequiredFieldValidator>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblShaft" runat="server" Text="Shaft: "></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtShaft" runat="server" Visible="True"></asp:TextBox>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblNumberofClubs" runat="server" Text="Number of Clubs: "></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtNumberofClubs" runat="server" Visible="True"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblClubSpec" runat="server" Text="Club Spec: "></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtClubSpec" runat="server" Visible="True"></asp:TextBox>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblShaftSpec" runat="server" Text="Shaft Spec: "></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtShaftSpec" runat="server" Visible="True"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblShaftFlex" runat="server" Text="ShaftFlex: "></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtShaftFlex" runat="server" Visible="True"></asp:TextBox>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblDexterity" runat="server" Text="Dexterity: "></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtDexterity" runat="server" Visible="True"></asp:TextBox>                        
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="4"><hr /></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="2">
                        <asp:Label ID="lblComments" runat="server" Text="Comments: "></asp:Label>
                    </asp:TableCell>                    
                    <asp:TableCell>
                        <asp:CheckBox ID="chkUsed" runat="server" Text="Used" Enabled="false"></asp:CheckBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="4">
                        <asp:TextBox Height="30px" Width="100%" ID="txtComments" runat="server" Visible="true"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="4"><hr /></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Button ID="btnAddItem" runat="server" Text="Add Item" OnClick="btnAddTradeIN_Click" Visible="true" CausesValidation="True" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" Visible="true" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
       
    </div>

            </div>
    </form>
</body>
</html>
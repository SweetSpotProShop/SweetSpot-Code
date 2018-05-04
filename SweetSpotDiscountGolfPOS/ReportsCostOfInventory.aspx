<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.Master" CodeBehind="ReportsCostOfInventory.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.ReportsCostOfInventory" %>

<asp:Content ID="ReportsPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <style media="print">
        .noPrint {
            display: none;
            /*margin-left: 0;*/
        }
        .yesPrint {
            display: inline-block !important;
            /* margin-right:100px;
           float: right;*/
            margin-left: 10px !important;
        }
    </style>
    <script>
        function CallPrint(strid) {
            var prtContent = document.getElementById(strid);
            var WinPrint = window.open('', '', 'letf=10,top=10,width="450",height="250",toolbar=1,scrollbars=1,status=0');
            WinPrint.document.write("<html><head><LINK rel=\"stylesheet\" type\"text/css\" href=\"css/print.css\" media=\"print\"><LINK rel=\"stylesheet\" type\"text/css\" href=\"css/print.css\" media=\"screen\"></head><body>");
            WinPrint.document.write(prtContent.innerHTML);
            WinPrint.document.write("</body></html>");
            WinPrint.document.close();
            WinPrint.focus();
            WinPrint.print();
            WinPrint.close();
            return false;
        }
    </script>
    <div id="print">
        <h2>Cost of Inventory</h2>
        <hr />
        <div>
            <asp:GridView ID="grdCostOfInventory" runat="server" AutoGenerateColumns="false" Width="60%" RowStyle-HorizontalAlign="Center">
                <Columns>
                    <asp:TemplateField HeaderText="Moose Jaw Clubs">
                        <ItemTemplate>
                            <asp:Label ID="lblcMJ" runat="server" Text='<%#Eval("cMJ", "{0:C}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Moose Jaw Accessories">
                        <ItemTemplate>
                            <asp:Label ID="lblaMJ" runat="server" Text='<%#Eval("aMJ","{0:C}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Moose Jaw Apparel">
                        <ItemTemplate>
                            <asp:Label ID="lblclMJ" runat="server" Text='<%#Eval("clMJ","{0:C}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Calgary Clubs">
                        <ItemTemplate>
                            <asp:Label ID="lblcCAL" runat="server" Text='<%#Eval("cCAL","{0:C}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Calgary Accessories">
                        <ItemTemplate>
                            <asp:Label ID="lblaCAL" runat="server" Text='<%#Eval("aCAL","{0:C}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Calgary Apparel">
                        <ItemTemplate>
                            <asp:Label ID="lblclCAL" runat="server" Text='<%#Eval("clCAL","{0:C}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Edmonton Clubs">
                        <ItemTemplate>
                            <asp:Label ID="lblcEDM" runat="server" Text='<%#Eval("cEDM","{0:C}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Edmonton Accessories">
                        <ItemTemplate>
                            <asp:Label ID="lblaEDM" runat="server" Text='<%#Eval("aEDM","{0:C}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Edmonton Apparel">
                        <ItemTemplate>
                            <asp:Label ID="lblclEDM" runat="server" Text='<%#Eval("clEDM","{0:C}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <hr />
    </div>
    <asp:Table runat="server">
        <asp:TableRow>
            <asp:TableCell>
                <asp:Button class="noPrint" ID="btnPrint" runat="server" Text="Print Report" Width="200px" OnClientClick="CallPrint('print');" />
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Content>




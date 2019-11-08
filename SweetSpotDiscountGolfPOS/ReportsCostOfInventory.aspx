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
                    <asp:TemplateField HeaderText="Store">
                        <ItemTemplate>
                            <asp:Label ID="lblLocationName" runat="server" Text='<%#Eval("varLocationName")%>' />
                        </ItemTemplate>
						<FooterTemplate>
							<asp:Label ID="lblTotal" runat="server" Text="Totals:" />
						</FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Accessories Cost">
                        <ItemTemplate>
                            <asp:Label ID="lblAccessoriesCost" runat="server" Text='<%#Eval("fltAccessoriesCost","{0:C}") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Clothing Cost">
                        <ItemTemplate>
                            <asp:Label ID="lblClothingCost" runat="server" Text='<%#Eval("fltClothingCost","{0:C}") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Club Cost">
                        <ItemTemplate>
                            <asp:Label ID="lblClubCost" runat="server" Text='<%#Eval("fltClubsCost","{0:C}") %>' />
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




﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReportsSales.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.ReportsSales" %>

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
        <h2>Sales Report By Date</h2>
        <hr />
        <div>
            <asp:Label ID="lblDates" runat="server" Font-Bold="true" Text="lblDates"></asp:Label>
        </div>
        <hr />
        <div>
            <asp:GridView ID="grdSalesByDate" runat="server" AutoGenerateColumns="false" Width="60%" RowStyle-HorizontalAlign="Center" ShowFooter="true" OnRowDataBound="grdSalesByDate_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="Date">
                        <ItemTemplate>
                            <asp:Label ID="lblDate" runat="server" Text='<%#Eval("invoiceDate","{0:d}")%>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblTotal" runat="server" Text="Totals:"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Sales Dollars">
                        <ItemTemplate>
                            <asp:Label ID="lblSalesDollars" runat="server" Text='<%#Eval("totalSales","{0:C}") %>'></asp:Label>
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

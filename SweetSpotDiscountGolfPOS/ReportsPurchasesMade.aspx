﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReportsPurchasesMade.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.ReportsPurchasesMade" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SPMaster" runat="server">
</asp:Content>--%>

<asp:Content ID="ReportsPurchasesMadePageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
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
    <link href="MainStyleSheet.css" rel="stylesheet" type="text/css" />
    <div id="Purchases" class="yesPrint">
        <h2>Purchases</h2>
        <hr />
        <%--Purchases Breakdown--%>
        <asp:Label ID="lblPurchasesMadeDate" Font-Bold="true" runat="server" />
        <hr />
        <div>
            <asp:GridView ID="grdPurchasesMade" runat="server" Width="75%" AutoGenerateColumns="false" ShowFooter="true" OnRowDataBound="grdPurchasesMade_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="Receipt Number" HeaderStyle-Width ="20%">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtnReceiptNumber" runat="server" Text='<%#Eval("receiptNumber") %>' OnClick="lbtnReceiptNumber_Click"></asp:LinkButton>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblTotals" runat="server" Text="Totals:"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Receipt Date" HeaderStyle-Width="20%" DataField="receiptDate" DataFormatString="{0:d}"/>
                    <asp:BoundField HeaderText="Purchase Method" HeaderStyle-Width="20%" DataField="mopDescription" />
                    <asp:BoundField HeaderText="Cheque Number" HeaderStyle-Width="20%" DataField="chequeNumber" />
                    <asp:BoundField HeaderText="Purchase Amount" HeaderStyle-Width="20%" DataField="amountPaid" DataFormatString="{0:C}"/>
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <hr />
    </div>
    <asp:Button class="noPrint" ID="btnPrint" runat="server" Text="Print Report" Width="200px" OnClientClick="printReport()" />
    <script>
        function printReport(printable) {
            window.print();
        }
    </script>
</asp:Content>

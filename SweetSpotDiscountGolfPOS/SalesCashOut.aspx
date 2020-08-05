<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="SalesCashOut.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.SalesCashOut" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SPMaster" runat="server">
</asp:Content>--%>

<asp:Content ID="SalesCashOutPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
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
    <link href="MainStyleSheet.css" rel="stylesheet" type="text/css" />
    <div id="CashOut" class="yesPrint">
        <div id="print">
            <h2>Cashout</h2>
            <hr />
            <%--Payment Breakdown--%>

            <div class="CashoutTable">
                <asp:Label ID="lblCashoutDate" Font-Bold="true" runat="server" />
                <hr />
                <h3>Balancing</h3>
                <br />
                <asp:Table ID="tblCashout" runat="server" GridLines="Both" CssClass="CashoutTable" Width="100%">
                    <asp:TableRow Width="100%">
                        <asp:TableCell ColumnSpan="11" Width="100%">
                            <asp:Label runat="server" ID="lblSales" Text="Sales" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow Width="100%">
                        <asp:TableCell Width="9%">
                            <asp:Label runat="server" ID="lblTradeInS" Text="Trade-In" Width="100%" />
                        </asp:TableCell>
                        <asp:TableCell Width="9%">
                            <asp:Label runat="server" ID="lblGiftCardS" Text="Gift Card" Width="100%" />
                        </asp:TableCell>
                        <asp:TableCell Width="9%">
                            <asp:Label runat="server" ID="lblCashS" Text="Cash" Width="100%" />
                        </asp:TableCell>
                        <asp:TableCell Width="9%">
                            <asp:Label runat="server" ID="lblDebitS" Text="Debit" Width="100%" />
                        </asp:TableCell>
                        <asp:TableCell Width="9%">
                            <asp:Label runat="server" ID="lblMasterCardS" Text="MasterCard" Width="100%" />
                        </asp:TableCell>
                        <asp:TableCell Width="9%">
                            <asp:Label runat="server" ID="lblVisaS" Text="Visa" Width="100%" />
                        </asp:TableCell>
                        <asp:TableCell Width="9%">
                            <asp:Label runat="server" ID="lblPreTaxS" Text="Pre Tax" Width="100%" />
                        </asp:TableCell>
                        <asp:TableCell Width="9%">
                            <asp:Label runat="server" ID="lblGSTS" Text="GST" Width="100%" />
                        </asp:TableCell>
                        <asp:TableCell Width="9%">
                            <asp:Label runat="server" ID="lblPSTS" Text="PST" Width="100%" />
                        </asp:TableCell>
						<asp:TableCell Width="9%">
                            <asp:Label runat="server" ID="LblLCTS" Text="LCT" Width="100%" />
                        </asp:TableCell>
                        <asp:TableCell Width="9%">
                            <asp:Label runat="server" ID="lblTotalS" Text="Total" Width="100%" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblTradeInDisplay" Text="" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblGiftCardDisplay" Text="" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblCashDisplay" Text="" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblDebitDisplay" Text="" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblMasterCardDisplay" Text="" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblVisaDisplay" Text="" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblPreTaxDisplay" Text="" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblGSTDisplay" Text="" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblPSTDisplay" Text="" />
                        </asp:TableCell>
						<asp:TableCell>
                            <asp:Label runat="server" ID="lblLCTDisplay" Text="" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblTotalDisplay" Text="" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell ColumnSpan="6">
                            <asp:Label runat="server" ID="lblReceipts" Text="Recipts" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblTradeInR" Text="Trade-In" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblGiftCardR" Text="Gift Card" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblCashR" Text="Cash" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblDebitR" Text="Debit" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblMasterCardR" Text="MasterCard" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblVisaR" Text="Visa" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:TextBox ID="txtTradeIn" runat="server" AutoCompleteType="Disabled" Text="0.00" Width="60" />
<%--                            <asp:RegularExpressionValidator ID="revTradeIn"
                                ControlToValidate="txtTradeIn"
                                ValidationExpression="[-+]?([0-9]*\.[0-9]+|[0-9]+)"
                                Display="Static"
                                EnableClientScript="true"
                                ErrorMessage="numbers only"
                                runat="server" />--%>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtGiftCard" runat="server" AutoCompleteType="Disabled" Text="0.00" Width="60" />
<%--                            <asp:RegularExpressionValidator ID="revGiftCard"
                                ControlToValidate="txtGiftCard"
                                ValidationExpression="[-+]?([0-9]*\.[0-9]+|[0-9]+)"
                                Display="Static"
                                EnableClientScript="true"
                                ErrorMessage="numbers only"
                                runat="server" />--%>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtCash" runat="server" AutoCompleteType="Disabled" Text="0.00" Width="60" />
                            <%--<asp:RegularExpressionValidator ID="revCash"
                                ControlToValidate="txtCash"
                                ValidationExpression="[-+]?([0-9]*\.[0-9]+|[0-9]+)"
                                Display="Static"
                                EnableClientScript="true"
                                ErrorMessage="numbers only"
                                runat="server" />--%>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtDebit" runat="server" AutoCompleteType="Disabled" Text="0.00" Width="60" />
                            <%--<asp:RegularExpressionValidator ID="revDebit"
                                ControlToValidate="txtDebit"
                                ValidationExpression="[-+]?([0-9]*\.[0-9]+|[0-9]+)"
                                Display="Static"
                                EnableClientScript="true"
                                ErrorMessage="numbers only"
                                runat="server" />--%>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtMasterCard" runat="server" AutoCompleteType="Disabled" Text="0.00" Width="60" />
                            <%--<asp:RegularExpressionValidator ID="revMasterCard"
                                ControlToValidate="txtMasterCard"
                                ValidationExpression="[-+]?([0-9]*\.[0-9]+|[0-9]+)"
                                Display="Static"
                                EnableClientScript="true"
                                ErrorMessage="numbers only"
                                runat="server" />--%>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtVisa" runat="server" AutoCompleteType="Disabled" Text="0.00" Width="60" />
<%--                            <asp:RegularExpressionValidator ID="revVisa"
                                ControlToValidate="txtVisa"
                                ValidationExpression="[-+]?([0-9]*\.[0-9]+|[0-9]+)"
                                Display="Static"
                                EnableClientScript="true"
                                ErrorMessage="numbers only"
                                runat="server" />--%>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Button ID="BtnCalculate" runat="server" Text="Calculate" OnClick="BtnCalculate_Click" CausesValidation="True"/>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Button ID="BtnClear" runat="server" Text="Clear" OnClick="BtnClear_Click" />
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <br />
            <hr />
            <div class="yesPrint" id="summary_header">
                <h3>Summary</h3>
            </div>
            <div class="yesPrint" id="summary">
                <asp:Table ID="tblSumm" runat="server" GridLines="none" CellSpacing="10">
                    <asp:TableRow>
                        <asp:TableCell Text="Receipts:">
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="lblReceiptsFinal" CssClass="Underline" runat="server" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell Text="Less Total Sales:">
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="lblTotalFinal" CssClass="Underline" runat="server" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="lblOverShort" runat="server" Text="Over(Short):" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="lblOverShortFinal" CssClass="Underline2" runat="server" />
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <br />
                <hr />
            </div>
            <asp:Button CssClass="noPrint" ID="BtnProcessReport" runat="server" Text="Process Cashout" Width="200px" OnClick="BtnProcessReport_Click" />
            <asp:Button CssClass="noPrint" ID="btnPrint" runat="server" Text="Print Report" Width="200px" Enabled="false" OnClientClick="CallPrint('print');" />
        </div>
    </div>
</asp:Content>
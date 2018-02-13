<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReportsCashOut.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.ReportsCashOut" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SPMaster" runat="server">
</asp:Content>--%>

<asp:Content ID="ReportsCashOutPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <script>
        function printReport(printable) {
            window.print();
        }
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
                <asp:Label ID="lblCashoutDate" Font-Bold="true" runat="server"></asp:Label>
                <hr />
                <h3>Balancing</h3>
                <br />
                <asp:Table ID="tblCashout" runat="server" GridLines="Both" CssClass="CashoutTable">
                    <asp:TableRow>
                        <asp:TableCell ColumnSpan="9">
                            <asp:Label runat="server" ID="lblSales" Text="Sales" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblTradeInS" Text="Trade-In" Width="80" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblGiftCardS" Text="Gift Card" Width="80" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblCashS" Text="Cash" Width="80" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblDebitS" Text="Debit" Width="80" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblMasterCardS" Text="MasterCard" Width="80" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblVisaS" Text="Visa" Width="80" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblPreTaxS" Text="Pre Tax" Width="80" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblGSTS" Text="GST" Width="80" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblPSTS" Text="PST" Width="80" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblTotalS" Text="Total" Width="80" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblTradeInDisplay" Text="" Width="80" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblGiftCardDisplay" Text="" Width="80" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblCashDisplay" Text="" Width="80" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblDebitDisplay" Text="" Width="80" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblMasterCardDisplay" Text="" Width="80" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblVisaDisplay" Text="" Width="80" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblPreTaxDisplay" Text="" Width="80" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblGSTDisplay" Text="" Width="80" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblPSTDisplay" Text="" Width="80" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblTotalDisplay" Text="" Width="80" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell ColumnSpan="6">
                            <asp:Label runat="server" ID="lblReceipts" Text="Recipts" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblTradeInR" Text="Trade-In" Width="80" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblGiftCardR" Text="Gift Card" Width="80" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblCashR" Text="Cash" Width="80" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblDebitR" Text="Debit" Width="80" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblMasterCardR" Text="MasterCard" Width="80" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblVisaR" Text="Visa" Width="80" />
                        </asp:TableCell>

                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:TextBox ID="txtTradeIn" runat="server" Width="80"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="revTradeIn"
                                ControlToValidate="txtTradeIn"
                                ValidationExpression="[-+]?([0-9]*\.[0-9]+|[0-9]+)"
                                Display="Static"
                                EnableClientScript="true"
                                ErrorMessage="Requires a number"
                                runat="server" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtGiftCard" runat="server" Width="80"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="revGiftCard"
                                ControlToValidate="txtGiftCard"
                                ValidationExpression="[-+]?([0-9]*\.[0-9]+|[0-9]+)"
                                Display="Static"
                                EnableClientScript="true"
                                ErrorMessage="Requires a number"
                                runat="server" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtCash" runat="server" Width="80"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="revCash"
                                ControlToValidate="txtCash"
                                ValidationExpression="[-+]?([0-9]*\.[0-9]+|[0-9]+)"
                                Display="Static"
                                EnableClientScript="true"
                                ErrorMessage="Requires a number"
                                runat="server" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtDebit" runat="server" Width="80"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="revDebit"
                                ControlToValidate="txtDebit"
                                ValidationExpression="[-+]?([0-9]*\.[0-9]+|[0-9]+)"
                                Display="Static"
                                EnableClientScript="true"
                                ErrorMessage="Requires a number"
                                runat="server" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtMasterCard" runat="server" Width="80"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="revMasterCard"
                                ControlToValidate="txtMasterCard"
                                ValidationExpression="[-+]?([0-9]*\.[0-9]+|[0-9]+)"
                                Display="Static"
                                EnableClientScript="true"
                                ErrorMessage="Requires a number"
                                runat="server" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtVisa" runat="server" Width="80"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="revVisa"
                                ControlToValidate="txtVisa"
                                ValidationExpression="[-+]?([0-9]*\.[0-9]+|[0-9]+)"
                                Display="Static"
                                EnableClientScript="true"
                                ErrorMessage="Requires a number"
                                runat="server" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Button ID="btnCalculate" runat="server" Text="Calculate" Width="100px" OnClick="btnCalculate_Click" CausesValidation="True"/>
                        </asp:TableCell>
                        <asp:TableCell ColumnSpan="5">
                            <asp:Button ID="btnClear" runat="server" Width="90px" Text="Clear" OnClick="btnClear_Click" />
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
                        <asp:TableCell Text="Receipts:"></asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="lblReceiptsFinal" CssClass="Underline" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell Text="Less Total Sales:"></asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="lblTotalFinal" CssClass="Underline" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="lblOverShort" runat="server" Text="Over(Short):"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="lblOverShortFinal" CssClass="Underline2" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <br />
                <hr />
            </div>
            <asp:Button class="noPrint" ID="btnProcessReport" runat="server" Text="Process Cashout" Width="200px" OnClick="btnProcessReport_Click" />
            <asp:Button class="noPrint" ID="btnPrint" runat="server" Text="Print Report" Width="200px" Enabled="false" OnClientClick="CallPrint('print');" />
        </div>
    </div>
</asp:Content>

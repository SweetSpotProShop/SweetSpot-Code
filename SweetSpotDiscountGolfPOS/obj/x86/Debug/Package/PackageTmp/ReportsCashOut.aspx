<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReportsCashOut.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.ReportsCashOut" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SPMaster" runat="server">
</asp:Content>--%>

<asp:Content ID="ReportsCashOutPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <link href="MainStyleSheet.css" rel="stylesheet" type="text/css" />
    <div id="CashOut">
        <h2>Cash Out</h2>
        <hr />
        <%--Payment Breakdown--%>       

       <div class="CashoutTable">
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
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtGiftCard" runat="server" Width="80"></asp:TextBox>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtCash" runat="server" Width="80"></asp:TextBox>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtDebit" runat="server" Width="80"></asp:TextBox>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtMasterCard" runat="server" Width="80"></asp:TextBox>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtVisa" runat="server" Width="80"></asp:TextBox>
                    </asp:TableCell>                  
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Button ID="btnCalculate" runat="server" Text="Calculate" Width="100px" OnClick="btnCalculate_Click" />
                    </asp:TableCell>
                    <asp:TableCell ColumnSpan="5">
                        <asp:Button ID="btnClear" runat="server" Width="90px" Text="Clear" OnClick="btnClear_Click" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>

        <div class="yesPrint" id="summary_header">
            <h2>Cash Out Summary</h2>
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
            <asp:Button class="noPrint" ID="btnProcessReport" runat="server" Text="Process and Print Cashout" Width="200px" OnClientClick="return validatePost()" OnClick="btnProcessReport_Click" /><asp:Button class="noPrint" ID="btnPrint" runat="server" Text="Print Report" Width="200px" OnClientClick="printReport()" />
        </div>
    </div>
</asp:Content>

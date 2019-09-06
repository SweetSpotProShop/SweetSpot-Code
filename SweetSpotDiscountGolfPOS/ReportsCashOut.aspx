<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReportsCashOut.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.ReportsCashOut" %>

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
        <h2>Cashouts Listed By Date</h2>
        <hr />
        <div>
            <asp:Label ID="lblDates" runat="server" Font-Bold="true" />
        </div>
        <hr />
        <div>
            <asp:GridView ID="grdCashoutByDate" runat="server" AutoGenerateColumns="false" Width="100%" RowStyle-HorizontalAlign="Center" 
                OnRowCommand="grdCashoutByDate_RowCommand" OnRowDataBound="grdCashoutByDate_RowDataBound" >
                <Columns>
                    <asp:TemplateField HeaderText="Date">
                        <ItemTemplate>
                            <asp:Label ID="lblDate" runat="server" Text='<%#Eval("dtmCashoutDate", "{0: dd/MMM/yy}")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Trade In<br />
                            Receipts -<br />
                            Sales
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblTradeInReceipts" runat="server" Text='<%#Eval("fltManuallyCountedBasedOnReceiptsTradeIn","{0:C}") %>' />
                            <br />
                            <asp:Label ID="lblTradeInSales" runat="server" Text='<%#Eval("fltSystemCountedBasedOnSystemTradeIn","{0:C}") %>' />
                            <br />
                            <asp:Label ID="lblTradeInBalance" runat="server" ForeColor="Green" Text='<%# Convert.ToDouble(Eval("fltManuallyCountedBasedOnReceiptsTradeIn")) == Convert.ToDouble(Eval("fltSystemCountedBasedOnSystemTradeIn")) ? "Balanced" : "Discrepancy" %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Gift Card<br />
                            Receipts -<br />
                            Sales
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblGiftCardReceipts" runat="server" Text='<%#Eval("fltManuallyCountedBasedOnReceiptsGiftCard","{0:C}") %>' />
                            <br />
                            <asp:Label ID="lblGiftCardSales" runat="server" Text='<%#Eval("fltSystemCountedBasedOnSystemGiftCard","{0:C}") %>' />
                            <br />
                            <asp:Label ID="lblGiftCardBalance" runat="server" ForeColor="Green" Text='<%# Convert.ToDouble(Eval("fltManuallyCountedBasedOnReceiptsGiftCard")) == Convert.ToDouble(Eval("fltSystemCountedBasedOnSystemGiftCard")) ? "Balanced" : "Discrepancy" %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Cash<br />
                            Receipts -<br />
                            Sales
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblCashReceipts" runat="server" Text='<%#Eval("fltManuallyCountedBasedOnReceiptsCash","{0:C}") %>' />
                            <br />
                            <asp:Label ID="lblCashSales" runat="server" Text='<%#Eval("fltSystemCountedBasedOnSystemCash","{0:C}") %>' />
                            <br />
                            <asp:Label ID="lblCashBalance" runat="server" ForeColor="Green" Text='<%# Convert.ToDouble(Eval("fltManuallyCountedBasedOnReceiptsCash")) == Convert.ToDouble(Eval("fltSystemCountedBasedOnSystemCash")) ? "Balanced" : "Discrepancy" %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Debit<br />
                            Receipts -<br />
                            Sales
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblDebitReceipts" runat="server" Text='<%#Eval("fltManuallyCountedBasedOnReceiptsDebit","{0:C}") %>' />
                            <br />
                            <asp:Label ID="lblDebitSales" runat="server" Text='<%#Eval("fltSystemCountedBasedOnSystemDebit","{0:C}") %>' />
                            <br />
                            <asp:Label ID="lblDebitBalance" runat="server" ForeColor="Green" Text='<%# Convert.ToDouble(Eval("fltManuallyCountedBasedOnReceiptsDebit")) == Convert.ToDouble(Eval("fltSystemCountedBasedOnSystemDebit")) ? "Balanced" : "Discrepancy" %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            MasterCard<br />
                            Receipts -<br />
                            Sales
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblMasterCardReceipts" runat="server" Text='<%#Eval("fltManuallyCountedBasedOnReceiptsMasterCard","{0:C}") %>' />
                            <br />
                            <asp:Label ID="lblMasterCardSales" runat="server" Text='<%#Eval("fltSystemCountedBasedOnSystemMasterCard","{0:C}") %>' />
                            <br />
                            <asp:Label ID="lblMasterCardBalance" runat="server" ForeColor="Green" Text='<%# Convert.ToDouble(Eval("fltManuallyCountedBasedOnReceiptsMasterCard")) == Convert.ToDouble(Eval("fltSystemCountedBasedOnSystemMasterCard")) ? "Balanced" : "Discrepancy" %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Visa<br />
                            Receipts -<br />
                            Sales
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblVisaReceipts" runat="server" Text='<%#Eval("fltManuallyCountedBasedOnReceiptsVisa","{0:C}") %>' />
                            <br />
                            <asp:Label ID="lblVisaSales" runat="server" Text='<%#Eval("fltSystemCountedBasedOnSystemVisa","{0:C}") %>' />
                            <br />
                            <asp:Label ID="lblVisaBalance" runat="server" ForeColor="Green" Text='<%# Convert.ToDouble(Eval("fltManuallyCountedBasedOnReceiptsVisa")) == Convert.ToDouble(Eval("fltSystemCountedBasedOnSystemVisa")) ? "Balanced" : "Discrepancy" %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Over/Short">
                        <ItemTemplate>
                            <asp:Label ID="lblOverShort" runat="server" Text='<%#Eval("fltCashDrawerOverShort","{0:C}")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Processed">
                        <ItemTemplate>
                            <asp:Label ID="lblProcessed" runat="server" Text='<%# Convert.ToDouble(Eval("bitIsCashoutProcessed")) == 1 ? "TRUE" : "FALSE" %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Finalized">
                        <ItemTemplate>
                            <asp:Label ID="lblFinalized" runat="server" Text='<%# Convert.ToDouble(Eval("bitIsCashoutFinalized")) == 1 ? "TRUE" : "FALSE" %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:Button ID="btnEdit" runat="server" Text="Edit" CausesValidation="false" CommandName="EditCashout" CommandArgument='<%#Eval("dtmCashoutDate","{0:d}") + " " + Eval("intLocationID")%>' />
                            <asp:Button ID="btnFinalize" runat="server" Text="Finalize" CausesValidation="false" CommandName="FinalizeCashout" CommandArgument='<%#Eval("dtmCashoutDate","{0:d}") + " " + Eval("intLocationID")%>' />
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
            <asp:TableCell>
                <asp:Button class="noPrint" ID="btnDownload" runat="server" Text="Download" Visible="true" Width="200px" OnClick="btnDownload_Click" />
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Content>


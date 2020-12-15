<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReportsTaxes.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.ReportsTaxes" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SPMaster" runat="server">
</asp:Content>--%>

<asp:Content ID="ReportsTaxesPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
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
	<link href="MainStyleSheet.css" rel="stylesheet" type="text/css" />
	<div id="Taxes" class="yesPrint">
		<h2>Taxes</h2>
		<hr />
		<%--Taxes Breakdown--%>
		<asp:Label ID="lblTaxDate" Font-Bold="true" runat="server" />
		<hr />

		<div>
			<asp:GridView ID="GrdTaxList" runat="server" Width="100%" AutoGenerateColumns="false" ShowFooter="true" OnRowDataBound="GrdTaxList_RowDataBound"
				FooterStyle-Font-Bold="true">
				<Columns>
					<asp:BoundField HeaderText="Date" HeaderStyle-Width="10%" DataField="dtmInvoiceDate" FooterText="Totals:" DataFormatString="{0:d}" />
					<asp:BoundField HeaderText="GST Collected" HeaderStyle-Width="10%" DataField="fltGovernmentTaxAmountCollected" DataFormatString="{0:C}" />
					<asp:BoundField HeaderText="HST Collected" HeaderStyle-Width="10%" DataField="fltHarmonizedTaxAmountCollected" DataFormatString="{0:C}" />
					<asp:BoundField HeaderText="LCT Collected" HeaderStyle-Width="10%" DataField="fltLiquorTaxAmountCollected" DataFormatString="{0:C}" />
					<asp:BoundField HeaderText="PST Collected" HeaderStyle-Width="10%" DataField="fltProvincialTaxAmountCollected" DataFormatString="{0:C}" />
					<asp:BoundField HeaderText="QST Collected" HeaderStyle-Width="10%" DataField="fltQuebecTaxAmountCollected" DataFormatString="{0:C}" />
					<asp:BoundField HeaderText="RST Collected" HeaderStyle-Width="10%" DataField="fltRetailTaxAmountCollected" DataFormatString="{0:C}" />
					<asp:BoundField HeaderText="GST Returned" HeaderStyle-Width="10%" DataField="fltGovernmentTaxAmountReturned" DataFormatString="{0:C}" />
					<asp:BoundField HeaderText="HST Returned" HeaderStyle-Width="10%" DataField="fltHarmonizedTaxAmountReturned" DataFormatString="{0:C}" />
					<asp:BoundField HeaderText="LCT Returned" HeaderStyle-Width="10%" DataField="fltLiquorTaxAmountReturned" DataFormatString="{0:C}" />
					<asp:BoundField HeaderText="PST Returned" HeaderStyle-Width="10%" DataField="fltProvincialTaxAmountReturned" DataFormatString="{0:C}" />
					<asp:BoundField HeaderText="QST Returned" HeaderStyle-Width="10%" DataField="fltQuebecTaxAmountReturned" DataFormatString="{0:C}" />
					<asp:BoundField HeaderText="RST Returned" HeaderStyle-Width="10%" DataField="fltRetailTaxAmountReturned" DataFormatString="{0:C}" />
					<asp:TemplateField HeaderText="GST Total" HeaderStyle-Width="10%">
						<ItemTemplate>
							<asp:Label ID="lblGovernmentTaxAmountTotaled" runat="server" Text='<%# (Convert.ToDouble(Eval("fltGovernmentTaxAmountCollected")) + Convert.ToDouble(Eval("fltGovernmentTaxAmountReturned"))).ToString("C") %>' />
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="HST Total" HeaderStyle-Width="10%">
						<ItemTemplate>
							<asp:Label ID="lblHarmonizedTaxAmountTotaled" runat="server" Text='<%# (Convert.ToDouble(Eval("fltHarmonizedTaxAmountCollected")) + Convert.ToDouble(Eval("fltHarmonizedTaxAmountReturned"))).ToString("C") %>' />
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="LCT Total" HeaderStyle-Width="10%">
						<ItemTemplate>
							<asp:Label ID="lblLiquorTaxAmountTotaled" runat="server" Text='<%# (Convert.ToDouble(Eval("fltLiquorTaxAmountCollected")) + Convert.ToDouble(Eval("fltLiquorTaxAmountReturned"))).ToString("C") %>' />
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="PST Total" HeaderStyle-Width="10%">
						<ItemTemplate>
							<asp:Label ID="lblProvincialTaxAmountTotaled" runat="server" Text='<%# (Convert.ToDouble(Eval("fltProvincialTaxAmountCollected")) + Convert.ToDouble(Eval("fltProvincialTaxAmountReturned"))).ToString("C") %>' />
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="QST Total" HeaderStyle-Width="10%">
						<ItemTemplate>
							<asp:Label ID="lblQuebecTaxAmountTotaled" runat="server" Text='<%# (Convert.ToDouble(Eval("fltQuebecTaxAmountCollected")) + Convert.ToDouble(Eval("fltQuebecTaxAmountReturned"))).ToString("C") %>' />
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="RST Total" HeaderStyle-Width="10%">
						<ItemTemplate>
							<asp:Label ID="lblRetailTaxAmountTotaled" runat="server" Text='<%# (Convert.ToDouble(Eval("fltRetailTaxAmountCollected")) + Convert.ToDouble(Eval("fltRetailTaxAmountReturned"))).ToString("C") %>' />
						</ItemTemplate>
					</asp:TemplateField>
				</Columns>
			</asp:GridView>
		</div>
		<br />
		<hr />
	</div>
	<asp:Button CssClass="noPrint" ID="btnPrint" runat="server" Text="Print Report" Width="200px" OnClientClick="CallPrint('Taxes');" />
	<asp:Button CssClass="noPrint" ID="BtnDownload" runat="server" Text="Download" Visible="true" Width="200px" OnClick="BtnDownload_Click" />
	<script>
		function printReport(printable) {
			window.print();
		}
	</script>
</asp:Content>

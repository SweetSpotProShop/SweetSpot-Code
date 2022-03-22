<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReportsInventoryChange.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.ReportsInventoryChange" %>

<asp:Content ID="ReportsPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
	<div id="print">
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

		<h2>Inventory Change Report</h2>
		<hr />
		<div>
			<asp:Label ID="lblDates" runat="server" Font-Bold="true" />
		</div>
		<div style="width: 90%;">
			<asp:Table ID="tblHeaders" runat="server" Width="100%">
				<asp:TableRow>
					<asp:TableCell BorderStyle="Groove" Width="17%" HorizontalAlign="Center">
						<asp:Label ID="lblChangeDate" runat="server" Text="Date / Time" />
					</asp:TableCell>
					<asp:TableCell BorderStyle="Groove" Width="17%" HorizontalAlign="Center">
						<asp:Label ID="lblEmployee" runat="server" Text="Employee / Location" />
					</asp:TableCell>
					<asp:TableCell BorderStyle="Groove" Width="15%" HorizontalAlign="Center">
						<asp:Label ID="lblSKU" runat="server" Text="SKU" />
					</asp:TableCell>
					<asp:TableCell BorderStyle="Groove" Width="17%" HorizontalAlign="Center">
						<asp:Label ID="lblCost" runat="server" Text="Cost" />
					</asp:TableCell>
					<asp:TableCell BorderStyle="Groove" Width="17%" HorizontalAlign="Center">
						<asp:Label ID="lblPrice" runat="server" Text="Price" />
					</asp:TableCell>
					<asp:TableCell BorderStyle="Groove" Width="17%" HorizontalAlign="Center">
						<asp:Label ID="lblQuantity" runat="server" Text="Quantity" />
					</asp:TableCell>
				</asp:TableRow>
			</asp:Table>
		</div>
		<div style="width: 90%; height: 480px; overflow: auto;">
			<asp:GridView ID="grdStats" runat="server" AutoGenerateColumns="false" ShowHeader="false" RowStyle-HorizontalAlign="Center" Width="100%">
				<Columns>
					<asp:TemplateField ControlStyle-BorderStyle="Groove">
						<ItemTemplate>
							<asp:Table ID="tblChangeItem" runat="server">
								<asp:TableRow>
									<asp:TableCell Width="17%" HorizontalAlign="Center">
										<asp:Label ID="lblGrdChangeDate" runat="server" Text='<%#Eval("dtmChangeDate", "{0: dd/MMM/yy}")%>' />
									</asp:TableCell>
									<asp:TableCell Width="17%" HorizontalAlign="Center">
										<asp:Label ID="lblGrdEmployee" runat="server" Text='<%#Eval("employeeName")%>' />
									</asp:TableCell>
									<asp:TableCell Width="15%" RowSpan="2" HorizontalAlign="Center">
										<asp:Label ID="lblGrdSKU" runat="server" Text='<%#Eval("varSku")%>' />
									</asp:TableCell>
									<asp:TableCell Width="17%" HorizontalAlign="Center">
										<asp:Label ID="lblOriginalC" runat="server" Text="Original: " />
										<asp:Label ID="lblOriginalCost" runat="server" Text='<%#Eval("fltOriginalCost", "{0:C}")%>' />
									</asp:TableCell>
									<asp:TableCell Width="17%" HorizontalAlign="Center">
										<asp:Label ID="lblOriginalP" runat="server" Text="Original: " />
										<asp:Label ID="lblOriginalPrice" runat="server" Text='<%#Eval("fltOriginalPrice", "{0:C}")%>' />
									</asp:TableCell>
									<asp:TableCell Width="17%" HorizontalAlign="Center">
										<asp:Label ID="lblOriginalQ" runat="server" Text="Original: " />
										<asp:Label ID="lblOriginalQuantity" runat="server" Text='<%#Eval("intOriginalQuantity")%>' />
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow>
									<asp:TableCell HorizontalAlign="Center">
										<asp:Label ID="lblGrdChangeTime" runat="server" Text='<%# DateTime.Parse(Eval("dtmChangeTime").ToString()).ToShortTimeString()%>' />
									</asp:TableCell>
									<asp:TableCell HorizontalAlign="Center">
										<asp:Label ID="lblGrdLocation" runat="server" Text='<%#Eval("varCityName")%>' />
									</asp:TableCell>
									<asp:TableCell HorizontalAlign="Center">
										<asp:Label ID="lblNewC" runat="server" Text="New: " />
										<asp:Label ID="lblNewCost" runat="server" Text='<%#Eval("fltNewCost", "{0:C}")%>' />
									</asp:TableCell>
									<asp:TableCell HorizontalAlign="Center">
										<asp:Label ID="lblNewP" runat="server" Text="New: " />
										<asp:Label ID="lblNewPrice" runat="server" Text='<%#Eval("fltNewPrice", "{0:C}")%>' />
									</asp:TableCell>
									<asp:TableCell HorizontalAlign="Center">
										<asp:Label ID="lblNewQ" runat="server" Text="New: " />
										<asp:Label ID="lblNewQuantity" runat="server" Text='<%#Eval("intNewQuantity")%>' />
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow>
									<asp:TableCell ColumnSpan="6">
										<asp:Label ID="lblOriginalD" runat="server" Text="Original Description: " />
										<asp:Label ID="lblOriginalDescription" runat="server" BorderWidth="1" Text='<%#Eval("varOriginalDescription")%>' />
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow>
									<asp:TableCell ColumnSpan="6">
										<asp:Label ID="lblNewD" runat="server" Text="New Description: " />
										<asp:Label ID="lblNewDescription" runat="server" BorderWidth="1" Text='<%#Eval("varNewDescription")%>' />
									</asp:TableCell>
								</asp:TableRow>
							</asp:Table>
						</ItemTemplate>
					</asp:TemplateField>
				</Columns>
			</asp:GridView>
		</div>
		<hr />
	</div>
	<asp:Table runat="server">
		<asp:TableRow>
			<asp:TableCell>
				<asp:Button CssClass="noPrint" ID="btnPrint" runat="server" Text="Print Report" Width="200px" OnClientClick="CallPrint('print');" />
			</asp:TableCell>
			<asp:TableCell>
				<asp:Button CssClass="noPrint" ID="BtnDownload" runat="server" Text="Download" Visible="true" Width="200px" OnClick="BtnDownload_Click" />
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
</asp:Content>

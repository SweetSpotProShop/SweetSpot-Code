<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="LoungeSalesCart.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.LoungeSalesCart" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>--%>

<asp:Content ID="NonActive" ContentPlaceHolderID="SPMaster" runat="server">
	<div id="menu_simple">
		<ul>
			<li><a>HOME</a></li>
			<li><a>CUSTOMERS</a></li>
			<li><a>SALES</a></li>
			<li><a>INVENTORY</a></li>
			<li><a>REPORTS</a></li>
			<li><a>SETTINGS</a></li>
		</ul>
	</div>
	<div id="image_simple">
		<img src="Images/SweetSpotLogo.jpg" />
	</div>
	<link rel="stylesheet" type="text/css" href="CSS/MainStyleSheet.css" />
</asp:Content>
<asp:Content ID="LoungeSalesCartPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
	<div id="Cart">
		<asp:Table ID="tblLoungeServicesAndReceipt" runat="server" Width="100%">
			<asp:TableRow>
				<asp:TableCell Width="40%">
					<asp:MultiView ID="mvApplicableServices" runat="server">
						<asp:View ID="viewMainServices" runat="server">
							<asp:Table ID="tblMainServices" runat="server" Width="100%" Height="100%">
								<asp:TableRow Width="100%" Height="20%">
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="BtnItemSelectionPageOne" CssClass="wrap" Text="Program Button" runat="server" Visible="true" OnClick="BtnItemSelectionPageOne_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="BtnItemSelectionPageTwo" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnItemSelectionPageTwo_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="BtnItemSelectionPageThree" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnItemSelectionPageThree_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="BtnItemSelectionPageFour" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnItemSelectionPageFour_Click" />
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow Width="100%" Height="20%">
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="BtnItemSelectionPageFive" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnItemSelectionPageFive_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="BtnItemSelectionPageSix" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnItemSelectionPageSix_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow Width="100%" Height="20%">
									<asp:TableCell Width="20%" Height="100%">
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow Width="100%" Height="20%">
									<asp:TableCell Width="20%" Height="100%">
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow Width="100%" Height="20%">
									<asp:TableCell Width="20%" Height="100%">
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="BtnExit" CssClass="wrap" Text="Exit" runat="server" OnClick="BtnExit_Click" Visible="false" />
									</asp:TableCell>
								</asp:TableRow>
							</asp:Table>
						</asp:View>
						<asp:View ID="viewPageOne" runat="server">
							<asp:Table ID="tblPageOne" runat="server" Width="100%" Height="100%">
								<asp:TableRow Width="100%" Height="20%">
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageOneItemOne" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageOneItemTwo" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageOneItemThree" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageOneItemFour" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageOneItemFive" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow Width="100%" Height="20%">
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageOneItemSix" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageOneItemSeven" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageOneItemEight" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageOneItemNine" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageOneItemTen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow Width="100%" Height="20%">
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageOneItemEleven" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageOneItemTwelve" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageOneItemThirteen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageOneItemFourteen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageOneItemFifteen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow Width="100%" Height="20%">
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageOneSixteen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageOneItemSeventeen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageOneItemEighteen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageOneItemNineteen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageOneItemTwenty" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow Width="100%" Height="20%">
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="BtnBackFromPageOne" CssClass="wrap" Text="Back" runat="server" OnClick="BtnBack_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageOneItemTwentyOne" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageOneItemTwentyTwo" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageOneItemTwentyThree" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnExitFromPageOne" CssClass="wrap" Text="Exit" runat="server" OnClick="BtnExit_Click" Visible="false" />
									</asp:TableCell>
								</asp:TableRow>
							</asp:Table>
						</asp:View>
						<asp:View ID="viewPageTwo" runat="server">
							<asp:Table ID="tblPageTwo" runat="server" Width="100%" Height="100%">
								<asp:TableRow Width="100%" Height="20%">
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageTwoItemOne" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageTwoItemTwo" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageTwoItemThree" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageTwoItemFour" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageTwoItemFive" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow Width="100%" Height="20%">
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageTwoItemSix" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageTwoItemSeven" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageTwoItemEight" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageTwoItemNine" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageTwoItemTen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow Width="100%" Height="20%">
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageTwoItemEleven" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageTwoItemTwelve" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageTwoItemThirteen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageTwoItemFourteen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageTwoItemFifteen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow Width="100%" Height="20%">
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageTwoItemSixteen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageTwoItemSeventeen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageTwoItemEighteen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageTwoItemNineteen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageTwoItemTwenty" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow Width="100%" Height="20%">
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnBackFromPageTwo" CssClass="wrap" Text="Back" runat="server" OnClick="BtnBack_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageTwoItemTwentyOne" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageTwoItemTwentyTwo" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageTwoItemTwentThree" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnExitFromPageTwo" CssClass="wrap" Text="Exit" runat="server" OnClick="BtnExit_Click" Visible="false" />
									</asp:TableCell>
								</asp:TableRow>
							</asp:Table>
						</asp:View>
						<asp:View ID="viewPageThree" runat="server">
							<asp:Table ID="tblPageThree" runat="server" Width="100%" Height="100%">
								<asp:TableRow Width="100%" Height="20%">
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageThreeItemOne" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageThreeItemTwo" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageThreeItemThree" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageThreeItemFour" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageThreeItemFive" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow Width="100%" Height="20%">
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageThreeItemSix" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageThreeItemSeven" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageThreeItemEight" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageThreeItemNine" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageThreeItemTen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow Width="100%" Height="20%">
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageThreeItemEleven" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageThreeItemTwelve" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageThreeItemThirteen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageThreeItemFourteen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageThreeItemFifteen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow Width="100%" Height="20%">
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageThreeItemSixteen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageThreeItemSeventeen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageThreeItemEighteen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageThreeItemNineteen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageThreeItemTwenty" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow Width="100%" Height="20%">
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnBackFromPageThree" CssClass="wrap" Text="Back" runat="server" OnClick="BtnBack_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageThreeItemTwentyOne" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageThreeItemTwentyTwo" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageThreeItemTwentyThree" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnExitFromPageThree" CssClass="wrap" Text="Exit" runat="server" OnClick="BtnExit_Click" Visible="false" />
									</asp:TableCell>
								</asp:TableRow>
							</asp:Table>
						</asp:View>
						<asp:View ID="viewPageFour" runat="server">
							<asp:Table ID="tblPageFour" runat="server" Width="100%" Height="100%">
								<asp:TableRow Width="100%" Height="20%">
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFourItemOne" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFourItemTwo" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFourItemThree" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFourItemFour" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFourItemFive" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow Width="100%" Height="20%">
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFourItemSix" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFourItemSeven" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFourItemEight" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFourItemNine" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFourItemTen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow Width="100%" Height="20%">
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFourItemEleven" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFourItemTwelve" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFourItemThirteen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFourItemFourteen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFourItemFifteen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow Width="100%" Height="20%">
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFourItemSixteen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFourItemSeventeen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFourItemEighteen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFourItemNineteen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFourItemTwenty" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow Width="100%" Height="20%">
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnBackFromPageFour" CssClass="wrap" Text="Back" runat="server" OnClick="BtnBack_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFourItemTwentyOne" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFourItemTwentyTwo" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFourItemTwentyThree" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnExitFromPageFour" CssClass="wrap" Text="Exit" runat="server" OnClick="BtnExit_Click" Visible="false" />
									</asp:TableCell>
								</asp:TableRow>
							</asp:Table>
						</asp:View>
						<asp:View ID="viewPageFive" runat="server">
							<asp:Table ID="tblPageFive" runat="server" Width="100%" Height="100%">
								<asp:TableRow Width="100%" Height="20%">
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFiveItemOne" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFiveItemTwo" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFiveItemThree" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFiveItemFour" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFiveItemFive" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow Width="100%" Height="20%">
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFiveItemSix" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFiveItemSeven" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFiveItemEight" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFiveItemNine" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFiveItemTen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow Width="100%" Height="20%">
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFiveItemEleven" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFiveItemTwelve" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFiveItemThirteen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFiveItemFourteen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFiveItemFifteen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow Width="100%" Height="20%">
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFiveItemSixteen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFiveItemSeventeen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFiveItemEighteen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFiveItemNineteen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFiveItemTwenty" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow Width="100%" Height="20%">
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnBackFromPageFive" CssClass="wrap" Text="Back" runat="server" OnClick="BtnBack_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFiveItemTwentyOne" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFiveItemTwentyTwo" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageFiveItemTwentyThree" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnExitFromPageFive" CssClass="wrap" Text="Exit" runat="server" OnClick="BtnExit_Click" Visible="false" />
									</asp:TableCell>
								</asp:TableRow>
							</asp:Table>
						</asp:View>
						<asp:View ID="viewPageSix" runat="server">
							<asp:Table ID="tblPageSix" runat="server" Width="100%" Height="100%">
								<asp:TableRow Width="100%" Height="20%">
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageSixItemOne" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageSixItemTwo" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageSixItemThree" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageSixItemFour" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageSixItemFive" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow Width="100%" Height="20%">
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageSixItemSix" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageSixItemSeven" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageSixItemEight" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageSixItemNine" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageSixItemTen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow Width="100%" Height="20%">
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageSixItemEleven" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageSixItemTwelve" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageSixItemThirteen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageSixItemFourteen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageSixItemFifteen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow Width="100%" Height="20%">
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageSixItemSixteen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageSixItemSeventeen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageSixItemEighteen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageSixItemNineteen" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageSixItemTwenty" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow Width="100%" Height="20%">
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnBackFromPageSix" CssClass="wrap" Text="Back" runat="server" OnClick="BtnBack_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageSixItemTwentyOne" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageSixItemTwentyTwo" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnPageSixItemTwentyThree" CssClass="wrap" Text="Program Button" runat="server" OnClick="BtnSelectedItem_Click" />
									</asp:TableCell>
									<asp:TableCell Width="20%" Height="100%">
										<asp:Button ID="btnExitFromPageSix" CssClass="wrap" Text="Exit" runat="server" OnClick="BtnExit_Click" Visible="false" />
									</asp:TableCell>
								</asp:TableRow>
							</asp:Table>
						</asp:View>
					</asp:MultiView>
					<hr />
					<asp:Table ID="tblSelectItem" runat="server" Width="100%" Visible="false">
						<asp:TableRow Width="100%">
							<asp:TableCell Width="50%">
								<asp:TextBox ID="txtButtonText" runat="server" Visible="false" />
							</asp:TableCell>
							<asp:TableCell Width="50%">
								<asp:Button ID="BtnEditButtonText" runat="server" Text="Enter Button Text" OnClick="BtnEditButtonText_Click" Visible="false" />
							</asp:TableCell>
						</asp:TableRow>
						<asp:TableRow Width="100%">
							<asp:TableCell Width="50%">
								<asp:TextBox ID="txtSearchText" runat="server" Visible="false" />
							</asp:TableCell>
							<asp:TableCell Width="50%">
								<asp:Button ID="BtnSearchText" runat="server" Text="Search Food Item" OnClick="BtnSearchText_Click" Visible="false" />
							</asp:TableCell>
						</asp:TableRow>
					</asp:Table>
					<asp:GridView ID="GrdSelectItem" runat="server" AutoGenerateColumns="false" Visible="false" OnRowCommand="GrdSelectItem_RowCommand">
						<Columns>
							<asp:TemplateField HeaderText="Program">
								<ItemTemplate>
									<asp:Button ID="BtnInsert" runat="server" Text="Program" CommandArgument='<%#Eval("intInventoryID") %>' />
								</ItemTemplate>
							</asp:TemplateField>
							<asp:BoundField DataField="varItemDescription" HeaderText="Description" ReadOnly="true" />
							<asp:BoundField DataField="fltItemPrice" HeaderText="Price" ReadOnly="true" />
						</Columns>
					</asp:GridView>
				</asp:TableCell>
				<asp:TableCell Width="60%">
					<asp:Panel ID="pnlDefaultButton" runat="server">
						<asp:Table ID="tblMasterReceipt" runat="server" Width="100%">
							<asp:TableRow Width="100%">
								<asp:TableCell ID="cellMaster" Width="50%" Visible="false">
									<asp:Label ID="lblMaster" runat="server" Text="Master" />
									<asp:GridView ID="GrdMasterCartItems" EmptyDataText=" No Records Found" runat="server" AutoGenerateColumns="false"
										Style="margin-right: 0px" RowStyle-HorizontalAlign="Center" OnRowCommand="GrdMasterCartItems_RowCommand"
										DataKeyNames="intInvoiceItemID" >
										<Columns>
											<asp:TemplateField HeaderText="Remove Item">
												<ItemTemplate>
													<asp:LinkButton Text="Remove" runat="server" CommandName="R" CommandArgument='<%# Eval("intInvoiceItemID") %>' OnClientClick="return confirm('Are you sure you want to delete?');" CausesValidation="false" />
												</ItemTemplate>
											</asp:TemplateField>
											<asp:BoundField DataField="varSku" HeaderText="SKU" ReadOnly="true" Visible="false" />
											<asp:BoundField DataField="intItemQuantity" HeaderText="Quantity" />
											<asp:BoundField DataField="varItemDescription" ReadOnly="true" HeaderText="Description" />
											<asp:TemplateField HeaderText="Price" ItemStyle-Width="50px">
												<ItemTemplate>
													<asp:Label ID="lblEachItemPrice" runat="server" Text='<%#  (Eval("fltItemPrice","{0:C}")).ToString() %>' />
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField HeaderText="Discount Amount">
												<ItemTemplate>
													<asp:CheckBox ID="ckbEachDiscountPercentDisplay" Checked='<%# Convert.ToBoolean(Eval("bitIsDiscountPercent")) %>' runat="server" Text="Discount by Percent" Enabled="false" />
													<div id="divAmountDisplay" class="txt" runat="server">
														<asp:Label ID="lblEachDiscountAmountDisplay" runat="server" Text='<%# Eval("fltItemDiscount") %>' Enabled="false" />
													</div>
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField HeaderText="Type ID" Visible="false">
												<ItemTemplate>
													<asp:Label ID="lblEachItemTypeID" Text='<%# Eval("intItemTypeID") %>' runat="server" />
												</ItemTemplate>
											</asp:TemplateField>
										</Columns>
									</asp:GridView>
								</asp:TableCell>
								<asp:TableCell ID="cellEachInvoice" Width="50%" Visible="false">
									<asp:Repeater ID="RptEachInvoice" OnItemDataBound="RptEachInvoice_ItemDataBound" runat="server">
										<ItemTemplate>
											<asp:Label ID="lblEachCustomer" runat="server" Text="Customer Name:" />
											<asp:Label ID="lblEachDescription" runat="server" Text="Customer Description:" Visible="false" />
											<asp:HiddenField ID="hidRepeatCustomerID" runat="server" Value='<%#Eval("customer.intCustomerID") %>' />
											<asp:TextBox ID="txtEachCustomer" runat="server" Text='<%#Eval("customer.varFirstName") + " " + Eval("customer.varLastName") %>' Enabled="false" />
											<asp:TextBox ID="txtEachDescription" runat="server" Text='<%#Eval("varAdditionalInformation") %>' Visible="false" />

											<div style="text-align: right">
												<asp:Label ID="lblInvoiceNumber" runat="server" Text="Invoice No:" />
												<asp:Label ID="lblInvoiceNumberDisplay" Text='<%# Eval("varInvoiceNumber") + "-" + Eval("intInvoiceSubNumber") %>' runat="server" />
												<br />
												<asp:Label ID="lblDate" runat="server" Text="Date:" />
												<asp:Label ID="lblDateDisplay" Text='<%# Eval("dtmInvoiceDate", "{0:d}") %>' runat="server" />
												<hr />
											</div>

											<asp:GridView ID="GrdEachCartItems" EmptyDataText=" No Records Found" runat="server" AutoGenerateColumns="false"
												Style="margin-right: 0px" RowStyle-HorizontalAlign="Center" OnRowCommand="GrdEachCartItems_RowCommand"
												DataKeyNames="intInvoiceItemID" DataSource='<%#Eval("invoiceItems") %>'>
												<Columns>
													<asp:TemplateField HeaderText="Remove Item">
														<ItemTemplate>
															<asp:LinkButton Text="Remove" runat="server" CommandName="R" CommandArgument='<%# Eval("intInvoiceItemID") %>' OnClientClick="return confirm('Are you sure you want to delete?');" CausesValidation="false" />
														</ItemTemplate>
													</asp:TemplateField>
													<asp:BoundField DataField="varSku" HeaderText="SKU" ReadOnly="true" Visible="false" />
													<asp:BoundField DataField="intItemQuantity" HeaderText="Quantity" />
													<asp:BoundField DataField="varItemDescription" ReadOnly="true" HeaderText="Description" />
													<asp:TemplateField HeaderText="Price" ItemStyle-Width="50px">
														<ItemTemplate>
															<asp:Label ID="lblEachItemPrice" runat="server" Text='<%#  (Eval("fltItemPrice","{0:C}")).ToString() %>' />
														</ItemTemplate>
													</asp:TemplateField>
													<asp:TemplateField HeaderText="Discount Amount">
														<ItemTemplate>
															<asp:CheckBox ID="ckbEachDiscountPercentDisplay" Checked='<%# Convert.ToBoolean(Eval("bitIsDiscountPercent")) %>' runat="server" Text="Discount by Percent" Enabled="false" />
															<div id="divAmountDisplay" class="txt" runat="server">
																<asp:Label ID="lblEachDiscountAmountDisplay" runat="server" Text='<%# Eval("fltItemDiscount") %>' Enabled="false" />
															</div>
														</ItemTemplate>
														<EditItemTemplate>
															<asp:CheckBox ID="ckbEachDiscountPercentEdit" Checked='<%# Convert.ToBoolean(Eval("bitIsDiscountPercent")) %>' runat="server" Text="Discount by Percent" Enabled="true" />
															<div id="divAmountEdit" class="txt" runat="server">
																<asp:TextBox ID="txtEachDiscountAmountDisplay" runat="server" AutoComplete="off" Text='<%# Eval("fltItemDiscount") %>' Enabled="true" />
															</div>
														</EditItemTemplate>
													</asp:TemplateField>
													<asp:TemplateField HeaderText="Type ID" Visible="false">
														<ItemTemplate>
															<asp:Label ID="lblEachItemTypeID" Text='<%# Eval("intItemTypeID") %>' runat="server" />
														</ItemTemplate>
													</asp:TemplateField>
												</Columns>
											</asp:GridView>
										</ItemTemplate>
										<SeparatorTemplate>
											<br>
										</SeparatorTemplate>
									</asp:Repeater>

								</asp:TableCell>
								<asp:TableCell ID="cellSingleInvoice" Width="100%" Visible="false">
									<asp:Label ID="lblCustomer" runat="server" Text="Customer Name:" />
									<asp:Label ID="lblCustomerDescription" runat="server" Text="Customer Description:" Visible="false" />
									<asp:TextBox ID="txtCustomer" runat="server" AutoComplete="off" />
									<asp:TextBox ID="txtCustomerDescription" runat="server" AutoComplete="off" Visible="false" />
									<asp:Button ID="BtnCustomerSelect" runat="server" Text="Change Customer" OnClick="BtnCustomerSelect_Click" CausesValidation="false" />
									<div>
										<br />
										<div>
											<asp:GridView ID="GrdCustomersSearched" runat="server" AutoGenerateColumns="false" ShowFooter="true"
												OnRowCommand="GrdCustomersSearched_RowCommand" AllowPaging="True" PageSize="5"
												OnPageIndexChanging="GrdCustomersSearched_PageIndexChanging">
												<Columns>
													<asp:TemplateField HeaderText="Switch Customer">
														<ItemTemplate>
															<asp:LinkButton ID="lbtnSwitchCustomer" CommandName="SwitchCustomer" CommandArgument='<%#Eval("intCustomerID") %>' Text="Switch Customer" runat="server" />
														</ItemTemplate>
														<FooterTemplate>
															<asp:Button ID="BtnAddCustomer" runat="server" Text="Add Customer" OnClick="BtnAddCustomer_Click" />
														</FooterTemplate>
													</asp:TemplateField>
													<asp:TemplateField HeaderText="Customer Name">
														<ItemTemplate>
															<asp:Label runat="server" Text='<%#Eval("varFirstName") + " " + Eval("varLastName") %>' />
														</ItemTemplate>
														<FooterTemplate>
															<div>
																<asp:TextBox ID="txtFirstName" runat="server" AutoComplete="off" placeholder="First Name" ToolTip="First Name" />
															</div>
															<div>
																<asp:TextBox ID="txtLastName" runat="server" AutoComplete="off" placeholder="Last Name" ToolTip="Last Name" />
															</div>
														</FooterTemplate>
													</asp:TemplateField>
													<asp:TemplateField HeaderText="Phone Number">
														<ItemTemplate>
															<asp:Label runat="server" Text='<%#Eval("varContactNumber") %>' />
														</ItemTemplate>
														<FooterTemplate>
															<div>
																<asp:TextBox ID="txtPhoneNumber" runat="server" AutoComplete="off" placeholder="Phone Number" ToolTip="Phone Number" />
															</div>
														</FooterTemplate>
													</asp:TemplateField>
													<asp:TemplateField HeaderText="Email Address">
														<ItemTemplate>
															<asp:Label runat="server" Text='<%#Eval("varEmailAddress") %>' />
														</ItemTemplate>
														<FooterTemplate>
															<div>
																<asp:TextBox ID="txtEmail" runat="server" AutoComplete="off" placeholder="Email" ToolTip="Email" />
															</div>
															<div>
																<asp:CheckBox ID="chkMarketingEnrollment" runat="server" Text="Marketing Enrollment" />
															</div>
														</FooterTemplate>
													</asp:TemplateField>
												</Columns>
											</asp:GridView>
										</div>
										<br />
									</div>
									<div style="text-align: right">
										<asp:Label ID="lblInvoiceNumber" runat="server" Text="Invoice No:" />
										<asp:Label ID="lblInvoiceNumberDisplay" runat="server" />
										<br />
										<asp:Label ID="lblDate" runat="server" Text="Date:" />
										<asp:Label ID="lblDateDisplay" runat="server" />
										<hr />
									</div>
									<asp:GridView ID="GrdCartItems" EmptyDataText=" No Records Found" runat="server" AutoGenerateColumns="false"
										Style="margin-right: 0px" OnRowEditing="OnRowEditing" OnRowUpdating="OnRowUpdating"
										OnRowCancelingEdit="ORowCanceling" OnRowDeleting="OnRowDeleting" RowStyle-HorizontalAlign="Center" DataKeyNames="intInvoiceItemID">
										<Columns>
											<asp:TemplateField HeaderText="Remove Item">
												<ItemTemplate>
													<asp:LinkButton Text="Remove" runat="server" CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete?');" CausesValidation="false" />
													<asp:Label ID="lblInvoiceItemID" Text='<%#Eval("intInvoiceItemID")%>' runat="server" Visible="false" />
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField HeaderText="Edit Item">
												<ItemTemplate>
													<asp:LinkButton Text="Edit" runat="server" CommandName="Edit" CommandArgument='<%#Eval("intInvoiceItemID")%>' CausesValidation="false" />
												</ItemTemplate>
												<EditItemTemplate>
													<asp:LinkButton Text="Update" runat="server" CommandName="Update" CommandArgument='<%#Eval("intInvoiceItemID")%>' CausesValidation="false" />
													<asp:LinkButton Text="Cancel" runat="server" CommandName="Cancel" CausesValidation="false" />
												</EditItemTemplate>
											</asp:TemplateField>
											<asp:BoundField DataField="varSku" HeaderText="SKU" ReadOnly="true" Visible="false" />
											<asp:BoundField DataField="intItemQuantity" HeaderText="Quantity" />
											<asp:BoundField DataField="varItemDescription" ReadOnly="true" HeaderText="Description" />
											<asp:TemplateField HeaderText="Price" ItemStyle-Width="50px">
												<ItemTemplate>
													<asp:Label ID="price" runat="server" Text='<%#  (Eval("fltItemPrice","{0:C}")).ToString() %>' />
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField HeaderText="Discount Amount">
												<ItemTemplate>
													<asp:CheckBox ID="ckbPercentageDisplay" Checked='<%# Convert.ToBoolean(Eval("bitIsDiscountPercent")) %>' runat="server" Text="Discount by Percent" Enabled="false" />
													<div id="divAmountDisplay" class="txt" runat="server">
														<asp:Label ID="lblAmountDisplay" runat="server" Text='<%# Eval("fltItemDiscount") %>' Enabled="false" />
													</div>
												</ItemTemplate>
												<EditItemTemplate>
													<asp:CheckBox ID="ckbPercentageEdit" Checked='<%# Convert.ToBoolean(Eval("bitIsDiscountPercent")) %>' runat="server" Text="Discount by Percent" Enabled="true" />
													<div id="divAmountEdit" class="txt" runat="server">
														<asp:TextBox ID="txtAmnt" runat="server" AutoComplete="off" Text='<%# Eval("fltItemDiscount") %>' Enabled="true" />
													</div>
												</EditItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField HeaderText="Type ID" Visible="false">
												<ItemTemplate>
													<asp:Label ID="lblTypeID" Text='<%# Eval("intItemTypeID") %>' runat="server" />
												</ItemTemplate>
											</asp:TemplateField>
										</Columns>
									</asp:GridView>
									<hr />
									<asp:Table ID="tblTotals" runat="server" Width="100%">
										<asp:TableRow Width="100%">
											<asp:TableCell Width="25%">
												<asp:Label ID="lblGovernmentTax" runat="server" Text="GST:" />
											</asp:TableCell>
											<asp:TableCell Width="25%">
												<asp:Label ID="lblGovernmentTaxDisplay" runat="server" />
											</asp:TableCell>
											<asp:TableCell Width="25%">
												<asp:Label ID="lblLiquorTax" runat="server" Text="LCT:" />
											</asp:TableCell>
											<asp:TableCell Width="25%">
												<asp:Label ID="lblLiquorTaxDisplay" runat="server" />
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow>
											<asp:TableCell>
												<asp:Label ID="lblProvincialTax" runat="server" Text="PST:" />
											</asp:TableCell>
											<asp:TableCell>
												<asp:Label ID="lblProvincialTaxDisplay" runat="server" />
											</asp:TableCell>
											<asp:TableCell>
												<asp:Label ID="lblSubtotal" runat="server" Text="Subtotal:" />
											</asp:TableCell>
											<asp:TableCell>
												<asp:Label ID="lblSubtotalDisplay" runat="server" />
											</asp:TableCell>
										</asp:TableRow>
									</asp:Table>
								</asp:TableCell>
							</asp:TableRow>
						</asp:Table>
					</asp:Panel>
				</asp:TableCell>
			</asp:TableRow>
		</asp:Table>
		<hr />
		<asp:Table runat="server">
			<asp:TableRow>
				<asp:TableCell>
					<asp:Button ID="BtnCancelSale" runat="server" Text="Void Transaction" OnClick="BtnCancelSale_Click" Width="163px" CausesValidation="false" />
				</asp:TableCell>
				<asp:TableCell>
					<asp:Button ID="BtnExitSale" runat="server" Text="Hold Bill" OnClick="BtnExitSale_Click" Width="163px" CausesValidation="false" />
				</asp:TableCell>
				<asp:TableCell>
					<asp:Button ID="BtnPrintBill" runat="server" Text="Print Bill" OnClick="BtnPrintBill_Click" Width="163px" CausesValidation="false" />
				</asp:TableCell>
				<asp:TableCell>
					<asp:Button ID="BtnProcessPayment" runat="server" Text="Process Payment" OnClick="BtnProcessPayment_Click" Width="163px" CausesValidation="false" />
				</asp:TableCell>
			</asp:TableRow>
		</asp:Table>
	</div>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="LoungeSims.aspx.cs" Inherits="SweetSpotDiscountGolfPOS.LoungeSims" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
	<style type="text/css">
		.auto-style1 {
			position: relative;
			left: 300px;
			top: -10px;
			width: 207px;
			height: 228px;
		}
	</style>
</asp:Content>
<%--<asp:Content ID="Content2" ContentPlaceHolderID="SPMaster" runat="server">
</asp:Content>--%>

<asp:Content ID="LoungeSimsPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
	<div id="LoungeSims">
		<asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="btnSimulatorOne">
			<h2>Lounge Layout</h2>
			<br />
			<div>
				<asp:Table runat="server" Width="100%">
					<asp:TableRow>
						<asp:TableCell Width="2%">
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell Width="21%" ColumnSpan="7" HorizontalAlign="Center">
							<asp:Button ID="btnSimulatorOne" runat="server" Text="Simulator One" Width="100%" OnClick="btnSimulator_Click" />
						</asp:TableCell>
						<asp:TableCell Width="2%">
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell Width="2%">
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell Width="21%" ColumnSpan="7" HorizontalAlign="Center">
							<asp:Button ID="btnSimulatorTwo" runat="server" Text="Simulator Two" Width="100%" OnClick="btnSimulator_Click" />
						</asp:TableCell>
						<asp:TableCell Width="2%">
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell Width="2%">
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell Width="21%" ColumnSpan="7" HorizontalAlign="Center">
							<asp:Button ID="btnSimulatorThree" runat="server" Text="Simulator Three" Width="100%" OnClick="btnSimulator_Click" />
						</asp:TableCell>
						<asp:TableCell Width="2%">
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell Width="2%">
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell Width="21%" ColumnSpan="7" HorizontalAlign="Center">
							<asp:Button ID="SimulatorFour" runat="server" Text="Simulator Four" Width="100%" OnClick="btnSimulator_Click" />
						</asp:TableCell>
						<asp:TableCell Width="2%">
							<%--BlankRow--%>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell Width="9.5%" ColumnSpan="3" HorizontalAlign="Center">
							<asp:Button ID="btnSimulatorOnePlayerOne" runat="server" Text="Player One" Width="100%" OnClick="btnPlayer_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell Width="2%">
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell Width="9.5%" ColumnSpan="3" HorizontalAlign="Center">
							<asp:Button ID="btnSimulatorOnePlayerThree" runat="server" Text="Player Three" Width="100%" OnClick="btnPlayer_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell Width="9.5%" ColumnSpan="3" HorizontalAlign="Center">
							<asp:Button ID="btnSimulatorTwoPlayerOne" runat="server" Text="Player One" Width="100%" OnClick="btnPlayer_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell Width="2%">
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell Width="9.5%" ColumnSpan="3" HorizontalAlign="Center">
							<asp:Button ID="btnSimulatorTwoPlayerThree" runat="server" Text="Player Three" Width="100%" OnClick="btnPlayer_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell Width="9.5%" ColumnSpan="3" HorizontalAlign="Center">
							<asp:Button ID="btnSimulatorThreePlayerOne" runat="server" Text="Player One" Width="100%" OnClick="btnPlayer_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell Width="2%">
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell Width="9.5%" ColumnSpan="3" HorizontalAlign="Center">
							<asp:Button ID="btnSimulatorThreePlayerThree" runat="server" Text="Player Three" Width="100%" OnClick="btnPlayer_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell Width="9.5%" ColumnSpan="3" HorizontalAlign="Center">
							<asp:Button ID="btnSimulatorFourPlayerOne" runat="server" Text="Player One" Width="100%" OnClick="btnPlayer_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell Width="2%">
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell Width="9.5%" ColumnSpan="3" HorizontalAlign="Center">
							<asp:Button ID="btnSimulatorFourPlayerThree" runat="server" Text="Player Three" Width="100%" OnClick="btnPlayer_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
							<asp:Button ID="btnSimulatorOnePlayerTwo" runat="server" Text="Player Two" Width="100%" OnClick="btnPlayer_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
							<asp:Button ID="btnSimulatorOnePlayerFour" runat="server" Text="Player Four" Width="100%" OnClick="btnPlayer_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
							<asp:Button ID="btnSimulatorTwoPlayerTwo" runat="server" Text="Player Two" Width="100%" OnClick="btnPlayer_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
							<asp:Button ID="btnSimulatorTwoPlayerFour" runat="server" Text="Player Four" Width="100%" OnClick="btnPlayer_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
							<asp:Button ID="btnSimulatorThreePlayerTwo" runat="server" Text="Player Two" Width="100%" OnClick="btnPlayer_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
							<asp:Button ID="btnSimulatorThreePlayerFour" runat="server" Text="Player Four" Width="100%" OnClick="btnPlayer_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
							<asp:Button ID="btnSimulatorFourPlayerTwo" runat="server" Text="Player Two" Width="100%" OnClick="btnPlayer_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
							<asp:Button ID="btnSimulatorFourPlayerFour" runat="server" Text="Player Four" Width="100%" OnClick="btnPlayer_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell ColumnSpan="36">
							<br />
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell Width="5.5%">
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell Width="1%">
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
							<asp:Button ID="btnTableOneSeatTwo" runat="server" Text="Seat 2" Width="80%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell Width="1%">
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell Width="5.5%">
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell Width="5.5%">
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell Width="1%">
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
							<asp:Button ID="btnTableTwoSeatTwo" runat="server" Text="Seat 2" Width="80%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell Width="1%">
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell Width="5.5%">
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell Width="5.5%">
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell Width="1%">
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
							<asp:Button ID="btnTableThreeSeatTwo" runat="server" Text="Seat 2" Width="80%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell Width="1%">
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell Width="5.5%">
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell Width="5.5%">
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell Width="1%">
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
							<asp:Button ID="btnTableFourSeatTwo" runat="server" Text="Seat 2" Width="80%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell Width="1%">
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell Width="5.5%">
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<asp:Button ID="btnTableOneSeatOne" runat="server" Text="Seat 1" Width="100%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell ColumnSpan="3">
							<asp:Button ID="btnTableOne" runat="server" Text="Table 1" Width="100%" OnClick="btnTable_Click" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<asp:Button ID="btnTableOneSeatThree" runat="server" Text="Seat 3" Width="100%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<asp:Button ID="btnTableTwoSeatOne" runat="server" Text="Seat 1" Width="100%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell ColumnSpan="3">
							<asp:Button ID="btnTableTwo" runat="server" Text="Table 2" Width="100%" OnClick="btnTable_Click" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<asp:Button ID="btnTableTwoSeatThree" runat="server" Text="Seat 3" Width="100%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<asp:Button ID="btnTableThreeSeatOne" runat="server" Text="Seat 1" Width="100%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell ColumnSpan="3">
							<asp:Button ID="btnTableThree" runat="server" Text="Table 3" Width="100%" OnClick="btnTable_Click" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<asp:Button ID="btnTableThreeSeatThree" runat="server" Text="Seat 3" Width="100%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<asp:Button ID="btnTableFourSeatOne" runat="server" Text="Seat 1" Width="100%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell ColumnSpan="3">
							<asp:Button ID="btnTableFour" runat="server" Text="Table 4" Width="100%" OnClick="btnTable_Click" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<asp:Button ID="btnTableFourSeatThree" runat="server" Text="Seat 3" Width="100%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
							<asp:Button ID="btnTableOneSeatFour" runat="server" Text="Seat 4" Width="80%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
							<asp:Button ID="btnTableTwoSeatFour" runat="server" Text="Seat 4" Width="80%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
							<asp:Button ID="btnTableThreeSeatFour" runat="server" Text="Seat 4" Width="80%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
							<asp:Button ID="btnTableFourSeatFour" runat="server" Text="Seat 4" Width="80%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell ColumnSpan="36">
							<br />
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
							<asp:Button ID="btnTableFiveSeatTwo" runat="server" Text="Seat 2" Width="80%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
							<asp:Button ID="btnTableSixSeatTwo" runat="server" Text="Seat 2" Width="80%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
							<asp:Button ID="btnTableSevenSeatTwo" runat="server" Text="Seat 2" Width="80%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
							<asp:Button ID="btnTableEightSeatTwo" runat="server" Text="Seat 2" Width="80%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<asp:Button ID="btnTableFiveSeatOne" runat="server" Text="Seat 1" Width="100%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell ColumnSpan="3">
							<asp:Button ID="btnTableFive" runat="server" Text="Table 5" Width="100%" OnClick="btnTable_Click" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<asp:Button ID="btnTableFiveSeatThree" runat="server" Text="Seat 3" Width="100%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<asp:Button ID="btnTableSixSeatOne" runat="server" Text="Seat 1" Width="100%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell ColumnSpan="3">
							<asp:Button ID="btnTableSix" runat="server" Text="Table 6" Width="100%" OnClick="btnTable_Click" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<asp:Button ID="btnTableSixSeatThree" runat="server" Text="Seat 3" Width="100%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<asp:Button ID="btnTableSevenSeatOne" runat="server" Text="Seat 1" Width="100%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell ColumnSpan="3">
							<asp:Button ID="btnTableSeven" runat="server" Text="Table 7" Width="100%" OnClick="btnTable_Click" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<asp:Button ID="btnTableSevenSeatThree" runat="server" Text="Seat 3" Width="100%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<asp:Button ID="btnTableEightSeatOne" runat="server" Text="Seat 1" Width="100%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell ColumnSpan="3">
							<asp:Button ID="btnTableEight" runat="server" Text="Table 8" Width="100%" OnClick="btnTable_Click" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<asp:Button ID="btnTableEightSeatThree" runat="server" Text="Seat 3" Width="100%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
							<asp:Button ID="btnTableFiveSeatFour" runat="server" Text="Seat 4" Width="80%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
							<asp:Button ID="btnTableSixSeatFour" runat="server" Text="Seat 4" Width="80%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
							<asp:Button ID="btnTableSevenSeatFour" runat="server" Text="Seat 4" Width="80%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
							<asp:Button ID="btnTableEightSeatFour" runat="server" Text="Seat 4" Width="80%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell ColumnSpan="36">
							<br />
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
							<asp:Button ID="btnTableNineSeatTwo" runat="server" Text="Seat 2" Width="80%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
							<asp:Button ID="btnTableTenSeatTwo" runat="server" Text="Seat 2" Width="80%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
							<asp:Button ID="btnTableElevenSeatTwo" runat="server" Text="Seat 2" Width="80%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
							<asp:Button ID="btnTableTwelveSeatTwo" runat="server" Text="Seat 2" Width="80%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<asp:Button ID="btnTableNineSeatOne" runat="server" Text="Seat 1" Width="100%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell ColumnSpan="3">
							<asp:Button ID="btnTableNine" runat="server" Text="Table 9" Width="100%" OnClick="btnTable_Click" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<asp:Button ID="btnTableNineSeatThree" runat="server" Text="Seat 3" Width="100%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<asp:Button ID="btnTableTenSeatOne" runat="server" Text="Seat 1" Width="100%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell ColumnSpan="3">
							<asp:Button ID="btnTableTen" runat="server" Text="Table 10" Width="100%" OnClick="btnTable_Click" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<asp:Button ID="btnTableTenSeatThree" runat="server" Text="Seat 3" Width="100%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<asp:Button ID="btnTableElevenSeatOne" runat="server" Text="Seat 1" Width="100%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell ColumnSpan="3">
							<asp:Button ID="btnTableEleven" runat="server" Text="Table 11" Width="100%" OnClick="btnTable_Click" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<asp:Button ID="btnTableElevenSeatThree" runat="server" Text="Seat 3" Width="100%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<asp:Button ID="btnTableTwelveSeatOne" runat="server" Text="Seat 1" Width="100%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell ColumnSpan="3">
							<asp:Button ID="btnTableTwelve" runat="server" Text="Table 12" Width="100%" OnClick="btnTable_Click" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<asp:Button ID="btnTableTwelveSeatThree" runat="server" Text="Seat 3" Width="100%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
							<asp:Button ID="btnTableNineSeatFour" runat="server" Text="Seat 4" Width="80%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
							<asp:Button ID="btnTableTenSeatFour" runat="server" Text="Seat 4" Width="80%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
							<asp:Button ID="btnTableElevenSeatFour" runat="server" Text="Seat 4" Width="80%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
							<asp:Button ID="btnTableTwelveSeatFour" runat="server" Text="Seat 4" Width="80%" OnClick="btnSeat_Click" Enabled="false" />
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
						<asp:TableCell>
							<%--BlankRow--%>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell ColumnSpan="36">
							<br />
						</asp:TableCell>
					</asp:TableRow>
				</asp:Table>
			</div>
			<hr />
		</asp:Panel>
	</div>
</asp:Content>

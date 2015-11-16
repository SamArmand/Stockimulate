<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AdminViews/AdminMaster.master" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="Stockimulate.Views.AdminViews.Admin" %>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdminMasterContentPlaceHolder" runat="server">
    <form runat="server">
        <fieldset>

        <!-- Form Name -->
          <legend><h1>Admin Panel</h1></legend>

        <div class="row">

            <asp:Button ID="PlayPracticeButton" CssClass="btn btn-primary" runat="server" Text="Play Practice" OnClick="PlayPractice_Click" />
            <asp:Button ID="PlayCompetitionButton" CssClass="btn btn-primary" runat="server" Text="Play Competition" OnClick="PlayCompetition_Click" />
            <asp:Button ID="ContinueButton" CssClass="btn btn-primary" runat="server" Text="Continue" OnClick="Continue_Click" />

            <br />
            <br />

            <asp:Button ID="ResetTradesButton" CssClass="btn btn-primary" runat="server" Text="Reset Trades" OnClick="ResetTrades_Click" />
            

            <br />

            <input id="VerifyInput" type="checkbox" runat="server" /> I am aware of what I am doing and that any wrong selection may completely break the competition.

          </div>
        </fieldset>
 

    </form>

    <br />
    <br />

    <div id="ErrorDiv" runat="server" class="alert alert-danger bg-danger col-lg-5" style="display: none;">

    </div>
    <div id="WarningDiv" runat="server" class="alert alert-warning bg-warning col-lg-5" style="display: none;">
        <a href="#" class="close" data-dismiss="alert">&times;</a>
        <strong>Info!</strong> Please read the above statement and check the checkbox before clicking any button.
    </div>

</asp:Content>

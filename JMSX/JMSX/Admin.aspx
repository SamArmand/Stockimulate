<%@ Page Title="" Language="C#" MasterPageFile="~/JMSXAdmin.Master" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="JMSX.Admin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form runat="server">
        <fieldset>

        <!-- Form Name -->
          <legend><h1>Admin Panel</h1></legend>

        <div class="row">

            <asp:Button id="PlayPractice" class="btn btn-primary" runat="server" Text="Play Practice" OnClick="PlayPractice_Click" />
            <asp:Button id="PlayCompetition" class="btn btn-primary" runat="server" Text="Play Competition" OnClick="PlayCompetition_Click" />
            <asp:Button id="Continue" class="btn btn-primary" runat="server" Text="Continue" OnClick="Continue_Click" />

            <br />

            <asp:Button id="ResetTrades" class="btn btn-primary" runat="server" Text="Reset Trades" OnClick="ResetTrades_Click" />
            

            <br />

            <input id="Verify" type="checkbox" runat="server" /> I am aware of what I am doing and that any wrong selection may completely break the competition.

          </div>
        </fieldset>
 

    </form>

    <div id="ErrorDiv" runat="server" class="alert alert-danger bg-danger col-lg-5" style="display: none;">

    </div>
    <div id="WarningDiv" runat="server" class="alert alert-warning bg-warning col-lg-5" style="display: none;">
        <a href="#" class="close" data-dismiss="alert">&times;</a>
        <strong>Info!</strong> Please read the above statement and check the checkbox before clicking any button.
    </div>

</asp:Content>

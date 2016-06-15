<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AdministratorViews/AdministratorMaster.master" AutoEventWireup="true" CodeBehind="AdminPanel.aspx.cs" Inherits="Stockimulate.Views.AdministratorViews.AdminPanel" %>
<asp:Content ID="AdminContent" ContentPlaceHolderID="AdministratorMasterContentPlaceHolder" runat="server">

        <fieldset>

        <!-- Form Name -->
          <legend><h1>AdminPanel Panel</h1></legend>
            
            
            <div class="row">
                
                    <div class="btn-group" role="group" ID="IndexButtonGroup" runat="server">
                        
                    </div>

            </div>

        <div class="row">
            


            <asp:Button ID="PlayPracticeButton" CssClass="btn btn-primary" runat="server" Text="Play Practice" OnClick="PlayPractice_Click" />
            <asp:Button ID="PlayCompetitionButton" CssClass="btn btn-primary" runat="server" Text="Play Competition" OnClick="PlayCompetition_Click" />
            <asp:Button ID="ContinueButton" CssClass="btn btn-primary" runat="server" Text="Continue" OnClick="Continue_Click" />

            <br />
            <br />

            <asp:Button ID="ResetTradesButton" CssClass="btn btn-primary" runat="server" Text="Reset Trades" OnClick="ResetTrades_Click" />
            

            <br />
            <br />

            <input id="VerifyInput" type="checkbox" runat="server" /> I am aware of what I am doing and that any wrong selection may completely break the competition.
            
                <br />
    <br />

    <div id="ErrorDiv" runat="server" class="alert alert-danger bg-danger col-lg-5" style="display: none;">

    </div>
    <div id="WarningDiv" runat="server" class="alert alert-warning bg-warning col-lg-5" style="display: none;">
        <a href="#" class="close" data-dismiss="alert">&times;</a>
        <strong>Info!</strong> Please read the above statement and check the checkbox before clicking any button.
    </div>
            

          </div>
        </fieldset>
 
        
        <br/>
        <br/>
        
    <fieldset>

        <div class="form-group row">
            
            <label class="col-sm-1 control-label">Security</label>
            <div class="col-sm-2">
                
                <asp:DropDownList ID="SecurityDropDownList" runat="server" CssClass="form-control"></asp:DropDownList>

            </div>

            <label class="col-sm-1 col-sm-offset-1 control-label">Price</label>
            <div class="col-sm-2">
              <input id="PriceInput" type="number" placeholder="Price" class="form-control" runat="server"/>
            </div>

            <asp:Button ID="UpdatePrice" CssClass="btn btn-primary" runat="server" Text="Update" OnClick="UpdatePrice_Click" />
            
            </div>

            <div class="form-group row">
            
            <label id="ReportsEnabledSpan" runat="server"></label>

            <asp:Button ID="ToggleReportsEnabled" CssClass="btn btn-primary" runat="server" Text="Toggle Reports" OnClick="ToggleReportsEnabled_Click" />
            


            <br />

            <br />

          </div>
        </fieldset>



</asp:Content>

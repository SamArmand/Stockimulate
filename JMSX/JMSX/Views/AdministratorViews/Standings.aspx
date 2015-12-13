<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AdministratorViews/AdministratorMaster.master" AutoEventWireup="true" CodeBehind="Standings.aspx.cs" Inherits="Stockimulate.Views.AdministratorViews.IndividualStandings" %>
<asp:Content ID="Content2" ContentPlaceHolderID="AdministratorMasterContentPlaceHolder" runat="server">

              <legend><h1>Standings</h1></legend>
    
    <div class="col-sm-6">
        
        <h2>Players</h2>

            <div id="PlayersTableDiv" runat="server">


            </div>

    </div>

        <div class="col-sm-6">
        
            <h2>Teams</h2>

            <div id="TeamsTableDiv" runat="server">


            </div>

    </div>
    
    <br/>

</asp:Content>

﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/RegulatorViews/RegulatorMaster.master" AutoEventWireup="true" CodeBehind="SearchReports.aspx.cs" Inherits="Stockimulate.Views.RegulatorViews.AdminReports" %>
<asp:Content ID="RegulatorReportsContent" ContentPlaceHolderID="RegulatorMasterContentPlaceHolder" runat="server">

      <div class="row">
    <div class="col-md-12">
      <div class="form-horizontal" role="form" runat="server">
        <fieldset>

          <!-- Form Name -->
          <legend><h1>Search Reports</h1></legend>
            
          <!-- Text input-->
          <div class="form-group">
            <label class="col-sm-1 control-label">Team Number</label>
            <div class="col-sm-2">
              <input id="TeamNumberInput" type="number" placeholder="Team Number" class="form-control" runat="server" required="required"/>
            </div>

          </div>

      <br/>

          <div class="row">
              <div class="btn-group col-sm-12"> 

                <asp:Button ID="CancelButton" CssClass="btn btn-default" runat="server" Text="Cancel" OnClick="Page_Load" />
                <asp:Button ID="SubmitButton" CssClass="btn btn-primary" runat="server" Text="Submit" OnClick="Submit_Click" />

            </div>              
          </div>

        </fieldset>
      </div>



    </div><!-- /.col-lg-12 -->
</div><!-- /.row -->

    <br />

    <div id="ErrorDiv" runat="server" class="alert alert-danger bg-danger col-lg-5" style="display: none;">
        <a href="#" class="close" data-dismiss="alert">&times;</a>
        <strong>Error!</strong> No such Team ID exists.
    </div>

    <br />

    <div id="TeamTable" runat="server">


        </div>

        <div id="PlayerTables" runat="server">

        </div>
    
    <br/>

</asp:Content>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/PublicViews/PublicMaster.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="Stockimulate.Views.PublicViews.Reports" %>
<asp:Content ID="ReportsContent" ContentPlaceHolderID="PublicMasterContentPlaceHolder" runat="server">

      <div class="row">
    <div class="col-md-12">
      <div class="form-horizontal" role="form">
        <fieldset>

          <!-- Form Name -->
          <legend><h1>View Reports</h1></legend>
            
          <!-- Text input-->
          <div class="form-group">
            <label class="col-sm-1 control-label">Team Number</label>
            <div class="col-sm-2">
              <input id="TeamNumberInput" type="number" placeholder="Team Number" class="form-control" runat="server" required="required"/>
            </div>

            <div id="TeamCodeDiv" runat="server">
            <label class="col-sm-1 col-sm-offset-1 control-label">Team Code</label>
            <div class="col-sm-2">
              <input id="TeamCodeInput" type="text" placeholder="Team Code" class="form-control" runat="server" required="required"/>
            </div>
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
        <strong>Error!</strong> No such Team ID/Code combination exists.
    </div>

    <div id="InfoDiv" runat="server" class="alert alert-info bg-info col-lg-5" style="display: none;">
        <a href="#" class="close" data-dismiss="alert">&times;</a>
        <strong>Info.</strong> The quarterly reports system is not yet open.
    </div>

    <br />

    <div id="TeamTable" runat="server">


        </div>

        <div id="PlayerTables" runat="server">

        </div>
    
    <br/>

</asp:Content>

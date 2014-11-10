<%@ Page Title="" Language="C#" MasterPageFile="~/JMSXBroker.Master" AutoEventWireup="true" CodeBehind="NewTrade.aspx.cs" Inherits="JMSX._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    
  <div class="row">
    <div class="col-md-12">
      <form class="form-horizontal" role="form" runat="server">
        <fieldset>

          <!-- Form Name -->
          <legend><h1>New Trade</h1></legend>
            
          <!-- Text input-->
          <div class="form-group">
            <label class="col-sm-1 control-label" for="textinput">Buyer ID</label>
            <div class="col-sm-2">
              <input id="BuyerID" type="number" placeholder="Buyer ID" class="form-control" runat="server"/>
            </div>

            <label class="col-sm-1 col-sm-offset-1 control-label" for="textinput">Seller ID</label>
            <div class="col-sm-2">
              <input id="SellerID" type="number" placeholder="Seller ID" class="form-control" runat="server"/>
            </div>
          </div>

            <!-- Text input-->
          <div class="form-group">
            <label class="col-sm-1 control-label" for="textinput">Security</label>
            <div class="col-sm-2">
                <select id="Security" class="form-control" runat="server">
                    <option value="OIL">OIL</option>
                    <option value="IND">IND</option>
                </select>
            </div>

            <label class="col-sm-1 col-sm-offset-1 control-label" for="textinput">Quantity</label>
            <div class="col-sm-2">
              <input id="Quantity" type="number" placeholder="Quantity" class="form-control" runat="server"/>
            </div>
          </div>

          <div class="form-group">
            <label class="col-sm-1 control-label " for="textinput">Price</label>
            <div class="col-sm-2 ">
              <input id="Price" type="number" placeholder="Price" class="form-control" runat="server"/>
            </div>
          </div>

      <br/>

            <div class="row">
                <input id="Verify" type="checkbox" runat="server" /> I have verified that all fields are correct and wish to submit this trade.
             
          </div>

          <div class="row">
              <div class="btn-group col-sm-12"> 

                <asp:Button id="Cancel" class="btn btn-default" runat="server" Text="Cancel" OnClick="Page_Load" />
                <asp:Button id="Submit" class="btn btn-primary" runat="server" Text="Submit" OnClick="Submit_Click" />

            </div>              
          </div>

        </fieldset>
      </form>



    </div><!-- /.col-lg-12 -->
</div><!-- /.row -->

    <br />

        <div id="ErrorDiv" runat="server" class="alert alert-danger alert-error bg-danger col-lg-5" style="display: none;">

    </div>

            <div id="SuccessDiv" runat="server" class="alert alert-success bg-success col-lg-5" style="display: none;">
        <a href="#" class="close" data-dismiss="alert">&times;</a>
        <strong>Success!</strong> Trade is complete.
    </div>

        <div id="WarningDiv" runat="server" class="alert alert-warning bg-warning col-lg-5" style="display: none;">
        <a href="#" class="close" data-dismiss="alert">&times;</a>
        <strong>Info!</strong> Verify all the fields in the form and check the checkbox before submitting.
    </div>

</asp:Content>

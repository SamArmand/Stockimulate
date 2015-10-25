<%@ Page Title="" Language="C#" MasterPageFile="~/Views/BrokerViews/MasterBroker.Master" AutoEventWireup="true" CodeBehind="NewTrade.aspx.cs" Inherits="Stockimulate.Views.BrokerViews.NewTrade" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    
  <div class="row">
    <div class="col-md-12">
      <form class="form-horizontal" role="form" runat="server" id="TradeInputForm">
        <fieldset>

          <!-- Form Name -->
          <legend><h1>New Trade</h1></legend>
            
          <!-- Text input-->
          <div class="form-group">
            <label class="col-sm-1 control-label">Buyer ID</label>
            <div class="col-sm-2">
              <input id="BuyerIdInput" type="number" placeholder="Buyer ID" class="form-control" runat="server"/>        
                <asp:RequiredFieldValidator runat="server" ID="BuyerIdInputValidation" controltovalidate="BuyerIdInput" ErrorMessage="This field is required" />
            </div>

            <label class="col-sm-1 col-sm-offset-1 control-label">Seller ID</label>
            <div class="col-sm-2">
              <input id="SellerIdInput" type="number" placeholder="Seller ID" class="form-control" runat="server"/>
                <asp:RequiredFieldValidator runat="server" ID="SellerIdInputValidation" ControlToValidate="SellerIdInput" ErrorMessage="This field is required" />
            </div>
          </div>

            <!-- Text input-->
          <div class="form-group">
            <label class="col-sm-1 control-label">Security</label>
            <div class="col-sm-2">
                
                <asp:DropDownList ID="SecurityDropDownList" runat="server" CssClass="form-control"></asp:DropDownList>

            </div>

            <label class="col-sm-1 col-sm-offset-1 control-label">Quantity</label>
            <div class="col-sm-2">
              <input id="QuantityInput" type="number" placeholder="Quantity" class="form-control" runat="server"/>
                <asp:RequiredFieldValidator runat="server" ID="QuantityInputValidation" ControlToValidate="QuantityInput" ErrorMessage="This field is required" />
            </div>
          </div>

          <div class="form-group">
            <label class="col-sm-1 control-label ">Price</label>
            <div class="col-sm-2 ">
              <input id="PriceInput" type="number" placeholder="Price" class="form-control" runat="server"/>
                <asp:RequiredFieldValidator runat="server" ID="PriceInputValidation" ControlToValidate="PriceInput" ErrorMessage="This field is required" />
            </div>
          </div>

      <br/>

            <div class="row">
                <input id="VerifyInput" type="checkbox" runat="server" /> I have verified that all fields are correct and wish to submit this trade.
             
          </div>

          <div class="row">
              <div class="btn-group col-sm-12"> 

                <asp:Button ID="CancelButton" class="btn btn-default" runat="server" Text="Cancel" OnClick="Page_Load" />
                <asp:Button ID="SubmitButton" class="btn btn-primary" runat="server" Text="Submit" OnClick="Submit_Click" />

            </div>              
          </div>

        </fieldset>
      </form>



    </div><!-- /.col-lg-12 -->
</div><!-- /.row -->

    <br />

        <div id="errorDiv" runat="server" class="alert alert-danger alert-error bg-danger col-lg-5" style="display: none;">

    </div>

            <div id="successDiv" runat="server" class="alert alert-success bg-success col-lg-5" style="display: none;">
        <a href="#" class="close" data-dismiss="alert">&times;</a>
        <strong>Success!</strong> Trade is complete.
    </div>

        <div id="warningDiv" runat="server" class="alert alert-warning bg-warning col-lg-5" style="display: none;">
        <a href="#" class="close" data-dismiss="alert">&times;</a>
        <strong>Info!</strong> Verify all the fields in the form and check the checkbox before submitting.
    </div>

</asp:Content>

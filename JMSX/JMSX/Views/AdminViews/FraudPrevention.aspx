<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AdminViews/MasterAdmin.Master" AutoEventWireup="true" CodeBehind="FraudPrevention.aspx.cs" Inherits="Stockimulate.Views.AdminViews.FraudPrevention" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link href="../../Content/tables.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <form runat="server">
        <fieldset>
            <legend><h1>Fraud Prevention - Under Construction!!!</h1></legend>
    
              <div class="form-group">
            <label class="col-sm-1 control-label">Buyer ID</label>
            <div class="col-sm-2">
              <input id="BuyerIdInput" type="number" placeholder="Buyer ID" class="form-control" runat="server"/>        
            </div>

            <label class="col-sm-1 col-sm-offset-1 control-label">Seller ID</label>
            <div class="col-sm-2">
              <input id="SellerIdInput" type="number" placeholder="Seller ID" class="form-control" runat="server"/>
            </div>
          </div>
    
            <br />

                  <div class="form-group">
            <label class="col-sm-1 control-label">Buyer Team ID</label>
            <div class="col-sm-2">
              <input id="BuyerTeamIdInput" type="number" placeholder="Buyer Team ID" class="form-control" runat="server"/>        
            </div>

            <label class="col-sm-1 col-sm-offset-1 control-label">Seller Team ID</label>
            <div class="col-sm-2">
              <input id="SellerTeamIdInput" type="number" placeholder="Seller Team ID" class="form-control" runat="server"/>
            </div>
          </div>

            <br/>

          <div class="form-group">
            <label class="col-sm-1 control-label">Security</label>
            <div class="col-sm-2">
                
                <asp:DropDownList ID="SecurityDropDownList" runat="server" CssClass="form-control">
                    <asp:ListItem Text="--" Value="" Selected="True"></asp:ListItem>
                </asp:DropDownList>

            </div>

            <label class="col-sm-1 col-sm-offset-1 control-label">Flagged</label>
            <div class="col-sm-2">
              <asp:DropDownList ID="FlaggedDropDownList" runat="server" CssClass="form-control">
                  <asp:ListItem Text="--" Value="" Selected="True"></asp:ListItem>
                  <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                  <asp:ListItem Text="No" Value="false"></asp:ListItem>
              </asp:DropDownList>
            </div>
          </div>
            
            <br/>

          <div class="row">
              <div class="btn-group col-sm-12"> 

                <asp:Button ID="CancelButton" class="btn btn-default" runat="server" Text="Cancel" OnClick="Page_Load" />
                <asp:Button ID="SubmitButton" class="btn btn-primary" runat="server" Text="Submit" OnClick="Submit_Click" />

            </div>              
          </div>

            <div id="TableDiv" runat="server">

            </div>
            
            </fieldset>
            </form>

</asp:Content>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AdminViews/MasterAdmin.Master" AutoEventWireup="true" CodeBehind="Override.aspx.cs" Inherits="Stockimulate.Views.AdminViews.Override" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

       <form runat="server">
        <fieldset>

        <!-- Form Name -->
          <legend><h1>Override Panel</h1></legend>

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
 

    </form>

</asp:Content>

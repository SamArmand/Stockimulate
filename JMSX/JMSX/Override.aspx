<%@ Page Title="" Language="C#" MasterPageFile="~/JMSXAdmin.Master" AutoEventWireup="true" CodeBehind="Override.aspx.cs" Inherits="JMSX.Override" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

       <form runat="server">
        <fieldset>

        <!-- Form Name -->
          <legend><h1>Override Panel</h1></legend>

        <div class="row">

            <asp:Button id="UpdatePrice1" class="btn btn-primary" runat="server" Text="Update" OnClick="UpdatePrice1_Click" />
            <asp:Button id="UpdatePrice2" class="btn btn-primary" runat="server" Text="Update" OnClick="UpdatePrice2_Click" />
            <asp:Button id="ToggleReportsEnabled" class="btn btn-primary" runat="server" Text="Toggle Reports" OnClick="ToggleReportsEnabled_Click" />

            <br />
            <br />

          </div>
        </fieldset>
 

    </form>

</asp:Content>

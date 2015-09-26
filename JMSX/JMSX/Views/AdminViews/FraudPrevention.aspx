<%@ Page Title="" Language="C#" MasterPageFile="~/Views/AdminViews/MasterAdmin.Master" AutoEventWireup="true" CodeBehind="FraudPrevention.aspx.cs" Inherits="Stockimulate.Views.AdminViews.FraudPrevention" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
            <legend><h1>Fraud Prevention</h1></legend>

            <div id="tableDiv" runat="server">
                
                

            </div>
    <asp:ListView ID="ListView1" runat="server" DataSourceID="SqlDataSource"></asp:ListView>

</asp:Content>

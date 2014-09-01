<%@ Page Title="" Language="C#" MasterPageFile="~/JMSXTeam.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="JMSX.Reports" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="http://yui.yahooapis.com/pure/0.5.0/pure-min.css"/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

      <div class="row">
    <div class="col-md-12">
      <form class="form-horizontal" role="form" runat="server">
        <fieldset>

          <!-- Form Name -->
          <legend><h1>View Reports</h1></legend>
            
          <!-- Text input-->
          <div class="form-group">
            <label class="col-sm-1 control-label" for="textinput">Team Number</label>
            <div class="col-sm-2">
              <input id="TeamNumber" type="number" placeholder="Team Number" class="form-control" runat="server"/>
            </div>

            <label class="col-sm-1 col-sm-offset-1 control-label" for="textinput">Team Code</label>
            <div class="col-sm-2">
              <input id="TeamCode" type="text" placeholder="Team Code" class="form-control" runat="server"/>
            </div>
          </div>

      <br/>

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

    <div id="ErrorDiv" runat="server" class="alert alert-danger bg-danger col-lg-5" style="display: none;">
        <a href="#" class="close" data-dismiss="alert">&times;</a>
        <strong>Error!</strong> No such Team ID/Code combination exists.
    </div>

    <br />

    <div id="TeamTable" runat="server" style="display: none;">
    <h3 id ="TeamNameHeader" runat="server"></h3>
    <table class="pure-table pure-table-bordered">
    <thead>
        <tr>
            <th>Security</th>
            <th>Position</th>
            <th>Current Price</th>
            <th>PnL</th>
        </tr>
    </thead>

    <tbody>
        <tr>
            <td>Security1 (SEC1)</td>
            <td id="TeamPosition1Data" runat="server"></td>
            <td id="TeamIndex1PriceData" runat="server"></td>
            <td id="TeamIndex1PnLData" runat="server"></td>
        </tr>

       <tr>
            <td>Security2 (SEC2)</td>
            <td id="TeamPosition2Data" runat="server"></td>
            <td id="TeamIndex2PriceData" runat="server"></td>
            <td id="TeamIndex2PnLData" runat="server"></td>
        </tr>

       <tr>
            <td>Funds</td>
            <td></td>
            <td></td>
            <td id="TeamFundsData" runat="server"></td>
        </tr>

        <tr>
            <td><strong>Total</strong></td>
            <td></td>
            <td></td>
            <td id="TeamTotalPnLData" runat="server"></td>
        </tr>

    </tbody>
</table>
        </div>

        <div id="Player1Table" runat="server" style="display: none;">
    <h3 id ="Player1NameHeader" runat="server"></h3>
    <table class="pure-table pure-table-bordered">
    <thead>
        <tr>
            <th>Security</th>
            <th>Position</th>
            <th>Current Price</th>
            <th>PnL</th>
        </tr>
    </thead>

    <tbody>
        <tr>
            <td>Security1 (SEC1)</td>
            <td id="Player1Position1Data" runat="server"></td>
            <td id="Player1Index1PriceData" runat="server"></td>
            <td id="Player1Index1PnLData" runat="server"></td>
        </tr>

       <tr>
            <td>Security2 (SEC2)</td>
            <td id="Player1Position2Data" runat="server"></td>
            <td id="Player1Index2PriceData" runat="server"></td>
            <td id="Player1Index2PnLData" runat="server"></td>
        </tr>

       <tr>
            <td>Funds</td>
            <td></td>
            <td></td>
            <td id="Player1FundsData" runat="server"></td>
        </tr>

        <tr>
            <td><strong>Total</strong></td>
            <td></td>
            <td></td>
            <td id="Player1TotalPnLData" runat="server"></td>
        </tr>

    </tbody>
</table>
        </div>

    <div id="Player2Table" runat="server" style="display: none;">
        <h3 id="Player2NameHeader" runat="server"></h3>
        <table class="pure-table pure-table-bordered">
            <thead>
                <tr>
                    <th>Security</th>
                    <th>Position</th>
                    <th>Current Price</th>
                    <th>PnL</th>
                </tr>
            </thead>

            <tbody>
                <tr>
                    <td>Security1 (SEC1)</td>
                    <td id="Player2Position1Data" runat="server"></td>
                    <td id="Player2Index1PriceData" runat="server"></td>
                    <td id="Player2Index1PnLData" runat="server"></td>
                </tr>

                <tr>
                    <td>Security2 (SEC2)</td>
                    <td id="Player2Position2Data" runat="server"></td>
                    <td id="Player2Index2PriceData" runat="server"></td>
                    <td id="Player2Index2PnLData" runat="server"></td>
                </tr>

                <tr>
                    <td>Funds</td>
                    <td></td>
                    <td></td>
                    <td id="Player2FundsData" runat="server"></td>
                </tr>

                <tr>
                    <td><strong>Total</strong></td>
                    <td></td>
                    <td></td>
                    <td id="Player2TotalPnLData" runat="server"></td>
                </tr>

            </tbody>
        </table>
    </div>

     <div id="Player3Table" runat="server" style="display: none;">
        <h3 id="Player3NameHeader" runat="server"></h3>
        <table class="pure-table pure-table-bordered">
            <thead>
                <tr>
                    <th>Security</th>
                    <th>Position</th>
                    <th>Current Price</th>
                    <th>PnL</th>
                </tr>
            </thead>

            <tbody>
                <tr>
                    <td>Security1 (SEC1)</td>
                    <td id="Player3Position1Data" runat="server"></td>
                    <td id="Player3Index1PriceData" runat="server"></td>
                    <td id="Player3Index1PnLData" runat="server"></td>
                </tr>

                <tr>
                    <td>Security2 (SEC2)</td>
                    <td id="Player3Position2Data" runat="server"></td>
                    <td id="Player3Index2PriceData" runat="server"></td>
                    <td id="Player3Index2PnLData" runat="server"></td>
                </tr>

                <tr>
                    <td>Funds</td>
                    <td></td>
                    <td></td>
                    <td id="Player3FundsData" runat="server"></td>
                </tr>

                <tr>
                    <td><strong>Total</strong></td>
                    <td></td>
                    <td></td>
                    <td id="Player3TotalPnLData" runat="server"></td>
                </tr>

            </tbody>
        </table>
    </div>

         <div id="Player4Table" runat="server" style="display: none;">
        <h3 id="Player4NameHeader" runat="server"></h3>
        <table class="pure-table pure-table-bordered">
            <thead>
                <tr>
                    <th>Security</th>
                    <th>Position</th>
                    <th>Current Price</th>
                    <th>PnL</th>
                </tr>
            </thead>

            <tbody>
                <tr>
                    <td>Security1 (SEC1)</td>
                    <td id="Player4Position1Data" runat="server"></td>
                    <td id="Player4Index1PriceData" runat="server"></td>
                    <td id="Player4Index1PnLData" runat="server"></td>
                </tr>

                <tr>
                    <td>Security2 (SEC2)</td>
                    <td id="Player4Position2Data" runat="server"></td>
                    <td id="Player4Index2PriceData" runat="server"></td>
                    <td id="Player4Index2PnLData" runat="server"></td>
                </tr>

                <tr>
                    <td>Funds</td>
                    <td></td>
                    <td></td>
                    <td id="Player4FundsData" runat="server"></td>
                </tr>

                <tr>
                    <td><strong>Total</strong></td>
                    <td></td>
                    <td></td>
                    <td id="Player4TotalPnLData" runat="server"></td>
                </tr>

            </tbody>
        </table>
    </div>

</asp:Content>

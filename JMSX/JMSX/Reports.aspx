<%@ Page Title="" Language="C#" MasterPageFile="~/JMSXTeam.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="JMSX.Reports" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/tables.css" rel="stylesheet" />
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
              <input id="TeamNumber" type="number" placeholder="Team Number" class="form-control" runat="server" required/>
            </div>

            <label class="col-sm-1 col-sm-offset-1 control-label" for="textinput">Team Code</label>
            <div class="col-sm-2">
              <input id="TeamCode" type="text" placeholder="Team Code" class="form-control" runat="server" required/>
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

    <div id="InfoDiv" runat="server" class="alert alert-info bg-info col-lg-5" style="display: none;">
        <a href="#" class="close" data-dismiss="alert">&times;</a>
        <strong>Info.</strong> The quarterly reports system is not yet open.
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
            <th>Value</th>
        </tr>
    </thead>

    <tbody>
        <tr>
            <td>OIL</td>
            <td id="TeamPosition1Data" runat="server"></td>
            <td id="TeamIndex1PriceData" runat="server"></td>
            <td id="TeamIndex1ValueData" runat="server"></td>
        </tr>

       <tr>
            <td>IND</td>
            <td id="TeamPosition2Data" runat="server"></td>
            <td id="TeamIndex2PriceData" runat="server"></td>
            <td id="TeamIndex2ValueData" runat="server"></td>
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
            <td id="TeamTotalValueData" runat="server"></td>
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
            <th>Value</th>
        </tr>
    </thead>

    <tbody>
        <tr>
            <td>OIL</td>
            <td id="Player1Position1Data" runat="server"></td>
            <td id="Player1Index1PriceData" runat="server"></td>
            <td id="Player1Index1ValueData" runat="server"></td>
        </tr>

       <tr>
            <td>IND</td>
            <td id="Player1Position2Data" runat="server"></td>
            <td id="Player1Index2PriceData" runat="server"></td>
            <td id="Player1Index2ValueData" runat="server"></td>
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
            <td id="Player1TotalValueData" runat="server"></td>
        </tr>

        <tr>
            <td><strong>PnL</strong></td>
            <td></td>
            <td></td>
            <td id="Player1PnLData" runat="server"></td>
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
                    <th>Value</th>
                </tr>
            </thead>

            <tbody>
                <tr>
                    <td>OIL</td>
                    <td id="Player2Position1Data" runat="server"></td>
                    <td id="Player2Index1PriceData" runat="server"></td>
                    <td id="Player2Index1ValueData" runat="server"></td>
                </tr>

                <tr>
                    <td>IND</td>
                    <td id="Player2Position2Data" runat="server"></td>
                    <td id="Player2Index2PriceData" runat="server"></td>
                    <td id="Player2Index2ValueData" runat="server"></td>
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
                    <td id="Player2TotalValueData" runat="server"></td>
                </tr>

                        <tr>
            <td><strong>PnL</strong></td>
            <td></td>
            <td></td>
            <td id="Player2PnLData" runat="server"></td>
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
                    <th>Value</th>
                </tr>
            </thead>

            <tbody>
                <tr>
                    <td>OIL</td>
                    <td id="Player3Position1Data" runat="server"></td>
                    <td id="Player3Index1PriceData" runat="server"></td>
                    <td id="Player3Index1ValueData" runat="server"></td>
                </tr>

                <tr>
                    <td>IND</td>
                    <td id="Player3Position2Data" runat="server"></td>
                    <td id="Player3Index2PriceData" runat="server"></td>
                    <td id="Player3Index2ValueData" runat="server"></td>
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
                    <td id="Player3TotalValueData" runat="server"></td>
                </tr>

                        <tr>
            <td><strong>PnL</strong></td>
            <td></td>
            <td></td>
            <td id="Player3PnLData" runat="server"></td>
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
                    <th>Value</th>
                </tr>
            </thead>

            <tbody>
                <tr>
                    <td>OIL</td>
                    <td id="Player4Position1Data" runat="server"></td>
                    <td id="Player4Index1PriceData" runat="server"></td>
                    <td id="Player4Index1ValueData" runat="server"></td>
                </tr>

                <tr>
                    <td>IND</td>
                    <td id="Player4Position2Data" runat="server"></td>
                    <td id="Player4Index2PriceData" runat="server"></td>
                    <td id="Player4Index2ValueData" runat="server"></td>
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
                    <td id="Player4TotalValueData" runat="server"></td>
                </tr>

                        <tr>
            <td><strong>PnL</strong></td>
            <td></td>
            <td></td>
            <td id="Player4PnLData" runat="server"></td>
        </tr>

            </tbody>
        </table>
    </div>

</asp:Content>

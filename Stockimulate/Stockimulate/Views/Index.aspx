<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Stockimulate.Views.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>&nbsp;</title>

    <script src="../Scripts/jquery-3.1.0.min.js"></script>    
    <script src="../Scripts/jquery.signalR-2.2.0.min.js"></script>
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/highcharts/4.2.0/highcharts.js"></script>
    <script src="../signalr/hubs"></script>

    <%-- ReSharper disable PossiblyUnassignedProperty --%>
    <script>
        $(function () {
            var text = $('#DataDiv').text();

            $('#GraphDiv').highcharts({

                title: {
                    text: $('#IndexNameSymbolDiv').text()
                },

                legend: {
                    enabled: false
                },

                plotOptions: {
                    line: {
                        marker: {
                            enabled: false
                        }
                    }
                },

                xAxis: {
                    title: {
                        text: 'Trading Day'
                    },

                    min: 0,
                    max: 252
                },

                yAxis: {

                    title: {
                        text: 'Price'
                    },

                    min: 0,
                    max: 4000
                },

                series: [{
                    color: '#000000',
                    data: eval(text)
                }]

            });

            var sim = $.connection.simulator;

            // Declare a function on the  hub so the server can invoke it          

            sim.client.sendMessage = function (message) {

                var id = $("#IndexDivId").text();
                
                var currentPrice = $("#IndexDivId").text();
                var effect = message[2 + id];
                currentPrice += effect;
                
                $('#GraphDiv').highcharts().series[0].addPoint(message[0], currentPrice);
                
                $(".IndexPriceDiv").html("<h2>$" + currentPrice + "</h2>");

                $('.IndexChangePositive').hide();
                $('.IndexChangeNegative').hide();
                $('.IndexChangeNone').hide();

                if (effect > 0) {
                    $('.IndexChangePositiveH1').html(effect);
                    $('.IndexChangePositive').show();
                }

                else if (effect < 0) {
                    $('.IndexChangeNegativeH1').html(effect*-1);
                    $('.IndexChangeNegative').show();
                }

                else {
                    $('.IndexChangeNone').show();
                }

                if (message[1] !== "") {
                    $('.NewsDiv').html("<h2>" + message[1] + "</h2>");
                }

            };

            $.connection.hub.start().done(function () {

            });


        });

    </script>
    <%-- ReSharper restore PossiblyUnassignedProperty --%>

    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
</head>
<body>

    <div class="container-fluid">
    
            
        <div class="col-sm-12">
            <img style="max-height: 100px;" src="../img/JMSX.png" alt="JMSX.png"/>

            </div>
        
            <div class="col-sm-10" id="GraphDiv">
    
                
            </div>

            
            <div class="col-sm-2">
    
                <div class="col-sm-12 bg-primary IndexPriceDiv" id="IndexPriceDiv" style="text-align:center" runat="server">

                </div>

                <div class="col-sm-12 bg-success IndexChangePositive" id="IndexChangePositiveDiv" style="display:none;text-align:center" runat="server">
    
                    <h1 style="display:inline-block"><span class="glyphicon glyphicon-arrow-up"></span></h1>

                    <h1 style="display:inline-block" class="IndexChangePositiveH1" id="IndexChangePositiveH1" runat="server"></h1>

                </div>

                <div class="col-sm-12 bg-danger IndexChangeNegative" id="IndexChangeNegativeDiv" style="display:none;text-align:center" runat="server">
    
                    <h1 style="display:inline-block"><span class="glyphicon glyphicon-arrow-down"></span></h1>

                    <h1 style="display:inline-block" class="IndexChangeNegativeH1" id="IndexChangeNegativeH1" runat="server"></h1>

                </div>

                <div class="col-sm-12 bg-warning IndexChangeNone" id="IndexChangeNoneDiv" style="display:none;text-align:center" runat="server">
    
                    <h1 style="display:inline-block"><span class="glyphicon glyphicon-resize-horizontal"></span></h1>

                    <h1 style="display:inline-block" class="IndexChangeNoneH1" id="IndexChangeNoneH1" runat="server">0</h1>

                </div>

           </div>

                            <div class="col-sm-12 NewsDiv" id="NewsDiv" runat="server">

</div>

</div>

    <div id="DataDiv" style="display:none;" runat="server"></div>
    <div id="IndexNameSymbolDiv" style="display:none;" runat="server"></div>

    <div id="IndexIdDiv" style="display:none;" runat="server"></div>

</body>
</html>

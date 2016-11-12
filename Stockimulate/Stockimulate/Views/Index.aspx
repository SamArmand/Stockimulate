<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Stockimulate.Views.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>&nbsp;</title>

    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.1.1/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/tether/1.3.7/js/tether.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/signalr.js/2.2.1/jquery.signalR.min.js"></script>
    <%-- ReSharper disable once Html.PathError --%>
    <script src="/signalr/hubs"></script>
      
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0-alpha.5/js/bootstrap.min.js" integrity="sha384-BLiI7JTZm+JWlgKa0M0kGRpJbF2J8q+qreVrKBC47e3K6BW78kGLrCkeRX6I9RoK" crossorigin="anonymous"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/highcharts/5.0.0/highcharts.js"></script>

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

                    min: 0
                },

                yAxis: {

                    title: {
                        text: 'Price'
                    }
                },

                series: [{
                    color: '#000000',
                    data: eval(text)
                }]

            });

            var sim = $.connection.simulator;

            // Declare a function on the  hub so the server can invoke it          

            sim.client.sendMessage = function (message) {

                var id = parseInt($("#IndexIdDiv").text());
                
                var currentPrice = parseInt($("#IndexPriceDiv").text().substring(1));
                var effect = parseInt(message[3 + id]);
                currentPrice += effect;
                
                $('#GraphDiv').highcharts().series[0].addPoint([parseInt(message[0]), currentPrice], true, false);
                
                $("#IndexPriceDiv").html("<h1>$" + currentPrice + "</h1>");

                
                $("#IndexChangeDiv").removeClass("bg-success");
                $("#IndexChangeDiv").removeClass("bg-danger");
                $("#IndexChangeDiv").removeClass("bg-warning");

                if (effect > 0) {
                    $("#IndexChangeDiv").addClass("bg-success");
                    $('#IndexChangeH1').html("+" + effect);
                }

                else if (effect < 0) {
                    $("#IndexChangeDiv").addClass("bg-danger");
                    $('#IndexChangeH1').html(effect);
                }

                else {
                    $("#IndexChangeDiv").addClass("bg-warning");
                    $('#IndexChangeH1').html("+" + effect);
                }

                if (message[1] !== "")
                    $('#NewsDiv').html("<h2>" + message[1] + "</h2>");


                if (message[2] === "closed") {
                    $("#StatusDiv").removeClass("bg-success");
                    $("#StatusDiv").addClass("bg-danger");
                    $("#StatusSpan").html("CLOSED");
                }

                else
                    $("#DaySpan").html(parseInt(message[0]) + 1);


            };

            sim.client.openMarket = function () {

                $("#StatusDiv").removeClass("bg-danger");
                $("#StatusDiv").addClass("bg-success");
                $("#StatusSpan").html("OPEN");

                $("#DaySpan").html(parseInt($("#DaySpan").html() + 1));
                $("#QuarterSpan").html(parseInt($("#QuarterSpan").html()) + 1);

            };

            $.connection.hub.start().done(function () {

            });


        });

    </script>
    <%-- ReSharper restore PossiblyUnassignedProperty --%>
     <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0-alpha.5/css/bootstrap.min.css" integrity="sha384-AysaV+vQoT3kOAXZkl02PThvDr8HYKPZhNT5h/CXfBThSRXQ6jW5DO2ekP5ViFdi" crossorigin="anonymous"/>
</head>
<body>

    <div class="container-fluid">
    
            
        <div class="col-sm-12">
            <img style="max-height: 100px;" src="/img/JMSX.png" alt="JMSX.png"/>

            </div>
        
            <div class="col-sm-10" id="GraphDiv">
    
                
            </div>

            
            <div class="col-sm-2">
    
                <div class="col-sm-12 bg-primary IndexPriceDiv text-white" id="IndexPriceDiv" style="text-align:center" runat="server">

                </div>

                <div class="col-sm-12 bg-warning text-white" id="IndexChangeDiv" style="display:block;text-align:center" runat="server">

                    <h1 style="display:inline-block" id="IndexChangeH1" runat="server">0</h1>

                </div>

           </div>
        
            <div class="text-white" id="StatusDiv" runat="server">
    
                <h1>Q<span id="QuarterSpan" runat="server"></span> | Day <span id="DaySpan" runat="server"></span> | Market <span id="StatusSpan" runat="server"></span> </h1>
            </div>

           <div class="col-sm-12" id="NewsDiv" runat="server"></div>

</div>

    <div id="DataDiv" style="display:none;" runat="server"></div>
    <div id="IndexNameSymbolDiv" style="display:none;" runat="server"></div>

    <div id="IndexIdDiv" style="display: none;" runat="server"></div>
</body>
</html>

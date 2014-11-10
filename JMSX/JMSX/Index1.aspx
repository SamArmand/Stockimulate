﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index1.aspx.cs" Inherits="JMSX.Index1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script src="scripts/jquery-1.9.0.min.js"></script>    
    <script src="scripts/jquery.signalR-2.1.2.min.js"></script>
    <script src="scripts/bootstrap.min.js"></script>
    <script src="scripts/Highcharts-4.0.1/js/highcharts.js"></script>
    <script src="/signalr/hubs"></script>

    <script>
        $(function () {
            var text = $('#data').text();

            $('#GraphDiv').highcharts({

                title: {
                    text: 'OIL'
                },

                xAxis: {
                    title: {
                        text: 'Trading Day'
                    },

                    min: 0,
                    max: 252,
                },

                yAxis: {

                    title: {
                        text: 'Price'
                    },

                    min: 0,
                    max: 3000,
                },

                series: [{
                    data: eval(text)
                }]

            });

            var sim = $.connection.simulator

            // Declare a function on the chat hub so the server can invoke it          

            sim.client.sendMessage = function (price1, price2, day, change1, change2, news) {
                $('#GraphDiv').highcharts().series[0].addPoint([day, price1]);
                
                $(".IndexPriceDiv").html("<h2>$" + price1 + "</h2>");

                $('.IndexChangePositive').hide();
                $('.IndexChangeNegative').hide();
                $('.IndexChangeNone').hide();

                if (change1 > 0) {
                    $('.IndexChangePositiveSpan').html(change1);
                    $('.IndexChangePositive').show();
                }

                else if (change1 < 0) {
                    $('.IndexChangeNegativeSpan').html(change1*-1);
                    $('.IndexChangeNegative').show();
                }

                else if (change1 == 0) {
                    $('.IndexChangeNoneSpan').html(change1);
                    $('.IndexChangeNone').show();
                }

                if (news != "null") {
                    $('.NewsDiv').html("<h2>" + news + "</h2>");
                }

            };

            $.connection.hub.start().done(function () {

            });


        });

    </script>

    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/bootstrap-theme.css" rel="stylesheet" />
</head>
<body>

    <div class="container-fluid">
    
            
                    <div class="col-sm-12">
    <img style="max-height: 100px;" src="img/JMSX.png"/>

</div>
        
        <div class="col-sm-10" id="GraphDiv">
    
                
</div>

            
            <div class="col-sm-2">
    
                <div class="col-sm-12 bg-primary IndexPriceDiv" id="IndexPriceDiv" style="text-align:center" runat="server">

                </div>

                <div class="col-sm-12 bg-success IndexChangePositive" id="IndexChangePositive" style="display:none;text-align:center" runat="server">
    
                    <h1 style="display:inline-block"><span class="glyphicon glyphicon-arrow-up"></span></h1>

                    <h1 style="display:inline-block" class="IndexChangePositiveSpan" id="IndexChangePositiveSpan" runat="server"></h1>

                </div>

                <div class="col-sm-12 bg-danger IndexChangeNegative" id="IndexChangeNegative" style="display:none;text-align:center" runat="server">
    
                    <h1 style="display:inline-block"><span class="glyphicon glyphicon-arrow-down"></span></h1>

                    <h1 style="display:inline-block" class="IndexChangeNegativeSpan" id="IndexChangeNegativeSpan" runat="server"></h1>

                </div>

                <div class="col-sm-12 bg-warning IndexChangeNone" id="IndexChangeNone" style="display:none;text-align:center" runat="server">
    
                    <h1 style="display:inline-block"><span class="glyphicon glyphicon-resize-horizontal"></span></h1>

                    <h1 style="display:inline-block" class="IndexChangeNoneSpan" id="IndexChangeNoneSpan" runat="server"></h1>

                </div>

           </div>

                            <div class="col-sm-12 NewsDiv" id="NewsDiv" runat="server">

</div>

</div>

    <div id="data" style="display:none;" runat="server"></div>


    

</body>
</html>

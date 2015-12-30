<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Stockimulate.Views.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>&nbsp;</title>

    <script src="../Scripts/jquery-2.1.4.min.js"></script>    
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

            // Declare a function on the chat hub so the server can invoke it          

            sim.client.sendMessage = function (price1, price2, day, change1, change2, news) {
                $('#GraphDiv').highcharts().series[0].addPoint([day, price1]);
                
                $(".IndexPriceDiv").html("<h2>$" + price1 + "</h2>");

                $('.IndexChangePositive').hide();
                $('.IndexChangeNegative').hide();
                $('.IndexChangeNone').hide();

                if (change1 > 0) {
                    $('.IndexChangePositiveH1').html(change1);
                    $('.IndexChangePositive').show();
                }

                else if (change1 < 0) {
                    $('.IndexChangeNegativeH1').html(change1*-1);
                    $('.IndexChangeNegative').show();
                }

                else if (change1 === 0) {
                    $('.IndexChangeNoneH1').html(change1);
                    $('.IndexChangeNone').show();
                }

                if (news !== "") {
                    $('.NewsDiv').html("<h2>" + news + "</h2>");
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

    

</body>
</html>

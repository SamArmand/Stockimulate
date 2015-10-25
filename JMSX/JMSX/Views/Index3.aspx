<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index3.aspx.cs" Inherits="Stockimulate.Views.Index3" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <title>&nbsp;</title>

    <script src="../scripts/jquery-2.1.4.min.js"></script>
    <script src="../scripts/bootstrap.min.js"></script>
    <script src="../scripts/Highcharts-4.0.1/js/highcharts.js"></script>
    <script src="../scripts/jquery.signalR-2.2.0.min.js"></script>
    <script src="../signalr/hubs"></script>

    <%-- ReSharper disable PossiblyUnassignedProperty --%>
    <script>
        $(function () {
            var text = $('#DataDiv').text();
            $('#GraphDiv').highcharts({

                title: {
                    text: "Medici Bank of Quebec (MBQ)"
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

            sim.client.sendMessage = function (price1, price2, price3, day, change1, change2, change3, news) {
                $('#GraphDiv').highcharts().series[0].addPoint([day, price3]);
                
                $(".IndexPriceDiv").html("<h2>$" + price3 + "</h2>");

                $('.IndexChangePositive').hide();
                $('.IndexChangeNegative').hide();
                $('.IndexChangeNone').hide();

                if (change3 > 0) {
                    $('.IndexChangePositiveSpan').html(change3);
                    $('.IndexChangePositive').show();
                }

                else if (change3 < 0) {
                    $('.IndexChangeNegativeSpan').html(change3*-1);
                    $('.IndexChangeNegative').show();
                }

                else if (change3 === 0) {
                    $('.IndexChangeNoneSpan').html(change3);
                    $('.IndexChangeNone').show();
                }

                if (news !== "") {
                    $(".NewsDiv").html("<h2>" + news + "</h2>");
                }

            };

            $.connection.hub.start().done(function () {

            });


        });

    </script>
    <%-- ReSharper restore PossiblyUnassignedProperty --%>

    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <link href="../Content/bootstrap-theme.css" rel="stylesheet" />
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

                    <h1 style="display:inline-block" class="IndexChangePositiveSpan" id="IndexChangePositiveH1" runat="server"></h1>

                </div>

                <div class="col-sm-12 bg-danger IndexChangeNegative" id="IndexChangeNegativeDiv" style="display:none;text-align:center" runat="server">
    
                    <h1 style="display:inline-block"><span class="glyphicon glyphicon-arrow-down"></span></h1>

                    <h1 style="display:inline-block" class="IndexChangeNegativeSpan" id="IndexChangeNegativeH1" runat="server"></h1>

                </div>

                <div class="col-sm-12 bg-warning IndexChangeNone" id="IndexChangeNoneDiv" style="display:none;text-align:center" runat="server">
    
                    <h1 style="display:inline-block"><span class="glyphicon glyphicon-resize-horizontal"></span></h1>

                    <h1 style="display:inline-block" class="IndexChangeNoneSpan" id="IndexChangeNoneH1" runat="server">0</h1>

                </div>

           </div>

                            <div class="col-sm-12 NewsDiv" id="NewsDiv" runat="server">

</div>

</div>

    <div id="DataDiv" style="display:none;" runat="server"></div>


</body>
</html>

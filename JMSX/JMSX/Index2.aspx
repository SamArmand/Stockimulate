<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index2.aspx.cs" Inherits="JMSX.Index2" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script src="scripts/jquery-1.9.0.min.js"></script>
    <script src="scripts/bootstrap.min.js"></script>
    <script src="scripts/Highcharts-4.0.1/js/highcharts.js"></script>
    <script src="scripts/jquery.signalR-2.1.2.min.js"></script>
    <script src="/signalr/hubs"></script>

    <script>
        $(function () {
            var text = $('#data').text();
            $('#GraphDiv').highcharts({

                xAxis: {
                    min: 0,
                    max: 252,
                },

                series: [{
                    data: eval(text)
                }]

            });

            var sim = $.connection.simulator

            // Declare a function on the chat hub so the server can invoke it          

            sim.client.sendMessage = function (price1, price2, day, change1, change2, news) {
                $('#GraphDiv').highcharts().series[0].addPoint([day, price2]);
                
                $(".IndexPriceDiv").html("<h2>$" + price2 + "</h2>");

                $('.IndexChangePositive').hide();
                $('.IndexChangeNegative').hide();
                $('.IndexChangeNone').hide();

                if (change2 > 0) {
                    $('.IndexChangePositiveSpan').html(change2);
                    $('.IndexChangePositive').show();
                }

                else if (change2 < 0) {
                    $('.IndexChangeNegativeSpan').html(change2*-1);
                    $('.IndexChangeNegative').show();
                }

                else if (change2 == 0) {
                    $('.IndexChangeNoneSpan').html(change2);
                    $('.IndexChangeNone').show();
                }

                if (news != "null") {
                    $(".NewsDiv").html("<h2>" + news + "</h2>");
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
    
            
                    <div class="col-lg-12">
    <img style="max-height: 100px;" src="img/JMSX.png"/>

</div>
        
        <div class="col-lg-10 bg-info" id="GraphDiv">
    
           
</div>

            
            <div class="col-lg-2">
    
                <div class="col-lg-12 bg-primary IndexPriceDiv" id="IndexPriceDiv" runat="server">

                </div>

                <div class="col-lg-12 bg-success IndexChangePositive" id="IndexChangePositive" style="display:none;" runat="server">
    
                    <h2><span class="glyphicon glyphicon-arrow-up"></span></h2>

                    <h2 class="IndexChangePositiveSpan" id="IndexChangePositiveSpan" runat="server"></h2>

                </div>

                <div class="col-lg-12 bg-danger IndexChangeNegative" id="IndexChangeNegative" style="display:none;" runat="server">
    
                    <h2><span class="glyphicon glyphicon-arrow-down"></span></h2>

                    <h2 class="IndexChangeNegativeSpan" id="IndexChangeNegativeSpan" runat="server"></h2>

                </div>

                <div class="col-lg-12 bg-warning IndexChangeNone" id="IndexChangeNone" style="display:none;" runat="server">
    
                    <h2><span class="glyphicon glyphicon-resize-horizontal"></span></h2>

                    <h2 class="IndexChangeNoneSpan" id="IndexChangeNoneSpan" runat="server"></h2>

                </div>


           </div>

                            <div class="col-lg-12 NewsDiv" id="NewsDiv" runat="server">
    <h2>News Item Here</h2>

</div>

</div>

    <div id="data" style="display:none;" runat="server"></div>


</body>
</html>

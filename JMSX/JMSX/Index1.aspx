<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index1.aspx.cs" Inherits="JMSX.Index1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script src="scripts/jquery-1.9.0.min.js"></script>    
    <script src="scripts/jquery.signalR-2.1.2.min.js"></script>
    <script src="scripts/bootstrap.min.js"></script>
    <script src="scripts/Highcharts-4.0.1/js/highcharts.js"></script>
    <script src="/signalr/hubs"></script>


    <script type="text/javascript">
    $(function () {
        // Proxy created on the fly          
        var sim = $.connection.simulator;

        // Declare a function on the chat hub so the server can invoke it          
        sim.client.sendMessage = function () {
            location.reload();
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
    
                <asp:Literal ID="Graph" runat="server"></asp:Literal>
</div>

            
            <div class="col-lg-2">
    
                <div class="col-lg-12 bg-primary" id="IndexPriceDiv" runat="server">
    


                </div>

                <div class="col-lg-12 bg-success" id="IndexChangePositive" style="display:none;" runat="server">
    
                    <span class="glyphicon glyphicon-arrow-up" id="IndexChangePositiveSpan" runat="server"></span>

                </div>

                <div class="col-lg-12 bg-danger" id="IndexChangeNegative" style="display:none;" runat="server">
    
                    <span class="glyphicon glyphicon-arrow-down" id="IndexChangeNegativeSpan" runat="server"></span>

                </div>

                <div class="col-lg-12 bg-warning" id="IndexChangeNone" style="display:none;" runat="server">
    
                    <span class="glyphicon glyphicon-resize-horizontal" id="IndexChangeNoneSpan" runat="server"></span>

                </div>


           </div>

                            <div class="col-lg-12" id="NewsDiv" runat="server">
    <h2>News Item Here</h2>

</div>

</div>

    <form runat="server">

        <asp:Button ID="HiddenRefresh" runat="server" style="display: none;" OnClick="Page_Load"/>
    </form>

    

</body>
</html>

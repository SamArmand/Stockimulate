<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index2.aspx.cs" Inherits="JMSX.Index2" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script src="scripts/jquery-1.9.0.min.js"></script>
    <script src="scripts/bootstrap.min.js"></script>
    <script src="scripts/Highcharts-4.0.1/js/highcharts.js"></script>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/bootstrap-theme.css" rel="stylesheet" />
</head>
<body>

    <div class="container-fluid">
    
            
                    <div class="col-lg-12">
    <img style="max-height: 100px;" src="img/JMSX.png"/>

</div>
        
        <div class="col-lg-10 bg-info" id="GraphDiv">
    
            Graph goes here

            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
</div>

            
            <div class="col-lg-2">
    
                <div class="col-lg-12 bg-primary" id="IndexPriceDiv" runat="server">
    


                </div>

                <div class="col-lg-12 bg-success" id="IndexChangePositive" style="display:none;" runat="server">
    


                </div>

                <div class="col-lg-12 bg-success" id="IndexChangeNegative" style="display:none;" runat="server">
    


                </div>

                <div class="col-lg-12 bg-warning" id="IndexChangeNone" style="display:none;" runat="server">
    


                </div>


           </div>

                            <div class="col-lg-12" id="NewsDiv" runat="server">
    <h2>News Item Here</h2>

</div>

</div>

</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Stockimulate.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script src="scripts/jquery-1.9.0.min.js"></script>    
    <script src="scripts/bootstrap.min.js"></script>


    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/bootstrap-theme.css" rel="stylesheet" />
    <link href="Content/signin.css" rel="stylesheet" />
</head>
<body>

    <div class="container">
    
            
                    <div class="col-lg-12">
    <img style="max-height: 100px;" src="img/JMSX.png"/>

</div>
        

      <form class="form-signin" role="form" runat="server">
        <h2 class="form-signin-heading">Please sign in</h2>
        <input id="user" type="text" class="form-control" placeholder="Username" runat="server" required autofocus/>
        <input id="password" type="password" class="form-control" placeholder="Password" runat="server" required />
        <asp:Button ID="signIn" OnClick="signIn_Click" class="btn btn-lg btn-primary btn-block" type="submit" runat="server" Text="Sign In"></asp:Button>
      </form>
            

</div>

   


    

</body>
</html>

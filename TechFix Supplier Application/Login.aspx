<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TechFix_Supplier_Application.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>TechFix Supplier Login</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="CSS/loginStyle.css" rel="stylesheet" />
</head>
<body>
    <div class="container">
        <img src="images/logo.png" />
        <form id="loginform" runat="server">
            <asp:TextBox ID="txtUsername" runat="server" CssClass="custom-txtbox" placeholder="Username"></asp:TextBox>
            <asp:TextBox ID="txtPassword" runat="server" CssClass="custom-txtbox" placeholder="Password" TextMode="Password"></asp:TextBox>
            <asp:Button ID="btnSubmit" runat="server" Text="LOG IN AS SUPPLIER" CssClass="custom-btn" OnClick="btnSubmit_Click" />
        </form>
        <asp:Label ID="lblErrorMessage" runat="server" Text="Error Message" CssClass="error-msg"></asp:Label>
    </div>
</body>
</html>

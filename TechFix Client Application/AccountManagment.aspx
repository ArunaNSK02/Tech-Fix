<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountManagment.aspx.cs" Inherits="TechFix_Client_Application.AccountManagment" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Shop Member Dashboard - TechFix</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" href="CSS/StyleSheet.css"/>
    <style>
        .container {
            width: 70%;
            margin-inline: auto;
            margin-block-start: 50px;
            display: grid;
            grid-template-columns: 1fr 1fr;
            padding-inline: 60px;
            padding-block: 40px;
            box-shadow: rgba(0, 0, 0, 0.25) 0px 0.0625em 0.0625em, rgba(0, 0, 0, 0.25) 0px 0.125em 0.5em, rgba(255, 255, 255, 0.1) 0px 0px 0px 1px inset;
        }

        .custom-txtbox {
            opacity: 0.5;
            margin-block-start: 5px;
            margin-block-end: 0;
        }

        .userform-label {
            display: block;
            font-weight: bold;
        }

        .server-response {
            opacity: 0.5;
            font-style: italic;
            margin-inline-start: 25px;
            font-weight:400;
        }

        .input-group {
            margin-block-end: 25px;
        }

        .custom-btn {
            margin-inline: auto;
            margin-block-start: 30px;
            grid-column: 1/3;
        }

        .error-msg {
            grid-column: 1/3;
            color: var(--color_two);
            display: block;
            margin-block-start: 20px;
            margin-inline: auto;
            width: fit-content;
            font-family: "Noto Sans", serif;
            font-size: 15px;
            opacity: 0.8;
            visibility: hidden;
        }

    </style>
        <script type="text/javascript">
            function checkFields() {
                var address = document.getElementById('<%= txtAddress.ClientID %>').value;
                var email = document.getElementById('<%= txtEmail.ClientID %>').value;
                var password = document.getElementById('<%= txtPassword.ClientID %>').value;
                var phoneNumber = document.getElementById('<%= txtPhoneNumber.ClientID %>').value;
                var btnSubmit = document.getElementById('<%= btnSubmit.ClientID %>');

                // Check if at least one field is filled
                if (address || email || password || phoneNumber) {
                    btnSubmit.disabled = false; // Enable button
                    btnSubmit.style.opacity = "1";
                    btnSubmit.style.cursor = "pointer";
                } else {
                    btnSubmit.disabled = true; // Disable button
                    btnSubmit.style.opacity = "0.5";
                    btnSubmit.style.cursor = "default";
                    
                }
            }
        </script>
</head>
<body>
    <form id="userform" runat="server">
    <div class="nav-bar">
        <a href="StaffDashboard.aspx"><img src="images/logo.png"/></a>
        <ul class="nav-menu">
            <li><a href="QuotationManagment.aspx" class="nav-link">Quotation Managment</a></li>
            <li><a href="OrderManagment.aspx" class="nav-link">Order Managment</a></li>
        </ul>
        <a id="profileBtn" runat="server" href="AccountManagment.aspx" class="user-profile" style="text-decoration: none;"></a>
        <asp:Button ID="btnLogout" runat="server" Text="LOG OUT" CssClass="logout-btn" OnClick="btnLogout_Click" />
    </div>
    <div class="container">
        <div class="input-group" style="grid-column: 1/2">
            <span class="userform-label">Name: </span>
            <asp:Label ID="lblName" runat="server" Text="" CssClass="server-response"></asp:Label>
        </div>

        <div class="input-group" style="grid-column: 2/3">
            <span class="userform-label">Username: </span>
            <asp:Label ID="lblUsername" runat="server" Text="" CssClass="server-response"></asp:Label>
        </div>
      
        <div class="input-group" style="grid-column: 1/2;">
             <span class="userform-label">Address: </span>
             <asp:TextBox ID="txtAddress" runat="server" CssClass="custom-txtbox" onkeyup="checkFields();"></asp:TextBox>
        </div>
            
        <div class="input-group" style="grid-column: 2/3;">
            <span class="userform-label">Email-Address: </span>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="custom-txtbox" onkeyup="checkFields();"></asp:TextBox>
        </div>

        <div class="input-group" style="grid-column: 1/2;">
            <span class="userform-label">New Password: </span>
            <asp:TextBox ID="txtPassword" runat="server" CssClass="custom-txtbox" onkeyup="checkFields();"></asp:TextBox>
        </div>
            
        <div class="input-group" style="grid-column: 2/3;">
            <span class="userform-label">Phone-Number: </span>
            <asp:TextBox ID="txtPhoneNumber" runat="server" CssClass="custom-txtbox" onkeyup="checkFields();"></asp:TextBox>
        </div>

        <div class="input-group" style="grid-column: 1/2">
            <span class="userform-label">Role: </span>
            <asp:Label ID="lblRole" runat="server" Text="" CssClass="server-response"></asp:Label>
        </div>

        <div class="input-group" style="grid-column: 2/3">
            <span class="userform-label">Memeber Since: </span>
            <asp:Label ID="lbljoinDate" runat="server" Text="" CssClass="server-response"></asp:Label>
        </div>

        <asp:Button ID="btnSubmit" runat="server" Text="Update Info." CssClass="custom-btn" OnClick="btnSubmit_Click" />

        <asp:Label ID="lblErrorMessage" runat="server" Text="Error Message" CssClass="error-msg"></asp:Label>
    </div>
    </form>
</body>
</html>

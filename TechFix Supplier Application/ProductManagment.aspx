<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductManagment.aspx.cs" Inherits="TechFix_Supplier_Application.ProductManagment" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Supplier Dashboard - TechFix</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="CSS/StyleSheet.css" rel="stylesheet" />
    <style>
        @import url('https://fonts.googleapis.com/css2?family=Noto+Sans:ital,wght@0,100..900;1,100..900&display=swap');

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

        .desc-box {
            font-family: "Noto Sans", serif;
            width: 100%;
            height: 150px;
            resize: none;
        }

        .userform-label {
            display: block;
            font-weight: bold;
        }

        .input-group {
            margin-block-end: 25px;
        }

        .custom-btn {
            margin-inline: auto;
            margin-block-start: 0;
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
</head>
<body>
    <form id="mainform" runat="server">
        <div class="nav-bar">
            <a href="SupplierDashboard.aspx"><img src="images/logo.png"/></a>
            <ul class="nav-menu">
                <li><a href="ProductManagment.aspx" class="nav-link">Product Managment</a></li>
                <li><a href="InventoryManagment.aspx" class="nav-link">Stock Managment</a></li>
                <li><a href="QuotationManagment.aspx" class="nav-link">Quotation Managment</a></li>
                <li><a class="nav-link">Order Managment</a></li>
            </ul>
            <a id="profileBtn" runat="server" href="AccountManagment.aspx" class="user-profile" style="text-decoration: none;"></a>
            <asp:Button ID="btnLogout" runat="server" Text="LOG OUT" CssClass="logout-btn" OnClick="btnLogout_Click"/>
        </div>

        <div class="container">
            <div class="input-group" style="grid-column: 1/2;">
                 <span class="userform-label">Product Name: </span>
                 <asp:TextBox ID="txtProductName" runat="server" CssClass="custom-txtbox" placeholder="Insert Name of the New Product Here..."></asp:TextBox>
            </div>
            
            <div class="input-group" style="grid-column: 1/3;">
                <span class="userform-label">Product Description: </span>
                <asp:TextBox ID="txtDescription" runat="server" CssClass="custom-txtbox desc-box" TextMode="MultiLine" placeholder="Insert Description of the Product Here..."></asp:TextBox>
            </div>

            <div class="input-group" style="grid-column: 1/2;">
                <span class="userform-label">Product Image: </span>
                <asp:FileUpload ID="fileUploadBtn" runat="server"/>
            </div>

            <asp:Button ID="btnSubmit" runat="server" Text="Add Product" CssClass="custom-btn" OnClick="btnSubmit_Click" />

            <asp:Label ID="lblErrorMessage" runat="server" Text="Error Message" CssClass="error-msg"></asp:Label>

        </div>
    </form>
</body>
</html>

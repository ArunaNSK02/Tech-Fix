<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SupplierDashboard.aspx.cs" Inherits="TechFix_Supplier_Application.SupplierDashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Supplier Dashboard - TechFix</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="CSS/StyleSheet.css" rel="stylesheet" />
    <link href='https://unpkg.com/boxicons@2.1.4/css/boxicons.min.css' rel='stylesheet'/>
    <style>
        i {
            margin-inline: 5px;
            cursor: pointer;
        }

        .link-buttons {
            text-decoration: none;
            color: white;
            margin-inline: 5px;
        }

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

        .Suppplier-DL {
            margin-block-start: 0;
            display: inline;
            margin-inline: 0;
            margin-inline-start: 15px;
        }

        .pop-up {
            z-index: 10;
            background-color: var(--color_one);
            box-shadow: rgba(0, 0, 0, 0.25) 0px 0.0625em 0.0625em, rgba(0, 0, 0, 0.25) 0px 0.125em 0.5em, rgba(255, 255, 255, 0.1) 0px 0px 0px 1px inset;
            width: 85vw;
            height: 80vh;
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            display: grid;
            grid-template-columns: 1fr 1fr;
            padding-inline: 60px;
            padding-block: 40px;
        }

        .overlay {
            position: fixed;
            width: 100vw;
            height: 100vh;
            background-color: black;
            filter: blur(20px);
            opacity: 0.6;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
        }

        .no-scroll {
            overflow: hidden;
            position: fixed;
            width: 100%;
        }

        .close-btn {
            position: absolute;
            top: 10px;
            right: 10px;
            color: white;
            font-size: 26px;
        }

        .pop-up .error-msg {
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            color: white;
        }

        .main-message {
            z-index: 10;
            background-color: var(--color_one);
            box-shadow: rgba(0, 0, 0, 0.25) 0px 0.0625em 0.0625em, rgba(0, 0, 0, 0.25) 0px 0.125em 0.5em, rgba(255, 255, 255, 0.1) 0px 0px 0px 1px inset;
            position: fixed;
            width: fit-content;
            height: fit-content;
            top: 50px;
            left: 50%;
            transform: translateX(-50%);
            display: flex;
            align-items: center;
            padding-inline: 30px;
            padding-block: 20px;
        }

        .lblSearchMessage {
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            font-weight: bold;

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

        <div class="search-form">
            <div>
                <asp:TextBox ID="searchbox" runat="server" CssClass="custom-txtbox" placeholder="Search Items..."></asp:TextBox>
                <asp:Button ID="btnSubmit" runat="server" CssClass="custom-btn" Text="Search" OnClick="btnSearch_Click" />
            </div>
        </div>
        
        <div class="menu-container" runat="server" id="menuContainer">
            <asp:Repeater ID="ProductRepeater" runat="server">
                <ItemTemplate>
                    <div class="menu-item">
                        <div class="product-img" style="background-image: url('<%# "/Shared/Images/" + Eval("ImagePath") %>');"></div>
                        <span class="title"><%# Eval("ProductName") %></span>
                        <div class="sub-detail">
                            <span style="margin-inline:0; font-size: small; opacity: 0.8;"><%# Eval("SupplierName") %></span>
                            <span style="margin-inline:0; font-size: small; opacity: 0.8;">Avbl. Units: <%# Eval("AvailableAmount") %></span>
                        </div>
                        <span class="price">Rs.<%# Eval("Price", "{0:F2}") %></span>
                        <span class="last-update">Last Updated: <%# Eval("LastUpdated", "{0:yyyy-MM-dd}") %></span>
                        <asp:Button ID="BtnAddtoCart"  runat="server" CssClass="custom-btn" CommandArgument='<%# Eval("ProductID") %>' Text="Delete"/>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <div class="overlay" runat="server" id="overlay">
        </div>

        <asp:Label ID="lblSearchMessage" runat="server" Text="" CssClass="lblSearchMessage" Visible="false"></asp:Label>
    </form>
</body>
</html>

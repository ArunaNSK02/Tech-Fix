<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StaffDashboard.aspx.cs" Inherits="TechFix_Client_Application.StaffDashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Shop Member Dashboard - TechFix</title>
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
            padding-inline: 60px;
            padding-block: 40px;
        }

        .overlay {
            z-index: 5;
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

        .menu-item .custom-txtbox {
            background-color: transparent;
            color: var(--color_four);
            width: 80px;
            padding: 10px;
            border: 2px solid var(--color_four);
            margin-inline: 5px;
            margin-block-start: 40px;
            margin-block-end: 30px;
        }

        .error-msg {
            color: var(--color_two);
            position: fixed;
            bottom: 25px;
            left: 50%;
            transform: translateX(-50%);
            width: fit-content;
            font-family: "Noto Sans", serif;
            font-size: 16px;
            font-weight: bold;
            opacity: 1;
        }

        .styled-table {
            margin-block: 0;
            margin-block-start: 50px;
            margin-inline: auto;
            border-collapse: collapse;
            min-width: 400px;
            height: fit-content;
            box-sizing:border-box;
            box-shadow: rgba(0, 0, 0, 0.25) 0px 0.0625em 0.0625em, rgba(0, 0, 0, 0.25) 0px 0.125em 0.5em, rgba(255, 255, 255, 0.1) 0px 0px 0px 1px inset;
        }

        .pop-up .custom-btn {
            margin-block-start: 0;
            margin-inline: 0;
            font-weight: bold;
            cursor: pointer;
            padding: 5px;
            border: none;
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
    <form id="mainform" runat="server" style="position: relative;">

        <div class="nav-bar">
            <a><img src="images/logo.png"/></a>
            <ul class="nav-menu">
                <li><a href="QuotationManagment.aspx" class="nav-link">Quotation Managment</a></li>
                <li><a href="OrderManagment.aspx" class="nav-link">Order Managment</a></li>
            </ul>
            <a id="profileBtn" runat="server" href="AccountManagment.aspx" class="user-profile" style="text-decoration: none;"></a>
            <asp:Button ID="btnLogout" runat="server" Text="LOG OUT" CssClass="logout-btn" OnClick="btnLogout_Click"/>
        </div>

        <div class="search-form">
            <div>
                <asp:TextBox ID="searchbox" runat="server" CssClass="custom-txtbox" placeholder="Search Items..."></asp:TextBox>
                <asp:DropDownList ID="supplierDL" runat="server" CssClass="custom-btn"></asp:DropDownList>
                <asp:Button ID="Button1" runat="server" CssClass="custom-btn" Text="Search" OnClick="BtnSearch_Click" />
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
                        <asp:Button ID="BtnAddtoCart"  runat="server" CssClass="custom-btn" CommandArgument='<%# Eval("ProductID") + "|" + Eval("Price", "{0:F2}") %>' Text="ADD TO CART" OnClick="btnAddtoCart_Click"/>
                        <asp:TextBox ID="txtItemCount" runat="server" CssClass="custom-txtbox" TextMode="Number"></asp:TextBox>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <div class="overlay" runat="server" id="overlay">
        </div>

        <div class="pop-up" runat="server" id="popup">
            <asp:LinkButton ID="closeBtn" runat="server" OnClick="btnClose_Click" CssClass="close-btn"><i class='bx bx-x'></i></asp:LinkButton>
                <asp:Repeater ID="CartRepeater" runat="server">
                    <HeaderTemplate>
                        <table class="styled-table">
                            <thead>
                                <tr>
                                    <th style="width: 500px; border-right: 2px solid hsl(208, 67%, 9%);">Product Name</th>
                                    <th style="width: 200px;  text-align:center; border-right: 2px solid hsl(208, 67%, 9%);">Supplier Name</th>
                                    <th style="width: 200px; text-align:center; border-right: 2px solid hsl(208, 67%, 9%);">Unit Price(Rs.)</th>
                                    <th style="width: 100px;  text-align: center; border-right: 2px solid hsl(208, 67%, 9%);">Quantity</th>
                                    <th style="width: 200px; text-align: center; border-right: 2px solid hsl(208, 67%, 9%);">Total Price(Rs.)</th>
                                    <th style="width: 175px; text-align: center;"></th>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>

                    <ItemTemplate>
                        <tr>
                            <td><%# Eval("ProductName") %></td>
                            <td style="text-align:center;"><%# Eval("SupplierName") %></td>
                            <td style="text-align:center;"><%# Eval("UnitPrice", "{0:F2}") %></td>
                            <td style="text-align:center;"><%# Eval("Quantity") %></td>
                            <td style="text-align:center;"><%# Eval("Price", "{0:F2}") %></td>
                            <td style="text-align:center;">
                                <asp:LinkButton ID="deleteLink" runat="server" CommandArgument='<%# Eval("CartItemID") %>' OnClick="btnDelete_Click" CssClass="link-buttons">
                                    <i class="bx bxs-trash"></i>
                                </asp:LinkButton>
                                <asp:Button ID="BtnProceed" runat="server" CommandArgument='<%# Eval("CartItemID") %>' CssClass="custom-btn" Text="ORDER" OnClick="btnProceed_Click"/>
                            </td>
                        </tr>
                    </ItemTemplate>


                    <FooterTemplate>
                            </tbody>
                        </table>
                        
                    </FooterTemplate>
                </asp:Repeater>
            <asp:Label ID="lblCartMessage" runat="server" Text="" Visible="false" CssClass="error-msg"></asp:Label>
        </div>

        <div class="view-cart">
            <asp:Button ID="BtnViewCart" runat="server" Text="View Cart" OnClick="btnViewCart_Click" CssClass="custom-btn" />
        </div>

        <div class="main-message" runat="server" id="mainmessagebox">
             <asp:LinkButton ID="BtnmessageClose" runat="server"  OnClick="btnMessageClose_Click" CssClass="link-buttons">
                <i class='bx bx-x-circle'></i>
             </asp:LinkButton>
            <asp:Label ID="mainMessage" runat="server" Text="main error message goes here." ></asp:Label>
        </div>

        <asp:Label ID="lblSearchMessage" runat="server" Text="" CssClass="lblSearchMessage" Visible="false"></asp:Label>
    </form>
</body>
</html>
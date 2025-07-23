<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InventoryManagment.aspx.cs" Inherits="TechFix_Supplier_Application.InventoryManagment" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Supplier Dashboard - TechFix</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="CSS/StyleSheet.css" rel="stylesheet" />
        <link href='https://unpkg.com/boxicons@2.1.4/css/boxicons.min.css' rel='stylesheet'/>
    <style>
        @import url('https://fonts.googleapis.com/css2?family=Noto+Sans:ital,wght@0,100..900;1,100..900&display=swap');

        .container {
            width: 70%;
            margin-inline: auto;
            margin-block-start: 50px;
            padding-inline: 60px;
            padding-block: 40px;
            box-shadow: rgba(0, 0, 0, 0.25) 0px 0.0625em 0.0625em, rgba(0, 0, 0, 0.25) 0px 0.125em 0.5em, rgba(255, 255, 255, 0.1) 0px 0px 0px 1px inset;
        }

        .form-container {
            width: fit-content;
            display: flex;
            justify-content: space-between;
            align-items: center;
            gap: 25px;
            margin-inline: auto;
        }

        .custom-txtbox {
            opacity: 0.5;
            margin-block-start: 5px;
            margin-block-end: 0;
            width: 250px;
            margin-inline: 0;
        }


        .userform-label {
            display: block;
            font-weight: bold;
        }

        .input-group {
            margin-block-end: 0;
        }

        .custom-btn {
            display: block;
            margin-inline: auto;
            margin-block-start: 30px;
        }

        .error-msg {
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

        .Product-DL {
            margin-block-start: 5px;
            display: inline;
            width: 250px;
            opacity: 0.5;
            padding: 9px;
        }

        i {
            cursor: pointer;
            color: white;
        }

        .link-button {
            text-decoration: none;
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
            <div class="form-container">
                <div class="input-group">
                    <span class="userform-label">Product: </span>
                    <asp:DropDownList ID="productDL" runat="server" CssClass="custom-btn Product-DL"></asp:DropDownList>
                </div>
            
                <div class="input-group">
                    <span class="userform-label">Price per unit: Rs. </span>
                    <asp:TextBox ID="txtPrice" runat="server" CssClass="custom-txtbox" TextMode="Number"></asp:TextBox>
                </div>

                <div class="input-group">
                    <span class="userform-label">Stock: </span>
                    <asp:TextBox ID="txtStock" runat="server" CssClass="custom-txtbox" TextMode="Number"></asp:TextBox>
                </div>
            </div>

            <asp:Button ID="btnSubmit" runat="server" Text="Add Stock" CssClass="custom-btn" OnClick="btnSubmit_Click" />
            
            <asp:Label ID="lblErrorMessage" runat="server" Text="Error Message" CssClass="error-msg"></asp:Label>

        </div>

        <div class="conatiner">

            <table class="styled-table">
                <thead>
                    <tr>
                        <th style="width: 400px; text-align:center; border-right: 2px solid hsl(208, 67%, 9%);">Product Name</th>
                        <th style="width: 200px; text-align:center; border-right: 2px solid hsl(208, 67%, 9%);">Price Per Unit (Rs.)</th>
                        <th style="width: 200px; text-align:center; border-right: 2px solid hsl(208, 67%, 9%);">Quantity</th>
                        <th style="width: 200px; text-align:center; border-right: 2px solid hsl(208, 67%, 9%);">Updated Date</th>
                        <th style="width: 200px; text-align:center;"></th>
                    </tr>
                </thead>
                    <tbody id="rowContainer" runat="server">
                        <asp:Repeater ID="InventoryRepeater" runat="server" OnItemDataBound="InventoryRepeater_ItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td style="text-align:left"><%# Eval("ProductName") %></td>
                                    <td style="text-align:center"><%# Eval("Price", "{0:F2}") %></td>
                                    <td style="text-align:center"><%# Eval("StockQuantity") %></td>
                                    <td style="text-align:center"><%# Eval("StockInsertDate", "{0:yyyy-MM-dd}") %></td>
                                    <td style="text-align:center">
                                        <asp:LinkButton ID="deleteLink" runat="server" CommandArgument='<%# Eval("InventoryID") %>' OnClick="DeleteInventory_Click" Visible="false" CssClass="link-button">
                                            <i class="bx bx-trash"></i>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="confirmLink" runat="server" CommandArgument='<%# Eval("InventoryID") %>' OnClick="ConfirmInventory_Click" Visible="false" CssClass="link-button">
                                            <i class="bx bx-check"></i>
                                        </asp:LinkButton>
                                        <asp:Literal ID="confirmedText" runat="server" Text="Confirmed" Visible="false"></asp:Literal>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
            </table>

        </div>
    </form>
</body>
</html>

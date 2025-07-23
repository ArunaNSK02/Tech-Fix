<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderManagment.aspx.cs" Inherits="TechFix_Client_Application.OrderManagment" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Shop Member Dashboard - TechFix</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" href="CSS/StyleSheet.css"/>
    <link href='https://unpkg.com/boxicons@2.1.4/css/boxicons.min.css' rel='stylesheet'/>
    <style>
        @import url('https://fonts.googleapis.com/css2?family=Noto+Sans:ital,wght@0,100..900;1,100..900&display=swap');

        .container {
            width: 70%;
            position:relative;
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
        i {
            margin-inline: 5px;
            cursor: pointer;
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

        .lblOrderMessage {
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            font-weight: bold;

        }

    </style>
</head>
<body id="body" runat="server">
    <form id="mainform" runat="server">
        <div class="nav-bar">
            <a href="StaffDashboard.aspx"><img src="images/logo.png"/></a>
            <ul class="nav-menu">
                <li><a href="QuotationManagment.aspx" class="nav-link">Quotation Managment</a></li>
                <li><a href="OrderManagment.aspx" class="nav-link">Order Managment</a></li>
            </ul>
            <a id="profileBtn" runat="server" href="AccountManagment.aspx" class="user-profile" style="text-decoration: none;"></a>
            <asp:Button ID="btnLogout" runat="server" Text="LOG OUT" CssClass="logout-btn" OnClick="btnLogout_Click" />
        </div>

        <div class="conatiner">
            <asp:Repeater ID="orderRepeater" runat="server" OnItemDataBound="orderRepeater_ItemDataBound">
                <HeaderTemplate>
                    <table class="styled-table">
                        <thead>
                            <tr>
                                    <th style="width: 400px; border-right: 2px solid hsl(208, 67%, 9%);">Orders</th>
                                    <th style="width: 200px;  text-align:center; border-right: 2px solid hsl(208, 67%, 9%);">Quantity</th>
                                    <th style="width: 200px; text-align:center; border-right: 2px solid hsl(208, 67%, 9%);">Supplier Name</th>
                                    <th style="width: 200px;  text-align: center; border-right: 2px solid hsl(208, 67%, 9%);">Order date</th>
                                    <th style="width: 200px; text-align: center; border-right: 2px solid hsl(208, 67%, 9%);">Price(Rs.)</th>
                                    <th style="width: 150px; text-align: center;"></th>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td><%# Eval("ProductName") %></td>
                        <td style="text-align:center;"><%# Eval("Quantity") %></td>
                        <td style="text-align:center;"><%# Eval("SupplierName") %></td>
                        <td style="text-align:center;"><%# Eval("OrderDate", "{0:MM/dd/yyyy}") %></td>
                        <td style="text-align:center;"><%# Eval("TotalAmount", "{0:N2}") %></td>
                        <td style="text-align:center;">
                            <asp:LinkButton ID="deleteLink" runat="server" CommandName="DeleteOrder" CommandArgument='<%# Eval("OrderID") %>' CssClass="link-buttons" OnClick="btnDelete_Click">
                                <i class="bx bx-trash"></i>
                            </asp:LinkButton>
                            <asp:Label ID="deliveredLabel" runat="server" Text="Delivered" Style="display:none;" />

                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </tbody>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
            
        </div>
        <asp:Label ID="lblOrderMessage" runat="server" Text="" CssClass="lblOrderMessage"></asp:Label>
    </form>
</body>
</html>

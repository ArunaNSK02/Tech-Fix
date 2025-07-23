<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuotationManagment.aspx.cs" Inherits="TechFix_Supplier_Application.QuotationManagment" %>

<!DOCTYPE html>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Supplier Dashboard - TechFix</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" href="CSS/StyleSheet.css"/>
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

        .file-upload {
            width: 200px;
        }

        .styled-table .custom-btn {
            margin-inline: 0;
            margin-block-start: 0;
            margin-inline-start: 15px;
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

        <div class="conatiner">
            <asp:Repeater ID="QuotationRepeater" runat="server">
                <HeaderTemplate>
                    <table class="styled-table">
                        <thead>
                            <tr>
                                <th style="width: 400px; border-right: 2px solid hsl(208, 67%, 9%);">Title</th>
                                <th style="width: 200px;  text-align:center; border-right: 2px solid hsl(208, 67%, 9%);">Quot. Attachment</th>
                                <th style="width: 200px; text-align:center; border-right: 2px solid hsl(208, 67%, 9%);">Request Date</th>
                                <th style="width: 200px;  text-align: center; border-right: 2px solid hsl(208, 67%, 9%);">Response Attachment</th>
                                <th style="width: 250px; text-align: center;"></th>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td style="text-align:left"><%# Eval("Title") %></td>
                        <td style="text-align:center"><a href='<%# "/Shared/Quotation/" + Eval("Attachment") %>'>Download</a></td>
                        <td style="text-align:center"><%# Eval("RequestDate") %></td>
                        <td style="text-align:center">
                            <%# string.IsNullOrEmpty(Eval("SupplierResponse")?.ToString() ?? "") ? "-" : "<a href='/Shared/Quotation/" + Eval("SupplierResponse") + "' >View Response Attachment</a>" %>
                        </td>
                        <td style="text-align:center; display: flex; align-items: center;">
                            <asp:FileUpload ID="btnResponseFileUpload" runat="server" CssClass="file-upload" />
                            <asp:Button ID="BtnSubmit"  runat="server" CssClass="custom-btn" CommandArgument='<%# Eval("QuotationID") %>' Text="Submit!" OnClick="BtnSubmit_Click"/>
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

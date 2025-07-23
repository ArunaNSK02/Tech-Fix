<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuotationManagment.aspx.cs" Inherits="TechFix_Client_Application.QuotationManagment" %>

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

    </style>
</head>
<body id="body" runat="server" style="padding-block-end: 20px;">
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

        <div class="container">
            <div class="input-group" style="grid-column: 1/2;">
                 <span class="userform-label">Quotation Title: </span>
                 <asp:TextBox ID="txtQuotationTitle" runat="server" CssClass="custom-txtbox" placeholder="Insert Title of the Quotation Here..."></asp:TextBox>
            </div>
            
            <div class="input-group" style="grid-column: 1/3;">
                <span class="userform-label">Description: </span>
                <asp:TextBox ID="txtDescription" runat="server" CssClass="custom-txtbox desc-box" TextMode="MultiLine" placeholder="Insert Summary of the Quotation Here...  (Not Mandotary)"></asp:TextBox>
            </div>

            <div class="input-group" style="grid-column: 1/2;">
                <span class="userform-label">Quotation file: </span>
                <asp:FileUpload ID="fileUploadBtn" runat="server"/>
            </div>
            
            <div class="input-group" style="grid-column: 2/3; display: flex; justify-items:left;">
                <span class="userform-label" style="display: inline; margin-block-start: 0;">Suppliers: </span>
                <asp:DropDownList ID="supplierDL" runat="server" CssClass="custom-btn Suppplier-DL"></asp:DropDownList>
            </div>

            <asp:Button ID="btnSubmit" runat="server" Text="Send Quotation" CssClass="custom-btn" OnClick="btnSubmit_Click" />

            <asp:Label ID="lblErrorMessage" runat="server" Text="Error Message" CssClass="error-msg"></asp:Label>

        </div>

        <div class="conatiner">
            <asp:Repeater ID="quotationsRepeater" runat="server" OnItemDataBound="quotationsRepeater_ItemDataBound">
                <HeaderTemplate>
                    <table class="styled-table">
                        <thead>
                            <tr>
                                    <th style="width: 400px; border-right: 2px solid hsl(208, 67%, 9%);">Title</th>
                                    <th style="width: 200px;  text-align:center; border-right: 2px solid hsl(208, 67%, 9%);">Quot. Attachment</th>
                                    <th style="width: 200px; text-align:center; border-right: 2px solid hsl(208, 67%, 9%);">Supplier Name</th>
                                    <th style="width: 200px;  text-align: center; border-right: 2px solid hsl(208, 67%, 9%);">Supplier Response</th>
                                    <th style="width: 200px; text-align: center; border-right: 2px solid hsl(208, 67%, 9%);">Request Date</th>
                                    <th style="width: 200px; text-align: center;"></th>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td style="text-align:left;"><%# Eval("Title") %></td>
                        <td style="text-align:center;">
                            <a href='<%# "/Shared/Quotation/" + Eval("Attachment") %>'>Download</a>
                        </td>
                        <td style="text-align:center;"><%# Eval("SupplierName") %></td>
                        <td style="text-align:center;">
                            <%# string.IsNullOrEmpty(Eval("SupplierResponse")?.ToString() ?? "") ? "Not Responded Yet..." : "<a href='/Shared/Quotation/" + Eval("SupplierResponse") + "' download>Download</a>" %>
                        </td>
                        <td style="text-align:center;"><%# Eval("RequestDate") %></td>
                        <td style="text-align:center;">
                            <asp:LinkButton ID="deleteLink" runat="server" CommandArgument='<%# Eval("QuotationID") %>' OnClick="btnDelete_Click" CssClass="link-buttons">
                                <i class="bx bxs-trash"></i>
                            </asp:LinkButton>
                            <asp:LinkButton ID="editLink" runat="server" CommandArgument='<%# Eval("QuotationID") %>' OnClick="btnEdit_Click" CssClass="link-buttons">
                                <i class="bx bxs-edit"></i>
                            </asp:LinkButton>
                            <asp:Label ID="respondedLabel" runat="server" CssClass="responded-label" Text="Responded" Visible="false"></asp:Label>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </tbody>
                    </table>
                </FooterTemplate>
            </asp:Repeater>

            
        </div>

        <div class="overlay" runat="server" id="overlay">
        </div>

        <div class="pop-up" runat="server" id="popup">
            <asp:LinkButton ID="closeBtn" runat="server" OnClick="btnClose_Click" CssClass="close-btn"><i class='bx bx-x'></i></asp:LinkButton>

            <div class="input-group" style="grid-column: 1/2;">
                 <span class="userform-label">Update Title: </span>
                 <asp:TextBox ID="updatetxtQuotationTitle" runat="server" CssClass="custom-txtbox"></asp:TextBox>
            </div>
            
            <div class="input-group" style="grid-column: 1/3;">
                <span class="userform-label">Update Description: </span>
                <asp:TextBox ID="updatetxtDescription" runat="server" CssClass="custom-txtbox desc-box" TextMode="MultiLine"></asp:TextBox>
            </div>

            <div class="input-group" style="grid-column: 1/2;">
                <span class="userform-label">Change Quotation file: </span>
                <asp:FileUpload ID="updatefileUpload" runat="server"/>
            </div>

            <asp:Button ID="updateBtn" runat="server" Text="Update Quotation" CssClass="custom-btn" OnClick="btnUpdate_Click" />
        </div>

    </form>
</body>
</html>


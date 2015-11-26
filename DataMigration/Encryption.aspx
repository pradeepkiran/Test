<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Encryption.aspx.cs" Inherits="Encryption" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <table class="style1">
            <tr>
                <td>
                    <asp:Label ID="Label3" runat="server" Text="Original Value"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtOriginal" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnEncrypt" runat="server" Text="Encrypt" 
                        onclick="btnEncrypt_Click" />
                </td>
                <td>
                    <asp:Button ID="btnDecrypt" runat="server" Text="Decrypt" 
                        onclick="btnDecrypt_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    
                    <asp:Label ID="Label1" runat="server" Text="Result"></asp:Label>
                    
                </td>
                <td>
                    <asp:Label ID="lblResult" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>

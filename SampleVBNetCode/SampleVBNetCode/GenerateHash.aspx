<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GenerateHash.aspx.vb" Inherits="SampleVBNetCode.GenerateHash" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table>
        
    <tr>
    <td>Input String :</td>
    <td><asp:TextBox ID="txtHashString" runat="server" Width="800px"></asp:TextBox></td>
    <td><asp:Button ID="btnSubmit" runat="server" Text="Generate Hash" /></td>
    </tr>
    <tr>
    <td>Result :</td>
    <td><asp:TextBox ID="txtResult" runat="server" Width="300px"></asp:TextBox></td>
    <td></td>
    </tr>
    </table>
    </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Wcf_HostWebSite.Default" Async="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h3>Home page</h3>
        <asp:Button ID="btnMessage" runat="server" Text="Message" 
            onclick="btnMessage_Click" />
        <asp:Button ID="btnError" runat="server" Text="Make an Error" 
            onclick="btnError_Click" />
        <asp:Literal ID="litMessage" runat="server"></asp:Literal>
    </div>
    </form>
</body>
</html>

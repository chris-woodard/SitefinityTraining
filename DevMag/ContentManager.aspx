<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContentManager.aspx.cs" Inherits="SitefinityWebApp.ContentManager" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button runat="server" ID="NewsExport" Text="News Export" OnClick="NewsExport_Click" />
            <asp:Button runat="server" ID="NewsImport" Text="News Import" OnClick="NewsImport_Click" />
            <asp:Button runat="server" ID="AuthorImport" Text="Author Import" OnClick="AuthorImport_Click" />
        </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test1.aspx.cs" Inherits="Groll.Schule.SchuleDBWeb.Zum_Testen.Test1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Calendar ID="Calendar1" runat="server" BackColor="White" 
            BorderColor="White" BorderWidth="1px" Font-Names="Verdana" Font-Size="9pt" 
            ForeColor="Black" Height="190px" NextPrevFormat="FullMonth" Width="350px">
            <DayHeaderStyle Font-Bold="True" Font-Size="8pt" />
            <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" 
                VerticalAlign="Bottom" />
            <OtherMonthDayStyle ForeColor="#999999" />
            <SelectedDayStyle BackColor="#333399" ForeColor="White" />
            <TitleStyle BackColor="White" BorderColor="Black" BorderWidth="4px" 
                Font-Bold="True" Font-Size="12pt" ForeColor="#333399" />
            <TodayDayStyle BackColor="#CCCCCC" />
        </asp:Calendar>
        <div>
            <asp:TextBox ID="textB" runat="server" Text="" EnableViewState="False"/></div>
        <asp:RequiredFieldValidator ErrorMessage="Wert eingeben" 
            ControlToValidate="textB" runat="server" CssClass="hasError" >sdsdsd</asp:RequiredFieldValidator>
        <div>
            <asp:Button ID="PostBack" Text="PostBack" runat="server" /></div>
    </div>
        <div>
            <asp:Button ID="SetDate" Text="SetDate" runat="server" OnClick="SetDate_Click" /></div>
    
    </form>
</body>
</html>

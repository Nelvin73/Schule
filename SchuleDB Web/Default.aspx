<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="True" CodeBehind="Default.aspx.cs" Inherits="Groll.Schule.SchuleDBWeb._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1 class="text-info">SchuleDB Web</h1>
        <p class="lead">Web-Anwendung zur Schul-Datenbank.</p>
        <asp:LoginView runat="server" ID="loginView1">
            <AnonymousTemplate>
                <div class="text-info"> <p>Keine Datenbank ausgewählt.<BR /> Bitte einloggen</p></div>
            </AnonymousTemplate>
            <LoggedInTemplate>
                <p class="text-info">Aktuelles Schuljahr: <asp:label runat="server" ID="aktSchuljahr"> </asp:label></p>
                <p><a href="http://www.asp.net" class="btn btn-primary btn-lg">Schuljahr auswählen &raquo;</a></p>
            </LoggedInTemplate>
        </asp:LoginView>
        
    </div>
    
    <div class="row">
        <div class="col-md-4">
            
            <h2>Getting started</h2>
            <p>
                ASP.NET Web Forms lets you build dynamic websites using a familiar drag-and-drop, event-driven model.
            A design surface and hundreds of controls and components let you rapidly build sophisticated, powerful UI-driven sites with data access.
            </p>
            <p>
                <a class="btn btn-default" href="http://go.microsoft.com/fwlink/?LinkId=301948">Learn more &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Sch&uuml;ler</h2>
            <p>
                Zeige eine Liste der Schüler an.
            </p>
            <p>
                <a class="btn btn-default" href="~/Pages/Schueler" runat="server" >Learn more &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Web Hosting</h2>
            <p>
                You can easily find a web hosting company that offers the right mix of features and price for your applications.
            </p>
            <p>
                <a class="btn btn-default" href="http://go.microsoft.com/fwlink/?LinkId=301950">Learn more &raquo;</a>
            </p>
        </div>
    </div>

</asp:Content>

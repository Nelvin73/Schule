<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Schueler.aspx.cs" Inherits="Groll.Schule.SchuleDBWeb.Pages.Schueler" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <div class="row">
        <div class="col-md-12">
            <h2>Schüler</h2>
        </div>
        <div class="col-md-12">


            <asp:ListView ID="ListView1" runat="server" ItemType="Groll.Schule.Model.Schueler" >
                <LayoutTemplate>
                    <table class="table">
                        <caption style="text-align:left">Komplette Liste aller Schüler</caption>
                        <thead>
                            <tr>
                                <th>Name</th>
                                <th>Strasse</th>
                                <th>Ort</th>
                                <th>Geschlecht</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                        </tbody>
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:Label Text="<%#Item.DisplayName %>" runat="server" /></td>
                        <td>
                            <asp:Label Text="<%#Item.Adresse.Strasse %>" runat="server" /></td>
                        <td>
                            <asp:Label Text="<%#Item.Adresse.Ort %>" runat="server" /></td>
                        <td>
                            <asp:Label Text="<%#Item.Geschlecht %>" runat="server" /></td>
                    </tr>

                </ItemTemplate>
            </asp:ListView>
        </div>
    </div>
</asp:Content>


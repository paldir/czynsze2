﻿<%@ Page Title="Generacja należności" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GeneracjaNaleznosci.aspx.cs" Inherits="czynsze.Formularze.GeneracjaNaleznosci" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div class="placeOfResultWindow">
        <div class="resultWindow">
            <form id="form" runat="server">
                <div id="placeOfDate" runat="server"></div>
                <div id="placeOfGeneration" runat="server"></div>
            </form>
        </div>
    </div>
</asp:Content>
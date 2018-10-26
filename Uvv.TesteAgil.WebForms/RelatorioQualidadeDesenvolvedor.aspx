<%@ Page Title="Relatório" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RelatorioQualidadeDesenvolvedor.aspx.cs" Inherits="Uvv.TesteAgil.WebForms.RelatorioQualidadeDesenvolvedor" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <div class="jumbotron">
        <h1 style="font-size: 24pt;">Relatório de Qualidade Por Desenvolvedor</h1>
    </div>
    <div class="container">
        <%--                <asp:UpdatePanel runat="server" ID="updRel">
            <ContentTemplate>--%>
        <div id="alertSuccess" class="alert alert-success collapse">
            <a id="closeSuccess" href="#" class="close">&times;</a>
            <strong>
                <asp:Label runat="server" ID="msgSucesso"></asp:Label></strong>
        </div>

        <div id="alertError" class="alert alert-danger collapse">
            <a href="#" id="closeError" class="close" data-dismiss="alert">&times;</a>
            <strong>
                <asp:Label runat="server" ID="msgErro"></asp:Label></strong>
        </div>
        <%--                <asp:UpdatePanel runat="server">
                    <ContentTemplate>--%>
        <table>
            <tr>
                <td>
                    <div class="input-group">
                        <span style="width: 30px" class="input-group-addon">Projeto</span>
                        <asp:DropDownList class="form-control" ID="ddlProjeto" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlProjeto_SelectedIndexChanged" />
                    </div>
                </td>
                <td>
                    <div class="input-group">
                        <span style="width: 30px" class="input-group-addon">Sprint</span>
                        <asp:DropDownList class="form-control" ID="ddlSprint" AutoPostBack="true" runat="server" />
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="input-group">
                        <span style="width: 30px" class="input-group-addon">Desenvolvedor</span>
                        <asp:DropDownList class="form-control" ID="ddlDesenvolvedor" AutoPostBack="true" runat="server" />
                    </div>
                </td>
            </tr>
        </table>
        <%--                    </ContentTemplate>
                </asp:UpdatePanel>--%>
        <div class="modal-footer">
            <asp:Button runat="server" CssClass="btn btn-primary" Text="Gerar Relatório" ID="btnGerarRelatorio" OnClick="btnGerarRelatorio_Click" />
            <asp:Button runat="server" CssClass="btn btn-primary" Text="Exportar PDF" ID="btnExportar" Visible="false" OnClick="btnExportar_Click" />
        </div>
        <div id="divRel" runat="server" visible="false">
            <asp:Chart ID="chartRel" runat="server" Height="300px" Width="600px">
                <Series>
                    <asp:Series Name="Series1" XValueMember="Situacao" YValueMembers="Quantidade" ChartType="Pie" IsValueShownAsLabel="True"></asp:Series>
                </Series>
                <ChartAreas>
                    <asp:ChartArea Name="ChartArea1">
                    </asp:ChartArea>
                </ChartAreas>
                <Legends>
                    <asp:Legend Name="Situacao" BackColor="Thistle" TitleAlignment="Near"></asp:Legend>
                </Legends>
                <Titles>
                    <asp:Title Font="Microsoft Sans Serif, 12pt, style=Bold" Text="Qualidade dos Testes"></asp:Title>
                </Titles>
            </asp:Chart>

            <asp:Chart ID="chartRel2" runat="server" Height="300px" Width="600px">
                <Series>
                    <asp:Series Name="Series1" XValueMember="Situacao" YValueMembers="Quantidade" ChartType="Bar" IsValueShownAsLabel="True"></asp:Series>
                </Series>
                <ChartAreas>
                    <asp:ChartArea Name="ChartArea1">
                    </asp:ChartArea>
                </ChartAreas>
                <Legends>
                    <asp:Legend Name="Situacao" BackColor="Thistle" TitleAlignment="Near"></asp:Legend>
                </Legends>
                <Titles>
                    <asp:Title Font="Microsoft Sans Serif, 12pt, style=Bold" Text="Qualidade dos Testes"></asp:Title>
                </Titles>
            </asp:Chart>

        </div>
        <div id="pieChart" style="width: 500px; height: 300px">
        </div>
     <%--   </ContentTemplate>
        </asp:UpdatePanel>--%>
        

    </div>
</asp:Content>

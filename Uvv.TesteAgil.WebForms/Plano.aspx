<%@ Page Title="Plano de Teste" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Plano.aspx.cs" Inherits="Uvv.TesteAgil.WebForms.Plano" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        h3 {
            font-size: 16pt;
            font-weight: bold;
        }

            h3 label {
                font-size: 16pt;
                font-weight: normal;
            }

        .formulario {
            display: inline-block;
            float: left;
            width: 100%;
        }

        #exTab2 h3 {
            color: white;
            background-color: #428bca;
            padding: 5px 15px;
        }

        .margemPadrao {
            margin-top: 10px;
            margin-bottom: 10px;
        }

        .ListBoxCssClass {
            color: darkslategrey;
            background-color: ghostwhite;
            font-family: Arial;
            font-weight: bold;
            font-size: 14pt;
            min-height: 100px;
            min-width: 400px;
            border: 1px solid #cccccc;
            border-radius: 4px;
            box-shadow: inset 0px 1px 1px rgba(0,0,0,0.075);
            transition: border-color ease-in-out 0.15s, box-shadow ease-in-out 0.15s;
            scrollbar-base-color: #C0C0C0;
            scrollbar-base-color: #C0C0C0;
            scrollbar-3dlight-color: #C0C0C0;
            scrollbar-highlight-color: #C0C0C0;
            scrollbar-track-color: #EBEBEB;
            scrollbar-arrow-color: black;
            scrollbar-shadow-color: #C0C0C0;
            font-family: inherit;
            font-size: inherit;
        }

        .tabmenu-inicial {
            display: block;
            padding: 4px 18px 4px 18px;
            float: left;
            background-color: white;
            color: black;
            font-weight: bold;
        }

            .tabmenu-inicial:hover {
                color: cornflowerblue;
                background-color: white;
            }

            .tabmenu-inicial:disabled {
                background-color: #cccccc;
                color: #666666;
            }

        .tabmenu-clicked {
            float: left;
            display: block;
            background-color: cornflowerblue;
            padding: 4px 18px 4px 18px;
            color: white;
            font-weight: bold;
        }

            .tabmenu-clicked:disabled {
                background-color: #cccccc;
                color: #666666;
            }

        .tab-table {
            width: 100%;
            border-style: none;
        }

        .txt {
            float: left;
        }

        .campo-tamanho {
            width: auto;
        }
    </style>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="jumbotron">
                <h1 style="font-size: 24pt;">
                    <asp:Label runat="server" ID="lbPage" Text="Plano de Teste"></asp:Label></h1>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="container">
        <div id="alertSuccess" class="alert alert-success collapse">
            <a id="closeSuccess" href="#" class="close">&times;</a>
            <strong>
                <asp:Label runat="server" ID="msgSucesso"></asp:Label>
            </strong>
        </div>

        <div id="alertError" class="alert alert-danger collapse">
            <a href="#" id="closeError" class="close" data-dismiss="alert">&times;</a>
            <strong>
                <asp:Label runat="server" ID="msgErro"></asp:Label>
            </strong>
        </div>

        <div id="divPlano" runat="server">
            <div class="formulario">
                <table class="tab-table">
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="pnlPlano" runat="server">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <div class="input-group margemPadrao dropStyle">
                                                    <span class="input-group-addon">Projeto</span>
                                                    <asp:DropDownList CssClass="form-control campo-tamanho" runat="server" ID="dblProjetos" AutoPostBack="true" OnSelectedIndexChanged="dblProjetos_SelectedIndexChanged" />
                                                </div>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="input-group margemPadrao dropStyle">
                                                    <span class="input-group-addon">Sprint</span>
                                                    <asp:DropDownList CssClass="form-control campo-tamanho" runat="server" ID="dblSprints" AutoPostBack="true" OnSelectedIndexChanged="dblSprints_SelectedIndexChanged" />
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="input-group margemPadrao">
                                                    <span class="input-group-addon">Descrição</span>
                                                    <asp:TextBox CssClass="form-control txt campo-tamanho" runat="server" ID="txtDescricao"></asp:TextBox>
                                                </div>
                                            </td>
                                            <td>
                                                <asp:Label CssClass="lb-erro txt" runat="server" ID="erroDescricao"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="input-group margemPadrao">
                                                    <span class="input-group-addon">Data de criação</span>
                                                    <asp:TextBox runat="server" Enabled="false" TextMode="Date" CssClass="form-control campo-tamanho" ID="txtDataCriacao"></asp:TextBox>
                                                </div>
                                            </td>
                                            <td>
                                                <asp:Label CssClass="lb-erro txt" runat="server" ID="erroDataCriacao"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="input-group margemPadrao">
                                                    <span class="input-group-addon">Data de finalização</span>
                                                    <asp:TextBox runat="server" TextMode="Date" CssClass="form-control campo-tamanho" ID="txtDataFim"></asp:TextBox>

                                                </div>
                                            </td>
                                            <td>
                                                <asp:Label CssClass="lb-erro txt" runat="server" ID="erroDataFim"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:HiddenField runat="server" ID="txtId" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <div class="input-group margemPadrao" style="width: 100%">
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td colspan="2"><span class="input-group-addon" style="width: 100%">Funcionalidades testadas</span></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:ListBox CssClass="ListBoxCssClass" runat="server" ID="lbxTestadas" AutoPostBack="true" OnSelectedIndexChanged="lbxTestadas_SelectedIndexChanged"></asp:ListBox>
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="btn btn-default" Enabled="false" ID="btnAddNaoTestada" Width="150" runat="server" Text="Não Testar" OnClick="btnAddNaoTestada_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2"><span class="input-group-addon" style="width: 100%">Funcionalidades não testadas</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:ListBox CssClass="list-group ListBoxCssClass" runat="server" ID="lbxNaoTestadas" AutoPostBack="true" OnSelectedIndexChanged="lbxNaoTestadas_SelectedIndexChanged"></asp:ListBox>
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="btn btn-default" Enabled="false" ID="btnAddTestada" Width="150" runat="server" Text="Testar" OnClick="btnAddTestada_Click" />
                                                </td>
                                            </tr>
                                            <tr style="margin: 0; padding: 0;">
                                                <td>
                                                    <asp:Label CssClass="lb-erro" runat="server" ID="erroFuncionalidadesTestadas"></asp:Label></td>
                                            </tr>
                                        </table>

                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <div class="modal-footer" style="width: 100%">
                                    <asp:Button runat="server" Enabled="false" CssClass="btn btn-success" Text="Cenário de Teste" ID="btnCenario" OnClick="btnCenario_Click" />
                                    <asp:Button runat="server" CssClass="btn btn-primary" Text="Salvar" ID="btnSalvar" OnClick="btnSalvar_Click" />
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>

        <div id="divCenario" runat="server" visible="false">
            <table class="tab-table">
                <tr>
                    <td style="width: 100%;">
                        <asp:UpdatePanel runat="server" ID="pnlCenario">
                            <ContentTemplate>
                                <asp:HiddenField runat="server" ID="txtIdCenario" />
                                <div class="input-group margemPadrao">
                                    <span class="input-group-addon">Funcionalidade</span>
                                    <asp:DropDownList CssClass="form-control dropStyle" runat="server" ID="ddlFuncionalidades" AutoPostBack="true" />
                                    <asp:Label CssClass="lb-erro" runat="server" ID="erroFuncionalidade"></asp:Label>
                                </div>
                                <div class="input-group margemPadrao">
                                    <span class="input-group-addon">Situação</span>
                                    <asp:DropDownList ID="ddlSituacao" CssClass="form-control" runat="server">
                                        <asp:ListItem Value="1" Text="A Fazer"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Desenvolvimento"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Teste"></asp:ListItem>
                                        <asp:ListItem Value="4" Text="Concluído"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Label CssClass="lb-erro" runat="server" ID="erroSituacao"></asp:Label>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div class="modal-footer">
                            <asp:Button runat="server" ID="btnVoltarPlano" Text="Voltar" CssClass="btn btn-default" OnClick="btnVoltarPlano_Click" />
                            <asp:Button runat="server" Enabled="false" ID="btnCaso" Text="Caso de Teste" CssClass="btn btn-success" OnClick="btnCaso_Click" />
                            <asp:Button runat="server" CssClass="btn btn-primary" Text="Salvar" ID="btnSalvarCenario" OnClick="btnSalvarCenario_Click" />
                        </div>
                        <div class="input-group margemPadrao">
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <asp:GridView CssClass="table table-hover table-striped" UseAccessibleHeader="true" EmptyDataText="Sem registros" Caption="Cenários de Teste" HeaderStyle-Font-Bold="true" ShowHeader="true"
                                        GridLines="None" runat="server" ID="gridCenarios" AutoGenerateColumns="false" OnRowCommand="gridCenarios_RowCommand">
                                        <RowStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                        <Columns>
                                            <asp:BoundField DataField="CenarioTesteId" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                            <asp:BoundField DataField="Descricao" HeaderText="Descrição" />
                                            <asp:BoundField DataField="Situacao" HeaderText="Situação" />
                                            <asp:ButtonField HeaderStyle-Width="20" ButtonType="Image" HeaderText="Editar" ShowHeader="True" CommandName="Editar" ImageUrl="~/Content/Images/pencil.png">
                                                <ControlStyle CssClass="grid-icon" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:ButtonField>
                                            <asp:ButtonField ButtonType="Image" HeaderText="Deletar" ShowHeader="True" CommandName="Deletar" ImageUrl="~/Content/Images/cross.png">
                                                <ControlStyle CssClass="grid-icon" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:ButtonField>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <script type="text/javascript">
        function openSuccessMsg() {
            document.getElementById("alertSuccess").focus();
            $('#alertSuccess').show("fade");
            setTimeout(function () {
                $('#alertSuccess').hide("fade");
            }, 8000);
            $("html, body").animate({ scrollTop: 0 }, "slow");
        }
        function openErrorMsg() {
            $('#alertError').show("fade");
            setTimeout(function () {
                $('#alertError').hide("fade");
            }, 10000);
            $("html, body").animate({ scrollTop: 0 }, "slow");
        }
    </script>
</asp:Content>

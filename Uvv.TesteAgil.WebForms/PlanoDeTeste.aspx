<%@ Page Title="Plano de Teste" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PlanoDeTeste.aspx.cs" Inherits="Uvv.TesteAgil.WebForms.PlanoDeTeste" %>

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
            margin: 50px;
            display: inline-block;
            float: left;
        }

        #exTab2 h3 {
            color: white;
            background-color: #428bca;
            padding: 5px 15px;
        }

        .margemPadrao {
            margin-top: 20px;
            margin-bottom: 20px;
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
            border-width: 1px;
            border-color: #cccccc;
            border-style: solid;
            border-radius: 10px;
        }

        .fa fa-search {
            background-color:aquamarine;
        }

        .txt {
            float: left;
        }

        td h1 {
            background-color: silver;
            color: white;
            font-family: Anna;
            border: thin;
        }

        .jumbotron {
            background-color: dimgray;
            color: white;
            font-family: Anna;
        }
    </style>
    <div class="jumbotron">
        <h1 style="font-size: 24pt;">Plano de Teste</h1>
    </div>
    <div class="container">
        <asp:UpdatePanel runat="server" ID="updPlano">
            <ContentTemplate>
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
                <table style="width: 100%; text-align: center;">
                    <tr>
                        <td>
                            <%-- Div Geral --%>
                            <div runat="server" id="divGeral">
                                <table class="tab-table">
                                    <tr>
                                        <td>
                                            <h1>Geral</h1>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%;">
                                            <div class="formulario">
                                                <asp:UpdatePanel runat="server">
                                                    <ContentTemplate>
                                                        <asp:HiddenField runat="server" ID="txtId" OnValueChanged="txtId_ValueChanged" />
                                                        <div class="input-group margemPadrao dropStyle">
                                                            <span class="input-group-addon">Projeto</span>
                                                            <asp:DropDownList CssClass="form-control dropStyle" runat="server" ID="dblProjetos" AutoPostBack="true" OnSelectedIndexChanged="dblProjetos_SelectedIndexChanged" />
                                                        </div>
                                                        <div class="input-group margemPadrao dropStyle">
                                                            <span class="input-group-addon">Sprint</span>
                                                            <asp:DropDownList CssClass="form-control dropStyle" runat="server" ID="dblSprints" AutoPostBack="true" OnSelectedIndexChanged="dblSprints_SelectedIndexChanged" />
                                                        </div>
                                                        <div class="input-group">
                                                            <span class="input-group-addon">Descrição</span>
                                                            <asp:TextBox CssClass="form-control txt" runat="server" ID="txtDescricao"></asp:TextBox>
                                                            <asp:Label CssClass="lb-erro" runat="server" ID="erroDescricao"></asp:Label>
                                                        </div>
                                                        <div class="input-group margemPadrao">
                                                            <span class="input-group-addon">Data de criação</span>
                                                            <asp:TextBox runat="server" TextMode="Date" CssClass="form-control" ID="txtDataCriacao"></asp:TextBox>
                                                            <asp:Label CssClass="lb-erro" runat="server" ID="erroDataCriacao"></asp:Label>
                                                        </div>
                                                        <div class="input-group margemPadrao">
                                                            <span class="input-group-addon">Data de finalização</span>
                                                            <asp:TextBox runat="server" TextMode="Date" CssClass="form-control" ID="txtDataFim"></asp:TextBox>
                                                            <asp:Label CssClass="lb-erro" runat="server" ID="erroDataFim"></asp:Label>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <div class="input-group margemPadrao">
                                                    <asp:UpdatePanel runat="server">
                                                        <ContentTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td colspan="2"><span class="input-group-addon">Funcionalidades testadas</span></td>
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
                                                                    <td colspan="2"><span class="input-group-addon">Funcionalidades não testadas</span>
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
                                                    <div class="modal-footer">
                                                        <asp:Button runat="server" CssClass="btn btn-primary" Text="Salvar" ID="btnSalvar" OnClick="btnSalvar_Click" />
                                                        <asp:Button runat="server" CssClass="btn btn-primary" Text="Cenários" ID="btnCenarios" Visible="false" OnClick="btnCenarios_Click" />
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>

                            <%-- Div Cenários de Teste --%>
                            <div id="divCenario" runat="server" visible="false">
                                <table class="tab-table">
                                    <tr>
                                        <td>
                                            <h1>Cenário de Teste</h1>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="formulario">
                                                <asp:UpdatePanel runat="server">
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
                                                                <asp:ListItem Value="2" Text="Desenvolvimeto"></asp:ListItem>
                                                                <asp:ListItem Value="3" Text="Teste"></asp:ListItem>
                                                                <asp:ListItem Value="4" Text="Concluído"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:Label CssClass="lb-erro" runat="server" ID="erroSituacao"></asp:Label>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <div class="modal-footer">
                                                    <asp:Button runat="server" CssClass="btn btn-primary" Text="Salvar" ID="btnSalvarCenario" OnClick="btnSalvarCenario_Click" />
                                                    <asp:Button runat="server" CssClass="btn btn-primary" Text="Cancelar" ID="btnCancelarEditarCenario" OnClick="btnCancelarEditarCenario_Click" Visible="false" />
                                                    <asp:Button runat="server" CssClass="btn btn-primary" Text="Voltar" ID="btnVoltarGeral" OnClick="btnVoltarGeral_Click" />
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
                                                                    <asp:ButtonField ButtonType="Image" HeaderText="Caso de Teste" ShowHeader="True" CommandName="Caso" ImageUrl="~/Content/Images/novo.png">
                                                                        <ControlStyle CssClass="grid-icon" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:ButtonField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>

                            <%-- Div de Caso de Teste --%>
                            <div id="divCaso" runat="server" visible="false">
                                <table class="tab-table">
                                    <tr>
                                        <td>
                                            <h1>Caso de Teste</h1>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="formulario">
                                                <asp:UpdatePanel runat="server">
                                                    <ContentTemplate>
                                                        <asp:HiddenField runat="server" ID="txtIdCaso" />
                                                        <div class="input-group margemPadrao">
                                                            <span class="input-group-addon">Cenário</span>
                                                            <asp:TextBox CssClass="form-control txt" runat="server" ID="txtCenarioDescricao" Enabled="false" />
                                                        </div>
                                                        <div class="input-group margemPadrao">
                                                            <span class="input-group-addon">Descrição</span>
                                                            <asp:TextBox CssClass="form-control txt" runat="server" ID="txtDescricaoCaso" />
                                                            <asp:Label CssClass="lb-erro" runat="server" ID="erroDescricaoCaso"></asp:Label>
                                                        </div>
                                                        <div class="input-group margemPadrao">
                                                            <span class="input-group-addon">Categoria</span>
                                                            <asp:TextBox CssClass="form-control txt" runat="server" ID="txtCategoria" />
                                                            <asp:Label CssClass="lb-erro" runat="server" ID="erroCategoria"></asp:Label>
                                                        </div>
                                                        <div class="input-group margemPadrao">
                                                            <span class="input-group-addon">Entrada</span>
                                                            <asp:TextBox CssClass="form-control txt" runat="server" ID="txtEntrada" />
                                                            <asp:Label CssClass="lb-erro" runat="server" ID="erroEntrada"></asp:Label>
                                                        </div>
                                                        <div class="input-group margemPadrao">
                                                            <span class="input-group-addon">Resposta Esperada</span>
                                                            <asp:TextBox CssClass="form-control txt" runat="server" ID="txtRespostaEsperada" />
                                                            <asp:Label CssClass="lb-erro" runat="server" ID="erroRespostaEsperada"></asp:Label>
                                                        </div>
                                                        <asp:LinkButton ID="btnAbrirPassos" CssClass="fa-search" Text="Passos" runat="server" OnClick="btnAbrirPassos_Click" ></asp:LinkButton>
                                                        <div runat="server" id="divPassos" visible="false">
                                                            <div class="input-group margemPadrao">
                                                                <h2>Passos</h2>
                                                            </div>
                                                            <div class="input-group margemPadrao">
                                                                <span class="input-group-addon">Descrição</span>
                                                                <asp:TextBox CssClass="form-control txt" runat="server" ID="txtDescricaoPasso" AutoPostBack="true" />
                                                                <asp:Label CssClass="lb-erro" runat="server" ID="erroDescricaoPasso"></asp:Label>
                                                            </div>
                                                            <div class="input-group margemPadrao">
                                                                <asp:Button runat="server" CssClass="btn btn-primary" Text="Adicionar" ID="btnAddPasso" OnClick="btnAddPasso_Click" />
                                                            </div>
                                                            <div class="input-group margemPadrao">
                                                                <asp:GridView CssClass="table table-hover table-striped" UseAccessibleHeader="true" EmptyDataText="Sem registros" Caption="Passos" HeaderStyle-Font-Bold="true" ShowHeader="true"
                                                                    GridLines="None" runat="server" ID="gridPassos" AutoGenerateColumns="false" OnRowCommand="gridPassos_RowCommand">
                                                                    <RowStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                    <Columns>
                                                                        <asp:BoundField DataField="PassoId" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                                                        <asp:BoundField DataField="Numero" HeaderText="Numero" />
                                                                        <asp:BoundField DataField="Descricao" HeaderText="Descricao" />
                                                                        <asp:ButtonField ButtonType="Image" HeaderText="Deletar" ShowHeader="True" CommandName="Deletar" ImageUrl="~/Content/Images/cross.png">
                                                                            <ControlStyle CssClass="grid-icon" />
                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                        </asp:ButtonField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <div class="modal-footer">
                                                    <asp:Button runat="server" CssClass="btn btn-primary" Text="Salvar" ID="btnSalvarCaso" OnClick="btnSalvarCaso_Click" />
                                                    <asp:Button runat="server" CssClass="btn btn-primary" Text="Cancelar" ID="btnCancelarEditarCaso" OnClick="btnCancelarEditarCaso_Click" Visible="false" />
                                                    <asp:Button runat="server" CssClass="btn btn-primary" Text="Voltar" ID="btnVoltarCenario" OnClick="btnVoltarCenario_Click" />
                                                </div>
                                                <div class="input-group margemPadrao">
                                                    <asp:UpdatePanel runat="server">
                                                        <ContentTemplate>
                                                            <asp:GridView CssClass="table table-hover table-striped" UseAccessibleHeader="true" EmptyDataText="Sem registros" Caption="Casos de Teste" HeaderStyle-Font-Bold="true" ShowHeader="true"
                                                                GridLines="None" runat="server" ID="gridCasos" AutoGenerateColumns="false" OnRowCommand="gridCasos_RowCommand">
                                                                <RowStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="CasoTesteId" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                                                    <asp:BoundField DataField="Descricao" HeaderText="Descrição" />
                                                                    <asp:BoundField DataField="Categoria" HeaderText="Categoria" />
                                                                    <asp:BoundField DataField="Entrada" HeaderText="Entrada" />
                                                                    <asp:BoundField DataField="RespostaEsperada" HeaderText="Resposta Esperada" />
                                                                    <asp:ButtonField HeaderStyle-Width="20" ButtonType="Image" HeaderText="Editar" ShowHeader="True" CommandName="Editar" ImageUrl="~/Content/Images/pencil.png">
                                                                        <ControlStyle CssClass="grid-icon" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:ButtonField>
                                                                    <asp:ButtonField ButtonType="Image" HeaderText="Deletar" ShowHeader="True" CommandName="Deletar" ImageUrl="~/Content/Images/cross.png">
                                                                        <ControlStyle CssClass="grid-icon" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:ButtonField>
                                                                    <asp:ButtonField ButtonType="Image" HeaderText="Teste" ShowHeader="True" CommandName="Teste" ImageUrl="~/Content/Images/novo.png">
                                                                        <ControlStyle CssClass="grid-icon" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:ButtonField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>

                            <%-- Div Testes --%>
                            <div id="divTeste" runat="server" visible="false">
                                <table class="tab-table">
                                    <tr>
                                        <td>
                                            <h1>Teste</h1>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="formulario">
                                                <asp:UpdatePanel runat="server">
                                                    <ContentTemplate>
                                                        <asp:HiddenField runat="server" ID="txtIdTeste" />

                                                        <div class="input-group margemPadrao">
                                                            <span class="input-group-addon">Caso de Teste: </span>
                                                            <asp:TextBox CssClass="form-control txt" runat="server" ID="txtCasoTeste" Enabled="false" />
                                                        </div>
                                                        <div class="input-group margemPadrao">
                                                            <span class="input-group-addon">Data</span>
                                                            <asp:TextBox runat="server" TextMode="DateTimeLocal" CssClass="form-control" ID="txtDataTeste"></asp:TextBox>
                                                            <asp:Label CssClass="lb-erro" runat="server" ID="erroDataTeste"></asp:Label>
                                                        </div>
                                                        <div class="input-group margemPadrao">
                                                            <span class="input-group-addon">Desenvolvedor</span>
                                                            <asp:DropDownList CssClass="form-control dropStyle" runat="server" ID="ddlDesenvolvedor" AutoPostBack="true" />
                                                            <asp:Label CssClass="lb-erro" runat="server" ID="erroDesenvolvedor"></asp:Label>
                                                        </div>
                                                        <div class="input-group margemPadrao">
                                                            <span class="input-group-addon">Tester</span>
                                                            <asp:DropDownList CssClass="form-control dropStyle" runat="server" ID="ddlTester" AutoPostBack="true" />
                                                            <asp:Label CssClass="lb-erro" runat="server" ID="erroTester"></asp:Label>
                                                        </div>
                                                        <div class="input-group margemPadrao">
                                                            <span class="input-group-addon">Situação</span>
                                                            <asp:DropDownList ID="ddlSituacaoTeste" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSituacaoTeste_SelectedIndexChanged">
                                                                <asp:ListItem Value="1" Text="Sucesso"></asp:ListItem>
                                                                <asp:ListItem Value="2" Text="Falha"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:Label CssClass="lb-erro" runat="server" ID="erroSituacaoTeste"></asp:Label>
                                                        </div>

                                                        <div runat="server" id="divTipoErro" visible="false">
                                                            <div class="input-group margemPadrao">
                                                                <span class="input-group-addon">Tipos de Erro</span>
                                                                <asp:DropDownList CssClass="form-control dropStyle" runat="server" ID="ddlTipoErro" AutoPostBack="true" />
                                                            </div>
                                                            <div class="input-group margemPadrao">
                                                                <asp:Button runat="server" CssClass="btn btn-primary" Text="Adicionar" ID="btnAddTipoErro" OnClick="btnAddTipoErro_Click" />
                                                            </div>
                                                            <div class="input-group margemPadrao">
                                                                <asp:GridView CssClass="table table-hover table-striped" UseAccessibleHeader="true" EmptyDataText="Sem registros" Caption="Erros" HeaderStyle-Font-Bold="true" ShowHeader="true"
                                                                    GridLines="None" runat="server" ID="gridTipoErro" AutoGenerateColumns="false" OnRowCommand="gridTipoErro_RowCommand">
                                                                    <RowStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                    <Columns>
                                                                        <asp:BoundField DataField="TipoErroId" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                                                        <asp:BoundField DataField="Descricao" HeaderText="Data" />
                                                                        <asp:BoundField DataField="Gravidade" HeaderText="Desenvolvedor" />
                                                                        <asp:ButtonField ButtonType="Image" HeaderText="Deletar" ShowHeader="True" CommandName="Deletar" ImageUrl="~/Content/Images/cross.png">
                                                                            <ControlStyle CssClass="grid-icon" />
                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                        </asp:ButtonField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </div>
                                                        </div>

                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <div class="modal-footer">
                                                    <asp:Button runat="server" CssClass="btn btn-primary" Text="Salvar" ID="btnSalvarTeste" OnClick="btnSalvarTeste_Click" />
                                                    <asp:Button runat="server" CssClass="btn btn-primary" Text="Cancelar" ID="btnCancelarEditarTeste" OnClick="btnCancelarEditarTeste_Click" Visible="false" />
                                                    <asp:Button runat="server" CssClass="btn btn-primary" Text="Voltar" ID="btnVoltarCaso" OnClick="btnVoltarCaso_Click" />
                                                </div>
                                                <div class="input-group margemPadrao">
                                                    <asp:UpdatePanel runat="server">
                                                        <ContentTemplate>
                                                            <asp:GridView CssClass="table table-hover table-striped" UseAccessibleHeader="true" EmptyDataText="Sem registros" Caption="Testes" HeaderStyle-Font-Bold="true" ShowHeader="true"
                                                                GridLines="None" runat="server" ID="gridTestes" AutoGenerateColumns="false" OnRowCommand="gridTestes_RowCommand">
                                                                <RowStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="TesteId" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                                                    <asp:BoundField DataField="Data" HeaderText="Data" />
                                                                    <asp:BoundField DataField="Desenvolvedor" HeaderText="Desenvolvedor" />
                                                                    <asp:BoundField DataField="Tester" HeaderText="Tester" />
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
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>

                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
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

        function btnCenariosVisible() {
            $('#btnCenario').show();
        }
        function btnCenarioHide() {
            $('#btnCenario').hide();
        }
    </script>
</asp:Content>

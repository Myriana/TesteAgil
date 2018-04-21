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
            display:inline-block;
            float:left;
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
            .tabmenu-inicial:disabled{
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
        .tabmenu-clicked:disabled{
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

        .txt{
            float:left;
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
                            <asp:Button runat="server" ID="tabGeral" CssClass="tabmenu-clicked" BorderStyle="None" Text="Geral" OnClick="tabGeral_Click" />
                            <asp:Button runat="server" ID="tabCenarios" Enabled="false" CssClass="tabmenu-inicial" BorderStyle="None" Text="Cenários de Teste" OnClick="tabCenarios_Click" />
                            <asp:Button runat="server" ID="tabCasos" Enabled="false" CssClass="tabmenu-inicial" BorderStyle="None" Text="Casos de Teste" OnClick="tabCasos_Click" />
                            <asp:Button runat="server" ID="tabTestes" Enabled="false" CssClass="tabmenu-inicial" BorderStyle="None" Text="Testes" OnClick="tabTestes_Click" />

                            <asp:MultiView runat="server" ID="mvPlano" ActiveViewIndex="0">
                                <asp:View runat="server" ID="viewGeral">
                                    <table class="tab-table">
                                        <tr>
                                            <td style="width:100%;">
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
                                                                    </tr><tr style="margin:0; padding:0;"><td><asp:Label CssClass="lb-erro" runat="server" ID="erroFuncionalidadesTestadas"></asp:Label></td></tr>
                                                                </table>
                                                                
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                        <div class="modal-footer">
                                                            <asp:Button runat="server" CssClass="btn btn-primary" Text="Salvar" ID="btnSalvar" OnClick="btnSalvar_Click" />
                                                        </div>
                                                    </div>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:View>
                                <asp:View runat="server" ID="viewCenarios">
                                    <table class="tab-table">
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
                                                                <asp:Button Enabled="false" runat="server" CssClass="btn btn-primary" Text="Salvar" ID="btnSalvarCenario" OnClick="btnSalvarCenario_Click" />
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
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:View>
                                <asp:View runat="server" ID="viewCasos">
                                    <table class="tab-table">
                                        <tr>
                                            <td>view 3
                                            </td>
                                        </tr>
                                    </table>
                                </asp:View>
                                <asp:View runat="server" ID="viewTestes">
                                    <table class="tab-table">
                                        <tr>
                                            <td>view 4
                                            </td>
                                        </tr>
                                    </table>
                                </asp:View>
                            </asp:MultiView>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>


        <%--        <div id="tabs">
            <ul class="nav nav-tabs">
                <li class="active">
                    <a href="#num1" data-toggle="tab">Geral</a>
                </li>
                <li><a href="#num2" data-toggle="tab">Cenário de Teste</a>
                </li>
                <li><a href="#num3" data-toggle="tab">Caso de Teste</a>
                </li>
                <li><a href="#num4" data-toggle="tab">Teste</a>
                </li>
            </ul>

            <div class="tab-content">
                <div class="tab-pane active" id="num1">
                    <div class="formulario">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:HiddenField runat="server" ID="txtId" OnValueChanged="txtId_ValueChanged" />
                                <div class="input-group margemPadrao">
                                    <span class="input-group-addon">Projeto</span>
                                    <asp:DropDownList CssClass="form-control dropStyle" runat="server" ID="dblProjetos" AutoPostBack="true" OnSelectedIndexChanged="dblProjetos_SelectedIndexChanged" />
                                </div>
                                <div class="input-group margemPadrao">
                                    <span class="input-group-addon">Sprint</span>
                                    <asp:DropDownList CssClass="form-control dropStyle" runat="server" ID="dblSprints" Enabled="false" AutoPostBack="false" OnSelectedIndexChanged="dblSprints_SelectedIndexChanged" />
                                </div>
                                <div class="input-group margemPadrao">
                                    <span class="input-group-addon">Descrição</span>
                                    <asp:TextBox CssClass="form-control" runat="server" ID="txtDescricao"></asp:TextBox>
                                    <asp:Label CssClass="lb-erro" runat="server" ID="erroDescricao"></asp:Label>
                                </div>
                                <div class="input-group margemPadrao">
                                    <span class="input-group-addon">Data de criação</span>
                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtDataCriacao"></asp:TextBox>
                                    <asp:Label CssClass="lb-erro" runat="server" ID="erroDataCriacao"></asp:Label>
                                </div>
                                <div class="input-group margemPadrao">
                                    <span class="input-group-addon">Data de finalização</span>
                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtDataFim"></asp:TextBox>
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
                                                <asp:Button CssClass="btn btn-default" Enabled="false" ID="btnAddNaoTestada" Width="150" runat="server" Text="Add Não Testadas" OnClick="btnAddNaoTestada_Click" />
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
                                                <asp:Button CssClass="btn btn-default" Enabled="false" ID="btnAddTestada" Width="150" runat="server" Text="Add Testadas" OnClick="btnAddTestada_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                    <div>
                                        <asp:Button runat="server" CssClass="btn btn-primary" Text="Salvar" ID="btnSalvar" OnClick="btnSalvar_Click" />
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </div>
                    </div>
                </div>

                <div class="tab-pane" id="num2">
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
                                <div>
                                    <asp:Button Enabled="false" runat="server" CssClass="btn btn-primary" Text="Salvar" ID="btnSalvarCenario" OnClick="btnSalvarCenario_Click" />
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
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
                    </div>
                </div>

                <div class="tab-pane" id="num3">
                    <h3>add clearfix to tab-content (see the css)</h3>
                </div>

                <div class="tab-pane" id="num4">
                    <h3>teste</h3>
                </div>
            </div>
        </div>--%>
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

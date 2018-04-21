<%@ Page Title="Script de Teste" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ScriptDeTeste.aspx.cs" Inherits="Uvv.TesteAgil.WebForms.ScriptDeTeste" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .hiddencol {
            display: none;
        }

        .btn-distancia {
            margin-right: 20px;
            margin-top: 10px;
        }

        .panel-default {
            border-color: #808080 !important;
            width:100%;
            padding: 20px;
            margin: 10px;
            width: 100% !important;
        }
    </style>
    <div class="jumbotron">
        <h1 style="font-size: 24pt;">Script de Teste</h1>
    </div>
    <div class="container">
        <div id="alertSuccess" class="alert alert-success collapse">
            <a id="closeSuccess" href="#" class="close">&times;</a>
            <strong><asp:Label runat="server" ID="msgSucesso"></asp:Label></strong>
        </div>

        <div id="alertError" class="alert alert-danger collapse">
            <a href="#" id="closeError" class="close" data-dismiss="alert">&times;</a>
            <strong><asp:Label runat="server" ID="msgErro"></asp:Label></strong>
        </div>
        <asp:Button runat="server" ID="btnCriar" CssClass="btn btn-primary" OnClick="btnCriar_Click" Text="Novo Script de Teste" />
        <asp:GridView CssClass="table table-hover table-striped" UseAccessibleHeader="true" EmptyDataText="Sem registros" Caption="Scripts de Teste" HeaderStyle-Font-Bold="true" ShowHeader="true"
            GridLines="None" runat="server" ID="gridScriptTeste" AutoGenerateColumns="false" OnRowCommand="gridScriptTeste_RowCommand">
            <RowStyle HorizontalAlign="Left" VerticalAlign="Middle" />
            <Columns>
                <asp:BoundField DataField="ScriptTesteId" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                <asp:BoundField DataField="Nome" HeaderText="Nome" />
                <asp:ButtonField ButtonType="Image" HeaderText="Visualizar" ShowHeader="True" CommandName="Visualizar" ImageUrl="~/Content/Images/glass.png">
                    <ControlStyle CssClass="grid-icon" />
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" Width="70" />
                </asp:ButtonField>
                <asp:ButtonField ButtonType="Image" HeaderText="Editar" ShowHeader="True" CommandName="Editar" ImageUrl="~/Content/Images/pencil.png">
                    <ControlStyle CssClass="grid-icon" />
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" Width="70" />
                </asp:ButtonField>
                <asp:ButtonField ButtonType="Image" HeaderText="Deletar" ShowHeader="True" CommandName="Deletar" ImageUrl="~/Content/Images/cross.png">
                    <ControlStyle CssClass="grid-icon" />
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" Width="70" />
                </asp:ButtonField>
            </Columns>
        </asp:GridView>
    </div>

    <!-- Modal -->
    <div class="modal fade" id="modalSt" tabindex="-1" role="dialog" aria-labelledby="lbModel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="lbModel">Script de Teste</h5>
                </div>
                <div class="modal-body">
                    <div class="container" data-toggle="validator">
                        <div class="input-group">
                            <asp:HiddenField ID="txtId" runat="server" />
                            <span class="input-group-addon">Nome</span>
                            <input type="text" class="form-control" id="txtNome" name="txtNome" runat="server">
                            <asp:Label CssClass="lb-erro" runat="server" ID="erroNome" />
                        </div>
                        <div class="input-group">
                            <div class="panel panel-default">
                                <div class="panel-heading">Passos</div>
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <div id="divEditar" runat="server" visible="true">
                                    <div class="panel-body">
                                        <div class="input-group">
                                            <span class="input-group-addon">Descrição</span>
                                            <input type="text" class="form-control" id="txtDescricao" name="txtNome" runat="server">
                                            <asp:HiddenField ID="txtIdPasso" runat="server" />
                                        </div>
                                        <div class="input-group">
                                            <asp:LinkButton runat="server" ID="btnAdicionar" CssClass="btn btn-primary btn-distancia" Text="Adicionar" OnClick="btnAdicionar_Click"></asp:LinkButton>
                                        </div>
                                        <asp:GridView CssClass="table table-hover table-striped" UseAccessibleHeader="true" EmptyDataText="Sem registros" Caption="Passos" HeaderStyle-Font-Bold="true" ShowHeader="true"
                                            runat="server" ID="gridPassos" AutoGenerateColumns="false" OnRowCommand="gridPassos_RowCommand" ShowHeaderWhenEmpty="true">
                                            <RowStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <Columns>
                                                <asp:BoundField DataField="PassoId" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                                <asp:BoundField DataField="Numero" HeaderText="Número" />
                                                <asp:BoundField DataField="Descricao" HeaderText="Descrição" />
                                                <asp:ButtonField ButtonType="Image" HeaderText="Deletar" ShowHeader="True" CommandName="Deletar" ImageUrl="~/Content/Images/cross.png">
                                                    <ControlStyle CssClass="grid-icon" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" Width="70" />
                                                </asp:ButtonField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:Label CssClass="lb-erro" runat="server" ID="erroPassos" />
                                    </div>
                                </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                
                                <div id="divVisualizar" runat="server" visible="false">
                                    <div class="panel-body">
                                        <asp:GridView CssClass="table table-hover table-striped" UseAccessibleHeader="true" EmptyDataText="Sem registros" Caption="Passos" HeaderStyle-Font-Bold="true" ShowHeader="true"
                                            runat="server" ID="gridPassosView" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true">
                                            <RowStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                            <Columns>
                                                <asp:BoundField DataField="PassoId" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                                <asp:BoundField DataField="Numero" HeaderText="Número" />
                                                <asp:BoundField DataField="Descricao" HeaderText="Descrição" />
                                            </Columns>
                                        </asp:GridView>
                                        <asp:Label CssClass="lb-erro" runat="server" ID="Label1" />
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button runat="server" type="button" ID="btnCancelar" class="btn btn-secondary" OnClick="btnCancelar_Click" Text="Cancelar" />
                    <asp:Button runat="server" CssClass="btn btn-primary" Text="Salvar" ID="btnSalvar" OnClick="btnSalvar_Click" />
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function openModal() {
            $('[id*=modalSt]').modal('show');
        }

        function closeModal() {
            $('[id*=modalSt]').modal('hide');
        }

        function limparCamposModal() {
            debugger;
            document.getElementById('txtTipoErroId').value = "";
            document.getElementById('txtDescricao').value = "";
            document.getElementById('ddlGravidade').value = "1";
        }

        function openSuccessMsg() {
            debugger;
            $('#alertSuccess').show("fade");
            setTimeout(function () {
                $('#alertSuccess').hide("fade");
            }, 5000);
        }
        function openErrorMsg() {
            $('#alertError').show("fade");
            setTimeout(function () {
                $('#alertError').hide("fade");
            }, 5000);
        }
    </script>
</asp:Content>

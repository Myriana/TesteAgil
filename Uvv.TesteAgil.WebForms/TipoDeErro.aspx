<%@ Page Title="Tipo de Erro" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TipoDeErro.aspx.cs" Inherits="Uvv.TesteAgil.WebForms.TipoDeErro" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h1 style="font-size: 24pt;">Tipo de Erro</h1>
    </div>
    <div id="alertSuccess" class="alert alert-success collapse">
            <a id="closeSuccess" href="#" class="close">&times;</a>
            <strong><asp:Label runat="server" ID="msgSucesso"></asp:Label></strong>
        </div>

        <div id="alertError" class="alert alert-danger collapse">
            <a href="#" id="closeError" class="close" data-dismiss="alert">&times;</a>
            <strong><asp:Label runat="server" ID="msgErro"></asp:Label></strong>
        </div>
    <div class="container">
        
        
        <%--<asp:Button runat="server" ID="btnCriar" CssClass="btn btn-primary" Text="Novo Tipo de Erro" OnClick="btnCriar_Click" />--%>
        <asp:UpdatePanel ID="updTpErro" runat="server">
            <ContentTemplate>
                
                <div class="container" data-toggle="validator">
                    <table>
                        <tr>
                            <td>
                                <asp:HiddenField ID="txtTipoErroId" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="input-group">
                                    <span style="width:30px" class="input-group-addon">Descrição</span>
                            <asp:TextBox class="form-control" ID="txtDescricao" name="txtDescricao" runat="server" />
                                </div>             
                            </td>
                            <td>
                                 <asp:Label CssClass="lb-erro" runat="server" ID="erroDescricao" />
                             </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="input-group">
                                    <span style="width:30px"  class="input-group-addon ddl-padrao">Gravidade</span>
                            <asp:DropDownList ID="ddlGravidade" CssClass="form-control" runat="server">
                                <asp:ListItem Value="1" Text="Baixo"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Médio"></asp:ListItem>
                                <asp:ListItem Value="3" Text="Alto"></asp:ListItem>
                            </asp:DropDownList>
                                </div>
                                
                            </td>
                            <td>

                            </td>
                        </tr>
                    </table>            
                    </div>
        
    </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="modal-footer">
                 <asp:Button runat="server" type="button" id="btnCancelar" class="btn btn-secondary" OnClick="btnCancelar_Click" Text="Cancelar" />
                 <asp:Button runat="server" CssClass="btn btn-primary" Text="Salvar" ID="btnSalvar" OnClick="btnSalvar_Click" />
             </div>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                 <asp:GridView CssClass="table table-hover table-striped" UseAccessibleHeader="true" EmptyDataText="Sem registros" Caption="Tipos de Erro" HeaderStyle-Font-Bold="true" ShowHeader="true"
            GridLines="None" runat="server" ID="gridTipoErro" AutoGenerateColumns="false" OnRowCommand="gridTipoErro_RowCommand">
            <RowStyle HorizontalAlign="Left" VerticalAlign="Middle" />
            <Columns>
                <asp:BoundField DataField="TipoErroId" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                <asp:BoundField DataField="Descricao" HeaderText="Descrição" />
                <asp:BoundField DataField="Gravidade" HeaderText="Gravidade" />
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
         
    <!-- Modal -->
    <div class="modal fade" id="modalTp" tabindex="-1" role="dialog" aria-labelledby="lbModel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="lbModel">Tipo de Erro</h5>
                </div>
                <div class="modal-body">
                   <%-- <div class="container" data-toggle="validator">
                        <div class="input-group">
                            <asp:HiddenField ID="txtTipoErroId" runat="server" />
                            <span style="width:30px" class="input-group-addon">Descrição</span>
                            <asp:TextBox class="form-control" ID="txtDescricao" name="txtDescricao" runat="server" />
                            <asp:Label CssClass="lb-erro" runat="server" ID="erroDescricao" />
                        </div>
                        <div class="input-group">
                            <span style="width:30px"  class="input-group-addon ddl-padrao">Gravidade</span>
                            <asp:DropDownList ID="ddlGravidade" CssClass="form-control" runat="server">
                                <asp:ListItem Value="1" Text="Baixo"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Médio"></asp:ListItem>
                                <asp:ListItem Value="3" Text="Alto"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>--%>
                </div>
                <div class="modal-footer">
                    <%--<asp:Button runat="server" type="button" id="btnCancelar" class="btn btn-secondary" OnClick="btnCancelar_Click" Text="Cancelar" />--%>
                    <%--<asp:Button runat="server" CssClass="btn btn-primary" Text="Salvar" ID="btnSalvar" OnClick="btnSalvar_Click" />--%>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function openModal() {
            $('[id*=modalTp]').modal('show');
        }

        function closeModal() {
            $('[id*=modalTp]').modal('hide');
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

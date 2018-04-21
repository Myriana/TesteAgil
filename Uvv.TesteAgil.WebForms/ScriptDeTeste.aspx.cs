using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Uvv.TesteAgil.Dados.Repositorio;
using Uvv.TesteAgil.Entidades.Modelos;
using Uvv.TesteAgil.WebForms.Util;

namespace Uvv.TesteAgil.WebForms
{
    public partial class ScriptDeTeste : PaginaBase
    {
        ScriptTesteRepositorio repo = new ScriptTesteRepositorio();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ValidarSessao();
                RecarregarGrid();

                if (gridPassos.Rows.Count == 0)
                    gridPassos.DataBind();
                if (gridScriptTeste.Rows.Count == 0)
                    gridScriptTeste.DataBind();
            }
        }

        protected void gridPassos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(e.CommandArgument);

                if (e.CommandName == "Deletar")
                {
                    try
                    {
                        List<Passo> passos = new List<Passo>();
                        if (gridPassos.Rows.Count > 0)
                        {
                            foreach (GridViewRow row in gridPassos.Rows)
                            {
                                if (row.RowIndex != index)
                                {
                                    var passo = new Passo();
                                    passo.PassoId = string.IsNullOrEmpty(row.Cells[0].Text) ? 0 : int.Parse(row.Cells[0].Text);
                                    passo.Descricao = row.Cells[2].Text;
                                    passos.Add(passo);
                                }
                            }
                            int numero = 1;
                            foreach(var passo in passos)
                            {
                                passo.Numero = numero;
                                numero++;
                            }

                            gridPassos.DataSource = null;
                            gridPassos.DataSource = passos;
                            gridPassos.DataBind();
                            ClientScript.RegisterStartupScript(this.GetType(), "Pop", "openModal();", true);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Ocorreu um erro ao deletar. Erro: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                msgErro.Text = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Pop", "openErrorMsg();", true);
                msgErro.DataBind();
            }
        }

        protected void btnAdicionar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtDescricao.Value))
                    throw new Exception("Favor informar a descrição do passo antes de adicionar");

                List<Passo> passos = new List<Passo>();
                if (gridPassos.Rows.Count > 0)
                {

                    foreach (GridViewRow row in gridPassos.Rows)
                    {
                        var passo = new Passo();
                        passo.PassoId = string.IsNullOrEmpty(row.Cells[0].Text) ? 0 : int.Parse(row.Cells[0].Text);
                        passo.Numero = string.IsNullOrEmpty(row.Cells[1].Text) ? 0 : int.Parse(row.Cells[1].Text);
                        var descricaoEncoded = row.Cells[2].Text;
                        passo.Descricao = Context.Server.HtmlDecode(descricaoEncoded);
                       
                        passos.Add(passo);
                    }

                    var novoPasso = new Passo();
                    novoPasso.PassoId = 0;
                    novoPasso.Numero = gridPassos.Rows.Count + 1;
                    novoPasso.Descricao = txtDescricao.Value;
                    passos.Add(novoPasso);
                }
                else
                {
                    var novoPasso = new Passo();
                    novoPasso.PassoId = 0;
                    novoPasso.Numero = gridPassos.Rows.Count + 1;
                    novoPasso.Descricao = txtDescricao.Value;
                    passos.Add(novoPasso);
                }
                txtDescricao.Value = string.Empty;

                gridPassos.DataSource = null;
                gridPassos.DataSource = passos;
                gridPassos.DataBind();
                LimparCamposPassos();
                ClientScript.RegisterStartupScript(this.GetType(), "Pop", "openModal();", true);
            }
            catch (Exception ex)
            {
                msgErro.Text = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Pop", "openErrorMsg();", true);
                msgErro.DataBind();
            }
        }

        protected void gridScriptTeste_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gridScriptTeste.Rows[index];
                if (row.Cells.Count >= 3)
                {
                    int id = 0;
                    int.TryParse(row.Cells[0].Text, out id);
                    if (id == 0)
                        throw new Exception("Id não pode ser zero");
                    var scriptTeste = repo.ObterScriptTestePorId(id);
                    if (scriptTeste == null)
                        throw new Exception("Script de Teste não encontrado");

                    if (e.CommandName == "Editar")
                    {
                        try
                        {
                            divVisualizar.Visible = false;
                            divEditar.Visible = true;
                            ;
                            btnSalvar.Visible = true;
                            txtId.Value = scriptTeste.ScriptTesteId.ToString();
                            txtNome.Value = scriptTeste.Nome;
                            gridPassos.DataSource = null;
                            gridPassos.DataSource = scriptTeste.Passos != null ? scriptTeste.Passos.ToList() : null;
                            gridPassos.DataBind();
                            ClientScript.RegisterStartupScript(this.GetType(), "Pop", "openModal();", true);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Ocorreu um erro ao editar. Erro: " + ex.Message);
                        }
                    }
                    else if (e.CommandName == "Deletar")
                    {
                        try
                        {
                            if (DialogResult.Yes == MessageBox.Show("Tem certeza que deseja apagar o registro?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                            {
                                //var usado = repo.VerificarUso(scriptTeste.ScriptTesteId);
                                //if (usado)
                                //    throw new Exception("O Script de Teste não pode ser deletado pois está sendo usado");

                                repo.DeletarTodosPassosScript(scriptTeste.ScriptTesteId);
                                repo.Deletar(scriptTeste);
                                repo.Commit();
                                RecarregarGrid();

                                msgSucesso.Text = "Script de Teste deletado com sucesso";
                                msgSucesso.DataBind();
                                ClientScript.RegisterStartupScript(this.GetType(), "Pop", "openSuccessMsg();", true);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Ocorreu um erro ao deletar. Erro: " + ex.Message);
                        }
                    }
                    else if (e.CommandName == "Visualizar")
                    {
                        divVisualizar.Visible = true;
                        divEditar.Visible = false;

                        gridPassosView.Visible = true;
                        txtNome.Value = string.Empty;
                        gridPassosView.DataSource = null;

                        txtNome.Value = scriptTeste.Nome;
                        gridPassosView.DataSource = scriptTeste.Passos.ToList();
                        gridPassosView.DataBind();

                        ClientScript.RegisterStartupScript(this.GetType(), "Pop", "openModal();", true);
                        btnSalvar.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                msgErro.Text = ex.Message;
                ClientScript.RegisterStartupScript(this.GetType(), "Pop", "openErrorMsg();", true);
                msgErro.DataBind();
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtNome.Value) || gridPassos.Rows.Count <= 0)
            {
                if(string.IsNullOrEmpty(txtNome.Value))
                {
                    erroNome.Text = "Campo Obrigatório";
                    erroNome.DataBind();
                }
                if(gridPassos.Rows.Count <= 0)
                {
                    erroPassos.Text = "Adicione pelo menos um passo";
                    erroPassos.DataBind();
                }
                ClientScript.RegisterStartupScript(this.GetType(), "Pop", "openModal();", true);
            }
            else
            {
                try
                {
                    if (gridPassos.Rows.Count <= 0)
                        throw new Exception("Adicione pelo menos um passo");

                    //Cadastrar novo
                    if (string.IsNullOrEmpty(txtId.Value))
                    {
                        //Validar se existe Script de Teste cadastrado com o mesmo nome
                        var existe = repo.ExisteScriptNome(txtNome.Value);
                        if (existe)
                            throw new Exception("Já existe um Script de Teste com este nome");

                        ScriptTeste scriptTeste = new ScriptTeste();
                        scriptTeste.Passos = new List<Passo>();
                        scriptTeste.Nome = txtNome.Value;
                        foreach (GridViewRow row in gridPassos.Rows)
                        {
                            var descricaoEncoded = row.Cells[2].Text;
                            var passo = new Passo();
                            passo.Descricao = Context.Server.HtmlDecode(descricaoEncoded);
                            passo.Numero = int.Parse(row.Cells[1].Text);
                            scriptTeste.Passos.Add(passo);
                        }
                        repo.Adicionar(scriptTeste);
                        repo.Commit();
                    }
                    //Editar
                    else
                    {
                        int id = int.Parse(txtId.Value);
                        var scriptTeste = repo.ObterScriptTestePorId(id);
                        if (scriptTeste == null)
                            throw new Exception("Script de Teste não encontrado");
                        //Se mudar o nome do Script, validar se já existe um script de teste com o novo nome
                        if(scriptTeste.Nome != txtNome.Value)
                        {
                            var existe = repo.ExisteScriptNome(txtNome.Value);
                            if (existe)
                                throw new Exception("Já existe um Script de Teste com este nome");
                        }

                        //Deletar os passos antigos
                        repo.DeletarTodosPassosScript(scriptTeste.ScriptTesteId);

                        //Obter os passos novos
                        List<Passo> passosNovos = new List<Passo>();
                        scriptTeste.Nome = txtNome.Value;
                        foreach (GridViewRow row in gridPassos.Rows)
                        {
                            var descricaoEncoded = row.Cells[2].Text;
                            var passo = new Passo();
                            passo.Numero = int.Parse(row.Cells[1].Text);
                            passo.Descricao = Context.Server.HtmlDecode(descricaoEncoded);
                            passosNovos.Add(passo);
                        }
                        //Adicionar os passos novos
                        repo.AdicionarPassos(passosNovos, scriptTeste);

                        repo.Atualizar(scriptTeste);
                        repo.Commit();
                    }
                    LimparModal();
                    RecarregarGrid();

                    msgSucesso.Text = "Script de Teste salvo com sucesso";
                    msgSucesso.DataBind();
                    ClientScript.RegisterStartupScript(this.GetType(), "Pop", "openSuccessMsg();", true);
                }
                catch (Exception ex)
                {
                    LimparModal();
                    msgErro.Text = ex.Message;
                    ClientScript.RegisterStartupScript(this.GetType(), "Pop", "openErrorMsg();", true);
                    msgErro.DataBind();
                }
            }            
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimparModal();
        }

        private void RecarregarGrid()
        {
            var lista = repo.GetAll().ToList();
            gridScriptTeste.DataSource = null;
            gridScriptTeste.DataSource = lista;
            gridScriptTeste.DataBind();
        }

        private void LimparModal()
        {
            txtNome.Value = string.Empty;
            txtDescricao.Value = string.Empty;
            txtId.Value = string.Empty;
            txtIdPasso.Value = string.Empty;
            gridPassos.DataSource = null;
            gridPassos.DataBind();
            gridPassosView.DataSource = null;
            gridPassosView.DataBind();
            txtNome.DataBind();
            txtDescricao.DataBind();
            txtId.DataBind();
            txtIdPasso.DataBind();
            btnSalvar.Visible = true;
        }

        private void LimparCamposPassos()
        {
            //Sempre limpar as mensagens de erro
            erroNome.Text = string.Empty;
            erroPassos.Text = string.Empty;
            erroNome.DataBind();
            erroPassos.DataBind();
            //Limpar campos de preenchimento dos passos
            txtDescricao.Value = string.Empty;
            txtIdPasso.Value = string.Empty;
            txtDescricao.DataBind();
            txtIdPasso.DataBind();
        }

        protected void btnCriar_Click(object sender, EventArgs e)
        {
            divVisualizar.Visible = false;
            divEditar.Visible = true;
            LimparModal();
            ClientScript.RegisterStartupScript(this.GetType(), "Pop", "openModal();", true);
        }
    }
}
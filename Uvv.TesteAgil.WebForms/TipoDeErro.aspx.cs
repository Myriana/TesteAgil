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
    public partial class TipoDeErro : PaginaBase
    {
        TipoErroRepositorio repo = new TipoErroRepositorio();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ValidarSessao();
                RecarregarGrid();
            }
        }

        private void RecarregarGrid()
        {
            var lista = repo.GetAll();
            gridTipoErro.DataSource = null;
            gridTipoErro.DataSource = lista.ToList();
            gridTipoErro.DataBind();
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtDescricao.Text))
            {
                erroDescricao.Text = "Campo obrigatório";
                erroDescricao.DataBind();
                //ClientScript.RegisterStartupScript(this.GetType(), "Pop", "openModal();", true);
            }
            else
            {
                try
                {
                    //Cadastrar novo
                    if (string.IsNullOrEmpty(txtTipoErroId.Value))
                    {
                        //Validar se existe tipo de erro com a mesma descrição
                        var existe = repo.ExisteTipoErroDescricao(txtDescricao.Text);
                        if(existe)
                            throw new Exception("Já existe um Tipo de Erro com esta descrição");

                        var tpErro = new TipoErro
                        {
                            Descricao = txtDescricao.Text,
                            Gravidade = ddlGravidade.SelectedItem.Text
                        };

                        repo.Adicionar(tpErro);
                        repo.Commit();       
                    }
                    //Editar
                    else
                    {
                        int id = int.Parse(txtTipoErroId.Value);
                        var tipoErro = repo.ObterTipoErroPorId(id);
                        if (tipoErro == null)
                            throw new Exception("Tipo de Erro não encontrado");

                        //Se mudar a descrição do tipo de erro, validar se existe tipo de erro com a mesma descrição
                        if(tipoErro.Descricao != txtDescricao.Text)
                        {
                            var existe = repo.ExisteTipoErroDescricao(txtDescricao.Text);
                            if (existe)
                                throw new Exception("Já existe um Tipo de Erro com esta descrição");
                        }

                        tipoErro.Descricao = txtDescricao.Text;
                        tipoErro.Gravidade = ddlGravidade.SelectedItem.Text;

                        repo.Atualizar(tipoErro);
                        repo.Commit();
                    }
                    LimparCamposModal();
                    RecarregarGrid();

                    msgSucesso.Text = "Tipo de Erro cadastrado com sucesso";
                    ClientScript.RegisterStartupScript(this.GetType(), "Pop", "openSuccessMsg();", true);
                    msgSucesso.DataBind();                    
                }
                catch (Exception ex)
                {
                    LimparCamposModal();
                    msgErro.Text =  ex.Message;
                    ClientScript.RegisterStartupScript(this.GetType(), "Pop", "openErrorMsg();", true);
                    msgErro.DataBind();
                }
            }
        }

        private void LimparCamposModal()
        {
            txtDescricao.Text = string.Empty;
            txtTipoErroId.Value = string.Empty;
            ddlGravidade.SelectedIndex = 0;            

            txtDescricao.DataBind();
            txtTipoErroId.DataBind();
            ddlGravidade.DataBind();

            LimparMsgErro();
        }

        private void LimparMsgErro()
        {
            erroDescricao.Text = string.Empty;
            erroDescricao.DataBind();
        }

        protected void gridTipoErro_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                LimparMsgErro();
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gridTipoErro.Rows[index];
                if (row.Cells.Count >= 3)
                {
                    int id = 0;
                    int.TryParse(row.Cells[0].Text, out id);
                    if (id == 0)
                        throw new Exception("Id não pode ser zero");
                    var tipoErro = repo.ObterTipoErroPorId(id);
                    if (tipoErro == null)
                        throw new Exception("Tipo de erro não encontrado");

                    if (e.CommandName == "Editar")
                    {
                        try
                        {
                            txtTipoErroId.Value = tipoErro.TipoErroId.ToString();
                            txtDescricao.Text = tipoErro.Descricao;
                            //Context.Server.HtmlDecode(descricaoEncoded);
                            if (tipoErro.Gravidade == "Médio")
                                ddlGravidade.SelectedValue = "2";
                            else if (tipoErro.Gravidade == "Alto")
                                ddlGravidade.SelectedValue = "3";
                            else
                                ddlGravidade.SelectedValue = "1";

                            //ClientScript.RegisterStartupScript(this.GetType(), "Pop", "openModal();", true);

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
                                var emUso = repo.VerificaUso(tipoErro.TipoErroId);
                                if (emUso)
                                    throw new Exception("Tipo de erro está sendo utilizado em um ou mais testes");
                                repo.Deletar(tipoErro);
                                RecarregarGrid();

                                msgSucesso.Text = "Tipo de Erro cadastrado com sucesso";
                                msgSucesso.DataBind();
                                ClientScript.RegisterStartupScript(this.GetType(), "Pop", "openSuccessMsg();", true);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Ocorreu um erro ao deletar. Erro: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                msgErro.Text = ex.Message;
                msgSucesso.DataBind();
                ClientScript.RegisterStartupScript(this.GetType(), "Pop", "openErrorMsg();", true);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            txtDescricao.Text = "";
            txtTipoErroId.Value = "";
            ddlGravidade.SelectedIndex = 0;
            erroDescricao.Text = "";
            txtDescricao.DataBind();
            txtTipoErroId.DataBind();
            ddlGravidade.DataBind();
            erroDescricao.DataBind();
        }

        protected void btnCriar_Click(object sender, EventArgs e)
        {
            LimparCamposModal();
            ClientScript.RegisterStartupScript(this.GetType(), "Pop", "openModal();", true);
        }
    }
}
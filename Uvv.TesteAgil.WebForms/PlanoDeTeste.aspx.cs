using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Uvv.TesteAgil.Dados.DAL;
using Uvv.TesteAgil.Dados.Repositorio;
using Uvv.TesteAgil.Entidades.Modelos;
using Uvv.TesteAgil.WebForms.Util;

namespace Uvv.TesteAgil.WebForms
{
    public partial class PlanoDeTeste : PaginaBase
    {
        //Repositórios necessários
        PlanoTesteRepositorio repoPlano = new PlanoTesteRepositorio();
        ScriptTesteRepositorio repoScript = new ScriptTesteRepositorio();
        CasoTesteRepositorio repoCaso = new CasoTesteRepositorio();
        CenarioTesteRepositorio repoCenario = new CenarioTesteRepositorio();
        MembroRepositorio repoMembro = new MembroRepositorio();
        ProjetoRepositorio repoProjeto = new ProjetoRepositorio();
        SprintRepositorio repoSprint = new SprintRepositorio();
        EstoriaRepositorio repoEstoria = new EstoriaRepositorio();
        FuncionalidadeRepositorio repoFuncionalidade = new FuncionalidadeRepositorio();
        PlanoTesteDAL planoDal = new PlanoTesteDAL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ValidarSessao();

                var idUsuario = GetUserId();

                CarregarProjetosUsuario(idUsuario);
            }
        }

        protected void dblProjetos_SelectedIndexChanged(object sender, EventArgs e)
        {
            LimparCamposGeralTexto();
            LimparListasFuncionalidades();
            CarregarDropSprints();
        }

        protected void dblSprints_SelectedIndexChanged(object sender, EventArgs e)
        {
            LimparCamposGeralTexto();
            if (!string.IsNullOrEmpty(dblSprints.SelectedValue))
            {
                int idSprint = 0;
                int.TryParse(dblSprints.SelectedValue, out idSprint);
                var plano = repoPlano.ObterPlanoTestePorSprint(idSprint);
                if (plano != null)
                {
                    txtId.Value = plano.PlanoTesteId.ToString();
                    txtDescricao.Text = plano.Descricao;
                    txtDataCriacao.Text = plano.DataCriacao.ToString("yyyy-MM-dd");
                    txtDataFim.Text = plano.DataFim.ToString("yyyy-MM-dd");
                    tabCenarios.Enabled = true;

                    var cenarios = repoCenario.ObterCenariosPorPlano(plano.PlanoTesteId);
                    if (cenarios != null && cenarios.Count() > 0)
                    {
                        tabCasos.Enabled = true;
                        gridCenarios.DataSource = cenarios;
                        gridCenarios.DataBind();
                        //TODO: Carregar grid de cenarios na aba de casos de teste e a grid de casos de teste
                    }
                    else
                        tabCasos.Enabled = false;
                }
                else
                {
                    tabCenarios.Enabled = false;
                    txtDataCriacao.Text = DateTime.Now.ToString("yyyy-MM-dd");
                }                  

                CarregarListaFuncionalidades(idSprint, plano);
            }
            else
            {
                LimparListasFuncionalidades();
                LimparCamposGeral();
            }
        }

        #region Métodos Auxiliares
        private void CarregarProjetosUsuario(int idUsuario)
        {
            var projetos = repoProjeto.ObterProjetosPorMembro(idUsuario);
            dblProjetos.DataSource = projetos;
            dblProjetos.DataTextField = "Nome";
            dblProjetos.DataValueField = "ProjetoId";
            dblProjetos.DataBind();

            if (projetos != null && projetos.Count > 0)
            {
                CarregarDropSprints();
            }
        }

        private void CarregarDropSprints()
        {
            if (!string.IsNullOrEmpty(dblProjetos.SelectedValue))
            {
                var sprints = repoSprint.ObterSprintsPorProjeto(int.Parse(dblProjetos.SelectedValue));
                dblSprints.DataSource = sprints;
                dblSprints.DataTextField = "Numero";
                dblSprints.DataValueField = "SprintId";
                dblSprints.DataBind();

                int idSprint = 0;
                int.TryParse(dblSprints.SelectedValue, out idSprint);
                var plano = repoPlano.ObterPlanoTestePorSprint(idSprint);
                if (plano != null)
                {
                    txtId.Value = plano.PlanoTesteId.ToString();
                    txtDescricao.Text = plano.Descricao;
                    txtDataCriacao.Text = plano.DataCriacao.ToString("yyyy-MM-dd");
                    txtDataFim.Text = plano.DataFim.ToString("yyyy-MM-dd");
                    tabCenarios.Enabled = true;

                    var funcionalidades = repoFuncionalidade.ObterFuncionalidadesNaoUsadasPorPlano(plano.PlanoTesteId);
                    ddlFuncionalidades.DataSource = null;
                    ddlFuncionalidades.DataSource = funcionalidades;
                    ddlFuncionalidades.DataTextField = "Nome";
                    ddlFuncionalidades.DataValueField = "FuncionalidadeId";
                    ddlFuncionalidades.DataBind();

                    var cenarios = repoCenario.ObterCenariosPorPlano(plano.PlanoTesteId);
                    if (cenarios != null && cenarios.Count() > 0)
                    {
                        tabCasos.Enabled = true;
                        gridCenarios.DataSource = cenarios;
                        gridCenarios.DataBind();
                        //TODO: Carregar grid de cenarios na aba de casos de teste e a grid de casos de teste
                    }
                    else
                        tabCasos.Enabled = false;
                }
                else
                    tabCenarios.Enabled = false;

                if (!string.IsNullOrEmpty(dblSprints.SelectedValue))
                {
                    CarregarListaFuncionalidades(idSprint, plano);
                }

            }
            else
            {
                dblSprints.DataSource = null;
                dblSprints.DataBind();
            }
        }

        public void CarregarListaFuncionalidades(int idSprint, PlanoTeste plano = null)
        {
            lbxTestadas.Items.Clear();
            lbxNaoTestadas.Items.Clear();
            if (plano != null)
            {
                var funcionalidades = plano.Funcionalidades;
                if (funcionalidades != null)
                {
                    foreach (var func in funcionalidades)
                    {
                        if (func.Testada)
                        {
                            ListItem item = new ListItem(func.Nome, func.EstoriaId.ToString());
                            lbxTestadas.Items.Add(item);
                        }
                        else
                        {
                            ListItem item = new ListItem(func.Nome, func.EstoriaId.ToString());
                            lbxNaoTestadas.Items.Add(item);
                        }
                    }
                }
            }
            else
            {
                var estorias = repoEstoria.ObterEstoriasPorSprint(idSprint);
                if (estorias != null)
                {
                    foreach (var estoria in estorias)
                    {
                        ListItem item = new ListItem(estoria.Nome, estoria.EstoriaId.ToString());
                        lbxTestadas.Items.Add(item);
                    }
                }
            }
        }

        public void CarregarCenariosTeste(long idPlano)
        {

        }
        #endregion

        #region Geral
        protected void btnAddNaoTestada_Click(object sender, EventArgs e)
        {
            if (lbxTestadas.SelectedItem != null)
            {
                var item = lbxTestadas.SelectedItem;
                lbxNaoTestadas.Items.Add(item);
                lbxTestadas.Items.Remove(item);
                lbxTestadas.ClearSelection();
                lbxNaoTestadas.ClearSelection();
                btnAddNaoTestada.Enabled = false;
                btnAddTestada.Enabled = false;
                lbxNaoTestadas.DataBind();
                lbxTestadas.DataBind();

                LimparCamposErroGeral();
            }
        }

        protected void btnAddTestada_Click(object sender, EventArgs e)
        {
            if (lbxNaoTestadas.SelectedItem != null)
            {
                var item = lbxNaoTestadas.SelectedItem;
                lbxTestadas.Items.Add(item);
                lbxNaoTestadas.Items.Remove(item);
                lbxTestadas.ClearSelection();
                lbxNaoTestadas.ClearSelection();
                btnAddNaoTestada.Enabled = false;
                btnAddTestada.Enabled = false;
                lbxTestadas.DataBind();
                lbxNaoTestadas.DataBind();

                LimparCamposErroGeral();
            }
        }

        protected void lbxNaoTestadas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxNaoTestadas.SelectedItem != null)
                btnAddTestada.Enabled = true;
            else
                btnAddTestada.Enabled = false;
        }

        protected void lbxTestadas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxTestadas.SelectedItem != null)
                btnAddNaoTestada.Enabled = true;
            else
                btnAddNaoTestada.Enabled = false;
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (ValidarCamposGeral())
            {
                try
                {
                    int idSprint = 0;
                    int.TryParse(dblSprints.SelectedValue, out idSprint);
                    var sprint = repoSprint.ObterSprintPorId(idSprint);
                    if (sprint == null)
                        throw new Exception("Sprint não encontrada");
                    #region Cadastrar Plano de Teste
                    if (string.IsNullOrEmpty(txtId.Value))
                    {
                        var existe = repoPlano.ExistePlanoTeste(txtDescricao.Text);
                        if (existe) throw new Exception("Já existe um plano de teste com esta descrição");

                        List<Funcionalidade> funcionalidades = new List<Funcionalidade>();

                        PlanoTeste plano = new PlanoTeste();
                        plano.Descricao = txtDescricao.Text;
                        plano.DataCriacao = DateTime.Parse(txtDataCriacao.Text);
                        plano.DataFim = DateTime.Parse(txtDataFim.Text);
                        plano.SprintId = sprint.SprintId;
                        plano.Funcionalidades = new List<Funcionalidade>();
                        foreach (ListItem item in lbxTestadas.Items)
                        {
                            var estoria = repoEstoria.Find(long.Parse(item.Value));
                            if (estoria == null)
                                throw new Exception("Estória não encontrada");
                            var funcionalidade = new Funcionalidade();
                            funcionalidade.Descricao = estoria.Descricao;
                            funcionalidade.Nome = estoria.Nome;
                            funcionalidade.Pontos = estoria.Pontos;
                            funcionalidade.Prioridade = estoria.Prioridade;
                            funcionalidade.Testada = true;
                            funcionalidade.EstoriaId = estoria.EstoriaId;
                            //funcionalidade.Planos = new List<PlanoTeste>();
                            //funcionalidade.Planos.Add(plano);
                            funcionalidades.Add(funcionalidade);
                        }
                        if (lbxNaoTestadas.Items != null)
                        {
                            foreach (ListItem item in lbxNaoTestadas.Items)
                            {
                                var estoria = repoEstoria.Find(long.Parse(item.Value));
                                if (estoria == null)
                                    throw new Exception("Estória não encontrada");
                                var funcionalidade = new Funcionalidade();
                                funcionalidade.Descricao = estoria.Descricao;
                                funcionalidade.Nome = estoria.Nome;
                                funcionalidade.Pontos = estoria.Pontos;
                                funcionalidade.Prioridade = estoria.Prioridade;
                                funcionalidade.Testada = false;
                                funcionalidade.EstoriaId = estoria.EstoriaId;
                                //funcionalidade.Planos = new List<PlanoTeste>();
                                //funcionalidade.Planos.Add(plano);
                                funcionalidades.Add(funcionalidade);
                            }
                        }
                        planoDal.AdicionaPlano(plano, funcionalidades);
                        //repoPlano.Commit();
                        txtId.Value = plano.PlanoTesteId.ToString();

                        msgSucesso.Text = "Plano de teste cadastrado com sucesso";
                        msgSucesso.DataBind();
                        ScriptManager.RegisterStartupScript(updPlano, updPlano.GetType(), "Pop", "openSuccessMsg();", true);
                    }
                    #endregion

                    #region Editar Plano de Teste
                    else
                    {
                        var plano = repoPlano.Find(long.Parse(txtId.Value));
                        if (plano == null)
                            throw new Exception("Plano de Teste não encontrado");
                        if (plano.Descricao.ToUpper() != txtDescricao.Text.ToUpper())
                        {
                            var existe = repoPlano.ExistePlanoTeste(txtDescricao.Text);
                            if (existe) throw new Exception("Já existe um plano de teste com esta descrição");
                        }

                        plano.Descricao = txtDescricao.Text;
                        plano.DataCriacao = DateTime.Parse(txtDataCriacao.Text);
                        plano.DataFim = DateTime.Parse(txtDataFim.Text);
                        repoPlano.Atualizar(plano);
                        var funcionalidades = plano.Funcionalidades?.ToList();
                        if (funcionalidades != null)
                        {
                            foreach (var funcionalidade in funcionalidades)
                            {
                                repoFuncionalidade.Deletar(funcionalidade);
                            }
                        }
                        foreach (ListItem item in lbxTestadas.Items)
                        {
                            var estoria = repoEstoria.Find(long.Parse(item.Value));
                            if (estoria == null)
                                throw new Exception("Estória não encontrada");
                            var funcionalidade = new Funcionalidade();
                            funcionalidade.Descricao = estoria.Descricao;
                            funcionalidade.Nome = estoria.Nome;
                            funcionalidade.Pontos = estoria.Pontos;
                            funcionalidade.Prioridade = estoria.Prioridade;
                            funcionalidade.Testada = true;
                            repoFuncionalidade.Adicionar(funcionalidade);
                        }
                        if (lbxNaoTestadas.Items != null)
                        {
                            foreach (ListItem item in lbxNaoTestadas.Items)
                            {
                                var estoria = repoEstoria.Find(long.Parse(item.Value));
                                if (estoria == null)
                                    throw new Exception("Estória não encontrada");
                                var funcionalidade = new Funcionalidade();
                                funcionalidade.Descricao = estoria.Descricao;
                                funcionalidade.Nome = estoria.Nome;
                                funcionalidade.Pontos = estoria.Pontos;
                                funcionalidade.Prioridade = estoria.Prioridade;
                                funcionalidade.Testada = false;
                                repoFuncionalidade.Adicionar(funcionalidade);
                            }
                        }

                        repoFuncionalidade.Commit();
                        repoPlano.Commit();

                        txtId.Value = plano.PlanoTesteId.ToString();
                        txtId.DataBind();

                        msgSucesso.Text = "Plano de teste editado com sucesso";
                        ScriptManager.RegisterStartupScript(updPlano, updPlano.GetType(), "Pop", "openSuccessMsg();", true);
                        msgSucesso.DataBind();
                    }
                    #endregion

                    LimparCamposErroGeral();

                    tabCenarios.Enabled = true;
                }
                catch (Exception ex)
                {
                    msgErro.Text = "Erro: " + ex.Message;
                    msgErro.DataBind();
                    ScriptManager.RegisterStartupScript(updPlano, updPlano.GetType(), "Pop", "openErrorMsg();", true);
                }
            }
        }

        private bool ValidarCamposGeral()
        {
            bool valido = true;
            if (string.IsNullOrWhiteSpace(txtDescricao.Text))
            {
                erroDescricao.Text = "Campo Obrigatório";
                valido = false;
            }
            if (string.IsNullOrWhiteSpace(txtDataCriacao.Text))
            {
                erroDataCriacao.Text = "Campo Obrigatório";
                valido = false;
            }
            else
            {
                DateTime dataCriacao = DateTime.MinValue;
                DateTime.TryParse(txtDataCriacao.Text, out dataCriacao);
                if (dataCriacao == DateTime.MinValue)
                {
                    erroDataCriacao.Text = "Data inválida";
                    valido = false;
                }
            }
            if (string.IsNullOrWhiteSpace(txtDataFim.Text))
            {
                erroDataFim.Text = "Campo Obrigatório";
                valido = false;
            }
            else
            {
                DateTime dataFim = DateTime.MinValue;
                DateTime.TryParse(txtDataFim.Text, out dataFim);
                if (dataFim == DateTime.MinValue)
                {
                    erroDataFim.Text = "Data inválida";
                    valido = false;
                }
            }
            if (lbxTestadas.Items == null || lbxTestadas.Items.Count == 0)
            {
                erroFuncionalidadesTestadas.Text = "Adicione pelo menos uma funcionalidade";
                erroFuncionalidadesTestadas.DataBind();
                valido = false;
            }

            return valido;
        }

        public void LimparCamposGeral()
        {
            LimparCamposErroGeral();

            LimparCamposGeralTexto();

            if (dblProjetos.Items != null && dblProjetos.Items.Count > 0)
                dblProjetos.SelectedIndex = 0;

            if (dblSprints.Items != null && dblSprints.Items.Count > 0)
                dblSprints.SelectedIndex = 0;
        }

        public void LimparCamposGeralTexto()
        {
            txtId.Value = string.Empty;
            txtDescricao.Text = string.Empty;
            txtDataCriacao.Text = string.Empty;
            txtDataFim.Text = string.Empty;

            txtDescricao.DataBind();
            txtDataCriacao.DataBind();
            txtDataFim.DataBind();
        }

        public void LimparCamposErroGeral()
        {
            erroDescricao.Text = string.Empty;
            erroDataCriacao.Text = string.Empty;
            erroDataFim.Text = string.Empty;
            erroFuncionalidadesTestadas.Text = string.Empty;
        }

        public void LimparListasFuncionalidades()
        {
            lbxTestadas.DataSource = null;
            lbxTestadas.Items.Clear();
            lbxTestadas.DataBind();

            lbxNaoTestadas.DataSource = null;
            lbxNaoTestadas.Items.Clear();
            lbxNaoTestadas.DataBind();
        }

        #endregion

        protected void btnSalvarCenario_Click(object sender, EventArgs e)
        {
            if (ValidarCamposCenario())
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(txtId.Value))
                        throw new Exception("Não foi possível encontrar o plano de teste. Favor atualizar a página.");

                    int idPlano = 0;
                    int.TryParse(txtId.Value, out idPlano);

                    var plano = repoPlano.ObterPlanoTestePorId(idPlano);
                    int idFuncionalidade = 0;
                    int.TryParse(ddlFuncionalidades.SelectedValue, out idFuncionalidade);
                    var funcionalidade = repoFuncionalidade.ObterFuncionalidadePorId(idFuncionalidade);
                    CenarioTeste cenario = new CenarioTeste();
                    cenario.PlanoTeste = plano ?? throw new Exception("Plano de Teste não encontrado");
                    cenario.Funcionalidade = funcionalidade ?? throw new Exception("Funcionalidade não encontrada");
                    cenario.Situacao = int.Parse(ddlSituacao.SelectedItem.Value);
                    cenario.DataAtualizacao = DateTime.Now;
                    repoCenario.Adicionar(cenario);
                    repoCenario.Commit();

                    msgSucesso.Text = "Cenário de teste salvo com sucesso";
                    msgSucesso.DataBind();
                    ScriptManager.RegisterStartupScript(updPlano, updPlano.GetType(), "Pop", "openSuccessMsg();", true);
                }
                catch (Exception ex)
                {
                    msgErro.Text = ex.Message;
                    ScriptManager.RegisterStartupScript(updPlano, updPlano.GetType(), "Pop", "openErrorMsg();", true);
                    msgErro.DataBind();
                }
            }
        }

        protected void gridCenarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        private bool ValidarCamposCenario()
        {
            bool valido = true;
            if (ddlFuncionalidades.SelectedItem == null)
            {
                erroFuncionalidade.Text = "Campo obrigatório";
                valido = false;
            }
            if (ddlSituacao.SelectedItem == null)
            {
                erroSituacao.Text = "Campo obrigatório";
                valido = false;
            }
            return valido;
        }

        protected void txtId_ValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtId.Value))
            {
                btnSalvarCenario.Enabled = false;
                tabCenarios.Enabled = false;
            }
            else
            {
                btnSalvarCenario.Enabled = true;
                tabCenarios.Enabled = true;
            }
        }

        protected void tabGeral_Click(object sender, EventArgs e)
        {
            OpcaoClasseTab(0);
        }

        protected void tabCenarios_Click(object sender, EventArgs e)
        {
            OpcaoClasseTab(1);
        }

        protected void tabCasos_Click(object sender, EventArgs e)
        {
            OpcaoClasseTab(2);
        }

        protected void tabTestes_Click(object sender, EventArgs e)
        {
            OpcaoClasseTab(3);
        }

        private void OpcaoClasseTab(int indexClicked)
        {
            switch (indexClicked)
            {
                case 0:
                    tabGeral.CssClass = "tabmenu-clicked";
                    tabCenarios.CssClass = "tabmenu-inicial";
                    tabCasos.CssClass = "tabmenu-inicial";
                    tabTestes.CssClass = "tabmenu-inicial";
                    mvPlano.ActiveViewIndex = 0;
                    break;
                case 1:
                    tabGeral.CssClass = "tabmenu-inicial";
                    tabCenarios.CssClass = "tabmenu-clicked";
                    tabCasos.CssClass = "tabmenu-inicial";
                    tabTestes.CssClass = "tabmenu-inicial";
                    mvPlano.ActiveViewIndex = 1;
                    break;
                case 2:
                    tabGeral.CssClass = "tabmenu-inicial";
                    tabCenarios.CssClass = "tabmenu-inicial";
                    tabCasos.CssClass = "tabmenu-clicked";
                    tabTestes.CssClass = "tabmenu-inicial";
                    mvPlano.ActiveViewIndex = 2;
                    break;
                case 3:
                    tabGeral.CssClass = "tabmenu-inicial";
                    tabCenarios.CssClass = "tabmenu-inicial";
                    tabCasos.CssClass = "tabmenu-inicial";
                    tabTestes.CssClass = "tabmenu-clicked";
                    mvPlano.ActiveViewIndex = 3;
                    break;
            }
        }

    }
}
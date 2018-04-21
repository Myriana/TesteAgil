using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Uvv.TesteAgil.Dados.DAL;
using Uvv.TesteAgil.Dados.Repositorio;
using Uvv.TesteAgil.Entidades.Modelos;

namespace Uvv.TesteAgil.WebForms
{
    public partial class Plano : PaginaBase
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

        #region DropDowns
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

                CarregarPlanoDeTeste(idSprint);
            }
            else
            {
                LimparListasFuncionalidades();
                LimparCamposGeral();
            }
        }
        #endregion

        #region Listbox
        protected void lbxTestadas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxTestadas.SelectedItem != null)
                btnAddNaoTestada.Enabled = true;
            else
                btnAddNaoTestada.Enabled = false;
        }

        protected void lbxNaoTestadas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxNaoTestadas.SelectedItem != null)
                btnAddTestada.Enabled = true;
            else
                btnAddTestada.Enabled = false;
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
        #endregion

        #region Grids
        protected void gridCenarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        private void RecarregarGridCenarios()
        {
            int idPlano = 0;
            int.TryParse(txtId.Value, out idPlano);
            var cenarios = repoCenario.ObterCenariosPorPlano(idPlano);
            if (cenarios != null && cenarios.Count() > 0)
            {
                gridCenarios.DataSource = cenarios;
                gridCenarios.DataBind();
            }
        }
        #endregion

        #region Buttons
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
                        ScriptManager.RegisterStartupScript(pnlPlano, pnlPlano.GetType(), "Pop", "openSuccessMsg();", true);
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
                            foreach (ListItem item in lbxTestadas.Items)
                            {
                                int idEstoria = 0;
                                int.TryParse(item.Value, out idEstoria);
                                var funcionalidade = funcionalidades.FirstOrDefault(f => f.EstoriaId == idEstoria);
                                if(funcionalidade != null)
                                {
                                    funcionalidade.Testada = true;
                                    repoFuncionalidade.Atualizar(funcionalidade);
                                }                                  
                            }

                            if (lbxNaoTestadas.Items != null)
                            {
                                foreach (ListItem item in lbxNaoTestadas.Items)
                                {
                                    int idEstoria = 0;
                                    int.TryParse(item.Value, out idEstoria);
                                    var funcionalidade = funcionalidades.FirstOrDefault(f => f.EstoriaId == idEstoria);
                                    if(funcionalidade != null)
                                    {
                                        funcionalidade.Testada = false;
                                        repoFuncionalidade.Adicionar(funcionalidade);
                                    }                                    
                                }
                            }
                        }
                                          
                        repoFuncionalidade.Commit();
                        repoPlano.Commit();

                        txtId.Value = plano.PlanoTesteId.ToString();
                        txtId.DataBind();

                        msgSucesso.Text = "Plano de teste editado com sucesso";
                        ScriptManager.RegisterStartupScript(pnlPlano, pnlPlano.GetType(), "Pop", "openSuccessMsg();", true);
                        msgSucesso.DataBind();
                    }
                    #endregion

                    LimparCamposErroGeral();
                }
                catch (Exception ex)
                {
                    msgErro.Text = "Erro: " + ex.Message;
                    msgErro.DataBind();
                    ScriptManager.RegisterStartupScript(pnlPlano, pnlPlano.GetType(), "Pop", "openErrorMsg();", true);
                }
            }
        }

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
                    //repoCenario.Adicionar(cenario);
                    //repoCenario.Commit();

                    msgSucesso.Text = "Cenário de teste salvo com sucesso";
                    msgSucesso.DataBind();
                    ScriptManager.RegisterStartupScript(pnlCenario, pnlCenario.GetType(), "Pop", "openSuccessMsg();", true);
                }
                catch (Exception ex)
                {
                    msgErro.Text = ex.Message;
                    ScriptManager.RegisterStartupScript(pnlCenario, pnlCenario.GetType(), "Pop", "openErrorMsg();", true);
                    msgErro.DataBind();
                }
            }
        }

        protected void btnCenario_Click(object sender, EventArgs e)
        {
            VisibilidadeDivs(2);
            lbPage.Text = "Cenário de Teste";
        }

        protected void btnVoltarPlano_Click(object sender, EventArgs e)
        {
            VisibilidadeDivs(1);
            lbPage.Text = "Plano de Teste";
        }

        protected void btnCaso_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region Métodos Auxiliares

        #region Carregamento de Campos
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
                //Carregar o DropDown de Sprint com as Sprints do projeto selecionadoS
                var sprints = repoSprint.ObterSprintsPorProjeto(int.Parse(dblProjetos.SelectedValue));
                dblSprints.DataSource = sprints;
                dblSprints.DataTextField = "Numero";
                dblSprints.DataValueField = "SprintId";
                dblSprints.DataBind();

                //Obter o ID da Sprint Selecionada
                int idSprint = 0;
                int.TryParse(dblSprints.SelectedValue, out idSprint);

                //Carregar o Plano de Teste, caso já exista
                CarregarPlanoDeTeste(idSprint);
            }
            else
            {
                dblSprints.DataSource = null;
                dblSprints.DataBind();
            }
        }

        public void CarregarPlanoDeTeste(int idSprint)
        {
            
            var plano = repoPlano.ObterPlanoTestePorSprint(idSprint);
            if (plano != null)
            {
                //Carrega os dados do plano de teste já existente
                txtId.Value = plano.PlanoTesteId.ToString();
                txtDescricao.Text = plano.Descricao;
                txtDataCriacao.Text = plano.DataCriacao.ToString("yyyy-MM-dd");
                txtDataFim.Text = plano.DataFim.ToString("yyyy-MM-dd");

                //Habilita Botão para abrir cenário de teste do plano de teste já existente
                btnCenario.Enabled = true;

                var funcionalidades = repoFuncionalidade.ObterFuncionalidadesPorPlano(plano.PlanoTesteId);
                //Carrega o dropdown de funcionalidades do plano de teste, caso tenha
                CarregarDropFuncionalidades(funcionalidades);

                //Carrega a grid de Cenários
                RecarregarGridCenarios();
            }
            else
            {
                txtDataCriacao.Text = DateTime.Now.ToString("yyyy-MM-dd");
                btnCenario.Visible = false;
                txtId.Value = "";
                txtId.DataBind();
                btnCenario.DataBind();
            }
            if (!string.IsNullOrEmpty(dblSprints.SelectedValue))
            {
                CarregarListaFuncionalidades(idSprint, plano);
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

        public void CarregarDropFuncionalidades(List<Funcionalidade> funcionalidades)
        {
            ddlFuncionalidades.DataSource = null;
            ddlFuncionalidades.DataSource = funcionalidades;
            ddlFuncionalidades.DataTextField = "Nome";
            ddlFuncionalidades.DataValueField = "FuncionalidadeId";
            ddlFuncionalidades.DataBind();

            var funcionalidadeSelecionada = funcionalidades.FirstOrDefault();
            if(funcionalidadeSelecionada != null)
            {
                //Verifica se já existe Cenário para esta funcionalidade e seta o ID
                var cenario = repoCenario.ObterCenarioPorFuncionalidade(funcionalidadeSelecionada.FuncionalidadeId);
                if (cenario != null)
                {
                    txtIdCenario.Value = cenario.CenarioTesteId.ToString();
                    ddlSituacao.SelectedValue = cenario.Situacao.ToString();
                }
                else
                {
                    ddlSituacao.SelectedValue = "1";
                }
            }
        }

        public void CarregarCamposCenario(int idPlano)
        {
            var cenarios = repoCenario.ObterCenariosPorPlano(idPlano);
            if (cenarios != null && cenarios.Count() > 0)
            {
                gridCenarios.DataSource = cenarios;
                gridCenarios.DataBind();
            }           
        }
        #endregion

        #region Limpeza de Campos
        public void LimparListasFuncionalidades()
        {
            lbxTestadas.DataSource = null;
            lbxTestadas.Items.Clear();
            lbxTestadas.DataBind();

            lbxNaoTestadas.DataSource = null;
            lbxNaoTestadas.Items.Clear();
            lbxNaoTestadas.DataBind();
        }

        public void LimparCamposErroGeral()
        {
            erroDescricao.Text = string.Empty;
            erroDataCriacao.Text = string.Empty;
            erroDataFim.Text = string.Empty;
            erroFuncionalidadesTestadas.Text = string.Empty;
        }

        public void LimparCamposGeral()
        {
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

            txtId.DataBind();
            txtDescricao.DataBind();
            txtDataCriacao.DataBind();
            txtDataFim.DataBind();

            LimparCamposErroGeral();
        }

        public void LimparCamposCenario()
        {
            if (ddlFuncionalidades.Items != null && ddlFuncionalidades.Items.Count > 0)
                ddlFuncionalidades.SelectedIndex = 0;
            ddlSituacao.SelectedIndex = 0;

            txtIdCenario.Value = "";

            txtIdCenario.DataBind();
        }
        #endregion

        #region Validação
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
        #endregion

        #region Visibilidade
        public void VisibilidadeDivs(int numDiv)
        {
            switch(numDiv)
            {
                case 1:
                    divPlano.Visible = true;
                    divCenario.Visible = false;
                    break;
                case 2:
                    divPlano.Visible = false;
                    divCenario.Visible = true;
                    break;
            }
        }
        #endregion

        #region Nome de Campos
        public string ObterNomeSituacaoCenario(int numSituacao)
        {
            switch(numSituacao)
            {
                case 1: return "A fazer";
                case 2: return "Desenvolvimento";
                case 3: return "Teste";
                case 4: return "Concluído";
            }
            return "";
        }

        public string ObterNomePrioridadeFuncionalidade(int numPrioridade)
        {
            switch (numPrioridade)
            {
                case 1: return "Alta";
                case 2: return "Média";
                case 3: return "Baixa";
            }
            return "";
        }
        #endregion

        #endregion

        
    }
}
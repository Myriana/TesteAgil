using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
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
        CasoTesteRepositorio repoCaso = new CasoTesteRepositorio();
        TesteRepositorio repoTeste = new TesteRepositorio();
        CenarioTesteRepositorio repoCenario = new CenarioTesteRepositorio();
        MembroRepositorio repoMembro = new MembroRepositorio();
        ProjetoRepositorio repoProjeto = new ProjetoRepositorio();
        SprintRepositorio repoSprint = new SprintRepositorio();
        EstoriaRepositorio repoEstoria = new EstoriaRepositorio();
        TipoErroRepositorio repoTipoErro = new TipoErroRepositorio();
        FuncionalidadeRepositorio repoFuncionalidade = new FuncionalidadeRepositorio();
        PlanoTesteDAL planoDal = new PlanoTesteDAL();
        PassoRepositorio repoPasso = new PassoRepositorio();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ValidarSessao();

                var idUsuario = GetUserId();

                CarregarProjetosUsuario(idUsuario);
            }
        }

        #region Eventos Geral
        protected void dblProjetos_SelectedIndexChanged(object sender, EventArgs e)
        {
            LimparCamposGeralTexto();
            LimparListasFuncionalidades();
            CarregarDropSprints();
            LimparMsgErro();
        }

        protected void dblSprints_SelectedIndexChanged(object sender, EventArgs e)
        {
            LimparMsgErro();
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
                    btnCenarios.Visible = true;

                    var cenarios = repoCenario.ObterCenariosPorPlano(plano.PlanoTesteId);
                    if (cenarios != null && cenarios.Count() > 0)
                    {
                        CarregarCenariosGrid(plano.PlanoTesteId);
                    }
                }
                else
                {
                    btnCenarios.Visible = false;
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

        protected void btnAddNaoTestada_Click(object sender, EventArgs e)
        {
            LimparMsgErro();
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
            LimparMsgErro();
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
            LimparMsgErro();
            if (lbxNaoTestadas.SelectedItem != null)
                btnAddTestada.Enabled = true;
            else
                btnAddTestada.Enabled = false;
        }

        protected void lbxTestadas_SelectedIndexChanged(object sender, EventArgs e)
        {
            LimparMsgErro();
            if (lbxTestadas.SelectedItem != null)
                btnAddNaoTestada.Enabled = true;
            else
                btnAddNaoTestada.Enabled = false;
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            LimparMsgErro();
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

                        foreach (ListItem item in lbxTestadas.Items)
                        {
                            //Se não tiver funcionalidades adiciona a lista atual
                            if (funcionalidades == null)
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
                            else
                            {
                                //Verifica se a funcionalidade do item existe na lista
                                var func = funcionalidades.FirstOrDefault(f => f.EstoriaId == long.Parse(item.Value));
                                //Se a funcionalidade existe, atualiza a flag Testada para true caso esteja false
                                if (func != null)
                                {
                                    if (func.Testada == false)
                                    {
                                        func.Testada = true;
                                        //repoFuncionalidade.Atualizar(func);
                                    }
                                }
                                //Caso não exista essa funcionalidade na lista, adicionar nova funcionalidade
                                else
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
                            }
                        }
                        if (lbxNaoTestadas.Items != null)
                        {
                            foreach (ListItem item in lbxNaoTestadas.Items)
                            {
                                //Se não tiver funcionalidades adiciona a lista atual
                                if (funcionalidades == null)
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
                                else
                                {
                                    //Verifica se a funcionalidade do item existe na lista
                                    var func = funcionalidades.FirstOrDefault(f => f.EstoriaId == long.Parse(item.Value));
                                    //Se a funcionalidade existe, atualiza a flag Testada para false caso esteja true
                                    if (func != null)
                                    {
                                        if (func.Testada == true)
                                        {
                                            func.Testada = false;
                                            //repoFuncionalidade.Atualizar(func);
                                        }
                                    }
                                    //Caso não exista essa funcionalidade na lista, adicionar nova funcionalidade
                                    else
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

                    btnCenarios.Visible = true;
                }
                catch (Exception ex)
                {
                    msgErro.Text = "Erro: " + ex.Message;
                    msgErro.DataBind();
                    ScriptManager.RegisterStartupScript(updPlano, updPlano.GetType(), "Pop", "openErrorMsg();", true);
                }
            }
        }

        protected void txtId_ValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtId.Value))
            {
                btnSalvarCenario.Enabled = false;
                btnCenarios.Visible = false;
            }
            else
            {
                btnSalvarCenario.Enabled = true;
                btnCenarios.Visible = true;
            }
        }

        protected void btnCenarios_Click(object sender, EventArgs e)
        {
            LimparMsgErro();
            int idPlano = int.Parse(txtId.Value);
            CarregarCenariosGrid(idPlano);
            this.CarregarFuncionalidadesDropDown(idPlano);
            OpcaoDiv("cenario");
        }

        #endregion

        #region Eventos Cenário
        protected void btnSalvarCenario_Click(object sender, EventArgs e)
        {
            LimparMsgErro();
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
                    #region Novo Cenário
                    if (string.IsNullOrEmpty(txtIdCenario.Value))
                    {
                        cenario.PlanoTesteId = plano == null ? throw new Exception("Plano de Teste não encontrado") : plano.PlanoTesteId;
                        cenario.FuncionalidadeId = funcionalidade == null ? throw new Exception("Funcionalidade não encontrada") : funcionalidade.FuncionalidadeId;
                        cenario.Situacao = int.Parse(ddlSituacao.SelectedItem.Value);
                        cenario.DataAtualizacao = DateTime.Now;
                        repoCenario.Adicionar(cenario);
                    }
                    #endregion
                    #region Editar Cenário
                    else
                    {
                        var idCenario = int.Parse(txtIdCenario.Value);
                        cenario = repoCenario.ObterCenarioPorId(idCenario);
                        if (cenario == null) throw new Exception("Não foi possível encontrar o cenário de teste. Favor atualizar a página.");
                        cenario.FuncionalidadeId = funcionalidade == null ? throw new Exception("Funcionalidade não encontrada") : funcionalidade.FuncionalidadeId;
                        cenario.Situacao = int.Parse(ddlSituacao.SelectedItem.Value);
                        cenario.DataAtualizacao = DateTime.Now;
                        repoCenario.Atualizar(cenario);
                    }
                    #endregion

                    repoCenario.Commit();

                    txtIdCenario.Value = cenario.CenarioTesteId.ToString();

                    this.CarregarCenariosGrid(idPlano);

                    this.CarregarFuncionalidadesDropDown(idPlano);

                    btnCancelarEditarCenario.Visible = true;
                    ddlFuncionalidades.Enabled = true;

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
            try
            {
                LimparMsgErro();
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gridCenarios.Rows[index];
                if (row.Cells.Count >= 3)
                {
                    int id = 0;
                    int.TryParse(row.Cells[0].Text, out id);
                    if (id == 0)
                        throw new Exception("Id não pode ser zero");
                    var cenario = repoCenario.ObterCenarioPorId(id);
                    if (cenario == null)
                        throw new Exception("Cenário de Teste não encontrado");

                    if (e.CommandName == "Editar")
                    {
                        try
                        {
                            txtIdCenario.Value = cenario.CenarioTesteId.ToString();
                            var funcionalidades = new List<Funcionalidade>();
                            funcionalidades.Add(cenario.Funcionalidade);
                            ddlFuncionalidades.DataSource = null;
                            ddlFuncionalidades.DataSource = funcionalidades;
                            ddlFuncionalidades.DataTextField = "Nome";
                            ddlFuncionalidades.DataValueField = "FuncionalidadeId";
                            ddlFuncionalidades.DataBind();
                            ddlSituacao.SelectedValue = cenario.Situacao.ToString();

                            btnCancelarEditarCenario.Visible = true;
                            ddlFuncionalidades.Enabled = false;
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
                                var casos = repoCaso.ObterCasosPorCenario(cenario.CenarioTesteId);
                                if (casos != null && casos.Count > 0)
                                    throw new Exception("Cenário de Teste não pode ser apagado pois existem Casos de Teste associados");
                                repoCenario.Deletar(cenario);
                                repoCenario.Commit();
                                CarregarFuncionalidadesDropDown(cenario.PlanoTesteId);
                                CarregarCenariosGrid(cenario.PlanoTesteId);

                                msgSucesso.Text = "Cenário de Teste excluído com sucesso";
                                msgSucesso.DataBind();
                                ScriptManager.RegisterStartupScript(updPlano, updPlano.GetType(), "Pop", "openSuccessMsg();", true);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Ocorreu um erro ao deletar. Erro: " + ex.Message);
                        }
                    }
                    else if (e.CommandName == "Caso")
                    {
                        CarregarGridCasos(cenario.CenarioTesteId);
                        txtIdCenario.Value = cenario.CenarioTesteId.ToString();
                        txtCenarioDescricao.Text = cenario.Funcionalidade.Nome;
                        OpcaoDiv("caso");
                    }
                }
            }
            catch (Exception ex)
            {
                msgErro.Text = ex.Message;
                msgSucesso.DataBind();
                ScriptManager.RegisterStartupScript(updPlano, updPlano.GetType(), "Pop", "openErrorMsg();", true);
            }
        }

        protected void btnVoltarGeral_Click(object sender, EventArgs e)
        {
            LimparMsgErro();
            OpcaoDiv("geral");
            LimparCamposCenario();
        }

        protected void btnCancelarEditarCenario_Click(object sender, EventArgs e)
        {
            LimparMsgErro();
            LimparCamposCenario();
        }
        #endregion

        #region Eventos Caso
        protected void btnSalvarCaso_Click(object sender, EventArgs e)
        {
            LimparMsgErro();
            if (ValidarCamposCaso())
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(txtIdCenario.Value))
                        throw new Exception("Não foi possível encontrar o cenário de teste. Favor atualizar a página.");

                    int idCenario = 0;
                    int.TryParse(txtIdCenario.Value, out idCenario);

                    var cenario = repoCenario.ObterCenarioPorId(idCenario);
                    var caso = new CasoTeste();
                    #region Novo Caso
                    if (string.IsNullOrEmpty(txtIdCaso.Value))
                    {
                        caso.CenarioTesteId = cenario == null ? throw new Exception("Cenário de Teste não encontrado") : cenario.CenarioTesteId;
                        caso.Descricao = txtDescricaoCaso.Text;
                        caso.Categoria = txtCategoria.Text;
                        caso.Entrada = txtEntrada.Text;
                        caso.RespostaEsperada = txtRespostaEsperada.Text;
                        repoCaso.Adicionar(caso);
                        repoCaso.Commit();
                        //Adicionar Passos se houver
                        if (gridPassos.Rows != null && gridPassos.Rows.Count > 0)
                        {
                            foreach (GridViewRow row in gridPassos.Rows)
                            {
                                Passo p = new Passo();
                                p.CasoTesteId = caso.CasoTesteId;
                                //p.CasoTeste = caso;
                                p.Numero = int.Parse(row.Cells[1].Text);
                                p.Descricao = Context.Server.HtmlDecode(row.Cells[2].Text);
                                repoPasso.Adicionar(p);
                            }
                            repoPasso.Commit();
                        }
                    }
                    #endregion
                    #region Editar Caso
                    else
                    {
                        var idCaso = int.Parse(txtIdCaso.Value);
                        caso = repoCaso.ObterCasoPorId(idCaso);
                        if (caso == null) throw new Exception("Não foi possível encontrar o caso de teste. Favor atualizar a página.");
                        caso.Descricao = txtDescricaoCaso.Text;
                        caso.Categoria = txtCategoria.Text;
                        caso.Entrada = txtEntrada.Text;
                        caso.RespostaEsperada = txtRespostaEsperada.Text;
                        repoCaso.Atualizar(caso);
                        repoCaso.Commit();

                        //Verificar Passos
                        List<Passo> passosAdd = new List<Passo>();
                        List<Passo> passosDel = new List<Passo>();
                        List<Passo> passosAtual = new List<Passo>();
                        if (gridPassos.Rows != null && gridPassos.Rows.Count > 0)
                        {
                            //Obter tipos de erro na lista atual
                            foreach (GridViewRow row in gridPassos.Rows)
                            {
                                int idPasso = int.Parse(row.Cells[0].Text);
                                Passo p = new Passo();
                                p.CasoTesteId = caso.CasoTesteId;
                                p.PassoId = idPasso;
                                p.Numero = int.Parse(row.Cells[1].Text);
                                p.Descricao = Context.Server.HtmlDecode(row.Cells[2].Text);
                                if (idPasso == 0)
                                {
                                    passosAdd.Add(p);
                                }
                                passosAtual.Add(p);
                            }
                            //Obter passos na lista
                            var passosAnteriores = repoPasso.ObterPassosPorCasos(caso.CasoTesteId);

                            //Se não tiver passos antes, deve-se considerar apenas os atuais
                            //Caso tenha passos anteriores, deve-se verificar se foi deletado algum passo
                            if (passosAnteriores != null)
                            {
                                if (passosAnteriores != null)
                                {
                                    foreach (var p in passosAnteriores)
                                    {
                                        //Verifica se o passo na lista anteror existe na atual
                                        var existe = passosAtual.FirstOrDefault(x => x.PassoId == p.PassoId);
                                        //Se não existe, deve-se deletar o tipo de erro
                                        if (existe == null)
                                            passosDel.Add(p);
                                    }
                                }
                            }

                            foreach (var p in passosAdd)
                            {
                                repoPasso.Adicionar(p);
                            }

                            foreach (var p in passosDel)
                            {
                                repoPasso.Deletar(p);
                            }

                            repoPasso.Commit();
                        }
                        else
                        {
                            var passos = repoPasso.ObterPassosPorCasos(caso.CasoTesteId);
                            if(passos != null)
                            {
                                foreach(var passo in passos)
                                {
                                    repoPasso.Deletar(passo);
                                }
                                repoPasso.Commit();
                            }
                        }
                    }
                    #endregion

                    //repoCaso.Commit();

                    txtIdCenario.Value = cenario.CenarioTesteId.ToString();
                    txtIdCaso.Value = caso.CasoTesteId.ToString();

                    this.CarregarGridCasos(idCenario);

                    btnCancelarEditarCaso.Visible = true;

                    msgSucesso.Text = "Caso de teste salvo com sucesso";
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

        protected void btnCancelarEditarCaso_Click(object sender, EventArgs e)
        {
            LimparMsgErro();
            LimparCamposCaso();
        }

        protected void btnVoltarCenario_Click(object sender, EventArgs e)
        {
            LimparMsgErro();
            LimparCamposCaso();
            OpcaoDiv("cenario");
        }

        protected void gridCasos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                LimparMsgErro();
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gridCasos.Rows[index];
                if (row.Cells.Count >= 3)
                {
                    int id = 0;
                    int.TryParse(row.Cells[0].Text, out id);
                    if (id == 0)
                        throw new Exception("Id não pode ser zero");
                    var caso = repoCaso.ObterCasoPorId(id);
                    if (caso == null)
                        throw new Exception("Caso de Teste não encontrado");

                    if (e.CommandName == "Editar")
                    {
                        try
                        {
                            txtIdCaso.Value = caso.CasoTesteId.ToString();
                            txtDescricaoCaso.Text = caso.Descricao;
                            txtCategoria.Text = caso.Categoria;
                            txtEntrada.Text = caso.Entrada;
                            txtRespostaEsperada.Text = caso.RespostaEsperada;

                            btnCancelarEditarCaso.Visible = true;

                            //Passos
                            var passos = caso.Passos;
                            if(passos != null)
                            {
                                gridPassos.DataSource = null;
                                gridPassos.DataSource = passos;
                                gridPassos.DataBind();
                            }
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
                                var testes = repoTeste.ObterTestesPorCaso(caso.CasoTesteId);
                                if (testes != null && testes.Count > 0)
                                    throw new Exception("Caso de Teste não pode ser apagado pois existem Testes associados");
                                repoCaso.Deletar(caso);
                                repoCaso.Commit();
                                CarregarGridCasos(caso.CenarioTesteId);
                                LimparCamposCaso();

                                if(caso.Passos != null)
                                {
                                    foreach (var passo in caso.Passos)
                                        repoPasso.Deletar(passo);
                                    repoPasso.Commit();
                                }

                                msgSucesso.Text = "Caso de Teste excluído com sucesso";
                                msgSucesso.DataBind();
                                ScriptManager.RegisterStartupScript(updPlano, updPlano.GetType(), "Pop", "openSuccessMsg();", true);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Ocorreu um erro ao deletar. Erro: " + ex.Message);
                        }
                    }
                    else if (e.CommandName == "Teste")
                    {
                        CarregarDesenvolvedorDropDown();
                        CarregarTesterDropDown();
                        CarregarTiposErroDropDown();
                        CarregarGridTestes(caso.CasoTesteId);
                        txtCasoTeste.Text = caso.Descricao;
                        txtIdCaso.Value = caso.CasoTesteId.ToString();
                        txtDataTeste.Text = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");
                        OpcaoDiv("teste");
                    }
                }
            }
            catch (Exception ex)
            {
                msgErro.Text = ex.Message;
                msgErro.DataBind();
                ScriptManager.RegisterStartupScript(updPlano, updPlano.GetType(), "Pop", "openErrorMsg();", true);
            }
        }

        protected void btnAddPasso_Click(object sender, EventArgs e)
        {
            LimparMsgErro();
            if (ValidarCamposPasso())
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(txtDescricaoPasso.Text))
                    {
                        erroDescricaoPasso.Text = "Campo Obrigatório";
                        erroDescricaoPasso.DataBind();
                    }
                    List<Passo> passos = new List<Passo>();
                    if (gridPassos.Rows != null)
                    {
                        int i = 1;
                        foreach (GridViewRow row in gridPassos.Rows)
                        {
                            int id = int.Parse(row.Cells[0].Text);
                            Passo passo = new Passo();
                            passo.PassoId = id;
                            passo.Numero = i;
                            passo.Descricao = Context.Server.HtmlDecode(row.Cells[2].Text);
                            passos.Add(passo);
                            i++;
                        }
                    }
                    var descricao = txtDescricaoPasso.Text;
                    var existe = passos?.FirstOrDefault(x => x.Descricao == descricao);
                    if(existe != null) throw new Exception("Já existe um passo com esta descrição");
                    Passo p = new Passo();
                    p.Numero = passos == null ? 1 : passos.Count() + 1;
                    p.Descricao = descricao;
                    passos.Add(p);

                    gridPassos.DataSource = null;
                    gridPassos.DataSource = passos;
                    gridPassos.DataBind();
                    txtDescricaoPasso.Text = string.Empty;
                }
                catch (Exception ex)
                {
                    gridPassos.DataSource = null;
                    msgErro.Text = ex.Message;
                    ScriptManager.RegisterStartupScript(updPlano, updPlano.GetType(), "Pop", "openErrorMsg();", true);
                    msgErro.DataBind();
                }
            }
        }

        protected void gridPassos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                LimparMsgErro();
                int index = Convert.ToInt32(e.CommandArgument);

                if (e.CommandName == "Deletar")
                {
                    try
                    {
                        int numero = 1;
                        List<Passo> passos = new List<Passo>();
                        for (int i = 0; i < gridPassos.Rows.Count; i++)
                        {
                            if (i != index)
                            {
                                int id = 0;
                                int.TryParse(gridPassos.Rows[i].Cells[0].Text, out id);
                                Passo passo = new Passo();
                                passo.PassoId = id;
                                passo.Numero = numero;
                                passo.Descricao = Context.Server.HtmlDecode(gridPassos.Rows[i].Cells[2].Text);
                                passos.Add(passo);
                                numero++;
                            }                         
                        }
                        gridPassos.DataSource = null;
                        gridPassos.DataSource = passos;
                        gridPassos.DataBind();
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
                msgSucesso.DataBind();
                ScriptManager.RegisterStartupScript(updPlano, updPlano.GetType(), "Pop", "openErrorMsg();", true);
            }
        }

        protected void btnAbrirPassos_Click(object sender, EventArgs e)
        {
            LimparMsgErro();
            if (divPassos.Visible) divPassos.Visible = false;
            else divPassos.Visible = true;
        }
        #endregion

        #region Teste
        protected void btnSalvarTeste_Click(object sender, EventArgs e)
        {
            LimparMsgErro();
            if (ValidarCamposTeste())
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(txtIdCaso.Value))
                        throw new Exception("Não foi possível encontrar o caso de teste. Favor atualizar a página.");

                    if (ddlDesenvolvedor.SelectedValue == ddlTester.SelectedValue)
                        throw new Exception("O desenvolvedor e o tester não podem ser a mesma pessoa.");

                    int idCaso = 0;
                    int.TryParse(txtIdCaso.Value, out idCaso);

                    var caso = repoCaso.ObterCasoPorId(idCaso);
                    var teste = new Teste();
                    #region Novo Teste
                    if (string.IsNullOrEmpty(txtIdTeste.Value))
                    {
                        var existe = caso.Testes?.FirstOrDefault(t => t.Situacao == 1);
                        if (existe != null) throw new Exception("Já existe um teste realizado com sucesso");
                        teste.CasoTesteId = caso == null ? throw new Exception("Caso de Teste não encontrado") : caso.CasoTesteId;
                        teste.DesenvolvedorId = int.Parse(ddlDesenvolvedor.SelectedValue);
                        teste.TesterId = int.Parse(ddlTester.SelectedValue);
                        teste.Data = DateTime.Parse(txtDataTeste.Text);
                        teste.Situacao = int.Parse(ddlSituacaoTeste.SelectedValue);

                        repoTeste.Adicionar(teste);
                        repoTeste.Commit();

                        //Adicionar Tipos de Erro se houver
                        if (gridTipoErro.Rows != null && gridTipoErro.Rows.Count > 0)
                        {
                            foreach (GridViewRow row in gridTipoErro.Rows)
                            {
                                var tipoErro = repoTipoErro.Find(long.Parse(row.Cells[0].Text));
                                if (tipoErro != null)
                                {
                                    if (teste.Erros == null) teste.Erros = new List<TipoErro>();
                                    teste.Erros.Add(tipoErro);
                                }
                            }
                            repoTeste.Commit();
                        }
                    }
                    #endregion
                    #region Editar Teste
                    else
                    {
                        var idTeste = int.Parse(txtIdTeste.Value);
                        teste = repoTeste.ObterTestePorId(idTeste);
                        if (teste == null) throw new Exception("Não foi possível encontrar teste. Favor atualizar a página.");
                        teste.DesenvolvedorId = int.Parse(ddlDesenvolvedor.SelectedValue);
                        teste.TesterId = int.Parse(ddlTester.SelectedValue);
                        teste.Data = DateTime.Parse(txtDataTeste.Text);
                        teste.Situacao = int.Parse(ddlSituacaoTeste.SelectedValue);
                        repoTeste.Commit();

                        //Verificar Tipos de erro
                        List<TipoErro> tiposErroAdd = new List<TipoErro>();
                        List<TipoErro> tiposErroDel = new List<TipoErro>();
                        List<TipoErro> tiposErroAtual = new List<TipoErro>();
                        if (gridTipoErro.Rows != null && gridTipoErro.Rows.Count > 0)
                        {
                            //Obter tipos de erro na lista atual
                            foreach (GridViewRow row in gridTipoErro.Rows)
                            {
                                var tipoErro = repoTipoErro.Find(long.Parse(row.Cells[0].Text));
                                if (tipoErro != null)
                                {
                                    tiposErroAtual.Add(tipoErro);
                                }
                            }
                            //Obter tipos de erro na lista
                            var tiposErroAnterior = repoTipoErro.ObterTipoErroPorTeste(teste.TesteId);

                            //Se não tiver tipos de erro antes, deve-se considerar apenas os atuais
                            //Caso tenha tipos de erro anterior, deve-se verificar se foi add ou del algum tipo de erro
                            if (tiposErroAnterior != null)
                            {
                                //Adicionar tipos de erro que devem ser adicionados na lista para adicionar
                                if (tiposErroAtual != null)
                                {
                                    foreach (var tp in tiposErroAtual)
                                    {
                                        //Verifica se o tipo de erro na lista atual existe na lista anterior
                                        var existe = tiposErroAnterior.FirstOrDefault(t => t.TipoErroId == tp.TipoErroId);
                                        //Se não existe, deve-se adicionar o novo tipo de erro
                                        if (existe == null)
                                            tiposErroAdd.Add(tp);
                                    }
                                    foreach (var tp in tiposErroAnterior)
                                    {
                                        //Verifica se o tipo de erro na lista anteror existe na atual
                                        var existe = tiposErroAtual.FirstOrDefault(t => t.TipoErroId == tp.TipoErroId);
                                        //Se não existe, deve-se deletar o tipo de erro
                                        if (existe == null)
                                            tiposErroDel.Add(tp);
                                    }
                                }
                            }

                            foreach (var tp in tiposErroAdd)
                            {
                                if (teste.Erros == null) teste.Erros = new List<TipoErro>();
                                teste.Erros.Add(tp);
                            }

                            foreach (var tp in tiposErroDel)
                            {
                                repoTeste.RemoveTipoErro(teste.TesteId, tp.TipoErroId);
                            }

                            repoTeste.Commit();
                        }
                    }
                    #endregion

                    //repoTeste.Commit();

                    txtIdCaso.Value = caso.CasoTesteId.ToString();
                    txtIdTeste.Value = teste.TesteId.ToString();

                    this.CarregarGridTestes(idCaso);

                    btnCancelarEditarTeste.Visible = true;

                    msgSucesso.Text = "Teste salvo com sucesso";
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

        protected void btnCancelarEditarTeste_Click(object sender, EventArgs e)
        {
            LimparMsgErro();
            LimparCamposTeste();
        }

        protected void btnVoltarCaso_Click(object sender, EventArgs e)
        {
            LimparMsgErro();
            LimparCamposTeste();
            OpcaoDiv("caso");
        }

        protected void gridTestes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                LimparMsgErro();
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gridTestes.Rows[index];
                if (row.Cells.Count >= 3)
                {
                    int id = 0;
                    int.TryParse(row.Cells[0].Text, out id);
                    if (id == 0)
                        throw new Exception("Id não pode ser zero");
                    var teste = repoTeste.ObterTestePorId(id);
                    if (teste == null)
                        throw new Exception("Teste não encontrado");

                    if (e.CommandName == "Editar")
                    {
                        try
                        {
                            txtIdTeste.Value = teste.TesteId.ToString();
                            txtCasoTeste.Text = teste.CasoTeste.Descricao;
                            txtDataTeste.Text = teste.Data.ToString("yyyy-MM-ddTHH:mm");
                            ddlDesenvolvedor.SelectedValue = teste.DesenvolvedorId.ToString();
                            ddlTester.SelectedValue = teste.TesterId.ToString();
                            ddlSituacaoTeste.SelectedValue = teste.Situacao.ToString();

                            if (teste.Situacao == 2) divTipoErro.Visible = true;

                            if (teste.Erros != null)
                            {
                                var erros = teste.Erros;
                                List<object> errosObj = new List<object>();
                                foreach (var erro in erros)
                                {
                                    var obj = new
                                    {
                                        TipoErroId = erro.TipoErroId,
                                        Descricao = erro.Descricao,
                                        Gravidade = obterTipoErroGravidade(erro.Gravidade)
                                    };
                                    errosObj.Add(obj);
                                }
                                gridTipoErro.DataSource = null;
                                gridTipoErro.DataSource = errosObj;
                                gridTipoErro.DataBind();
                            }
                            else
                            {
                                gridTipoErro.DataSource = null;
                                gridTipoErro.DataBind();
                            }

                            btnCancelarEditarTeste.Visible = true;
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
                                repoTeste.Deletar(teste);
                                repoTeste.Commit();
                                CarregarGridTestes(teste.CasoTesteId);

                                msgSucesso.Text = "Teste excluído com sucesso";
                                msgSucesso.DataBind();
                                ScriptManager.RegisterStartupScript(updPlano, updPlano.GetType(), "Pop", "openSuccessMsg();", true);
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
                ScriptManager.RegisterStartupScript(updPlano, updPlano.GetType(), "Pop", "openErrorMsg();", true);
            }
        }

        protected void btnAddTipoErro_Click(object sender, EventArgs e)
        {
            try
            {
                LimparMsgErro();
                List<TipoErro> erros = new List<TipoErro>();
                if (gridTipoErro.Rows != null)
                {
                    foreach (GridViewRow row in gridTipoErro.Rows)
                    {
                        long id = long.Parse(row.Cells[0].Text);
                        var tp = repoTipoErro.Find(id);
                        if (tp != null)
                        {
                            erros.Add(tp);
                        }
                    }
                }
                var idTipoErro = int.Parse(ddlTipoErro.SelectedValue);
                var existe = erros.FirstOrDefault(t => t.TipoErroId == idTipoErro);
                if (existe == null)
                {
                    var tipoErro = repoTipoErro.ObterTipoErroPorId(idTipoErro);
                    if (tipoErro == null) throw new Exception("Tipo de erro não encontrado. Favor atualizar a página");
                    erros.Add(tipoErro);
                }

                List<object> errosObj = new List<object>();
                foreach (var erro in erros)
                {
                    var obj = new
                    {
                        TipoErroId = erro.TipoErroId,
                        Descricao = erro.Descricao,
                        Gravidade = obterTipoErroGravidade(erro.Gravidade)
                    };
                    errosObj.Add(obj);
                }
                gridTipoErro.DataSource = null;
                gridTipoErro.DataSource = errosObj;
                gridTipoErro.DataBind();
            }
            catch (Exception ex)
            {
                gridTipoErro.DataSource = null;
                msgErro.Text = ex.Message;
                ScriptManager.RegisterStartupScript(updPlano, updPlano.GetType(), "Pop", "openErrorMsg();", true);
                msgErro.DataBind();
            }
        }

        protected void ddlSituacaoTeste_SelectedIndexChanged(object sender, EventArgs e)
        {
            LimparMsgErro();
            if (ddlSituacaoTeste.SelectedIndex == 0)
            {
                divTipoErro.Visible = false;
                gridTipoErro.DataSource = null;
                ddlTipoErro.SelectedIndex = -1;
            }

            else
                divTipoErro.Visible = true;
        }

        protected void gridTipoErro_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                LimparMsgErro();
                int index = Convert.ToInt32(e.CommandArgument);

                if (e.CommandName == "Deletar")
                {
                    try
                    {
                        List<TipoErro> erros = new List<TipoErro>();
                        for (int i = 0; i < gridTipoErro.Rows.Count; i++)
                        {
                            if (i != index)
                            {
                                int id = 0;
                                int.TryParse(gridTipoErro.Rows[i].Cells[0].Text, out id);
                                var erro = repoTipoErro.Find(id);
                                if (erro != null)
                                    erros.Add(erro);
                            }
                        }
                        List<object> errosObj = new List<object>();
                        foreach (var erro in erros)
                        {
                            var obj = new
                            {
                                TipoErroId = erro.TipoErroId,
                                Descricao = erro.Descricao,
                                Gravidade = obterTipoErroGravidade(erro.Gravidade)
                            };
                            errosObj.Add(obj);
                        }
                        gridTipoErro.DataSource = null;
                        gridTipoErro.DataSource = errosObj;
                        gridTipoErro.DataBind();
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
                msgSucesso.DataBind();
                ScriptManager.RegisterStartupScript(updPlano, updPlano.GetType(), "Pop", "openErrorMsg();", true);
            }
        }
        #endregion


        #region Métodos Auxiliares   

        #region Geral  
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
                    btnCenarios.Visible = true;

                    this.CarregarFuncionalidadesDropDown(plano.PlanoTesteId);

                    var cenarios = repoCenario.ObterCenariosPorPlano(plano.PlanoTesteId);
                    if (cenarios != null && cenarios.Count() > 0)
                    {
                        //btnCasos.Visible = true;
                        CarregarCenariosGrid(plano.PlanoTesteId);
                        //TODO: Carregar grid de cenarios na aba de casos de teste e a grid de casos de teste
                    }
                    //else
                    //btnCasos.Visible = false;
                }
                else
                    btnCenarios.Visible = false;

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

        private void CarregarListaFuncionalidades(int idSprint, PlanoTeste plano = null)
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

        #region Cenário
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

        private void LimparCamposCenario()
        {
            var idPlano = int.Parse(txtId.Value);
            CarregarFuncionalidadesDropDown(idPlano);
            ddlSituacao.SelectedIndex = 0;
            txtIdCenario.Value = string.Empty;
            btnCancelarEditarCenario.Visible = false;
            ddlFuncionalidades.Enabled = true;
        }

        private void CarregarFuncionalidadesDropDown(int idPlano)
        {
            var funcionalidades = repoFuncionalidade.ObterFuncionalidadesNaoUsadasPorPlano(idPlano);
            ddlFuncionalidades.DataSource = null;
            ddlFuncionalidades.DataSource = funcionalidades;
            ddlFuncionalidades.DataTextField = "Nome";
            ddlFuncionalidades.DataValueField = "FuncionalidadeId";
            ddlFuncionalidades.DataBind();
        }

        private void CarregarCenariosGrid(int idPlano)
        {
            gridCenarios.DataSource = null;
            var cenarios = repoCenario.ObterCenariosPorPlano(idPlano);

            var cenariosObj = new List<object>();

            foreach (var cenario in cenarios)
            {
                var obj = new
                {
                    CenarioTesteId = cenario.CenarioTesteId,
                    Descricao = cenario.Funcionalidade.Nome,
                    Situacao = getNomeSituacao(cenario.Situacao)
                };
                cenariosObj.Add(obj);
            }

            if (cenariosObj != null && cenariosObj.Count == 0) cenariosObj = null;

            gridCenarios.DataSource = cenariosObj;
            gridCenarios.DataBind();
        }
        #endregion

        #region Caso
        private void LimparCamposCaso()
        {
            txtDescricaoCaso.Text = string.Empty;
            txtCategoria.Text = string.Empty;
            txtEntrada.Text = string.Empty;
            txtRespostaEsperada.Text = string.Empty;
            txtIdCaso.Value = string.Empty;
            btnCancelarEditarCaso.Visible = false;
            divPassos.Visible = false;
            txtDescricaoPasso.Text = string.Empty;
        }

        private void CarregarGridCasos(int idCenario)
        {
            gridCasos.DataSource = null;
            var casos = repoCaso.ObterCasosPorCenario(idCenario);

            if (casos != null && casos.Count == 0) casos = null;

            gridCasos.DataSource = casos;
            gridCasos.DataBind();
        }

        private bool ValidarCamposCaso()
        {
            bool valido = true;
            if (string.IsNullOrWhiteSpace(txtDescricaoCaso.Text))
            {
                erroDescricaoCaso.Text = "Campo obrigatório";
                valido = false;
            }
            if (string.IsNullOrWhiteSpace(txtCategoria.Text))
            {
                erroCategoria.Text = "Campo obrigatório";
                valido = false;
            }
            if (string.IsNullOrWhiteSpace(txtEntrada.Text))
            {
                erroEntrada.Text = "Campo obrigatório";
                valido = false;
            }
            if (string.IsNullOrWhiteSpace(txtRespostaEsperada.Text))
            {
                erroRespostaEsperada.Text = "Campo obrigatório";
                valido = false;
            }
            return valido;
        }

        private bool ValidarCamposPasso()
        {
            if (string.IsNullOrWhiteSpace(txtDescricaoPasso.Text))
            {
                erroDescricaoPasso.Text = "Campo obrigatório";
                return false;
            }
            return true;
        }
        #endregion

        #region Teste
        private string obterTipoErroGravidade(int codGravidade)
        {
            if (codGravidade == 1)
                return "Baixo";
            else if (codGravidade == 2)
                return "Médio";
            else
                return "Alto";
        }
        private void CarregarTiposErroDropDown()
        {
            var tiposErro = repoTipoErro.GetAll()?.ToList();
            ddlTipoErro.DataSource = null;
            ddlTipoErro.DataSource = tiposErro;
            ddlTipoErro.DataTextField = "Descricao";
            ddlTipoErro.DataValueField = "TipoErroId";
            ddlTipoErro.DataBind();
        }
        private void CarregarDesenvolvedorDropDown()
        {
            int idProjeto = int.Parse(dblProjetos.SelectedValue);
            var desenvolvedores = repoMembro.ObterMembrosPorProjeto(idProjeto);
            ddlDesenvolvedor.DataSource = null;
            ddlDesenvolvedor.DataSource = desenvolvedores;
            ddlDesenvolvedor.DataTextField = "Nome";
            ddlDesenvolvedor.DataValueField = "MembroId";
            ddlDesenvolvedor.DataBind();
        }
        private void CarregarTesterDropDown()
        {
            int idProjeto = int.Parse(dblProjetos.SelectedValue);
            var testers = repoMembro.ObterMembrosPorProjeto(idProjeto);
            ddlTester.DataSource = null;
            ddlTester.DataSource = testers;
            ddlTester.DataTextField = "Nome";
            ddlTester.DataValueField = "MembroId";
            ddlTester.DataBind();
        }
        private void LimparCamposTeste()
        {
            txtIdTeste.Value = string.Empty;
            txtDataTeste.Text = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");
            ddlDesenvolvedor.SelectedIndex = 0;
            ddlTester.SelectedIndex = 0;
            divTipoErro.Visible = false;
            ddlSituacaoTeste.SelectedIndex = 0;
            gridTipoErro.DataSource = null;
            gridTipoErro.DataBind();
            btnCancelarEditarTeste.Visible = false;
        }

        private void CarregarGridTestes(int idCaso)
        {
            gridTestes.DataSource = null;
            var testes = repoTeste.ObterTestesPorCaso(idCaso);

            if (testes != null && testes.Count == 0) testes = null;

            List<object> testesObj = new List<object>();
            if (testes != null)
            {
                foreach (var teste in testes)
                {
                    var desenvolvidor = repoMembro.Find(teste.DesenvolvedorId);
                    var tester = repoMembro.Find(teste.TesterId);
                    var obj = new
                    {
                        TesteId = teste.TesteId,
                        Data = teste.Data.ToString("dd/MM/yyyy HH:mm"),
                        Desenvolvedor = desenvolvidor.Nome ?? string.Empty,
                        Tester = tester.Nome ?? string.Empty,
                        Situacao = teste.Situacao == 1 ? "Sucesso" : "Falha"
                    };
                    testesObj.Add(obj);
                }
            }

            gridTestes.DataSource = testesObj;
            gridTestes.DataBind();
        }

        private bool ValidarCamposTeste()
        {
            bool valido = true;
            if (ddlDesenvolvedor.SelectedItem == null)
            {
                erroDesenvolvedor.Text = "Campo obrigatório";
                valido = false;
            }
            if (ddlTester.SelectedItem == null)
            {
                erroTester.Text = "Campo obrigatório";
                valido = false;
            }
            if (string.IsNullOrWhiteSpace(txtDataTeste.Text))
            {
                erroDataTeste.Text = "Campo obrigatório";
                valido = false;
            }
            if (ddlSituacaoTeste.SelectedItem == null)
            {
                erroSituacaoTeste.Text = "Campo obrigatório";
                valido = false;
            }
            return valido;
        }
        #endregion

        private string getNomeSituacao(int codSituacao)
        {
            switch (codSituacao)
            {
                case 1: return "A fazer";
                case 2: return "Desenvolvimeto";
                case 3: return "Teste";
                case 4: return "Concluído";
                default: return "Erro";
            }
        }

        private void LimparMsgErro()
        {
            erroDescricao.Text = string.Empty;
            erroDescricao.DataBind();

            erroDataCriacao.Text = string.Empty;
            erroDataCriacao.DataBind();

            erroDataFim.Text = string.Empty;
            erroDataFim.DataBind();

            erroFuncionalidade.Text = string.Empty;
            erroFuncionalidade.DataBind();

            erroFuncionalidadesTestadas.Text = string.Empty;
            erroFuncionalidadesTestadas.DataBind();

            erroSituacao.Text = string.Empty;
            erroSituacao.DataBind();

            erroDescricaoCaso.Text = string.Empty;
            erroCategoria.Text = string.Empty;
            erroEntrada.Text = string.Empty;
            erroRespostaEsperada.Text = string.Empty;

            erroDataTeste.Text = string.Empty;
            erroSituacaoTeste.Text = string.Empty;

            erroDescricaoPasso.Text = string.Empty;
        }

        private void OpcaoDiv(string nome)
        {
            bool geral = true;
            bool cenario = false;
            bool caso = false;
            bool teste = false;
            switch (nome)
            {
                case "geral":
                    geral = true;
                    cenario = false;
                    caso = false;
                    teste = false;
                    break;
                case "cenario":
                    geral = false;
                    cenario = true;
                    caso = false;
                    teste = false;
                    break;
                case "caso":
                    geral = false;
                    cenario = false;
                    caso = true;
                    teste = false;
                    break;
                case "teste":
                    geral = false;
                    cenario = false;
                    caso = false;
                    teste = true;
                    break;
            }

            divGeral.Visible = geral;
            divCenario.Visible = cenario;
            divCaso.Visible = caso;
            divTeste.Visible = teste;
        }
        #endregion
    }
}
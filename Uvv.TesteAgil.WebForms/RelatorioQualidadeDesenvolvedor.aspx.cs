using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Uvv.TesteAgil.Dados.Repositorio;
using Uvv.TesteAgil.Entidades.Modelos;
using System.Web.UI.DataVisualization;
using System.Web.UI.DataVisualization.Charting;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using TuesPechkin;
using System.Drawing.Printing;

namespace Uvv.TesteAgil.WebForms
{
    public partial class RelatorioQualidadeDesenvolvedor : PaginaBase
    {
        TesteRepositorio repoTeste = new TesteRepositorio();
        MembroRepositorio repoMembro = new MembroRepositorio();
        ProjetoRepositorio repoProjeto = new ProjetoRepositorio();
        SprintRepositorio repoSprint = new SprintRepositorio();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ValidarSessao();

                var idUsuario = GetUserId();

                CarregarDropProjetos(idUsuario);
                CarregarDropDesenvolvedor();
            }
        }

        protected string ObterDados()
        {
            //DataTable dados = new DataTable();
            //dados.Columns.Add("Task", typeof(string));
            //dados.Columns.Add("Ours Per Day", typeof(string));

            //dados.Rows.Add(new Object[] { "Work", 11 });
            //dados.Rows.Add(new Object[] { "Eat", 2 });
            //dados.Rows.Add(new Object[] { "Watch TV", 3 });
            //dados.Rows.Add(new Object[] { "Sleep", 8 });

            //string strDados = "[['Task', 'Hours Per Day'],";
            //foreach(DataRow dr in dados.Rows)
            //{
            //    strDados = strDados + "[";
            //    strDados = strDados + "'"+dr[0]+"'"+","+dr[1];
            //    strDados = strDados + "],";
            //}
            //strDados = strDados + "]";
            //return strDados;

            return GerarRelatorioQualidadePorDesenvolvedor();
        }

        protected void btnExportar_Click(object sender, EventArgs e)
        {
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    Page.RenderControl(hw);
                    string html = sw.ToString();
                    //StringReader html = new StringReader(sw.ToString());
                    //Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                    //var writer = PdfWriter.GetInstance(pdfDoc, html);
                    //PdfWriter.GetInstance(pdfDoc, new FileStream(Server.MapPath("~") + "Export" + ".pdf", FileMode.Create));
                    //PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                    //Response.Write(pdfDoc);
                    //Response.End();
                    var pdf = RenderizarPdf(html, "Relatório de Qualidade");
                    //System.IO.MemoryStream ms = new System.IO.MemoryStream(pdf);
                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Type", "application/pdf");
                    Response.AddHeader("Content-Disposition", "inline");
                    Response.BinaryWrite(pdf);
                    Response.End();
                }
            }
        }

        private void CarregarDropProjetos(int idUsuario)
        {
            var projetos = repoProjeto.ObterProjetosPorMembro(idUsuario);
            var projetosObj = new List<object>();
            projetosObj.Add(new
            {
                Nome = "Todos",
                ProjetoId = 0
            });
            foreach (var projeto in projetos)
            {
                projetosObj.Add(new
                {
                    Nome = projeto.Nome,
                    ProjetoId = projeto.ProjetoId
                });
            }
            ddlProjeto.DataSource = projetosObj;
            ddlProjeto.DataTextField = "Nome";
            ddlProjeto.DataValueField = "ProjetoId";
            ddlProjeto.DataBind();

            if (projetos != null && projetos.Count > 0)
            {
                CarregarDropSprints();
            }
        }

        public void CarregarDropSprints()
        {
            var sprintsObj = new List<object>();
            sprintsObj.Add(new
            {
                Numero = "Todas",
                SprintId = 0
            });
            if (!string.IsNullOrEmpty(ddlProjeto.SelectedValue) || ddlProjeto.SelectedValue != "0")
            {
                var sprints = repoSprint.ObterSprintsPorProjeto(int.Parse(ddlProjeto.SelectedValue));
                
                foreach(var sprint in sprints)
                {
                    sprintsObj.Add(new
                    {
                        Numero = sprint.Numero.ToString(),
                        SprintId = sprint.SprintId
                    });
                }                
            }
            ddlSprint.DataSource = sprintsObj;
            ddlSprint.DataTextField = "Numero";
            ddlSprint.DataValueField = "SprintId";
            ddlSprint.DataBind();
        }

        private void CarregarDropDesenvolvedor()
        {
            var desenvObj = new List<object>();
            desenvObj.Add(new
            {
                Nome = "Todos",
                MembroId = 0
            });
            var desenvolvedores = repoMembro.GetAll()?.ToList();
            foreach(var desenv in desenvolvedores)
            {
                desenvObj.Add(new
                {
                    Nome = desenv.Nome,
                    MembroId = desenv.MembroId
                });
            }
            ddlDesenvolvedor.DataSource = null;
            ddlDesenvolvedor.DataSource = desenvObj;
            ddlDesenvolvedor.DataTextField = "Nome";
            ddlDesenvolvedor.DataValueField = "MembroId";
            ddlDesenvolvedor.DataBind();
        }

        protected void ddlProjeto_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarDropSprints();
        }

        private string GerarRelatorioQualidadePorDesenvolvedor()
        {
            if (!IsPostBack)
            {
                //updRel.DataBind();
                int idProjeto = ddlProjeto.SelectedItem == null ? 0 : int.Parse(ddlProjeto.SelectedValue);
                int idSprint = ddlSprint.SelectedItem == null ? 0 : int.Parse(ddlSprint.SelectedValue);
                int idDesenvolvedor = ddlDesenvolvedor.SelectedItem == null ? 0 : int.Parse(ddlDesenvolvedor.SelectedValue);

                if (idDesenvolvedor == 0) return string.Empty;

                var testes = new List<Teste>();
                if (idProjeto != 0 && idSprint != 0)
                {
                    testes = repoTeste.ObterTestesPorDesenvolvedor(idProjeto, idSprint, idDesenvolvedor);
                }
                else if (idProjeto != 0)
                {
                    testes = repoTeste.ObterTestesPorDesenvolvedor(idProjeto, idDesenvolvedor);
                }
                else
                {
                    testes = repoTeste.ObterTestesPorDesenvolvedor(idDesenvolvedor);
                }

                var testesSucesso = testes?.Where(t => t.Situacao == 1)?.ToList();
                var testesFalha = testes?.Where(t => t.Situacao == 2)?.ToList();

                var qntTotal = testes == null ? 0 : testes.Count();
                var qntSucessos = testesSucesso == null ? 0 : testesSucesso.Count();
                var qntFalhas = testesFalha == null ? 0 : testesFalha.Count();

                if (qntSucessos == 0 && qntFalhas == 0)
                {
                    qntSucessos = 1; qntFalhas = 1;
                }

                string strDados = "[['Qualidade', 'Quantidade'],";
                strDados = strDados + "[";
                //strDados = strDados + "'Total'" + "," + qntTotal;
                //strDados = strDados + "],";
                //strDados = strDados + "[";
                strDados = strDados + "'Sucesso'" + "," + qntSucessos;
                strDados = strDados + "],";
                strDados = strDados + "[";
                strDados = strDados + "'Falha'" + "," + qntFalhas;
                strDados = strDados + "],";
                strDados = strDados + "]";
                return strDados;
            }
            return "[]";
        }

        private void GerarRelatorioQualidade()
        {
            int idProjeto = ddlProjeto.SelectedItem == null ? 0 : int.Parse(ddlProjeto.SelectedValue);
            int idSprint = ddlSprint.SelectedItem == null ? 0 : int.Parse(ddlSprint.SelectedValue);
            int idDesenvolvedor = ddlDesenvolvedor.SelectedItem == null ? 0 : int.Parse(ddlDesenvolvedor.SelectedValue);

            var testes = new List<Teste>();
            bool projeto = idProjeto != 0;
            bool sprint = idSprint != 0;
            bool desenv = idDesenvolvedor != 0;
            if (desenv && projeto && sprint)
                testes = repoTeste.ObterTestesPorDesenvolvedor(idProjeto, idSprint, idDesenvolvedor);
            else if (projeto && desenv)
                testes = repoTeste.ObterTestesPorDesenvolvedor(idProjeto, idDesenvolvedor);
            else if (projeto && sprint)
                testes = repoTeste.ObterTestesPorProjeto(idProjeto, idSprint);
            else if (projeto)
                testes = repoTeste.ObterTestesPorProjeto(idProjeto);
            else if (desenv)
                testes = repoTeste.ObterTestesPorDesenvolvedor(idDesenvolvedor);
            else
                testes = repoTeste.GetAll()?.ToList();

            var testesSucesso = testes?.Where(t => t.Situacao == 1)?.ToList();
            var testesFalha = testes?.Where(t => t.Situacao == 2)?.ToList();

            var qntTotal = testes == null ? 0 : testes.Count();
            var qntSucessos = testesSucesso == null ? 0 : testesSucesso.Count();
            var qntFalhas = testesFalha == null ? 0 : testesFalha.Count();

            List<object> listaRel = new List<object>();
            listaRel.Add(new
            {
                Situacao = "Sucesso",
                Quantidade = qntSucessos
            });
            listaRel.Add(new
            {
                Situacao = "Falha",
                Quantidade = qntFalhas
            });

            chartRel.DataSource = null;
            chartRel.DataSource = listaRel;
            chartRel.DataBind();

            chartRel2.DataSource = null;
            chartRel2.DataSource = listaRel;
            chartRel2.DataBind();
        }

        protected void btnGerarRelatorio_Click(object sender, EventArgs e)
        {
            GerarRelatorioQualidade();
            divRel.Visible = true;
            btnExportar.Visible = true;
        }

        private byte[] RenderizarPdf(string htmlCorpo, string nmeTitulo)
        {
            //Conversor PDF
        IConverter converter = new ThreadSafeConverter(new RemotingToolset<PdfToolset>(new WinAnyCPUEmbeddedDeployment(new TempFolderDeployment())));
        byte[] retorno = null;

            try
            {
                //Parâmetros para conversão do HTML em PDF
                var documento = new HtmlToPdfDocument
                {
                    GlobalSettings = {
                        ProduceOutline = true,
                        DocumentTitle = nmeTitulo,
                        PaperSize = PaperKind.A4,
                        Margins =
                        {
                            Top = 1,
                            Left = 1,
                            Right = 1,
                            Bottom = 1,
                            Unit = TuesPechkin.Unit.Centimeters
                        }
                    },

                    Objects = {
                        new ObjectSettings {
                            HtmlText = htmlCorpo,
                            WebSettings = new WebSettings() { LoadImages = true, PrintBackground = true, PrintMediaType = true },
                            IncludeInOutline = false
                        },
                    }
                };

                // Converter o documento HTML para um PDF
                retorno = converter.Convert(documento);
            }
            catch (Exception ex)
            {
                msgErro.Text = ex.Message;
                msgErro.Text = "Erro: " + ex.Message;
                msgErro.DataBind();
                //ScriptManager.RegisterStartupScript(updRel, updRel.GetType(), "Pop", "openErrorMsg();", true);
            }

            return retorno;
        }

    }
}
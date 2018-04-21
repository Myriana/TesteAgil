using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Uvv.TesteAgil.Dados.Contexto;
using Uvv.TesteAgil.Entidades.Modelos;
using Uvv.TesteAgil.Web.Models.ViewModels;
using Uvv.TesteAgil.Web.Util;

namespace Uvv.TesteAgil.Web.Controllers
{
    public class ScriptTesteController : Controller
    {
        private Contexto db = new Contexto();
        private MensagensPadrao msg = new MensagensPadrao();

        // GET: ScriptTeste
        public ActionResult Index()
        {
            return View(db.ScriptTeste.ToList());
        }

        // GET: ScriptTeste/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScriptTeste scriptTeste = db.ScriptTeste.Find(id);
            if (scriptTeste == null)
            {
                return HttpNotFound();
            }
            return View(scriptTeste);
        }

        // GET: ScriptTeste/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ScriptTeste/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ScriptTesteId,Nome")] ScriptTeste scriptTeste)
        {
            if (ModelState.IsValid)
            {
                db.ScriptTeste.Add(scriptTeste);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(scriptTeste);
        }

        // GET: ScriptTeste/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScriptTeste scriptTeste = db.ScriptTeste.Find(id);
            if (scriptTeste == null)
            {
                return HttpNotFound();
            }
            return View(scriptTeste);
        }

        // POST: ScriptTeste/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ScriptTesteId,Nome")] ScriptTeste scriptTeste)
        {
            if (ModelState.IsValid)
            {
                db.Entry(scriptTeste).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(scriptTeste);
        }

        // GET: ScriptTeste/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScriptTeste scriptTeste = db.ScriptTeste.Find(id);
            if (scriptTeste == null)
            {
                return HttpNotFound();
            }
            return View(scriptTeste);
        }

        // POST: ScriptTeste/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ScriptTeste scriptTeste = db.ScriptTeste.Find(id);
            var passos = db.Passo.Where(x => x.ScriptTeste.ScriptTesteId == id);
            foreach (var passo in passos)
                db.Passo.Remove(passo);
            db.ScriptTeste.Remove(scriptTeste);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult SalvarPasso(PassoVM[] passos, string nome)
        {
            try
            {
                ScriptTeste script = new ScriptTeste();
                script.Nome = nome;
                script.Passos = new List<Passo>();

                foreach (var passoVm in passos)
                {
                    var passo = new Passo();
                    passo.Numero = int.Parse(passoVm.Numero);
                    passo.Descricao = passoVm.Descricao;
                    
                    script.Passos.Add(passo);
                }
                db.ScriptTeste.Add(script);
                db.SaveChanges();
                TempData["Sucesso"] = msg.mensagemSucesso("Script de Teste", "criado");
            }
            catch (Exception ex)
            {
                TempData["Erro"] = msg.mensagemErro(ex.Message);
            }

            return Json(passos);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

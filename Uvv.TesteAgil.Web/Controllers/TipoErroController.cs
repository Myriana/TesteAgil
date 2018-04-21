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
using Uvv.TesteAgil.Web.Util;

namespace Uvv.TesteAgil.Web.Controllers
{
    public class TipoErroController : Controller
    {
        private Contexto db = new Contexto();
        private MensagensPadrao msg = new MensagensPadrao();

        // GET: TipoErro
        public ActionResult Index()
        {
            return View(db.TipoErro.ToList());
            //return View();
        }

        // GET: TipoErro/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoErro tipoErro = db.TipoErro.Find(id);
            if (tipoErro == null)
            {
                return HttpNotFound();
            }
            return View(tipoErro);
        }

        // GET: TipoErro/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoErro/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TipoErroId,Descricao,Gravidade")] TipoErro tipoErro)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.TipoErro.Add(tipoErro);
                    db.SaveChanges();

                    TempData["Sucesso"] = msg.mensagemSucesso("Tipo de Erro", "cadastrado");

                    return RedirectToAction("Index");
                    //return Json(new { resultado = true, message = "Tipo de erro cadastrado com sucesso" });
                }

                IEnumerable<ModelError> erros = ModelState.Values.SelectMany(item => item.Errors);

                //return PartialView();
                return Json(new { resultado = false, message = erros });
            }
            catch(Exception ex)
            {
                TempData["Erro"] = msg.mensagemErro(ex.Message);
                return RedirectToAction("Index");
            }
        }

        // GET: TipoErro/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoErro tipoErro = db.TipoErro.Find(id);
            if (tipoErro == null)
            {
                return HttpNotFound();
            }
            return View(tipoErro);
        }

        // POST: TipoErro/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TipoErroId,Descricao,Gravidade")] TipoErro tipoErro)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(tipoErro).State = EntityState.Modified;
                    db.SaveChanges();

                    TempData["Sucesso"] = msg.mensagemSucesso("Tipo de Erro", "editado");

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["Erro"] = msg.mensagemErro(ex.Message);
                return RedirectToAction("Index");
            }

            return View(tipoErro);
        }

        // GET: TipoErro/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoErro tipoErro = db.TipoErro.Find(id);
            if (tipoErro == null)
            {
                return HttpNotFound();
            }
            return View(tipoErro);
        }

        // POST: TipoErro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)

        {
            try
            {
                TipoErro tipoErro = db.TipoErro.Find(id);
                db.TipoErro.Remove(tipoErro);
                db.SaveChanges();

                TempData["Sucesso"] = msg.mensagemSucesso("Tipo de Erro", "deletado");

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Erro"] = msg.mensagemErro(ex.Message);
                return RedirectToAction("Index");
            }           
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

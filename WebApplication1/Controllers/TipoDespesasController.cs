using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.Classes;

namespace WebApplication1.Controllers
{
    public class TipoDespesasController : Controller
    {
        private WebApplication1Context db = new WebApplication1Context();

        // GET: TipoDespesas
        public ActionResult Index(string buscar)
        {
            if (String.IsNullOrEmpty(buscar))
            {
                return View(db.TipoDespesas.ToList());
            }
            var result = db.TipoDespesas.Where(x => x.Nome.ToLower().Contains(buscar) && x.Status == EnumStatus.ATIVO);
            return View(result.ToList());
        }

        // GET: TipoDespesas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDespesa tipoDespesa = db.TipoDespesas.Find(id);
            if (tipoDespesa == null)
            {
                return HttpNotFound();
            }
            return View(tipoDespesa);
        }

        // GET: TipoDespesas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoDespesas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nome,Status")] TipoDespesa tipoDespesa)
        {
            if (ModelState.IsValid)
            {
                //var result = db.TipoDespesas.Where(tipo => tipo.Nome.ToUpper().Equals(tipoDespesa.Nome.ToUpper()));
                //if (result.Count() == 0)
                //{

                //}
                db.TipoDespesas.Add(tipoDespesa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipoDespesa);
        }

        // GET: TipoDespesas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDespesa tipoDespesa = db.TipoDespesas.Find(id);
            if (tipoDespesa == null)
            {
                return HttpNotFound();
            }
            return View(tipoDespesa);
        }

        // POST: TipoDespesas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nome,Status")] TipoDespesa tipoDespesa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoDespesa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipoDespesa);
        }

        // GET: TipoDespesas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDespesa tipoDespesa = db.TipoDespesas.Find(id);
            if (tipoDespesa == null)
            {
                return HttpNotFound();
            }
            return View(tipoDespesa);
        }

        // POST: TipoDespesas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TipoDespesa tipoDespesa = db.TipoDespesas.Find(id);
            tipoDespesa.Status = EnumStatus.INATIVO;
            // db.TipoDespesas.Remove(tipoDespesa);
            db.SaveChanges();
            return RedirectToAction("Index");
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

using System;
using System.Collections;
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
    public class DespesasController : Controller
    {
        private WebApplication1Context db = new WebApplication1Context();

        // GET: Despesas
        public ActionResult Index(String buscar, String buscarIni, String buscarFim, String Tipo)
        {
            DateTime date1, date2;
            if (String.IsNullOrEmpty(buscarIni) && String.IsNullOrEmpty(buscar) && String.IsNullOrEmpty(buscarFim) && (String.IsNullOrEmpty(Tipo) || Tipo.Equals("Todas")))
            {
                return View(db.Despesas.Include(db => db.TipoDespesa).ToList());
            }
            if (String.IsNullOrEmpty(buscarIni))
            {
                date1 = new DateTime(0001, 01, 01);
            }
            else
            {
                date1 = DateTime.Parse(buscarIni);
            }
            if (String.IsNullOrEmpty(buscarFim))
            {
                date2 = DateTime.Today;
            }
            else
            {
                date2 = DateTime.Parse(buscarFim);
            }

            var result = db.Despesas.Include(db => db.TipoDespesa).Where(x => x.DataRealizacao.CompareTo(date1) >= 0 && x.DataRealizacao.CompareTo(date2) < 0 && x.TipoDespesa.Nome.ToLower().Contains(buscar));
            if (Tipo.Equals("Ofx"))
            {
                result = result.Where(x => x.Ofx == true);
            }


            return View(result.ToList());
        }

        // GET: Despesas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Despesa despesa = db.Despesas.Find(id);
            if (despesa == null)
            {
                return HttpNotFound();
            }
            return View(despesa);
        }

        // GET: Despesas/Create
        public ActionResult Create()
        {
            ArrayList lista = new ArrayList();
            for (int i = 1; i < 7; i++)
            {
                lista.Add(i);
            }
            ViewBag.NumParcelas = new SelectList(lista);
            ViewBag.IdTipoDespesa = new SelectList(db.TipoDespesas, "Id", "Nome");
            return View();
        }

        // POST: Despesas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,NomeDespesa,Valor,IdTipoDespesa,CaractDespesa,DataRealizacao,Parcelamento,NumeroParcelas,VencimentoPrimeiraParcela")] Despesa despesa, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                despesa.NumeroParcelas = int.Parse(form["NumParcelas"]);

                despesa.Ofx = false;
                db.Despesas.Add(despesa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ArrayList lista = new ArrayList();
            for (int i = 1; i < 7; i++)
            {
                lista.Add(i);
            }
            ViewBag.NumParcelas = new SelectList(lista, despesa.NumeroParcelas);
            ViewBag.IdTipoDespesa = new SelectList(db.TipoDespesas, "Id", "Nome", despesa.IdTipoDespesa);
            return View(despesa);
        }

        // GET: Despesas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Despesa despesa = db.Despesas.Find(id);
            if (despesa == null)
            {
                return HttpNotFound();
            }
            ArrayList lista = new ArrayList();
            for (int i = 1; i < 7; i++)
            {
                lista.Add(i);
            }
            ViewBag.NumParcelas = new SelectList(lista, despesa.NumeroParcelas);
            ViewBag.IdTipoDespesa = new SelectList(db.TipoDespesas, "Id", "Nome", despesa.IdTipoDespesa);
            return View(despesa);
        }

        // POST: Despesas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,NomeDespesa,Valor,IdTipoDespesa,CaractDespesa,DataRealizacao,Parcelamento,NumeroParcelas,VencimentoPrimeiraParcela")] Despesa despesa, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                despesa.NumeroParcelas = int.Parse(form["NumParcelas"]);
                db.Entry(despesa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ArrayList lista = new ArrayList();
            for (int i = 1; i < 7; i++)
            {
                lista.Add(i);
            }
            ViewBag.NumParcelas = new SelectList(lista, despesa.NumeroParcelas);
            ViewBag.IdTipoDespesa = new SelectList(db.TipoDespesas, "Id", "Nome", despesa.IdTipoDespesa);
            return View(despesa);
        }

        // GET: Despesas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Despesa despesa = db.Despesas.Find(id);
            if (despesa == null)
            {
                return HttpNotFound();
            }
            return View(despesa);
        }

        // POST: Despesas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Despesa despesa = db.Despesas.Find(id);
            db.Despesas.Remove(despesa);
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

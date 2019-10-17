using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.Classes;

namespace ASP.Net_CanvasJS_Tutorial.Controllers
{
    public class HomeController : Controller
    {
        private WebApplication1Context db = new WebApplication1Context();
        // GET: Home
        public ActionResult Index()
        {
            //Obter lista com lançamentos mensais
            #region Lançamentos
            var lista = new List<ItemExtrato>();
            var despesas = db.Despesas.ToList();
            foreach (var obj in despesas)
            {
                for (int i = 1; i <= obj.NumeroParcelas; i++)
                {
                    ItemExtrato item = new ItemExtrato();
                    item.Valor = obj.Valor / obj.NumeroParcelas * -1;
                    item.DataVencimento = obj.VencimentoPrimeiraParcela.AddMonths(i - 1);
                    lista.Add(item);
                }
            }
            var receitas = db.Receitas.ToList();
            foreach (var obj in receitas)
            {
                for (int i = 1; i <= obj.NumeroParcelas; i++)
                {
                    ItemExtrato item = new ItemExtrato();
                    item.Valor = obj.Valor / obj.NumeroParcelas;
                    item.DataVencimento = obj.PrimeiraDataVencimento.AddMonths(i - 1);
                    lista.Add(item);
                }
            }
            lista.Sort();
            #endregion

            //Obter Total de lançamentos (receitas - despesas)
            #region Total por mês
            var query = from item in lista
                        group item by new { item.DataVencimento.Year, item.DataVencimento.Month }
                        into grp
                        select new
                        {
                            ano = grp.Key.Year,
                            mes = grp.Key.Month,
                            valor = grp.Sum(x => x.Valor)
                        };
            #endregion

            //Cálculo do saldo parcial por mês
            List<DataPoint> dataPoints = new List<DataPoint>();
            double parcial = 0;
            int count = 0;
            foreach (var item in query)
            {
                DateTime pastDate = new DateTime(DateTime.Today.AddMonths(-6).Year, DateTime.Today.AddMonths(-6).Month, 01);
                DateTime itemDate = new DateTime(item.ano, item.mes, DateTime.DaysInMonth(item.ano, item.mes));
                //Será exibido somente os saldos dos últimos 6 meses até os próximos 6 meses
                if (itemDate.CompareTo(pastDate) >= 0 && itemDate.CompareTo(DateTime.Today.AddMonths(6)) <= 0)
                {
                    dataPoints.Add(new DataPoint(itemDate.ToString("MMM-yyyy").ToUpper(), item.valor + parcial));
                }
                parcial += item.valor;
                count++;
            }

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            float totalDespesas = despesas.Sum(x => x.Valor);
            var porTiposDespesas = from item in despesas
                                   join tipos in db.TipoDespesas on item.IdTipoDespesa equals tipos.Id
                                   group item by new { tipos.Nome }
                                   into grp
                                   select new
                                   {
                                       tipo = grp.Key.Nome,
                                       valor = grp.Sum(x => x.Valor)
                                   };
            float totalReceitas = receitas.Sum(x => x.Valor);
            var porTiposReceitas = from item in receitas
                                   group item by new { item.TipoReceita }
                                   into grp
                                   select new
                                   {
                                       tipo = grp.Key.TipoReceita,
                                       valor = grp.Sum(x => x.Valor)
                                   };
            dataPoints = new List<DataPoint>();
            foreach (var item in porTiposDespesas)
            {
                dataPoints.Add(new DataPoint(item.tipo, Math.Round(item.valor / totalDespesas * 100, 2)));
            }
            ViewBag.DataPointsDespesas = JsonConvert.SerializeObject(dataPoints);

            dataPoints = new List<DataPoint>();
            foreach (var item in porTiposReceitas)
            {
                dataPoints.Add(new DataPoint(item.tipo.ToString(), Math.Round(item.valor / totalReceitas * 100, 2)));
            }
            ViewBag.DataPointsReceitas = JsonConvert.SerializeObject(dataPoints);
            return View();
        }


    }
}
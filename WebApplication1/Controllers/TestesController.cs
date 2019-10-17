using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using WebApplication1.Models;
using WebApplication1.Models.Classes;
using OfxSharpLib;

namespace WebApplication1.Controllers
{
    public class TestesController : Controller
    {
        private WebApplication1Context db = new WebApplication1Context();
        // GET: Testes
        public ActionResult Index()
        {
            return View("Index");
        }

        [HttpPost]
        public ActionResult Upar(HttpPostedFileBase file)
        {
            var lista = new List<LancamentoOFX>();
            if (file != null && file.ContentLength > 0)
                try
                {
                    ViewBag.Message = "Arquivo carregado com sucesso.";
                    var filename = file.FileName;
                    var diretorio = Server.MapPath("~/");
                    var path = Path.Combine(diretorio, filename);
                    file.SaveAs(path);

                    var parser = new OfxDocumentParser();
                    var ofxDocument = parser.Import(new FileStream(path, FileMode.Open));

                    foreach (var item in ofxDocument.Transactions)
                    {
                        lista.Add(new LancamentoOFX(item.TransType.ToString(), item.Date, item.Name ?? item.Memo, Math.Abs((float)item.Amount) / 100));
                    }

                    //XDocument xmlDoc = new XDocument();
                    //xmlDoc = XDocument.Load(path);

                    //var lancamentos = from item in xmlDoc.Descendants("STMTTRN")
                    //                  select new
                    //                  {
                    //                      tipo = item.Element("TRNTYPE").Value,
                    //                      data = new DateTime(
                    //                              int.Parse(item.Element("DTPOSTED").Value.Substring(0, 4)),
                    //                              int.Parse(item.Element("DTPOSTED").Value.Substring(4, 2)),
                    //                              int.Parse(item.Element("DTPOSTED").Value.Substring(6, 2))),
                    //                      descricao = item.Element("NAME").Value,
                    //                      qnt = (float)Math.Round(Math.Abs(float.Parse(item.Element("TRNAMT").Value)), 2)
                    //                  };

                    //foreach (var item in lancamentos)
                    //{
                    //    lista.Add(new LancamentoOFX(item.tipo, item.data, item.descricao, item.qnt));
                    //}

                    //ViewBag.Lancamentos = lista;

                    return View("Index", lista);
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "Nenhum arquivo selecionado.";
            }
            return View("Index");
        }

        [HttpPost]
        public ActionResult Importar(List<LancamentoOFX> lista)
        {
            IList<Receita> receitas = new List<Receita>();
            IList<Despesa> despesas = new List<Despesa>();

            var lista2 = db.LancamentosOFX.ToList();

            if (db.LancamentosOFX.Count() > 0 && lista2.Contains(lista.First()))
            {
                return RedirectToAction("Confirm", new { importado = false });
            }

            foreach (var item in lista)
            {
                if (item.Tipo.Equals("CREDIT"))
                {
                    receitas.Add(new Receita()
                    {
                        TipoReceita = TipoReceita.Transferencia,
                        FormaRecebimento = FormaRecebimento.Cartao,
                        DataRecebimento = item.DataRealizacao,
                        PrimeiraDataVencimento = item.DataRealizacao,
                        Valor = item.Valor,
                        Parcelamento = Parcelamento.Unico,
                        NumeroParcelas = 1,
                        Descricao = item.Descricao,
                        OFX = true
                    });
                }
                else if (item.Tipo.Equals("DEBIT"))
                {
                    despesas.Add(new Despesa()
                    {
                        NomeDespesa = item.Descricao,
                        CaractDespesa = EnumCaracteristicaDespesa.FIXA,
                        IdTipoDespesa = db.TipoDespesas.First().Id,
                        TipoDespesa = db.TipoDespesas.First(),
                        Valor = item.Valor,
                        DataRealizacao = item.DataRealizacao,
                        Parcelamento = Parcelamento.Unico,
                        NumeroParcelas = 1,
                        VencimentoPrimeiraParcela = item.DataRealizacao,
                        Ofx = true
                    });
                }
            }
            db.Receitas.AddRange(receitas);
            db.Despesas.AddRange(despesas);
            db.LancamentosOFX.AddRange(lista);
            db.SaveChanges();
            return RedirectToAction("Confirm", new { importado = true });
        }

        public ActionResult Confirm(Boolean importado)
        {
            if (importado)
            {
                ViewBag.Message = "Lançamentos importados com sucesso. Por favor, visite as páginas de Receitas e Despesas para editar informações adicionais (opcional).";
            }
            else
            {
                ViewBag.Message = "OFX já importado. Por favor selecione outro.";
            }
            return View("Index");
        }
    }
}
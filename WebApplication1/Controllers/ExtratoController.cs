using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using System.Collections;
using WebApplication1.Models.Classes;
using OfficeOpenXml;
using System.Data.Entity;

namespace WebApplication1.Controllers
{
    public class ExtratoController : Controller
    {
        private String buscarIni, buscarFim, Tipo, banco;
        private WebApplication1Context db = new WebApplication1Context();
        // GET: Extrato
        public ActionResult Index(String buscarIni, String buscarFim, String Tipo, String banco)
        {
            if (buscarIni != null) { TempData["buscarIni"] = buscarIni; }
            if (buscarFim != null) { TempData["buscarFim"] = buscarFim; }
            if (Tipo != null) { TempData["Tipo"] = Tipo; }
            if (banco != null) { TempData["Banco"] = banco; }

            this.buscarIni = (string)TempData.Peek("buscarIni");
            this.buscarFim = (string)TempData.Peek("buscarFim");
            this.Tipo = (string)TempData.Peek("Tipo");
            this.banco = (string)TempData.Peek("Banco");

            ItemExtrato item;
            var lista = new List<ItemExtrato>();
            var despesas = db.Despesas.Include(desp => desp.conta).Include(conta => conta.conta.banco).ToList();
            foreach (var obj in despesas)
            {
                for (int i = 1; i <= obj.NumeroParcelas; i++)
                {
                    item = new ItemExtrato();
                    item.Valor = obj.Valor / obj.NumeroParcelas;
                    item.Tipo = 1;
                    item.Definicao = obj.CaractDespesa + "/" + obj.NomeDespesa;
                    item.DataRealizacao = obj.DataRealizacao;
                    item.DataVencimento = obj.VencimentoPrimeiraParcela.AddMonths(i - 1);
                    item.Pagamento = i + "/" + obj.NumeroParcelas;
                    item.NomeBanco = obj.conta.banco.Nome;

                    lista.Add(item);
                }
            }
            var receitas = db.Receitas.Include(desp => desp.conta).Include(conta => conta.conta.banco).ToList();
            foreach (var obj in receitas)
            {
                for (int i = 1; i <= obj.NumeroParcelas; i++)
                {
                    item = new ItemExtrato();
                    item.Valor = obj.Valor / obj.NumeroParcelas;
                    item.Tipo = 2;
                    item.DataRealizacao = obj.DataRecebimento;
                    item.DataVencimento = obj.PrimeiraDataVencimento.AddMonths(i - 1);
                    item.Definicao = obj.TipoReceita.ToString() + "/" + obj.Descricao;
                    item.Pagamento = i + "/" + obj.NumeroParcelas;
                    item.NomeBanco = obj.conta.banco.Nome;

                    lista.Add(item);
                }
            }

            lista.Sort();
            item = lista.First();
            float saldoParcial = 0;
            foreach (var obj in lista)
            {
                if (obj.Tipo == 1)
                {
                    saldoParcial -= obj.Valor;
                    obj.Saldo = saldoParcial;
                }
                else
                {
                    saldoParcial += obj.Valor;
                    obj.Saldo = saldoParcial;
                }
            }

            if (!String.IsNullOrEmpty(this.buscarIni) && !String.IsNullOrEmpty(this.buscarFim))
            {
                DateTime date1 = DateTime.Parse(this.buscarIni);
                DateTime date2 = DateTime.Parse(this.buscarFim);
                if (Tipo.Equals("credito"))
                {
                    lista = lista.Where(x => x.DataVencimento.CompareTo(date1) >= 0 && x.DataVencimento.CompareTo(date2) <= 0 && x.Tipo == 2).ToList();
                }
                else if (Tipo.Equals("debito"))
                {
                    lista = lista.Where(x => x.DataVencimento.CompareTo(date1) >= 0 && x.DataVencimento.CompareTo(date2) <= 0 && x.Tipo == 1).ToList();
                }
                else
                {
                    lista = lista.Where(x => x.DataVencimento.CompareTo(date1) >= 0 && x.DataVencimento.CompareTo(date2) <= 0).ToList();
                }
            }
            else if (!String.IsNullOrEmpty(this.Tipo) && this.Tipo.Equals("debito"))
            {
                lista = lista.Where(x => x.Tipo == 1 && x.DataVencimento.Month == DateTime.Today.Month).ToList();
            }
            else if (!String.IsNullOrEmpty(this.Tipo) && this.Tipo.Equals("credito"))
            {
                lista = (from obj in lista
                         where obj.Tipo == 2 && obj.DataVencimento.Month == DateTime.Today.Month
                         select obj).ToList();
                //ViewBag.Lista = lista.Where(x => x.Tipo == 2 && x.DataVencimento.Month == DateTime.Today.Month);
            }
            else
            {
                lista = (from obj in lista
                         where obj.DataVencimento.Month == DateTime.Today.Month
                         select obj).ToList();
                //ViewBag.Lista = lista.Where(x => x.DataVencimento.Month == DateTime.Today.Month);
            }
            if (!String.IsNullOrEmpty(this.banco))
            {
                lista = lista.Where(x => x.NomeBanco.ToUpper().Equals(this.banco.ToUpper())).ToList();
            }
            TempData["lista"] = lista;
            return View("Index", lista);

        }

        [HttpPost]
        public ActionResult GeneratePDF()
        {
            this.buscarIni = (string)TempData.Peek("buscarIni");
            this.buscarFim = (string)TempData.Peek("buscarFim");
            this.Tipo = (string)TempData.Peek("Tipo");
            this.banco = (string)TempData.Peek("Banco");
            return new Rotativa.ActionAsPdf("Index", new { buscarIni = this.buscarIni, buscarFim = this.buscarFim, Tipo = this.Tipo, banco = this.banco });
        }

        public void ExportExcel()
        {
            ExcelPackage excel = new ExcelPackage();
            ExcelWorksheet ws = excel.Workbook.Worksheets.Add("Relatório");

            ws.Cells["A1"].Value = "Valor";
            ws.Cells["B1"].Value = "Tipo/Nome";
            ws.Cells["C1"].Value = "Data de Realização";
            ws.Cells["D1"].Value = "Data de Vencimento";
            ws.Cells["E1"].Value = "Pagamento";
            ws.Cells["F1"].Value = "Saldo Parcial";
            ws.Cells["G1"].Value = "Banco";

            int linhaInicial = 2;
            var lista = (List<ItemExtrato>)TempData["lista"];

            foreach (var item in lista)
            {
                ws.Cells[string.Format("A{0}", linhaInicial)].Value = item.Valor;
                ws.Cells[string.Format("A{0}", linhaInicial)].Style.Numberformat.Format = "_-R$* #,##0.00_-;-R$* #,##0.00_-;_-R$* \"-\"??_-;_-@_-";
                if (item.Tipo == 1)
                {
                    ws.Cells[string.Format("B{0}", linhaInicial)].Value = "Débito";
                }
                else
                {
                    ws.Cells[string.Format("B{0}", linhaInicial)].Value = "Crédito";
                }

                ws.Cells[string.Format("C{0}", linhaInicial)].Value = item.DataRealizacao;
                ws.Cells[string.Format("D{0}", linhaInicial)].Value = item.DataVencimento;
                ws.Cells[string.Format("C{0}:D{0}", linhaInicial)].Style.Numberformat.Format = "dd/MM/yyyy";
                ws.Cells[string.Format("E{0}", linhaInicial)].Value = item.Pagamento;
                ws.Cells[string.Format("F{0}", linhaInicial)].Value = item.Saldo;
                ws.Cells[string.Format("F{0}", linhaInicial)].Style.Numberformat.Format = "_-R$* #,##0.00_-;-R$* #,##0.00_-;_-R$* \"-\"??_-;_-@_-";
                ws.Cells[string.Format("G{0}", linhaInicial)].Value = item.NomeBanco;

                linhaInicial++;
            }
            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "Relatório.xlsx");
            Response.BinaryWrite(excel.GetAsByteArray());
            Response.End();
        }
    }
}
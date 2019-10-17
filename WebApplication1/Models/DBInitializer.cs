using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Transactions;
using WebApplication1.Models.Classes;

namespace WebApplication1.Models
{
    public class DBInitializer : DropCreateDatabaseAlways<WebApplication1Context>
    {
        protected override void Seed(WebApplication1Context context)
        {
            TransactionOptions transactionOption = new TransactionOptions();
            transactionOption.IsolationLevel = IsolationLevel.Serializable;

            using (var scope = new TransactionScope(TransactionScopeOption.Required, transactionOption))
            {
                try
                {
                    #region Bancos
                    IList<Banco> bancos = new List<Banco>();

                    bancos.Add(new Banco() { Nome = "Banese", Numero = 47 });
                    bancos.Add(new Banco() { Nome = "Caixa", Numero = 22 });

                    context.Bancos.AddRange(bancos);
                    context.SaveChanges();
                    #endregion

                    #region TipoDespesas
                    IList<TipoDespesa> tipoDespesas = new List<TipoDespesa>();

                    tipoDespesas.Add(new TipoDespesa() { Nome = "Alimentação", Status = EnumStatus.ATIVO });
                    tipoDespesas.Add(new TipoDespesa() { Nome = "Educação", Status = EnumStatus.ATIVO });
                    tipoDespesas.Add(new TipoDespesa() { Nome = "Saúde", Status = EnumStatus.ATIVO });

                    context.TipoDespesas.AddRange(tipoDespesas);
                    context.SaveChanges();
                    #endregion

                    #region Clientes
                    IList<Cliente> clientes = new List<Cliente>();

                    clientes.Add(new Cliente() { Nome = "Alexandre", Email = "alexandre@gmail.com", DataAniversario = new DateTime(1990, 02, 18), ConfirmacaoEmail = "alexandre@gmail.com" });
                    clientes.Add(new Cliente() { Nome = "Breno", Email = "breno@gmail.com", DataAniversario = new DateTime(1990, 03, 18), ConfirmacaoEmail = "breno@gmail.com" });
                    clientes.Add(new Cliente() { Nome = "Carlos", Email = "carlos@gmail.com", DataAniversario = new DateTime(1990, 04, 18), ConfirmacaoEmail = "carlos@gmail.com" });

                    context.Clientes.AddRange(clientes);
                    context.SaveChanges();
                    #endregion

                    #region Contas
                    IList<Conta> contas = new List<Conta>();

                    contas.Add(new Conta() { IDBanco = bancos[0].ID, IDCliente = clientes[0].ID, Numero = 1234, SaldoInicial = 1000 });
                    context.Contas.AddRange(contas);
                    context.SaveChanges();
                    #endregion

                    #region Despesas
                    IList<Despesa> despesas = new List<Despesa>();

                    despesas.Add(new Despesa()
                    {
                        NomeDespesa = "Restaurante",
                        CaractDespesa = EnumCaracteristicaDespesa.FIXA,
                        IdTipoDespesa = tipoDespesas[0].Id,
                        TipoDespesa = tipoDespesas[0],
                        Valor = 45,
                        DataRealizacao = new DateTime(2019, 01, 01),
                        Parcelamento = Parcelamento.Parcelado,
                        NumeroParcelas = 3,
                        VencimentoPrimeiraParcela = new DateTime(2019, 01, 30),
                        Ofx = false,
                        IDConta = contas[0].ID

                    });
                    despesas.Add(new Despesa() { IDConta = contas[0].ID, NomeDespesa = "Médico", CaractDespesa = EnumCaracteristicaDespesa.FIXA, IdTipoDespesa = tipoDespesas[2].Id, TipoDespesa = tipoDespesas[2], Valor = 200, DataRealizacao = new DateTime(2019, 02, 01), Parcelamento = Parcelamento.Unico, NumeroParcelas = 1, VencimentoPrimeiraParcela = new DateTime(2019, 02, 01) });
                    despesas.Add(new Despesa() { IDConta = contas[0].ID, NomeDespesa = "Curso", CaractDespesa = EnumCaracteristicaDespesa.FIXA, IdTipoDespesa = tipoDespesas[1].Id, TipoDespesa = tipoDespesas[1], Valor = 900, DataRealizacao = new DateTime(2019, 01, 30), Parcelamento = Parcelamento.Parcelado, NumeroParcelas = 6, VencimentoPrimeiraParcela = new DateTime(2019, 02, 06) });

                    context.Despesas.AddRange(despesas);
                    context.SaveChanges();
                    #endregion

                    #region Receitas
                    IList<Receita> receitas = new List<Receita>();

                    receitas.Add(new Receita()
                    {
                        TipoReceita = TipoReceita.Transferencia,
                        DataRecebimento = new DateTime(2019, 01, 06),
                        PrimeiraDataVencimento = new DateTime(2019, 01, 31),
                        FormaRecebimento = FormaRecebimento.Dinheiro,
                        NumeroParcelas = 1,
                        Parcelamento = Parcelamento.Unico,
                        Valor = 1250,
                        Descricao = "Serviço Avulso",
                        OFX = false,
                        IDConta = contas[0].ID
                    });
                    receitas.Add(new Receita()
                    {
                        TipoReceita = TipoReceita.Indenizacao,
                        DataRecebimento = new DateTime(2019, 01, 30),
                        PrimeiraDataVencimento = new DateTime(2019, 02, 02),
                        FormaRecebimento = FormaRecebimento.Cartao,
                        NumeroParcelas = 3,
                        Parcelamento = Parcelamento.Parcelado,
                        Valor = 3000,
                        Descricao = "Recebimento Salário",
                        OFX = false,
                        IDConta = contas[0].ID
                    });
                    receitas.Add(new Receita()
                    {
                        TipoReceita = TipoReceita.Salario,
                        DataRecebimento = new DateTime(2018, 12, 30),
                        PrimeiraDataVencimento = new DateTime(2018, 12, 30),
                        FormaRecebimento = FormaRecebimento.Cartao,
                        NumeroParcelas = 1,
                        Parcelamento = Parcelamento.Unico,
                        Valor = 500,
                        Descricao = "Transferencia",
                        OFX = false,
                        IDConta = contas[0].ID
                    });

                    context.Receitas.AddRange(receitas);
                    context.SaveChanges();
                    #endregion

                    scope.Complete();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            base.Seed(context);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.Classes
{
    public class Conta
    {
        public int Numero { get; set; }
        public float SaldoInicial { get; set; }
        public float SaldoAtual { get; set; }

        public int ID { get; set; }

        public Cliente cliente { get; set; }
        public int IDCliente { get; set; }

        public Banco banco { get; set; }
        public int IDBanco { get; set; }

        /// <summary>
        /// Lançamentos são receitas e despesas
        /// </summary>
        public ICollection<Receita> Receitas { get; set; }
        public ICollection<Despesa> Despesas { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.Classes
{
    public class ItemExtrato : IComparable<ItemExtrato>
    {
        public int Tipo { get; set; }
        [DisplayFormat(DataFormatString = "{0:c2}", ApplyFormatInEditMode = true)]
        public float Valor { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataRealizacao { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataVencimento { get; set; }
        public String Definicao { get; set; }
        public String Pagamento { get; set; }
        [DisplayFormat(DataFormatString = "{0:c2}", ApplyFormatInEditMode = true)]
        public float Saldo { get; set; }
        public String NomeBanco { get; set; }

        public int CompareTo(ItemExtrato other)
        {
            return this.DataVencimento.CompareTo(other.DataVencimento);
        }
    }
}
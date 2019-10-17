using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.Classes
{
    public class Parcela
    {
        public float Valor { get; set; }
        public DateTime DataVencimento { get; set; }
        public int NumeroParcela { get; set; }
    }
}
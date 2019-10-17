using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.Classes
{
    public class LancamentoOFX
    {
        public int ID { get; set; }
        public String Tipo { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataRealizacao { get; set; }
        public String Descricao { get; set; }
        [DisplayFormat(DataFormatString = "{0:c2}", ApplyFormatInEditMode = true)]
        public float Valor { get; set; }

        public LancamentoOFX()
        {

        }

        public LancamentoOFX(String Tipo, DateTime Data, String Descricao, float Valor)
        {
            this.Tipo = Tipo;
            this.DataRealizacao = Data;
            this.Descricao = Descricao;
            this.Valor = Valor;
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is LancamentoOFX)
            {
                LancamentoOFX objj = (LancamentoOFX)obj;
                if (this.Tipo.Equals(objj.Tipo) && this.Valor == objj.Valor && this.DataRealizacao.Equals(objj.DataRealizacao) && this.Descricao.Equals(objj.Descricao))
                {
                    return true;
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


    }
}
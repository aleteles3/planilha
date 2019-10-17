using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApplication1.Models.Validator;

namespace WebApplication1.Models.Classes
{
    public class Despesa : IValidatableObject
    {
        public int Id { get; set; }
        [Display(Name = "Nome da Despesa")]
        public String NomeDespesa { get; set; }
        [DisplayFormat(DataFormatString = "{0:c2}", ApplyFormatInEditMode = true)]
        public float Valor { get; set; }
        [Display(Name = "Tipo da Despesa")]
        public TipoDespesa TipoDespesa { get; set; }
        public int IdTipoDespesa { get; set; }
        public EnumCaracteristicaDespesa CaractDespesa { get; set; }
        [Display(Name = "Data de Realização")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataRealizacao { get; set; }

        public Parcelamento Parcelamento { get; set; }
        public int NumeroParcelas { get; set; }
        [Display(Name = "Primeiro Vencimento")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime VencimentoPrimeiraParcela { get; set; }

        public Boolean Ofx { get; set; }

        public Conta conta { get; set; }
        public int IDConta { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new DespesaValidator();
            var result = validator.Validate(this);
            return result.Errors.Select(erro => new ValidationResult(erro.ErrorMessage, new[] { erro.PropertyName }));
        }
    }
}
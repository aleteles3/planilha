using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApplication1.Models.Validator;

namespace WebApplication1.Models.Classes
{
    public class TipoDespesa : IValidatableObject
    {
        public int Id { get; set; }
        public String Nome { get; set; }
        public EnumStatus Status { get; set; }
        public ICollection<Despesa> Despesas { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            var validator = new TipoDespesaValidator(this.Id);
            var result = validator.Validate(this);
            return result.Errors
                .Select(erro =>
                new ValidationResult(erro.ErrorMessage, new[] { erro.PropertyName }));
        }
    }
}
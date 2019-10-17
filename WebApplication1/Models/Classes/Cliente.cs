using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApplication1.Models.Validator;

namespace WebApplication1.Models.Classes
{
    public class Cliente : IValidatableObject
    {
        public int ID { get; set; }
        [Display(Name = "Nome Completo")]
        public String Nome { get; set; }
        [Display(Name = "Data de Aniversário")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataAniversario { get; set; }
        [Display(Name = "E-mail")]
        public String Email { get; set; }
        [Display(Name = "Confirmação do E-mail")]
        public String ConfirmacaoEmail { get; set; }

        public ICollection<Conta> Contas { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new ClienteValidator();
            var result = validator.Validate(this);
            return result.Errors.Select(erro => new ValidationResult(erro.ErrorMessage, new[] { erro.PropertyName }));
        }
    }
}
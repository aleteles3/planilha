using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using FluentValidation;
using WebApplication1.Models.Classes;

namespace WebApplication1.Models.Validator
{
    public class DespesaValidator : AbstractValidator<Despesa>
    {

        public DespesaValidator()
        {
            RuleFor(depesa => depesa.NomeDespesa).NotEmpty().WithMessage("Nome da Despesa obrigatório!");
            RuleFor(despesa => despesa.Valor).GreaterThan(0).WithMessage("Despesa precisa ser maior que zero!");
            RuleFor(despesa => despesa.DataRealizacao).Must(ValidarData).WithMessage("Data inválida!.");
            RuleFor(despesa => despesa.VencimentoPrimeiraParcela).Must(ValidarData).WithMessage("Data inválida!.");
        }

        private bool ValidarData(DateTime date)
        {
            string expressao = @"^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$";

            Regex rg = new Regex(expressao);
            return rg.IsMatch(date.ToString("dd/MM/yyyy"));
        }
    }
}
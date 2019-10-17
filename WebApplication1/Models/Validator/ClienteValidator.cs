using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using FluentValidation;
using WebApplication1.Models.Classes;

namespace WebApplication1.Models.Validator
{
    public class ClienteValidator : AbstractValidator<Cliente>
    {
        WebApplication1Context db = null;
        public ClienteValidator()
        {
            this.db = new WebApplication1Context();

            RuleFor(cliente => cliente.Email).EmailAddress().WithMessage("E-mail inválido!");
            RuleFor(cliente => cliente.Email).Equal(cliente => cliente.ConfirmacaoEmail).WithMessage("Os E-mails precisam ser iguais!");
            RuleFor(cliente => cliente.DataAniversario).Must(ValidarData).WithMessage("Data inválida!.");
            RuleFor(cliente => cliente.Email).Must((tipo, nome) => { return UniqueEmail(tipo.ID, tipo.Email); }).WithMessage("E-mail já cadastrado.");
        }

        private bool ValidarData(DateTime date)
        {
            string expressao = @"^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$";

            Regex rg = new Regex(expressao);
            return rg.IsMatch(date.ToString("dd/MM/yyyy"));
        }

        private bool UniqueEmail(int id, String nome)
        {

            int count = 0;
            string email = String.Empty;
            if (id == 0)
            {
                // Se ID = 0, inserção
                count = this.db.Clientes
                                  .Where(x => x.Email.Equals(nome)).Count();

                return count == 0;
            }
            else if (!String.IsNullOrEmpty(nome))
            {
                // Atualização....
                var result = this.db.Clientes
                                 .Where(x => x.ID != id && x.Email.Equals(nome))
                                 .Select(x => new { nome = x.Email });

                count = result.Count();
                // Se resultado = NULL, trocou para um nome que não existe => OK
                // Se resultado igual a algum nome, há um TipoDespesa já cadastrado
                email = result.FirstOrDefault() == null ? String.Empty : result.FirstOrDefault().nome;

                return count == 0 || (count == 1 && !email.Equals(nome));
            }
            else
            {
                // deletando... não precisa validar
                return true;
            }
        }
    }
}
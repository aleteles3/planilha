using System.Collections.Generic;

namespace WebApplication1.Models.Classes
{
    public class Banco
    {
        public int Numero { get; set; }
        public string Nome { get; set; }
        public int ID { get; set; }

        /// <summary>
        /// Um banco possui várias contas-corrente
        /// </summary>
        public ICollection<Conta> Contas { get; set; }
    }
}
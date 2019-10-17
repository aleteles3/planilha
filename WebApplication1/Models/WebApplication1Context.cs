using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApplication1.Models.Classes;
using WebApplication1.Models.FluentAPI;

namespace WebApplication1.Models
{
    public class WebApplication1Context : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        public WebApplication1Context() : base("name=WebApplication1Context")
        {
        }
        public System.Data.Entity.DbSet<WebApplication1.Models.Classes.Cliente> Clientes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ClienteMap());
            modelBuilder.Configurations.Add(new TipoDespesaMap());
            modelBuilder.Configurations.Add(new DespesaMap());
            modelBuilder.Configurations.Add(new ReceitaMap());
            modelBuilder.Configurations.Add(new LancamentoOFXMap());
            modelBuilder.Configurations.Add(new ContaMap());
            modelBuilder.Configurations.Add(new BancoMap());
            base.OnModelCreating(modelBuilder);

        }

        //object placeHolderVariable;
        public System.Data.Entity.DbSet<WebApplication1.Models.Classes.TipoDespesa> TipoDespesas { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.Classes.Despesa> Despesas { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.Classes.Receita> Receitas { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.Classes.LancamentoOFX> LancamentosOFX { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.Classes.Conta> Contas { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.Classes.Banco> Bancos { get; set; }
    }
}

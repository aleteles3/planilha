using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Models.Classes;

namespace WebApplication1.Models.FluentAPI
{
    public class ContaMap : EntityTypeConfiguration<Conta>
    {
        public ContaMap()
        {
            ToTable("TB_CONTA");
            HasKey(conta => conta.ID)
             .Property(conta => conta.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasRequired<Cliente>(conta => conta.cliente)
                .WithMany(cliente => cliente.Contas)
                .HasForeignKey<int>(conta => conta.IDCliente);

            HasRequired<Banco>(conta => conta.banco)
                .WithMany(banco => banco.Contas)
                .HasForeignKey<int>(conta => conta.IDBanco);
        }
    }
}
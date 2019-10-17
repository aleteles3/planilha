using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Models.Classes;

namespace WebApplication1.Models.FluentAPI
{
    public class DespesaMap : EntityTypeConfiguration<Despesa>
    {
        public DespesaMap()
        {
            ToTable("TB_DESPESA");


            HasKey(despesa => despesa.Id)
             .Property(despesa => despesa.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasRequired<TipoDespesa>(desp => desp.TipoDespesa)
                .WithMany(tipo => tipo.Despesas)
                .HasForeignKey<int>(desp => desp.IdTipoDespesa);

            HasRequired<Conta>(despesa => despesa.conta)
                .WithMany(conta => conta.Despesas)
                .HasForeignKey<int>(despesa => despesa.IDConta);

            Property(despesa => despesa.NomeDespesa).HasMaxLength(255).IsRequired().HasColumnType("Varchar");
            Property(despesa => despesa.CaractDespesa).IsRequired().HasColumnType("int");
            Property(despesa => despesa.Valor).IsRequired().HasColumnType("float");
            Property(despesa => despesa.DataRealizacao).IsRequired().HasColumnType("Datetime2");
            Property(despesa => despesa.VencimentoPrimeiraParcela).IsRequired().HasColumnType("Datetime2");


        }
    }
}
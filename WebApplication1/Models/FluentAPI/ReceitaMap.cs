using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using WebApplication1.Models.Classes;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models.FluentAPI
{
    public class ReceitaMap : EntityTypeConfiguration<Receita>
    {
        public ReceitaMap()
        {
            ToTable("TB_RECEITA");
            HasKey(receita => receita.ID).
                Property(receita => receita.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasRequired<Conta>(receita => receita.conta)
                .WithMany(conta => conta.Receitas)
                .HasForeignKey<int>(receita => receita.IDConta);

            Property(receita => receita.Descricao).HasMaxLength(255).IsOptional().HasColumnType("Varchar");
            Property(receita => receita.Observacao).HasMaxLength(255).IsOptional().HasColumnType("Varchar");
            Property(receita => receita.DataRecebimento).IsRequired().HasColumnType("Datetime2");
            Property(receita => receita.TipoReceita).IsRequired().HasColumnType("int");
            Property(receita => receita.FormaRecebimento).IsRequired().HasColumnType("int");
            Property(receita => receita.Valor).IsRequired().HasColumnType("Float");
            Property(receita => receita.NumeroParcelas).IsRequired().HasColumnType("int");
            Property(receita => receita.PrimeiraDataVencimento).IsRequired().HasColumnType("Datetime2");
        }
    }
}
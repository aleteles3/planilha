using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using WebApplication1.Models.Classes;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models.FluentAPI
{
    public class TipoDespesaMap : EntityTypeConfiguration<TipoDespesa>
    {
        public TipoDespesaMap()
        {
            ToTable("TB_TIPO_DESPESA");
            HasKey(categoria => categoria.Id).
                Property(categoria => categoria.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(categoria => categoria.Nome).HasMaxLength(255).IsRequired().HasColumnType("Varchar");
            Property(categoria => categoria.Status).IsRequired();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using WebApplication1.Models.Classes;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models.FluentAPI
{
    public class LancamentoOFXMap : EntityTypeConfiguration<LancamentoOFX>
    {
        public LancamentoOFXMap()
        {
            ToTable("TB_LANCAMENTOOFX");
            HasKey(lancamento => lancamento.ID).
                Property(cliente => cliente.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
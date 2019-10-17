using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using WebApplication1.Models.Classes;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models.FluentAPI
{
    public class BancoMap : EntityTypeConfiguration<Banco>
    {
        public BancoMap()
        {
            ToTable("TB_BANCO");
            HasKey(banco => banco.ID).
                    Property(banco => banco.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
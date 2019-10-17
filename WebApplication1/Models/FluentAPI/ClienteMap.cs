using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using WebApplication1.Models.Classes;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models.FluentAPI
{
    public class ClienteMap : EntityTypeConfiguration<Cliente>
    {
        public ClienteMap()
        {
            ToTable("TB_CLIENTE");
            HasKey(cliente => cliente.ID).
                Property(cliente => cliente.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(cliente => cliente.Nome).HasMaxLength(255).IsRequired().HasColumnType("Varchar");
            Property(cliente => cliente.Email).HasMaxLength(255).IsRequired().HasColumnType("Varchar");
            Property(cliente => cliente.DataAniversario).IsRequired().HasColumnType("Datetime2");
            //Property(cliente => cliente.ConfirmacaoEmail).IsRequired();
            Ignore(cliente => cliente.ConfirmacaoEmail);

        }
    }
}
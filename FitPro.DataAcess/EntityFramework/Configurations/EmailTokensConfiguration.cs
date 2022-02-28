using FitPro.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitPro.DataAcess
{
    public class EmailTokensConfiguration : IEntityTypeConfiguration<EmailTokens>
    {
        public void Configure(EntityTypeBuilder<EmailTokens> entity)
        {
            entity.HasKey(e => e.Email)
                   .HasName("PK__EmailTok__A9D1053545231B18");

            entity.ToTable("EmailTokens");

            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.Date).HasColumnType("datetime");
        }
    }
}

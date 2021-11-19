using FitPro.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitPro.DataAccess
{
    public class RequestConfiguration : IEntityTypeConfiguration<Request>
    {
        public void Configure(EntityTypeBuilder<Request> entity)
        {
            entity.HasKey(e => new { e.IdFromUser, e.IdToUser })
                   .HasName("PK__Request__5383DAF15C2BCD6D");

            entity.ToTable("Request");


            entity.Property(e => e.Date)
                .IsRequired()
                .HasColumnType("date");

            entity.HasOne(d => d.IdFromUserNavigation)
                .WithMany(p => p.RequestIdFromUserNavigations)
                .HasForeignKey(d => d.IdFromUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RequestFromRegularUser");

            entity.HasOne(d => d.IdToUserNavigation)
                .WithMany(p => p.RequestIdToUserNavigations)
                .HasForeignKey(d => d.IdToUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RequestToRegularUser");
        }
    }
}
using FitPro.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitPro.DataAccess
{
    public class RecommandationConfiguration : IEntityTypeConfiguration<Recommandation>
    {
        public void Configure(EntityTypeBuilder<Recommandation> entity)
        {
            entity.HasKey(e => e.IdRecommandation)
                    .HasName("PK__Recomman__11E1EB347D57AB37");

            entity.ToTable("Recommandation");

            entity.HasIndex(e => new { e.IdFromUser, e.IdToUser, e.IdWorkout, e.IdRecipe, e.SendDate }, "UQ__Recomman__5531A2B0B49214A8")
                .IsUnique();

            entity.Property(e => e.IdRecommandation).ValueGeneratedNever();

            entity.Property(e => e.SendDate).HasColumnType("date");

            entity.Property(e => e.Comment).HasMaxLength(500);

            entity.HasOne(d => d.IdFromUserNavigation)
                .WithMany(p => p.RecommandationIdFromUserNavigations)
                .HasForeignKey(d => d.IdFromUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RecommandationFromRegularUser");

            entity.HasOne(d => d.IdRecipeNavigation)
                .WithMany(p => p.Recommandations)
                .HasForeignKey(d => d.IdRecipe)
                .HasConstraintName("FK_RecommandationRecipe");

            entity.HasOne(d => d.IdToUserNavigation)
                .WithMany(p => p.RecommandationIdToUserNavigations)
                .HasForeignKey(d => d.IdToUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RecommandationToRegularUser");

            entity.HasOne(d => d.IdWorkoutNavigation)
                .WithMany(p => p.Recommandations)
                .HasForeignKey(d => d.IdWorkout)
                .HasConstraintName("FK_RecommandationWorkout");
        }
    }
}
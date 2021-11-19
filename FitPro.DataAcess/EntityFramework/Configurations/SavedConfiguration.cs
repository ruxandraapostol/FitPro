using FitPro.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitPro.DataAcess
{
    public class SavedConfiguration : IEntityTypeConfiguration<Saved>
    {
        public void Configure(EntityTypeBuilder<Saved> entity)
        {
            entity.HasKey(e => e.IdSaved)
                    .HasName("PK__Saved__720EAA8ACF1A8E02");

            entity.ToTable("Saved");

            entity.HasIndex(e => new { e.IdRegularUser, e.IdWorkout, e.IdRecipe }, "UQ__Saved__9432E4764A13FCAF")
                .IsUnique();

            entity.Property(e => e.IdSaved).ValueGeneratedNever();

            entity.Property(e => e.Date).HasColumnType("date");

            entity.HasOne(d => d.IdRecipeNavigation)
                .WithMany(p => p.Saveds)
                .HasForeignKey(d => d.IdRecipe)
                .HasConstraintName("FK_Saved_Recipe");

            entity.HasOne(d => d.IdRegularUserNavigation)
                .WithMany(p => p.Saveds)
                .HasForeignKey(d => d.IdRegularUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Saved_RegularUser");

            entity.HasOne(d => d.IdWorkoutNavigation)
                .WithMany(p => p.Saveds)
                .HasForeignKey(d => d.IdWorkout)
                .HasConstraintName("FK_Saved_Workout");
        }
    }
}

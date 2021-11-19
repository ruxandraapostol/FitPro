using FitPro.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitPro.DataAccess
{
    public class WorkoutConfiguration : IEntityTypeConfiguration<Workout>
    {
        public void Configure(EntityTypeBuilder<Workout> entity)
        {
            entity.HasKey(e => e.IdWorkout)
                   .HasName("PK__Workout__BF705A0C968C821A");

            entity.HasIndex(e => e.LinkUrl).IsUnique();
                

            entity.ToTable("Workout");

            entity.Property(e => e.IdWorkout).ValueGeneratedNever();

            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(e => e.LinkUrl)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);


            entity.HasOne(d => d.IdTrainerNavigation)
                .WithMany(p => p.Workouts)
                .HasForeignKey(d => d.IdTrainer)
                .HasConstraintName("FK_WorkoutTrainer");

            entity.HasOne(d => d.IdLastModifiedNavigation)
                .WithMany(p => p.WorkoutsLastModified)
                .HasForeignKey(d => d.LastModified)
                .HasConstraintName("FK_WorkoutLastModifiedTrainer");
        }
    }
}

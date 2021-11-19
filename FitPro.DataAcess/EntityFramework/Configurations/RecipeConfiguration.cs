using FitPro.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitPro.DataAccess
{
    public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
    {
        public void Configure(EntityTypeBuilder<Recipe> entity)
        {
            entity.HasKey(e => e.IdRecipe)
                    .HasName("PK__Recipe__2FEC16D4E01A7864");


            entity.HasIndex(e => e. Name, "UQ__Recipe__A9D1053492FF36C1")
                .IsUnique();


            entity.ToTable("Recipe");

            entity.Property(e => e.IdRecipe).ValueGeneratedNever();

            entity.Property(e => e.Preparation).IsRequired();

            entity.Property(e => e.AlimentsList).IsRequired();

            entity.Property(e => e.Time).IsRequired();

            entity.Property(e => e.Calories).IsRequired();

            entity.Property(e => e.Name).IsRequired();

            entity.HasOne(d => d.IdCategoryNavigation)
                .WithMany(p => p.Recipes)
                .HasForeignKey(d => d.IdCategory)
                .HasConstraintName("FK_RecipeCategory");

            entity.HasOne(d => d.IdNutritionistNavigation)
                .WithMany(p => p.Recipes)
                .HasForeignKey(d => d.IdNutritionist)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RecipeNutritionist");
        }
    }
}
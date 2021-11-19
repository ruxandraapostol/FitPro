using FitPro.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitPro.DataAccess
{
    public class FriendsListConfiguration : IEntityTypeConfiguration<FriendsList>
    {
        public void Configure(EntityTypeBuilder<FriendsList> entity)
        {
            entity.HasKey(e => new { e.IdUser1, e.IdUser2 })
                     .HasName("PK__FriendsL__5BF9E10AEE5FAD20");

            entity.ToTable("FriendsList");

            entity.HasOne(d => d.IdUser1Navigation)
                .WithMany(p => p.FriendsListIdUser1Navigations)
                .HasForeignKey(d => d.IdUser1)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FriendsListUser1");

            entity.HasOne(d => d.IdUser2Navigation)
                .WithMany(p => p.FriendsListIdUser2Navigations)
                .HasForeignKey(d => d.IdUser2)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FriendsListUser2");
        }
    }
}
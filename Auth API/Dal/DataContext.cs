using Auth_API.Models.Dto.User;
using Microsoft.EntityFrameworkCore;

namespace Auth_API.Dal
{
    public class DataContext : DbContext
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public virtual DbSet<UserDto> User { get; set; }
        public virtual DbSet<UserTokensDto> RefreshToken { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDto>(entity =>
            {
                entity.HasKey(user => user.Uuid);
                entity.HasOne(e => e.SpotifyAccountData)
                    .WithOne()
                    .HasForeignKey<UserTokensDto>(e => e.UserUuid);
            });
            modelBuilder.Entity<UserTokensDto>(entity =>
            {
                entity.HasKey(rt => rt.Uuid);
            });

            modelBuilder.Entity<UserTokensDto>(entity =>
            {
                entity.HasKey(s => s.Uuid);
            });
        }
    }
}

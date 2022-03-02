using Auth_API.Models.Dto.Spotify;
using Auth_API.Models.Dto.User;
using Microsoft.EntityFrameworkCore;

namespace Auth_API.Dal
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public virtual DbSet<UserDto> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDto>(entity =>
            {
                entity.HasKey(user => user.Uuid);
                entity.HasOne(e => e.SpotifyAccountData)
                    .WithOne()
                    .HasForeignKey<SpotifyAccountDataDto>(e => e.UserUuid);
            });

            modelBuilder.Entity<RefreshTokenDto>(entity =>
            {
                entity.HasKey(rt => rt.Uuid);
            });
        }
    }
}

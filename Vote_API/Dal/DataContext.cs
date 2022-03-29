using Microsoft.EntityFrameworkCore;
using Vote_API.Models.Dto;

namespace Vote_API.Dal
{
    public class DataContext : DbContext
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public virtual DbSet<VoteDataDto> VoteData { get; set; }
        public virtual DbSet<PlaylistVoteDto> PlaylistVote { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VoteDataDto>(entity =>
            {
                entity.HasKey(e => e.Uuid);
                entity.HasMany(e => e.VoteablePlaylistCollection)
                    .WithOne()
                    .HasForeignKey(e => e.VoteDataUuid);
            });
            modelBuilder.Entity<VoteablePlaylistDto>(entity =>
            {
                entity.HasKey(e => e.Uuid);
                entity.HasMany(e => e.SongsInPlaylist)
                    .WithOne()
                    .HasForeignKey(e => e.PlaylistUuid);
                entity.HasMany(e => e.Votes)
                    .WithOne()
                    .HasForeignKey(e => e.VoteDataUuid);
            });
            modelBuilder.Entity<SpotifyPlaylistSongDto>(entity =>
            {
                entity.HasKey(e => e.Uuid);
            });
            modelBuilder.Entity<PlaylistVoteDto>(entity =>
            {
                entity.HasKey(e => e.Uuid);
            });
        }
    }
}

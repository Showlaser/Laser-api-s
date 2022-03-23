namespace Vote_API.Models.FromFrontend
{
    public class VoteData
    {
        public Guid Uuid { get; set; }
        public Guid AuthorUserUuid { get; set; }
        public DateTime ValidUntil { get; set; }
        public List<VoteablePlaylist>? VoteablePlaylistCollection { get; set; }
    }
}

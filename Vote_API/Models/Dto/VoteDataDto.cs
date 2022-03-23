namespace Vote_API.Models.Dto
{
    public class VoteDataDto
    {
        public Guid Uuid { get; set; }
        public Guid AuthorUserUuid { get; set; }
        public DateTime ValidUntil { get; set; }
        public string JoinCode { get; set; }
        public string? Password { get; set; }
        public byte[]? Salt { get; set; }
        public List<VoteablePlaylistDto>? VoteablePlaylistCollection { get; set; }
    }
}

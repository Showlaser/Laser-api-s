namespace Vote_API.Models.ToFrontend
{
    public class VoteDataViewmodel
    {
        public Guid Uuid { get; set; }
        public DateTime ValidUntil { get; set; }
        public List<VoteablePlaylistViewmodel>? VoteablePlaylistCollection { get; set; }
    }
}

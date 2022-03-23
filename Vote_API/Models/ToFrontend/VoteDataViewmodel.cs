using Vote_API.Models.FromFrontend;

namespace Vote_API.Models.ToFrontend
{
    public class VoteDataViewmodel
    {
        public Guid Uuid { get; set; }
        public DateTime ValidUntil { get; set; }
        public List<VoteablePlaylist>? VoteablePlaylistCollection { get; set; }
    }
}

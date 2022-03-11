namespace Vote_API.Models.ToFrontend
{
    public class PlaylistVoteViewmodel
    {
        public Guid Uuid { get; set; }
        public Guid VoteDataUuid { get; set; }
        public Guid PlaylistUuid { get; set; }
    }
}

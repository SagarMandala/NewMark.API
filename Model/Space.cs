namespace NewMark.API.Model
{
    public class Space
    {
        public string? SpaceId { get; set; }
        public string? SpaceName { get; set; }
        public List<RentRoll>? RentRoll { get; set; }
    }
}

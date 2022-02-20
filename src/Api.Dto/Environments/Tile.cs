namespace Paladin.Api.Dto.Environments
{
    public class Tile
    {
        public TileType Type { get; set; }
        public Player Player { get; set; }
        public Enemy Enemy { get; set; }
        public Trap Trap { get; set; }
    }
}

namespace Paladin.CotNd;

public record Tile(TileType Type)
{
    public Player? Player { get; set; }
    public Enemy? Enemy { get; set; }
    public Trap? Trap { get; set; }
}

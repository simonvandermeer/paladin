using Paladin.CotNd;

namespace Paladin.CotNdSim;

public class MapGenerator
{
    public Map GenerateSimpleMap()
    {
        return new Map
        {
            Width = 5,
            Height = 5,
            Tiles = new MapTilesBuilder()
                .StartNewRow()
                .AddTile(new Tile(TileType.UnbreakableWall))
                .AddTile(new Tile(TileType.UnbreakableWall))
                .AddTile(new Tile(TileType.UnbreakableWall))
                .AddTile(new Tile(TileType.UnbreakableWall))
                .AddTile(new Tile(TileType.UnbreakableWall))
                .StartNewRow()
                .AddTile(new Tile(TileType.UnbreakableWall))
                .AddTile(new Tile(TileType.Floor))
                .AddTile(new Tile(TileType.Floor))
                .AddTile(new Tile(TileType.Floor))
                .AddTile(new Tile(TileType.UnbreakableWall))
                .StartNewRow()
                .AddTile(new Tile(TileType.UnbreakableWall))
                .AddTile(new Tile(TileType.Floor))
                .AddTile(new Tile(TileType.Floor))
                .AddTile(new Tile(TileType.Floor))
                .AddTile(new Tile(TileType.UnbreakableWall))
                .StartNewRow()
                .AddTile(new Tile(TileType.UnbreakableWall))
                .AddTile(new Tile(TileType.Floor))
                .AddTile(new Tile(TileType.Floor))
                .AddTile(new Tile(TileType.Floor))
                .AddTile(new Tile(TileType.UnbreakableWall))
                .StartNewRow()
                .AddTile(new Tile(TileType.UnbreakableWall))
                .AddTile(new Tile(TileType.UnbreakableWall))
                .AddTile(new Tile(TileType.UnbreakableWall))
                .AddTile(new Tile(TileType.UnbreakableWall))
                .AddTile(new Tile(TileType.UnbreakableWall))
                .Build()
        };
    }
}

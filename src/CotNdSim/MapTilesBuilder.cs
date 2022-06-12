using Paladin.CotNd;

namespace Paladin.CotNdSim;

public class MapTilesBuilder
{
    private List<List<Tile>> _tiles;
    private int _rowIndex = -1;

    public MapTilesBuilder()
    {
        _tiles = new List<List<Tile>>();
    }

    public MapTilesBuilder StartNewRow()
    {
        _tiles.Add(new List<Tile>());
        _rowIndex++;

        return this;
    }

    public MapTilesBuilder AddTile(Tile tile)
    {
        _tiles[_rowIndex].Add(tile);
        return this;
    }

    public IEnumerable<IEnumerable<Tile>> Build()
    {
        return _tiles;
    }
}

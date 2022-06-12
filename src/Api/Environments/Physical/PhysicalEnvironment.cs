using Paladin.Api.Dto.Environments;
using Paladin.CotNd;
using Paladin.Interop;

namespace Paladin.Api.Environments.Physical;

public class PhysicalEnvironment : IEnvironment
{
    private readonly GameProxy _gameProxy;

    internal static PhysicalEnvironment Create()
    {
        return new PhysicalEnvironment(GameProxy.Attach());
    }

    private PhysicalEnvironment(GameProxy gameProxy)
    {
        _gameProxy = gameProxy;
    }

    public EnvironmentId Id => EnvironmentId.Physical;
    public bool Simulated => false;
    public EnvironmentState GetState()
    {
        Map? map;
        try
        {
            map = GetMap();
        }
        catch (Exception)
        {
            // TODO: Log exception.
            // Vulnerable to exceptions in development phase.
            // Return null if the map is not yet ready to load.
            map = null;
        }

        return new EnvironmentState(map);
    }

    public Map GetMap()
    {
        var tiles = _gameProxy.GetTiles().ToList();

        // Get min max x and y
        var minX = 0;
        var minY = 0;
        var maxX = 0;
        var maxY = 0;
        foreach (var tile in tiles)
        {
            if (tile._x < minX)
            {
                minX = tile._x;
            }
            if (tile._x > maxX)
            {
                maxX = tile._x;
            }
            if (tile._y < minY)
            {
                minY = tile._y;
            }
            if (tile._y > maxY)
            {
                maxY = tile._y;
            }
        }

        (int x, int y) GetZeroBasedCoordinates(int x, int y)
        {
            return (x - minX, y - minY);
        }

        (var mapWidth, var mapHeight) = GetZeroBasedCoordinates(maxX + 1, maxY + 1);

        // Initialize map.
        var mapTiles = new Tile[mapHeight][];
        for (var i = 0; i < mapHeight; ++i)
        {
            mapTiles[i] = new Tile[mapWidth];
        }

        // Fill map.
        foreach (var tile in tiles)
        {
            (var x, var y) = GetZeroBasedCoordinates(tile._x, tile._y);
            mapTiles[y][x] = tile.ToDto(_gameProxy);
        }

        // Add player.
        var player = _gameProxy.GetPlayer();
        (var playerX, var playerY) = GetZeroBasedCoordinates(player._x, player._y);
        mapTiles[playerY][playerX].Player = player.ToDto();

        var map = new Map
        {
            Width = mapWidth,
            Height = mapHeight,
            Tiles = mapTiles
        };

        return map;
    }

    public bool IsUp => !_gameProxy.HasProcessExited;

    public void RunAction(EnvironmentAction action)
    {
        switch (action.Type)
        {
            case ActionType.MoveUp: _gameProxy.Move(0, -1); break;
            case ActionType.MoveDown: _gameProxy.Move(0, 1); break;
            case ActionType.MoveLeft: _gameProxy.Move(-1, 0); break;
            case ActionType.MoveRight: _gameProxy.Move(1, 0); break;
        }
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

    public void Close()
    {
        throw new NotImplementedException();
    }
}

namespace Paladin.CotNdSimulated;

public class Game
{
    public Game(int playerCount)
    {
        if (playerCount <= 0
            || playerCount > 2)
        {
            throw new ArgumentException("Player count must be 1 or 2.", nameof(playerCount));
        }

        PlayerCount = playerCount;
        State = GameState.MainMenu;
    }

    public int PlayerCount { get; }
    public GameState State { get; private set; }

    public Map CurrentMap { get; private set; }
}

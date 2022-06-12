namespace Paladin.CotNd;

public record Enemy(EnemyType Type)
{
    public int Health { get; set; }
}

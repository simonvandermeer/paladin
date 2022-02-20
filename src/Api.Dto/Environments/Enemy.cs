namespace Paladin.Api.Dto.Environments;

public record Enemy(EnemyType Type)
{
    public int Health { get; set; }
}

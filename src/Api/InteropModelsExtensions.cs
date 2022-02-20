using Paladin.Interop;
using Paladin.Interop.Models.Generated;

namespace Paladin.Api
{
    public static class InteropModelsExtensions
    {
        public static Dto.Environments.Tile ToDto(this Tile tile, GameProxy gameProxy)
        {
            var sprite = gameProxy.GetTileSprite(tile);
            var imagePath = gameProxy.GetSpriteImagePath(sprite);

            var tileType = imagePath switch
            {
                "level/floor_dirt1.png" => Dto.Environments.TileType.Floor,
                "level/floor_dirt2.png" => Dto.Environments.TileType.Floor,
                "level/TEMP_npc_floor.png" => Dto.Environments.TileType.Floor,
                "level/stairs.png" => Dto.Environments.TileType.Exit,
                "level/stairs_locked.png" => Dto.Environments.TileType.Exit,
                "level/stairs_locked_miniboss.png" => Dto.Environments.TileType.Exit,
                "level/wall_dirt_crypt.png" => Dto.Environments.TileType.DirtWall,
                "level/zone1_wall_dirt_cracked.png" => Dto.Environments.TileType.DirtWall,
                "level/wall_dirt_crypt_diamond1.png" => Dto.Environments.TileType.DirtWall,
                "level/wall_dirt_crypt_diamond2.png" => Dto.Environments.TileType.DirtWall,
                "level/wall_dirt_crypt_diamond3.png" => Dto.Environments.TileType.DirtWall,
                "level/wall_dirt_crypt_diamond4.png" => Dto.Environments.TileType.DirtWall,
                "level/wall_stone_crypt.png" => Dto.Environments.TileType.StoneWall,
                "level/zone1_wall_stone_cracked.png" => Dto.Environments.TileType.StoneWall,
                "level/zone1_catacomb_cracked.png" => Dto.Environments.TileType.StoneWall,
                "level/wall_catacomb_crypt1.png" => Dto.Environments.TileType.UnbreakableWall,
                "level/wall_catacomb_crypt2.png" => Dto.Environments.TileType.UnbreakableWall,
                "level/wall_shop_crypt.png" => Dto.Environments.TileType.UnbreakableWall,
                "level/end_of_world.png" => Dto.Environments.TileType.UnbreakableWall,
                "level/TEMP_shop_floor.png" => Dto.Environments.TileType.Floor,
                "level/TEMP_floor_water.png" => Dto.Environments.TileType.Water,
                _ => throw new ArgumentException($"Unknown image path: '{imagePath}'.")
            };

            return new Dto.Environments.Tile
            {
                Type = tileType
            };
        }

        public static Dto.Environments.Player ToDto(this Player player)
        {
            return new Dto.Environments.Player
            {
                Health = player._health
            };
        }

        public static Dto.Environments.Enemy ToDto(this Enemy enemy, string enemyType)
        {
            var type = enemyType switch
            {
                "c_Slime" => Dto.Environments.EnemyType.Slime,
                "c_Shopkeeper" => Dto.Environments.EnemyType.Shopkeeper,
                "c_Conjurer" => Dto.Environments.EnemyType.Conjurer,
                "c_BatMiniboss" => Dto.Environments.EnemyType.BatMiniboss,
                "c_Bat" => Dto.Environments.EnemyType.Bat,
                "c_Monkey" => Dto.Environments.EnemyType.Monkey,
                "c_Skeleton" => Dto.Environments.EnemyType.Skeleton,
                "c_Crate" => Dto.Environments.EnemyType.Crate,
                "c_Zombie" => Dto.Environments.EnemyType.Zombie,
                "c_Medic" => Dto.Environments.EnemyType.Medic,
                "c_Bossmaster" => Dto.Environments.EnemyType.Bossmaster,
                "c_Merlin" => Dto.Environments.EnemyType.Merlin,
                "c_Hephaestus" => Dto.Environments.EnemyType.Hephaestus,
                "c_Beastmaster" => Dto.Environments.EnemyType.Beastmaster,
                "c_Trainer" => Dto.Environments.EnemyType.Trainer,
                "c_Janitor" => Dto.Environments.EnemyType.Janitor,
                "c_DiamondDealer" => Dto.Environments.EnemyType.DiamondDealer,
                "c_Weaponmaster" => Dto.Environments.EnemyType.Weaponmaster,
                "c_Pawnbroker" => Dto.Environments.EnemyType.Pawnbroker,
                "c_Minotaur" => Dto.Environments.EnemyType.Minotaur,
                "c_Dragon" => Dto.Environments.EnemyType.Dragon,
                "c_Ghost" => Dto.Environments.EnemyType.Ghost,
                "c_Wraith" => Dto.Environments.EnemyType.Wraith,
                _ => throw new ArgumentException($"Unknown enemy type {enemyType}.")
            };

            return new Dto.Environments.Enemy(type)
            {
                Health = enemy._health
            };
        }

        public static Dto.Environments.Trap ToDto(this Trap trap, string trapType)
        {
            return new Dto.Environments.Trap
            {
                Type = trapType switch
                {
                    "c_BounceTrap" => Dto.Environments.TrapType.BounceTrap,
                    "c_TravelRune" => Dto.Environments.TrapType.TravelRune,
                    "c_SlowDownTrap" => Dto.Environments.TrapType.SlowDownTrap,
                    "c_SpeedUpTrap" => Dto.Environments.TrapType.SpeedUpTrap,
                    "c_TrapDoor" => Dto.Environments.TrapType.TrapDoor,
                    "c_BombTrap" => Dto.Environments.TrapType.BombTrap,
                    _ => throw new ArgumentException($"Unknown trap type {trapType}.")
                }
            };
        }
    }
}

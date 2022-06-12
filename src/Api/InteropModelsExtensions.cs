using Paladin.Interop;
using Paladin.Interop.Models.Generated;

namespace Paladin.Api
{
    public static class InteropModelsExtensions
    {
        public static Paladin.CotNd.Tile ToDto(this Tile tile, GameProxy gameProxy)
        {
            var sprite = gameProxy.GetTileSprite(tile);
            var imagePath = gameProxy.GetSpriteImagePath(sprite);

            var tileType = imagePath switch
            {
                "level/floor_dirt1.png" => Paladin.CotNd.TileType.Floor,
                "level/floor_dirt2.png" => Paladin.CotNd.TileType.Floor,
                "level/TEMP_npc_floor.png" => Paladin.CotNd.TileType.Floor,
                "level/stairs.png" => Paladin.CotNd.TileType.Exit,
                "level/stairs_locked.png" => Paladin.CotNd.TileType.Exit,
                "level/stairs_locked_miniboss.png" => Paladin.CotNd.TileType.Exit,
                "level/wall_dirt_crypt.png" => Paladin.CotNd.TileType.DirtWall,
                "level/zone1_wall_dirt_cracked.png" => Paladin.CotNd.TileType.DirtWall,
                "level/wall_dirt_crypt_diamond1.png" => Paladin.CotNd.TileType.DirtWall,
                "level/wall_dirt_crypt_diamond2.png" => Paladin.CotNd.TileType.DirtWall,
                "level/wall_dirt_crypt_diamond3.png" => Paladin.CotNd.TileType.DirtWall,
                "level/wall_dirt_crypt_diamond4.png" => Paladin.CotNd.TileType.DirtWall,
                "level/wall_stone_crypt.png" => Paladin.CotNd.TileType.StoneWall,
                "level/zone1_wall_stone_cracked.png" => Paladin.CotNd.TileType.StoneWall,
                "level/zone1_catacomb_cracked.png" => Paladin.CotNd.TileType.StoneWall,
                "level/wall_catacomb_crypt1.png" => Paladin.CotNd.TileType.UnbreakableWall,
                "level/wall_catacomb_crypt2.png" => Paladin.CotNd.TileType.UnbreakableWall,
                "level/wall_shop_crypt.png" => Paladin.CotNd.TileType.UnbreakableWall,
                "level/end_of_world.png" => Paladin.CotNd.TileType.UnbreakableWall,
                "level/TEMP_shop_floor.png" => Paladin.CotNd.TileType.Floor,
                "level/TEMP_floor_water.png" => Paladin.CotNd.TileType.Water,
                _ => throw new ArgumentException($"Unknown image path: '{imagePath}'.")
            };

            return new Paladin.CotNd.Tile(tileType);
        }

        public static Paladin.CotNd.Player ToDto(this Player player)
        {
            return new Paladin.CotNd.Player
            {
                Health = player._health
            };
        }

        public static Paladin.CotNd.Enemy ToDto(this Enemy enemy, string enemyType)
        {
            var type = enemyType switch
            {
                "c_Slime" => Paladin.CotNd.EnemyType.Slime,
                "c_Shopkeeper" => Paladin.CotNd.EnemyType.Shopkeeper,
                "c_Conjurer" => Paladin.CotNd.EnemyType.Conjurer,
                "c_BatMiniboss" => Paladin.CotNd.EnemyType.BatMiniboss,
                "c_Bat" => Paladin.CotNd.EnemyType.Bat,
                "c_Monkey" => Paladin.CotNd.EnemyType.Monkey,
                "c_Skeleton" => Paladin.CotNd.EnemyType.Skeleton,
                "c_Crate" => Paladin.CotNd.EnemyType.Crate,
                "c_Zombie" => Paladin.CotNd.EnemyType.Zombie,
                "c_Medic" => Paladin.CotNd.EnemyType.Medic,
                "c_Bossmaster" => Paladin.CotNd.EnemyType.Bossmaster,
                "c_Merlin" => Paladin.CotNd.EnemyType.Merlin,
                "c_Hephaestus" => Paladin.CotNd.EnemyType.Hephaestus,
                "c_Beastmaster" => Paladin.CotNd.EnemyType.Beastmaster,
                "c_Trainer" => Paladin.CotNd.EnemyType.Trainer,
                "c_Janitor" => Paladin.CotNd.EnemyType.Janitor,
                "c_DiamondDealer" => Paladin.CotNd.EnemyType.DiamondDealer,
                "c_Weaponmaster" => Paladin.CotNd.EnemyType.Weaponmaster,
                "c_Pawnbroker" => Paladin.CotNd.EnemyType.Pawnbroker,
                "c_Minotaur" => Paladin.CotNd.EnemyType.Minotaur,
                "c_Dragon" => Paladin.CotNd.EnemyType.Dragon,
                "c_Ghost" => Paladin.CotNd.EnemyType.Ghost,
                "c_Wraith" => Paladin.CotNd.EnemyType.Wraith,
                _ => throw new ArgumentException($"Unknown enemy type {enemyType}.")
            };

            return new Paladin.CotNd.Enemy(type)
            {
                Health = enemy._health
            };
        }

        public static Paladin.CotNd.Trap ToDto(this Trap trap, string trapType)
        {
            return new Paladin.CotNd.Trap
            {
                Type = trapType switch
                {
                    "c_BounceTrap" => Paladin.CotNd.TrapType.BounceTrap,
                    "c_TravelRune" => Paladin.CotNd.TrapType.TravelRune,
                    "c_SlowDownTrap" => Paladin.CotNd.TrapType.SlowDownTrap,
                    "c_SpeedUpTrap" => Paladin.CotNd.TrapType.SpeedUpTrap,
                    "c_TrapDoor" => Paladin.CotNd.TrapType.TrapDoor,
                    "c_BombTrap" => Paladin.CotNd.TrapType.BombTrap,
                    _ => throw new ArgumentException($"Unknown trap type {trapType}.")
                }
            };
        }
    }
}

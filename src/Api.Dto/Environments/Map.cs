using System.Collections.Generic;

namespace Paladin.Api.Dto.Environments
{
    public class Map
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public IEnumerable<IEnumerable<Tile>> Tiles { get; set; }
    }
}

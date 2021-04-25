using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerrariaBiomes
{
    public class BiomeWorker_NoTest : BiomeWorker
    {

        public override float GetScore(Tile tile, int tileID)
        {
            if(tile.WaterCovered)
            {
                return -100f;
            }

            return -100f;
        }
    }
}

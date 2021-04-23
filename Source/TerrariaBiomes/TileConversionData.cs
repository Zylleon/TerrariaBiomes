using RimWorld;
using Verse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerrariaBiomes
{
    public class TileConversionData
    {
        public int tile;

        public int lastConvertedTick = Find.TickManager.TicksGame;
        public BiomeDef originalBiome;
        public ConvStatus convStatus = ConvStatus.Pure;
        
    }
}

using RimWorld;
using Verse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerrariaBiomes
{
    public class TileConversionData : IExposable
    {
        public int tile;

        public int lastConvertedTick = Find.TickManager.TicksGame;
        public string originalBiome = BiomeDefOf.TemperateForest.defName;
        public ConvStatus convStatus = ConvStatus.Pure;

        public void ExposeData()
        {
            Scribe_Values.Look(ref tile, "tile", 0);
            Scribe_Values.Look(ref lastConvertedTick, "lastConvertedTick", 0);
            Scribe_Values.Look(ref originalBiome, "originalBiome", BiomeDefOf.TemperateForest.defName);
            Scribe_Values.Look(ref convStatus, "convStatus", ConvStatus.Pure);
        }

    }
}

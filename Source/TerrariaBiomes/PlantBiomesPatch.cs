using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace TerrariaBiomes
{
    [HarmonyPatch(typeof(PlantUtility), nameof(PlantUtility.CanEverPlantAt_NewTemp))]
    static class PlantBiomesPatch
    {
        static bool Prefix(ref bool __result, ThingDef plantDef, IntVec3 c, Map map)
        {
            if(plantDef.HasModExtension<ZTB_AllowedRange>())
            {
                ZTB_AllowedRange allowedRange = plantDef.GetModExtension<ZTB_AllowedRange>();
                if(map.TileInfo.temperature > allowedRange.maxTileTemp || map.TileInfo.temperature < allowedRange.minTileTemp)
                {
                    __result = false;
                    return false;
                }
                if (map.TileInfo.rainfall > allowedRange.maxTileRainfall || map.TileInfo.rainfall < allowedRange.minTileRainfall)
                {
                    __result = false;
                    return false;
                }
            }

            //map.TileInfo.swampiness

            return true;
        }
    }

    public class ZTB_AllowedRange : DefModExtension
    {
        public float minTileTemp = -999f;
        public float maxTileTemp = 999f;
        public float minTileRainfall = -9999f;
        public float maxTileRainfall = 99999f;

        //public bool requireSwamp = false;
        //public bool allowSwamp = true;
    }
}

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
                __result = allowedRange.IsAllowedIn(map);

                // This is intentional. If this filter allows the plant, the parent method still needs to run.
                return __result;            
            }

            return true;
        }
    }


    public class ZTB_AllowedRange : DefModExtension
    {
        public float minTileTemp = -999f;
        public float maxTileTemp = 999f;
        public float minTileRainfall = -9999f;
        public float maxTileRainfall = 99999f;
        public bool? swamp = null;          // true = require swamp, false = forbid swamp
        
        public bool IsAllowedIn(Map map)
        {
            if (map.TileInfo.temperature > maxTileTemp || map.TileInfo.temperature < minTileTemp)
            {
                return false;
            }
            if (map.TileInfo.rainfall > maxTileRainfall || map.TileInfo.rainfall < minTileRainfall)
            {
                return false;
            }

            if (swamp != null)
            {
                if (map.TileInfo.swampiness > 0.5f != swamp)
                {
                    return false;
                }
            }
            return true;
        }
    }


    [HarmonyPatch(typeof(Command_SetPlantToGrow), "IsPlantAvailable")]
    static class PlantGrowingZonesPatch
    {
        static void Postfix(ThingDef plantDef, Map map, ref bool __result)
        {
            if(__result)
            {
                if(plantDef.HasModExtension<ZTB_AllowedRange>())
                {
                    ZTB_AllowedRange allowedRange = plantDef.GetModExtension<ZTB_AllowedRange>();
                    __result = allowedRange.IsAllowedIn(map);
                }
            }
        }
    }

}

using HarmonyLib;
using RimWorld;
using Verse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerrariaBiomes
{
    [HarmonyPatch(typeof(WildAnimalSpawner), "DesiredTotalAnimalWeight", MethodType.Getter)]
    static class AnimalDensityPatch
    {
        static void Postfix(Map ___map, ref float __result)
        {
            if (___map.Biome.defName != "ZTB_Corruption")
            {
                return;
            }
            __result *= HelperMethods.GetDensityMultiplier(___map);
        }
    }


    [HarmonyPatch(typeof(WildPlantSpawner), "CurrentPlantDensity", MethodType.Getter)]
    static class PlantDensityPatch
    {
        static void Postfix(Map ___map, ref float __result)
        {
            if(___map.Biome.defName != "ZTB_Corruption")
            {
                return;
            }
            __result *= HelperMethods.GetDensityMultiplier(___map);
        }
    }


}

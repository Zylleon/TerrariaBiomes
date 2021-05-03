using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Verse;
using RimWorld;


namespace TerrariaBiomes
{
    [StaticConstructorOnStartup]
    public static class TerrariaBiomes
    {
        static TerrariaBiomes()
        {
            Harmony harmony = new Harmony("zylle.TerrariaBiomes");
            Log.Message("TERRARRRRIA BIOMES");
            harmony.PatchAll();
        }
    }

}

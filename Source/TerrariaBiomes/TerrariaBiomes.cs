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
    //[StaticConstructorOnStartup]
    public sealed class TerrariaBiomes : Mod
    {
        public TerrariaBiomes(ModContentPack content) : base(content)
        {
            new Harmony("zylle.TerrariaBiomes").PatchAll();
            Log.Message("Initializing Terraria Biomes");
        }
    }

}

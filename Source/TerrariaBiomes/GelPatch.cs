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
    [HarmonyPatch(typeof(ThingDefGenerator_Meat), "ImpliedMeatDefs")]
    static class GelPatch
    {
        static IEnumerable<ThingDef> Postfix(IEnumerable<ThingDef> meatThings)
        {
            Log.Message("running patch");
            foreach (var thingDef in meatThings)
            {
                Log.Message(thingDef.defName);
                if (thingDef.defName == "Meat_ZTB_CorruptSlime")
                {
                    Log.Message("......Found gel");
                    thingDef.graphicData.texPath = "ZTBiomes/Item/Gel/Gel";
                    thingDef.graphicData.graphicClass = typeof(Graphic_Single);

                    thingDef.SetStatBaseValue(StatDefOf.Nutrition, 0.03f);
                    thingDef.SetStatBaseValue(StatDefOf.Flammability, 2.0f);
                    thingDef.description = "ZTB_GelDesc".Translate();


                    thingDef.comps.Clear();

                    thingDef.comps.Add(new CompProperties_Forbiddable());
                    CompProperties_Rottable compProperties_Rottable = new CompProperties_Rottable();
                    compProperties_Rottable.daysToRotStart = 8f;
                    compProperties_Rottable.rotDestroys = true;
                    thingDef.comps.Add(compProperties_Rottable);

                }

                yield return thingDef;

            }
        }

    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace TerrariaBiomes
{
    public class TerrairaBiomesSettings : ModSettings
    {
        // the first day the biome can spawn
        public int corrFirstDays = 15;
        public int halFirstDays = 60;

        // new spawn interval
        public int corrSpawnDays = 7;
        public int halSpawnDays = 7;

        // spread interval
        public int corrSpreadDays = 1;
        public int halSpreadDays = 1;

        //misc
        public int aiPurifyDays = 10;
        public int regenPlanetDays = 1;


        public override void ExposeData()
        {
            Scribe_Values.Look(ref corrFirstDays, "corrFirstDays", 15);
            Scribe_Values.Look(ref halFirstDays, "halFirstDays", 60);

            Scribe_Values.Look(ref corrSpawnDays, "corrSpawnDays", 7);
            Scribe_Values.Look(ref halSpawnDays, "halSpawnDays", 7);

            Scribe_Values.Look(ref corrSpreadDays, "corrSpreadDays", 1);
            Scribe_Values.Look(ref halSpreadDays, "halSpreadDays", 1);

            Scribe_Values.Look(ref aiPurifyDays, "aiPurifyDays", 10);
            Scribe_Values.Look(ref regenPlanetDays, "regenPlanetDays", 1);

        }
    }


    public class TerrariaBiomesMod : Mod
    {
        public TerrairaBiomesSettings settings;
        public static TerrariaBiomesMod mod;
        public static ModContentPack modContentPack;
        public TerrariaBiomesMod(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<TerrairaBiomesSettings>();
            modContentPack = content;
            mod = this;
        }
        public override string SettingsCategory()
        {
            return "ZTB_TerrariaBiomes".Translate();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();

            listingStandard.Begin(inRect);

            //Corruption
            listingStandard.Label("ZTB_Corruption".Translate());
            mod.settings.corrFirstDays = (int)listingStandard.SliderLabeled(String.Format("ZTB_FirstDaysLabel".Translate(),  mod.settings.corrFirstDays * 0.1), mod.settings.corrFirstDays, 1, 2000);
            mod.settings.corrSpawnDays = (int)listingStandard.SliderLabeled(String.Format("ZTB_SpawnDaysLabel".Translate(), mod.settings.corrSpawnDays * 0.1), mod.settings.corrSpawnDays, 1, 1000);
            mod.settings.corrSpreadDays = (int)listingStandard.SliderLabeled(String.Format("ZTB_SpreadDaysLabel".Translate(), mod.settings.corrSpreadDays * 0.1), mod.settings.corrSpreadDays, 1, 100);
            listingStandard.GapLine();

            //Hallow
            listingStandard.Label("ZTB_Hallow".Translate());
            mod.settings.halFirstDays = (int)listingStandard.SliderLabeled(String.Format("ZTB_FirstDaysLabel".Translate(), mod.settings.halFirstDays * 0.1), mod.settings.halFirstDays, 1, 2000);
            mod.settings.halSpawnDays = (int)listingStandard.SliderLabeled(String.Format("ZTB_SpawnDaysLabel".Translate(), mod.settings.halSpawnDays * 0.1), mod.settings.halSpawnDays, 1, 1000);
            mod.settings.halSpreadDays = (int)listingStandard.SliderLabeled(String.Format("ZTB_SpreadDaysLabel".Translate(), mod.settings.halSpreadDays * 0.1), mod.settings.halSpreadDays, 1, 100);
            listingStandard.GapLine();

            //Misc
            mod.settings.aiPurifyDays = (int)listingStandard.SliderLabeled(String.Format("ZTB_AiPurifyDaysLabel".Translate(), mod.settings.aiPurifyDays * 0.1), mod.settings.aiPurifyDays, 1, 500);
            mod.settings.regenPlanetDays = (int)listingStandard.SliderLabeled(String.Format("ZTB_RegenPlanetDaysLabel".Translate(), mod.settings.regenPlanetDays * 0.1), mod.settings.regenPlanetDays, 1, 100);

            listingStandard.End();
        }


    }


}

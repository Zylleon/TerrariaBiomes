using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace TerrariaBiomes
{
    public class DeathActionWorker_SpawnSlimelings : DeathActionWorker
    {
        //public override RulePackDef DeathRules
        //{
        //    get
        //    {
        //        return RulePackDef.Named("ZEle_DiedCrumbling");
        //    }
        //}

        private int totalCount = 10;
        private int chunkCount = 10;
        private int brickCount = 10;

        public override void PawnDied(Corpse corpse)
        {
            if(Rand.Bool)
            {
                return;
            }

            PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDef.Named("ZTB_Slimeling"), corpse.InnerPawn.Faction, PawnGenerationContext.NonPlayer, -1, forceGenerateNewPawn: false, newborn: true);

            Pawn pawn = null;
            int spawnCount = Rand.Range(3, 5);
            for(int i = 0; i < spawnCount; i++)
            {
                pawn = PawnGenerator.GeneratePawn(request);

                GenSpawn.Spawn(pawn, corpse.Position, corpse.Map);
                pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter);
            }
            // send letter
            Find.LetterStack.ReceiveLetter("ZTB_LetterSlimelingsLabel".Translate(), "ZTB_LetterSlimelings".Translate(),LetterDefOf.ThreatBig);
        
        
        
        }

    }
}


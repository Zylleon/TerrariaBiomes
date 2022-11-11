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

        public override void PawnDied(Corpse corpse)
        {
            if(Rand.Bool)
            {
                return;
            }

            //PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDef.Named("ZTB_Slimeling"), corpse.InnerPawn.Faction, PawnGenerationContext.NonPlayer, -1, forceGenerateNewPawn: false, newborn: true);
            PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDef.Named("ZTB_Slimeling"), corpse.InnerPawn.Faction, PawnGenerationContext.NonPlayer, -1, forceGenerateNewPawn: false, fixedBiologicalAge: 0.0f);


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


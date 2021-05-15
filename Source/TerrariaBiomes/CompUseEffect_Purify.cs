using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace TerrariaBiomes
{
    public class CompUseEffect_Purify : CompUseEffect
    {
        public override void DoEffect(Pawn usedBy)
        {
            base.DoEffect(usedBy);
            WorldComponent_Conversion conversion = (WorldComponent_Conversion)Find.World.GetComponent(typeof(WorldComponent_Conversion));
            conversion.SpreadPurityFromPoint(usedBy.Map);

        }
    }
}

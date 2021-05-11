using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace TerrariaBiomes
{
    public static class HelperMethods
    {
        public static float GetDensityMultiplier(Map map)
        {
            float factor = 1f;
            if (map.TileInfo.temperature < -25f)
            {
                factor *= 0.1f;
            }
            else if (map.TileInfo.temperature < -15f)
            {
                factor *= 0.3f;
            }
            else if (map.TileInfo.temperature < -5f)
            {
                factor *= 0.75f;
            }
            else if (map.TileInfo.rainfall < 600f)
            {
                factor *= 0.15f;
            }
            else if (map.TileInfo.temperature > 18)
            {
                if (map.TileInfo.rainfall > 2200)
                {
                    factor *= 1.5f;
                }
                else
                {
                    factor *= 0.5f;
                }
            }

            if (map.TileInfo.swampiness > 0.5f)
            {
                factor *= 1.5f;
            }

            return factor;
        }
    }
}

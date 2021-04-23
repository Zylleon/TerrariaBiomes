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
    public class WorldComponent_Conversion : WorldComponent
    {

        private TileConversionData[] cache;
        //private List<int> usedSlots;

        public WorldComponent_Conversion(World world)
        : base(world)
        {
            //ClearCaches();
        }


        public override void WorldComponentTick()
        {
            if(cache == null)
            {
                PopulateCache();
            }

          
            if (Find.TickManager.TicksGame % 500 == 84)         // 5 days to cover standard world at this speed
            {
                //Log.Message("Growing tiles");
                if(!Find.WorldGrid.tiles.Any(t => t.biome.defName == "ZTB_Corruption"))
                {
                    Log.Message("Spawning initial corruption");
                    Tile tile = Find.WorldGrid.tiles.Where(t => !t.WaterCovered).RandomElement();
                    tile.biome = BiomeDef.Named("ZTB_Corruption");
                    CorruptTile(Find.WorldGrid.tiles.FindIndex(x => x == tile));
                }

                else
                {
                    //Log.Message("Expanding corruption.....");
                    List<TileConversionData> corruptTiles = cache.Where(x => x.convStatus == ConvStatus.Corrupt).ToList();

                    List<int> tmpTiles = new List<int>();

                    foreach(TileConversionData tile in corruptTiles)
                    {
                        Find.WorldGrid.GetTileNeighbors(tile.tile, tmpTiles);
                        int tileID = tmpTiles.RandomElement();
                        Tile toConvert = Find.WorldGrid[tileID];

                        if(toConvert.biome.defName != "ZTB_Corruption" && !toConvert.WaterCovered)
                        {
                            toConvert.biome = BiomeDef.Named("ZTB_Corruption");
                            CorruptTile(tileID);
                        }
                    }

                }

            }

        }



        private void PopulateCache()
        {
            Log.Message("Populating conversion cache");

            cache = new TileConversionData[Find.WorldGrid.tiles.Count];

            for(int i = 0; i < Find.WorldGrid.tiles.Count(); i++)
            {
                Tile tile = Find.WorldGrid.tiles[i];
                TileConversionData tileData = new TileConversionData();
                tileData.tile = i;
                tileData.originalBiome = tile.biome;

                if(tile.biome.defName == "ZTB_Corruption")
                {
                    tileData.convStatus = ConvStatus.Corrupt;
                }
                //if (tile.biome.defName == "ZTB_Hallow")
                //{
                //    tileData.convStatus = ConvStatus.Hallowed;
                //}
                cache[i] = tileData;
            }

            Log.Message("Conversion cache has been populated");
        }



        private void CorruptTile(int tileId)
        {
            cache[tileId].lastConvertedTick = Find.TickManager.TicksGame;
            cache[tileId].convStatus = ConvStatus.Corrupt;
        }


       

    }
}

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

        //private TileConversionData[] cache;
        private List<TileConversionData> cache;

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

            //if (Find.TickManager.TicksGame % 500 == 84)         // 5 days to cover standard world at this speed
            if (Find.TickManager.TicksGame % 2000 == 84)
            {
                //Log.Message("Growing tiles");
                if(!Find.WorldGrid.tiles.Any(t => t.biome.defName == "ZTB_Corruption") || Find.TickManager.TicksGame % 200000 == 84)
                {
                    Log.Message("Spawning corruption");
                    Tile tile = Find.WorldGrid.tiles.Where(t => !t.WaterCovered && t.biome != ZTB_DefOf.ZTB_Corruption).RandomElement();
                    if(tile != null)
                    {
                        tile.biome = BiomeDef.Named("ZTB_Corruption");
                        CorruptTile(Find.WorldGrid.tiles.FindIndex(x => x == tile));
                    }
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
                            if(cache[tileID].lastConvertedTick + 500000 < Find.TickManager.TicksGame)       // don't instantly reconvert tiles
                            {
                                toConvert.biome = BiomeDef.Named("ZTB_Corruption");
                                CorruptTile(tileID);
                            }
                        }
                    }
                }


                if (Find.TickManager.TicksGame % 60000 == 84)
                {
                    // randomly clear corruption from faction bases
                    Settlement settlement = Find.World.worldObjects.Settlements.RandomElement();

                    foreach(Settlement sett in Find.World.worldObjects.Settlements)
                    {
                        if(Rand.Chance(0.005f))
                        {
                            if (!sett.Faction.IsPlayer && cache[sett.Tile].convStatus != ConvStatus.Pure)
                            {
                                SpreadPurityFromPoint(settlement.Tile);
                            }
                        }
                    }

                    // regenerate world map
                    Find.World.renderer.SetDirty<WorldLayer_Terrain>();
                }
            }
        }


        public void SpreadPurityFromPoint(int start)
        {
            HashSet<int> toConvert = new HashSet<int>();
            HashSet<int> neighbors = new HashSet<int>();
            toConvert.Add(start);

            List<int> tmpTiles = new List<int>();

            for(int i = 0; i <= 12; i++)
            {
                foreach(int t in toConvert)
                {
                    Find.WorldGrid.GetTileNeighbors(t, tmpTiles);
                    //neighbors.UnionWith(tmpTiles);
                    //neighbors.Add(tmpTiles.RandomElement());

                    neighbors.AddRange(tmpTiles.TakeRandom(2));
                    
                }

                toConvert.UnionWith(neighbors);
            }

            foreach(int t in toConvert)
            {
                Tile tile = Find.WorldGrid[t];
                tile.biome = BiomeDef.Named(cache[t].originalBiome);
                PurifyTile(t);
            }
        }


        private void PopulateCache()
        {
            Log.Message("Populating conversion cache");

            //cache = new TileConversionData[Find.WorldGrid.tiles.Count];
            cache = new List<TileConversionData>();

            for(int i = 0; i < Find.WorldGrid.tiles.Count(); i++)
            {
                Tile tile = Find.WorldGrid.tiles[i];
                TileConversionData tileData = new TileConversionData();
                tileData.tile = i;

                if(tile.biome.defName == "ZTB_Corruption")
                {
                    tileData.convStatus = ConvStatus.Corrupt;
                }
                else if (tile.biome.defName == "ZTB_Hallow")
                {
                    tileData.convStatus = ConvStatus.Hallowed;
                }
                else
                {
                    tileData.originalBiome = tile.biome.defName;
                }
                //cache[i] = tileData;
                cache.Add(tileData);
            }

        }

         

        private void CorruptTile(int tileId)
        {
            cache[tileId].lastConvertedTick = Find.TickManager.TicksGame;
            cache[tileId].convStatus = ConvStatus.Corrupt;

            //Find.LetterStack.ReceiveLetter("ZTB_LetterCorruptionLabel".Translate(), "ZTB_LetterCorruption".Translate(), LetterDefOf.NeutralEvent);

        }


        private void PurifyTile(int tileId)
        {
            cache[tileId].lastConvertedTick = Find.TickManager.TicksGame;
            cache[tileId].convStatus = ConvStatus.Pure;

            //Find.LetterStack.ReceiveLetter("ZTB_LetterPurityLabel".Translate(), "ZTB_LetterPurity".Translate(), LetterDefOf.NeutralEvent);

        }


        public override void ExposeData()
        {
            Scribe_Collections.Look(ref cache, "cache", LookMode.Deep);
        }

    }
}

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


        TerrairaBiomesSettings settings = TerrariaBiomesMod.mod.settings;
        //private TileConversionData[] cache;
        private List<TileConversionData> cache;


        private int ticksToReconvert = 150000;
        private int ticksDay = 6000;    // really the ticks in 0.1 days


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

            if (Find.TickManager.TicksGame > settings.corrFirstDays * ticksDay && !Find.WorldGrid.tiles.Any(t => t.biome.defName == "ZTB_Corruption"))
            {
                StartCorruption();
            }

            if (Find.TickManager.TicksGame + settings.corrFirstDays * ticksDay % (settings.corrSpawnDays * ticksDay) == 84)
            {
                StartCorruption();
            }

            //if (Find.TickManager.TicksGame % 500 == 84)         // about 5 days to cover standard world at this speed
            if (Find.WorldGrid.tiles.Any(t => t.biome.defName == "ZTB_Corruption") && Find.TickManager.TicksGame % (settings.corrSpreadDays * ticksDay) == 84)
            {
                SpreadCorruption();
            }


            if (Find.TickManager.TicksGame > settings.halFirstDays * ticksDay && Find.TickManager.TicksGame % (settings.halSpawnDays * ticksDay) == 84)
            {
                StartHallow();
            }
            
            if (Find.WorldGrid.tiles.Any(t => t.biome.defName == "ZTB_Hallow") && Find.TickManager.TicksGame % (settings.halSpreadDays * ticksDay) == 84)
            {
                SpreadHallow();
            }


            // AI factions use purification powder
            if (Find.TickManager.TicksGame % (settings.aiPurifyDays * ticksDay) == 84)
            {
                // randomly clear corruption from faction bases
                foreach (Settlement sett in Find.World.worldObjects.Settlements)
                {
                    if (Rand.Chance(0.005f))
                    {
                        if (!sett.Faction.IsPlayer && cache[sett.Tile].convStatus != ConvStatus.Pure)
                        {
                            SpreadPurityFromPoint(sett.Tile);
                        }
                    }
                }
            }


            // regenerate world map, otherwise the spread is invisible
            if (Find.TickManager.TicksGame % (settings.regenPlanetDays * 6000) == 84)
            {
                Find.World.renderer.SetDirty<WorldLayer_Terrain>();
            }
        }


        private void StartCorruption()
        {
            // find a random uncorrupted tile and corrupt it
            Tile tile = Find.WorldGrid.tiles.Where(t => t.biome != BiomeDefOf.Ocean && t.biome != BiomeDefOf.Lake && t.biome != ZTB_DefOf.ZTB_Corruption).RandomElement();
            if (tile != null)
            {
                CorruptTile(Find.WorldGrid.tiles.FindIndex(x => x == tile));
            }
            
        }

        private void SpreadCorruption()
        {
            // for each corrupted tile, pick 1 random neighbor to convert
            List<TileConversionData> corruptTiles = cache.Where(x => x.convStatus == ConvStatus.Corrupt).ToList();
            List<int> tmpTiles = new List<int>();

            foreach (TileConversionData tile in corruptTiles)
            {
                Find.WorldGrid.GetTileNeighbors(tile.tile, tmpTiles);
                int tileID = tmpTiles.RandomElement();
                Tile toConvert = Find.WorldGrid[tileID];

                if (toConvert.biome.defName != "ZTB_Corruption" && toConvert.biome != BiomeDefOf.Ocean && toConvert.biome != BiomeDefOf.Lake)
                {
                    if (cache[tileID].lastConvertedTick + ticksToReconvert < Find.TickManager.TicksGame)       // don't instantly reconvert tiles
                    {
                        CorruptTile(tileID);
                    }
                }
            }
        }


        private void StartHallow()
        {
            Tile tile = Find.WorldGrid.tiles.Where(t => t.biome != BiomeDefOf.Ocean && t.biome != BiomeDefOf.Lake && t.biome != ZTB_DefOf.ZTB_Hallow).RandomElement();
            if (tile != null)
            {
                HallowTile(Find.WorldGrid.tiles.FindIndex(x => x == tile));
            }
        }


        // it's sloppy to copy this method instead of combining it with SpreadHallow into one general method. Fix later maybe?
        private void SpreadHallow()
        {
            List<TileConversionData> hallowedTiles = cache.Where(x => x.convStatus == ConvStatus.Hallowed).ToList();
            List<int> tmpTiles = new List<int>();

            foreach (TileConversionData tile in hallowedTiles)
            {
                Find.WorldGrid.GetTileNeighbors(tile.tile, tmpTiles);
                int tileID = tmpTiles.RandomElement();
                Tile toConvert = Find.WorldGrid[tileID];

                if (toConvert.biome.defName != "ZTB_Hallow" && toConvert.biome != BiomeDefOf.Ocean && toConvert.biome != BiomeDefOf.Lake)
                {
                    if (cache[tileID].lastConvertedTick + ticksToReconvert < Find.TickManager.TicksGame)       // don't instantly reconvert tiles
                    {
                        HallowTile(tileID);
                    }
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
            Find.WorldGrid[tileId].biome = ZTB_DefOf.ZTB_Corruption;
            cache[tileId].lastConvertedTick = Find.TickManager.TicksGame;
            cache[tileId].convStatus = ConvStatus.Corrupt;

            //Find.LetterStack.ReceiveLetter("ZTB_LetterCorruptionLabel".Translate(), "ZTB_LetterCorruption".Translate(), LetterDefOf.NeutralEvent);
        }

        private void HallowTile(int tileId)
        {
            Find.WorldGrid[tileId].biome = ZTB_DefOf.ZTB_Hallow;

            cache[tileId].lastConvertedTick = Find.TickManager.TicksGame;
            cache[tileId].convStatus = ConvStatus.Hallowed;

            //Find.LetterStack.ReceiveLetter("ZTB_LetterCorruptionLabel".Translate(), "ZTB_LetterHallow".Translate(), LetterDefOf.NeutralEvent);
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

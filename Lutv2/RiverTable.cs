using System;

namespace Lutv2
{
    public class RiverTable:Table
    {
        // Says to which rank pattern this rank belongs.
	    private int[,,,,,,] rankPatternIndex = new int[7,7,7,7,7,7,7];
	
	    private static int[] n = {0,12,23,33,42,50,57,63,68,72,75,77,78};
	    private static int[] m = {0,78,144,199,244,280,308,329,344,354,360,363,364};
	    private static int[] o = {0,364,650,870,1035,1155,1239,1295,1330,1350,1360,1364,1365};
	    private static int[] p = {0,1365,2366,3081,3576,3906,4116,4242,4312,4347,4362,4367,4368};
	
	    private int tableSize = 52402675;

        // Magic number ~6000
	    private int enumerateRank = 0;
	
	    public RiverTable()
	    {
		    numCards = 7;

		    rankPatternSuits = new RiverSuit[214];
		    numRankPattern = new int[214];
		    rankPositionMap = new int[563199];
		    rankIndexMap = new int[563199];
		
		    int[] sizev = new int[] {7,7,7,7,7,7,7};

		    // Set all rankPatternIndex entries to -1
		    Helper.init_int7(rankPatternIndex, sizev, -1);
	    }
	

	    public override HandInfo HandEval(int[] cards)
	    {
            HandInfo h = new HandInfo();

            ulong pocket = Converter.HandConverter.ConvertToUlong(new int[] { cards[0], cards[1] });
            ulong board = Converter.HandConverter.ConvertToUlong(new int[] { cards[2], cards[3], cards[4], cards[5], cards[6] });

            double hs = HoldemHand.Hand.HandStrength(pocket, board);

            h.ehs2 = hs;
            //h.texture = (byte)FO2.FO2.getRiverBoardIndex_Extended(board);
            //h.ev = HoldemHand.Hand.Evaluate(pocket | board);

            return h;
	    }

        public override void HandEval(int[] cards, ref HandInfo existing)
        {
            //ulong pocket = Converter.HandConverter.ConvertToUlong(new int[] { cards[0], cards[1] });
            //ulong board = Converter.HandConverter.ConvertToUlong(new int[] { cards[2], cards[3], cards[4], cards[5], cards[6] });
            //ulong hand  = Converter.HandConverter.ConvertToUlong(new int[] { cards[0], cards[1], cards[2], cards[3], cards[4], cards[5], cards[6] });
            ////existing.ev = HoldemHand.Hand.Evaluate(hand);

            //existing.bucket = (int)Math.Round(existing.ehs2 * 60-1);
            //existing.bucket_special = (int)Math.Round(existing.ehs2 * 60 * 2-1);

        }


        /// <summary>
        /// Rank index of the board [0, 6188-1]
        /// </summary>
        /// <param name="bRank"></param>
        /// <returns></returns>
	    private int boardRankIndex(int[] bRank)
	    {
		    return p[bRank[0]] + o[bRank[1]] + m[bRank[2]] + n[bRank[3]] + bRank[4];  
	    }
	
        /// <summary>
        /// Creates a unique index for every rank (hole rank, board rank) combination. 
        /// </summary>
        /// <param name="rank"></param>
        /// <returns></returns>
	    public override int HandRankIndex(int[] rank)
	    {
		    int[] hRank= new int[] {rank[0], rank[1]};
		    int[] bRank = new int[] {rank[2], rank[3], rank[4], rank[5], rank[6]};
		
		    int hridx = HoleRankIndex(hRank);
		    int bridx = boardRankIndex(bRank);

		    return bridx*91 + hridx;
	    }
	
        /// <summary>
        /// Only used when doing a dry run to count and generate tables.
        /// </summary>
        /// <param name="Rank"></param>
	    private void countRankSuits(int[] Rank)
	    {
		    int[] r = LowestRank(Rank);
		    int rankIsoIndex = rankPatternIndex[r[0],r[1],r[2],r[3],r[4],r[5],r[6]];

		    // Haven't come upon this rank pattern yet, add it.
		    if (rankIsoIndex == -1) {
			    rankPatternSuits[rankPatternCount] = new RiverSuit();
			    Suits s = rankPatternSuits[rankPatternCount];
			    rankPatternIndex[r[0],r[1],r[2],r[3],r[4],r[5],r[6]] = rankPatternCount;

			    s.EnumSuits(r);

			    if (generationDebug) {
				    Helper.printArray("", r);
                    Console.WriteLine("Num suits:" + s.GetSize() + " rankindex:" + rankPatternCount);
			    }

			    rankIsoIndex = rankPatternCount;
			    rankPatternCount++;
		    }
		
		    int rankidx = HandRankIndex(Rank);

		    rankPositionMap[rankidx] = numRankPattern[rankIsoIndex];
		    rankIndexMap[rankidx] = rankIsoIndex;

		    numRankPattern[rankIsoIndex]++;
	    }
	
	    private void enumerateBoard(int[] Rank)
	    {
    	    for(int i=0; i < 13; i++)
        	    for(int j=i; j < 13; j++)
        		    for (int k=j; k < 13; k++)
        			    for (int l=k; l < 13; l++)
        				    for (int m=l; m < 13; m++) {
        					    Rank[2] = i;
        					    Rank[3] = j;
        					    Rank[4] = k;
        					    Rank[5] = l;
        					    Rank[6] = m;

        					    //	skip 5 of a kind
        					    if (Helper.numMaxRanks(Rank) > 4)
        						    continue;

        					    if (dryrun==1)
        						    countRankSuits(Rank);
        					    else {
        						    EnumerateSuits(Rank);
        					    }
        				    }
	    }

	    private void enumerateHole()
	    {
		    int[] Rank = new int[7];

		    for (int i = 0; i < 13; i++) {
			    for (int j = i; j < 13; j++) {
				    Rank[0] = i;
				    Rank[1] = j;

				    enumerateBoard(Rank);
			    }
		    }
	    }

        public void addAdditional()
        {
            dryrun = 0;
            enumerateHole();
        }
	
	    public override void InitializeTable()
	    {
            LUT_ehs2 = new Single[tableSize];
            LUT_texture = new byte[tableSize];
            
            enumerateHole();

            if (!Helper.load("riverehs_4.dat", ref LUT_ehs2, ref LUT_hp, ref LUT_texture, 0))
                throw new Exception("Error loading river LUT.");

        }

        public void InstantiateEVTables()
        {
            LUT_ehs2 = new Single[tableSize];
            LUT_ev = new Single[tableSize];
            LUT_ev_count = new uint[tableSize];
        }

        public override void InitializeTableEV()
        {
            LUT_ehs2 = new Single[tableSize];
            LUT_ev = new Single[tableSize];
            LUT_ev_count = new uint[tableSize];

            enumerateHole();

            if (!Helper.load("riverehs_4.dat", ref LUT_ehs2, ref LUT_ev, ref LUT_ev_count,
                                               ref ev_max, ref ev_max_count, ref ev_min, ref ev_min_count,
                                               ref ev_avg, ref ev_avg_count, 0))
            {
                throw new Exception("Error loading river LUT.");

                dryrun = 0;
                enumerateHole();

                FO2.FO2.riverBT_cache.Clear();

                // wait for the threads to finish their work (1 can be in progress)
                //waitForThreads();

                // now write to disk
                //Helper.save("riverhs.dat", LUT);
            }
        }

        public override void InitializeEmpty()
        {
            enumerateHole();
        }

    }
}

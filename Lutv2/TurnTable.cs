using System;

namespace Lutv2
{
    public class TurnTable:Table
    {
        // Says to which rank pattern this rank belongs.
	    private int[,,,,,] rankPatternIndex = new int[6,6,6,6,6,6];
	
	    // For boardRankIndex
	    private static int[] n = {0,12,23,33,42,50,57,63,68,72,75,77,78};
	    private static int[] m = {0,78,144,199,244,280,308,329,344,354,360,363,364};
	    private static int[] o = {0,364,650,870,1035,1155,1239,1295,1330,1350,1360,1364,1365};

	    private int tableSize = 15111642;

	    // number of entries done
	    private int count=0;

	    public TurnTable()
	    {
		    numCards = 6;

		    rankPatternSuits = new TurnSuit[89];
		    numRankPattern = new int[89];
		    rankPositionMap = new int[165620];
		    rankIndexMap = new int[165620];
			
		    int[]  sizev= new int[] {6,6,6,6,6,6};
		
		    // Set all rankPatternIndex entries to -1
		    Helper.init_int6(rankPatternIndex, sizev, -1);
	    }
	
	    public override HandInfo HandEval(int[] cards)
	    {
            HandInfo h = new HandInfo();

            ulong pocket = Converter.HandConverter.ConvertToUlong(new int[] { cards[0], cards[1] });
            ulong board = Converter.HandConverter.ConvertToUlong(new int[] { cards[2], cards[3], cards[4], cards[5] });

            h.ehs2 = FO2.FO2.EHS2(pocket, board);
            h.texture = (byte)FO2.FO2.getTurnBoardIndex_Extended(board);
            HoldemHand.Hand.HandPotential(pocket, board, out h.hp, out h.hn);

            return h;
	    }

        public override void HandEval(int[] cards, ref HandInfo existing)
        {
            ulong pocket = Converter.HandConverter.ConvertToUlong(new int[] { cards[0], cards[1] });
            ulong board = Converter.HandConverter.ConvertToUlong(new int[] { cards[2], cards[3], cards[4], cards[5] });           
            HoldemHand.Hand.HandPotential(pocket, board, out existing.hp, out existing.hn);
            if (existing.hp > 1 || existing.hn > 1) throw new Exception("No.");
            //existing.bucket = FO2.FO2.getBucket(111-1, 4, existing.ehs2, existing.hp);
            //existing.bucket_special = FO2.FO2.getBucket(111 * 2-1, 5, existing.ehs2, existing.hp);
        }


        /// <summary>
        /// Rank index of the board [0, 1820-1]
        /// </summary>
        /// <param name="bRank"></param>
        /// <returns></returns>
	    private int boardRankIndex(int[] bRank)
	    {
		    return o[bRank[0]] + m[bRank[1]] + n[bRank[2]] + bRank[3];
	    }

        /// <summary>
        /// Creates a unique index for every rank (hole rank, board rank) combination.
        /// </summary>
        /// <param name="rank"></param>
        /// <returns></returns>
	    public override int HandRankIndex(int[] rank)
	    {
		    int[] hRank = new int [] {rank[0], rank[1]};
		    int[] bRank = new int [] {rank[2], rank[3], rank[4], rank[5]};

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
		    int rankIsoIndex = rankPatternIndex[r[0],r[1],r[2],r[3],r[4],r[5]];

		    // Haven't come upon this rank pattern yet, add it.
		    if (rankIsoIndex == -1) {
			    rankPatternSuits[rankPatternCount] = new TurnSuit();
			    Suits t = rankPatternSuits[rankPatternCount];
			    rankPatternIndex[r[0],r[1],r[2],r[3],r[4],r[5]] = rankPatternCount;

			    t.EnumSuits(r);

			    if (generationDebug) 
                {
				    Helper.printArray("", r);
                    Console.WriteLine("Num suits:" + t.GetSize() + " rankindex:" + rankPatternCount);
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
        			    for (int l=k; l < 13; l++) {
        				    Rank[2] = i;
        				    Rank[3] = j;
        				    Rank[4] = k;
        				    Rank[5] = l;

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
		    int[] Rank = new int[6];

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
            LUT_hp = new Single[tableSize];
            LUT_texture = new byte[tableSize];

		    enumerateHole();

            if (!Helper.load("turnehs_5.dat", ref LUT_ehs2, ref LUT_hp, ref LUT_texture, 1))
                throw new Exception("Error loading turn LUT.");

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

            if (!Helper.load("turnehs_5.dat", ref LUT_ehs2, ref LUT_ev, ref LUT_ev_count,
                                              ref ev_max, ref ev_max_count, ref ev_min, ref ev_min_count,
                                              ref ev_avg, ref ev_avg_count, 1))
            {
                throw new Exception("Error loading turn LUT.");
                dryrun = 0;
                enumerateHole();
            }
        }

        public override void InitializeEmpty()
        {
            enumerateHole();
        }


    }
}

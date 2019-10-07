using System;
using FO2;

namespace Lutv2
{
    public class FlopTable : Table
    {
        // Says to which rank pattern this rank belongs.
        private int[,,,,] rankPatternIndex = new int[5,5,5,5,5];

        public static int[] bRank1 = {0, 12, 23, 33, 42, 50, 57, 63, 68, 72, 75, 77, 78};
        public static int[] bRank2 = {0, 78, 144, 199, 244, 280, 308, 329, 344, 354, 360, 363, 364};

        public int tableSize = 1361802;

        public FlopTable()
        {
            numCards = 5;


            rankPatternSuits = new FlopSuit[36];
            numRankPattern = new int[36];
            rankPositionMap = new int[41405];
            rankIndexMap = new int[41405];

            int[] sizev = new int[] {5, 5, 5, 5, 5};

            // Set all rankPatternIndex entries to -1
            Helper.init_int5(rankPatternIndex, sizev, -1);
        }

        public override HandInfo HandEval(int[] cards)
        {
            HandInfo h = new HandInfo();

            ulong pocket = Converter.HandConverter.ConvertToUlong(new int[] {cards[0], cards[1]});
            ulong board = Converter.HandConverter.ConvertToUlong(new int[] {cards[2], cards[3], cards[4]});

            h.ehs2 = FO2.FO2.EHS2(pocket, board);
            h.texture = (byte)FO2.FO2.getFlopBoardIndex_Extended(board);
            HoldemHand.Hand.HandPotential(pocket, board, out h.hp, out h.hn);

            return h;

            //	return FlopEHS.flopklaatuEHS(cards);
            //return -1;
        }

        public override void HandEval(int[] cards, ref HandInfo existing)
        {
            ulong pocket = Converter.HandConverter.ConvertToUlong(new int[] { cards[0], cards[1] });
            ulong board = Converter.HandConverter.ConvertToUlong(new int[] { cards[2], cards[3], cards[4]});
            HoldemHand.Hand.HandPotential(pocket, board, out existing.hp, out existing.hn);
            if (existing.hp > 1 || existing.hn > 1) throw new Exception("No.");
            //existing.bucket = FO2.FO2.getBucket(147-1, 4, existing.ehs2, existing.hp);
            //existing.bucket_special = FO2.FO2.getBucket(147 * 2-1, 5, existing.ehs2, existing.hp);
        }

       
        /// <summary>
        /// Rank index of the board [0, 454]
        /// </summary>
        /// <param name="bRank"></param>
        /// <returns></returns>
        public static int boardRankIndex(int[] bRank)
        {
            return bRank2[bRank[0]] + bRank1[bRank[1]] + bRank[2];
        }

        
        /// <summary>
        /// Creates index for every rank (hole rank, board rank) combination.
        /// </summary>
        /// <param name="rank"></param>
        /// <returns></returns>
        public override int HandRankIndex(int[] rank)
        {
            int[] hRank = new int[] {rank[0], rank[1]};
            int[] bRank = new int[] {rank[2], rank[3], rank[4]};

            int hridx = HoleRankIndex(hRank);
            int bridx = boardRankIndex(bRank);

            return bridx*91 + hridx;
        }

        private void countRankSuits(int[] Rank)
        {
            int[] r = LowestRank(Rank);
            int rankIsoIndex = rankPatternIndex[r[0], r[1], r[2], r[3], r[4]];

            // Haven't come upon this rank pattern yet, add it.
            if (rankIsoIndex == -1)
            {
                rankPatternSuits[rankPatternCount] = new FlopSuit();
                Suits f = rankPatternSuits[rankPatternCount];
                rankPatternIndex[r[0], r[1], r[2], r[3], r[4]] = rankPatternCount;

                f.EnumSuits(r);

                if (generationDebug)
                {
                    Helper.printArray("", r);
                    Console.WriteLine("Num suits:" + f.GetSize() + " rankindex:" + rankPatternCount);
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
            for (int k = 0; k < 13; k++)
            {
                for (int l = k; l < 13; l++)
                {
                    for (int m = l; m < 13; m++)
                    {
                        Rank[2] = k;
                        Rank[3] = l;
                        Rank[4] = m;

                        // no 5 of a kind please
                        if (Rank[0] == Rank[1] && Rank[1] == Rank[2] && Rank[2] == Rank[4])
                            continue;

                        if (dryrun == 1)
                            countRankSuits(Rank);
                        else
                        {
                            throw new Exception("No.");
                            EnumerateSuits(Rank);
                        }
                    }
                }
            }
        }

        public void enumerateHole()
        {
            int[] Rank = new int[5];

            for (int i = 0; i < 13; i++)
            {
                for (int j = i; j < 13; j++)
                {
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

            //if (!Helper.load("flopehs_5.dat", ref LUT, 1))
            if (!Helper.load("flopehs_5.dat", ref LUT_ehs2, ref LUT_hp, ref LUT_texture, 1))
            {
                throw new Exception("Error loading flop LUT.");
                //dryrun = 0;
                //enumerateHole();


                // wait for the threads to finish their work (1 can be in progress)
                //			waitForThreads();

                // now write to disk
                //Helper.save("flopehs.dat", LUT);
            }

        }


        public void InstantiateEVTables()
        {
            LUT_ev = new Single[tableSize];
            LUT_ev_count = new uint[tableSize];
        }

        public override void InitializeTableEV()
        {
            LUT_ehs2 = new Single[tableSize];
            LUT_ev = new Single[tableSize];
            LUT_ev_count = new uint[tableSize];

            enumerateHole();

            if (!Helper.load("flopehs_5.dat", ref LUT_ehs2, ref LUT_ev, ref LUT_ev_count,
                                              ref ev_max, ref ev_max_count, ref ev_min, ref ev_min_count,
                                              ref ev_avg, ref ev_avg_count, 1))
            {
                throw new Exception("Error loading flop LUT.");
            }

        }

        public override void InitializeEmpty()
        {
            enumerateHole();
        }

    }
}

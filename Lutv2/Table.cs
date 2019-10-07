using System;
using SimmoTech.Utils.Serialization;

namespace Lutv2
{
    public abstract class Table
    {
        #region Fields

        /// <summary>
        /// dryrun=1 means only counting, dryrun=0 generates actual tables.
        /// </summary>
        protected int dryrun = 1;
        
        /// <summary>
        /// number of entries done
        /// </summary>
        protected int count = 0;


        //[Serializable]
        //public struct HandInfo
        //{
        //    public double ehs2;
        //    public byte texture;
        //};

        [Serializable]
        public class HandInfo : IOwnedDataSerializableAndRecreatable
        {
            public byte texture;
            public double ehs2;
            public Single ev;
            public double hp;
            public double hn;

            public HandInfo()
            {
            }

            public void SerializeOwnedData(SerializationWriter writer, object context)
            {
                if ((int)context == 1)
                {
                    Single ehs2_s = Convert.ToSingle(ehs2);
                    writer.Write(ehs2_s);
                }
                else
                {
                    writer.Write(texture);
                    writer.Write(ehs2);
                    writer.Write(ev);
                    writer.Write(hp);
                    writer.Write(hn);
                }
            }

            public void DeserializeOwnedData(SerializationReader reader, object context)
            {
                if ((int)context == 2)
                {
                    ehs2 = Convert.ToDouble(reader.ReadSingle());
                }
                else
                {
                    texture = reader.ReadByte();
                    ehs2 = reader.ReadDouble();
                    ev = reader.ReadUInt32();
                    if ((int)context >= 1)
                    {
                        hp = reader.ReadDouble();
                        hn = reader.ReadDouble();
                    }
                }
            }
        }

        /// <summary>
        /// whether to give out extra debug info
        /// </summary>
        protected bool generationDebug = false;
        protected int numCards = -1;

        public double ev_min = -0.0001;
        public double ev_max = 0.0001;
        public ulong ev_min_count = 0;
        public ulong ev_max_count = 0;
        public double ev_avg = 0;
        public ulong ev_avg_count = 0;

        public Single[] LUT_ehs2 = null;
        public Single[] LUT_hp = null;
        public Single[] LUT_ev = null;
        public uint[] LUT_ev_count = null;
        public byte[] LUT_texture = null;


        /// <summary>
        /// The different suit patterns matching to each rank pattern. [number of rank patterns]
        /// </summary>
        protected Suits[] rankPatternSuits;
   
       /// <summary>
       /// how many ranks that compress into a certain rank pattern [number of rank patterns]
       /// </summary>
        protected int[] numRankPattern;
        
        /// <summary>
        /// Gives each rank a unique position within the rank pattern it belongs to [number of ranks]
        /// </summary>
        protected int[] rankPositionMap;
        
        /// <summary>
        /// Says to which rank pattern index this rank belongs to [number of ranks]
        /// </summary>
        protected int[] rankIndexMap;
        
        /// <summary>
        /// The current smallest un-used rank pattern index
        /// </summary>
        protected int rankPatternCount = 0;
        
        /// <summary>
        /// Cumulative sum of offsets (see tableIndex)
        /// </summary>
        private int[] cumOffsets;

        /// <summary>
        /// 0..90
        /// </summary>
        private static readonly int[,] holeRankIndex = new int[,]
                                                  {
                                                      {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12},
                                                      {1, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24},
                                                      {2, 14, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35},
                                                      {3, 15, 26, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45},
                                                      {4, 16, 27, 37, 46, 47, 48, 49, 50, 51, 52, 53, 54},
                                                      {5, 17, 28, 38, 47, 55, 56, 57, 58, 59, 60, 61, 62},
                                                      {6, 18, 29, 39, 48, 56, 63, 64, 65, 66, 67, 68, 69},
                                                      {7, 19, 30, 40, 49, 57, 64, 70, 71, 72, 73, 74, 75},
                                                      {8, 20, 31, 41, 50, 58, 65, 71, 76, 77, 78, 79, 80},
                                                      {9, 21, 32, 42, 51, 59, 66, 72, 77, 81, 82, 83, 84},
                                                      {10, 22, 33, 43, 52, 60, 67, 73, 78, 82, 85, 86, 87},
                                                      {11, 23, 34, 44, 53, 61, 68, 74, 79, 83, 86, 88, 89},
                                                      {12, 24, 35, 45, 54, 62, 69, 75, 80, 84, 87, 89, 90}
                                                  };

        #endregion

        #region Methods

        public abstract int HandRankIndex(int[] rank);
        public abstract HandInfo HandEval(int[] cards);
        public abstract void HandEval(int[] cards, ref HandInfo existing);
        public abstract void InitializeTable();
        public abstract void InitializeTableEV();
        public abstract void InitializeEmpty();

        /// <summary>
        /// sort hole cards, sort board cards and put into Rank and Suit
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="rank"></param>
        /// <param name="suit"></param>
        private void getSortedRankSuits(int[] cards, int[] rank, int[] suit)
        {
            int[] hole = new int[] {cards[0], cards[1]};
            int[] board = new int[cards.Length - 2];
            Array.Copy(cards, 2, board, 0, cards.Length - 2);

            Array.Sort(board);
            Array.Sort(hole);

            for (int i = 0; i < 2; i++)
            {
                rank[i] = hole[i]/4;
                suit[i] = hole[i]%4;
            }

            for (int i = 2; i < cards.Length; i++)
            {
                rank[i] = board[i - 2]/4;
                suit[i] = board[i - 2]%4;
            }
        }

        /// <summary>
        /// Entry for looking up a hand. First two cards are hole rest board.
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public HandInfo LookupOne(int[] cards)
        {
            int[] Rank = new int[cards.Length];
            int[] Suit = new int[cards.Length];
           
            getSortedRankSuits(cards, Rank, Suit);
            //Helper.printArray("Rank", Rank);
            //Helper.printArray("Suit", Suit);

            int finalindex = TableIndex(Rank, Suit);          

            HandInfo hi = new HandInfo();

            hi.ehs2 = LUT_ehs2[finalindex];
            if (LUT_hp != null) hi.hp = LUT_hp[finalindex];
            hi.texture = LUT_texture[finalindex];

            hi.hn = (double)finalindex;

            return hi;
        }

        /// <summary>
        /// Just get the index for a hand.
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public int getHandIndex(int[] cards)
        {
            int[] Rank = new int[cards.Length];
            int[] Suit = new int[cards.Length];

            getSortedRankSuits(cards, Rank, Suit);

            int finalindex = TableIndex(Rank, Suit);

            return finalindex;
        }

        /// <summary>
        /// Generates a sorted deck except for SORTED cards in deadcards.
        /// </summary>
        /// <param name="deadcards"></param>
        /// <returns></returns>
        private int[] getDeck(int[] deadcards)
        {
            int ndeadcards = deadcards.Length;
            int di = 0;
            int[] deck = new int[52 - ndeadcards];

            Array.Sort(deadcards);

            for (int c = 0, idx = 0; c < 52; c++)
            {
                if (di < ndeadcards && c == deadcards[di])
                {
                    di++;
                    continue;
                }

                deck[idx] = c;
                idx++;
            }

            return deck;
        }

        /// <summary>
        /// Index of the hand into [0, nchoosek(52,2)-1]
        /// see http://en.wikipedia.org/wiki/Combinadic
        /// hole1 < hole2
        /// </summary>
        /// <param name="hole1"></param>
        /// <param name="hole2"></param>
        /// <returns></returns>
        private int holeIndex(int hole1, int hole2)
        {
            return Helper.nchoosek(hole2, 2) + Helper.nchoosek(hole1, 1);
        }

        ///// <summary>
        ///// Looks up all hole card measures for a certain board
        ///// </summary>
        ///// <param name="board"></param>
        ///// <returns></returns>
        //public HandInfo[] LookupAll(int[] board)
        //{
        //    int ncards = board.Length + 2;
        //    int[] Rank = new int[ncards];
        //    int[] Suit = new int[ncards];
        //    HandInfo[] values = new HandInfo[1326];

        //    // -1 for impossible hole cards
        //    //for (int i = 0; i < values.Length; i++)
        //    //    values[i] = null;

        //    Array.Sort(board);
        //    for (int i = 0; i < board.Length; i++)
        //    {
        //        Rank[i + 2] = board[i]/4;
        //        Suit[i + 2] = board[i]%4;
        //    }

        //    int[] deck = getDeck(board);
        //    for (int i = 0; i < deck.Length; i++)
        //    {
        //        for (int j = i + 1; j < deck.Length; j++)
        //        {
        //            Rank[0] = deck[i]/4;
        //            Suit[0] = deck[i]%4;
        //            Rank[1] = deck[j]/4;
        //            Suit[1] = deck[j]%4;

        //            int holeindex = holeIndex(deck[i], deck[j]);

        //            values[holeindex] = LUT[TableIndex(Rank, Suit)];
        //        }
        //    }

        //    return values;
        //}

        /// <summary>
        /// Build offset table
        /// </summary>
        private void CountOffsets()
        {
            cumOffsets = new int[numRankPattern.Length];

            for (int i = 1; i < numRankPattern.Length; i++)
            {
                cumOffsets[i] = cumOffsets[i - 1] + numRankPattern[i - 1]*rankPatternSuits[i - 1].GetSize();
            }
        }

        private int TableIndex(int[] rank, int[] suit)
        {
            int rankidx = HandRankIndex(rank);
            int rankIsoIndex = rankIndexMap[rankidx];
            int suitIndex = rankPatternSuits[rankIsoIndex].GetPatternIndex(suit);
          
            // useful for debugging.
            //		int suitPattern [] = rankPatternSuits[rankIsoIndex].getPattern(suitIndex);
            //		Helper.printArray("suitPattern", suitPattern);
            //		System.out.println("suitIndex" + suitIndex);

            if (cumOffsets == null) CountOffsets();

            int index = cumOffsets[rankIsoIndex] + rankPositionMap[rankidx] * rankPatternSuits[rankIsoIndex].GetSize() + suitIndex;

            return index;
        }

        ///// <summary>
        ///// Common for each generation of [street]Table
        ///// </summary>
        ///// <param name="rank"></param>
        ///// <param name="suitpattern"></param>
        //private void FillTable(int[] rank, int[] suitpattern)
        //{
        //    int idx = TableIndex(rank, suitpattern);

        //    // Generates one hand
        //    int[] cards = Helper.getHand(rank, suitpattern);

        //    // some info of how it's doing since it takes a very long time.
        //    count++;

        //    if (LUT[idx] == null)
        //    {
        //        LUT[idx] = HandEval(cards);
        //        if (count % 1000 == 0)
        //            Console.WriteLine("Finished (new) " + count + " / " + LUT.Length);
        //    }
        //    else
        //    {
        //        HandEval(cards, ref LUT[idx]);
        //        if (count % 100 == 0)
        //            Console.WriteLine("Finished (existing) " + count + " / " + LUT.Length);
        //    }
            
        //}

        /// <summary>
        /// Common for each generation of [street]Table
        /// </summary>
        /// <param name="Rank"></param>
        public void EnumerateSuits(int[] Rank)
        {
            int rankidx = HandRankIndex(Rank);

            // index of this rank pattern
            int rankIsoIndex = rankIndexMap[rankidx];

            // These are the suit patterns belonging to this rank pattern
            Suits f = rankPatternSuits[rankIsoIndex];

            // For each suit pattern belonging to this rank pattern
            for (int i = 0; i < f.GetSize(); i++)
            {
                //FillTable(Rank, f.GetPattern(i));
            }
        }

        public void Initialize()
        {

            InitializeTable();

            CountOffsets();
           
            //System.gc();
        }

        public void InitializeEV()
        {

            InitializeTableEV();

            CountOffsets();

            //System.gc();
        }

        public void DebugInfo()
        {
            int s = 0;

            for (int i = 0; i < rankPatternCount; i++)
            {
                s += numRankPattern[i]*rankPatternSuits[i].GetSize();
            }

            //long mem0 = Runtime.getRuntime().totalMemory() - Runtime.getRuntime().freeMemory();
            //System.out.println("Mem used:" + mem0 / 1048576 + " MB");

            //System.out.println("Size:" + s);
        }

        /// <summary>
        /// Tests the lookup table wrt. the measure function
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public int TestLUT(int n)
        {
            int tested = 0;

            for (int i = 0; i < n; i++)
            {
                int[] cards = Helper.randomHand(numCards);

                HandInfo lookup = LookupOne(cards);
                HandInfo direct = HandEval(cards);

                //if (Math.Abs(lookup.ehs2 - direct.ehs2) > 0.00001 || lookup.texture != direct.texture)
                //{
                //    Helper.printArray("Bad lookup in the LUT:", cards);
                //    Console.WriteLine("direct: " + direct.ehs2 + ", " + direct.texture + " and lookupOne: " + lookup.ehs2 + ", " + lookup.texture);
                //}

                if (Math.Abs(lookup.hp - direct.hp) > 0.00001 || Math.Abs(lookup.ehs2 - direct.ehs2) > 0.00001 || lookup.ev != direct.ev)
                {
                    Helper.printArray("Bad lookup in the LUT:", cards);
                    Console.WriteLine("direct: " + direct.hp + ", " + direct.ehs2 +
                                      " and lookupOne: " + lookup.hp + ", " + lookup.ehs2);
                }

                tested++;
            }
            return tested;
        }
        
        /// <summary>
        /// Returns the isomorphic lowest rank pattern.
        /// </summary>
        /// <param name="ranks"></param>
        /// <returns></returns>
        public int[] LowestRank(int[] ranks)
        {
            int[] map = new int[] {-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1};
            int[] isoRank = new int[ranks.Length];

            int currentrank = 0;

            for (int i = 0; i < ranks.Length; i++)
            {
                if (map[ranks[i]] == -1)
                {
                    map[ranks[i]] = currentrank;
                    currentrank++;
                }
                isoRank[i] = map[ranks[i]];
            }
            return isoRank;
        }

        /// <summary>
        /// Hole rank index.
        /// </summary>
        /// <param name="hRank"></param>
        /// <returns></returns>
        public int HoleRankIndex(int[] hRank)
        {
            return holeRankIndex[hRank[0], hRank[1]];
        }


        /// <summary>
        /// Update EV LUT
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public void UpdateLUT_ev(Single ev, int lut_index, bool is_lower_bound, bool is_upper_bound)
        {
            if (Single.IsNaN(ev) || Single.IsInfinity(ev)) return;


            const double ev_weight = 1 / 10000;
            const double bound_weight = 1 / 10000;

            LUT_ev_count[lut_index]++;

            //double weight = Math.Max(1 / LUT_ev_count[lut_index], min_weight);
            //LUT_ev[lut_index] = Convert.ToSingle((1 - weight) * LUT_ev[lut_index] + (weight * ev));

            if (ev_min_count > 0 && ev_max_count > 0)
                LUT_ev[lut_index] =
                    Convert.ToSingle((1 - ev_weight) * LUT_ev[lut_index] + (ev_weight * ((ev - ev_min) / (ev_max - ev_min))));

            ev_avg += ev;
            ev_avg_count++;

            if (is_lower_bound)
            {
                ev_min_count++;
                double weight = Math.Max(1 / ev_min_count, bound_weight);
                ev_min = (1 - weight) * ev_min + (weight * ev);
            }

            if (is_upper_bound)
            {
                ev_max_count++;
                double weight = Math.Max(1 / ev_max_count, bound_weight);
                ev_max = (1 - weight) * ev_max + (weight * ev);
            }

        }

        public void ResetLUT_ev()
        {

            for (int i = 0; i < LUT_ev.Length; i++)
            {
                LUT_ev[i] = 0;
                LUT_ev_count[i] = 0;
            }

            ev_max = 0.001;
            ev_min = -0.001;
            ev_max_count = 0;
            ev_min_count = 0;
            ev_avg = 0;
            ev_avg_count = 0;
        }

        public double getNormalizedEHS2_Flop(double ehs2, int texture)
        {
            double[] mins = {0.0995603576302528, 0.0615814402699471, 0.0506691634654999, 0.0614949390292168, 0.0520584955811501, 0.0539553724229336, 0.0566428862512112, 0.0562749989330769, 0.0592361427843571, 0.0646036863327026, 0.0927125737071037, 0.0570261180400848, 0.0468424446880817, 0.0568750984966755, 0.0481426864862442, 0.0499274097383022, 0.0523430332541466, 0.0521218031644821, 0.054796788841486, 0.0598554946482182, 0.0377944223582745};
            return (ehs2 - mins[texture]) / (1 - mins[texture]);
        }

        public double getNormalizedEHS2_Turn(double ehs2, int texture)
        {
            double[] mins = {0.0269480925053358, 0.0197460111230612, 0.0197460111230612, 0.0197460111230612, 0.0197460111230612, 0.0202075429260731, 0.0202075429260731, 0.0210885535925627, 0.0221790373325348, 0.0240512508898973, 0.0199382938444614, 0.0134303513914347, 0.0134303513914347, 0.0134303513914347, 0.013419970870018, 0.0137596651911736, 0.0137596651911736, 0.0143258394673467, 0.015062166377902, 0.0163450054824352, 0.0289840660989285};
            return (ehs2 - mins[texture]) / (1 - mins[texture]);
        }

        public void ResetToEHS2LUT_ev()
        {

            for (int i = 0; i < LUT_ev.Length; i++)
            {
                LUT_ev[i] = LUT_ehs2[i];
                LUT_ev_count[i] = 0;
            }

            ev_max = 520;
            ev_min = -520;
            ev_max_count = 0;
            ev_min_count = 0;
            ev_avg = 0;
            ev_avg_count = 0;
        }

        public void ResetToEHS2NormalizedLUT_ev(int round)
        {

            if (round == 1)
            {
                for (int i = 0; i < LUT_ev.Length; i++)
                {
                    LUT_ev[i] = Convert.ToSingle(getNormalizedEHS2_Flop(LUT_ehs2[i], LUT_texture[i]));
                    LUT_ev_count[i] = 0;
                }
            }
            else if (round == 2)
            {
                for (int i = 0; i < LUT_ev.Length; i++)
                {
                    LUT_ev[i] = Convert.ToSingle(getNormalizedEHS2_Turn(LUT_ehs2[i], LUT_texture[i]));
                    LUT_ev_count[i] = 0;
                }
            }
            else 
            {
                for (int i = 0; i < LUT_ev.Length; i++)
                {
                    LUT_ev[i] = LUT_ehs2[i];
                    LUT_ev_count[i] = 0;
                }
            }


            ev_max = 100;
            ev_min = -100;
            ev_max_count = 0;
            ev_min_count = 0;
            ev_avg = 0;
            ev_avg_count = 0;
        }

        #endregion
    }
}

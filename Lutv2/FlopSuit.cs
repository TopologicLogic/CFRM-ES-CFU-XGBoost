
namespace Lutv2
{
    public class FlopSuit : Suits
    {
        private int[,,,,] suitMap = new int[4,4,4,4,4];
        private int isoSuitIndex = 0;
        private static int[] sameBoard = new int[2*1140];

        public FlopSuit()
        {
            int[] sizev = new int[] {4, 4, 4, 4, 4};

            // just set all suitMap entries to -1
            Helper.init_int5(suitMap, sizev, -1);

            for (int i = 0; i < sameBoard.Length; i++)
                sameBoard[i] = -1;
        }

        public override int GetPatternIndex(int[] p)
        {
            return suitMap[p[0], p[1], p[2], p[3], p[4]];
        }

        public override void EnumSuits(int[] rank)
        {
            int[] suits = new int[5];

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    for (int k = 0; k < 4; k++)
                        for (int l = 0; l < 4; l++)
                            for (int m = 0; m < 4; m++)
                            {
                                suits[0] = i;
                                suits[1] = j;
                                suits[2] = k;
                                suits[3] = l;
                                suits[4] = m;

                                addSuit(rank, suits);
                            }
        }

        private int sameBoardIndex(int[] ranks, int[] suits)
        {
            int[] cards = Helper.sortedIsoBoard(ranks, suits);
            int suited = (suits[0] == suits[1]) ? 1 : 0;

            int hidx = 0;
            for (int i = 0; i < 3; i++)
            {
                hidx += Helper.nchoosek(cards[i], i + 1);
            }

            // different if suited hole or not
            hidx += suited*1140;

            return hidx;
        }

        private int SameBoard(int[] ranks, int[] suits)
        {
            int hidx = sameBoardIndex(ranks, suits);

            return sameBoard[hidx];
        }

        private void addSameHand(int[] ranks, int[] suits, int index)
        {
            int hidx = sameBoardIndex(ranks, suits);

            sameBoard[hidx] = index;
        }

        private int getSuitMapIndex(int[] s)
        {
            return suitMap[s[0], s[1], s[2], s[3], s[4]];
        }

        private void setSuitMapIndex(int[] s, int index)
        {
            suitMap[s[0], s[1], s[2], s[3], s[4]] = index;
        }

        /// <summary>
        /// Pick out the suit patterns which are interesting wrt. the rank pattern.
        /// </summary>
        /// <param name="Rank"></param>
        /// <param name="suits"></param>
        private void addSuit(int[] Rank, int[] suits)
        {
            int[] isuit = LowestSuit(suits);

            // See if suit pattern even works on this board.
            if (!Helper.isoHandCheck(Rank, isuit))
            {
                return;
            }

            // If this board has been seen before.
            int seenBoardIndex = SameBoard(Rank, isuit);
            if (seenBoardIndex > -1)
            {
                setSuitMapIndex(suits, seenBoardIndex);
                return;
            }

            int lowSuitIndex = getSuitMapIndex(isuit);
            // we haven't come across this suit pattern yet
            if (lowSuitIndex == -1)
            {
                setSuitMapIndex(isuit, isoSuitIndex);
                setSuitMapIndex(suits, isoSuitIndex);

                addSameHand(Rank, isuit, isoSuitIndex);

                patterns.Add(isuit);
                isoSuitIndex++;
            }
            else
            {
                setSuitMapIndex(suits, lowSuitIndex);
            }
        }

    }
}

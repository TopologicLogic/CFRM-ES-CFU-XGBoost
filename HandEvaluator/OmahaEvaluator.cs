using System;
using System.Collections.Generic;
using System.Text;

namespace HoldemHand
{
    
    public class OmahaEvaluator
    {
        List<OmahaHand> fourcards = new List<OmahaHand>(270725);
        int[] validbits = new int[249];
        int[,] lowhands = new int[182, 241];

        public OmahaEvaluator()
        {
            for (int a = 0; a < 49; a++)
                for (int b = a + 1; b < 50; b++)
                    for (int c = b + 1; c < 51; c++)
                        for (int d = c + 1; d < 52; d++)
                            fourcards.Add(new OmahaHand((1UL << a), (1UL << b), (1UL << c), (1UL << d)));
            fourcards.Sort();

            int cnt = 0;
            for (ulong i = 0; i < 249; i++)
                if (Hand.BitCount(i) >= 3 && Hand.BitCount(i) < 6)
                {
                    validbits[i] = cnt;
                    for (ulong j = 0; j <= 240; j++)
                    {
                        if (Hand.BitCount(j) <= 4 && Hand.BitCount(j) >= 2 && Hand.BitCount(j | i) >= 5)
                        {
                            ulong h = j, t = i, b = 128;
                            while (Hand.BitCount(t | h) != 5)
                            {
                                if ((h & b) == b)
                                    h -= b;
                                b >>= 1;
                            }
                            lowhands[cnt, (int)j] = (int)(t | h);
                        }
                        else
                            lowhands[cnt, (int)j] = -1;
                    }
                    cnt++;
                }
                else
                    validbits[i] = -1;
        }

        public string DescriptionFromMask(ulong hand, ulong table)
        {
            OmahaHand h = fourcards[(fourcards.BinarySearch(new OmahaHand(hand)))];
            uint besthand = 0;
            int idx = -1;
            for (int i = 0; i < 6; i++)
            {
                uint currenthand = Hand.Evaluate(h.hands[i] | table);
                if (currenthand > besthand)
                {
                    idx = i;
                    besthand = currenthand;
                }
            }
            return Hand.DescriptionFromMask(h.hands[idx] | table);
        }

        public uint EvaluateHigh(ulong hand, ulong table)
        {
            OmahaHand h = fourcards[(fourcards.BinarySearch(new OmahaHand(hand)))];
            uint besthand = 0;
            for (int i = 0; i < 6; i++)
            {
                uint currenthand = Hand.Evaluate(h.hands[i] | table);
                if (currenthand > besthand)
                    besthand = currenthand;
            }
            return besthand;
        }

        public int EvaluateLow(ulong hand, ulong table)
        {
            // Move down all aces to bit 0 and 2... up to bit 1
            hand = (((hand >> 13) | (hand >> 26) | (hand >> 39) | hand) & 4223) << 1;
            hand = ((hand & 254) | (hand >> 13)) & 255;
            table = (((table >> 13) | (table >> 26) | (table >> 39) | table) & 4223) << 1;
            table = ((table & 254) | (table >> 13)) & 255;
            // Get valid bitmasks for low hand
            int validbitmask = validbits[table];
            // Less than three lows,no possible combo of a low hand
            if (validbitmask == -1) return -1;
            // Get the low cards on hand
            return lowhands[validbitmask, (int)hand];
        }
    }
    ///
    /// Represents a omaha-hand
    ///
    class OmahaHand : IComparable<OmahaHand>
    {
        public ulong cards;
        public ulong[] hands;
        public OmahaHand(ulong crds)
        {
            cards = crds;
            hands = new ulong[6];
        }
        public OmahaHand(ulong c1, ulong c2, ulong c3, ulong c4)
        {
            hands = new ulong[6];
            cards = c1 | c2 | c3 | c4;
            hands[0] = c1 | c2;
            hands[1] = c1 | c3;
            hands[2] = c1 | c4;
            hands[3] = c2 | c3;
            hands[4] = c2 | c4;
            hands[5] = c3 | c4;
        }
        #region IComparable Members
        public int CompareTo(OmahaHand other)
        {

            if (cards > other.cards)
                return 1;
            if (cards == other.cards)
                return 0;
            // if (cards < other.cards)
            return -1;
        }
        #endregion
    }


    //Example Usage:

    //OmahaEvaluator ev = new OmahaEvaluator();
    //ulong hand = Hand.ParseHand("ac kc 2h ah");
    //ulong table = Hand.ParseHand("6c 4c 3c");
    //// Lower value means better hand
    //int lo = ev.EvaluateLow(hand, table);
    //if (lo == -1)
    //Console.WriteLine("No low hand");

    //Console.WriteLine(ev.DescriptionFromMask(hand, table));
}

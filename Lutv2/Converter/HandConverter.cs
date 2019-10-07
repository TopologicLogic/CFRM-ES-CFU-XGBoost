using System;
using System.Collections.Generic;

namespace Lutv2.Converter
{
    public static class HandConverter
    {
        public static int[] ConvertToIntArray(ulong cards)
        {
            List<int> cardsResult = new List<int>();

            int iteratorCount = (int) Math.Log(cards, 2) + 1;

            for (int i = 0; i < iteratorCount; i++)
            {
                if((cards >> i) %2==1)
                {
                    int cardRank = i%13;
                    int cardSuit = i/13;
                    cardsResult.Add(cardSuit + cardRank*4);
                }
            }

            return cardsResult.ToArray();
        }

        public static ulong ConvertToUlong(int[] cards)
        {
            ulong cardsResult = 0;

            foreach (int card in cards)
            {
                int cardRank = card/4;
                int cardSuit = card%4;

                int cardResult = cardRank + cardSuit*13;

                cardsResult |= 1UL << cardResult;
            }

            return cardsResult;
        }
    }




}

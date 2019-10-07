using System.Collections.Generic;

namespace Lutv2
{
    public abstract class Suits
    {
        #region Fields
        /// <summary>
        /// The list of suit patterns we see are the only suit patterns mattering
        /// with respect to this rank pattern.  Only used during enumeration, not lookup.
        /// </summary>
        protected List<int[]> patterns = new List<int[]>();
        #endregion 
        
        #region Methods
        public abstract int GetPatternIndex(int[] p);
        public abstract void EnumSuits(int[] rank);
        public int GetSize()
        {
            return patterns.Count;
        }
        public int[] GetPattern(int i)
        {
            return patterns[i];
        }
        /// <summary>
        /// Returns the isomorphic lowest suit pattern.
        /// </summary>
        /// <param name="suits"></param>
        /// <returns></returns>
        public static int[] LowestSuit(int[] suits)
        {
            int[] map = new int[] { -1, -1, -1, -1 };
            int[] isoSuit = new int[suits.Length];

            int currentsuit = 0;

            for (int i = 0; i < suits.Length; i++)
            {
                if (map[suits[i]] == -1)
                {
                    map[suits[i]] = currentsuit;
                    currentsuit++;
                }
                isoSuit[i] = map[suits[i]];
            }
            return isoSuit;
        }   
        /// <summary>
        /// Returns vector with number of suits of each type.
        /// </summary>
        /// <param name="suits"></param>
        /// <returns></returns>
        public static int[] SuitCount(int[] suits)
        {
            int[] count = new int[4];

            for (int i = 0; i < suits.Length; i++)
            {
                count[suits[i]]++;
            }

            return count;
        }
        #endregion
    }
}

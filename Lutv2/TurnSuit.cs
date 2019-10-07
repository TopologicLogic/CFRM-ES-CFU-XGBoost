
namespace Lutv2
{
    public class TurnSuit:Suits
    {
	    private int[,,,,,] suitMap = new int[4,4,4,4,4,4];
	    private int isoSuitIndex = 0;

        // Suits 0..3, Ranks 0..5, 6 cards, max card index 5*4+3
	    // see http://en.wikipedia.org/wiki/Combinadic
	    private static int[] sameHand = new int[2*10626];

	    public TurnSuit()
	    {
		    int[] sizev = new int[] {4,4,4,4,4,4};

		    Helper.init_int6(suitMap, sizev, -1);

		    for (int i=0; i < sameHand.Length; i++)
			    sameHand[i] = -1;
	    }

	    public override int GetPatternIndex(int[] p)
	    {
		    return suitMap[p[0],p[1],p[2],p[3],p[4],p[5]];
	    }

	    private int sameHandIndex(int[] ranks, int[] suits)
	    {
		    int [] cards = Helper.sortedIsoBoard(ranks, suits);		
		    int suited = (suits[0] == suits[1]) ? 1 : 0;
		
		    int hidx = 0;
		    for (int i=0; i < 4; i++) {
			    hidx += Helper.nchoosek(cards[i], i+1);
		    }
		
		    hidx += suited * 10626;
		
		    return hidx;
	    }

	    private int sameBoard(int[] ranks, int[] suits)
	    {
		    int hidx = sameHandIndex(ranks, suits);

		    return sameHand[hidx];
	    }
	
	    private void addSameBoard(int[] ranks, int[] suits, int index)
	    {
		    int hidx = sameHandIndex(ranks, suits);
		
		    sameHand[hidx] = index;
	    }

	    private int getSuitMapIndex(int[] s)
	    {
		    return suitMap[s[0],s[1],s[2],s[3],s[4],s[5]];
	    }
	
	    private void setSuitMapIndex(int[] s, int index)
	    {
		    suitMap[s[0],s[1],s[2],s[3],s[4],s[5]] = index;
	    }
	
	    private void addSuit(int[] Rank, int[] suits)
	    {
		    int[] isuit = LowestSuit(suits);

		    if (!Helper.isoHandCheck(Rank, isuit)) 
            {
			    return;
		    }

		    int seenHandIndex = sameBoard(Rank, isuit);
		    if (seenHandIndex > -1) 
            {
			    setSuitMapIndex(suits, seenHandIndex);
			    return;
		    }

		    int lowSuitIndex = getSuitMapIndex(isuit);
		    // we haven't come across this suit iso pattern yet
		    if (lowSuitIndex == -1) 
            {
			    setSuitMapIndex(isuit, isoSuitIndex);
			    setSuitMapIndex(suits, isoSuitIndex);

			    addSameBoard(Rank, isuit, isoSuitIndex);
			
			    patterns.Add(isuit);
			    isoSuitIndex++;
		    } 
            else 
            {
			    setSuitMapIndex(suits, lowSuitIndex);
		    }
	    }
	
        /// <summary>
        /// Enumerate the all suits
        /// </summary>
        /// <param name="rank"></param>
	    public override void EnumSuits(int [] rank)
	    {
		    int[] suits = new int [6];

		    for (int i=0; i < 4; i++)
			    for (int j=0; j < 4; j++)
				    for (int k=0; k < 4; k++)
					    for (int l=0; l < 4; l++)
						    for (int m=0; m < 4; m++)
							    for (int n=0; n < 4; n++) {
								    suits[0] = i;
								    suits[1] = j;
								    suits[2] = k;
								    suits[3] = l;
								    suits[4] = m;
								    suits[5] = n;
								
								    addSuit(rank, suits);
							    }
	    }
    }
}

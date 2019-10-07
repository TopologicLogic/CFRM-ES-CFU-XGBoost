using System;
using FO2;

namespace Lutv2
{
    public class PreFlopTable : Table
    {
        public int tableSize = 169;

        public PreFlopTable()
        {
            LUT_ev = new Single[tableSize];
        }

        public override HandInfo HandEval(int[] cards)
        {
            return null;
        }

        public override void HandEval(int[] cards, ref HandInfo existing)
        {

        }

        public override int HandRankIndex(int[] rank)
        {
            return 0;
        }

        public override void InitializeTable()
        {
            ////if (!Helper.load("flopehs_5.dat", ref LUT, 1))
            //if (!Helper.load("flopehs_5.dat", ref LUT_ehs2, ref LUT_hp, ref LUT_ev, ref LUT_texture, 1))
            //{
            //    throw new Exception("Error loading preflop LUT.");
            //}
        }

        public override void InitializeTableEV()
        {
            ////if (!Helper.load("flopehs_5.dat", ref LUT, 1))
            //if (!Helper.load("flopehs_5.dat", ref LUT_ehs2, ref LUT_hp, ref LUT_ev, ref LUT_texture, 1))
            //{
            //    throw new Exception("Error loading preflop LUT.");
            //}
        }

        public override void InitializeEmpty()
        {

        }


    }
}
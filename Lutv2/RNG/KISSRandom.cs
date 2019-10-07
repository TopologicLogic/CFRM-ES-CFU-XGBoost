using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Lutv2.RNG
{
	public class KISSRandom
	{
		static RNGCryptoServiceProvider cryptoRng;

		static uint SecureRandomUInt()
		{
			byte[] b = new byte[4];
			cryptoRng.GetBytes(b);
			return (uint)BitConverter.ToInt32(b, 0);
		}

		static uint SecureRandomNonZeroUInt()
		{
			uint x;
			do { x = SecureRandomUInt(); } while (x == 0);
			return x;
		}

		uint x, y, z, c;
		
		public KISSRandom()
		{
			lock (typeof(KISSRandom))
			{
				if (cryptoRng == null) 
					cryptoRng = new RNGCryptoServiceProvider();
				x = SecureRandomUInt();
				y = SecureRandomNonZeroUInt();
				z = SecureRandomUInt();
				c = SecureRandomUInt() & 0x1fffffff;
			}
		}

		public uint RandomUInt()
		{
			long t, a=698769069L;
			x=69069*x+12345;
			y^=(y<<13); y^=(y>>17); y^=(y<<5);
			t=a*z+c; c=(uint)(t>>32);
			return x+y+(z=(uint)t);
		}

        public int RandomBit() { return (int)(RandomUInt() >> 31); }
		public int RandomInt31() { return (int)(RandomUInt() >> 1); }
		public double RandomDoubleClosed() { return RandomUInt() * (1.0 / 4294967295.0); }                                               // [0,1]
		public double RandomDoubleLeftClosed() { return RandomUInt() * (1.0 / 4294967296.0); }                                       // [0,1)
		public double RandomDoubleOpen() { return (((double)RandomUInt()) + 0.5) * (1.0 / 4294967296.0); }             // (0,1)
		public int RandomInt(int range) { return (int)(RandomDoubleLeftClosed() * range); }

		public double NextDouble() { return RandomDoubleLeftClosed(); }
		public int Next(int range) { return RandomInt(range); }
	}
}

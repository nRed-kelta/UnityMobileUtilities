// nRed Unity Mobile Utilities
// Copyright (c) nRed-kelta. For terms of use, see
// https://github.com/nRed-kelta/UnityMobileUtilities

using UnityEngine;

namespace nRed.MathUtil
{
    public static class MathUtility
    {
		public static byte Abs( this byte self )
        {
			return (byte)(self<0 ? -self : self);
		}
		public static short Abs( this short self )
        {
			return (short)(self<0 ? -self : self);
		}
		public static int Abs( this int self )
        {
			return self<0 ? -self : self;
		}
		public static long Abs( this long self )
        {
			return self<0 ? -self : self;
		}
        public static float Abs( this float self )
        {
			return self<0 ? -self : self;
		}
		public static double Abs( this double self )
        {
			return self<0 ? -self : self;
		}
		
    }
}
// nRed Unity Mobile Utilities
// Copyright (c) nRed-kelta. For terms of use, see
// https://github.com/nRed-kelta/UnityMobileUtilities

using System.Collections;
using System.Collections.Generic;

namespace nRed.CollectionUtil
{
	public static class ICollectionUtility
	{
		public static void SafeClear(this IList self)
		{
			if(self!=null)
				self.Clear();
		}
		public static void SafeClear(this IDictionary self)
		{
			if(self!=null)
				self.Clear();
		}

		// Caution: Shallow copy.
		public static Dictionary<K,V> Clone<K,V>(this Dictionary<K,V> self)
		{
			if(self==null)
				return null;

			Dictionary<K,V> ret = new Dictionary<K,V>( self.Count );
			var enm = self.GetEnumerator();
			while( enm.MoveNext() )
			{
				ret[enm.Current.Key] = enm.Current.Value;
			}

			return ret;
		}

		public static bool SafeGetValue<K,V>(this Dictionary<K,V> self, K key, out V val, V defaultVal=default(V))
		{
			if(self==null  ||  key==null  ||  !self.ContainsKey(key))
			{
				val = defaultVal;
				return false;
			}
				
			val = self[key];
			return true;
		}

		public static bool IsAvailable<T>(this ICollection<T> self)
		{
			return (self!=null) && (self.Count>0);
		}

		public static bool InRange(this byte self, byte inclusive_min, byte exclusive_max)
		{
			return self>=inclusive_min && self<exclusive_max;
		}
		public static bool InRange(this int self, int inclusive_min, int exclusive_max)
		{
			return self>=inclusive_min && self<exclusive_max;
		}
		public static bool InRange(this float self, float inclusive_min, float exclusive_max)
		{
			return self>=inclusive_min && self<exclusive_max;
		}
		public static bool InRange(this double self, double inclusive_min, double exclusive_max)
		{
			return self>=inclusive_min && self<exclusive_max;
		}
		public static bool InRange(this long self, long inclusive_min, long exclusive_max)
		{
			return self>=inclusive_min && self<exclusive_max;
		}

		public static bool InRange(this ICollection self, int index )
		{
			return self!=null && index>=0 && index<self.Count;
		}
	}
}
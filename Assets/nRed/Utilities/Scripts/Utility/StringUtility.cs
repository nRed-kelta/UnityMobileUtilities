// nRed Unity Mobile Utilities
// Copyright (c) nRed-kelta. For terms of use, see
// https://github.com/nRed-kelta/UnityMobileUtilities

using System.Text;

namespace nRed.StringUtil
{
    public static class StringUtility
    {
        public static bool IsReadable(this string self)
        {
            return self!=null  &&  self.Length>0;
        }

        public static string TrimAll(this string self, string trimWord)
        {
            if(self.IsReadable())
                return self.Replace(trimWord, string.Empty);

            return string.Empty;
        }
    
		private static StringBuilder sb = new StringBuilder(1024);

		public static StringBuilder Add(this string a, string b) 
		{
			return sb.Append(a).Append(b);
		}
		
		// sb.Add("ABC").Add("DEF...").Add("InternedString".Cache())
		public static StringBuilder Add(this StringBuilder a, string b) 
		{
			return a.Append(b);
		}

		public static string Cache(this string a) 
		{
			return string.Intern(a);
		}
		
		public static string Cache(this StringBuilder a) 
		{
			return string.Intern(a.ToString());
		}
	}

    public static class StringBuilderUtility
    {
        public static void Clear(this System.Text.StringBuilder sb)
        {
            if( sb!=null )
                sb.Length = 0;
        }
    }

}
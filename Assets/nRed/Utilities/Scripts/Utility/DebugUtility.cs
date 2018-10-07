// nRed Unity Mobile Utilities
// Copyright (c) nRed-kelta. For terms of use, see
// https://github.com/nRed-kelta/UnityMobileUtilities

using UnityEngine;
using System.IO;
namespace nRed.DebugUtil
{
    public static class D
    {
        [System.Diagnostics.Conditional("DEBUG")]
        public static void OnCalled(string msg)
        {
            #if DEBUG
            Debug.Log( string.Format("<color=#FF9900><b>CALLED</b></color>:{0}", msg) );
            #endif
        }

        [System.Diagnostics.Conditional("DEBUG")]
        public static void Log(string msg)
        {
            #if DEBUG
            Debug.Log( string.Format("<color=#CCCCCC><i>LOG</i></color>:{0}", msg) );
            #endif
        }

		[System.Diagnostics.Conditional("DEBUG")]
        public static void LogError(string msg)
        {
            #if DEBUG
            Debug.LogError( string.Format("<color=#FFCCCC><i>ERROR</i></color>:{0}", msg) );
            #endif
        }

		[System.Diagnostics.Conditional("DEBUG")]
		public static void FLog(string msg, bool isAppend=true)
		{

			#if DEBUG
            string path = Application.persistentDataPath + "/dlog.txt";
			try
			{
				using (StreamWriter writer = new StreamWriter(path, isAppend))
				{
					writer.Write(msg);
					writer.Flush();
					writer.Close();
				}
			}
			catch (System.Exception e)
			{
				Debug.Log(e.Message);
			}
			#endif
            
		}
    }
}
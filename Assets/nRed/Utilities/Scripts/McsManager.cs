// nRed Unity Mobile Utilities
// Copyright (c) nRed-kelta. For terms of use, see
// https://github.com/nRed-kelta/UnityMobileUtilities

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using nRed.CollectionUtil;
using nRed.StringUtil;

public class McsManager : MonoBehaviour
{
	#if UNITY_EDITOR
	public const string MCS_PATH = "Assets/mcs.rsp";
	public struct KeyValue
	{
		public string key;
		public string value;
	}
	public static readonly Dictionary<string,List<KeyValue>> keyValues = new Dictionary<string,List<KeyValue>>(32);
	public static void LoadMcs()
	{
		if( Application.isPlaying )
			return;

		keyValues.SafeClear();

		if( File.Exists(McsManager.MCS_PATH) )
		{
			string text = File.ReadAllText (McsManager.MCS_PATH).Trim();
			if (text.Length > 0)
			{
				IEnumerable<string>	lines	= text.Split('\n').Where(line => line.Length > 0);	
				foreach( string line in lines )
				{
					if( line.IsReadable() )
					{
						string[] define = line.Split(':');
						if( define.IsAvailable() && define.Length==2 )
						{
							KeyValue kv = new KeyValue
							{
								key		= define[0],
								value	= define[1],
							};

							if( keyValues.ContainsKey(kv.key)==false )
							{
								keyValues[kv.key] = new List<KeyValue>(16);
							}
							
							keyValues[kv.key].Add(kv);
						}
					}
				}
			}
		}
	}
	#endif
}
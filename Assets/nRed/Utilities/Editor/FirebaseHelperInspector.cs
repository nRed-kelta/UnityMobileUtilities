// nRed Unity Mobile Utilities
// Copyright (c) nRed-kelta. For terms of use, see
// https://github.com/nRed-kelta/UnityMobileUtilities

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using nRed.GuiUtility;
using nRed.StringUtil;
using nRed.CollectionUtil;
using nRed.Editor.GuiUtility;
using nRed.FirebaseUtil;

[CustomEditor(typeof(FirebaseHelper))]
//==============================================================================
/**
 *	@brief	FirebaseHelper Inspector drawer class
 */
//==============================================================================
public sealed class FirebaseHelperInspector : Editor
{
// V A L U E S
//##########################################################################
	private FirebaseHelper.Defines[] config = new FirebaseHelper.Defines[FirebaseHelper.Index.Length];
	private bool canPressSaveBtn			= false;


// P R O P E R T Y
//##########################################################################

	//==============================================================================
	/**
	 *	@brief	Get nRed path in the project.
	 */
	//==============================================================================
	private string nRedPath
	{
		get
		{
			string[] pathes = Directory.GetDirectories(Application.dataPath, "nRed", System.IO.SearchOption.AllDirectories);
			for(int i=0; i<pathes.Length; i++)
			{
				string[] detailPathes = Directory.GetDirectories(pathes[i], "Scripts", System.IO.SearchOption.AllDirectories);
				if( detailPathes.IsAvailable() && detailPathes.Length==1 )
				{
					string[] trimbuf = detailPathes[0].Split(new string[]{Application.dataPath}, StringSplitOptions.None);
					if( trimbuf.Length==2 )
					{
						return "Assets" + trimbuf[1];
					}
				}
			}

			return null;
		}
	}


// M E T H O D S
//##########################################################################

	//==============================================================================
	/**
	 *	@brief	Save state to mcs file.
	 */
	//==============================================================================
	private void SaveConfig()
	{
		var fbhelper = target as FirebaseHelper;

		bool useSdk = false;
		for(int i=0; i<FirebaseHelper.Index.Length; ++i)
		{
			if(i==FirebaseHelper.Index.SdkBase)
				continue;

			fbhelper.defines[i].enable = config[i].enable;
			useSdk |= fbhelper.defines[i].enable;
		}
		fbhelper.defines[FirebaseHelper.Index.SdkBase].enable = useSdk;
	}

	//==============================================================================
	/**
	 *	@brief	Load state from mcs file.
	 */
	//==============================================================================
	private void LoadConfig()
	{
		var fbhelper = target as FirebaseHelper;
		var defines  = fbhelper.defines;

		McsManager.LoadMcs();
		if( McsManager.keyValues.ContainsKey("-define") )
		{
			int count = McsManager.keyValues["-define"].Count;
			for( int i=0; i<count; ++i )
			{
				string fileDefines = McsManager.keyValues["-define"][i].value;
				if( fileDefines.IsReadable()==false )
					continue;

				for( int d=0; d<defines.Length; ++d )
				{
					if( fileDefines==defines[d].defineName )
					{
						defines[d].enable = config[d].enable = true;
						break;
					}
				}
			}			
		}

		bool useSdk = false;
		for(int i=0; i<FirebaseHelper.Index.Length; ++i)
		{
			if(i==FirebaseHelper.Index.SdkBase)
				continue;
			
			config[i] = fbhelper.defines[i];
			useSdk |= config[i].enable;
		}
		config[FirebaseHelper.Index.SdkBase].enable = useSdk;
	}
	
	//==============================================================================
	/**
	 *	@brief	Update mcs file.
	 */
	//==============================================================================
	private void UpdateMsc()
	{
		var fbhelper = target as FirebaseHelper;

		try
		{
			string text = "";

			// Trim FirebaseUtility's defines.
			if( File.Exists(McsManager.MCS_PATH) )
			{
				text = File.ReadAllText (McsManager.MCS_PATH).Trim();
				IEnumerable<string>	lines	= text.Split('\n').Where(line => line.Length > 0);
				text = string.Join("\n", lines.Where( line=>fbhelper.defines.All(define=>(line.Contains(define.defineName)==false))).ToArray ());
				if (text.Length > 0)
				{
					text += "\n";
				}
			}

			string dir_libpath = nRedPath;
			if( dir_libpath.IsReadable() )
			{
				dir_libpath += "/Firebase/";
			
				List<string> recompileList = new List<string>(8);

				// Add FirebaseUtility's defines.											
				for( int i=0; i<fbhelper.defines.Length; i++ )
				{
					string define = fbhelper.defines[i].defineName;
					string script = fbhelper.defines[i].scriptName;
					if( define.IsReadable() && config[i].enable )
					{
						text += "-define:" + define + "\n";
						if( script.IsReadable() )
						{
							recompileList.Add(script);
						}
					}
				}
				
				File.WriteAllText (McsManager.MCS_PATH, text);
				
				AssetDatabase.ImportAsset (McsManager.MCS_PATH);
				foreach(string scriptPath in recompileList)
				{
					AssetDatabase.ImportAsset (dir_libpath + scriptPath);
				}

				if( recompileList.Count>0 )
				{
					AssetDatabase.ImportAsset (nRedPath + "/FirebaseHelper.cs");
				}
				McsManager.LoadMcs();
				
			}
			
		}
		catch( System.Exception e )
		{
			nRed.DebugUtil.D.Log("[nRed][FirebaseUtility][Inspector] Could not update mcs: \n-----------------------\n" + e.ToString());
		}
	}


// U N I T Y   E V E N T S
//##########################################################################

	//==============================================================================
	/**
	 *	@brief	OnEnable inspector event.
	 */
	//==============================================================================
	private void OnEnable()
	{
		LoadConfig();
		canPressSaveBtn = false;
	}

	//==============================================================================
	/**
	 *	@brief	OnInspectorGUI event.
	 */
	//==============================================================================
    public override void OnInspectorGUI()
    {
		var fbhelper = target as FirebaseHelper;

		// Don't handle inspector values on compiling.
		if( EditorApplication.isCompiling)
		{
			EditorGUILayout.HelpBox(message:"[COMPILING SCRIPT]\nPlease wait...", type:MessageType.Info);
		}

		using( new DISABLE( Application.isPlaying || EditorApplication.isCompiling ) )
		{
			for(int i=0; i<FirebaseHelper.Index.Length; ++i)
			{
				if(i==FirebaseHelper.Index.SdkBase || i==FirebaseHelper.Index.InitOnAwake)
					continue;
				
				config[i].enable = EditorGUILayout.Toggle(fbhelper.defines[i].inspectorCaption, config[i].enable);
			}
			
			GUILayout.Space(4);
			EditorGUILayout.Separator();
			GUILayout.Space(4);

			config[FirebaseHelper.Index.InitOnAwake].enable = EditorGUILayout.Toggle(fbhelper.defines[FirebaseHelper.Index.InitOnAwake].inspectorCaption, config[FirebaseHelper.Index.InitOnAwake].enable);
			
			if( GUI.changed )
			{
				canPressSaveBtn = true;
			}

			using( Application.isPlaying? new COLOR_BG(Color.red) : new COLOR_BG(Color.yellow) )
			{
				using( new DISABLE( canPressSaveBtn==false ) )
				{
					if( GUILayout.Button(Application.isPlaying ? "Can't press at playing mode":"Save & update") )
					{
						SaveConfig();
						canPressSaveBtn = false;
						AssetDatabase.Refresh();
						UpdateMsc();
					}	
				}
			}
		}
    }
}

// nRed Unity Mobile Utilities
// Copyright (c) nRed-kelta. For terms of use, see
// https://github.com/nRed-kelta/UnityMobileUtilities

using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using nRed.CollectionUtil;
using nRed.StringUtil;
using nRed.Editor.GuiUtility;


[CustomEditor(typeof(McsManager))]
//==============================================================================
/**
 *	@brief	McsViewer Inspector drawer class
 */
//==============================================================================
public class McsViewerInspector : Editor
{

// U N I T Y   E V E N T S
//##########################################################################

	//==============================================================================
	/**
	 *	@brief	OnEnable inspector event.
	 */
	//==============================================================================
	public void OnEnable()
	{
		McsManager.keyValues.Clear();
		McsManager.LoadMcs();
	}

	//==============================================================================
	/**
	 *	@brief	OnInspectorGUI event.
	 */
	//==============================================================================
	public override void OnInspectorGUI()
    {
		if( McsManager.keyValues.IsAvailable() )
		{
			foreach( var kv in McsManager.keyValues )
			{
				GUILayout.Label( kv.Key );
				using( new YLAYOUT("Box") )
				{
					foreach( var v in kv.Value )
					{
						GUILayout.Label( v.value );
					}
				}
			}
		}
		else
		{
			GUILayout.Label( "Not found mcs Settings.", "Box" );
		}
    }
}
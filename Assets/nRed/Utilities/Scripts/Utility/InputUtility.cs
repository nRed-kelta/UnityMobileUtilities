// nRed Unity Mobile Utilities
// Copyright (c) nRed-kelta. For terms of use, see
// https://github.com/nRed-kelta/UnityMobileUtilities

using UnityEngine;
using System.Collections.Generic;

namespace nRed.InputUtil
{
	[System.Serializable]
	public class KeyAction
	{
		private static readonly KeyAction instance = new KeyAction();
		public static KeyAction Instance
		{
			get
			{
				return instance;
			}
		}

		private List<KeyMapping> mapList = new List<KeyMapping>(32);
		private List<int> existsIndexesList = new List<int>(32);
		delegate bool CheckMethod(KeyCode key);
	
		public static class Default{
			public static KeyCode nonAttachedKeyCode = KeyCode.None;
		}

		[System.Serializable]
		public struct KeyMapping
		{
			public string			inputName;
			public int				actionCode;
			public KeyCode			bind1; // Non-heap structure
			public KeyCode			bind2;
		}

		public interface ITouchHandler
		{
			void Update();
		}

		public List<int> BindKey( int actionCode, KeyCode keyCode, int bindIndex=0, string actionNameOnNew="" )
		{
			int index = FindActionCode( actionCode );
			if( index>=0 )
			{
				if( FindKeyCode( mapList[index], keyCode )==false )
				{
					mapList[index] = new KeyMapping{
						inputName	= mapList[index].inputName,
						actionCode	= actionCode,
						bind1		= bindIndex==0 ? keyCode : mapList[index].bind1,
						bind2		= bindIndex==1 ? keyCode : mapList[index].bind2,
					};
				}
			}
			else
			{
				mapList.Add( new KeyMapping{
					inputName	= actionNameOnNew,
					actionCode	= actionCode,
					bind1		= bindIndex==0 ? keyCode : Default.nonAttachedKeyCode,
					bind2		= bindIndex==1 ? keyCode : Default.nonAttachedKeyCode,
				});
				index = mapList.Count;
			}

			return ExistsKeyCodeIndex(keyCode, index);
		}

		public void UnbindKey( int actionCode, int bindIndex )
		{
			int index = FindActionCode( actionCode );
			if( index>=0 )
			{
				
				mapList[index] = new KeyMapping{
					inputName	= mapList[index].inputName,
					actionCode	= actionCode,
					bind1		= bindIndex==0 ? Default.nonAttachedKeyCode : mapList[index].bind1,
					bind2		= bindIndex==1 ? Default.nonAttachedKeyCode : mapList[index].bind2,
				};
			
			}
		}

		private int FindActionCode( int actionCode )
		{
			for( int i=0; i<mapList.Count; i++ )
			{
				if( mapList[i].actionCode == actionCode )
				{
					return i;
				}
			}
			return -1;
		}
		
		private bool FindKeyCode( KeyMapping map, KeyCode keyCode )
		{
			return (map.bind1==keyCode) || (map.bind2==keyCode);
		}

		public List<int> ExistsKeyCodeIndex( KeyCode keyCode, int ignoreIndex=-1 )
		{
			existsIndexesList.Clear();

			for( int i=0; i<mapList.Count; i++ )
			{
				if( FindKeyCode(mapList[i], keyCode) && ignoreIndex!=i )
					existsIndexesList.Add(i);
			}

			return existsIndexesList;
		}

		public bool IsKeyDown( int actionCode )
		{
			return KeyCheckMethod( actionCode, Input.GetKeyDown );
		}
		public bool IsKeyUp( int actionCode )
		{
			return KeyCheckMethod( actionCode, Input.GetKeyUp );
		}
		public bool IsKeyHold( int actionCode )
		{
			return KeyCheckMethod( actionCode, Input.GetKey );
		}
		private bool KeyCheckMethod( int actionCode, CheckMethod method )
		{
			int i = FindActionCode(actionCode);
			if( i>=0 )
			{
				KeyCode kcode = mapList[i].bind1;
				if( kcode!=KeyCode.None )
				{
					if( Input.GetKeyDown(kcode) )
					{
						return true;
					}
				}

				kcode = mapList[i].bind2;
				if( kcode!=KeyCode.None )
				{
					if( Input.GetKeyDown(kcode) )
					{
						return true;
					}
				}
			}
			return false;
		}
		
	}
}
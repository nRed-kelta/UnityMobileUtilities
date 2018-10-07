// nRed Unity Mobile Utilities
// Copyright (c) nRed-kelta. For terms of use, see
// https://github.com/nRed-kelta/UnityMobileUtilities

using System;
using System.Collections.Generic;
using UnityEngine;
using nRed.ActionUtil;
using nRed.CollectionUtil;
using nRed.ResultUtil;
#if NRED_FIREBASE_REMOTECFG_UTIL
using Firebase;
using Firebase.RemoteConfig;
using Firebase.Unity.Editor;
#endif
namespace nRed.FirebaseUtil
{
	// https://firebase.google.com/docs/reference/unity/namespace/firebase/remote-config
	public class RemoteConfigComponent : MonoBehaviour, IFirebaseComponent
	{
		[System.Serializable]
		public struct KeyValue
		{
			public string key;
			public string value;
		}

		#if NRED_FIREBASE_REMOTECFG_UTIL
		private static RemoteConfigComponent instance = null;
		[SerializeField][Header("Default key value")] public List<KeyValue> defaults;
		[SerializeField][Header("Use DeveloperMode settings(Enable only debug build)")] private bool isDeveloperMode = true;

		private void Awake()
		{
			DontDestroyOnLoad (this);
			instance = this;
		}
		#endif
		public bool isLoaded
		{
			get;
			private set;
		}

		public void Initialize( Action onSuccess=null, Action<IResultBase> onFault=null )
		{
			#if NRED_FIREBASE_REMOTECFG_UTIL
			if(isLoaded)
			{
				onFault.SafeCall( new Result{ ok=false, errorCode=FirebaseUtil.ErrorCode.ALREADY_LOADED } );
				return;
			}
			
			try
			{
				ConfigSettings c = FirebaseRemoteConfig.Settings;
				#if DEBUG
				c.IsDeveloperMode = isDeveloperMode;
				#else
				c.IsDeveloperMode = false;
				#endif

				if( defaults.IsAvailable() )
				{
					Dictionary<string,object> registDic = new Dictionary<string,object>( defaults.Count );
					var enm = defaults.GetEnumerator();
					while( enm.MoveNext() )
					{
						if( enm.Current.key!=null )
						{
							registDic[enm.Current.key] = (object)enm.Current.value;
						}
					}
					FirebaseRemoteConfig.SetDefaults( registDic );
				}
				
				isLoaded = true;
				onSuccess.SafeCall();
			}
			catch( System.Exception e )
			{
				onFault.SafeCall( new Result{ ok=false, errorCode=FirebaseUtil.ErrorCode.INIT_EXCEPTION } );
			}
			#endif
		}
	}

}
// nRed Unity Mobile Utilities
// Copyright (c) nRed-kelta. For terms of use, see
// https://github.com/nRed-kelta/UnityMobileUtilities

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nRed.DebugUtil;
using nRed.ActionUtil;
using nRed.FirebaseUtil;
using nRed.CollectionUtil;
using nRed.StringUtil;

public class FirebaseHelper : MonoBehaviour
{
	public static class Index
	{
		public const int
		SdkBase				= 0,
		AdMob				= 1,
		Analytics			= 2,
		Authentication		= 3,
		RealtimeDatabase	= 4,
		Storage				= 5,
		RemoteConfig		= 6,
		CloudMessaging		= 7,
		InitOnAwake			= 8,
		Length				= 9;
	}

	[System.Serializable]
	public struct Defines
	{
		public string	defineName;
		public string	scriptName;
		public string	inspectorCaption;
		public Type		type;
		public bool		enable;

	}

	
	public readonly Defines[] defines = new Defines[Index.Length]{
		new Defines{ defineName="NRED_FIREBASE_UTIL",			scriptName="",								inspectorCaption="",							type=null,								enable=false },
		new Defines{ defineName="NRED_FIREBASE_ADMOB_UTIL",		scriptName="AdmobComponent.cs",				inspectorCaption="Use AdMob Utility",			type=typeof(AdmobComponent),			enable=false },
		new Defines{ defineName="NRED_FIREBASE_ANALYTICS_UTIL",	scriptName="AnalyticsComponent.cs",			inspectorCaption="Use Analytics Utility",		type=typeof(AnalyticsComponent),		enable=false },
		new Defines{ defineName="NRED_FIREBASE_AUTH_UTIL",		scriptName="AuthenticationComponent.cs",	inspectorCaption="Use Auth Utility",			type=typeof(AuthenticationComponent),	enable=false },
		new Defines{ defineName="NRED_FIREBASE_RTDB_UTIL",		scriptName="RTDBComponent.cs",				inspectorCaption="Use RealtimeDatabase Utility",type=typeof(RTDBComponent),				enable=false },
		new Defines{ defineName="NRED_FIREBASE_STORAGE_UTIL",	scriptName="StorageComponent.cs",			inspectorCaption="Use Storage Utility",			type=typeof(StorageComponent),			enable=false },
		new Defines{ defineName="NRED_FIREBASE_REMOTECFG_UTIL",	scriptName="RemoteConfigComponent.cs",		inspectorCaption="Use RemoteConfig Utility",	type=typeof(RemoteConfigComponent),		enable=false },
		new Defines{ defineName="NRED_FIREBASE_CLOUDMSG_UTIL",	scriptName="CloudMessageComponent.cs",		inspectorCaption="Use FCM",						type=typeof(CloudMessageComponent),		enable=false },
		new Defines{ defineName="NRED_FIREBASE_INIT_ON_AWAKE",	scriptName="",								inspectorCaption="Init on Awake",				type=null,								enable=false },
	};

	public AdmobComponent admob{ get{ return components[Index.AdMob]==null?null: components[Index.AdMob] as AdmobComponent; } }
	public AnalyticsComponent analytics{ get{ return components[Index.Analytics]==null?null: components[Index.Analytics] as AnalyticsComponent; } }
	public AuthenticationComponent auth{ get{ return components[Index.Authentication]==null?null: components[Index.Authentication] as AuthenticationComponent; } }
	public RemoteConfigComponent remoteConfig{ get{ return components[Index.RemoteConfig]==null?null: components[Index.RemoteConfig] as RemoteConfigComponent; } }
	public RTDBComponent rtdb{ get{ return components[Index.RealtimeDatabase]==null?null: components[Index.RealtimeDatabase] as RTDBComponent; } }
	public StorageComponent storage{ get{ return components[Index.Storage]==null?null: components[Index.Storage] as StorageComponent; } }
	public CloudMessageComponent fcm{ get{ return components[Index.CloudMessaging]==null?null: components[Index.CloudMessaging] as CloudMessageComponent; } }
	
	private IFirebaseComponent[] components = new IFirebaseComponent[Index.Length];
	
	public void Initialize( Action onInitializeSuccess=null, Action onInitializeError=null )
	{
		#if NRED_FIREBASE_UTIL
		try
		{
			Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
				var dependencyStatus = task.Result;
				if (dependencyStatus == Firebase.DependencyStatus.Available) {
					
					onInitializeSuccess.SafeCall();

					if( defines[Index.InitOnAwake].enable )
					{
						InitializeComponents();
					}
					
				} else {
					UnityEngine.Debug.LogError(System.String.Format(
						"[Firebase][SDK][Init]\n\tCould not resolve all Firebase dependencies: {0}", dependencyStatus));
					onInitializeError.SafeCall();
				}
			});
		}
		catch( System.Exception e )
		{
			D.LogError( string.Format("[Firebase][SDK][Init]\n\tException:{0}", e.ToString()) );
			onInitializeError.SafeCall();
		}
		#endif
	}

	public void InitializeComponents()
	{
		for( int c=0; c<Index.Length; ++c )
		{
			if( defines[c].type!=null )
			{
				var component = GetComponent(defines[c].type);
				if( component!=null )
				{
					components[c] = component as IFirebaseComponent;
					components[c].Initialize();
				}
			}
		}
	}

	private void Awake()
	{
		if( defines[Index.InitOnAwake].enable )
		{
			// InitOnAwake does not use finish event callbacks.
			// If you need callbacks, when disable init on awake and call Initialize with params your self.
			Initialize();
		}
	}

}
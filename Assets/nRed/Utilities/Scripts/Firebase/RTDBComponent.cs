// nRed Unity Mobile Utilities
// Copyright (c) nRed-kelta. For terms of use, see
// https://github.com/nRed-kelta/UnityMobileUtilities

using System;
using System.Collections.Generic;
using UnityEngine;
using nRed.ActionUtil;
using nRed.ResultUtil;
#if NRED_FIREBASE_RTDB_UTIL
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
#endif

namespace nRed.FirebaseUtil
{
	// https://firebase.google.com/docs/database/unity/start
	// * .NET3.5 compatible only
	public class RTDBComponent : MonoBehaviour, IFirebaseComponent
	{
		#if UNITY_EDITOR
		[System.Serializable]
		public class PlatformSettings
		{
			public string editorDbUrl;
			public string editorP12FileName;
			public string editorServiceAccountEmail;
			public string editorP12Password;
		}

		[SerializeField][Header("RealtimeDatabase on Editor")] PlatformSettings unityEditor = new PlatformSettings();
		#endif

		public bool isLoaded
		{
			get;
			private set;
		}

		#if NRED_FIREBASE_RTDB_UTIL
		private void Awake()
		{
			DontDestroyOnLoad (this);
		}

		private DatabaseReference reference = null;

		public DatabaseReference dbroot
		{
			get{ return reference; }
		}
		#endif

		public void Initialize( Action onSuccess=null, Action<IResultBase> onFault=null )
		{
			#if NRED_FIREBASE_RTDB_UTIL
			try
			{
				if(isLoaded)
				{
					onFault.SafeCall( new Result{ ok=false, errorCode=FirebaseUtil.ErrorCode.ALREADY_LOADED } );
					return;
				}

				FirebaseApp.DefaultInstance.SetEditorDatabaseUrl( unityEditor.editorDbUrl );
				FirebaseApp.DefaultInstance.SetEditorP12FileName( unityEditor.editorP12FileName );
				FirebaseApp.DefaultInstance.SetEditorServiceAccountEmail( unityEditor.editorServiceAccountEmail );
				FirebaseApp.DefaultInstance.SetEditorP12Password( unityEditor.editorP12Password );
				reference = FirebaseDatabase.DefaultInstance.RootReference;

				isLoaded = true;

				onSuccess.SafeCall();
			}
			catch( System.Exception e )
			{
				DebugUtil.D.LogError( string.Format("[Firebase][RealTimeDatabase] Initialize Exception:{0}", e.ToString()) );
				onFault.SafeCall( new Result{ ok=false, errorCode=FirebaseUtil.ErrorCode.INIT_EXCEPTION } );
			}
			#endif
		}
	}

}
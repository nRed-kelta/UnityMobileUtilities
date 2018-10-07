// nRed Unity Mobile Utilities
// Copyright (c) nRed-kelta. For terms of use, see
// https://github.com/nRed-kelta/UnityMobileUtilities

using System;
using System.Collections.Generic;
using UnityEngine;
using nRed.ActionUtil;
using nRed.CollectionUtil;
using nRed.StringUtil;
using nRed.ResultUtil;
#if NRED_FIREBASE_ANALYTICS_UTIL
using Firebase;
using Firebase.Messaging;
using Firebase.Unity.Editor;
#endif
namespace nRed.FirebaseUtil
{
	// https://firebase.google.com/docs/analytics/
	// * .NET3.5 compatible only

	//===============================================================
	/**
	 *	@brief	Analytics Helper class
	 */
	//===============================================================
	public class AnalyticsComponent : MonoBehaviour, IFirebaseComponent
	{
		public bool isLoaded
		{
			get;
			private set;
		}

		#if NRED_FIREBASE_ANALYTICS_UTIL
		//===============================================================
		/**
		*	@brief	Awake event
		*/
		//===============================================================
		private void Awake()
		{
			DontDestroyOnLoad (this);
		}
		#endif

		//===============================================================
		/**
		*	@brief	Initialize
		*/
		//===============================================================
		public void Initialize( Action onSuccess=null, Action<IResultBase> onFault=null )
		{
			#if NRED_FIREBASE_ANALYTICS_UTIL
			if( isLoaded )
			{
				onFault.SafeCall( new Result{ ok=false, errorCode=FirebaseUtil.ErrorCode.ALREADY_LOADED } );
				return;
			}
			
			try
			{
				isLoaded = true;
				onSuccess.SafeCall();
			}
			catch( System.Exception )
			{
				onFault.SafeCall( new Result{ ok=false, errorCode=FirebaseUtil.ErrorCode.INIT_EXCEPTION } );
			}
			#endif
		}

		// Caution: Event log can send max by 500 kinds. [https://firebase.google.com/docs/analytics/unity/start]
		//===============================================================
		/**
		*	@brief	Log event wrapper.
		*/
		//===============================================================
		public void LogEvent<T>(string eventName, string paramKey, T paramValue)
		{
			if( isLoaded==false )
			{
				return;
			}
			#if NRED_FIREBASE_ANALYTICS_UTIL && !DEBUG
			Firebase.Analytics.FirebaseAnalytics.LogEvent( eventName, paramKey, paramKey );
			#endif
		}

		[System.Diagnostics.Conditional("DEBUG")]
		//===============================================================
		/**
		*	@brief	Log event wrapper(DEBUG build only)
		*/
		//===============================================================
		public void DLogEvent<T>(string eventName, string paramKey, T paramValue)
		{
			if( isLoaded==false )
			{
				return;
			}
			#if NRED_FIREBASE_ANALYTICS_UTIL && DEBUG
			Firebase.Analytics.FirebaseAnalytics.LogEvent( eventName, paramKey, paramKey );
			#endif
		}

	}
}
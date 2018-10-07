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
#if NRED_FIREBASE_CLOUDMSG_UTIL
using Firebase;
using Firebase.Messaging;
using Firebase.Unity.Editor;
#endif
namespace nRed.FirebaseUtil
{
	// https://firebase.google.com/docs/cloud-messaging/unity/client
	// * .NET3.5 compatible only
	public class CloudMessageComponent : MonoBehaviour, IFirebaseComponent
	{
		[System.Serializable]
		public class PlatformSettings
		{
			public string[] topicOnDebugBuild;		// This is subscribe on debug build only. otherwise does unsubscribed. ex) on the release/product build.
			public string[] topicOnReleaseBuild;	// This is subscribe on release build only. otherwise does unsubscribed. ex) on the debug/product build.
			public string[] topicOnProductBuild;	// This is subscribe on product build only. otherwise does unsubscribed. ex) on the debug/release build.
		}

		public PlatformSettings iOS;
		public PlatformSettings android;
		public PlatformSettings editor;

		public string token{ get; private set; }

		private Action<string> onTokenReceived = null;

		public bool isLoaded
		{
			get;
			private set;
		}

		#if NRED_FIREBASE_CLOUDMSG_UTIL
		private void Awake()
		{
			DontDestroyOnLoad (this);
		}

		private void AddDebugBuildTopics()
		{
			#if UNITY_IOS
			PlatformSettings settings = iOS;
			#elif UNITY_ANDROID
			PlatformSettings settings = android;
			#else
			PlatformSettings settings = editor;
			#endif

			if( settings.topicOnDebugBuild.IsAvailable() )
			{
				for(int i=0; i<settings.topicOnDebugBuild.Length; ++i)
				{
					AddTopic( settings.topicOnDebugBuild[i] );
				}
			}
		}

		private void RemoveDebugBuildTopics()
		{
			#if UNITY_IOS
			PlatformSettings settings = iOS;
			#elif UNITY_ANDROID
			PlatformSettings settings = android;
			#else
			PlatformSettings settings = editor;
			#endif

			if( settings.topicOnDebugBuild.IsAvailable() )
			{
				for(int i=0; i<settings.topicOnDebugBuild.Length; ++i)
				{
					RemoveTopic( settings.topicOnDebugBuild[i] );
				}
			}
		}

		private void AddReleaseBuildTopics()
		{
			#if UNITY_IOS
			PlatformSettings settings = iOS;
			#elif UNITY_ANDROID
			PlatformSettings settings = android;
			#else
			PlatformSettings settings = editor;
			#endif

			if( settings.topicOnReleaseBuild.IsAvailable() )
			{
				for(int i=0; i<settings.topicOnReleaseBuild.Length; ++i)
				{
					AddTopic( settings.topicOnReleaseBuild[i] );
				}
			}
		}

		private void RemoveReleaseBuildTopics()
		{
			#if UNITY_IOS
			PlatformSettings settings = iOS;
			#elif UNITY_ANDROID
			PlatformSettings settings = android;
			#else
			PlatformSettings settings = editor;
			#endif

			if( settings.topicOnReleaseBuild.IsAvailable() )
			{
				for(int i=0; i<settings.topicOnReleaseBuild.Length; ++i)
				{
					RemoveTopic( settings.topicOnReleaseBuild[i] );
				}
			}
		}

		private void AddProductBuildTopics()
		{
			#if UNITY_IOS
			PlatformSettings settings = iOS;
			#elif UNITY_ANDROID
			PlatformSettings settings = android;
			#else
			PlatformSettings settings = editor;
			#endif

			if( settings.topicOnProductBuild.IsAvailable() )
			{
				for(int i=0; i<settings.topicOnProductBuild.Length; ++i)
				{
					AddTopic( settings.topicOnProductBuild[i] );
				}
			}
		}

		private void RemoveProductBuildTopics()
		{
			#if UNITY_IOS
			PlatformSettings settings = iOS;
			#elif UNITY_ANDROID
			PlatformSettings settings = android;
			#else
			PlatformSettings settings = editor;
			#endif
			
			if( settings.topicOnProductBuild.IsAvailable() )
			{
				for(int i=0; i<settings.topicOnProductBuild.Length; ++i)
				{
					RemoveTopic( settings.topicOnProductBuild[i] );
				}
			}
		}

		private void OnTokenReceived(object sender, TokenReceivedEventArgs e)
		{
			this.token = e.Token;

			#if DEBUG
			{
				AddDebugBuildTopics();
				RemoveReleaseBuildTopics();
				RemoveProductBuildTopics();
			}
			#elif RELEASE
			{
				AddReleaseBuildTopics();
				RemoveDebugBuildTopics();
				RemoveProductBuildTopics();
			}
			#elif !DEBUG && !RELEASE
			{
				AddProductBuildTopics();
				RemoveDebugBuildTopics();
				RemoveReleaseBuildTopics();
			}
			#endif

			onTokenReceived.SafeCall(token);
		}

		public void SetCallback( System.Action<string> onTokenReceived, System.EventHandler<MessageReceivedEventArgs> onMessageReceived=null )
		{
			if( isLoaded==false )
			{
				return;
			}
			
			this.onTokenReceived = onTokenReceived;
			
			if( onMessageReceived!=null )
			{
				Firebase.Messaging.FirebaseMessaging.MessageReceived += onMessageReceived;
			}
		}
		#endif

		public void Initialize( Action onSuccess=null, Action<IResultBase> onFault=null )
		{
			#if NRED_FIREBASE_CLOUDMSG_UTIL
			if( isLoaded )
			{
				onFault.SafeCall( new Result{ ok=false, errorCode=FirebaseUtil.ErrorCode.ALREADY_LOADED } );
				return;
			}
			
			try
			{
				Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
				isLoaded = true;
				onSuccess.SafeCall();
			}
			catch( System.Exception )
			{
				onFault.SafeCall( new Result{ ok=false, errorCode=FirebaseUtil.ErrorCode.INIT_EXCEPTION } );
			}
			#endif
		}

		public void AddTopic(string registTopic, string withUnregistTopic=null)
		{
			if( isLoaded==false )
			{
				return;
			}
			#if NRED_FIREBASE_CLOUDMSG_UTIL
			if( withUnregistTopic.IsReadable() )
			{
				Firebase.Messaging.FirebaseMessaging.UnsubscribeAsync(withUnregistTopic).ContinueWith(task => 
				{
					
				});
			}

			if( registTopic.IsReadable() )
			{
				Firebase.Messaging.FirebaseMessaging.SubscribeAsync(registTopic).ContinueWith(task => 
				{
					
				});
			}
			#endif
		}

		public void RemoveTopic(string unregistTopic)
		{
			if( isLoaded==false )
			{
				return;
			}
			#if NRED_FIREBASE_CLOUDMSG_UTIL
			if( unregistTopic.IsReadable() )
			{
				Firebase.Messaging.FirebaseMessaging.UnsubscribeAsync(unregistTopic).ContinueWith(task => 
				{
					
				});
			}
			#endif
		}
	}
}
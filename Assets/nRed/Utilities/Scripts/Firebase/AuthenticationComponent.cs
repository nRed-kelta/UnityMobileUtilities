// nRed Unity Mobile Utilities
// Copyright (c) nRed-kelta. For terms of use, see
// https://github.com/nRed-kelta/UnityMobileUtilities

using System;
using System.Collections.Generic;
using UnityEngine;
using nRed.ActionUtil;
using nRed.ResultUtil;
#if NRED_FIREBASE_AUTH_UTIL
using Firebase.Auth;
#endif
namespace nRed.FirebaseUtil
{
	// https://firebase.google.com/docs/auth/
	// * .NET3.5 compatible only
	public class AuthenticationComponent : MonoBehaviour, IFirebaseComponent
	{
		public static class ErrorCode
		{
			public const int INIT_EXCEPTION = 1;
		}


		#if UNITY_EDITOR
		[System.Serializable]
		public class PlatformSettings
		{
			public string debugEmail;
			public string debugPass;
		}

		[SerializeField][Header("Anonymous auth substitution for debug")] PlatformSettings unityEditor = new PlatformSettings();
		#endif
		
		#if NRED_FIREBASE_AUTH_UTIL
		private static AuthenticationComponent instance = null;
		
		private void Awake()
		{
			DontDestroyOnLoad (this);
			instance = this;
		}

		public static AuthenticationComponent Instance
		{
			get{
				return instance;
			}
		}
		#endif

		public bool isLoaded
		{
			get;
			private set;
		}

		public void Initialize( Action onSuccess=null, Action<IResultBase> onFault=null )
		{
			if( isLoaded )
			{
				onFault.SafeCall( new Result{ ok=false, errorCode=FirebaseUtil.ErrorCode.ALREADY_LOADED } );
			}

			#if NRED_FIREBASE_AUTH_UTIL
			try
			{
				isLoaded = true;
				onSuccess.SafeCall();
			}
			catch( System.Exception e )
			{
				isLoaded = false;
				DebugUtil.D.LogError( string.Format("[Firebase][AuthenticationComponent] Initialize Exception:{0}", e.ToString()) );
				onFault.SafeCall( new Result{ ok=false, errorCode=FirebaseUtil.ErrorCode.INIT_EXCEPTION } );
			}
			#endif
		}
		
		#if NRED_FIREBASE_AUTH_UTIL
		public void AnonymousSignIn( Action<Firebase.Auth.FirebaseUser> onCompleted, Action onFaulted, Action onCanceled )
		{
			// Can not running the anonymous signin on Unity editor.
			// So, Use email signin logic by the dummy email & pass signin.
			// https://groups.google.com/forum/#!topic/firebase-talk/hUPQFWT4vvM
			#if UNITY_EDITOR
			{
				EmailPassSignIn( unityEditor.debugEmail, unityEditor.debugPass, onCompleted, onFaulted, onCanceled );
			}
			#else
			{
				AnonymousAuth().SignIn( onCompleted, onFaulted, onCanceled );
			}
			#endif
		}

		public void EmailPassCreateUser( string registEmail, string registPassword, Action<Firebase.Auth.FirebaseUser> onCompleted, Action onFaulted, Action onCanceled )
		{
			new EmailAuth().CreateUser( new string[]{registEmail, registPassword}, onCompleted, onFaulted, onCanceled );
		}

		public void EmailPassSignIn( string registedEmail, string registedPassword, Action<Firebase.Auth.FirebaseUser> onCompleted, Action onFaulted, Action onCanceled )
		{
			new EmailAuth().SignIn( new string[]{registedEmail, registedPassword}, onCompleted, onFaulted, onCanceled );
		}

		public void SignOut()
		{
			Firebase.Auth.FirebaseAuth.DefaultInstance.SignOut();
		}
		#endif
	}

	#if NRED_FIREBASE_AUTH_UTIL
	public interface IAuthentication
	{
		void SignIn( string[] context, Action<Firebase.Auth.FirebaseUser> onCompleted, Action onFaulted, Action onCanceled );
		void CreateUser( string[] context, Action<Firebase.Auth.FirebaseUser> onCompleted, Action onFaulted, Action onCanceled );
	}

	// https://firebase.google.com/docs/auth/unity/anonymous-auth
	public class AnonymousAuth : IAuthentication
	{
		public void SignIn( string[] context, Action<Firebase.Auth.FirebaseUser> onCompleted, Action onFaulted, Action onCanceled )
		{
			Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

			auth.SignInAnonymouslyAsync().ContinueWith(
				task =>
				{
					if (task.IsCanceled)
					{
						DebugUtil.D.LogError("SignInAnonymouslyAsync was canceled.");
						onCanceled.SafeCall();
						return;
					}
					
					if (task.IsFaulted)
					{
						DebugUtil.D.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
						onFaulted.SafeCall();
						return;
					}

					if (task.IsCompleted)
					{
						Firebase.Auth.FirebaseUser newUser = task.Result;
						onCompleted.SafeCall(newUser);
					}
				}
			);
		}

		public void CreateUser( string[] context, Action<Firebase.Auth.FirebaseUser> onCompleted, Action onFaulted, Action onCanceled )
		{
			Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
			DebugUtil.D.LogError("Anonymous user create was canceled. because no exists create method.");
			onCanceled.SafeCall();
		}
	}

	// https://firebase.google.com/docs/auth/unity/password-auth
	public class EmailAuth : IAuthentication
	{
		public void CreateUser( string[] context, Action<Firebase.Auth.FirebaseUser> onCompleted, Action onFaulted, Action onCanceled )
		{
			if( context==null || context.Length!=2 )
			{
				DebugUtil.D.LogError("EmailAuth CreateUser context error.");
				onFaulted.SafeCall();
				return;
			}

			Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
			auth.CreateUserWithEmailAndPasswordAsync(context[0], context[1]).ContinueWith(
				task =>
				{
					if (task.IsCanceled)
					{
						DebugUtil.D.LogError("EmailAuth CreateUser was canceled.");
						onCanceled.SafeCall();
						return;
					}
					
					if (task.IsFaulted)
					{
						DebugUtil.D.LogError("EmailAuth CreateUser encountered an error: " + task.Exception);
						onFaulted.SafeCall();
						return;
					}

					if (task.IsCompleted)
					{
						Firebase.Auth.FirebaseUser newUser = task.Result;
						onCompleted.SafeCall(newUser);
					}
				}
			);
		}

		public void SignIn( string[] context, Action<Firebase.Auth.FirebaseUser> onCompleted, Action onFaulted, Action onCanceled )
		{
			if( context==null || context.Length!=2 )
			{
				DebugUtil.D.LogError("EmailAuth CreateUser context error.");
				onFaulted.SafeCall();
				return;
			}

			Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

			auth.SignInWithEmailAndPasswordAsync (context[0], context[1]).ContinueWith(
				task =>
				{
					if (task.IsCanceled)
					{
						DebugUtil.D.LogError("EmailAuth CreateUser was canceled.");
						onCanceled.SafeCall();
						return;
					}
					
					if (task.IsFaulted)
					{
						DebugUtil.D.LogError("EmailAuth CreateUser encountered an error: " + task.Exception);
						onFaulted.SafeCall();
						return;
					}

					if (task.IsCompleted)
					{
						Firebase.Auth.FirebaseUser newUser = task.Result;
						onCompleted.SafeCall(newUser);
					}
				}
			);
		}
	}
	#endif
}
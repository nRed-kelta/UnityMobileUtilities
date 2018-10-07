// nRed Unity Mobile Utilities
// Copyright (c) nRed-kelta. For terms of use, see
// https://github.com/nRed-kelta/UnityMobileUtilities

using System;
using System.Collections.Generic;
using UnityEngine;
using nRed.ActionUtil;
using nRed.CollectionUtil;
using nRed.ResultUtil;
#if NRED_FIREBASE_STORAGE_UTIL
using Firebase;
using Firebase.Storage;
using Firebase.Unity.Editor;
#endif
namespace nRed.FirebaseUtil
{
	// https://firebase.google.com/docs/storage/unity/start
	// * .NET3.5 compatible only
	public class StorageComponent : MonoBehaviour, IFirebaseComponent
	{
		#if NRED_FIREBASE_STORAGE_UTIL
		[SerializeField] private string[] bucketsPath;

		private void Awake()
		{
			DontDestroyOnLoad (this);
		}

		public FirebaseStorage storage
		{
			get;
			private set;
		}

		public List<FirebaseStorage> buckets {
			get;
			private set;
		}
		#endif

		public bool isLoaded
		{
			get;
			private set;
		}

		public void Initialize( Action onSuccess=null, Action<IResultBase> onFault=null )
		{
			#if NRED_FIREBASE_STORAGE_UTIL
			if(isLoaded)
			{
				onFault.SafeCall( new Result{ ok=false, errorCode=FirebaseUtil.ErrorCode.ALREADY_LOADED } );
				return;
			}
			
			if( buckets==null )
			{
				buckets = new List<FirebaseStorage>(8);
			}
			else
			{
				buckets.Clear();
			}

			try
			{
				storage = FirebaseStorage.DefaultInstance;

				if( bucketsPath.IsAvailable() )
				{
					for( int i=0; i<bucketsPath.Length; i++ )
					{
						buckets.Add( FirebaseStorage.GetInstance(bucketsPath[i]) );
					}
				}
				
				isLoaded = true;
				onSuccess.SafeCall();
			}
			catch( System.Exception e )
			{
				onFault.SafeCall( new Result{ ok=false, errorCode=FirebaseUtil.ErrorCode.INIT_EXCEPTION} );
			}
			#endif
		}
	}
}
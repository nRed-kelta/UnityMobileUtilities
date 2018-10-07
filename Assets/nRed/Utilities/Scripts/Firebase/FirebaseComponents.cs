// nRed Unity Mobile Utilities
// Copyright (c) nRed-kelta. For terms of use, see
// https://github.com/nRed-kelta/UnityMobileUtilities

using System;
using nRed.ResultUtil;
namespace nRed.FirebaseUtil
{
	public static class ErrorCode
	{
		public const int ALREADY_LOADED = 1;
		public const int INIT_EXCEPTION = 2;
	}
	
	public interface IFirebaseComponent
	{
		bool isLoaded{get;}
		void Initialize( Action onSuccess=null, Action<IResultBase> onFault=null );
	}	
}
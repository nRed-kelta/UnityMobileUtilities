// nRed Unity Mobile Utilities
// Copyright (c) nRed-kelta. For terms of use, see
// https://github.com/nRed-kelta/UnityMobileUtilities

using System;
namespace nRed.ActionUtil
{
    public static class ActionUtility
    {
		//========================================================================================
		//
		//	@brief	Safe action call without param.
		//
		//========================================================================================
        public static void SafeCall(this Action self)
        {
            if(self!=null)
                self();
        }

		//========================================================================================
		//
		//	@brief	Safe action call with param.
		//
		//========================================================================================
        public static void SafeCall<T>(this Action<T> self, T param1)
        {
            if(self!=null)
                self(param1);
        }

		//========================================================================================
		//
		//	@brief	Safe action call with 2 params.
		//
		//========================================================================================
        public static void SafeCall<T1,T2>(this Action<T1,T2> self, T1 param1, T2 param2)
        {
            if(self!=null)
                self(param1, param2);
        }

		//========================================================================================
		//
		//	@brief	Safe action call with 3 params.
		//
		//========================================================================================
        public static void SafeCall<T1,T2,T3>(this Action<T1,T2,T3> self, T1 param1, T2 param2, T3 param3)
        {
            if(self!=null)
                self(param1, param2, param3);
        }

		//========================================================================================
		//
		//	@brief	Safe action call with 4 params.
		//
		//========================================================================================
        public static void SafeCall<T1,T2,T3,T4>(this Action<T1,T2,T3,T4> self, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            if(self!=null)
                self(param1, param2, param3, param4);
        }
    }
}
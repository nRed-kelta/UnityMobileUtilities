// nRed Unity Mobile Utilities
// Copyright (c) nRed-kelta. For terms of use, see
// https://github.com/nRed-kelta/UnityMobileUtilities

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace nRed.CodeInfo
{
	public static class CallInfo
	{
		private static StackTrace st = new StackTrace( fNeedFileInfo:true );

		public static string GetFileName(int offset=0)
		{
			return string.Intern(  st.GetFrame( offset + 1 ).GetFileName()  );
		}

		public static string GetMethodName(int offset=0)
		{
			return string.Intern(  st.GetFrame( offset + 1 ).GetMethod().Name  );
		}

		public static int GetLineNumber(int offset=0)
		{
			return st.GetFrame( offset + 1 ).GetFileLineNumber();
		}

		public static int GetColumnNumber(int offset=0)
		{
			return st.GetFrame( offset + 1 ).GetFileColumnNumber();
		}

	}
}
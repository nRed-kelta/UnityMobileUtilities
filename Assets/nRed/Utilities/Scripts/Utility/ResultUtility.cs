// nRed Unity Mobile Utilities
// Copyright (c) nRed-kelta. For terms of use, see
// https://github.com/nRed-kelta/UnityMobileUtilities

using System;
namespace nRed.ResultUtil
{
	public interface IResultBase
	{
		bool	ok{get;}
		int		errorCode{get;}
		string	errorMsg{get;}
	}

	public class Result : IResultBase
	{
		public bool		ok{get;set;}
		public int		errorCode{get;set;}
		public string	errorMsg{get;set;}
	}
}
// nRed Unity Mobile Utilities
// Copyright (c) nRed-kelta. For terms of use, see
// https://github.com/nRed-kelta/UnityMobileUtilities

using UnityEngine;

namespace nRed.GuiUtility
{
	public class COLOR_BG : GUI.Scope
	{
		private readonly Color color;
		public COLOR_BG(Color color)
		{
			this.color = GUI.backgroundColor;
			GUI.backgroundColor = color;
		}


		protected override void CloseScope()
		{
			GUI.backgroundColor = color;
		}
	}
}
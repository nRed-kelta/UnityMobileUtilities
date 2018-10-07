// nRed Unity Mobile Utilities
// Copyright (c) nRed-kelta. For terms of use, see
// https://github.com/nRed-kelta/UnityMobileUtilities

using UnityEngine;
namespace nRed.ColorUtil
{
	public static partial class ColorUtility
	{
		public static void r( this SpriteRenderer self, float r )
		{
			if(self==null)
			{
				return;
			}
			self.color = new Color(r, self.color.g, self.color.b, self.color.a);
		}
		public static void g( this SpriteRenderer self, float g )
		{
			if(self==null)
			{
				return;
			}
			self.color = new Color(self.color.r, g, self.color.b, self.color.a);
		}
		public static void b( this SpriteRenderer self, float b )
		{
			if(self==null)
			{
				return;
			}
			self.color = new Color(self.color.r, self.color.g, b, self.color.a);
		}
		public static void a( this SpriteRenderer self, float a )
		{
			if(self==null)
			{
				return;
			}
			self.color = new Color(self.color.r, self.color.g, self.color.b, a);
		}

		public static void r( this SpriteRenderer self, byte r )
		{
			if(self==null)
			{
				return;
			}
			Color32 c32 = self.color;
			self.color = new Color32(r, c32.g, c32.b, c32.a);
		}
		public static void g( this SpriteRenderer self, byte g )
		{
			if(self==null)
			{
				return;
			}
			Color32 c32 = self.color;
			self.color = new Color32(c32.r, g, c32.b, c32.a);
		}
		public static void b( this SpriteRenderer self, byte b )
		{
			if(self==null)
			{
				return;
			}
			Color32 c32 = self.color;
			self.color = new Color32(c32.r, c32.g, b, c32.a);
		}
		public static void a( this SpriteRenderer self, byte a )
		{
			if(self==null)
			{
				return;
			}
			Color32 c32 = self.color;
			self.color = new Color32(c32.r, c32.g, c32.b, a);
		}
	}
}
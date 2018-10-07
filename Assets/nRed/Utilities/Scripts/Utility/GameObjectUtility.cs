// nRed Unity Mobile Utilities
// Copyright (c) nRed-kelta. For terms of use, see
// https://github.com/nRed-kelta/UnityMobileUtilities

using UnityEngine;
namespace nRed.GameObjectUtil
{
	public static class GameObjectUtility
	{
		public static void ToActive(this GameObject self)
		{
			if(self!=null)
				self.SetActive(true);
		}
		public static void ToDeactive(this GameObject self)
		{
			if(self!=null)
				self.SetActive(false);
		}

		public static void SetParent( this GameObject self, GameObject parent, bool worldPosStay=false )
		{
			if(self!=null)
				self.transform.SetParent(parent.transform, worldPosStay);
		}

		public static void SetParent( this GameObject self, Transform parent, bool worldPosStay=false )
		{
			if(self!=null)
				self.transform.SetParent(parent, worldPosStay);
		}
		
		public static void MoveTo( this GameObject self, Vector3 pos )
		{
			if(self!=null)
				self.transform.localPosition = pos;
		}

		public static void ScaleTo( this GameObject self, Vector3 scale )
		{
			if(self!=null)
				self.transform.localScale = scale;
		}
		
		public static Vector3 lp( this GameObject self )
		{
			if(self!=null)
			{
				return self.transform.localPosition;
			}
			return Vector3.negativeInfinity;
		}
		public static void lp( this GameObject self, float new_x, float new_y, float new_z )
		{
			if(self!=null)
			{
				self.transform.localPosition = new Vector3( new_x, new_y, new_z );
			}
		}
		public static void lp( this GameObject self, Vector3 new_scale_vector )
		{
			if(self!=null)
			{
				self.transform.localPosition = new_scale_vector;
			}
		}
		public static float lx( this GameObject self )
		{
			if(self!=null)
				return self.transform.localPosition.x;

			return float.NaN;
		}

		public static float ly( this GameObject self )
		{
			if(self!=null)
				return self.transform.localPosition.y;

			return float.NaN;
		}

		public static float lz( this GameObject self )
		{
			if(self!=null)
				return self.transform.localPosition.z;

			return float.NaN;
		}

		public static void lx( this GameObject self, float new_x )
		{
			if(self!=null)
			{
				var v = self.transform.localPosition;
				self.transform.localPosition = new Vector3( new_x, v.y, v.z );
			}
		}

		public static void ly( this GameObject self, float new_y )
		{
			if(self!=null)
			{
				var v = self.transform.localPosition;
				self.transform.localPosition = new Vector3( v.x, new_y, v.z );
			}
		}

		public static void lz( this GameObject self, float new_z )
		{
			if(self!=null)
			{
				var v = self.transform.localPosition;
				self.transform.localPosition = new Vector3( v.x, v.y, new_z );
			}
		}

		public static void lxy( this GameObject self, float new_x, float new_y )
		{
			if(self!=null)
			{
				var v = self.transform.localPosition;
				self.transform.localPosition = new Vector3( new_x, new_y, v.z );
			}
		}
		public static void lxz( this GameObject self, float new_x, float new_z )
		{
			if(self!=null)
			{
				var v = self.transform.localPosition;
				self.transform.localPosition = new Vector3( new_x, v.y, new_z );
			}
		}
		public static void lyz( this GameObject self, float new_y, float new_z )
		{
			if(self!=null)
			{
				var v = self.transform.localPosition;
				self.transform.localPosition = new Vector3( v.x, new_y, new_z );
			}
		}

		public static float lsx( this GameObject self )
		{
			if(self!=null)
				return self.transform.localScale.x;

			return float.NaN;
		}

		public static float lsy( this GameObject self )
		{
			if(self!=null)
				return self.transform.localScale.y;

			return float.NaN;
		}

		public static float lsz( this GameObject self )
		{
			if(self!=null)
				return self.transform.localScale.z;

			return float.NaN;
		}

		public static Vector3 ls( this GameObject self )
		{
			if(self!=null)
			{
				return self.transform.localScale;
			}
			return Vector3.negativeInfinity;
		}
		public static void ls( this GameObject self, float new_x, float new_y, float new_z )
		{
			if(self!=null)
			{
				self.transform.localScale = new Vector3( new_x, new_y, new_z );
			}
		}
		public static void ls( this GameObject self, Vector3 new_scale_vector )
		{
			if(self!=null)
			{
				self.transform.localScale = new_scale_vector;
			}
		}
		public static void lsx( this GameObject self, float new_x )
		{
			if(self!=null)
			{
				var v = self.transform.localScale;
				self.transform.localScale = new Vector3( new_x, v.y, v.z );
			}
		}

		public static void lsy( this GameObject self, float new_y )
		{
			if(self!=null)
			{
				var v = self.transform.localScale;
				self.transform.localScale = new Vector3( v.x, new_y, v.z );
			}
		}

		public static void lsz( this GameObject self, float new_z )
		{
			if(self!=null)
			{
				var v = self.transform.localScale;
				self.transform.localScale = new Vector3( v.x, v.y, new_z );
			}
		}

		public static void lsxy( this GameObject self, float new_x, float new_y )
		{
			if(self!=null)
			{
				var v = self.transform.localScale;
				self.transform.localScale = new Vector3( new_x, new_y, v.z );
			}
		}
		public static void lsxz( this GameObject self, float new_x, float new_z )
		{
			if(self!=null)
			{
				var v = self.transform.localScale;
				self.transform.localScale = new Vector3( new_x, v.y, new_z );
			}
		}
		public static void lsyz( this GameObject self, float new_y, float new_z )
		{
			if(self!=null)
			{
				var v = self.transform.localScale;
				self.transform.localScale = new Vector3( v.x, new_y, new_z );
			}
		}
	}
}
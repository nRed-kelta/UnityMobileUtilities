// nRed Unity Mobile Utilities
// Copyright (c) nRed-kelta. For terms of use, see
// https://github.com/nRed-kelta/UnityMobileUtilities

using UnityEngine;
using nRed.MathUtil;

namespace nRed.ColliderUtil
{
	public struct RaycastInfo
	{
		public bool			isHit;
		public RaycastHit	raycastHit;

		public static bool operator true(RaycastInfo raycastInfo)
		{
			return raycastInfo.isHit;
		}
		public static bool operator false(RaycastInfo raycastInfo)
		{
			return raycastInfo.isHit==false;
		}
	}

	public static class ColliderUtility
	{
		public static RaycastInfo HitRay( this Collider self, Vector2 rayPos, Camera camera=null )
		{
			RaycastInfo info = new RaycastInfo{ isHit=false };
			if( self==null )
				return info;

			Camera cam = camera?? Camera.main;
			Ray ray = cam.ScreenPointToRay(rayPos);
			
			info.isHit = self.Raycast(ray, out info.raycastHit, (cam.farClipPlane-cam.nearClipPlane).Abs());
			return info;
		}
	}
}
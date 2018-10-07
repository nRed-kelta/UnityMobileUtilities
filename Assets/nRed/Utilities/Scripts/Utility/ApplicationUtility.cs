// nRed Unity Mobile Utilities
// Copyright (c) nRed-kelta. For terms of use, see
// https://github.com/nRed-kelta/UnityMobileUtilities

using UnityEngine;

namespace nRed.AppUtil
{
    public static class ApplicationUtility
    {
        public static void OpenByBrowser(string url)
        {
            #if UNITY_EDITOR
            Application.OpenURL(url);
            #elif UNITY_WEBGL
            Application.ExternalEval(string.Format("window.open('{0}','_blank')", url));
            #else
            Application.OpenURL(url);
            #endif
        }
    }
}
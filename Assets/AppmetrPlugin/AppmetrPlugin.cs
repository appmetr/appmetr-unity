using UnityEngine;
using System.Collections;

#if UNITY_ANDROID
using AppmetrPlatformPlugin = AppmetrPluginAndroid;
#elif UNITY_IPHONE
using AppmetrPlatformPlugin = AppmetrPluginIOS;
#else
using AppmetrPlatformPlugin = AppmetrPluginDefault;
#endif

public class AppmetrPlugin : AppmetrWrapper
{
}

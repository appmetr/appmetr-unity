#if UNITY_STANDALONE
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using Appmetr.Unity.Json;
using AppmetrCS;
using AppmetrCS.Actions;
using UnityEngine;
using AppMetrWin = AppmetrCS.AppMetr;

namespace Appmetr.Unity.Impl
{
    public class AppmetrPluginStandalone
    {

        public static string MobUuid
        {
            get
            {
                var guid = PlayerPrefs.GetString(PlayerPrefsMobUuidKey);
                if (string.IsNullOrEmpty(guid))
                {
                    guid = Guid.NewGuid().ToString();
                    PlayerPrefs.SetString(PlayerPrefsMobUuidKey, guid);
                }
                return guid;
            }
        }
        public static void SetupWithToken(string token, string platform)
        {
            if (string.IsNullOrEmpty(platform))
            {
                platform = SubPlatformDefault;
            }

            LogUtils.CustomLog = new AppmetrPluginLogger();
            var serverUserId = PlayerPrefs.GetString(PlayerPrefsServerUserId, null);
            var persister = new AppmetrCS.Persister.FileBatchPersister(Path.Combine(Application.persistentDataPath, AppmetrCacheFolder), NewtonsoftSerializerTyped.Instance);
            _appMetr = new AppMetrWin(_serverAddress, token, MobUuid, platform, serverUserId, DeviceType, persister, new HttpRequestService(NewtonsoftSerializerTyped.Instance));
            _appMetr.Start();
            AttachProperties();
            SessionInit();
        }

        public static void OnPause()
        {
            SessionOnPause();
            if (_appMetr != null)
                _appMetr.Stop();
        }

        public static void OnResume()
        {
            SessionOnResume();
            if (_appMetr != null)
                _appMetr.Start();
        }

        public static void TrackSession()
        {
            TrackSession(null);
        }

        public static void TrackSession(IDictionary<string, object> properties)
        {
            properties = FillProperties(properties);

            int duration = (int)(_sessionDuration / 1000L);

            if (!_isFirstTrackSessionSent)
                duration = -1;
            else if (duration <= 0)
                return;

            properties.Add("$duration", duration);

            _sessionDuration = 0;
            SessionSaveProps();

            _appMetr.Track(new TrackSession { Properties = properties });

            if (!_isFirstTrackSessionSent)
            {
                Flush();
                _isFirstTrackSessionSent = true;
            }
        }

        public static void TrackEvent(string eventName)
        {
            _appMetr.Track(new TrackEvent(eventName));
        }

        public static void TrackEvent(string eventName, IDictionary<string, object> properties)
        {
            if (properties == null)
            {
                properties = new Dictionary<string, object>();
            }
            _appMetr.Track(new TrackEvent(eventName) { Properties = properties });
        }

        public static void TrackPayment(IDictionary<string, object> payment)
        {
            TrackPayment(payment, new Dictionary<string, object>());
        }

        public static void TrackPayment(IDictionary<string, object> payment, IDictionary<string, object> properties)
        {
            if (properties == null)
            {
                properties = new Dictionary<string, object>();
            }
            var orderId = ValueToString(payment, "orderId");
            var transactionId = ValueToString(payment, "transactionId");
            var processor = ValueToString(payment, "processor");
            var psUserSpentCurrencyCode = ValueToString(payment, "psUserSpentCurrencyCode");
            var psUserSpentCurrencyAmount = ValueToString(payment, "psUserSpentCurrencyAmount");
            var psReceivedCurrencyCode = ValueToString(payment, "psReceivedCurrencyCode");
            var psReceivedCurrencyAmount = ValueToString(payment, "psReceivedCurrencyAmount");
            var appCurrencyCode = ValueToString(payment, "appCurrencyCode");
            var appCurrencyAmount = ValueToString(payment, "appCurrencyAmount");
            
            _appMetr.Track(new TrackPayment(orderId, transactionId, processor, psUserSpentCurrencyCode, psUserSpentCurrencyAmount, psReceivedCurrencyCode, psReceivedCurrencyAmount, appCurrencyCode, appCurrencyAmount)
            {
                Properties = properties
            });
        }

        public static void AttachProperties()
        {
            AttachProperties(null);
        }

        public static void AttachProperties(IDictionary<string, object> properties)
        {
            _appMetr.Track(new AttachProperties {Properties = FillProperties(properties)});
        }

        public static void TrackState(IDictionary<string, object> state)
        {
            var stateDict = new Dictionary<string, object>(state);
            _appMetr.Track(new TrackState(stateDict));
        }

        public static void Identify(string userId)
        {
            _appMetr.Track(new TrackIdentify(userId));
            _appMetr.UserId = userId;
            if (string.IsNullOrEmpty(userId))
            {
                PlayerPrefs.DeleteKey(PlayerPrefsServerUserId);
            }
            else
            {
                PlayerPrefs.SetString(PlayerPrefsServerUserId, userId);
            }
        }

        public static void Flush()
        {
            new Thread(() =>
            {
                _appMetr.Flush();
                _appMetr.Upload();
            }).Start();
        }

        public static void FlushLocal()
        {
            new Thread(() => { _appMetr.Flush(); }).Start();
        }

        public static string GetInstanceIdentifier()
        {
            return MobUuid;
        }
        
        public static string GetDeviceKey() 
        {
            return _appMetr.GetDeviceKey();
        }

        public static void AttachEntityAttributes(string entityName, string entityValue,
            IDictionary<string, object> properties)
        {
            _appMetr.Track(new AttachEntityAttributes(entityName, entityValue) { Properties = properties });
        }
    
        /// <summary>
        /// Helps to convert Unity's Application.systemLanguage to a 
        /// 2 letter ISO country code. There is unfortunately not more
        /// countries available as Unity's enum does not enclose all
        /// countries.
        /// </summary>
        /// <returns>The 2-letter ISO code from system language.</returns>
        private static string Get2LetterIsoCodeFromSystemLanguage() {
            var lang = Application.systemLanguage;
            var res = "EN";
            switch (lang) {
                case SystemLanguage.Afrikaans: res = "AF"; break;
                case SystemLanguage.Arabic: res = "AR"; break;
                case SystemLanguage.Basque: res = "EU"; break;
                case SystemLanguage.Belarusian: res = "BY"; break;
                case SystemLanguage.Bulgarian: res = "BG"; break;
                case SystemLanguage.Catalan: res = "CA"; break;
                case SystemLanguage.Chinese: res = "ZH"; break;
                case SystemLanguage.Czech: res = "CS"; break;
                case SystemLanguage.Danish: res = "DA"; break;
                case SystemLanguage.Dutch: res = "NL"; break;
                case SystemLanguage.English: res = "EN"; break;
                case SystemLanguage.Estonian: res = "ET"; break;
                case SystemLanguage.Faroese: res = "FO"; break;
                case SystemLanguage.Finnish: res = "FI"; break;
                case SystemLanguage.French: res = "FR"; break;
                case SystemLanguage.German: res = "DE"; break;
                case SystemLanguage.Greek: res = "EL"; break;
                case SystemLanguage.Hebrew: res = "IW"; break;
                case SystemLanguage.Hungarian: res = "HU"; break;
                case SystemLanguage.Icelandic: res = "IS"; break;
                case SystemLanguage.Indonesian: res = "IN"; break;
                case SystemLanguage.Italian: res = "IT"; break;
                case SystemLanguage.Japanese: res = "JA"; break;
                case SystemLanguage.Korean: res = "KO"; break;
                case SystemLanguage.Latvian: res = "LV"; break;
                case SystemLanguage.Lithuanian: res = "LT"; break;
                case SystemLanguage.Norwegian: res = "NO"; break;
                case SystemLanguage.Polish: res = "PL"; break;
                case SystemLanguage.Portuguese: res = "PT"; break;
                case SystemLanguage.Romanian: res = "RO"; break;
                case SystemLanguage.Russian: res = "RU"; break;
                case SystemLanguage.SerboCroatian: res = "SH"; break;
                case SystemLanguage.Slovak: res = "SK"; break;
                case SystemLanguage.Slovenian: res = "SL"; break;
                case SystemLanguage.Spanish: res = "ES"; break;
                case SystemLanguage.Swedish: res = "SV"; break;
                case SystemLanguage.Thai: res = "TH"; break;
                case SystemLanguage.Turkish: res = "TR"; break;
                case SystemLanguage.Ukrainian: res = "UK"; break;
                case SystemLanguage.Unknown: res = "EN"; break;
                case SystemLanguage.Vietnamese: res = "VI"; break;
                case SystemLanguage.ChineseSimplified: res = "CN"; break;
                case SystemLanguage.ChineseTraditional: res = "CN"; break;
            }
            return res.ToLower();
        }


        //---------------------------------
        // Session duration calculation
        private static void SessionInit()
        {
            _sessionStartTick = Environment.TickCount;

            _isFirstTrackSessionSent = PlayerPrefs.HasKey(PlayerPrefsSessionDuration);
            _sessionDuration = PlayerPrefs.GetInt(PlayerPrefsSessionDuration, 0);
            _sessionDurationCurrent = PlayerPrefs.GetInt(PlayerPrefsSessionCurrent, 0);

            SessionStart();
        }

        private static void SessionSaveProps()
        {
            PlayerPrefs.SetInt(PlayerPrefsSessionDuration, (int)_sessionDuration);
            PlayerPrefs.SetInt(PlayerPrefsSessionCurrent, (int)_sessionDurationCurrent);

            PlayerPrefs.Save();
        }

        private static void SessionStart()
        {
            // ntrf: should not be possible if game makes TrackSession calls on start
            if (_sessionDuration > 0)
            {
                TrackSession();
            }

            // remember current session as previous
            _sessionDuration = _sessionDurationCurrent;
            _sessionDurationCurrent = 0;
            SessionSaveProps();
        }

        private static void SessionOnPause()
        {
            // Accumulate session
            var tk = Environment.TickCount;
            _sessionDurationCurrent = _sessionDurationCurrent + (tk - _sessionStartTick);
            SessionSaveProps();

            // Remember when we started session gap
            _sessionStartTick = tk;
        }

        private static void SessionOnResume()
        {
            var tk = Environment.TickCount;
            if ((tk - _sessionStartTick) >= SessionSplitTimout)
            {
                SessionStart();
            }

            _sessionStartTick = tk;
        }

        private static IDictionary<string, object> FillProperties(IDictionary<string, object> properties)
        {
            if (properties == null)
            {
                properties = new Dictionary<string, object>();
            }
            if (!properties.ContainsKey(AttachPropertiesLanguage))
            {
                properties[AttachPropertiesLanguage] = Get2LetterIsoCodeFromSystemLanguage();
            }
            if (!properties.ContainsKey(AttachPropertiesVersion))
            {
                properties[AttachPropertiesVersion] = Application.version;
            }

            return properties;
        }

        private static string ValueToString(IDictionary<string, object> dict, string valueName)
        {
            if (!dict.ContainsKey(valueName)) return null;
            var value = dict[valueName];
            if (value == null) return null;
            if (value is float) return ((float) value).ToString(CultureInfo.InvariantCulture);
            if (value is int) return ((int) value).ToString(CultureInfo.InvariantCulture);
            if (value is bool) return (bool) value ? "true" : "false";
            return value.ToString();
        }
        
        /// <summary>
        /// Overrides server address
        /// </summary>
        /// <param name="addr">new address of "/api" node</param>
        /// <remarks>This should be called BEFORE <see cref="Setup"/></remarks>
        public static void SetServerUri(string addr)
        {
            _serverAddress = addr;
        }

        private static AppMetrWin _appMetr;
        private static string _serverAddress = "http://appmetr.com/api";
        private const string SubPlatformDefault = "Facebook";
        private const string AppmetrCacheFolder = "Appmetr";
        private const string AttachPropertiesLanguage = "$language";
        private const string AttachPropertiesVersion = "$version";
        private const string PlayerPrefsMobUuidKey = "AppmetrUuid";
        private const string PlayerPrefsSessionDuration = "AppmetrSessionDuration";
        private const string PlayerPrefsSessionCurrent = "AppmetrSessionCurrent";
        private const string PlayerPrefsServerUserId = "AppmetrServerUserId";

        private static int _sessionStartTick;
        private static long _sessionDuration;
        private static long _sessionDurationCurrent;
        private static bool _isFirstTrackSessionSent;

        private const long SessionSplitTimout = 600000L;
        
#if UNITY_STANDALONE_OSX
        private const string DeviceType = "osx";
#elif UNITY_STANDALONE_WIN
        private const string DeviceType = "win";
#elif UNITY_STANDALONE_LINUX
        private const string DeviceType = "lin";
#elif UNITY_PS4
        private const string DeviceType = "ps4";
#elif UNITY_XBOXONE
        private const string DeviceType = "xb1";
#elif UNITY_WSA || UNITY_WSA_10_0
        private const string DeviceType = "uwp";
#else
        #error "This platform is undefined"
#endif
    
    }
}

#endif
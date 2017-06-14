#if UNITY_STANDALONE
using System;
using System.Collections.Generic;
using System.IO;
using Appmetr.Json;
using AppmetrCS;
using AppmetrCS.Actions;
using UnityEngine;
using AppMetrWin = AppmetrCS.AppMetr;

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
    public static void SetupWithToken(string token, string commandListenerName)
    {
        LogUtils.CustomLog = new AppmetrPluginLogger();
        var presister = new AppmetrCS.Persister.FileBatchPersister(Path.Combine(Application.persistentDataPath, AppmetrCacheFolder), NewtonsoftSerializerTyped.Instance);
        _appMetr = new AppMetrWin(ServerDefaultAddress, token, MobUuid, SubPlatformDefault, presister, new HttpRequestService(NewtonsoftSerializerTyped.Instance));
        _appMetr.Start();
        AttachProperties();
    }

    public static void OnPause()
    {
        if(_appMetr != null)
            _appMetr.Stop();
    }

    public static void OnResume()
    {
        if(_appMetr != null)
            _appMetr.Start();
    }

    public static void TrackSession()
    {
        _appMetr.Track(new TrackSession());
    }

    public static void TrackSession(IDictionary<string, object> properties)
    {
        if (properties == null)
        {
            properties = new Dictionary<string, object>();
        }
        _appMetr.Track(new TrackSession { Properties = properties });
    }

    public static void TrackLevel(int level)
    {
        _appMetr.Track(new TrackLevel(level));
    }

    public static void TrackLevel(int level, IDictionary<string, object> properties)
    {
        if (properties == null)
        {
            properties = new Dictionary<string, object>();
        }
        _appMetr.Track(new TrackLevel(level) { Properties = properties });
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
        var orderId = payment.ContainsKey("orderId") ? payment["orderId"].ToString() : null;
        var transactionId = payment.ContainsKey("transactionId") ? payment["transactionId"].ToString() : null;
        var processor = payment.ContainsKey("processor") ? payment["processor"].ToString() : null;
        var psUserSpentCurrencyCode = payment.ContainsKey("psUserSpentCurrencyCode") ? payment["psUserSpentCurrencyCode"].ToString() : null;
        var psUserSpentCurrencyAmount = payment.ContainsKey("psUserSpentCurrencyAmount") ? payment["psUserSpentCurrencyAmount"].ToString() : null;
        var psReceivedCurrencyCode = payment.ContainsKey("psReceivedCurrencyCode") ? payment["psReceivedCurrencyCode"].ToString() : null;
        var psReceivedCurrencyAmount = payment.ContainsKey("psReceivedCurrencyAmount") ? payment["psReceivedCurrencyAmount"].ToString() : null;
        var appCurrencyCode = payment.ContainsKey("appCurrencyCode") ? payment["appCurrencyCode"].ToString() : null;
        var appCurrencyAmount = payment.ContainsKey("appCurrencyAmount") ? payment["appCurrencyAmount"].ToString() : null;
            
        _appMetr.Track(new TrackPayment(orderId, transactionId, processor, psUserSpentCurrencyCode, psUserSpentCurrencyAmount, psReceivedCurrencyCode, psReceivedCurrencyAmount, appCurrencyCode, appCurrencyAmount)
        {
            Properties = properties
        });
    }

    public static void TrackAdsEvent(string eventName) {}

    public static void AttachProperties()
    {
        AttachProperties(null);
    }

    public static void AttachProperties(IDictionary<string, object> properties)
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
        _appMetr.Track(new AttachProperties() { Properties = properties });
    }

    public static void TrackOptions(string commandId, IDictionary<string, object>[] options) {}

    public static void TrackOptionsError(string commandId, IDictionary<string, object>[] options, string code, string message) {}

    public static void TrackExperimentStart(string experiment, string groupId) {}

    public static void TrackExperimentEnd(string experiment) {}

    public static bool VerifyIOSPayment(string productId, string transactionId, string receipt, string privateKey) { return false; }

    public static bool VerifyAndroidPayment(string purchaseInfo, string signature, string privateKey) { return false; }

    public static void TrackState(IDictionary<string, object> state)
    {
        var stateDict = new Dictionary<string, object>(state);
        _appMetr.Track(new TrackState(stateDict));
    }

    public static void Identify(string userId)
    {
        _appMetr.Track(new TrackIdentify(userId));
    }

    public static void Flush()
    {
        _appMetr.Flush();
    }

    public static string GetInstanceIdentifier()
    {
        return MobUuid;
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
    
    private static AppMetrWin _appMetr;
    private const string ServerDefaultAddress = "https://appmetr.com/api";
    private const string SubPlatformDefault = "Facebook";
    private const string AppmetrCacheFolder = "Appmetr";
    private const string AttachPropertiesLanguage = "$language";
    private const string AttachPropertiesVersion = "$version";
    private const string PlayerPrefsMobUuidKey = "AppmetrUuid";
}
#endif
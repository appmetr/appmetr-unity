#if UNITY_STANDALONE
using System;
using AppmetrCS;
using Debugger = UnityEngine.Debug;

namespace Appmetr.Unity.Impl
{
    public class AppmetrPluginLogger : ILog
    {
        public AppmetrPluginLogger()
        {
            IsDebugEnabled = false;
            IsInfoEnabled = false;
            IsWarnEnabled = true;
            IsErrorEnabled = true;
            IsFatalEnabled = true;
        }

        public void Debug(object message)
        {
            if (!IsDebugEnabled) return;
            Debugger.Log(Tag + message);
        }

        public void Debug(object message, Exception exception)
        {
            if (!IsDebugEnabled) return;
            Debugger.Log(Tag + message);
            Debugger.LogException(exception);
        }

        public void DebugFormat(string format, params object[] args)
        {
            if (!IsDebugEnabled) return;
            Debugger.LogFormat(Tag + format, args);
        }

        public void DebugFormat(string format, object arg0)
        {
            if (!IsDebugEnabled) return;
            Debugger.LogFormat(Tag + format, arg0);
        }

        public void DebugFormat(string format, object arg0, object arg1)
        {
            if (!IsDebugEnabled) return;
            Debugger.LogFormat(Tag + format, arg0, arg1);
        }

        public void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            if (!IsDebugEnabled) return;
            Debugger.LogFormat(Tag + format, arg0, arg1, arg2);
        }

        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (!IsDebugEnabled) return;
            DebugFormat(format, args);
        }

        public void Info(object message)
        {
            if (!IsInfoEnabled) return;
            Debugger.Log(Tag + message);
        }

        public void Info(object message, Exception exception)
        {
            if (!IsInfoEnabled) return;
            Debug(message, exception);
        }

        public void InfoFormat(string format, params object[] args)
        {
            if (!IsInfoEnabled) return;
            DebugFormat(format, args);
        }

        public void InfoFormat(string format, object arg0)
        {
            if (!IsInfoEnabled) return;
            DebugFormat(format, arg0);
        }

        public void InfoFormat(string format, object arg0, object arg1)
        {
            if (!IsInfoEnabled) return;
            DebugFormat(format, arg0, arg1);
        }

        public void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            if (!IsInfoEnabled) return;
            DebugFormat(format, arg0, arg1, arg2);
        }

        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (!IsInfoEnabled) return;
            DebugFormat(provider, format, args);
        }

        public void Warn(object message)
        {
            if (!IsWarnEnabled) return;
            Debugger.LogWarning(Tag + message);
        }

        public void Warn(object message, Exception exception)
        {
            if (!IsWarnEnabled) return;
            Debugger.LogWarning(Tag + message);
            Debugger.LogException(exception);
        }

        public void WarnFormat(string format, params object[] args)
        {
            if (!IsWarnEnabled) return;
            Debugger.LogWarningFormat(format, args);
        }

        public void WarnFormat(string format, object arg0)
        {
            if (!IsWarnEnabled) return;
            Debugger.LogWarningFormat(format, arg0);
        }

        public void WarnFormat(string format, object arg0, object arg1)
        {
            if (!IsWarnEnabled) return;
            Debugger.LogWarningFormat(format, arg0, arg1);
        }

        public void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            if (!IsWarnEnabled) return;
            Debugger.LogWarningFormat(format, arg0, arg1, arg2);
        }

        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (!IsWarnEnabled) return;
            Debugger.LogWarningFormat(format, args);
        }

        public void Error(object message)
        {
            if (!IsErrorEnabled) return;
            Debugger.LogError(Tag + message);
        }

        public void Error(object message, Exception exception)
        {
            if (!IsErrorEnabled) return;
            Debugger.LogError(Tag + message);
            Debugger.LogException(exception);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            if (!IsErrorEnabled) return;
            Debugger.LogErrorFormat(format, args);
        }

        public void ErrorFormat(string format, object arg0)
        {
            if (!IsErrorEnabled) return;
            Debugger.LogErrorFormat(format, arg0);
        }

        public void ErrorFormat(string format, object arg0, object arg1)
        {
            if (!IsErrorEnabled) return;
            Debugger.LogErrorFormat(format, arg0, arg1);
        }

        public void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            if (!IsErrorEnabled) return;
            Debugger.LogErrorFormat(format, arg0, arg1, arg2);
        }

        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (!IsErrorEnabled) return;
            Debugger.LogErrorFormat(format, args);
        }

        public void Fatal(object message)
        {
            if (!IsFatalEnabled) return;
            Error(message);
        }

        public void Fatal(object message, Exception exception)
        {
            if (!IsFatalEnabled) return;
            Error(message, exception);
        }

        public void FatalFormat(string format, params object[] args)
        {
            if (!IsFatalEnabled) return;
            ErrorFormat(format, args);
        }

        public void FatalFormat(string format, object arg0)
        {
            if (!IsFatalEnabled) return;
            ErrorFormat(format, arg0);
        }

        public void FatalFormat(string format, object arg0, object arg1)
        {
            if (!IsFatalEnabled) return;
            ErrorFormat(format, arg0, arg1);
        }

        public void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            if (!IsFatalEnabled) return;
            ErrorFormat(format, arg0, arg1, arg2);
        }

        public void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (!IsFatalEnabled) return;
            ErrorFormat(provider, format, args);
        }

        public bool IsDebugEnabled { get; private set; }
        public bool IsInfoEnabled { get; private set; }
        public bool IsWarnEnabled { get; private set; }
        public bool IsErrorEnabled { get; private set; }
        public bool IsFatalEnabled { get; private set; }

        private const string Tag = "[Appmetr] ";
    }
}

#endif
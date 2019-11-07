using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace STSys.Core.MQ
{
    public static class ConfigManagerConf
    {
        public static IConfiguration Configuration = null;

        static ConcurrentDictionary<string, List<string>> _dicCache = new ConcurrentDictionary<string, List<string>>();

        /// <summary>
        /// 值类型
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValue(string key)
        {
            List<string> refValue;
            _dicCache.TryGetValue(key, out refValue);
            if (refValue != null)
                return refValue?[0];

            if (Configuration == null)
                return "";
            string value = Configuration[key];
            if (!string.IsNullOrEmpty(value)) //本地存在则返回
                return value;
            return "";
        }

        /// <summary>
        /// 引用类型(开发建议使用) List<string> list= ConfigManagerConf.GetReferenceValue("") 使用List 即可
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<string> GetReferenceValue(string key)
        {
            List<string> refValue;
            _dicCache.TryGetValue(key, out refValue);
            if (refValue == null)
            {
                refValue = new List<string>();
                refValue.Add(GetValue(key));//[0]
                _dicCache.TryAdd(key, refValue);
            }
            return refValue;
        }

        public static void SetConfiguration(IConfiguration configuration)
        {
            if (Configuration == null)
                Configuration = configuration;

        }
    }
}

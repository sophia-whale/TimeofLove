using System;
using Xamarin.Essentials;

namespace TabbedTemplate.Services
{
    public class PreferenceStorage : IPreferenceStorage
    {/// <param name="key">Preference key.</param>
     /// <param name="value">Preference value.</param>
     /// <summary>Sets a value for a given key.</summary>
     /// <remarks>
     ///     <para />
     /// </remarks>
        public void Set(string key, int value) => Preferences.Set(key, value);

        public void Set(string key, string value) => Preferences.Set(key, value);

        /// <param name="key">Preference key.</param>
        /// <param name="defaultValue">Default value to return if the key does not exist.</param>
        /// <summary>Gets the value for a given key, or the default specified if the key does not exist.</summary>
        /// <returns>Value for the given key, or the default if it does not exist.</returns>
        /// <remarks />
        public int Get(string key, int defaultValue) => Preferences.Get(key, defaultValue);

        public string Get(string key, string defaultValue) => Preferences.Get(key, defaultValue);

        /// <summary>
        /// 获取日期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultTime"></param>
        /// <returns></returns>
        public DateTime Get(string key, DateTime defaultTime) =>
            Preferences.Get(key, defaultTime);

        /// <summary>
        /// 设置日期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(string key, DateTime value) =>
                Preferences.Set(key, value);

    }
}

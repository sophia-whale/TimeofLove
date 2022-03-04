using System;

namespace TabbedTemplate.Services
{
    public interface IPreferenceStorage
    {
        /// <param name="key">Preference key.</param>
        /// <param name="value">Preference value.</param>
        /// <summary>Sets a value for a given key.</summary>
        /// <remarks>
        ///     <para />
        /// </remarks>
        void Set(string key, int value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Set(string key, string value);

        /// <param name="key">Preference key.</param>
        /// <param name="defaultValue">Default value to return if the key does not exist.</param>
        /// <summary>Gets the value for a given key, or the default specified if the key does not exist.</summary>
        /// <returns>Value for the given key, or the default if it does not exist.</returns>
        /// <remarks />
        int Get(string key, int defaultValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        string Get(string key, string defaultValue);

        /// <summary>
        /// 获取日期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultTime"></param>
        /// <returns></returns>
        DateTime Get(string key, DateTime defaultTime);

        /// <summary>
        /// 设置日期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Set(string key, DateTime value);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TabbedTemplate.Services
{
    /// <summary>
    /// 导航参数
    /// </summary>
    public static class NavigationContext
    { /// <summary>
      /// 导航参数属性。
      /// </summary>
        public static readonly BindableProperty NavigationParameterProperty = BindableProperty.CreateAttached("NavigationParameter", typeof(object), typeof(NavigationContext), null, BindingMode.OneWayToSource);

        /// <summary>
        /// 设置导航参数。
        /// </summary>
        /// <param name="bindable">可绑定对象。</param>
        /// <param name="parameter">参数。</param>
        public static void SetParameter(BindableObject bindable,
            object parameter) =>
            bindable.SetValue(NavigationParameterProperty, parameter);
    }
}

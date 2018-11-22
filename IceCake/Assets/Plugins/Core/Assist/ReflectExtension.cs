using System;
using System.Reflection;

namespace IceCake.Core
{
    public static class ReflectExtension
    {
        public static object Construct(Type type, params object[] _params)
        {
            var paramType = new Type[_params.Length];
            for (int index = 0; index < _params.Length; index++)
            {
                paramType[index] = _params[index].GetType();
            }

            var constructor = type.GetConstructor(paramType);
            return constructor.Invoke(_params);
        }

        public static T Construct<T>(params object[] _params)
        {
            return (T)Construct(typeof(T), _params);
        }

        public static T TConstruct<T>(Type type, params object[] _params)
        {
            return (T)Construct(type, _params);
        }
    }

    public static class ICustomAttributeProviderExtension
    {
        public static T GetCustomAttribute<T>(this ICustomAttributeProvider provider, bool inherit) where T : Attribute
        {
            var attributes = provider.GetCustomAttributes(typeof(T), inherit);
            if (attributes.Length == 0)
                return default(T);
            return (T)attributes[0];
        }

        public static T[] GetCustomAttributes<T>(this ICustomAttributeProvider provider, bool inherit) where T : Attribute
        {
            var attributes = provider.GetCustomAttributes(typeof(T), inherit);
            var resultAttrs = new T[attributes.Length];
            for (int index = 0; index < attributes.Length; index++)
            {
                resultAttrs[index] = attributes[index] as T;
            }
            return resultAttrs;
        }

        /// <summary>
        /// IsApplyAttr<T>/IsApplyAttr
        ///     IsDefined函数功能是判定一个Attribute的使用被定义在该类中或者父类中。但如果使用的Attribute标记的是Inherit=false
        ///     该函数返回true，但无法通过GetCustomAttributes获取。IsApplyAttr为了和GetCustomAttributes的结果对应，
        ///     IsApplyAttr返回true，GetCustomAttributes一定能获得。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="provider"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public static bool IsApplyAttr<T>(this ICustomAttributeProvider provider, bool inherit)
        {
            return provider.GetCustomAttributes(typeof(T), inherit).Length > 0;
        }

        public static bool IsApplyAttr(this ICustomAttributeProvider provider, Type type, bool inherit)
        {
            return provider.GetCustomAttributes(type, inherit).Length > 0;
        }
    }
}

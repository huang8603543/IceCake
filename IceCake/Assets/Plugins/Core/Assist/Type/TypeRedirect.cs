using System;

namespace IceCake.Core
{
    /// <summary>
    /// 类型重定向///
    /// </summary>
    public class ITypeRedirect
    {
        public static Func<Type, Type> GetRedirectTypeHandler;

        public static Type GetRedirectType(Type srcType)
        {
            if (GetRedirectTypeHandler == null)
                return srcType;
            return GetRedirectTypeHandler(srcType);
        }
    }
}

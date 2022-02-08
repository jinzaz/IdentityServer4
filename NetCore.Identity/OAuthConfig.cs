using System;

namespace NetCore.Identity
{
    public class OAuthConfig
    {
        /// <summary>
        /// 过期秒数
        /// </summary>
        public const int ExpireIn = 600;

        /// <summary>
        /// 用户Api相关
        /// </summary>
        public static class UserApi
        {
            public static string ApiName = "user_api";
            public static string ApiName2 = "user_api2";
            public static string ApiName3 = "user_api3";

            public static string ClientId = "user_clientid";

            public static string Secret = "user_secret";

            public static string ApiScopeName1 = "user_get";
            public static string ApiScopeName2 = "user_get/Id";
            public static string ApiScopeName3 = "user_post";
        }
    }
}

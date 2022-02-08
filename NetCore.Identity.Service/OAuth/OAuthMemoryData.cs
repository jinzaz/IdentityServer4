using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Test;
using NetCore.Identity.Model;
using NetCore.Identity.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace NetCore.Identity.Service.OAuth
{
    public class OAuthMemoryData
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource(
                    OAuthConfig.UserApi.ApiName,
                    OAuthConfig.UserApi.ApiName,
                    new List<string>(){JwtClaimTypes.Role }
                    )
                {
                     Scopes={OAuthConfig.UserApi.ApiScopeName1},
                },
                new ApiResource(
                    OAuthConfig.UserApi.ApiName2,
                    OAuthConfig.UserApi.ApiName2,
                    new List<string>(){JwtClaimTypes.Role }
                    )
                {
                     Scopes={OAuthConfig.UserApi.ApiScopeName2 },
                },
                new ApiResource(
                    OAuthConfig.UserApi.ApiName3,
                    OAuthConfig.UserApi.ApiName3,
                    new List<string>(){JwtClaimTypes.Role }
                    )
                {
                     Scopes={OAuthConfig.UserApi.ApiScopeName3 },
                },
            };

        }

        public static IEnumerable<ApiResource> GetApiResources2()
        {
            return new List<ApiResource>
            {
                new ApiResource(OAuthConfig.UserApi.ApiName,"api1 servces"),
                new ApiResource(OAuthConfig.UserApi.ApiName2,"api2 servces"),
                new ApiResource(OAuthConfig.UserApi.ApiName3,"api3 servces")
            };

        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client()
                {
                   ClientId = OAuthConfig.UserApi.ClientId,
                   AllowedGrantTypes = new List<string>()//配置授权类型，可以配置多个授权类型
                   {
                      GrantTypes.ResourceOwnerPassword.FirstOrDefault(),//Resource Onwer Password模式
                      GrantTypeConstants.ResourceWeixinOpen,//新增的自定义微信客户端的授权模式
                   },
                   ClientSecrets = {new Secret(OAuthConfig.UserApi.Secret.Sha256()) },//客户端加密方式 
                   AllowOfflineAccess = true,//如果要获取refresh_tokens ,必须把AllowOfflineAccess设置为true
                   AllowedScopes = {
                        OAuthConfig.UserApi.ApiName2,
                        OAuthConfig.UserApi.ApiName3,
                        StandardScopes.OfflineAccess,
                    },//配置授权范围，这里指定哪些API 受此方式保护
                   AccessTokenLifetime = OAuthConfig.ExpireIn//配置Token 失效时间
                }

            };
        }

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope(OAuthConfig.UserApi.ApiScopeName1),
                new ApiScope(OAuthConfig.UserApi.ApiScopeName2),
            };




        /// <summary>
        /// 测试的账号和密码
        /// </summary>
        /// <returns></returns>
        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>
            {
                new TestUser()
                {
                    SubjectId = "1",
                    Username = "test",
                    Password = "123456",
                }
            };
        }

        /// <summary>
        /// 微信端测试的账号和密码
        /// </summary>
        /// <returns></returns>
        public static List<TestUser> GetWeiXinOpenIdTestUsers()
        {
            return new List<TestUser>
            {
                new TestUser()
                {
                    SubjectId = "1",
                    Username = "test",
                    Password = "123456",
                }
            };
        }

        /// <summary>
        /// 为了演示，硬编码了，
        /// 这个方法可以通过DDD设计到底层数据库去查询数据库
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static UserModel GetUserByUserName(string userName)
        {
            var normalUser = new UserModel()
            {
                DisplayName = "张三",
                MerchantId = 10001,
                Password = "123456",
                Role = Enums.EnumUserRole.Normal,
                SubjectId = "1",
                UserId = 20001,
                UserName = "testNormal"
            };
            var manageUser = new UserModel()
            {
                DisplayName = "李四",
                MerchantId = 10001,
                Password = "123456",
                Role = Enums.EnumUserRole.Manage,
                SubjectId = "1",
                UserId = 20001,
                UserName = "testManage"
            };
            var supperManageUser = new UserModel()
            {
                DisplayName = "dotNET博士",
                MerchantId = 10001,
                Password = "123456",
                Role = Enums.EnumUserRole.SupperManage,
                SubjectId = "1",
                UserId = 20001,
                UserName = "testSupperManage"
            };
            var list = new List<UserModel>() {
                 normalUser,
                 manageUser,
                 supperManageUser
             };
            return list?.Where(item => item.UserName.Equals(userName))?.FirstOrDefault();
        }
    }
}

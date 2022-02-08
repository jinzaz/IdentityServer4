using IdentityModel;
using IdentityServer4.Validation;
using NetCore.Identity.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NetCore.Identity.Service.OAuth
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            try
            {
                var userName = context.UserName;
                var passowrd = context.Password;

                //验证用户，这么可以到数据库里面验证用户名和密码是否正确
                var claimList = await ValidateUserByRoleAsync(userName, passowrd);

                //验证账号
                context.Result = new GrantValidationResult
                (
                    subject: userName,
                    authenticationMethod: "custom",
                    claims: claimList.ToArray()
                 );
            }
            catch (Exception ex)
            {
                //验证异常结果
                context.Result = new GrantValidationResult()
                {
                    IsError = true,
                    Error = ex.Message
                };
            }
        }

        private async Task<List<Claim>> ValidateUserByRoleAsync(string loginName, string password)
        {
            //TODO 这里可以通过用户名和密码到数据库中去验证是否存在，
            // 以及角色相关信息，我这里还是使用内存中已经存在的用户和密码
            var user = OAuthMemoryData.GetUserByUserName(loginName);
            
            if (user == null)
            {
                throw new Exception("登录失败，用户名和密码不正确");
            }
            return new List<Claim>()
            {
                new Claim(ClaimTypes.Name,$"{user.UserName}"),
                new Claim(EnumUserClaim.DisplayName.ToString(),user.DisplayName),
                new Claim(EnumUserClaim.UserId.ToString(),user.UserId.ToString()),
                new Claim(EnumUserClaim.MerchantId.ToString(),user.MerchantId.ToString()),
                new Claim(JwtClaimTypes.Role.ToString(),user.Role.ToString())
            };
        }
    }
}

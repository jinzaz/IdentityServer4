using IdentityServer4.Validation;
using NetCore.Identity.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NetCore.Identity.Service.OAuth
{
    public class WeiXinOpenGrantValidator : IExtensionGrantValidator
    {
        public string GrantType => GrantTypeConstants.ResourceWeixinOpen;

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            try
            {
                #region 获取参数
                var openId = context.Request.Raw[ParamConstants.OpenId];
                var unionId = context.Request.Raw[ParamConstants.UnionId];
                var userName = context.Request.Raw[ParamConstants.UserName];
                #endregion

                #region 通过openId和UnionId 参数来进行数据库的相关验证
                var claimList = await ValidateUserAsync(openId,unionId);
                #endregion

                #region 授权通过
                //授权通过返回
                context.Result = new GrantValidationResult
                (
                    subject:openId,
                    authenticationMethod:"custom",
                    claims:claimList.ToArray()
                );
                #endregion
            }
            catch (Exception ex)
            {
                context.Result = new GrantValidationResult()
                { 
                   IsError = true,
                   Error = ex.Message
                };
            }
        
        }
        #region Private Method
        /// <summary>
        /// 验证用户
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="unionId"></param>
        /// <returns></returns>
        private async Task<List<Claim>> ValidateUserAsync(string openId,string unionId)
        {
            //TODO 这里可以通过openId 和unionId 来查询用户信息(数据库查询)，
            //我这里为了方便测试还是直接写测试的openId 相关信息用户
            var user = OAuthMemoryData.GetWeiXinOpenIdTestUsers();

            if (user == null)
            { 
                //注册用户
            }
            return new List<Claim>()
            { 
                new Claim(ClaimTypes.Name,$"{openId}"),
            };
        }
        #endregion
    }
}
